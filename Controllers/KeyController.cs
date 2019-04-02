using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BotExtension.Helper;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BotExtension.Controllers
{
    [Route("api/key")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        [HttpPost]
        [Route("microsoft")]
        public async Task<HttpResponseMessage> CheckMicrosoftKeys([FromBody]MSKEY keys)
        {
            string decodedAppId = HttpUtility.HtmlDecode(keys.appId);
            string decodedPassword = HttpUtility.HtmlDecode(keys.password);
            string token = await GetToken(decodedAppId, decodedPassword);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        [Route("luis")]
        public async Task<JsonResult> CheckLuisKeys([FromBody]LUISKEY keys)
        {
            string decodedAppId = HttpUtility.HtmlDecode(keys.LuisAppId);
            string decodedPassword = HttpUtility.HtmlDecode(keys.LuisKey); 
            string decodedHostname = HttpUtility.HtmlDecode(keys.Hostname);
            string decodedQuery = HttpUtility.HtmlDecode(keys.Query);

            string response = await TestLuis(decodedAppId, decodedPassword, decodedHostname, decodedQuery);
            if (string.IsNullOrEmpty(response))
                return new JsonResult(System.Net.HttpStatusCode.InternalServerError);

            return new JsonResult(response);
        }

        [Route("qna")]
        public async Task<JsonResult> CheckQnaKeys([FromBody]QNAKEY keys)
        {
            string decodedKbId = HttpUtility.HtmlDecode(keys.KbId);
            string decodedPassword = HttpUtility.HtmlDecode(keys.Key);
            string decodedHostname = HttpUtility.HtmlDecode(keys.Hostname);
            string decodedQuery = HttpUtility.HtmlDecode(keys.Query);

            string response = await TestQna(decodedKbId, decodedPassword, decodedHostname, decodedQuery);
            if (string.IsNullOrEmpty(response))
                return new JsonResult(System.Net.HttpStatusCode.InternalServerError);

            return new JsonResult(response);
        }


        private async Task<string> GetToken(string appId, string password)
        {
            string tokenUrl = "https://login.microsoftonline.com/botframework.com/oauth2/v2.0/token";
            Dictionary<string,string> dict = new Dictionary<string, string>();
            dict.Add("grant_type", "client_credentials");
            dict.Add("client_id", appId);
            dict.Add("client_secret", password);
            dict.Add("scope", appId+"/.default");
            var client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, tokenUrl) { Content = new FormUrlEncodedContent(dict) };
            HttpResponseMessage response = await client.SendAsync(request);
            string token = await response.Content.ReadAsStringAsync();

            // Get the token value from the response
            var parser = JObject.Parse(token);
            string tokenValue = parser["access_token"].ToObject<string>();
            return tokenValue;
        }

        private async Task<string> TestLuis(string appId, string password, string hostname, string query)
        {
            string Url = "https://" + hostname + "/luis/v2.0/apps/"+ appId + "?verbose=true&subscription-key="+ password + "&q="+ query;
            var client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url);
            HttpResponseMessage response = await client.SendAsync(request);
            string value = await response.Content.ReadAsStringAsync();
            return value;
        }

        private async Task<string> TestQna(string kbId, string password, string hostname, string query)
        {
            string Url = "https://" + hostname + "/knowledgebases/" + kbId + "/generateAnswer";

            var client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
            request.Headers.Add("Authorization", "EndpointKey " + password);
            request.Content = new StringContent("{\"question\": \""+query+"\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            string value = await response.Content.ReadAsStringAsync();
            return value;
        }

    }
}