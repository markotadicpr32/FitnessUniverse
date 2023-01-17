using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using FitnessUniverse.Models;
using System.Web.Hosting;

namespace FitnessUniverse.Controllers
{
    public class VlasnikController : Controller
    {
        public ActionResult Index()
        {

            Korisnik u = (Korisnik)Session["Korisnik"];
            if (u == null) return RedirectToAction("Index", "Posetilac");
            if (u.Uloga != "Vlasnik")
                return RedirectToAction("Index", "Posetilac");

            ViewBag.message = "Welcome Owner " + u.Username;


            ViewBag.time = DateTime.Now;

            List<FitnesCentar> ret = (List<FitnesCentar>)HttpContext.Application["a"];





            if (HttpContext.Application["sorted"] != null)
            {
                ret = (List<FitnesCentar>)HttpContext.Application["sorted"];
                HttpContext.Application["sorted"] = null;
            }

            return View(ret);
        }

        [HttpPost]
        public ActionResult ObrisiT()
        {
            string us = Request["us"];

            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];


            users[users.FindIndex(o => o.Username == us)].IsDeleted = true;

            Data d = new Data();
            d.saveUsers(users);

            return RedirectToAction("Users");
        }

        public ActionResult Detalji()
        {
            string name = Request["name"];

            List<FitnesCentar> fcs = (List<FitnesCentar>)HttpContext.Application["a"];


            foreach (var a in fcs)
            {
                if (a.Naziv == name)
                {
                    return View(a);
                }
            }


            return RedirectToAction("Index");

        }
        
        public ActionResult ObrisiFC()
        {
            string naziv = Request["naziv"];

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];
            FitnesCentar a = fc.Find(o => o.Naziv == naziv);

