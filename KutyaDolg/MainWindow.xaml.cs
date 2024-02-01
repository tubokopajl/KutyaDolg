using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using KutyaDolg.Models;
using Microsoft.Win32;
namespace KutyaDolg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        private readonly KutyaContext _context;
        public MainWindow()
        {
            _context = new KutyaContext();
            _context.Database.EnsureCreated();
            InitializeComponent();
            CountDb();
            FillcbKutyaFajta();
        }

        void Import(string path)
        {
            List<Kutya> kutyak = new();
            //read and skip first line
            var sorok = File.ReadAllLines(path, Encoding.UTF8).Skip(1).ToList();
            foreach (var s in sorok)
            {
                string[] sor = s.Split(',');
                int id = int.Parse(sor[0]);

                if (!_context.kutyak.Any(k => k.Id == id))
                {
                    kutyak.Add(new Kutya
                    {
                        Id = id,
                        Nev = sor[1],
                        Fajta = sor[2],
                        Efajta = sor[3],
                        Eletkor = int.Parse(sor[4]),
                        OrvostLatogatott = DateTime.Parse(sor[5])
                    });
                }
            }

            _context.kutyak.AddRange(kutyak);
            _context.SaveChanges();
        }

        void CountDb()
        {
            lbKutyakSzama.Content = _context.kutyak.Count();
        }

        private void btnKutyakFeltoltese_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "CSV fájlok|*.csv|Minden fájl|*.*";
            if (ofd.ShowDialog() == true)
            {
                Import(ofd.FileName);
            }
            CountDb();
            
        }

        private void cbKutyaFajta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFajta = cbKutyaFajta.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedFajta))
            {
                var selectedKutyak = _context.kutyak.Where(k => k.Fajta == selectedFajta).ToList();
                lbKutyak.ItemsSource = selectedKutyak.Select(k => $"{k.Id} - {k.Nev} ({k.Eletkor} éves)").ToList();
            }
            {
                var selectedKutyak = _context.kutyak.Where(k => k.Fajta == selectedFajta).ToList();
                lbKutyak.ItemsSource = selectedKutyak.Select(k => $"{k.Id} - {k.Nev} ({k.Eletkor} éves)").ToList();
            }
        }

        void FillcbKutyaFajta()
        {
            cbKutyaFajta.ItemsSource = _context.kutyak.Select(k => k.Fajta).Distinct().ToList();
        }

        private void tbOrvosnalRegenJart_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            //kapott értéknél régebben járt orvosnál
            int nemLatogatottNapokSzama = int.TryParse(tbOrvosnalRegenJart.Text, out int napokSzama) ? napokSzama : 0;
            if(nemLatogatottNapokSzama > 0)
            {
                var selectedKutyak = _context.kutyak.AsEnumerable()
                    .Where(k => (DateTime.Now - k.OrvostLatogatott).TotalDays > nemLatogatottNapokSzama)
                    .ToList();
                lbOrvosnalRegenJart.ItemsSource = selectedKutyak.Select(k => $"{k.Id} - {k.Nev} ({k.Eletkor} éves)").ToList();
            }
           
        }
    }
}