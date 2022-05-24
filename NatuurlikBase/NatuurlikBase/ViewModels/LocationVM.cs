using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;


namespace NatuurlikBase.ViewModels
{
    public class LocationVM
    {
        [DisplayName("Country")]
        public IList<SelectListItem> CountryNames { get; set; }
        public IList<SelectListItem> ProvinceNames { get; set; }
        public IList<SelectListItem> CityNames { get; set; }
        public IList<SelectListItem> SuburbNames { get; set; }

    }
}
