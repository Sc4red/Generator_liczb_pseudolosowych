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
using OxyPlot;
using Microsoft.Win32;
using System.Data;

namespace Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
//Deklaracja obiektu

        public Window4()
        {
            InitializeComponent();
            //W konstruktorze
            this.Plot1.Model = new OxyPlot.PlotModel();
            policz();
            //for(int i=0;i<(Global.listaX.Count - 1); i++)
            //{
            //     List<double> listaXX = new List<double>();

            //     List<double> listaYY = new List<double>();

            //    listaXX.Add(Global.listaX[i]);
            //    listaXX.Add(Global.listaX[i+1]);

            //    listaYY.Add(Global.listaY[i]);
            //    listaYY.Add(Global.listaY[i]);

            //    PodajDaneDoWykresu(listaXX, listaYY);
            //}

            PodajDaneDoWykresu(Global.listaX, Global.listaY);


        }

        public Window4(string policz)
        {
            InitializeComponent();
            this.Plot1.Model = new OxyPlot.PlotModel();
            Global.zakres_od = Global.listaX[0];
            Global.zakres_do = Global.listaX[Global.listaX.Count-1];
            PodajDaneDoWykresu(Global.listaX, Global.listaY);
        }

        public void policz()
        {
            Global.listaX.Clear();
            Global.listaY.Clear();
            Generator gen = new Generator();
            Global.values = new double[Global.liczba_probek]; //os y
            for (var j = 0; j < Global.liczba_probek; j++) // os x
            {

                Global.values[j] = gen.wyznacz(Global.zakres_od-0.5, Global.zakres_do+.5);//trend;
                Global.SeriaDanych.Rows.Add(j, Global.values[j]);
            }
           
            for (int i = (int)Global.zakres_od; i <= Global.zakres_do; i++)
            {
                Global.listaX.Add(i);
            }

            Global.Dystrybuanta.Clear();



            for (int i = (int)Global.zakres_od; i <= Global.zakres_do; i++)
            {
                Global.Dystrybuanta.Rows.Add(i, 0);
            }
            int a = 0;
            for (int i = 0; i < Global.liczba_probek; i++)
            {
                a = int.Parse(Global.Dystrybuanta.Rows[(int)Global.values[i] - (int)Global.zakres_od]["Ile"].ToString());
                Global.Dystrybuanta.Rows[(int)Global.values[i] - (int)Global.zakres_od]["Ile"] = a + 1;
            }
           
            double[] values1 = new double[(int)Global.zakres_do - (int)Global.zakres_od];
            for (int i = 0; i <= Global.zakres_do - Global.zakres_od; i++)
            {
                //try { values1[i] = values1[i - 1] + int.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()); }
                //catch
                //{
                //    values1[i] = int.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString());
                //}
                try
                { Global.listaY.Add((double)Global.listaY[i - 1] + (double.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()) / (double)Global.liczba_probek));
                    
                }
                catch
                {
                    Global.listaY.Add(double.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()) / (double)Global.liczba_probek);
                    
                }
            }




        }

        OxyPlot.Series.LineSeries[] punktySerii = new OxyPlot.Series.LineSeries[(int)(Global.zakres_do - Global.zakres_od) + 2];  //zakres do - zakres od



        public void PodajDaneDoWykresu(List<double> X, List<double> Y)//Lista X i Y podana jako parametr metody
        {
            

            this.Plot1.Model = new OxyPlot.PlotModel();
            //Usunięcie ustawionych parametrów z poprzedniego uruchomienia metody
            this.Plot1.Model.Series = new System.Collections.ObjectModel.Collection<OxyPlot.Series.Series> { };
            this.Plot1.Model.Axes = new System.Collections.ObjectModel.Collection<OxyPlot.Axes.Axis> { };

            punktySerii = new OxyPlot.Series.LineSeries[(int)(Global.zakres_do - Global.zakres_od) + 2];  //zakres do - zakres od

            //Graficzne ustawienia wykresów
            for (int i = 0; i < ((Global.zakres_do - Global.zakres_od) + 2); i++)
            {
             
                punktySerii[i] = new OxyPlot.Series.LineSeries
                {
                    MarkerType = ksztaltPunktowWykresu[4], //oznaczenie punktów - definicja poniżej
                    MarkerSize = 4, //wielkość punktów
                    MarkerStroke = koloryWykresow[3], //Kolor linii wykresu - definicja poniżej
                    Title = "Seria nr: " + (i).ToString() //tytuł serii
                };
            }
   
            //Uzupełnianie danych

            // pierwszy
            {
                List<double> listaXX = new List<double>();

                List<double> listaYY = new List<double>();


                listaXX.Add(Global.listaX[0]-1);
                listaXX.Add(Global.listaX[0]);

                listaYY.Add(0);
                listaYY.Add(0);

                for (int n = 0; n < listaXX.Count; n++)
                    punktySerii[0].Points.Add(new OxyPlot.DataPoint(listaXX[n], listaYY[n]));//dodanie wszystkich serii do wykresu

                this.Plot1.Model.Series.Add(punktySerii[0]);
            }

            for (int i = 0; i < ((Global.zakres_do - Global.zakres_od)) ; i++)
            {

              
                    List<double> listaXX = new List<double>();

                    List<double> listaYY = new List<double>();
              
                 
                    listaXX.Add(Global.listaX[i]);
                    listaXX.Add(Global.listaX[i+1]);

                    listaYY.Add(Global.listaY[i]);
                    listaYY.Add(Global.listaY[i]);
                

                   
                


                    for (int n = 0; n < listaXX.Count; n++)
                    punktySerii[i+1].Points.Add(new OxyPlot.DataPoint(listaXX[n], listaYY[n]));//dodanie wszystkich serii do wykresu

                this.Plot1.Model.Series.Add(punktySerii[i+1]);
            }



            // ostatni
            {
                List<double> listaXX = new List<double>();

                List<double> listaYY = new List<double>();


                
                listaXX.Add(Global.listaX[Global.listaX.Count-1]);
                listaXX.Add(Global.listaX[Global.listaX.Count-1]+2);

                listaYY.Add(1);
                listaYY.Add(1);

                for (int n = 0; n < listaXX.Count; n++)
                    punktySerii[punktySerii.Length-1].Points.Add(new OxyPlot.DataPoint(listaXX[n], listaYY[n]));//dodanie wszystkich serii do wykresu

                this.Plot1.Model.Series.Add(punktySerii[punktySerii.Length-1]);
            }


            //Opis i parametry osi wykresu
            var xAxis = new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Bottom, "X") { MajorGridlineStyle = OxyPlot.LineStyle.Solid, MinorGridlineStyle = OxyPlot.LineStyle.Dot };
            this.Plot1.Model.Axes.Add(xAxis);
            var yAxis = new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Left, "Y") { MajorGridlineStyle = OxyPlot.LineStyle.Solid, MinorGridlineStyle = OxyPlot.LineStyle.Dot };
            this.Plot1.Model.Axes.Add(yAxis);
            
        }


  
        //Wypisane po to, by zmieniać kolor i kształt wraz z numerem klasy
        private readonly List<OxyPlot.OxyColor> koloryWykresow = new List<OxyPlot.OxyColor>
                                    {
                                        OxyPlot.OxyColors.Green,
                                        OxyPlot.OxyColors.IndianRed,
                                        OxyPlot.OxyColors.Coral,
                                        OxyPlot.OxyColors.Chartreuse,
                                        OxyPlot.OxyColors.Peru
                                    };
        private readonly List<OxyPlot.MarkerType> ksztaltPunktowWykresu = new List<OxyPlot.MarkerType>
                                            {
                                                OxyPlot.MarkerType.Plus,
                                                OxyPlot.MarkerType.Star,
                                                OxyPlot.MarkerType.Cross,
                                                OxyPlot.MarkerType.Custom,
                                                OxyPlot.MarkerType.Square
                                            };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Grafika (*.jpg)|*.jpg"
            };
            saveFileDialog.FileName = "Wykres " + DateTime.Now.ToString("dd/MM/yyyy");
            if (saveFileDialog.ShowDialog() == true)
            {
                OxyPlot.Wpf.PngExporter.Export(this.Plot1.Model, saveFileDialog.FileName, 1600, 1600, OxyPlot.OxyColors.White, 96);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
                nowa.Columns.Add("x", typeof(int));
                nowa.Columns.Add("y", typeof(int));
                nowa.Columns.Add("z", typeof(double));
                for (int i = 0; i <= Global.zakres_do - Global.zakres_od; i++)
                {
                    nowa.Rows[i]["x"] = Global.Dystrybuanta.Rows[i]["Wylosowana"];
                    nowa.Rows[i]["y"] = Global.Dystrybuanta.Rows[i]["Ile"];
                    nowa.Rows[i]["z"] = Global.listaY[i];
                }

                Global.CreateFile(nowa, saveFileDialog.FileName, (System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)) + "\\Excel\\Szablon.xlsx");

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var OpenFileDialog = new OpenFileDialog()
            {
                Filter = "Arkusz programu Microsoft Excel (*.xlsx)|*.xlsx"
            };
            if (OpenFileDialog.ShowDialog() == true)
            {
                ExcelReader r = new ExcelReader(OpenFileDialog.FileName);
                r.Process(Global.OnExcelCell);
            }
        }



    }



    //public class OxyPlotModel : INotifyPropertyChanged
    //{

    //    private OxyPlot.PlotModel plotModel;
    //    public OxyPlot.PlotModel PlotModel
    //    {
    //        get
    //        {
    //            return plotModel;
    //        }
    //        set
    //        {
    //            plotModel = value; OnPropertyChanged("PlotModel");
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void OnPropertyChanged(string name)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(name));
    //        }
    //    }
    //}

}
