using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SalesAplication.Controllers
{
    public static partial class ExtHelpers
    {

        private static readonly JsonSerializerSettings jsSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static MvcHtmlString ToJson(this Object obj)
        {
            var json = JsonConvert.SerializeObject(obj, jsSettings);
            return MvcHtmlString.Create(json);
        }
    }
}
