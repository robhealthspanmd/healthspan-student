using healthspanmd.core.CQRS.Content;
using healthspanmd.core.Services.FileSystem;
using infrastructure.sqlserver.Data;
using infrastructure.sqlserver.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.CQRS.Content
{
    public class ContentCommands : IContentCommands
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IFileSystemManager _fileSystemManager;

        public ContentCommands(
            IServiceScopeFactory scopeFactory,
            IFileSystemManager fileSystemManager
            )
        {
            _scopeFactory = scopeFactory;
            _fileSystemManager = fileSystemManager;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Add TEXT ContentItem
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ContentCardModel AddContentItemToContentCard(int contentCardId, string Text, bool isCaption)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = ContentQueries.GetContentCardEntity(contentCardId, dbContext);

                var contentItem = new ContentItem
                {
                    ItemType = ContentItemType.Text,
                    Text = Text,
                    IsCaption = isCaption,
                };

                contentCard.ContentCardMembers.Add(new ContentCardMember
                {
                    ContentItem = contentItem,
                    SortOrder = contentCard.ContentCardMembers.Count
                });

                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();

                return contentCard.ToContentCardModel();
            }
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Add VIDEO ContentItem
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ContentCardModel AddContentItemToContentCard(int contentCardId, string title, string description, string youTubeVideoId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = ContentQueries.GetContentCardEntity(contentCardId, dbContext);

                var contentItem = new ContentItem
                {
                    ItemType = ContentItemType.Video,
                    Url = youTubeVideoId,
                    Name = title,
                    Text = description
                };

                contentCard.ContentCardMembers.Add(new ContentCardMember
                {
                    ContentItem = contentItem,
                    SortOrder = contentCard.ContentCardMembers.Count
                });

                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();

                return contentCard.ToContentCardModel();

            }
        }



        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Add WEBLINK ContentItem
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ContentCardModel AddContentItemToContentCard(int contentCardId, string LinkCaption, string LinkUrl)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = ContentQueries.GetContentCardEntity(contentCardId, dbContext);

                var contentItem = new ContentItem
                {
                    ItemType = ContentItemType.WebLink,
                    Url = LinkUrl,
                    Text = LinkCaption
                };

                contentCard.ContentCardMembers.Add(new ContentCardMember
                {
                    ContentItem = contentItem,
                    SortOrder = contentCard.ContentCardMembers.Count
                });

                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();

                return contentCard.ToContentCardModel();

            }
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Add PDFDOCUMENT ContentItem
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ContentCardModel AddContentItemToContentCard(int contentCardId, string documentTitle, string documentDescription, string filename, string fileExtension, byte[] fileContent)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = ContentQueries.GetContentCardEntity(contentCardId, dbContext);

                var contentItem = new ContentItem
                {
                    ItemType = ContentItemType.PDFDocument,
                    ContentFile = new ContentFile
                    {
                        Title = documentTitle,
                        Description = documentDescription,
                        Filename = filename,
                        FileExtension = fileExtension,
                    }
                };

                contentCard.ContentCardMembers.Add(new ContentCardMember
                {
                    ContentItem = contentItem,
                    SortOrder = contentCard.ContentCardMembers.Count
                });

                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();

                // determine the file system name
                var contentFile = contentItem.ContentFile.ToContentFileModel();
                _fileSystemManager.SaveFile("contentfiles", contentFile.FileSystemPath, fileContent);

                return contentCard.ToContentCardModel();

            }
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Add Existing PDFDOCUMENT ContentItem
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ContentCardModel AddContentItemToContentCard(int contentCardId, int contentFileId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = ContentQueries.GetContentCardEntity(contentCardId, dbContext);

                var contentItem = new ContentItem
                {
                    ItemType = ContentItemType.PDFDocument,
                    ContentFileId = contentFileId
                };

                contentCard.ContentCardMembers.Add(new ContentCardMember
                {
                    ContentItem = contentItem,
                    SortOrder = contentCard.ContentCardMembers.Count
                });

                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();

                //var contentFile = contentItem.ContentFile.ToContentFileModel();

                return contentCard.ToContentCardModel();

            }
        }

        public ContentCardModel CreateContentCard(ContentCardModel card)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = card.ToContentCard();
                dbContext.ContentCards.Add(contentCard);
                dbContext.SaveChanges();
                return contentCard.ToContentCardModel();
            }
        }

        public ContentItemModel CreateContentItem(ContentItemModel item)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentItem = item.ToContentItem();
                dbContext.ContentItems.Add(contentItem);
                dbContext.SaveChanges();
                return contentItem.ToContentItemModel();
            }
        }

        public ContentCardModel RemoveContentItemMemberFromContentCard(int contentCardId, int contentMemberId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = ContentQueries.GetContentCardEntity(contentCardId, dbContext);
                var member = contentCard.ContentCardMembers.Where(m => m.ContentCardMemberId == contentMemberId).FirstOrDefault();
                if (member != null)
                    contentCard.ContentCardMembers.Remove(member);
                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();
                return contentCard.ToContentCardModel();
            }
        }

        public void SetContentCardMemberSortOrder(int contentCardMemberId, int sortOrder)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var member = dbContext.ContentCardsMembers.Find(contentCardMemberId);
                member.SortOrder = sortOrder;
                dbContext.ContentCardsMembers.Update(member);
                dbContext.SaveChanges();
            }
        }

        public ContentCardModel UpdateContentCard(ContentCardModel card)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = card.ToContentCard();
                dbContext.ContentCards.Update(contentCard);
                dbContext.SaveChanges();
                return contentCard.ToContentCardModel();
            }
        }

        public ContentItemModel UpdateContentItem(ContentItemModel item)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentItem = item.ToContentItem();
                dbContext.ContentItems.Update(contentItem);
                dbContext.SaveChanges();
                return contentItem.ToContentItemModel();
            }
        }

        public ContentFileModel CreateContentFile(ContentFileModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentFile = model.ToContentFile();
                dbContext.ContentFiles.Add(contentFile);
                dbContext.SaveChanges();
                return contentFile.ToContentFileModel();
            }
        }

        public void UpdateContentFile(ContentFileModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentFile = model.ToContentFile();
                dbContext.ContentFiles.Update(contentFile);
                dbContext.SaveChanges();
            }
        }

        public ContentTagModel CreateContentTag(ContentTagModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentTag = model.ToContentTag();
                dbContext.ContentTags.Add(contentTag);
                dbContext.SaveChanges();
                return contentTag.ToContentTagModel();
            }
        }

        public void UpdateContentTag(ContentTagModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentTag = model.ToContentTag();
                dbContext.ContentTags.Update(contentTag);
                dbContext.SaveChanges();
            }
        }

        public void DeleteContentTag(int contentTagId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var tagToDelete = dbContext.ContentTags.Find(contentTagId);
                dbContext.ContentTags.Remove(tagToDelete);
                dbContext.SaveChanges();
            }
        }

        public ContentTagAssignmentModel CreateContentTagAssignment(ContentTagAssignmentModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var assignment = model.ToContentTagAssignment();
                dbContext.ContentTagAssignments.Add(assignment);
                dbContext.SaveChanges();
                return assignment.ToContentTagAssignmentModel();
            }
        }

        public void UpdateContentTagAssignment(ContentTagAssignmentModel model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var assignment = model.ToContentTagAssignment();
                dbContext.ContentTagAssignments.Update(assignment);
                dbContext.SaveChanges();
            }
        }

        public void DeleteContentTagAssignment(int contentTagAssignmentId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var assignmentToDelete = dbContext.ContentTagAssignments.Find(contentTagAssignmentId);
                dbContext.ContentTagAssignments.Remove(assignmentToDelete);
                dbContext.SaveChanges();
            }
        }
    }
}
