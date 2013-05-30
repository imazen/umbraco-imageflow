using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.BusinessLogic;
using umbraco.IO;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls;
using umbraco.editorControls.MultiNodeTreePicker;
using umbraco.interfaces;
using umbraco.uicontrols;

namespace Our.Umbraco.IRImagePicker.DataType
{
    /// <summary>
    /// The Data Editor for the IRImagePicker data type
    /// </summary>
    public class IRImagePickerDataEditor : Panel, IDataEditor
    {
        /// <summary>
        /// The IData for the data-type.
        /// </summary>
        private readonly IData _data;

        /// <summary>
        /// The prevalue for the data-type.
        /// </summary>
        private readonly IRImagePickerPreValue _preValue;

        /// <summary>
        /// The data type definition id for the data-type;
        /// </summary>
        private readonly int _dtdId;

        /// <summary>
        /// Gets a value indicating whether to show a label or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if want to show label; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLabel
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether treat as rich text editor.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if treat as rich text editor; otherwise, <c>false</c>.
        /// </value>
        public bool TreatAsRichTextEditor
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        public Control Editor
        {
            get { return this; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IRImagePickerDataEditor"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="preValue">The pre value.</param>
        /// <param name="dtdId">The DTD id.</param>
        internal IRImagePickerDataEditor(IData data, IRImagePickerPreValue preValue, int dtdId)
        {
            _data = data;
            _preValue = preValue;
            _dtdId = dtdId;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Save()
        {
            EnsureChildControls();

            int parsed;

            var val = new IRImagePickerValue
            {
                ImageId = Int32.TryParse(hdnImageId.Value, out parsed) ? parsed : 0,
                QueryString = hdnQueryString.Value
            };

            _data.Value = val.SerializeToJson();
        }

        private Panel ctrlWrapper = new Panel();

        private HiddenField hdnImageId = new HiddenField();
        private HiddenField hdnQueryString = new HiddenField();

        private HyperLink lnkChoose = new HyperLink();

        private Panel pnlValueContainer = new Panel();

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.RegisterEmbeddedClientResource(typeof(IRImagePickerDataEditor), "Our.Umbraco.IRImagePicker.Resources.Styles.IRImagePicker.css", ClientDependencyType.Css);
            this.RegisterEmbeddedClientResource(typeof(IRImagePickerDataEditor), "Our.Umbraco.IRImagePicker.Resources.Scripts.IRImagePicker.js", ClientDependencyType.Javascript);

            EnsureChildControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                var val = _data.Value.ToString().DeserializeJsonTo<IRImagePickerValue>();
                if (val != null && val.ImageId > 0)
                {
                    hdnImageId.Value = val.ImageId.ToString();
                    hdnQueryString.Value = val.QueryString;
                }
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(IRImagePickerDataEditor), "IRImagePicker", @"
            var irip_viewtext = '" + Web.UI.App_GlobalResources.IRImagePicker.lbl_view + @"';
            var irip_editcroptext = '" + Web.UI.App_GlobalResources.IRImagePicker.lbl_editCrop + @"';
            var irip_removetext = '" + Web.UI.App_GlobalResources.IRImagePicker.lbl_remove + @"';
            var irip_choosemodaltext = '" + Web.UI.App_GlobalResources.IRImagePicker.lbl_choosemodal + @"';
            var irip_cropmodaltext = '" + Web.UI.App_GlobalResources.IRImagePicker.lbl_cropmodal + @"';
            var irip_umbracopath = '" + IOHelper.ResolveUrl(SystemDirectories.Umbraco) + @"';
            ", true);

            Page.ClientScript.RegisterClientScriptBlock(typeof(IRImagePickerDataEditor), "IRImagePicker_" + ctrlWrapper.ClientID, @"
            $(function(){
                $('#"+ ctrlWrapper.ClientID + @"').irImagePicker();
            });", true);
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Setup wrapper
            ctrlWrapper.ID = ID + "_pnlWrapper";
            ctrlWrapper.CssClass = "IRImagePicker";
            ctrlWrapper.Attributes.Add("data-dtdid", _dtdId.ToString());
            ctrlWrapper.Attributes.Add("data-width", _preValue.Width.ToString());
            ctrlWrapper.Attributes.Add("data-height", _preValue.Height.ToString());
            ctrlWrapper.Attributes.Add("data-thumbwidth", _preValue.ThumbWidth.ToString());
            ctrlWrapper.Attributes.Add("data-thumbheight", Math.Round(((decimal)_preValue.Height / _preValue.Width) * _preValue.ThumbWidth).ToString());
            ctrlWrapper.Attributes.Add("data-autolaunchcropper", _preValue.AutoLaunchCropper.ToString().ToLower());

            // Setup hidden fields
            hdnImageId.ID = ID + "_hdnImageId";
            hdnQueryString.ID = ID + "_hdnQueryString";

            ctrlWrapper.Controls.Add(hdnImageId);
            ctrlWrapper.Controls.Add(hdnQueryString);

            // Setup choose link
            lnkChoose.ID = ID + "_lnkChoose";
            lnkChoose.Text = Web.UI.App_GlobalResources.IRImagePicker.lbl_choose;
            lnkChoose.NavigateUrl = "#";
            lnkChoose.Attributes.Add("data-treeurl", GetTreeUrl());

            ctrlWrapper.Controls.Add(lnkChoose);

            // Setup value container
            pnlValueContainer.ID = ID + "_pnlValueContainer";
            pnlValueContainer.CssClass = "irip-valuecontainer";

            ctrlWrapper.Controls.Add(pnlValueContainer);

            Controls.Add(ctrlWrapper);

            // We need to add the controls before we access ClientID's, so we set the onClick last
            //lnkChoose.Attributes["onClick"] = CreateChooseLink(ctrlWrapper.ClientID);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            ctrlWrapper.RenderControl(writer);
        }

        /// <summary>
        /// Gets the tree URL.
        /// </summary>
        /// <returns></returns>
        protected string GetTreeUrl()
        {
            var treeUrl = TreeUrlGenerator.GetPickerUrl("media", "FilteredMediaTree");

            /* 
             * =============================================
             * WARNING! DIRTY, DIRTY, DIRTY CODE
             * =============================================
             * Force MNTP to save it's cookie
             */
            var xpathFilter = "Image[normalize-space(umbracoFile)]";
            var tree = new MNTP_DataEditor
            {
                TreeToRender = "media",
                XPathFilter = xpathFilter,
                MaxNodeCount = 0,
                ShowToolTips = false,
                XPathFilterMatchType = XPathFilterType.Enable,
                StartNodeId = User.GetCurrent().StartMediaId,
                DataTypeDefinitionId = _dtdId,
                ShowThumbnailsForMedia = false,
                PropertyName = "IRImagePicker",
                StartNodeSelectionType = NodeSelectionType.Picker,
                StartNodeXPathExpression = "",
                StartNodeXPathExpressionType = XPathExpressionType.Global,
                ControlHeight = 200,
                MinNodeCount = 0
            };

            var dynMethod = tree.GetType().GetMethod("SavePersistentValuesForTree", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(tree, new object[] { xpathFilter });

            return treeUrl.Replace("/dialogs/", "/plugins/irimagepicker/") + "&nodeKey=" + _dtdId;
        }
    }
}
