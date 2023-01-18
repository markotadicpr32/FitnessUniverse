using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class PrijavaTrening
    {
        public PrijavaTrening(Korisnik prijavljivac, FitnesCentar izabraniFC, GrupniTrening izabraniTr)
        {
            Prijavljivac = prijavljivac;
            IzabraniFC = izabraniFC;
            IzabraniTr = izabraniTr;
            Id = prijavljivac.Username + IzabraniFC.Naziv + IzabraniTr.num.ToString();
            IsActive = false;
        }

        public string Id { get; set; }
        public Korisnik Prijavljivac { get; set; }
        public bool IsActive { get; set; }
        public FitnesCentar IzabraniFC { get; set; }
        public GrupniTrening IzabraniTr { get; set; }
    }
}