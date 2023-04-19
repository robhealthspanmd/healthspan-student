using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.Services.HealthAgeCalculator_v4
{
    public class Email
    {
        public bool moderateActivity { get; set; }
        public bool vigorousActivity { get; set; }
        public bool strengthTraining { get; set; }
        public bool diet { get; set; }
        public bool BMI { get; set; }
        public bool sleep { get; set; }
        public bool cholesterol { get; set; }
        public bool bloodPressure { get; set; }
        public bool smoker { get; set; }
        public bool diabetes { get; set; }
        public bool lonely { get; set; }
        public bool stress { get; set; }
        public bool canEmail { get; set; }

        public async Task<string> sendEmail()
        {
            
            string input = "I am designing a healthage calculator, that takes information about a user and returns a healthage, or rather their age plus or minus their healthly habit. For example, " +
                "smoking would add an extra 12% to their total years in their healthage. The heathage tracks moderate activity, vigorous activity, strength training, diet, BMI, sleep, cholesterol, blood pressure, " +
                "smoking, diabetes, social connection, and stress. I want you to write a positive email to a person about how to improve their health age. Here's what is negatively impacting their health age. I want you to address" +
                "every aspect. This person is diffecent in ";
            if (moderateActivity)
            {
                input += ", moderate activity (include info about zone 2 training)";
            }
            if (vigorousActivity)
            {
                input += ", vigorous activity (include info about HIIT training)";
            }
            if (strengthTraining)
            {
                input += ", strength training";
            }
            if (diet)
            {
                input += ", diet (include advice on limiting high sugar foods, refined grains, alcohol, and sugar added drinks";
            }
            if (bloodPressure)
            {
                input += ", blood pressure";
            }
            if (BMI)
            {
                input += ", BMI";
            }
            if (sleep)
            {
                input += ", sleep";
            }
            if (cholesterol)
            {
                input += ", cholesterol";
            }
            if (smoker)
            {
                input += ", smoking";
            }
            if (diabetes)
            {
                input += ", diabetes";
            }
            if (lonely)
            {
                input += ", social connection";
            }
            if (stress)
            {
                input += ", stress";
            }
            // Initialize the API
          

            try
            {
                // Initialize the API
                OpenAIAPI api = new OpenAIAPI("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");

                // Create a ChatRequest with the user's input
                var request = new ChatRequest
                {
                    Model = Model.ChatGPTTurbo,
                    Temperature = 0.1,
                    MaxTokens = 3000,
                    Messages = new ChatMessage[]
                    {
                new ChatMessage(ChatMessageRole.User, input)
                    }
                };

                // Get the response from ChatGPT
                var result = await api.Chat.CreateChatCompletionAsync(request);
                var reply = result.Choices[0].Message.Content.Trim();
                return reply;
            }
            catch (Exception ex)
            {
                // Log the exception or return a custom error message
                Console.WriteLine("Error while calling ChatGPT: " + ex.Message);
                return "An error occurred while generating the email content.";
            }


        }
    }
}