            fc[fc.FindIndex(o => o.Naziv == a.Naziv)].IsDeleted = true;
            Data d = new Data();
            d.saveFC(fc);
            return RedirectToAction("Index");

        }


        public ActionResult Users()
        {
            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];
            if (HttpContext.Application["sortedU"] != null)
            {
                users = (List<Korisnik>)HttpContext.Application["sortedU"];
                HttpContext.Application["sortedU"] = null;
            }
            return View(users);
        }

        public ActionResult FitnesCentri()
        {
            List<FitnesCentar> users = (List<FitnesCentar>)HttpContext.Application["users"];
            if (HttpContext.Application["sortedU"] != null)
            {
                users = (List<FitnesCentar>)HttpContext.Application["sortedU"];
                HttpContext.Application["sortedU"] = null;
            }
            return View(users);
        }

        public ActionResult DodajTrenera()
        {
            return View();
        }

        public ActionResult DodajFC()
        {
            return View();
        }

        public ActionResult IzmenaFC()
        {
            string naziv = Request["naziv"];

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];
            FitnesCentar a = fc.Find(o => o.Naziv == naziv);

            return View(a);

        }

        public ActionResult IzmeniFC()
        {
            string naziv = Request["naziv"];
            string adresa = Request["adresa"];
            DateTime d = new DateTime(int.Parse(Request["bdatey"]), 1, 1);
            int cenam = int.Parse(Request["cenam"]);
            int cenag = int.Parse(Request["cenag"]);
            int cenat = int.Parse(Request["cenat"]);
            int cenagt = int.Parse(Request["cenagt"]);
            int cenapt = int.Parse(Request["cenapt"]);

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];
            foreach (var a in fc)
                if (a.Naziv == naziv)
                {
                    a.Naziv = naziv;
                    a.Adresa = adresa;
                    a.GodinaOtvaranja = d;
                    a.CenaMesec = cenam;
                    a.CenaGodina = cenag;
                    a.CenaTrening = cenat;
                    a.CenaGrupni = cenagt;
                    a.CenaPersonal = cenapt;
                }

            Data da = new Data();
            da.saveFC(fc);


            FitnesCentar ar = fc.Find(o => o.Naziv == naziv);
            Korisnik u = (Korisnik)Session["Korisnik"];
            ViewBag.user = u.Username;
            return View("Detalji", ar);
        }


        public ActionResult RegisterT()
        {
            string us = Request["us"];
            string pass = Request["ps"];
            string fname = Request["fname"];
            string lname = Request["lname"];
            string gender = Request["pol"];
            string mail = Request["mail"];

            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];

            foreach (Korisnik u in users)
            {
                if (u.Username == us)
                {
                    ViewBag.message = "That Username is taken, please try another one";
                    return RedirectToAction("DodajTrenera");
                }
            }


            DateTime dobs = new DateTime(int.Parse(Request["bdatey"]), int.Parse(Request["bdatem"]), int.Parse(Request["bdated"]));

            Korisnik newUser = new Korisnik(us, pass, fname, lname, gender, mail, dobs);
            newUser.Uloga = "Trener";


            users.Add(newUser);

            Data d = new Data();
            d.saveUsers(users);

            return RedirectToAction("Index");
        }


        public ActionResult UnesiFC(string naziv, string adresa, string bdatey, string cenam, string cenag, string cenat, string cenagt, string cenapt)
        {

            if (naziv.Trim() == string.Empty)
            {
                ViewBag.error = "Mora postojati NAZIV za fitnes centar";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            if (adresa.Trim() == string.Empty)
            {
                ViewBag.error = "Mora postojati ADRESA za fitnes centa";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            if (bdatey.Trim() == string.Empty || bdatey.Trim() == "YYYY")
            {
                ViewBag.error = "Mora postojati GODINA OTVARANJA fitnes centra";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            DateTime d = new DateTime(int.Parse(bdatey), 1, 1);

            if (cenam.Trim() == string.Empty)
            {
                ViewBag.error = "Nije uneta CENA MESECNE CLANARINE za fitnes centar";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            if (cenag.Trim() == string.Empty)
            {
                ViewBag.error = "Nije uneta CENA GODISNJE CLANARINE za fitnes centar";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            if (cenat.Trim() == string.Empty)
            {
                ViewBag.error = "Nije uneta CENA TRENINGA za fitnes centar";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            if (cenagt.Trim() == string.Empty)
            {
                ViewBag.error = "Nije uneta CENA GRUPNOG TRENINGA za fitnes centar";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }
            if (cenapt.Trim() == string.Empty)
            {
                ViewBag.error = "Nije uneta CENA PERSONALNOG TRENINGA za fitnes centar";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajFC");
            }




            FitnesCentar a = new FitnesCentar(naziv, adresa, d, int.Parse(cenam), int.Parse(cenag), int.Parse(cenat), int.Parse(cenagt), int.Parse(cenapt));
            Korisnik u = (Korisnik)Session["Korisnik"];
            a.Vlasnik = u.Username;
            List<FitnesCentar> fcs = (List<FitnesCentar>)HttpContext.Application["a"];
            fcs.Add(a);

            Data da = new Data();
            da.saveFC(fcs);


            return RedirectToAction("Index");

        }

        public ActionResult LogOut()
        {
            Session["Korisnik"] = null;

            return RedirectToAction("Index", "Posetilac");
        }



        public ActionResult PretragaSortiranjeProsli()
        {

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];

            List<FitnesCentar> retfc = new List<FitnesCentar>();

            DateTime bDate = new DateTime();
            DateTime tDate = new DateTime();

            if (Request["mingod"].Trim() != String.Empty && Request["mingod"] != "YYYY")
            {

                bDate = new DateTime(int.Parse(Request["mingod"]), 1, 1);
            }
            else
            {
                bDate = new DateTime(1971, 1, 1);
            }

            if (Request["maxgod"].Trim() != String.Empty && Request["maxgod"] != "YYYY")
            {

                bDate = new DateTime(int.Parse(Request["maxgod"]), 1, 1);
            }
            else
            {
                tDate = new DateTime(2050, 2, 2);
            }

            foreach (var a in fc)
            {
                if (a.GodinaOtvaranja >= bDate && a.GodinaOtvaranja <= tDate)
                    retfc.Add(a);
            }

            if (Request["sortby"] != string.Empty)
            {
                string g = Request["sortby"];
                if (Request["sortby"] == "Sortiraj po nazivu")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Naziv).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Naziv).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po adresi")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Adresa).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Adresa).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po godini otaranja")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.GodinaOtvaranja).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.GodinaOtvaranja).ToList<FitnesCentar>();
                }
            }


            if (Request["name"].Trim() != string.Empty)
            {
                foreach (var a in retfc)
                    if (!(a.Naziv.ToLower().Contains(Request["name"].ToLower())))
                        retfc.Remove(a);
            }



            HttpContext.Application["sorted"] = retfc;

            return RedirectToAction("Prethodna");
        }

        public ActionResult Prethodna()
        {
            List<FitnesCentar> args = (List<FitnesCentar>)HttpContext.Application["a"];

            foreach (var a in args)
            {
                if (a.GodinaOtvaranja > DateTime.Now)
                    args.Remove(a);
            }

            if (HttpContext.Application["sorted"] != null)
            {
                args = (List<FitnesCentar>)HttpContext.Application["sorted"];
                HttpContext.Application["sorted"] = null;
            }

            return View(args);
        }



        public ActionResult PretragaSortiranje()
        {

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];

            List<FitnesCentar> retfc = new List<FitnesCentar>();

            DateTime bDate = new DateTime();
            DateTime tDate = new DateTime();

            if (Request["mingod"].Trim() != String.Empty && Request["mingod"] != "YYYY")
            {

                bDate = new DateTime(int.Parse(Request["mingod"]), 1, 1);
            }
            else
            {
                bDate = new DateTime(1971, 1, 1);
            }

            if (Request["maxgod"].Trim() != String.Empty && Request["maxgod"] != "YYYY")
            {

                bDate = new DateTime(int.Parse(Request["maxgod"]), 1, 1);
            }
            else
            {
                tDate = new DateTime(2050, 2, 2);
            }

            foreach (var a in fc)
            {
                if (a.GodinaOtvaranja >= bDate && a.GodinaOtvaranja <= tDate)
                    retfc.Add(a);
            }

            if (Request["sortby"] != string.Empty)
            {
                string g = Request["sortby"];
                if (Request["sortby"] == "Sortiraj po nazivu")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Naziv).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Naziv).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po adresi")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Adresa).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Adresa).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po godini otaranja")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.GodinaOtvaranja).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.GodinaOtvaranja).ToList<FitnesCentar>();
                }
            }

            if (Request["name"].Trim() != string.Empty)
            {
                foreach (var a in retfc.ToList())
                    if (!(a.Naziv.ToLower().Contains(Request["name"].ToLower())))
                        retfc.Remove(a);
            }


            HttpContext.Application["sorted"] = retfc;

            return RedirectToAction("Index");
        }

        public ActionResult IsApproved()
        {
            string naziv = Request["name"];
            int id = int.Parse(Request["id"]);
            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];
            foreach (var a in fc)
                if (a.Naziv == naziv)
                {
                    foreach (var c in a.Komentari.ToList())
                    {
                        if (c.ID == id)
                            c.IsApproved = true;
                    }
                }

            Data d = new Data();
            d.saveFC(fc);


            FitnesCentar fit = fc.Find(o => o.Naziv == naziv);
            Korisnik u = (Korisnik)Session["Korisnik"];
            ViewBag.user = u.Username;
            return View("Detalji", fit);




        }

        public ActionResult DelComment()
        {
            string naziv = Request["name"];
            int id = int.Parse(Request["id"]);
            List<FitnesCentar> fcs = (List<FitnesCentar>)HttpContext.Application["a"];
            foreach (var a in fcs)
                if (a.Naziv == naziv)
                {
                    foreach (var c in a.Komentari)
                    {
                        if (c.ID == id)
                            c.IsDeleted = true;
                    }
                }

            Data d = new Data();
            d.saveFC(fcs);

            FitnesCentar fc = fcs.Find(o => o.Naziv == naziv);
            Korisnik u = (Korisnik)Session["Korisnik"];
            ViewBag.user = u.Username;

            return View("Detalji", fc);

        }
    }
}