using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class GrupniTrening
    {
        public GrupniTrening(string naziv, string tip, FitnesCentar fc, int trajanje, DateTime datumVreme, int maxPosetioci)
        {
            Naziv = naziv;
            Tip = tip;
            Fc = fc;
            Trajanje = trajanje;
            DatumVreme = datumVreme;
            MaxPosetioci = maxPosetioci;
            SpisakPosetioca = new List<Korisnik>();
            IsDeleted = false;
        }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public FitnesCentar Fc { get; set; }
        public int Trajanje { get; set; }
        public DateTime DatumVreme { get; set; }
        public int MaxPosetioci { get; set; }
        public List<Korisnik> SpisakPosetioca { get; set; }
        public bool IsDeleted { get; set; }
    }
}