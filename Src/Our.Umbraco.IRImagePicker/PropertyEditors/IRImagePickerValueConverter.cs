using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Our.Umbraco.IRImagePicker.DataType;
using umbraco.presentation;

namespace Our.Umbraco.IRImagePicker.PropertyEditors
{
    //public class IRImagePickerValueConverter : IPropertyEditorValueConverter
    //{
    //    private string _propertyTypeAlias;
    //    private string _docTypeAlias;

    //    public bool IsConverterFor(Guid propertyEditorId, string docTypeAlias, string propertyTypeAlias)
    //    {
    //        _propertyTypeAlias = propertyTypeAlias;
    //        _docTypeAlias = docTypeAlias;

    //        return Guid.Parse("A8A9AD5C-4496-45A7-B06B-EF748B34DB29").Equals(propertyEditorId);
    //    }

    //    public Attempt<object> ConvertPropertyValue(object value)
    //    {
    //        if (UmbracoContext.Current != null)
    //        {
    //            try
    //            {
    //                var result = ((string)value).DeserializeJsonTo<IRImagePickerValue>();

    //                return new Attempt<object>(true, result);
    //            }
    //            catch (System.Xml.XmlException e)
    //            {
    //                return Attempt<object>.False;
    //            }
    //        }

    //        return Attempt<object>.False;
    //    }
    //}
}
