using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutyaDolg.Models
{
    internal class KutyaContext:DbContext
    {
        public readonly string connStr = "server=localhost;userid=root;password=;database=kutya";

        public DbSet<Kutya> kutyak {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        }
    }
}
