using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ContentFile
    {
        public int ContentFileId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public string FileExtension { get; set; }

    }
}
