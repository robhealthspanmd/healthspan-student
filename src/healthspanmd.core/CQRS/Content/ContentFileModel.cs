using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public class ContentFileModel
    {
        public int ContentFileId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public string FileExtension { get; set; }

        public string FileSystemFilename => $"contentfile-{ContentFileId}.{FileExtension}";
        public string FileSystemPath => $"/ContentFiles/{FileSystemFilename}";
    }
}
