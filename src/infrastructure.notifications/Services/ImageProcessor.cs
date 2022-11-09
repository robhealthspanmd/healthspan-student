using HtmlAgilityPack;
using infrastructure.notifications.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.notifications
{
    public class ImageProcessor
    {
        public static EmbedImagesResponse EmbedImages(string htmlString)
        {
            var response = new EmbedImagesResponse();
            var linkedResources = new List<LinkedResource>();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlString);
            document.DocumentNode.Descendants("img")
                .Where(e =>
                {
                    string src = e.GetAttributeValue("src", null) ?? "";
                    return !string.IsNullOrEmpty(src);
                })
                .ToList()
                .ForEach(x =>
                {
                    string currentSrcValue = x.GetAttributeValue("src", null);
                    byte[] imageData = (byte[])ImageResources.ResourceManager.GetObject(currentSrcValue);

                    string contentId = Guid.NewGuid().ToString();
                    //response.Attachments.Add(new Attachment(new MemoryStream(imageData), contentId));
                    //contentId = response.Attachments.ToList()[response.Attachments.Count - 1].ContentId;
                    LinkedResource img = new LinkedResource(new MemoryStream(imageData), "image/png");
                    img.ContentId = contentId;
                    img.TransferEncoding = TransferEncoding.Base64;
                    img.ContentType.Name = img.ContentId;
                    img.ContentLink = new Uri("cid:" + img.ContentId);
                    linkedResources.Add(img);


                    x.SetAttributeValue("src", "cid:" + contentId);
                });

            response.htmlView = AlternateView.CreateAlternateViewFromString(document.DocumentNode.OuterHtml, Encoding.UTF8, MediaTypeNames.Text.Html);
            foreach (var linkedResource in linkedResources)
                response.htmlView.LinkedResources.Add(linkedResource);

            return response;
        }
    }
}
