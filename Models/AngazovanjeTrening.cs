using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class AngazovanjeTrening
    {
        public AngazovanjeTrening(Korisnik angaz, FitnesCentar izabraniFC, GrupniTrening izabraniTr)
        {
            Angaz = angaz;
            IzabraniFC = izabraniFC;
            IzabraniTr = izabraniTr;
            Id = angaz.Username + IzabraniFC.Naziv + IzabraniTr.num.ToString();
            IsActive = false;
        }

        public string Id { get; set; }
        public Korisnik Angaz { get; set; }
        public bool IsActive { get; set; }
        public FitnesCentar IzabraniFC { get; set; }
        public GrupniTrening IzabraniTr { get; set; }
    }
}