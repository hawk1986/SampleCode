using PRS_New.Attributes;
using PRS_New.Interface;
using PRS_New.ViewModel;
using PRS_New.ViewModel.ListResult;
using PRS_New.ViewModel.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;
//using static PRS_NewManager.MemberAccountManager;
using Utilities.Utility;

namespace PRS_New.Controllers.Api
{
    [RoutePrefix("api")]
    public class apiController : BaseApiController
    {
        //readonly IActCarouselManager _actCarouselManager;

        //public apiController(IActCarouselManager actCarouselManager)
        //{
        //    _actCarouselManager = actCarouselManager;
        //}

        //#region config
        //[HttpGet]
        //public HttpResponseMessage config()
        //{
        //    //初始化回傳物件
        //    var resp = new HttpResponseMessage();

        //    try
        //    {
        //        string URL = Tools.GetConfigValue("AppApiConfig", string.Empty);
        //        var result = GetObject<configReturn>(URL);

        //        resp.StatusCode = HttpStatusCode.OK;
        //        resp.Content = new ObjectContent<configReturn>(result, new JsonMediaTypeFormatter(), "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.StatusCode = HttpStatusCode.InternalServerError;
        //        resp.Content = new StringContent(ex.Message);
        //    }

        //    return resp;
        //}

        //public class configReturn
        //{
        //    public configReturn()
        //    {
        //        rcrm = new rcrm();
        //        results = new ConfigResults();
        //    }
        //    public rcrm rcrm { get; set; }
        //    public ConfigResults results { get; set; }
        //}
        //public class ConfigResults
        //{
        //    public ConfigResults()
        //    {
        //        config_info = new List<config_info>();
        //    }
        //    public List<config_info> config_info { get; set; }
        //}
        //public class config_info
        //{
        //    public string title { get; set; }
        //    public string key { get; set; }
        //    public string type { get; set; }
        //    public string value { get; set; }
        //    public string image { get; set; }
        //    public string sorting_rank { get; set; }
        //    public DateTime edit_datetime { get; set; }
        //}
        //#endregion




        #region call api
        private T GetObject<T>(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(url).GetAwaiter().GetResult();
                var jsonResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiData = JsonConvert.DeserializeObject<T>(jsonResult);
                return (T)apiData;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private T PostObject<T>(string data, string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                var jsonResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var apiData = JsonConvert.DeserializeObject<T>(jsonResult);
                return (T)apiData;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
