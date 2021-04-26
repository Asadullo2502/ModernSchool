using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Controllers
{
    public class ApiListController : Controller
    {
        public IActionResult Index()
        {
            var client = new RestClient("https://ustozfondi.uz/api/locations/getCities");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-TOKEN", "3f0f6bb3a9b784a98c62154e3259e015");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            JObject jObject = JObject.Parse(response.Content);
            var entities = jObject["cities"];
            return View();
        }
    }
}
