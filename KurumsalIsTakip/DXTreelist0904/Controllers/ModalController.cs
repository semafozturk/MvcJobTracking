using DXTreelist0904.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXTreelist0904.Controllers
{
    public class ModalController : Controller
    {
        // GET: Modal
        public ActionResult AdminEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminEkle(Adminler admin)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            db.Adminler.Add(admin);
            int sonuc = db.SaveChanges();
            if (sonuc > 0)
            {
                ViewBag.Result = "Kişi Kaydedildi.";
                ViewBag.Status = "success";
            }
            else
            {
                ViewBag.Result = "Kişi Kaydedilemedi.";
                ViewBag.Status = "danger";
            }
            return View(admin);
        }
        public ActionResult YanitSil(int? talepID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            AdminTalepler talep = null;
            talep = db.AdminTalepler.Where(x => x.TalepId == talepID).FirstOrDefault();
            return View(talep);
        }

        [HttpPost, ActionName("YanitSil")]
        //[AuthFilter]
        public ActionResult YanitSilOk(int? talepID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();

            AdminTalepler talep = db.AdminTalepler.Where(x => x.TalepId == talepID).FirstOrDefault();
           db.AdminTalepler.Remove(talep);
           db.SaveChanges();

            if (Session["OturumYetki"].ToString() == "1")
            {
                return RedirectToAction("TalepGoster", "Admin");
            }
            else if (Session["OturumYetki"].ToString() == "0")
            {
                return RedirectToAction("TalepGoster", "Kullanici",new { talepID = talep.yanitId });
            }
            return View();
        }
        public ActionResult YanitDuzenle(int? talepID)
        {
            AdminTalepler talep = null;
            if (talepID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
                talep = db.AdminTalepler.Where(x => x.TalepId == talepID).FirstOrDefault();
            }
            return View(talep);
        }
        [HttpPost]
        public ActionResult YanitDuzenle(AdminTalepler model,int? talepID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            AdminTalepler talep = db.AdminTalepler.Where(x => x.TalepId == talepID).FirstOrDefault();
            if (talep != null)
            {
                talep.IsTanim = model.IsTanim;

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenememiştir.";
                    ViewBag.Status = "danger";
                }
            }
            return RedirectToAction("TalepGoster", "Admin",new { talepID = talep.yanitId });
        }

        public ActionResult TalepDuzenle(int? adminID)
        {
            Adminler admin = null;
            if (adminID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
                admin = db.Adminler.Where(x => x.AdminId == adminID).FirstOrDefault();
            }
            return View(admin);

        }
        public ActionResult AdminDuzenle(int? adminID)
        {
            Adminler admin = null;
            if (adminID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
                admin = db.Adminler.Where(x => x.AdminId == adminID).FirstOrDefault();
            }
            return View(admin);
        }
        [HttpPost]
        public ActionResult AdminDuzenle(Adminler model, int? adminID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            Adminler admin = db.Adminler.Where(x => x.AdminId == adminID).FirstOrDefault();
            if (admin != null)
            {
                admin.AdminKAdi = model.AdminKAdi;
                admin.AdminSifre = model.AdminSifre;
                admin.AdminAdSoyad = model.AdminAdSoyad;
                admin.AdminBolum = model.AdminBolum;
                admin.AdminGorev = model.AdminGorev;
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenememiştir.";
                    ViewBag.Status = "danger";
                }
            }
            return RedirectToAction("Adminler", "Admin");
        }


        public ActionResult AdminSil(int? adminID)
        {
            Adminler admin = null;
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            if (adminID != null)
            {
                
                admin = db.Adminler.Where(x => x.AdminId == adminID).FirstOrDefault();
            }
            List<AdminTalepler> talepler = db.AdminTalepler.Where(x => x.adminId == adminID).ToList();
            if (talepler.Count > 0)
            {
                ViewBag.adminDurum = "danger";
                ViewBag.adminSonuc = "Silmek istediğiniz kullanıcıya ait "+talepler.Count+" adet talep bulunmaktadır. Yine de silmek istiyor musunuz?";

            }
            else
            {
                ViewBag.adminDurum = "danger";
                ViewBag.adminSonuc = "Silmek istediğiniz kullanıcıya ait talep bulunmamaktadır.";
            }


            return View(admin);
        }

        [HttpPost, ActionName("AdminSil")]
        //[AuthFilter]
        public ActionResult SilOk(int? adminID, Adminler admin)
        {
            if (adminID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
                admin = db.Adminler.FirstOrDefault(x => x.AdminId == adminID);

                var bagliTalepler = db.AdminTalepler.Where(x => x.adminId == adminID & x.TalepDurumu != "Yanıt").ToList();
                var bagliDosyalar = db.Dosyalar.Where(x => x.AdminTalepler.adminId == adminID).ToList();

                foreach (var dosya in bagliDosyalar)
                {
                    db.Dosyalar.Remove(dosya);
                }
                foreach (var talep in bagliTalepler)
                {
                    db.AdminTalepler.Remove(talep);
                }
                db.Adminler.Remove(admin);
                db.SaveChanges();
            }

            return RedirectToAction("Adminler", "Admin");
        }

        public ActionResult SifreDegistir(int? adminID)
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifreDegistir(int? adminID,Adminler modal, string eskiSifre)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            var admin = db.Adminler.FirstOrDefault(x => x.AdminId == adminID);
            if (eskiSifre == modal.AdminSifre)
            {
                ViewBag.Durum = "Mevcut şifreniz ile yeni şifreniz aynı olamaz.";
                ViewBag.Alert = "danger";
            }
            else if (admin.AdminSifre == eskiSifre)
            {
                admin.AdminSifre = modal.AdminSifre;
                ViewBag.Durum = "Bilgiler Kaydedildi.";
                ViewBag.Alert = "success";
              
                db.SaveChanges();
            }
            else if (admin.AdminSifre != eskiSifre)
            {
                ViewBag.Durum = "Eski şifre yanlış girildi.";
                ViewBag.Alert = "danger";
            }
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Giris","Home");
           
        }
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategoriler kategori)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            db.Kategoriler.Add(kategori);
            int sonuc = db.SaveChanges();
            if (sonuc > 0)
            {
                ViewBag.Result = "Kişi Kaydedildi.";
                ViewBag.Status = "success";
            }
            else
            {
                ViewBag.Result = "Kişi Kaydedilemedi.";
                ViewBag.Status = "danger";
            }
            return RedirectToAction("Kategoriler", "Admin");
        }
        public ActionResult KategoriDuzenle(int? katID)
        {
            Kategoriler kategori = null;
            if (katID != null)
            {
                KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
                kategori = db.Kategoriler.FirstOrDefault(x => x.kategoriId == katID);
            }
            return View(kategori);
        }
        [HttpPost]
        public ActionResult KategoriDuzenle(Kategoriler model, int? katID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            Kategoriler kategoriler = db.Kategoriler.FirstOrDefault(x => x.kategoriId == katID);
            if (kategoriler != null)
            {
                kategoriler.kategoriAdi = model.kategoriAdi;

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenememiştir.";
                    ViewBag.Status = "danger";
                }
            }

            return RedirectToAction("Kategoriler", "Admin");
        }
        public ActionResult KategoriSil(int? katID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<AdminTalepler> talepler = db.AdminTalepler.Where(x => x.kategoriId == katID).ToList();
            if (talepler.Count > 0)
            {
                ViewBag.katDurum = "danger";
                ViewBag.katSonuc = "Bu kategoride " + talepler.Count + " adet talep bulunmaktadır. Bu kategoriyi yine de silmek istiyor musunuz?";
            }
            else if (talepler.Count == 0)
            {
                ViewBag.katDurum = "success";
                ViewBag.katSonuc = "Bu kategoriye ait talep bulunmamaktadır.";

            }
            Kategoriler kategori = null;
            if (katID != null)
            {
                kategori = db.Kategoriler.Where(x => x.kategoriId == katID).FirstOrDefault();
            }

            return View(kategori);
        }
        [HttpPost]
        public ActionResult KategoriSil(Kategoriler model,int? katID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            List<AdminTalepler> talepler = db.AdminTalepler.Where(x => x.kategoriId == katID).ToList();
                Kategoriler kategori = db.Kategoriler.Where(x => x.kategoriId == katID).FirstOrDefault();
            
            if (talepler.Count == 0)
            {
                db.Kategoriler.Remove(kategori);
                db.SaveChanges();
                ViewBag.acilSonuc = "Seçtiğiniz aciliyet tanımı (" + kategori.kategoriAdi + ") na ait talep bulunmamaktadır..";
                return RedirectToAction("Kategoriler", "Admin");
            }
            else
            {
                foreach (var talep in talepler)
                {
                    db.AdminTalepler.Remove(talep);
                }
                db.Kategoriler.Remove(kategori);
                db.SaveChanges();
                ViewBag.acilSonuc = kategori.kategoriAdi + " aciliyette " + talepler.Count + " adet talep var. Bu Aciliyet tanımını silemezsiniz..";
            }


            return RedirectToAction("Kategoriler", "Admin");
        }
        public ActionResult AciliyetEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AciliyetEkle(Aciliyetler aciliyet)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            db.Aciliyetler.Add(aciliyet);
            int sonuc = db.SaveChanges();
            if (sonuc > 0)
            {
                ViewBag.Result = "Kişi Kaydedildi.";
                ViewBag.Status = "success";
            }
            else
            {
                ViewBag.Result = "Kişi Kaydedilemedi.";
                ViewBag.Status = "danger";
            }
            return RedirectToAction("Aciliyetler", "Admin");
        }

        public ActionResult AciliyetDuzenle(int? acilID)
        {
            Aciliyetler aciliyet = null;
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            if (acilID != null)
            {
                aciliyet = db.Aciliyetler.FirstOrDefault(x => x.AcilId == acilID);

            }
            return View(aciliyet);
        }
        [HttpPost]
        public ActionResult AciliyetDuzenle(Aciliyetler model, int? acilID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            Aciliyetler aciliyetler = db.Aciliyetler.FirstOrDefault(x => x.AcilId == acilID);
            if (aciliyetler != null)
            {
                aciliyetler.AciliyetTanim = model.AciliyetTanim;

                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenmiştir.";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Kişi Bilgileri Güncellenememiştir.";
                    ViewBag.Status = "danger";
                }
            }

            return RedirectToAction("Aciliyetler", "Admin");
        }

        public ActionResult AciliyetSil(int? acilID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            Aciliyetler aciliyet = null;
                aciliyet = db.Aciliyetler.Where(x => x.AcilId == acilID).FirstOrDefault();
            List<AdminTalepler> talepler = db.AdminTalepler.Where(x => x.acilId == acilID).ToList();
            if (talepler.Count == 0)
            {
                ViewBag.acilDurumu = "success";
                ViewBag.acilSonuc = "seçtiğiniz aciliyet tanımı (" + aciliyet.AciliyetTanim + ") na ait talep bulunmamaktadır.. Silinebilir.";

            }
            else
            {
                ViewBag.acilDurumu = "danger";
                ViewBag.acilSonuc = aciliyet.AciliyetTanim + " aciliyette " + talepler.Count + " adet talep var. Bu Aciliyet tanımını silemezsiniz..";
            }
            return View(aciliyet);
        }
        [HttpPost, ActionName("AciliyetSil")]
        //[AuthFilter]
        public ActionResult AciliyetSilOk(int? acilID)
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            Aciliyetler aciliyet = db.Aciliyetler.Where(x => x.AcilId == acilID).FirstOrDefault();
            List<AdminTalepler> talepler = db.AdminTalepler.Where(x => x.acilId == acilID).ToList();

            if (talepler.Count == 0)
            {
                
                db.Aciliyetler.Remove(aciliyet);
                db.SaveChanges();
                ViewBag.acilSonuc = "Seçtiğiniz aciliyet tanımı ("+aciliyet.AciliyetTanim+") na ait talep bulunmamaktadır..";

            }
            else
            {
                foreach (var talep in talepler)
                {
                    db.AdminTalepler.Remove(talep);
                }
                db.Aciliyetler.Remove(aciliyet);
                db.SaveChanges();
                ViewBag.acilSonuc = aciliyet.AciliyetTanim+" aciliyette "+talepler.Count+" adet talep var. Bu Aciliyet tanımını silemezsiniz..";
            }
              

            return RedirectToAction("Aciliyetler", "Admin");
        }
        public ActionResult Bilgilerim()
        {
            KurumsalIsTakipEntities db = new KurumsalIsTakipEntities();
            if (Session["Kullanici"] != null)
            {
                string kadi = Session["kullanici"].ToString();
                Adminler admin = db.Adminler.FirstOrDefault(x => x.AdminAdSoyad == kadi);
                return View(admin);
            }
            else
            {
                Adminler admin = db.Adminler.FirstOrDefault(x => x.AdminId == 1);
                return View(admin);
            }
            
           
        }
        
    }
}