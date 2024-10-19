using BankingProject.Data;
using BankingProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace BankingProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, ApplicationDbContext context)
		{
			_logger = logger;
            _httpClientFactory = httpClientFactory;
            _context = context;
		}

        public async Task<IActionResult> Index()
        {
            var currentDate = DateTime.Now;//16/10/2024

            if (!_context.ExchangeRates.Any())
            {
                await CreateExchangeRate("ALL");
                await CreateExchangeRate("EUR");
                await CreateExchangeRate("USD");
                await CreateExchangeRate("GBP");
            }
            else
            {
                var currentRates = await _context.ExchangeRates.ToListAsync();
                var createRecords = false;

                foreach (var rate in currentRates)
                {
                    if (rate.FetchDate.ToString("dd/MM/yyyy") != currentDate.ToString("dd/MM/yyyy"))
                    {
                        _context.ExchangeRates.Remove(rate);
                        await _context.SaveChangesAsync();
                        createRecords = true;
                    }
                }

                if (createRecords)
                {
                    await CreateExchangeRate("ALL");
                    await CreateExchangeRate("EUR");
                    await CreateExchangeRate("USD");
                    await CreateExchangeRate("GBP");
                }
            }


            var exhangeRateALL = await _context.ExchangeRates.Where(e => e.Currency == "ALL").FirstAsync();
            var exhangeRateEUR = await _context.ExchangeRates.Where(e => e.Currency == "EUR").FirstAsync();
			var exhangeRateUSD = await _context.ExchangeRates.Where(e => e.Currency == "USD").FirstAsync();
			var exhangeRateGBP = await _context.ExchangeRates.Where(e => e.Currency == "GBP").FirstAsync();

			ViewBag.ExhangeRateALL = exhangeRateALL;
			ViewBag.ExhangeRateEUR = exhangeRateEUR;
			ViewBag.ExhangeRateUSD = exhangeRateUSD;
			ViewBag.ExhangeRateGBP = exhangeRateGBP;

            ViewBag.Transfers = await _context.Transfers.Where(t => t.CreatedAt.Year == currentDate.Year).ToListAsync();

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
        
        private async Task CreateExchangeRate(string currency)//ALL EUR USD GBP
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync($"https://v6.exchangerate-api.com/v6/27fd1188b64da1c0700b3ab3/latest/{currency}");

            using var jsonDocument = JsonDocument.Parse(response);
            var root = jsonDocument.RootElement;

            //"Sat, 05 Oct 2024 00:00:01 +0000"
            string timeLastUpdateUtc = root.GetProperty("time_last_update_utc").GetString();

            string format = "ddd, dd MMM yyyy HH:mm:ss K";

            DateTime lastUpdateDate = DateTime.ParseExact(timeLastUpdateUtc, format, System.Globalization.CultureInfo.InvariantCulture);

            var conversionRates = jsonDocument.RootElement.GetProperty("conversion_rates");


            var eurRate = 0.0;
            var usdRate = 0.0;
            var gbpRate = 0.0;
            var lekRate = 0.0;


            switch (currency)
            {
                case "ALL":
                    eurRate = conversionRates.GetProperty("EUR").GetDouble();
                    usdRate = conversionRates.GetProperty("USD").GetDouble();
                    gbpRate = conversionRates.GetProperty("GBP").GetDouble();
                    lekRate = 1;
                    break;

                case "EUR":
                    eurRate = 1;
                    usdRate = conversionRates.GetProperty("USD").GetDouble();
                    gbpRate = conversionRates.GetProperty("GBP").GetDouble();
                    lekRate = conversionRates.GetProperty("ALL").GetDouble();
                    break;

                case "USD":
                    eurRate = conversionRates.GetProperty("EUR").GetDouble(); ;
                    usdRate = 1;
                    gbpRate = conversionRates.GetProperty("GBP").GetDouble();
                    lekRate = conversionRates.GetProperty("ALL").GetDouble();
                    break;

                case "GBP":
                    eurRate = conversionRates.GetProperty("EUR").GetDouble(); ;
                    usdRate = conversionRates.GetProperty("USD").GetDouble();
                    gbpRate = 1;
                    lekRate = conversionRates.GetProperty("ALL").GetDouble();
                    break;
            }



            var newRate = new ExchangeRate()
            {
                FetchDate = lastUpdateDate,
                Euro = eurRate,
                Dollar = usdRate,
                Pound = gbpRate,
                Lek = lekRate,
                Currency = currency
            };

            await _context.ExchangeRates.AddAsync(newRate);
            await _context.SaveChangesAsync();
        }
    
    }
}
