﻿using healthspanmd.calculator.website.Models;
using healthspanmd.core.Services.HealthAgeCalculator_v3;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace healthspanmd.calculator.website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private Patient _patient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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