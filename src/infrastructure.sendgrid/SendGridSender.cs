using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using healthspanmd.core.Services.HealthAgeCalculator_v3;
using healthspanmd.calculator.website.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace infrastructure.sendgrid
{
    public class SendGridSender : Controller
    {
        private Patient _patient;

        [HttpPost]
        public async Task<IActionResult> HealthAge(Patient patient)
        {
            // Do something with the patient object
            HealthAgeCalculator cal = new HealthAgeCalculator();
            HealthAgeCalculatorResult result = await cal.CalculateResultAsync(patient);
            HealthAgeFinal healthAgeFinal = new HealthAgeFinal();
            healthAgeFinal.result = result;
            healthAgeFinal.patient = patient;

            _patient = patient;

            async Task sendEmail()
            {
                // API key will be provided offline due to being a public GitHub repo
                var apiKey = "";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("healthagecalc.asu@gmail.com", "HealthspanMD");
                var subject = "HealthAge Calculator Results";
                var to = new EmailAddress($"{_patient.email}", "Patient");
                var plainTextContent = "HealthAge: " + result.HealthAge.ToString();
                var htmlContent = "<strong>HealthAge: </strong>" + result.HealthAge.ToString();
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }

            sendEmail().Wait();

            return View(healthAgeFinal);
        }
    }
}