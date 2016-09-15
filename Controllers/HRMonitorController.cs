using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DreamforceIOTCloudApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DreamforceIOTCloudApp.Controllers
{
    public class HRMonitorController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> SendData(int goalHeartRateThreshold, int goalDuration)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HeartRateMonitor hrMonitor = new HeartRateMonitor();
                    hrMonitor.goalHeartRateThreshold = goalHeartRateThreshold;
                    hrMonitor.goalDuration = goalDuration;

                    httpClient.BaseAddress = new Uri("https://ingestion-xcdvudaz0dz3.us3.sfdcnow.com/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer iguZgpnIwr8N60cD7cYgSMbYQm9QZXEY9AaqmeD6f4d2DoyvZNEhcQdzGSSFivDylWcXR5ShTu1AfMSCJi9sAj");
                    HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(hrMonitor), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("streams/heart_rate_monito001/heart_rate_monito002/event", contentPost);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return Json(result);
                    }

                    return Json(null);
                }
            }
            catch(Exception ex)
            {
                return Json(null);
            }
        }
    }
}
