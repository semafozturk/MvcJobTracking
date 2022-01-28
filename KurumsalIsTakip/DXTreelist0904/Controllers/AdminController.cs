using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DXTreelist0904.Models;
using System.IO;
using DXTreelist0904.Filters;

namespace DXTreelist0904.Controllers
{
    [AuthFilter]
    public class AdminController : Controller
    {
        
        // GET: Admin
        KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
        public ActionResult Index(string IsDurumu)
        {
            List<AdminTalepler> TalepList = null;
            string kadi = Session["Kullanici"].ToString();
            if (Session["IsDurumu"] == null)
                Session["IsDurumu"] = "Baslamis";
            Session["IsDurumu"] = IsDurumu;

            if (string.IsNullOrWhiteSpace(IsDurumu))
            {
                TalepList = db.AdminTalepler.ToList();
                ViewBag.durum = "Tüm Talepler";
            }
            if (IsDurumu == "Baslamis")
            {
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Başlamış").ToList();
                ViewBag.durum = "Başlanmış Talepler";
            }
            else if (IsDurumu == "BitmisIsler")
            {
                DateTime sonUcAy = DateTime.Now.AddDays(-90);
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Tamamlandı" & t.TalepTarihi > sonUcAy).ToList();
                ViewBag.durum = "Bitmiş Talepler";
            }
            else if (IsDurumu == "İptalEdilen")
            {
                DateTime sonUcAy = DateTime.Now.AddDays(-90);
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "İptal Edildi" & t.TalepTarihi > sonUcAy).ToList();
                ViewBag.durum = "İptal Edilen Talepler";
            }
            else if (IsDurumu == "BekleyenIsler")
            {
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Beklemede").ToList();
                ViewBag.durum = "Bekleyen Talepler";
            }
            else if (IsDurumu == "Yanitlananlar" && Session["OturumYetki"] == "1")
            {
                TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" & x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
                ViewBag.durum = "Yanıtlanan Talepler";
            }
            
            List<AdminTalepler> yanitlar = db.AdminTalepler.Where(x=>x.TalepDurumu == "scsd").ToList();
            foreach (var tlp in TalepList)
            {
                var talebim = db.AdminTalepler.Where(x => x.TalepId == tlp.TalepId && x.TalepDurumu == "Yanıt").ToList();
                if (talebim.Count != 0)
                {
                    foreach (var t in talebim)
                    {
                        yanitlar.Add(t);
                    }
                }
            }
            
            var liste = TalepList.Union(yanitlar);
            return View("Index", liste);

        }
       
        [HttpPost]
        public ActionResult Index()
        {
            string IsDurumu = Session["IsDurumu"].ToString();
            string kadi = Session["Kullanici"].ToString();
            List<AdminTalepler> TalepList = null;
            if (string.IsNullOrWhiteSpace(IsDurumu))
                TalepList = db.AdminTalepler.ToList();
            else if (IsDurumu == "Baslamis")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Başlamış").ToList();
            else if (IsDurumu == "BitmisIsler")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Tamamlandı").ToList();
            else if (IsDurumu == "İptalEdilen")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "İptal Edildi").ToList();
            else if (IsDurumu == "BekleyenIsler")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Beklemede").ToList();
            else if (IsDurumu == "Yanitlananlar" && Session["OturumYetki"] == "1")
                TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" & x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
            List<AdminTalepler> yanitlar = db.AdminTalepler.Where(x => x.TalepDurumu == "scsd").ToList();
            foreach (var tlp in TalepList)
            {
                var talebim = db.AdminTalepler.Where(x => x.yanitId == tlp.TalepId && x.TalepDurumu == "Yanıt").ToList();
                if (talebim.Count != 0)
                {
                    foreach (var t in talebim)
                    {
                        yanitlar.Add(t);
                    }
                }
            }

