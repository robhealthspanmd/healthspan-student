﻿using healthspanmd.core.CQRS.Content;
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
    public class ContentQueries : IContentQueries
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ContentQueries(
            IServiceScopeFactory scopeFactory
            )
        {
            _scopeFactory = scopeFactory;
        }


        public ICollection<ContentCardModel> GetList(GetContentCardListQueryFilter filter)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var query = dbContext.ContentCards
                    .Include(c => c.ContentTagAssignments)
                        .ThenInclude(a => a.ContentTag)
                    .AsQueryable();


                if (filter.ActiveOnly.HasValue)
                    if (filter.ActiveOnly.Value)
                        query = query.Where(c => c.IsActive);

                if (filter.ContentTagId.HasValue)
                    query = query.Where(c => c.ContentTagAssignments.Any(a => a.ContentTagId == filter.ContentTagId));


                return query.Select(c => c.ToContentCardModel()).ToList();
            }
        }

        public ContentCardModel GetContentCard(int contentCardId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentCard = GetContentCardEntity(contentCardId,dbContext);
                if (contentCard.ImageFileId.HasValue)
                    if (contentCard.ImageFileId.Value == 0)
                        contentCard.ImageFileId = null; // remove a corrupt assignment
                return contentCard.ToContentCardModel();
            }
        }

        public static ContentCard GetContentCardEntity(int contentCardId, HealthSpanMdDbContext dbContext)
        {
            var contentCard = dbContext.ContentCards
                .Where(c => c.ContentCardId == contentCardId)
                .Include(c => c.ContentCardMembers)
                    .ThenInclude(m => m.ContentItem)
                        .ThenInclude(i => i.ContentFile)
                .Include(c => c.ContentTagAssignments)
                    .ThenInclude(a => a.ContentTag)
                .FirstOrDefault();
            return contentCard;
        }

        public ContentFileModel GetContentFile(int contentFileId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                var contentFile = dbContext.ContentFiles.Find(contentFileId);
                return contentFile.ToContentFileModel();
            }
        }

        public ICollection<ContentTagModel> GetAllContentTags()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();
                return dbContext.ContentTags
                    .Select(t => t.ToContentTagModel())
                    .ToList();
            }
        }

        public ICollection<ContentTagModel> GetContentTagsWithAssignments()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthSpanMdDbContext>();

                var assignedContentTags = dbContext.ContentTagAssignments
                    .Select(a => a.ContentTagId).Distinct()
                    .ToList();

                return dbContext.ContentTags
                    .Where(t => assignedContentTags.Contains(t.ContentTagId))
                    .Select(t => t.ToContentTagModel())
                    .ToList();
            }
        }
    }
}
