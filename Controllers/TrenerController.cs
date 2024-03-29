﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using FitnessUniverse.Models;
using System.Web.Hosting;

namespace FitnessUniverse.Controllers
{
    public class TrenerController : Controller
    {
        public ActionResult Index()
        {
            List<FitnesCentar> ret = (List<FitnesCentar>)HttpContext.Application["a"];

            if (HttpContext.Application["sorted"] != null)
            {
                ret = (List<FitnesCentar>)HttpContext.Application["sorted"];
                HttpContext.Application["sorted"] = null;
            }
            Korisnik u = (Korisnik)Session["Korisnik"];
            if (u == null) return RedirectToAction("Posetilac", "Index");
            if (u.Uloga != "Trener") return RedirectToAction("Posetilac", "Index");

            ViewBag.user = u.Username;
            ViewBag.message = "Welcome Trainer " + u.Username;
            ViewBag.time = DateTime.Now;
            return View(ret);
        }

        public ActionResult PretragaSoritiranjeTr()
        {
            string naziv = Request["naziv"];

            List<FitnesCentar> args = (List<FitnesCentar>)HttpContext.Application["a"];

            FitnesCentar a = args.Find(o => o.Naziv == naziv);

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

                tDate = new DateTime(int.Parse(Request["maxgod"]), 1, 1);
            }
            else
            {
                tDate = new DateTime(2050, 2, 2);
            }

            foreach (var c in a.GrupniTreninzi)
            {
                if (c.DatumVreme >= bDate && c.DatumVreme <= tDate)
                    a.GrupniTreninzi.Add(c);
            }

            if (Request["sortby"] != string.Empty)
            {
                string g = Request["sortby"];
                if (Request["sortby"] == "Sortiraj po Nazivu")
                {
                    if (Request["sortin"] == "Opadajuce")
                        a.GrupniTreninzi = a.GrupniTreninzi.OrderByDescending(o => o.Naziv).ToList();
                    else
                        a.GrupniTreninzi = a.GrupniTreninzi.OrderBy(o => o.Naziv).ToList();
                }
                if (Request["sortby"] == "Sortiraj po Tipu")
                {
                    if (Request["sortin"] == "Opadajuce")
                        a.GrupniTreninzi = a.GrupniTreninzi.OrderByDescending(o => o.Tip).ToList();
                    else
                        a.GrupniTreninzi = a.GrupniTreninzi.OrderBy(o => o.Tip).ToList();
                }
                if (Request["sortby"] == "Sortiraj po Datumu odrzavanja")
                {
                    if (Request["sortin"] == "Opadajuce")
                        a.GrupniTreninzi = a.GrupniTreninzi.OrderByDescending(o => o.DatumVreme).ToList();
                    else
                        a.GrupniTreninzi = a.GrupniTreninzi.OrderBy(o => o.DatumVreme).ToList();
                }
            }

            if (Request["name"].Trim() != string.Empty)
            {
                foreach (var b in a.GrupniTreninzi.ToList())
                    if (!(b.Naziv.ToLower().Contains(Request["name"].ToLower())))
                        a.GrupniTreninzi.Remove(b);
            }
            /*
            if (Request["tip"].Trim() != string.Empty)
            {
                foreach (var b in a.GrupniTreninzi.ToList())
                    if (!(b.Tip.ToLower().Contains(Request["tip"].ToLower())))
                        a.GrupniTreninzi.Remove(b);
            }*/
            if (Request["tip"].Trim() == string.Empty)
            {
                foreach (var b in a.GrupniTreninzi.ToList())
                    if (b.Tip != Request["tip"])
                        a.GrupniTreninzi.Remove(b);
            }

            return View("Detalji", a);
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

