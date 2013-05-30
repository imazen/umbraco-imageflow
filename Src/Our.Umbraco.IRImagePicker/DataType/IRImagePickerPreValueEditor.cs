using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls;
using BaseDataType = umbraco.cms.businesslogic.datatype.BaseDataType;
using DBTypes = umbraco.cms.businesslogic.datatype.DBTypes;

namespace Our.Umbraco.IRImagePicker.DataType
{
    /// <summary>
    /// The PreValue Editor for the IRImagePicker data type
    /// </summary>
    public class IRImagePickerPreValueEditor : AbstractJsonPrevalueEditor
    {
        /// <summary>
        /// The image width
        /// </summary>
        protected TextBox txtWidth;

        /// <summary>
        /// The image height
        /// </summary>
        protected TextBox txtHeight;

        /// <summary>
        /// The thumb image width
        /// </summary>
        protected TextBox txtThumbWidth;

        /// <summary>
        /// The data format to retreive the value as
        /// </summary>
        protected RadioButtonList rdoDataFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="IRImagePickerPreValueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public IRImagePickerPreValueEditor(BaseDataType dataType)
            : base(dataType, DBTypes.Nvarchar)
        { }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Adds the client dependencies.
            this.RegisterEmbeddedClientResource(typeof(AbstractPrevalueEditor), "umbraco.editorControls.PrevalueEditor.css", ClientDependencyType.Css);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // get PreValues, load them into the controls.
            var preValue = GetPreValue<IRImagePickerPreValue>() ?? new IRImagePickerPreValue();

            // set the values
            txtWidth.Text = preValue.Width.ToString();
            txtHeight.Text = preValue.Height.ToString();
            txtThumbWidth.Text = preValue.ThumbWidth.ToString();
            rdoDataFormat.SelectedValue = preValue.DataFormat.ToString();
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // add property fields
            writer.AddPrevalueHeading("<img src='" + Page.ClientScript.GetWebResourceUrl(typeof(IRImagePickerDataEditor), "Our.Umbraco.IRImagePicker.Resources.Images.ir_logo.png") + "' alt='Image Resizer' style='width:200px;margin:0 0 5px 130px;' />");
            writer.AddPrevalueRow("Image Dimensions", "Enter the width and height for the selected image.", txtWidth, new LiteralControl(" x "), txtHeight);
            writer.AddPrevalueRow("Thumb Width", "Set the width of the thumbnail to show in the editor.", txtThumbWidth);
            writer.AddPrevalueRow("Data format", "Select the data format in which to store the value of this data type in.<br />XML if you intend to work with it in XSLT or JSON if you intend to work with it via Razor or C#.", rdoDataFormat);
        }

        /// <summary>
        /// Creates child controls for this control
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // set-up child controls
            txtWidth = new TextBox { ID = "txtWidth" };
            txtHeight = new TextBox { ID = "txtHeight" };
            txtThumbWidth = new TextBox { ID = "txtThumbWidth" };

            rdoDataFormat = new RadioButtonList { ID = "rdoDataFormat" };
            rdoDataFormat.Items.Add(IRImagePickerDataFormat.Xml.ToString());
            rdoDataFormat.Items.Add(IRImagePickerDataFormat.Json.ToString());
            rdoDataFormat.RepeatDirection = RepeatDirection.Horizontal;

            // add the child controls
            Controls.AddPrevalueControls(txtWidth);
            Controls.AddPrevalueControls(txtHeight);
            Controls.AddPrevalueControls(txtThumbWidth);
            Controls.AddPrevalueControls(rdoDataFormat);
        }

        /// <summary>
        /// Saves the data-type PreValue options.
        /// </summary>
        public override void Save()
        {
            int parsed;

            // set the options
            var options = new IRImagePickerPreValue
            {
                Width = Int32.TryParse(txtWidth.Text, out parsed) ? parsed : 0,
                Height = Int32.TryParse(txtHeight.Text, out parsed) ? parsed : 0,
                ThumbWidth = Int32.TryParse(txtThumbWidth.Text, out parsed) ? parsed : 0,
                DataFormat = (IRImagePickerDataFormat)Enum.Parse(typeof(IRImagePickerDataFormat), rdoDataFormat.SelectedValue)
            };

            // save the options as JSON
            SaveAsJson(options);
        }
    }
}
