using System.Collections.Generic;
using System.Web.Mvc;
using FilmBarcodes.Common.Models.CafePress;

namespace FilmBarcodes.Admin.Web.Models
{
    public class DesignViewModel
    {
        public Design Design { get; set; }
        public IEnumerable<SelectListItem> Folders { get; set; }
    }
}