using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Our.Umbraco.IRImagePicker.DataType
{
    /// <summary>
    /// A IRImagePicker data type
    /// </summary>
    public class IRImagePickerDataType : AbstractDataEditor
    {
        /// <summary>
        /// The Data Editor for the data-type.
        /// </summary>
        private IRImagePickerDataEditor _dataEditor;

        /// <summary>
        /// The PreValue Editor for the data-type.
        /// </summary>
        private IRImagePickerPreValueEditor _preValueEditor;

        /// <summary>
        /// The Data for the data-type.
        /// </summary>
        private IRImagePickerData _data;

        /// <summary>
        /// Gets the id of the data-type.
        /// </summary>
        /// <value>
        /// The id of the data-type.
        /// </value>
        public override Guid Id
        {
            get { return new Guid("A8A9AD5C-4496-45A7-B06B-EF748B34DB29"); }
        }

        /// <summary>
        /// Gets the name of the data type.
        /// </summary>
        /// <value>
        /// The name of the data type.
        /// </value>
        public override string DataTypeName
        {
            get { return "IR Image Picker"; }
        }

        /// <summary>
        /// Gets the prevalue editor.
        /// </summary>
        /// <value>
        /// The prevalue editor.
        /// </value>
        public override IDataEditor DataEditor
        {
            get { return _dataEditor ?? (_dataEditor = new IRImagePickerDataEditor(Data, ((IRImagePickerPreValueEditor)PrevalueEditor).GetPreValue<IRImagePickerPreValue>(), DataTypeDefinitionId)); }
        }

        /// <summary>
        /// Gets the prevalue editor.
        /// </summary>
        /// <value>
        /// The prevalue editor.
        /// </value>
        public override IDataPrevalue PrevalueEditor
        {
            get { return _preValueEditor ?? (_preValueEditor = new IRImagePickerPreValueEditor(this)); }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public override IData Data
        {
            get { return _data ?? (_data = new IRImagePickerData(this, ((IRImagePickerPreValueEditor)PrevalueEditor).GetPreValue<IRImagePickerPreValue>())); }
        }
    }
}
