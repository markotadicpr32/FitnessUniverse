using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace FitnessUniverse.Models
{
    public class Data
    {
        private string userFile;
        private string fcFile;

        public Data()
        {
            userFile = HostingEnvironment.MapPath("~/AppData/users.txt");
            fcFile = HostingEnvironment.MapPath("~/AppData/fc.txt");
        }

        public void saveFC(List<FitnesCentar> fc)
        {
            string text = "";

            foreach (var a in fc)
            {
                string del = "Y";
                if (!a.IsDeleted) del = "N";

                text = text + a.Naziv + "|" + a.Adresa + "|" + a.GodinaOtvaranja.ToShortDateString() + "|" + a.CenaMesec + "|" + a.CenaGodina + "|" + a.CenaTrening + "|" + a.CenaGrupni + "|" + a.CenaPersonal + "|" + del + "|" + a.Vlasnik + "|";

                foreach (var c in a.Komentari)
                {
                    string delll = "N";
                    string apr = "N";
                    if (c.IsDeleted) delll = "N";
                    if (c.IsApproved) apr = "Y";
                    text = text + c.Posetilac + "~" + c.ID + "~" + c.Ocena + "~" + c.Tekst + "~" + c.Centar + "~" + delll + "~" + apr + "#";
                }

                text = text + "\n";
            }

            StreamWriter sw = new StreamWriter(fcFile, false, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();

        }

        public List<FitnesCentar> readFC()
        {
            StreamReader sr = new StreamReader(fcFile);

            List<FitnesCentar> ret = new List<FitnesCentar>();
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line != "" && line != "\n")
                {
                    string[] lines = line.Split('|');

                    DateTime d = new DateTime(int.Parse(lines[2]), 1, 1);


                    FitnesCentar a = new FitnesCentar(lines[0], lines[1], d, int.Parse(lines[3]), int.Parse(lines[4]), int.Parse(lines[5]), int.Parse(lines[6]), int.Parse(lines[7]));

                    if (lines[8] == "Y")
                        a.IsDeleted = true;
                    else
                        a.IsDeleted = false;

                    a.Vlasnik = lines[9];

                    foreach (var r in lines[10].Split('#'))
                    {
                        if (r != "" && r != "\n")
                        {
                            string[] cs = r.Split('~');

                            bool del = false;
                            bool spr = false;
                            if (cs[5] == "Y") del = true;
                            if (cs[6] == "Y") spr = true;


                            Komentar c = new Komentar(cs[0], cs[4], cs[3], int.Parse(cs[2]));
                            c.ID = int.Parse(cs[1]);
                            c.IsDeleted = del;
                            c.IsApproved = spr;


                            a.Komentari.Add(c);
                        }
                    }


                    ret.Add(a);

                }
                line = sr.ReadLine();
            }


            return ret;

        }

        public void saveUsers(List<Korisnik> users)
        {
            string text = "";


            foreach (var s in users)
            {

                string del;
                if (s.IsDeleted) del = "Y"; else del = "N";

                text = text + s.Username + "|" + s.Password + "|" + s.Ime + "|" + s.Prezime + "|" + s.Uloga + "|" + s.Email + "|" + s.Cancels + "|" + s.DatumRodjenja.ToShortDateString() + "|" + s.Pol + "|" + del + "|";
            }

            StreamWriter sw = new StreamWriter(userFile, false, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();

        }


        public List<Korisnik> readUser(List<FitnesCentar> fc)
        {
            StreamReader sr = new StreamReader(userFile);

            List<Korisnik> ret = new List<Korisnik>();
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line != "" && line != "\n")
                {
                    string[] lines = line.Split('|');

                    DateTime dob = new DateTime(int.Parse(lines[7].Split('/')[2]), int.Parse(lines[7].Split('/')[0]), int.Parse(lines[7].Split('/')[1]));


                    Korisnik u = new Korisnik(lines[0], lines[1], lines[2], lines[3], lines[8], lines[5], dob);

                    u.Uloga = lines[4];
                    u.Cancels = int.Parse(lines[6]);
                    bool del = false;
                    if (lines[9] == "Y") del = true;
                    u.IsDeleted = del;

                    ret.Add(u);

                }
                line = sr.ReadLine();
            }
            return ret;
        }

        public void saveGT(List<GrupniTrening> gt)
        {
            string text = "";


            foreach (var s in gt)
            {

                string del;
                if (s.IsDeleted) del = "Y"; else del = "N";

                text = text + s.Naziv + "|" + s.Tip + "|" + s.Fc + "|" + s.Trajanje + "|" + s.DatumVreme.ToShortDateString() + "|" + s.MaxPosetioci + "|" + del + "|";
            }

            StreamWriter sw = new StreamWriter(userFile, false, Encoding.ASCII);
            sw.WriteLine(text);
            sw.Close();

        }
    }
}