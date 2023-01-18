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
            GTPrijavljen = new List<PrijavaTrening>();
            GTAngazovan = new List<AngazovanjeTrening>();
            //FCAngazovan = fcangazovan;
            FCVlasnik = new List<FitnesCentar>();
            Cancels = 0;
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
        public List<PrijavaTrening> GTPrijavljen { get; set; }
        public List<AngazovanjeTrening> GTAngazovan { get; set; }
        public List<FitnesCentar> FCVlasnik { get; set; }
        public bool IsDeleted { get; set; }
        public int Cancels { get; set; }
    }
}