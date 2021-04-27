using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernSchool.DB;
using ModernSchool.Models;
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
        private DataContext db;

        public ApiListController(DataContext context)
        {
            db = context;
        }
        public IActionResult GetRegions()
        {
            var client = new RestClient("https://ustozfondi.uz/api/locations/getCities");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-TOKEN", "3f0f6bb3a9b784a98c62154e3259e015");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            JObject jObject = JObject.Parse(response.Content);
            var entities = jObject["cities"];
            List<RegionApiModel> regions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RegionApiModel>>(entities.ToString());
            foreach (var item in regions)
            {
                db.Regions.Add(new Region
                {
                    id = item.id,
                    name_uz = item.name,
                    name_ru = item.name_ru
                });
                db.SaveChanges();
            }
            return View();
        }
        public IActionResult GetDistricts()
        {
            var client = new RestClient("https://ustozfondi.uz/api/locations/getDistricts");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-TOKEN", "3f0f6bb3a9b784a98c62154e3259e015");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            JObject jObject = JObject.Parse(response.Content);
            var entities = jObject["districts"];
            List<DistrictApiModel> districts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DistrictApiModel>>(entities.ToString());
            foreach (var item in districts)
            {
                db.Districts.Add(new District
                {
                    id = item.district_id,
                    parent_id = item.city_id,
                    name_uz = item.district_name,
                    name_ru = item.district_name_ru
                });
                db.SaveChanges();
            }
            return View();
        }
        public IActionResult GetSchools()
        {
            var client = new RestClient("https://ustozfondi.uz/api/schools/getAll");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-API-TOKEN", "3f0f6bb3a9b784a98c62154e3259e015");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            JObject jObject = JObject.Parse(response.Content);
            var entities = jObject["schools"];
            List<SchoolApiModel> schools = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolApiModel>>(entities.ToString());
            foreach (var item in schools)
            {
                db.Schools.Add(new School
                {
                    Id = item.id,
                    DistrictId = item.district_id,
                    NameUz = item.name,
                    NameRu = item.name_ru
                });
                db.SaveChanges();
            }
            return View();
        }
        //public IActionResult UpdateSchools()
        //{
        //    List<School> schools = db.Schools.ToList();
        //    foreach (var item in schools)
        //    {
        //        var client = new RestClient("https://ustozfondi.uz/api/schools/getSchool/" + item.Id);
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("X-API-TOKEN", "3f0f6bb3a9b784a98c62154e3259e015");
        //        IRestResponse response = client.Execute(request);
        //        Console.WriteLine(response.Content);
        //        JObject jObject = JObject.Parse(response.Content);
        //        var entities = jObject["school"];
        //        SchoolApiModel school = Newtonsoft.Json.JsonConvert.DeserializeObject<SchoolApiModel>(entities.ToString());

        //        item.Phone = school.phone;
        //        item.Address = school.address;
        //        item.langs = school.langs;
        //        item.school_address_type = school.school_address_type;
        //        item.distanse_to_city = school.distanse_to_city;
        //        item.max_pupils = school.max_pupils;
        //        item.now_pupils = school.now_pupils;
        //        item.coefficient = school.coefficient;
        //        item.available_classes = school.available_classes;
        //        item.teachers_count = school.teachers_count;
        //        item.highest_category_teachers_count = school.highest_category_teachers_count;
        //        item.first_category_teachers_count = school.first_category_teachers_count;

        //        db.Entry(item).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    return View();
        //}
    }
}
