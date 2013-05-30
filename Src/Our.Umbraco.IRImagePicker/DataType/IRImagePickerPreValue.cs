using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.IRImagePicker.DataType
{
    /// <summary>
    /// The PreValue for the IRImagePicker data type
    /// </summary>
    public class IRImagePickerPreValue
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the thumb.
        /// </summary>
        /// <value>
        /// The width of the thumb.
        /// </value>
        public int ThumbWidth { get; set; }

        /// <summary>
        /// Gets or sets the data format.
        /// </summary>
        /// <value>
        /// The data format.
        /// </value>
        public IRImagePickerDataFormat DataFormat { get; set; }
    }
}
