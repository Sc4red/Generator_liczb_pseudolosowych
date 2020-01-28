using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Geared;
using System.Data;
using Microsoft.Win32;

namespace Projekt
{
   
    /// <summary>
    /// Logika interakcji dla klasy Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public int liczba_probek;
     //   public MultipleSeriesVm x;
        public Window3()
        {
            InitializeComponent();
         //   x = new MultipleSeriesVm();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Arkusz programu Microsoft Excel (*.xlsx)|*.xlsx"
            };
            saveFileDialog.FileName = "Seria Danych " + DateTime.Now.ToString("dd/MM/yyyy");
            if (saveFileDialog.ShowDialog() == true)
            {
                DataTable nowa = new DataTable();
                nowa = Global.SeriaDanych.Copy();
                nowa.Columns.Add("x",typeof(int));
                nowa.Columns.Add("y",typeof(int));
                for(int i = 0; i <= Global.zakres_do- Global.zakres_od; i++)
                {
                    nowa.Rows[i]["x"] = Global.Dystrybuanta.Rows[i]["Wylosowana"];
                    nowa.Rows[i]["y"] = Global.Dystrybuanta.Rows[i]["Ile"];
                }

                    Global.CreateFile(nowa, saveFileDialog.FileName, (System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)) + "\\Excel\\Szablon.xlsx");
                
            }

          
        
        }
    }

//}


//namespace Geared.Wpf.MultipleSeriesTest
//{
   // using Projekt;
    public class MultipleSeriesVm
    {
     

        //public void save()
        //{ string text = string.Empty;
        //    for (int i = 0; i < Global.liczba_probek; i++) text += Global.values[i].ToString() +"_";


        //  System.IO.File.WriteAllText(string.Format((System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\WriteLines.txt")), text);

        //}
    
        public MultipleSeriesVm()
        {
           
            Series = new SeriesCollection();

            Generator gen = new Generator();
           Global.values = new double[Global.liczba_probek]; //os y
            for (var j = 0; j < Global.liczba_probek; j++) // os x
            {

                 Global.values[j] = gen.wyznacz(Global.zakres_od, Global.zakres_do);//trend;
                Global.SeriaDanych.Rows.Add(j, Global.values[j]);
            }


           
            Global.Dystrybuanta.Clear();
            
          

            for(int i = (int)Global.zakres_od; i <= Global.zakres_do; i++)
            {
                Global.Dystrybuanta.Rows.Add(i, 0);
            }
            int a = 0;
            for(int i = 0; i < Global.liczba_probek; i++)
            {
                a = int.Parse(Global.Dystrybuanta.Rows[(int)Global.values[i] - (int)Global.zakres_od]["Ile"].ToString());
                Global.Dystrybuanta.Rows[(int)Global.values[i] - (int)Global.zakres_od]["Ile"] = a + 1;
            }

            double[] values1 = new double[(int)Global.zakres_do-(int)Global.zakres_od];
            for(int i=0;i< Global.zakres_do - Global.zakres_od; i++)
            {
                try { values1[i] = values1[i-1] + int.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()); }
                catch
                {
                    values1[i] = int.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString());
                }
            }



            var series = new GLineSeries
                {
                    Values = values1.AsGearedValues().WithQuality(Quality.Low),
                    Fill = Brushes.Transparent,
                    StrokeThickness = .5,
                    PointGeometry = null //use a null geometry when you have many series
                };
                Series.Add(series);
            
            
        }

        public SeriesCollection Series { get; set; }
    }
}

