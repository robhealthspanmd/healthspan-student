using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public interface IContentCommands
    {
        ContentItemModel CreateContentItem(ContentItemModel item);
        ContentItemModel UpdateContentItem(ContentItemModel item);


        ContentCardModel CreateContentCard(ContentCardModel card);
        ContentCardModel UpdateContentCard(ContentCardModel card);


        void SetContentCardMemberSortOrder(int contentCardMembmerId, int sortOrder);


        /// <summary>
        /// Create and add a Text ContentItem to a ContentCard
        /// </summary>
        /// <param name="contentCardId"></param>
        /// <param name="Text"></param>
        /// <param name="isCaption"></param>
        /// <returns></returns>
        ContentCardModel AddContentItemToContentCard(int contentCardId, string Text, bool isCaption);


        /// <summary>
        /// Create and add a video ContentItem to a ContentCard
        /// </summary>
        /// <param name="contentCardId"></param>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <param name="youTubeVideoId"></param>
        /// <returns></returns>
        ContentCardModel AddContentItemToContentCard(int contentCardId, string Title, string Description, string youTubeVideoId);


        /// <summary>
        /// Create and add a WebLink ContentITem to a ContentCard
        /// </summary>
        /// <param name="contentCardId"></param>
        /// <param name="LinkCaption"></param>
        /// <param name="LinkUrl"></param>
        /// <returns></returns>
        ContentCardModel AddContentItemToContentCard(int contentCardId, string LinkCaption, string LinkUrl);


        /// <summary>
        /// Create and add a (PDF) document ContentItem to a ContentCard
        /// </summary>
        /// <param name="contentCardId"></param>
        /// <param name="documentTitle"></param>
        /// <param name="documentDescription"></param>
        /// <param name="filename"></param>
        /// <param name="fileExtension"></param>
        /// <param name="documentContent"></param>
        /// <returns></returns>
        ContentCardModel AddContentItemToContentCard(int contentCardId, string documentTitle, string documentDescription, string filename, string fileExtension, byte[] documentContent);
        
        
        /// <summary>
        /// Remove a ContentItem from a ContentCard
        /// </summary>
        /// <param name="contentCardId"></param>
        /// <param name="contentItemId"></param>
        /// <returns></returns>
        ContentCardModel RemoveContentItemMemberFromContentCard(int contentCardId, int contentItemId);


        /// <summary>
        /// Create a (PDF) document ContentItem from an existing ContentFile and add to the ContentCard
        /// </summary>
        /// <param name="contentCardId"></param>
        /// <param name="contentFileId"></param>
        /// <returns></returns>
        ContentCardModel AddContentItemToContentCard(int contentCardId, int contentFileId);


        /// <summary>
        /// Creates a new content file (meta) record and returns the corresponding ContentFileModel object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ContentFileModel CreateContentFile(ContentFileModel model);

        void UpdateContentFile(ContentFileModel model);
    }
}
