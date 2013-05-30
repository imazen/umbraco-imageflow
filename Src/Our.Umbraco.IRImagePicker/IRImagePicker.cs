using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Our.Umbraco.IRImagePicker.DataType;
using umbraco;
using umbraco.cms.businesslogic.media;

namespace Our.Umbraco.IRImagePicker
{
    public static class IRImagePicker
    {
        /// <summary>
        /// Deserializes an IR Image Picker JSON value to an actual IRImagePickerValue entity.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IRImagePickerValue DeserializeValue(string value)
        {
            return value.DeserializeJsonTo<IRImagePickerValue>();
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        internal static string GetUrl(int imageId, string queryString)
        {
            if (imageId <= 0)
                return null;

            try
            {
                var media = new Media(imageId);
                var imageUrl = media.getProperty("umbracoFile").Value as String;
                if (string.IsNullOrEmpty(imageUrl))
                    return null;

                return imageUrl + "?" + queryString.TrimStart('?', '&');
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    [XsltExtension("IRImagePicker")]
    public static class IRImagePickerXslt
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="imageId">The image id.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public static XPathNavigator GetUrl(int imageId, string queryString)
        {
            return IRImagePicker.GetUrl(imageId, queryString).ToXml();
        }
    }
}
