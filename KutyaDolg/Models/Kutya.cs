using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutyaDolg.Models
{
    internal class Kutya
    {
        public int Id { get; set; }

        public string Nev { get; set; }

        public string Fajta { get; set; }

        public string Efajta { get; set; }

        public int Eletkor { get; set; }

        public DateTime OrvostLatogatott  { get; set; }
    }
}
