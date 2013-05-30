using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.IRImagePicker.DataType
{
    /// <summary>
    /// The Value for the IRImagePicker data type
    /// </summary>
    public class IRImagePickerValue
    {
        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        public int ImageId { get; set; }

        /// <summary>
        /// Gets or sets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public string QueryString { get; set; }
    }
}
