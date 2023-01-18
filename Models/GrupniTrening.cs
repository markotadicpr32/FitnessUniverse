using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class GrupniTrening
    {
        public GrupniTrening(int num, string naziv, string tip, int trajanje, DateTime datumVreme, int maxPosetioci, bool zauzeto)
        {
            this.num = num;
            Naziv = naziv;
            Tip = tip;
            Trajanje = trajanje;
            DatumVreme = datumVreme;
            MaxPosetioci = maxPosetioci;
            Posetioci = 0;
            SpisakPosetioca = new List<Korisnik>();
            GrupniTreninzi = new List<GrupniTrening>();
            IsDeleted = false;
        }
        public int num { get; set; }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public string Fc { get; set; }
        public int Trajanje { get; set; }
        public DateTime DatumVreme { get; set; }
        public int Posetioci { get; set; }
        public int MaxPosetioci { get; set; }
        public bool Zauzeto { get; set; }
        public List<GrupniTrening> GrupniTreninzi { get; set; }
        public List<Korisnik> SpisakPosetioca { get; set; }
        public bool IsDeleted { get; set; }
    }
}