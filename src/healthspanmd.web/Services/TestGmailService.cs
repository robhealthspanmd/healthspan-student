

using EASendMail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace healthspanmd.web.Services
{
    public class TestGmailService
    {

        static string[] Scopes = { GmailService.Scope.MailGoogleCom };
        static string ApplicationName = "HealthspanMD Platform OAuth";

        public static void SendTestMessageUsingApi(IConfiguration config)
        {
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/gmail-dotnet-quickstart.json
            var credPath = config.GetSection("GoogleApiSetting:PathToKeyFile").Value;


            //UserCredential credential;

            //using (var stream =
            //    new FileStream(credPath, FileMode.Open, FileAccess.Read))
            //{
            //    // The file token.json stores the user's access and refresh tokens, and is created
            //    // automatically when the authorization flow completes for the first time.
            //    string tokenPath = config.GetSection("GoogleApiSetting:PathToTokenStore").Value;
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.FromStream(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(tokenPath, true)).Result;
            //    Console.WriteLine("Credential file saved to: " + tokenPath);
            //}


            GoogleCredential credential;

            using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential
                    .FromStream(stream)
                    .CreateScoped(GmailService.Scope.MailGoogleCom)
                    .CreateWithUser("notification@healthspanmd.com");
            }


            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });








            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("notification@healthspanmd.com");
            mailMessage.To.Add("hoffman@oemdata.com");
            mailMessage.Subject = "test email";
            mailMessage.Body = "My first Gmail Api test email from azure";
            mailMessage.IsBodyHtml = false;

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Encode(mimeMessage)
            };

            Google.Apis.Gmail.v1.UsersResource.MessagesResource.SendRequest request = service.Users.Messages.Send(gmailMessage, "notification@healthspanmd.com");

            request.Execute();














            // Create a message
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("My first Gmail Api test email from azure");
            //Message email = new Message
            //{
            //    Payload = new MessagePart
            //    {
            //        //Body = new MessagePartBody { Data = System.Convert.ToBase64String(plainTextBytes) },
            //        //MimeType = "message/rfc822",
            //        Headers = new List<MessagePartHeader>()
            //        {
            //            new MessagePartHeader
            //            {
            //                Name = "To",
            //                Value = "hoffman@oemdata.com"
            //            },
            //             new MessagePartHeader
            //            {
            //                Name = "From",
            //                Value = "notification@healthspanmd.com"
            //            },
            //             new MessagePartHeader
            //            {
            //                Name = "Subject",
            //                Value = "test email"
            //            }
            //        }
            //    }
            //};




            //var headerTo = new MessagePartHeader
            //{
            //    Name = "To",
            //    Value = "hoffman@oemdata.com"
            //};
            //email.Payload.Headers.Add(headerTo);
            //var headerFrom = new MessagePartHeader
            //{
            //    Name = "From",
            //    Value = "notification@healthspanmd.com"
            //};
            //email.Payload.Headers.Add(headerFrom);
            //var headerSubject = new MessagePartHeader
            //{
            //    Name = "Subject",
            //    Value = "test email"
            //};
            //email.Payload.Headers.Add(headerSubject);

            //var message = service.Users.Messages.Send(email, "notification@healthspanmd.com").Execute();


            //// Define parameters of request.
            //UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

            //// List labels.
            //IList<Label> labels = request.Execute().Labels;
            //Console.WriteLine("Labels:");
            //if (labels != null && labels.Count > 0)
            //{
            //    foreach (var labelItem in labels)
            //    {
            //        Console.WriteLine("{0}", labelItem.Name);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No labels found.");
            //}

            var bogus = 1;
            
        }


        public static string Encode(MimeMessage mimeMessage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                mimeMessage.WriteTo(ms);
                return Convert.ToBase64String(ms.GetBuffer())
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }

        public static async Task SendTestMessageUsingServiceAccountAsync(IConfiguration config)
        {

            // service account email address
            //https://www.emailarchitect.net/easendmail/ex/c/21.aspx
            const string serviceAccount = "healthspanmd-platform-service@healthspanmd-platform.iam.gserviceaccount.com";
            var filePath = config.GetSection("GoogleApiSetting:PathToKeyFile").Value;
            // import service account key p12 certificate.
            //var certificate = new X509Certificate2("C:\\Users\\kennethhoffman\\source\\repos\\HealthSpanMD\\healthspanmd-platform\\src\\healthspanmd.web\\Output\\healthspanmd-platform-c78781648266.p12",
            //    "notasecret", X509KeyStorageFlags.Exportable);
            var certificate = new X509Certificate2(filePath, "notasecret", X509KeyStorageFlags.Exportable);

            // G Suite user email address
            var gsuiteUser = "notification@healthspanmd.com";

            var serviceAccountCredentialInitializer = new ServiceAccountCredential.Initializer(serviceAccount)
            {
                User = gsuiteUser,
                Scopes = new[] { "https://mail.google.com/" }

            }.FromCertificate(certificate);

            // if service account key is in json format, copy the private key from json file:
            // "private_key": "-----BEGIN PRIVATE KEY-----\n...\n-----END PRIVATE KEY-----\n"
            // and import it like this:
            // string privateKey = "-----BEGIN PRIVATE KEY-----\nMIIEv...revdd\n-----END PRIVATE KEY-----\n";

            // var serviceAccountCredentialInitializer = new ServiceAccountCredential.Initializer(serviceAccount)
            //{
            //    User = gsuiteUser,
            //    Scopes = new[] { "https://mail.google.com/" }
            // }.FromPrivateKey(privateKey);

            // request access token
            var credential = new ServiceAccountCredential(serviceAccountCredentialInitializer);
            if (!credential.RequestAccessTokenAsync(CancellationToken.None).Result)
                throw new InvalidOperationException("Access token failed.");

            var server = new SmtpServer("smtp.gmail.com 587");
            server.ConnectType = SmtpConnectType.ConnectSSLAuto;

            server.User = gsuiteUser;
            server.Password = credential.Token.AccessToken;

            server.AuthType = SmtpAuthType.XOAUTH2;

            var mail = new SmtpMail("TryIt");

            mail.From = gsuiteUser;
            // Please change recipient address to yours for test
            mail.To = "hoffman@oemdata.com";

            mail.Subject = "service account oauth test";
            mail.TextBody = "this is a test, don't reply";

            var smtp = new SmtpClient();
            smtp.SendMail(server, mail);

            Console.WriteLine("Message delivered!");


        }


    }
}

