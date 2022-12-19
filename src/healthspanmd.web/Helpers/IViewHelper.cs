using healthspanmd.core.CQRS.Content;
using healthspanmd.web.Models.Content;

namespace healthspanmd.web.Helpers
{
    public interface IViewHelper
    {
        ContentCardPreviewViewModel GetContentCardPreviewViewModel(ContentCardModel contentCard);
    }
}
