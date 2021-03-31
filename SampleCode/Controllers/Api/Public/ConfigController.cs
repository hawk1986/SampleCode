using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using SampleCode.Models.Public;
using Utilities.Utility;
//using static SampleCodeManager.MemberAccountManager;

namespace SampleCode.Controllers.Api.Public
{
    [RoutePrefix("api/config")]
    public class ConfigController : ApiController
    {
        //public HttpResponseMessage Get()
        //{
        //    //初始化回傳物件
        //    var resp = new HttpResponseMessage();
        //    resp.StatusCode = HttpStatusCode.OK;
        //    resp.Content = new ObjectContent<List<Config>>(Global.GetConfigs(), new JsonMediaTypeFormatter(), "application/json");
        //    return resp;
        //}

        //public HttpResponseMessage Get()
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

        //public HttpResponseMessage Post()
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
    }
}