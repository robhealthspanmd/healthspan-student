using healthspanmd.core.CQRS.Content;

namespace healthspanmd.web.Models.ContentAdmin
{
    public static class ContentCardListItemViewModel_Mapping
    {
        public static ContentCardListItemViewModel ToContentCardListItemViewModel(this ContentCardModel c)
        {
            return new ContentCardListItemViewModel
            {
                ContentCardId = c.ContentCardId,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive
            };
        }
    }
}
