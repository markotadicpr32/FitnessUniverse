using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class Korisnik
    {
        public Korisnik(string username, string password, string ime, string prezime, string pol, string email, DateTime datumRodjenja)
        {
            Username = username;
            Password = password;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = "Posetilac";
            GTPrijavljen = new List<GrupniTrening>();
            GTAngazovan = new List<GrupniTrening>();
            //FCAngazovan = fcangazovan;
            FCVlasnik = new List<FitnesCentar>();
            IsDeleted = false;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string Email { get; set; }
        public string Uloga { get; set; }
        public List<GrupniTrening> GTPrijavljen { get; set; }
        public List<GrupniTrening> GTAngazovan { get; set; }
        //public FitnesCentar FCAngazovan { get; set; }
        public List<FitnesCentar> FCVlasnik { get; set; }
        public bool IsDeleted { get; set; }

    }
}