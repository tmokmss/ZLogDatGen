using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZLOGdat.Models;
using ZLOGdat.Components;

namespace ZLOGdat.Controllers
{
    public class DatGenController : Controller
    {
        private Prefectures prefectures = new Prefectures();

        [HttpGet]
        public ActionResult Index()
        {
            return View(prefectures);
        }

        [ActionName("SelectAll")]
        [HttpPost]
        public ActionResult SelectAll(Prefectures model)
        {
            if (Request.IsAjaxRequest())
            {
            }
            var newModel = new Prefectures();
            newModel.CopyFrom(model);
            return PartialView("Prefs", newModel);
        }

        public ActionResult BeginGenerate(Prefectures model)
        {
            var reflectedModel = new Prefectures();
            reflectedModel.CopyFrom(model);
            return Generate(reflectedModel.GetID());
        }

        public ActionResult Generate(ulong id = 0)
        {
            var model = new Prefectures();
            model.SetID(id);
            var dat = new DatGenerator(model);
            var str = dat.GenerateDat();
            var resultModel = new Dat()
            {
                ID = model.GetID(),
                dat = str
            };
            var filePath = Server.MapPath(string.Format("~/{0}.dat", resultModel.ID));
            using (var fout = new StreamWriter(filePath, false))
            {
                fout.WriteLine(str);
            }

            return View("Result", resultModel);
        }

        public ActionResult Download(string id)
        {
            var filePath = Server.MapPath(string.Format("~/{0}.dat", id));
            return File(filePath, "text/plain", "acag.dat");
        }
    }
}