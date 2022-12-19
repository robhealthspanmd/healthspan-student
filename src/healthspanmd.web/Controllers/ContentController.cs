using healthspanmd.core.CQRS.Content;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.FileSystem;
using healthspanmd.web.Helpers;
using healthspanmd.web.Models.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace healthspanmd.web.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private readonly IFileSystemManager _fileSystemManager;
        private readonly IContentQueries _contentQueries;
        private readonly IUserCommands _userCommands;
        private readonly IUserQueries _userQueries;


        public ContentController(
            IFileSystemManager fileSystemManager, 
            IContentQueries contentQueries,
            IUserCommands userCommands,
            IUserQueries userQueries
            )
        {
            _fileSystemManager = fileSystemManager;
            _contentQueries = contentQueries;
            _userCommands = userCommands;
            _userQueries = userQueries;
        }


        [HttpGet]
        [Route("/Content")]
        public IActionResult Index()
        {
            var user = _userQueries.GetUserDetailModel(User.Identity.Name, true);
            var model = new ContentLibraryViewModel
            {
                AssignedContent = user.ContentCardAssignments
                    .Where(a => !a.CompletedUtc.HasValue)
                    .OrderBy(a => a.SortOrder)
                    .Select(a => a.ContentCard)
                    .ToList(),
                Tags = _contentQueries.GetContentTagsWithAssignments()
            };

            this.AddBreadCrumb("Library");
            return View(model);
        }


        [HttpGet]
        [Route("/Content/Tag/{contentTagId}")]
        public IActionResult ContentTagListing(int contentTagId)
        {
            var model = new ContentTagListingViewModel
            {
                TaggedContent = _contentQueries.GetList(new GetContentCardListQueryFilter
                {
                    ActiveOnly = true,
                    ContentTagId = contentTagId
                }),
                Tags = _contentQueries.GetContentTagsWithAssignments(),
            };
            model.SelectedTag = model.Tags.Where(t => t.ContentTagId == contentTagId).FirstOrDefault();

            this.AddBreadCrumb("Library", "/Content");
            this.AddBreadCrumb(model.SelectedTag.Name);
            return View(model);
        }



        [HttpGet]
        [Route("/Content/{contentCardId}")]
        public IActionResult CardView(int contentCardId)
        {
            var model = new ContentCardViewModel
            {
                ContentCard = _contentQueries.GetContentCard(contentCardId),
            };

            this.AddBreadCrumb(model.ContentCard.Name);
            return View(model);
        }
    

        [HttpGet]
        [Route("/Content/Card/{contentCardId}")]
        public IActionResult ContentCard(int contentCardId)
        {
            var card = _contentQueries.GetContentCard(contentCardId);
            return PartialView("_ContentCard", card);
        }


        [HttpGet]
        [Route("/Content/DownloadFile/{contentFileId}")]
        public IActionResult DownloadFile(int contentFileId)
        {
            var contentFile = _contentQueries.GetContentFile(contentFileId);

            var bytes = _fileSystemManager.GetFile("contentfiles", contentFile.FileSystemPath);
            var s = new MemoryStream(bytes);
            return new FileStreamResult(s, "application/pdf");
            //return File(s, "application/pdf", contentFile.Filename);
        }


        [HttpGet]
        [Route("/Content/MarkCardAsCompleted/{contentCardId}/{contentItemId}")]
        public IActionResult MarkCardAsCompleted(int contentCardId, int contentItemId)
        {
            var user = _userQueries.GetUserDetailModel(User.Identity.Name, true);
            _userCommands.MarkContentCardAssignmentAsComplete(user.UserId, contentCardId);
            return Json(new { success = true });
        }

    }
}
