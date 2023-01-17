using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUniverse.Models
{
    public class Komentar
    {
        public Komentar(string posetilac, string centar, string tekst, int ocena)
        {
            Posetilac = posetilac;
            Centar = centar;
            Tekst = tekst;
            Ocena = ocena;
            IsDeleted = false;
            IsApproved = false;
        }
        public string Posetilac { get; set; }
        public int ID { get; set; }
        public string Centar { get; set; }
        public string Tekst { get; set; }
        public int Ocena { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsApproved { get; set; }
    }
}