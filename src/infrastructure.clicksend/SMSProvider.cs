using healthspanmd.core.Services.SMS;
using IO.ClickSend.ClickSend.Api;
using IO.ClickSend.ClickSend.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace infrastructure.clicksend
{
    public class SMSProvider : ISMSProvider
    {
        private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;


        public SMSProvider(
            IConfiguration configuration
            //ILogger logger
            )
        {
            _configuration = configuration;
           //_logger = logger;
        }

        public SendResponse Send(SendRequest sendRequest)
        {


            var smsApi = new SMSApi();
            smsApi.Configuration.Username = _configuration.GetSection("ClickSendSettings:apiUsername").Value;
            smsApi.Configuration.Password = _configuration.GetSection("ClickSendSettings:apiKey").Value;


            var smsList = new List<SmsMessage>
            {
                new SmsMessage(
                    to: ConditionPhoneNumber(sendRequest.PhoneNumber),
                    body: sendRequest.Message.Replace("\n",Environment.NewLine),
                    from: _configuration.GetSection("ClickSendSettings:dedicatedSendFromNumber").Value
                )
            };
            //_logger.LogInformation($"Attempting to send message to {ConditionPhoneNumber(sendRequest.PhoneNumber)}");
            Console.WriteLine($"Attempting to send message to {ConditionPhoneNumber(sendRequest.PhoneNumber)}");

            var smsCollection = new SmsMessageCollection(smsList);
            var response = smsApi.SmsSendPost(smsCollection);

            
            var responseObject = JsonSerializer.Deserialize<SendSMSResponse>(response);



            //_logger.LogInformation($"Message sent to {ConditionPhoneNumber(sendRequest.PhoneNumber)}; response_code = {responseObject.response_code}");
            Console.WriteLine($"Message sent to {ConditionPhoneNumber(sendRequest.PhoneNumber)}; response_code = {responseObject.response_code}");
            if (responseObject.response_code != "SUCCESS")
            {
                //_logger.LogError($"response_msg: {responseObject.response_msg}");
                Console.WriteLine($"response_msg: {responseObject.response_msg}");
            }

            return new SendResponse
            {
                Success = responseObject.response_code == "SUCCESS",
                Message = responseObject.response_msg
            };

        }

        public string ConditionPhoneNumber(string input)
        {
            string result = input.Trim();
            if (!result.StartsWith("+")) 
                result = "+" + result;

            return result
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace(".", "")
                    .Replace("(", "")
                    .Replace(")", "");
        }

    }
}
