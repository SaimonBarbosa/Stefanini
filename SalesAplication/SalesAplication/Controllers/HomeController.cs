using Microsoft.AspNet.Identity.Owin;
using SalesAplication.Models;
using System.Web.Mvc;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System;

namespace SalesAplication.Controllers
{
    public class HomeController : Controller
    {

        bool isadmin = false;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerList(bool? admin)
        {
            if (admin != null)
                ViewBag.Admin = admin;

            CustomerViewModel filters = new CustomerViewModel();

            var Model = GetCustomerList(filters);

            return View("CustomerList", Model);
        }


        public ActionResult CustomerListFilters(string Name, string Gender, int? City, int? Region, int? Classification,
                 int? Selle, DateTime? LastPurchase, DateTime? LastPurchaseEnd, bool? admin)
        {
            if (admin != null)
                ViewBag.Admin = true;

            CustomerViewModel filters = new CustomerViewModel();
            filters.City = City;
            filters.Classification = Classification;
            filters.Gender = Gender;
            filters.LastPurchase = LastPurchase;
            filters.LastPurchaseEnd = LastPurchaseEnd;
            filters.Name = Name;
            filters.Region = Region;
            filters.Selle = Selle;

            var Model = GetCustomerList(filters);

            return View("CustomerList", Model);
        }

        public JsonResult LoadCity()
        {
            var jsonreturn = GetCity();
            return Json(jsonreturn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadRegion()
        {
            var jsonreturn = GetRegion();
            return Json(jsonreturn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSeller()
        {
            var jsonreturn = GetSeller();
            return Json(jsonreturn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadClassification()
        {
            var jsonreturn = GetClassification();
            return Json(jsonreturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login)
        {
            ViewBag.Error = null;
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            if (UserLogin(login))
            {
                return CustomerList(isadmin);
            }
            else {
                ViewBag.Error = "The email and/ or password entered is invalid.Please try again.";
                return View("Index");
            }
        }


        private bool UserLogin(LoginViewModel login)
        {
            DatabaseEntities context = new DatabaseEntities();

            var code = GetMd5(login.Password);
            var user = context.Query<User>().Where(x => x.Email.Contains(login.Email) && x.Password == code).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            else
            {
                if (user.Name == "Admin")
                    isadmin = true;

                return true;
            }

        }


        private List<City> GetCity()
        {
            DatabaseEntities context = new DatabaseEntities();
            var Cities = context.Query<City>().ToList();

            return Cities;
        }

        private List<Region> GetRegion()
        {
            DatabaseEntities context = new DatabaseEntities();
            var Region = context.Query<Region>().ToList();

            return Region;
        }
        private List<User> GetSeller()
        {
            DatabaseEntities context = new DatabaseEntities();
            var seller = context.Query<User>().ToList();

            return seller;
        }

        private List<Classification> GetClassification()
        {
            DatabaseEntities context = new DatabaseEntities();
            var Classification = context.Query<Classification>().ToList();

            return Classification;
        }

        private List<CustomerListView> GetCustomerList(CustomerViewModel filters)
        {
            DatabaseEntities context = new DatabaseEntities();
            var Classification = (from c in context.Query<Client>()
                                  join r in context.Query<Region>()
                                  on c.RegionId equals r.RegionId
                                  join ct in context.Query<City>()
                                  on r.CityId equals ct.CityId
                                  join cl in context.Query<Classification>()
                                  on c.ClassificationId equals cl.ClassificationId
                                  join u in context.Query<User>()
                                  on c.SellerId equals u.UserId

                                  select new
                                  {
                                      idu = c.SellerId,
                                      idr = c.RegionId,
                                      idc = ct.CityId,
                                      idcl = c.ClassificationId,
                                      dt = c.LastPurchase,
                                      Name = c.Name,
                                      Phone = c.Phone,
                                      Gender = c.Gender,
                                      City = ct.CityName,
                                      Classification = cl.ClassificationName,
                                      LastPurchase = c.LastPurchase,
                                      Region = r.RegionName,
                                      Selle = u.Name
                                  });

            if (!string.IsNullOrEmpty(filters.Name))
                Classification = Classification.Where(x => x.Name.Contains(filters.Name));
            if (!string.IsNullOrEmpty(filters.Gender))
                Classification = Classification.Where(x => x.Gender.Contains(filters.Gender));
            if(filters.LastPurchase != null)
                Classification = Classification.Where(x => x.LastPurchase >= filters.LastPurchase);
            if (filters.LastPurchaseEnd != null)
                Classification = Classification.Where(x => x.LastPurchase <= filters.LastPurchaseEnd);
            if(filters.City != null)
                Classification = Classification.Where(x => x.idc == filters.City);
            if (filters.Classification != null)
                Classification = Classification.Where(x => x.idcl == filters.Classification);
            if (filters.Region != null)
                Classification = Classification.Where(x => x.idr == filters.Region);
            if (filters.Selle != null)
                Classification = Classification.Where(x => x.idu == filters.Selle);

            var CustomerList = Classification.Select(x => new CustomerListView
            {
                Name = x.Name,
                Phone = x.Phone,
                Gender = x.Gender,
                City = x.City,
                Classification = x.Classification,
                LastPurchase = x.LastPurchase,
                Region = x.Region,
                Selle = x.Selle
            }).ToList();

            return CustomerList;
        }

        private static string GetMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}