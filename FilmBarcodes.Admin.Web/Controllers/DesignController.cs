using System.Linq;
using System.Web.Mvc;
using CafePress.Api;
using FilmBarcodes.Admin.Web.Models;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace FilmBarcodes.Admin.Web.Controllers
{
    public class DesignController : Controller
    {
        private SettingsWrapper _settings;
        private DesignMethods _designMethods;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _settings = Settings.GetSettings();
            _designMethods = new DesignMethods(_settings);
        }

        // GET: Design
        public ActionResult Index()
        {
            return View();
        }

        // GET: Design/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Design/Create
        public ActionResult Create()
        {
            Folders folders = _designMethods.ListFolders();

            var folderSelectItems = folders.FolderList.Select(f => new SelectListItem {Text = f.Name, Value = f.Id}).ToList();

            folderSelectItems.Insert(0, new SelectListItem {Text = "Please select", Value = "0", Selected = true});

            DesignViewModel model = new DesignViewModel {Folders = folderSelectItems};

            return View(model);
        }

        // POST: Design/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Design/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Design/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Design/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Design/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}