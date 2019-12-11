using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Mail(string email)
        {
            var callbackUrl = Url.Link("Default", new { Controller = "Home", Action = "Index"});
            var htmlMessage = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"//www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
                "<html lang=\"es\" xmlns=\"//www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head>" +
                "    <title></title>" +
                "    <meta charset=\"utf-8\" /> <!-- utf-8 works for most cases -->" +
                "    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" +
                "    <meta name=\"viewport\" content=\"width=device-width\" /> <!-- Forcing initial-scale shouldn't be necessary -->" +
                "    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> <!-- Use the latest (edge) version of IE rendering engine -->" +
                "    <meta name=\"x-apple-disable-message-reformatting\" />  <!-- Disable auto-scale in iOS 10 Mail entirely -->" +
                $"</head><body><h1>Titulo</h1><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Ver los detalles</a></body></html>";

            await _emailSender.SendEmailAsync(email, "Subjet", htmlMessage);

            return Content("Enviado");
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
    }
}