                tDate = new DateTime(int.Parse(Request["maxgod"]), 1, 1);
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
                if (Request["sortby"] == "Sortiraj po Nazivu")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Naziv).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Naziv).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po Adresi")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Adresa).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Adresa).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po Godini otaranja")
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

            if (Request["address"].Trim() != string.Empty)
            {
                foreach (var a in retfc.ToList())
                    if (!(a.Adresa.ToLower().Contains(Request["address"].ToLower())))
                        retfc.Remove(a);
            }

            HttpContext.Application["sorted"] = retfc;

            return RedirectToAction("Index");
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

                tDate = new DateTime(int.Parse(Request["maxgod"]), 1, 1);
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
                if (Request["sortby"] == "Sortiraj po Nazivu")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Naziv).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Naziv).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po Adresi")
                {
                    if (Request["sortin"] == "Opadajuce")
                        retfc = retfc.OrderByDescending(o => o.Adresa).ToList<FitnesCentar>();
                    else
                        retfc = retfc.OrderBy(o => o.Adresa).ToList<FitnesCentar>();
                }
                if (Request["sortby"] == "Sortiraj po Godini otaranja")
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

            if (Request["address"].Trim() != string.Empty)
            {
                foreach (var a in retfc.ToList())
                    if (!(a.Adresa.ToLower().Contains(Request["address"].ToLower())))
                        retfc.Remove(a);
            }

            HttpContext.Application["sorted"] = retfc;

            return RedirectToAction("Prethodna");
        }

        public ActionResult Prethodna()
        {
            List<FitnesCentar> fcs = (List<FitnesCentar>)HttpContext.Application["a"];

            foreach (var a in fcs)
            {
                if (a.GodinaOtvaranja > DateTime.Now)
                    fcs.Remove(a);
            }

            if (HttpContext.Application["sorted"] != null)
            {
                fcs = (List<FitnesCentar>)HttpContext.Application["sorted"];
                HttpContext.Application["sorted"] = null;
            }

            return View(fcs);
        }

        public ActionResult IzmeniT()
        {
            string naziv = Request["naziv"];

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];
            FitnesCentar a = fc.Find(o => o.Naziv == naziv);

            return View(a);

        }

        public ActionResult IzmenaT()
        {
            string naziv = Request["naziv"];
            string tip = Request["tip"];
            int duration = int.Parse(Request["duration"]);
            DateTime d = new DateTime(1, 1, 1, int.Parse(Request["vdateh"]), int.Parse(Request["vdatem"]), 1);
            int max = int.Parse(Request["max"]);


            List<GrupniTrening> gt = (List<GrupniTrening>)HttpContext.Application["a"];
            foreach (var a in gt)
                if (a.Naziv == naziv)
                {
                    a.Naziv = naziv;
                    a.Tip = tip;
                    a.Trajanje = duration;
                    a.DatumVreme = d;
                    a.MaxPosetioci = max;
                }

            Data da = new Data();
            da.saveGT(gt);


            GrupniTrening ar = gt.Find(o => o.Naziv == naziv);
            Korisnik u = (Korisnik)Session["Korisnik"];
            ViewBag.user = u.Username;
            return View("Detalji", ar);
        }

        public ActionResult DodajTrening()
        {
            string naziv = Request["naziv"];

            ViewBag.naziv = naziv;


            return View();
        }

        /*
        public ActionResult UnesiT(string naziv, string tip, string duration, string vdateh, string vdatem, string max)
        {

            if (naziv.Trim() == string.Empty)
            {
                ViewBag.error = "Mora postojati NAZIV za trening";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajTrenera");
            }
            if (tip.Trim() == string.Empty)
            {
                ViewBag.error = "Mora postojati TIP za trening";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajTrenera");
            }
            if (duration.Trim() == string.Empty)
            {
                ViewBag.error = "Nije uneto TRAJANJE treninga";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajTrenera");
            }
            if (vdateh.Trim() == string.Empty || vdateh.Trim() == "HH" || vdatem.Trim() == string.Empty || vdatem.Trim() == "mm")
            {
                ViewBag.error = "Mora postojati VREME ODRZAVANJA treninga";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajTrenera");
            }
            DateTime d = new DateTime(1, 1, 1, int.Parse(vdateh), int.Parse(vdatem), 1);
            if (max.Trim() == string.Empty)
            {
                ViewBag.error = "Nije unet MAKSIMALAN BROJ POSETIOCA";
                Korisnik user = (Korisnik)Session["Korisnik"];
                ViewBag.username = user.Username;
                return View("DodajTrenera");
            }

            GrupniTrening a = new GrupniTrening(naziv, tip, int.Parse(duration), d, int.Parse(max));
            Korisnik u = (Korisnik)Session["Korisnik"];
            a.Fc = u.Username;
            List<GrupniTrening> gt = (List<GrupniTrening>)HttpContext.Application["a"];
            gt.Add(a);

            Data da = new Data();
            da.saveGT(gt);


            return RedirectToAction("Index");

        }
        */
        public ActionResult UnosenjeTreninga()
        {
            string nazivA = Request["nazivA"];

            string naziv = Request["naziv"];
            string tip = Request["tip"];
            int duration = int.Parse(Request["duration"]);
            DateTime d = new DateTime(int.Parse(Request["pdatey"]), int.Parse(Request["pdatemo"]), int.Parse(Request["pdated"]), int.Parse(Request["pdateh"]), int.Parse(Request["pdatemi"]), 1);
            int max = int.Parse(Request["max"]);
            bool zauzeto = false;

            Korisnik u = (Korisnik)Session["Korisnik"];

            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];
            foreach (var a in fc)

                if (a.Naziv == nazivA)
                {
                    int num = 1;
                    if (a.GrupniTreninzi.Count != 0)
                    {
                        int maks = 0;
                        foreach (var s in a.GrupniTreninzi)
                        {
                            if (s.num > maks) maks = s.num;
                            AngazovanjeTrening at = new AngazovanjeTrening(u, a, s);
                            u.GTAngazovan.Add(at);
                        }

                        num = maks;
                        num++;
                    }

                    GrupniTrening sj = new GrupniTrening(num, naziv, tip, duration, d, max, zauzeto);

                    a.GrupniTreninzi.Add(sj);

                    

                }

            Data dat = new Data();
            dat.saveFC(fc);

            FitnesCentar ar = fc.Find(o => o.Naziv == nazivA);

            ViewBag.user = u.Username;

            return View("Detalji", ar);

        }

        
        

        public ActionResult Detalji()
        {
            List<FitnesCentar> fcs = (List<FitnesCentar>)HttpContext.Application["a"];
            string naziv = Request["name"];
            FitnesCentar a = fcs.Find(o => o.Naziv == naziv);
            Korisnik u = (Korisnik)Session["Korisnik"];
            ViewBag.user = u.Username;
            return View(a);
        }

        public ActionResult PretragaSortiranjePosetilac()
        {
            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];

            List<Korisnik> retUsers = users;

            foreach (var p in users.ToList())
            {
                if(p.Uloga == "Posetilac")
                {
                    if (Request["fname"] != string.Empty)
                    {


                        string name = Request["fname"];

                        foreach (var u in retUsers.ToList())
                        {
                            if (!(u.Ime.ToLower().Contains(name.ToLower())))
                                retUsers.Remove(u);
                        }


                    }


                    if (Request["lname"] != string.Empty)
                    {


                        string lastName = Request["lname"];

                        foreach (var u in retUsers)
                        {
                            if (!(u.Ime.ToLower().Contains(lastName.ToLower())))
                                retUsers.Remove(u);
                        }


                    }

                    if (Request["fname"] != string.Empty)
                    {


                        string role = Request["role"];

                        foreach (var u in retUsers)
                        {
                            if (!(u.Ime.ToLower().Contains(role.ToLower())))
                                retUsers.Remove(u);
                        }


                    }

                    if (Request["sortBy"] != string.Empty)
                    {
                        if (Request["sortBy"] == "Sortiraj po Imenu")
                        {
                            if (Request["sortIn"] == "Opadajuce")
                                retUsers = retUsers.OrderByDescending(o => o.Ime).ToList();
                            else
                                retUsers = retUsers.OrderBy(o => o.Ime).ToList();
                        }

                        if (Request["sortBy"] == "Sortiraj po Prezimenu")
                        {
                            if (Request["sortIn"] == "Opadajuce")
                                retUsers = retUsers.OrderByDescending(o => o.Prezime).ToList();
                            else
                                retUsers = retUsers.OrderBy(o => o.Prezime).ToList();
                        }
                    }



                    
                }
                
            }
            HttpContext.Application["sortedU"] = retUsers;
            return RedirectToAction("Users");
        }
    }
}