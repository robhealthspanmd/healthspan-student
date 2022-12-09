using healthspanmd.core.CQRS.Content;
using healthspanmd.core.Services.FileSystem;
using healthspanmd.shared.Extensions;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models.ContentAdmin;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class ContentAdminController : Controller
    {

        private readonly IFileSystemManager _fileSystemManager;
        private readonly IContentCommands _contentCommands;
        private readonly IContentQueries _contentQueries;

        public ContentAdminController(
            IFileSystemManager fileSystemManager,
            IContentCommands contentCommands,
            IContentQueries contentQueries
            )
        {
            _fileSystemManager = fileSystemManager;
            _contentCommands = contentCommands;
            _contentQueries = contentQueries;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Card()
        {
            return View();
        }


        public IActionResult ContentCards()
        {
            return View();
        }

        [Route("/ContentAdmin/ContentCardList_Read")]
        public IActionResult ContentCardList_Read([DataSourceRequest] DataSourceRequest request)
        {
            var cards = _contentQueries.GetList(new GetContentCardListQueryFilter());
            var result = cards.Select(c => c.ToContentCardListItemViewModel())
                            .AsEnumerable();
            return Json(result.ToDataSourceResult(request));
        }

        


        [HttpGet]
        [Route("/ContentAdmin/CardBuilderItems/{contentCardId}")]
        public async Task<IActionResult> CardBuilderItemsAsync(int contentCardId)
        {
            // generate partial view and return rendered html
            var contentCard = _contentQueries.GetContentCard(contentCardId);
            //var html = await this.RenderViewAsync("/Views/ContentAdmin/_ContentBuilder_Items.cshtml", contentCard, true);
            var html = await this.RenderViewAsync("/Views/ContentAdmin/_ContentBuilder_Items.cshtml", contentCard, true);
            return Json(new { success = true, html });
        }


        [HttpGet]
        [Route("/ContentAdmin/CardBuilder/{contentCardId}")]
        public IActionResult CardBuilder(int contentCardId)
        {
            var model = new CardBuilderViewModel
            {
                ContentCard = _contentQueries.GetContentCard(contentCardId),
                ContentTags = _contentQueries.GetAllContentTags()
            };

            if (model.ContentCard.ImageFileId.HasValue)
            {
                var imageContentFile = _contentQueries.GetContentFile(model.ContentCard.ImageFileId.Value);
                var imageData = _fileSystemManager.GetFile("contentfiles", imageContentFile.FileSystemPath);
                var imageBase64Data = Convert.ToBase64String(imageData);
                var imageSrc = $"data:image/{imageContentFile.FileExtension};base64,{imageBase64Data}";
                model.CardImageData = imageSrc;
            }
            else
            {
                model.CardImageData = "/assets/media/illustrations//dozzy-1/18.png";
            }

            this.AddBreadCrumb("Content Cards","/ContentAdmin/ContentCards");
            this.AddBreadCrumb(model.ContentCard.Name);
            return View(model);






            //POCSaveFile();


            //var card = _contentCommands.CreateContentCard(new ContentCardModel
            //{
            //    Name = "My First Card",               
            //});


            //card = _contentCommands.AddContentItemToContentCard(card.ContentCardId, "This is my first content item created with the infrastructure.", true);
            //card = _contentCommands.AddContentItemToContentCard(card.ContentCardId, "Go To HealthspanMD", "https://www.healthspanmd.com");
            //card = _contentCommands.AddContentItemToContentCard(card.ContentCardId, "MRiN9pjyBKo");


            //var filePath = @"C:\Users\kennethhoffman\source\repos\HealthSpanMD\healthspanmd-platform\src\healthspanmd.web\wwwroot\pdf\testdoc.pdf";
            //FileStream fileStream = System.IO.File.OpenRead(filePath);
            //BinaryReader reader = new BinaryReader(fileStream);
            //byte[] buffer = new byte[fileStream.Length];
            //card = _contentCommands.AddContentItemToContentCard(
            //    card.ContentCardId, 
            //    "Test File", 
            //    "This is the first file that was saved to the blob via the data layer.",
            //    "ThePDFfile.pdf","pdf",buffer);

            

            //return View();
        }

        public IActionResult PDFDocument()
        {
            var bytes = _fileSystemManager.GetFile("container2", "CoolFile.pdf");
            var s = new MemoryStream(bytes);
            //return new FileStreamResult(s, "application/pdf");
            return File(s, "application/pdf", "CoolFile.pdf");
        }


        private void POCSaveFile()
        {
            var filePath = @"C:\Users\kennethhoffman\source\repos\HealthSpanMD\healthspanmd-platform\src\healthspanmd.web\wwwroot\pdf\testdoc.pdf";
            FileStream fileStream = System.IO.File.OpenRead(filePath);
            BinaryReader reader = new BinaryReader(fileStream);
            byte[] buffer = new byte[fileStream.Length];
            reader.Read(buffer, 0, buffer.Length);


            _fileSystemManager.SaveFile("container2", "CoolFile2.pdf", buffer);
            var bogus = 1;
        }


        [HttpPost]
        [Route("/ContentAdmin/AddContentItem")]
        public async Task<IActionResult> AddContentItem([FromBody] AddContentItemModel model)
        {
            //var newItem = _contentCommands.CreateContentItem(new ContentItemModel
            //{             
            //    Name = model.Name,
            //    Text = model.Text,
            //    Url = model.Url,
            //    IsCaption = model.IsCaption,
            //    ItemType = model.ItemType,
            //    ContentFileId = model.ContentFileId,
            //});

            //var contentCard = _contentQueries.GetContentCard(model.ContentCardId);
            ContentCardModel contentCard = null;

            switch (model.ItemType)
            {
                case ContentItemType.Text:
                    contentCard = _contentCommands.AddContentItemToContentCard(model.ContentCardId, model.Text, model.IsCaption);
                    break;

                case ContentItemType.WebLink:
                    contentCard = _contentCommands.AddContentItemToContentCard(model.ContentCardId, model.Text, model.Url);
                    break;

                case ContentItemType.Video:
                    contentCard = _contentCommands.AddContentItemToContentCard(model.ContentCardId, model.Name, model.Text, model.Url);
                    break;

                case ContentItemType.PDFDocument:
                    contentCard = _contentCommands.AddContentItemToContentCard(model.ContentCardId, model.ContentFileId.Value);

                    // update meta on ContentFile
                    var contentFile = _contentQueries.GetContentFile(model.ContentFileId.Value);
                    contentFile.Title = model.Name;
                    contentFile.Description = model.Text;
                    _contentCommands.UpdateContentFile(contentFile);

                    break;
            }

            return Json(new { success = true });
        }


        [HttpGet]
        [Route("/ContentAdmin/RemoveContentItem/{contentCardId}/{contentItemId}")]
        public IActionResult RemoveContentItem(int contentCardId, int contentItemId)
        {
            _contentCommands.RemoveContentItemMemberFromContentCard(contentCardId, contentItemId);
            return Json(new { success = true });
        }


        [HttpPost]
        [Route("/ContentAdmin/UpdateCardName")]
        public IActionResult UpdateCardName([FromBody] UpdateContentCardNameViewModel model)
        {
            var card = _contentQueries.GetContentCard(model.ContentCardId);
            card.Name = model.Name;
            card.Description = model.Description;
            card.NotificationMessage = model.NotificationMessage;

            // translate the list of Tagify Tags into a List of TagIds
            var selectedTags = new List<TagifyTag>();
            if (model.SelectedTags.Count() > 0)
                selectedTags = JsonSerializer.Deserialize<List<TagifyTag>>(model.SelectedTags);
            var tags = _contentQueries.GetAllContentTags();
            var tagIds = new List<int>();
            foreach (var tag in selectedTags)
            {
                var contentTag = tags.Where(t => t.Name == tag.value).FirstOrDefault();
                if (contentTag != null)
                    tagIds.Add(contentTag.ContentTagId);
            }

            card.SetTags(tagIds);

            _contentCommands.UpdateContentCard(card);

            var selectedTagList = selectedTags.Select(t => t.value).ToList();
            var selectedTagListStr = string.Join(",", selectedTagList);
            return Json(new { success = true, selectedTagListStr });
        }

        

        [HttpGet]
        [Route("/ContentAdmin/UpdateCardImage/{contentCardId}/{contentFileId}")]
        public IActionResult UpdateCardImage(int contentCardId, int contentFileId)
        {
            var card = _contentQueries.GetContentCard(contentCardId);
            card.ImageFileId = contentFileId;
            _contentCommands.UpdateContentCard(card);

            var imageContentFile = _contentQueries.GetContentFile(contentFileId);
            var imageData = _fileSystemManager.GetFile("contentfiles", imageContentFile.FileSystemPath);
            var imageBase64Data = Convert.ToBase64String(imageData);
            var imageSrc = $"data:image/{imageContentFile.FileExtension};base64,{imageBase64Data}";

            return Json(new { success = true, imageSrc });
        }


        [HttpPost]
        [Route("/ContentAdmin/CreateContentCard")]
        public IActionResult CreateContentCard([FromBody] UpdateContentCardNameViewModel model)
        {
            var contentCard = _contentCommands.CreateContentCard(new ContentCardModel
            {
                IsActive = true,
                Name = model.Name
            });
            return Json(new { success = true, contentCardId = contentCard.ContentCardId });
        }


        [HttpPost]
        [Route("/ContentAdmin/UploadFile")]
        public IActionResult UploadFile()
        {
            var fileCollection = HttpContext.Request.Form.Files;
            var file = fileCollection[0];
            byte[] fileData = null;
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileData = ms.ToArray();
                }
            }

            // create a ContentFile (meta) object
            var contentFile = _contentCommands.CreateContentFile(new ContentFileModel
            {
                Filename = file.FileName,
                FileExtension = Path.GetExtension(file.FileName).Replace(".","").ToLower(),
            });

            // save file to file system
            _fileSystemManager.SaveFile("contentfiles", contentFile.FileSystemPath, fileData);

            // return the ContentFileId
            return Json(new { success = true, contentFileId = contentFile.ContentFileId });
        }


        [HttpPost]
        [Route("/ContentAdmin/ReOrderCard")]
        public async Task<IActionResult> ReOrderCardAsync([FromBody] ContentCardSortOrderModel sortModel)
        {
            var contentCard = _contentQueries.GetContentCard(sortModel.ContentCardId);
            int sortOrder = 0;
            foreach (var contentItemId in sortModel.ContentItemIds)
            {
                var member = contentCard.ContentCardMembers
                    .Where(m => m.ContentItemId == contentItemId)
                    .FirstOrDefault();
                _contentCommands.SetContentCardMemberSortOrder(member.ContentCardMemberId, sortOrder);
                sortOrder += 1;
            }
            contentCard = _contentQueries.GetContentCard(sortModel.ContentCardId);
            var html = await this.RenderViewAsync("/Views/Content/_ContentCard.cshtml", contentCard, true);
            return Json(new { success = true, html });
        }

        [HttpGet]
        [Route("/ContentAdmin/ContentTags")]
        public IActionResult ContentTags()
        {
            return View();
        }


        [Route("/ContentAdmin/ContentTag_Read")]
        public IActionResult ContentTag_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = _contentQueries.GetAllContentTags().OrderBy(t => t.Name).AsEnumerable();
            return Json(result.ToDataSourceResult(request));
        }

        [Route("/ContentAdmin/ContentTag_Create")]
        public IActionResult ContentTag([DataSourceRequest] DataSourceRequest request, ContentTagModel model)
        {
            if (ModelState.IsValid)
            {
                model = _contentCommands.CreateContentTag(model);
            }
            // Return the updated product. Also return any validation errors.
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [Route("/ContentAdmin/ContentTag_Destroy")]
        public IActionResult ContentTag_Destroy([DataSourceRequest] DataSourceRequest request, ContentTagModel model)
        {
            if (ModelState.IsValid)
            {
                _contentCommands.DeleteContentTag(model.ContentTagId);
            }
            // Return the updated product. Also return any validation errors.
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [Route("/ContentAdmin/ContentTag_Update")]
        public IActionResult ContentTag_Update([DataSourceRequest] DataSourceRequest request, ContentTagModel model)
        {
            if (ModelState.IsValid)
            {
                _contentCommands.UpdateContentTag(model);
            }
            // Return the updated product. Also return any validation errors.
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

    }
}
