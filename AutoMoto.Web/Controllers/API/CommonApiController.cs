using AutoMoto.Contracts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AutoMoto.Web.Controllers.API
{
    public class CommonApiController : ApiController
    {
        private readonly IFeatureService _featureService;
        private IEnumerable<SelectItem> _makes = new List<SelectItem>
        {
            new SelectItem { id = 1, text = "Ford" },
            new SelectItem { id = 2, text = "Chevy" },
            new SelectItem { id = 3, text = "Chrysler" },
            new SelectItem { id = 4, text = "Honda" },
            new SelectItem { id = 5, text = "Toyota" }
        };

        public CommonApiController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        //[HttpGet]
        //public IEnumerable<SelectItem> SearchFeature(int id)
        //{
        //    var query = _featureService.Queryable().Where(m => m..text.ToLower().Contains(id.ToLower()));

        //    return query;
        //}

        [HttpGet]
        public IEnumerable<SelectItem> GetMake(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    items.Add(_makes.FirstOrDefault(m => m.id == idInt));
                }
            }

            return items;
        }
    }

    public class SelectItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
