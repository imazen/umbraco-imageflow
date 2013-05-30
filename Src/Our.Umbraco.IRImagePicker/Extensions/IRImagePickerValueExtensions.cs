using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Our.Umbraco.IRImagePicker.DataType;
using umbraco.cms.businesslogic.media;

namespace Our.Umbraco.IRImagePicker
{
    public static class IRImagePickerValueExtensions
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetUrl(this IRImagePickerValue value)
        {
            if (value == null)
                return null;

            return IRImagePicker.GetUrl(value.ImageId, value.QueryString);
        }
    }
}
