using healthspanmd.core.CQRS.Content;
using healthspanmd.core.Services.FileSystem;
using healthspanmd.web.Models.Content;
using System;

namespace healthspanmd.web.Helpers
{
    public class ViewHelper : IViewHelper
    {

        private readonly IContentQueries _contentQueries;
        private readonly IFileSystemManager _fileSystemManager;

        public ViewHelper(
            IContentQueries contentQueries,
            IFileSystemManager fileSystemManager
            )
        {
            _contentQueries = contentQueries;
            _fileSystemManager = fileSystemManager;
        }

        public ContentCardPreviewViewModel GetContentCardPreviewViewModel(ContentCardModel contentCard)
        {
            var contentCardPreview = new ContentCardPreviewViewModel
            {
                ContentCardId = contentCard.ContentCardId,
                Name = contentCard.Name,
                Description = contentCard.Description
            };
            if (contentCard.ImageFileId.HasValue)
            {
                var imageContentFile = _contentQueries.GetContentFile(contentCard.ImageFileId.Value);
                var imageData = _fileSystemManager.GetFile("contentfiles", imageContentFile.FileSystemPath);
                var imageBase64Data = Convert.ToBase64String(imageData);
                contentCardPreview.CardImageSrc = $"data:image/{imageContentFile.FileExtension};base64,{imageBase64Data}";
            }
            return contentCardPreview;
        }
    }
}
