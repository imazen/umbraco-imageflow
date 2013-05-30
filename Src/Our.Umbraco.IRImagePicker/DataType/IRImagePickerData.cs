using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using umbraco.cms.businesslogic.datatype;

namespace Our.Umbraco.IRImagePicker.DataType
{
    /// <summary>
    /// The Data for the IRImagePicker data type
    /// </summary>
    public class IRImagePickerData : DefaultData
    {
        protected IRImagePickerPreValue _preValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IRImagePickerData" /> class.
        /// </summary>
        /// <param name="DataType">Type of the data.</param>
        /// <param name="preValue">The pre value.</param>
        public IRImagePickerData(BaseDataType DataType,
            IRImagePickerPreValue preValue) 
            : base(DataType)
        {
            _preValue = preValue;
        }

        /// <summary>
        /// Converts the data to XML.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The data as XML.
        /// </returns>
        public override XmlNode ToXMl(XmlDocument data)
        {
            if (Value != null && !string.IsNullOrEmpty(Value.ToString()))
            {
                var val = Value.ToString().DeserializeJsonTo<IRImagePickerValue>();

                val.QueryString = string.Format("?w={0}&h={1}&mode=crop{2}",
                    _preValue.Width,
                    _preValue.Height,
                    val.QueryString);

                switch (_preValue.DataFormat)
                {
                    case IRImagePickerDataFormat.Xml:

                        var xmlString = new XDocument(new XElement("IRImagePicker",
                            new XElement("ImageId", val.ImageId),
                            new XElement("QueryString", val.QueryString)
                        )).ToString();

                        var xd = new XmlDocument();
                        xd.LoadXml(xmlString);

                        return data.ImportNode(xd.DocumentElement, true);

                    case IRImagePickerDataFormat.Json:

                        return data.CreateTextNode(val.SerializeToJson());
                }

            }

            return base.ToXMl(data);
        }
    }
}
