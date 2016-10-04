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
using System.Threading;
using System.Net;
using System.IO;
using System.Dynamic;

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
        public async Task<JsonResult> SendData(int heartRate)
        {
            try
            {
                HeartRateMessage hmMessage = null;
                using (var httpClient1 = new HttpClient())
                {
                    HeartRateMonitor hrMonitor = new HeartRateMonitor();
                    hrMonitor.deviceID = ConfigVars.Instance.DeviceID;
                    hrMonitor.heartRate = heartRate;
                    httpClient1.DefaultRequestHeaders.Accept.Clear();
                    httpClient1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient1.DefaultRequestHeaders.Add("Authorization", ConfigVars.Instance.DeviceToken);
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(hrMonitor), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(ConfigVars.Instance.EnpointUrl)
                    };
                    var response = await httpClient1.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        using (var httpClient2 = new HttpClient())
                        {
                            dynamic jsonData = new ExpandoObject();
                            jsonData.n = 1;
                            jsonData.timeout = 60000;
                            jsonData.wait = 0;
                            jsonData.delete = false;
                            request = new HttpRequestMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json"),
                                Method = HttpMethod.Post,
                                RequestUri = new Uri(string.Format("{0}/reservations?oauth={1}", ConfigVars.Instance.IronMQUrl, ConfigVars.Instance.IronMQToken))
                            };

                            response = await httpClient2.SendAsync(request);
                            if (response.IsSuccessStatusCode)
                            {
                                IList<HeartRateMessage> messagesLst = null;
                                var ironResponse = await response.Content.ReadAsStringAsync();
                                var ironMessage = JsonConvert.DeserializeObject<IDictionary<string, object>>(ironResponse);
                                if (ironMessage.Count > 0 && ironMessage.ContainsKey("messages"))
                                    messagesLst = JsonConvert.DeserializeObject<IList<HeartRateMessage>>(ironMessage["messages"].ToString());
                                if (messagesLst != null && messagesLst.Where(p => !string.IsNullOrEmpty(p.body) && p.body.Contains(ConfigVars.Instance.DeviceID)).Count() > 0)
                                    hmMessage = messagesLst.Where(p => p.body.Contains(ConfigVars.Instance.DeviceID)).LastOrDefault();

                                if (hmMessage != null)
                                {
                                    Parallel.Invoke(() =>
                                    {
                                        //delete iron queue message after reading
                                        DeleteIronMessageByID(hmMessage.id, hmMessage.reservation_id);
                                    });
                                }
                            }
                        }
                    }
                }

                if (hmMessage == null)
                    return Json(null);
                else
                    return Json(hmMessage.body);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        private async void DeleteIronMessageByID(string pMessageID, string pReservationID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    dynamic jsonData = new ExpandoObject();
                    jsonData.reservation_id = pReservationID;
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Format("{0}/messages/{1}?oauth={2}", ConfigVars.Instance.IronMQUrl, pMessageID, ConfigVars.Instance.IronMQToken))
                    };
                    var response = await httpClient.SendAsync(request);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