            var liste = TalepList.Union(yanitlar);
            return View("Index", liste);
        }
        public ActionResult TreeListPartial()
        {
            if (Session["IsDurumu"] == null)
                Session["IsDurumu"] = "Tum";
            var IsDurumu = Session["IsDurumu"].ToString();
            string kadi = Session["Kullanici"].ToString();
            List<AdminTalepler> TalepList = null;
            if (string.IsNullOrWhiteSpace(IsDurumu))
                TalepList = db.AdminTalepler.ToList();
            else if (IsDurumu == "Tum")
                TalepList = db.AdminTalepler.ToList();
            else if (IsDurumu == "BitmisIsler")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Tamamlandı").ToList();
            else if (IsDurumu == "İptalEdilen")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "İptal Edildi").ToList();
            else if (IsDurumu == "Baslamis")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Başlamış").ToList();
            else if (IsDurumu == "BekleyenIsler")
                TalepList = db.AdminTalepler.Where(t => t.IsDurumu == "Beklemede").ToList();
            else if (IsDurumu == "Yanitlananlar" && Session["OturumYetki"] == "1")
                TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" & x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
            List<AdminTalepler> yanitlar = db.AdminTalepler.Where(x => x.TalepDurumu == "scsd").ToList();
            foreach (var tlp in TalepList)
            {
                var talebim = db.AdminTalepler.Where(x => x.yanitId == tlp.TalepId && x.TalepDurumu == "Yanıt").ToList();
                if (talebim.Count != 0)
                {
                    foreach (var t in talebim)
                    {
                        yanitlar.Add(t);
                    }
                }
            }

            var liste = TalepList.Union(yanitlar);
            return View("_TreeListPartial", liste);
        }
        public ActionResult Adminler()
        {
            var model = db.Adminler.ToList();
            return View(model);
        }
     
        
        public ActionResult YeniYanit(int? talepID)
        {
            AdminTalepler talep = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);
            return View(talep);
        }
        [HttpPost]
        public ActionResult YeniYanit(AdminTalepler model, string DetayTanim,double? hSure)
        {
            string kadi = Session["kullanici"].ToString();
            Adminler admin = db.Adminler.FirstOrDefault(x => x.AdminAdSoyad == kadi);

            var talep = db.AdminTalepler.FirstOrDefault(t => t.TalepId == model.TalepId);
            if (Session["OturumYetki"].ToString() == "1")
            {
            talep.IsYuzde = model.IsYuzde;
            talep.IsDurumu = model.IsDurumu;
            talep.TerminGunu = model.TerminGunu;
                if (hSure == null)
                {
                    hSure = 0;
                }
            talep.HarcananSure = hSure;
            }
            
            if (model.IsDurumu == "Başlamış" && model.IseBaslananTarih == null)
            {
                talep.IseBaslananTarih = DateTime.Now;
            }
             if ((model.TerminGunu != null || model.TahminiBitisTarihi != null) && model.IseBaslananTarih != null)
            {
                DateTime baslangic = talep.IseBaslananTarih.Value;
                talep.TahminiBitisTarihi = baslangic.AddDays((double)model.TerminGunu);

            }

            var talepYanit = new AdminTalepler();
           
            if (DetayTanim != "")
            {

            talepYanit.yanitId = talep.TalepId;
            talepYanit.TalepTarihi = DateTime.Now;
            talepYanit.TalepDurumu = "Yanıt";
            talepYanit.adminId = admin.AdminId;
            talepYanit.IsTanim = DetayTanim;
            db.AdminTalepler.Add(talepYanit);
                if (Session["OturumYetki"].ToString() == "1")
                    talep.TalepDurumu = "Kullanici";
                else if (Session["OturumYetki"].ToString() == "0")
                    talep.TalepDurumu = "Admin";
            }


            int sonuc = db.SaveChanges();

            if (sonuc > 0)
            {
                ViewBag.Result = "Bilgiler Kaydedildi.";
                ViewBag.Status = "success";
            }
            else
            {
                ViewBag.Result = "Bilgiler Kaydedilemedi.";
                ViewBag.Status = "danger";
            }

            return RedirectToAction("TalepGoster", "Admin", new { talepID = model.TalepId });//TalepGoster sayfasına talepidyi gönderiyoruz. mikemmel
        }

     
        public ActionResult TalepGoster(Dosyalar dosya,int? talepID)
        {
            AdminTalepler talep = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);
            dosya = db.Dosyalar.FirstOrDefault(x => x.talepId == talepID);
            

            if (talep.TalepDurumu == "Yanıt")
            {
               /* int? yanitId = talep.yanitId;
                AdminTalepler yanitliTalep = db.AdminTalepler.FirstOrDefault(x => x.TalepId = yanitId);*/
                return RedirectToAction("TalepGoster", "Admin",new { talepID = talep.yanitId });
            }
            else
            {
            return View(talep);
            }
            
        }

        [HttpPost]
        public ActionResult TalepGoster(int? talepID, AdminTalepler model)
        {

            string kullaniciAdi = Session["Kullanici"].ToString();

            Adminler admin =
            db.Adminler
            .FirstOrDefault(x => x.AdminAdSoyad == kullaniciAdi);



            Kategoriler kategoriler = db.Kategoriler.FirstOrDefault(x => x.kategoriId == model.Kategoriler.kategoriId);
            Aciliyetler aciliyetler = db.Aciliyetler.FirstOrDefault(x => x.AcilId == model.Aciliyetler.AcilId);

            //Adminler admin = db.Adminler.Where(x => x.AdminId == model.AdminId).FirstOrDefault();
            AdminTalepler talep = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);
            if (admin != null)
            {
                talep.Adminler = admin;
                talep.TalepId = model.TalepId;
                talep.TalepTarihi = model.TalepTarihi;
                talep.IsTanim = model.IsTanim;
                talep.Aciliyetler = aciliyetler;
                talep.Kategoriler = kategoriler;
                talep.IsYuzde = model.IsYuzde;
                //talep.Baslik = model.Baslik;
                //talep.IsDurumu = model.IsDurumu;
                //talep.IseBaslananTarih = model.IseBaslananTarih;
                //talep.TahminiBitisTarihi = model.TahminiBitisTarihi;
                //talep.TerminGunu = model.TerminGunu;
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Giriş Bilgileri Güncellendi.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Giriş Bilgileri Güncellenemedi.";
                    ViewBag.Status = "danger";
                }
            }
                return RedirectToAction("TalepGoster", "Admin", new { talepID = talep.TalepId });
            }
        public ActionResult TalepSil(int? talepID)
        {
            AdminTalepler talep = null;
            if (talepID != null)
            {
                talep = db.AdminTalepler.Where(x => x.TalepId == talepID).FirstOrDefault();
            }

            return View(talep);
        }
        [HttpPost, ActionName("TalepSil")]
        public ActionResult TalepSilOk(int? talepID, AdminTalepler model)
        {
            if (talepID != null)
            {
                model = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);
                var bagliDosyalar = db.Dosyalar.Where(x => x.AdminTalepler.TalepId == talepID).ToList();
                var bagliYanitlar = db.AdminTalepler.Where(x => x.yanitId == talepID).ToList();
                db.AdminTalepler.Remove(model);
                foreach (var dosya in bagliDosyalar)
                {
                    db.Dosyalar.Remove(dosya);
                }
                foreach (var yanit in bagliYanitlar)
                {
                    db.AdminTalepler.Remove(yanit);
                }
                db.SaveChanges();
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResimGoster(int resimID)
        {
            var resim = db.Dosyalar.FirstOrDefault(x => x.DosyaId == resimID);
            return View(resim);
        }
        public ActionResult Aciliyetler()
        {
            var aciliyet = db.Aciliyetler.ToList();
            return View(aciliyet);
        }
        public ActionResult Kategoriler()
        {
            var kategoriler = db.Kategoriler.ToList();
            return View(kategoriler);
        }
        
    }
}