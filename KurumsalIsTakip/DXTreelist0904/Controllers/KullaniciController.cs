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
    public class KullaniciController : Controller
    {
        
        // GET: Kullanici
        public ActionResult Index()
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            var model = db.AdminTalepler.ToList();
            return View(model);
        }
        public ActionResult Taleplerim(string IsDurumu)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<AdminTalepler> TalepList = null;
            string kadi = Session["Kullanici"].ToString();
            if (Session["IsDurumu"] == null) Session["IsDurumu"] = "Acik";
            Session["IsDurumu"] = IsDurumu;
            if (string.IsNullOrWhiteSpace(IsDurumu))
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = "Tüm Talepler";
            }
            else if (IsDurumu == "Acik")
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu != "Tamamlandı").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = "Açık Taleplerim";
            }
            else if (IsDurumu == "İptal")
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu == "İptal Edildi").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = "İptal Edilen Taleplerim";
            }
            else if (IsDurumu == "Tamamlananlar")
            {
                TalepList = db.AdminTalepler.Where(x => x.IsDurumu == "Tamamlandı" & x.Adminler.AdminAdSoyad == kadi).OrderBy(x => x.IsDurumu).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = "Tamamlanan Taleplerim";
            }
            else if (IsDurumu == "Yanitlilar" & Session["OturumYetki"] == "0")
            {
                TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" && x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
                ViewBag.durum = "Yanıtlı Taleplerim";
            }
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
            return View("Taleplerim", liste);
        }
        [HttpPost]
        public ActionResult Taleplerim()
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<AdminTalepler> TalepList = null;
            string kadi = Session["Kullanici"].ToString();
            if (Session["IsDurumu"] == null) Session["IsDurumu"] = "Acik";
            string IsDurumu = Session["IsDurumu"].ToString();
            if (string.IsNullOrWhiteSpace(IsDurumu))
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu != "Tamamlandı").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "Acik")
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu != "Tamamlandı").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "İptal")
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu == "İptal Edildi").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "Tamamlananlar")
            {
                TalepList = db.AdminTalepler.Where(x => x.IsDurumu == "Tamamlandı" & x.Adminler.AdminAdSoyad == kadi).OrderBy(x => x.IsDurumu).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "Yanitlilar" & Session["OturumYetki"] == "0")
            {
                TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" && x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }

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
            return View("Taleplerim", liste);
        }

        public ActionResult KullaniciPartial()
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<AdminTalepler> TalepList = null;
            if (Session["IsDurumu"] == null)  Session["IsDurumu"] = "Acik"; 
            var IsDurumu = Session["IsDurumu"].ToString(); 
            string kadi = Session["Kullanici"].ToString();
            if (string.IsNullOrWhiteSpace(IsDurumu))
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu != "Tamamlandı").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "Acik")
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu != "Tamamlandı").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "İptal")
            {
                TalepList = db.AdminTalepler.Where(t => t.Adminler.AdminAdSoyad == kadi & t.TalepDurumu != "Yanıt" & t.IsDurumu == "İptal Edildi").OrderBy(x => x.kategoriId).ThenByDescending(x => x.acilId).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "Tamamlananlar")
            {
                TalepList = db.AdminTalepler.Where(x => x.IsDurumu == "Tamamlandı" & x.Adminler.AdminAdSoyad == kadi).OrderBy(x => x.IsDurumu).ThenByDescending(x => x.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }
            else if (IsDurumu == "Yanitlilar" & Session["OturumYetki"] == "0")
            {
                TalepList = db.AdminTalepler.Where(x => x.TalepDurumu == "Kullanici" && x.Adminler.AdminAdSoyad == kadi).OrderByDescending(s => s.TalepTarihi).ToList();
                ViewBag.durum = IsDurumu;
            }

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
            return View("_KullaniciPartial", liste);
        }
        public ActionResult TalepGoster(AdminTalepler talep, Dosyalar dosya, int? talepID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            talep = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);
            dosya = db.Dosyalar.FirstOrDefault(x => x.talepId == talepID);
            if (talep.TalepDurumu == "Yanıt")
            {
                /* int? yanitId = talep.yanitId;
                 AdminTalepler yanitliTalep = db.AdminTalepler.FirstOrDefault(x => x.TalepId = yanitId);*/
                return RedirectToAction("TalepGoster", "Admin", new { talepID = talep.yanitId });
            }
            else
            {
                return View(talep);
            }

            
        }
        public ActionResult Yeni()
        {
            //kategori listesi oluşturma
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<SelectListItem> kategoriList =
                (from Kategoriler in db.Kategoriler.ToList()
                 select new SelectListItem()
                 {
                     Text = Kategoriler.kategoriAdi,
                     Value = Kategoriler.kategoriId.ToString()
                 }).ToList();

            TempData["kategoriler"] = kategoriList;
            ViewBag.kategoriler = kategoriList;
           

            //aciliyet listesi oluşturma
            List<SelectListItem> aciliyetList =
                (from aciliyet in db.Aciliyetler.ToList()
                 select new SelectListItem()
                 {
                     Text = aciliyet.AciliyetTanim,
                     Value = aciliyet.AcilId.ToString()
                 }
                 ).ToList();


            TempData["aciliyetler"] = aciliyetList;
            ViewBag.aciliyetler = aciliyetList;

            return View();

        }
        [HttpPost]
        public ActionResult Yeni(AdminTalepler talep,HttpPostedFileBase[] Dosya1)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            

            //ModelState.Clear();
            //Adminler admin = db.Adminler.Where(x => x.AdminId == talep.AdminId).FirstOrDefault();
            string kullaniciAdi = Session["Kullanici"].ToString();

            Adminler admin =
            db.Adminler
            .FirstOrDefault(x => x.AdminAdSoyad == kullaniciAdi);

            Kategoriler kategoriler =
           db.Kategoriler
           .FirstOrDefault(x => x.kategoriId == talep.Kategoriler.kategoriId);

            Aciliyetler aciliyetler =
           db.Aciliyetler
           .FirstOrDefault(x => x.AcilId == talep.Aciliyetler.AcilId);


            if (admin != null & Dosya1 != null)
            {
                talep.Adminler = admin;
                talep.Aciliyetler = aciliyetler;
                talep.Kategoriler = kategoriler;
                talep.TalepTarihi = DateTime.Now;
                talep.TalepDurumu = "Yeni";
                talep.IsYuzde = 0;
                talep.IsDurumu = "Beklemede";
                talep.yanitId = null;
                db.AdminTalepler.Add(talep);
                int sonuc = db.SaveChanges();


            }


            foreach (HttpPostedFileBase dsya in Dosya1)
            {
                Dosyalar yeniDosya = new Dosyalar();
                if (dsya != null)
                {
                   
            string uzanti = Path.GetExtension(dsya.FileName);
                    yeniDosya.uzanti = uzanti;
            yeniDosya.DosyaAdi = Path.GetFileNameWithoutExtension(dsya.FileName);
            string dosyaAdi = talep.TalepId + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH;mm")+ Path.GetFileNameWithoutExtension(dsya.FileName) + uzanti;
            yeniDosya.DosyaYolu = "~/Dosyalar/" + dosyaAdi;
            dosyaAdi = Path.Combine(Server.MapPath("~/Dosyalar/"), dosyaAdi);
            dsya.SaveAs(dosyaAdi);
            yeniDosya.talepId = talep.TalepId;
            
            db.Dosyalar.Add(yeniDosya);
            var asc = yeniDosya.DosyaId;
                }

                int sonuc = db.SaveChanges();
            }
            
            List<SelectListItem> kategoriList =
                (from Kategoriler in db.Kategoriler.ToList()
                 select new SelectListItem()
                 {
                     Text = Kategoriler.kategoriAdi,
                     Value = Kategoriler.kategoriId.ToString()
                 }).ToList();
            ViewBag.kategoriler = kategoriList;
            List<SelectListItem> aciliyetList =
                (from aciliyet in db.Aciliyetler.ToList()
                 select new SelectListItem()
                 {
                     Text = aciliyet.AciliyetTanim,
                     Value = aciliyet.AcilId.ToString()
                 }
                 ).ToList();


            TempData["aciliyetler"] = aciliyetList;
            ViewBag.aciliyetler = aciliyetList;
            string IsDurumu = null;
            string durum = "Acik";
            return RedirectToAction("Taleplerim","Kullanici",new { IsDurumu = durum});
        }
        public ActionResult Duzenle(int? talepID)
        {
            AdminTalepler talep = null;
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<SelectListItem> kategoriList =
             (from Kategoriler in db.Kategoriler.ToList()
              select new SelectListItem()
              {
                  Text = Kategoriler.kategoriAdi,
                  Value = Kategoriler.kategoriId.ToString()
              }).ToList();

            TempData["kategoriler2"] = kategoriList;
            ViewBag.kategoriler2 = kategoriList;

            //aciliyet listesi oluşturma
            List<SelectListItem> aciliyetList =
                (from aciliyet in db.Aciliyetler.ToList()
                 select new SelectListItem()
                 {
                     Text = aciliyet.AciliyetTanim,
                     Value = aciliyet.AcilId.ToString()
                 }
                 ).ToList();
            ViewBag.aciliyetler2 = aciliyetList;
            if (talepID != null)
            {


                var admini =
                    from admin in db.Adminler.ToList()
                    where admin.AdminId == talep.TalepId
                    select admin.AdminId;

                //-----------------------------------------------------------
                //List<SelectListItem> adminlerList =
                //    (from admin in db.Adminler.ToList()
                //     select new SelectListItem()
                //     {

                //         Text = admin.AdminAdSoyad,
                //         Value = admin.AdminId.ToString()
                //     }).ToList();

                //TempData["adminler"] = adminlerList;
                //ViewBag.adminler = adminlerList;
                //-----------------------------------------------------------
                TempData["admin"] = admini;
                ViewBag.admin = admini;
                talep = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);

            }
            return View(talep);
        }

        [HttpPost]
        public ActionResult Duzenle(AdminTalepler model, int? talepID)
        {
            //context.Database.ExecuteSqlCommand("UPDATE Table SET [Name] = {0} WHERE [Name] = {1}", nameProperty, oldNameProperty);
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();

            string kullaniciAdi = Session["Kullanici"].ToString();

            Adminler admin =
            db.Adminler
            .FirstOrDefault(x => x.AdminAdSoyad == kullaniciAdi);



            Kategoriler kategoriler =
           db.Kategoriler
           .Where(x => x.kategoriId == model.Kategoriler.kategoriId).FirstOrDefault();

            Aciliyetler aciliyetler =
           db.Aciliyetler
           .Where(x => x.AcilId == model.Aciliyetler.AcilId).FirstOrDefault();


            //Adminler admin = db.Adminler.Where(x => x.AdminId == model.AdminId).FirstOrDefault();
            AdminTalepler talep = db.AdminTalepler.FirstOrDefault(x => x.TalepId == talepID);
            if (admin != null)
            {
                talep.TalepId = model.TalepId;
                talep.TalepTarihi = model.TalepTarihi;
                talep.IsTanim = model.IsTanim;
                talep.Aciliyetler = aciliyetler;
                talep.Kategoriler = kategoriler;
                talep.IsDurumu = model.IsDurumu;
                talep.Baslik = model.Baslik;

                //db.AdminTalepler.Attach(talep);
                //db.Entry(talep).State = EntityState.Modified;

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
            ViewBag.admin = TempData["admin"];
            return RedirectToAction("Taleplerim", "Kullanici");
        }

        public ActionResult Sil(int? talepID)
        {
            AdminTalepler talep = null;
            if (talepID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
                talep = db.AdminTalepler.Where(x => x.TalepId == talepID).FirstOrDefault();
            }

            return View(talep);
        }
        [HttpPost, ActionName("Sil")]
        public ActionResult SilOk(int? talepID,AdminTalepler model)
        {
            if (talepID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
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

            return RedirectToAction("Taleplerim", "Kullanici");
        }

       

    }
}