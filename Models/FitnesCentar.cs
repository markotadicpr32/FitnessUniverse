using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class FitnesCentar
    {
        public FitnesCentar(string naziv, string adresa, DateTime godinaOtvaranja, int cenaMesec, int cenaGodina, int cenaTrening, int cenaGrupni, int cenaPersonal)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            CenaMesec = cenaMesec;
            CenaGodina = cenaGodina;
            CenaTrening = cenaTrening;
            CenaGrupni = cenaGrupni;
            CenaPersonal = cenaPersonal;
            IsDeleted = false;
            GrupniTreninzi = new List<GrupniTrening>();
            Komentari = new List<Komentar>();
        }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public DateTime GodinaOtvaranja { get; set; }
        public string Vlasnik { get; set; }
        public int CenaMesec { get; set; } 
        public int CenaGodina { get; set; }
        public int CenaTrening { get; set; }
        public int CenaGrupni { get; set; }
        public int CenaPersonal { get; set; }
        public bool IsDeleted { get; set; }
        public GrupniTrening Grupni { get; set; }
        public List<GrupniTrening> GrupniTreninzi { get; set; }
        public List<Komentar> Komentari { get; set; }
    }
}