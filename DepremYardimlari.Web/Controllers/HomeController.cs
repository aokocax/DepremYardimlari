using DepremYardimlari.Web.Models;
using DepremYardimlari.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DepremYardimlari.Web.Controllers
{
    public class HomeController : Controller
    {
        const float dolarExc = 18.87f;
        const float euroExc = 20.11f;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string sektor="")
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var avmList=new List<AidViewModel>();
            var aidList=new List<Aid>();
            string path = "";
            path = Path.Combine(webRootPath, "aid_f1.json");
            var content=System.IO.File.ReadAllText(path);
       
            if (String.IsNullOrEmpty(sektor))
            {
                aidList = JsonConvert.DeserializeObject<List<Aid>>(content);
            }
                
            else
            {
                aidList= JsonConvert.DeserializeObject<List<Aid>>(content)
                    .ToList().Where(p => p.Sektor == sektor).ToList();
               
            }
            foreach (var item in aidList)
            {
                avmList.Add(new AidViewModel
                {
                    Marka = item.Marka,
                    Tutar = item.Tutar,
                    Birim = item.Birim,
                    Durum = item.Durum,
                    Kaynak = item.Kaynak,
                    Detay = item.Detay,
                    Sektor = item.Sektor,
                    Tur = item.Tur,
                    NetTutar = item.Birim == "TL" ? item.Tutar : item.Birim == "$" ? item.Tutar * dolarExc : item.Tutar * euroExc
                });
            }

            ViewBag.Data = avmList;
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
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Contact(string name, string company, string email, string message, string messageFrom)
        {
            // accountService.SaveContactData(name, company, email, message,Request.ServerVariables["remote_addr"],messageFrom);
            MailService.SendEmail("oguz@quikmotion.com", "AidTurkey Contact Request",
                "Name:" + name + "<br>" +
                 "Email:" + email + "<br>" +
                "Company:" + company + "<br>" +
                "Message:" + message + "<br>" +
                "Message from:" + messageFrom

                );
            return Json(new { success = true });
        }
    }
}