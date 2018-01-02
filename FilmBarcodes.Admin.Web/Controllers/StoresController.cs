using System.Web.Mvc;
using CafePress.Api;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace FilmBarcodes.Admin.Web.Controllers
{
    public class StoresController : Controller
    {
        public ActionResult Index()
        {
            SettingsWrapper settings = Settings.GetSettings();

            Stores stores = new StoreMethods(settings).ListStores();

            return View();
        }
    }
}