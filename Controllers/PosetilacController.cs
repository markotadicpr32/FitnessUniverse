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
    public class PosetilacController : Controller
    {

        public ActionResult Index()
        {

            Korisnik u = (Korisnik)Session["Korisnik"];

            if (u != null)
            {
                if (u.Uloga == "Trener")
                    return RedirectToAction("Index", "Trener");
                if (u.Uloga == "Vlasnik")
                    return RedirectToAction("Index", "Vlasnik");
            }

            if (u == null)
                ViewBag.message = " Welcome! Please Login or Create an Account in order to see your personal data.";
            else
                ViewBag.message = " Welcome " + u.Ime + " " + u.Prezime + " ";

            List<FitnesCentar> ret = (List<FitnesCentar>)HttpContext.Application["a"];

            if (HttpContext.Application["sorted"] != null)
            {
                ret = (List<FitnesCentar>)HttpContext.Application["sorted"];
                HttpContext.Application["sorted"] = null;
            }

            ViewBag.time = DateTime.Now;
            return View(ret);
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

        public ActionResult MojProfil()
        {
            Korisnik u = (Korisnik)Session["Korisnik"];
            return View(u);
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Detalji()
        {
            string name = Request["name"];

            List<FitnesCentar> args = (List<FitnesCentar>)HttpContext.Application["a"];


            foreach (var a in args)
            {
                if (a.Naziv == name)
                {
                    return View(a);
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult OstaviKomentar()
        {
            Korisnik u = (Korisnik)Session["Korisnik"];

            if (u == null) return RedirectToAction("Login");

            List<FitnesCentar> fcs = (List<FitnesCentar>)HttpContext.Application["a"];

            foreach (var a in fcs)
            {
                if (a.Naziv == Request["name"])
                {

                    a.Komentari.Add(new Komentar(u.Username, a.Naziv, Request["text"], int.Parse(Request["grade"])));

                }
            }

            Data d = new Data();
            d.saveFC(fcs);

            return RedirectToAction("MojProfil");
        }

        [HttpPost]
        public ActionResult LoginC()
        {

            string us = Request["uname"];
            string pass = Request["upass"];

            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];

            foreach (Korisnik u in users)
            {
                if (u.Username == us)
                {
                    if (u.Password == pass)
                    {
                        if (u.IsDeleted == true)
                        {
                            ViewBag.message = "ACCESS DENIED";
                            return View("Login");
                        }
                        Session["User"] = u;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.message = "YOUR PASSWORD IS INCORECT";
                        return View("Login");
                    }
                }
            }
            ViewBag.message = "THAT USERNAME DOESN'T EXIST";
            return View("Login");
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



        [HttpPost]
        public ActionResult RegisterC()
        {

            string us = Request["us"];
            string pass = Request["pass"];
            string fname = Request["fname"];
            string lname = Request["lname"];
            string gender = Request["pol"];
            string mail = Request["mail"];


            if (us == string.Empty) { ViewBag.message = "You must have a Username"; return View("Register"); }
            if (pass == string.Empty) { ViewBag.message = "You must have a Password"; return View("Register"); }
            if (fname == string.Empty) { ViewBag.message = "You must have a first name"; return View("Register"); }
            if (lname == string.Empty) { ViewBag.message = "You must have a last name"; return View("Register"); }
            if (gender == string.Empty) { ViewBag.message = "You must have a Gender"; return View("Register"); }
            if (mail == string.Empty) { ViewBag.message = "You must have an Email"; return View("Register"); }
            if (Request["bdated"] == string.Empty || Request["bdated"] == "DD") { ViewBag.message = "Your Date of birth is not adequite (DAY)"; return View("Register"); }
            if (Request["bdatem"] == string.Empty || Request["bdatem"] == "MM") { ViewBag.message = "Your Date of birth is not adequite (MONTH)"; return View("Register"); }
            if (Request["bdatey"] == string.Empty || Request["bdatey"] == "YYYY") { ViewBag.message = "Your Date of birth is not adequite (YEAR)"; return View("Register"); }



            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];

            foreach (Korisnik u in users)
            {
                if (u.Username == us)
                {
                    ViewBag.message = "That Username is taken, please try another one";
                    return RedirectToAction("Register");
                }
            }

            string dob = Request["DatumRodjenja"];
            DateTime dobs = new DateTime(int.Parse(Request["bdatey"]), int.Parse(Request["bdatem"]), int.Parse(Request["bdated"]));

            Korisnik newUser = new Korisnik(us, pass, fname, lname, gender, mail, dobs);

            Session["Korisnik"] = newUser;

            users.Add(newUser);

            Data d = new Data();
            d.saveUsers(users);

            return RedirectToAction("Index");
        }
        

        public ActionResult LogOut()
        {
            Session["Korisnik"] = null;

            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];
            Data d = new Data();
            d.saveUsers(users);

            return RedirectToAction("Index");
        }

        public ActionResult Komentarisi()
        {
            string name = Request["name"];
            List<FitnesCentar> fc = (List<FitnesCentar>)HttpContext.Application["a"];

            foreach (var a in fc)
                if (a.Naziv == name)
                    return View(a);

            Data d = new Data();
            d.saveFC(fc);

            return RedirectToAction("Index");
        }

        public ActionResult Izmena()
        {
            Korisnik help = (Korisnik)Session["Korisnik"];

            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];

            foreach (var u in users)
                if (u.Username == help.Username)
                {
                    u.Password = Request["pass"];
                    u.Ime = Request["fname"];
                    u.Prezime = Request["lname"];
                    u.Email = Request["mail"];
                    u.Pol = Request["pol"];


                    DateTime Dob = new DateTime(int.Parse(Request["bdatey"]), int.Parse(Request["bdatem"]), int.Parse(Request["bdated"]));

                    u.DatumRodjenja = Dob;


                }

            Data d = new Data();
            d.saveUsers(users);

            return RedirectToAction("Index");
        }

        public ActionResult IzmeniProfil()
        {
            string us = Request["us"];



            List<Korisnik> users = (List<Korisnik>)HttpContext.Application["users"];

            foreach (var u in users)
                if (u.Username == us)
                    return View(u);


            Data d = new Data();
            d.saveUsers(users);

            return RedirectToAction("Index");
        }
    }
}