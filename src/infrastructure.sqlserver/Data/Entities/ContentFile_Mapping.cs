using healthspanmd.core.CQRS.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public static class ContentFile_Mapping
    {
        public static ContentFileModel ToContentFileModel(this ContentFile f)
        {
            return new ContentFileModel
            {
                ContentFileId = f.ContentFileId,
                Description = f.Description,
                FileExtension = f.FileExtension,
                Filename = f.Filename,
                Title = f.Title,           
            };
        }

        public static ContentFile ToContentFile(this ContentFileModel f)
        {
            return new ContentFile
            {
                ContentFileId = f.ContentFileId,
                Description = f.Description,
                FileExtension = f.FileExtension,
                Filename = f.Filename,
                Title = f.Title,
            };
        }
    }
}
