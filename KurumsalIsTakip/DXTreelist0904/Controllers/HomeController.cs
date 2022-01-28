using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DXTreelist0904.Models;
using static DXTreelist0904.Filters.AuthFilter;
using DXTreelist0904.Filters;

namespace DXTreelist0904.Controllers
{
    
    public class HomeController : Controller
    {
        
        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Giris(Adminler model)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            Adminler admin = db.Adminler.FirstOrDefault(x => x.AdminKAdi == model.AdminKAdi & x.AdminSifre == model.AdminSifre);

           
            if (admin == null)
            {
                ModelState.AddModelError("", "Hatalý Kullanýcý Adý veya Þifre!");
                return View(model);
            }

            TempData["OturumKullanici"] = admin;
            ViewBag.OturumKullanici = admin;
            Session["Kullanici"] = admin.AdminAdSoyad;


            if (admin.Yetki == 1)
            {
                Session["OturumYetki"] = "1";
                string IsDurumu=null;
                string durum = "BekleyenIsler";
                return RedirectToAction("Index", "Admin", new { IsDurumu = durum });

            }
            else if (admin.Yetki == 0)
            {
                Session["OturumYetki"] = "0";
                return RedirectToAction("Index", "Kullanici");
            }
            else
            {
                return View(model);
            }

        }
        public ActionResult Cikis()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Giris", "Home");
        }
        public ActionResult Index(string IsDurumu,string TalepDurumu)
        {
            ViewBag.EnableCheckedListMode = true;
            List<AdminTalepler> TalepList = null;
            string kadi = Session["Kullanici"].ToString();

            if (string.IsNullOrWhiteSpace(IsDurumu))
                TalepList = db.AdminTalepler.ToList();
            else if (IsDurumu == "BitmisIsler")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Tamamlandý").ToList();
            else if (IsDurumu == "DevamEden")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Devam Ediyor").ToList();
            else if (IsDurumu == "BekleyenIsler")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Beklemede").ToList();
            else if(IsDurumu == "Yanitlananlar")
            {
                if (Session["OturumYetki"] == "1" )
                    TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" && x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
                if (Session["OturumYetki"] == "0")
                    TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Admin" && x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
            }



            return View("Index", TalepList);
        }
        [HttpPost]
        public ActionResult Index(bool enableCheckedListMode = true)
        {
            ViewBag.EnableCheckedListMode = enableCheckedListMode;
            return View("Index", db.AdminTalepler.ToList());
        }
        public ActionResult TreeListPartial(bool enableCheckedListMode = true)
        {
            ViewBag.EnableCheckedListMode = enableCheckedListMode;
            return PartialView("_TreeListPartial", db.AdminTalepler.ToList());
        }


        KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
    }
}