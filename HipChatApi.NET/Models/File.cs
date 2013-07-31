
namespace HipChatApi.Models
{
    public class File
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Size in bytes.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// URL of uploaded file.
        /// </summary>
        public string Url { get; set; }

        internal FileRaw ToFileRaw()
        {
            return new FileRaw
            {
                name = Name,
                size = Size,
                url = Url
            };
        }
    }

    internal class FileRaw
    {
        /// <summary>
        /// File name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Size in bytes.
        /// </summary>
        public int? size { get; set; }

        /// <summary>
        /// URL of uploaded file.
        /// </summary>
        public string url { get; set; }

        public File ToFile()
        {
            return new File
            {
                Name = name,
                Size = size,
                Url = url
            };
        }
    }
}
