
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Wykres : Window
    {
        public Wykres()
        {
            InitializeComponent();
            List<Bar> _bar = new List<Bar>();
            policz();
            for(int i=0;i<Global.Dystrybuanta.Rows.Count;i++)
            _bar.Add(new Bar() { BarName = (i+1).ToString(), Value = Int32.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()) });
         
            this.DataContext = new RecordCollection(_bar);
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

            for(int i= (int)Global.zakres_od; i <= Global.zakres_do; i++)
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
                try { Global.listaY.Add((double)Global.listaY[i - 1] + (double.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()) / (double)Global.liczba_probek)); }
                catch { Global.listaY.Add(double.Parse(Global.Dystrybuanta.Rows[i]["Ile"].ToString()) / (double)Global.liczba_probek); }
            }




        }
    }

    class RecordCollection : ObservableCollection<Record>
    {

        public RecordCollection(List<Bar> barvalues)
        {
            Random rand = new Random();
            BrushCollection brushcoll = new BrushCollection();

            foreach (Bar barval in barvalues)
            {
                int num = rand.Next(brushcoll.Count / 3);
                Add(new Record(barval.Value, brushcoll[num], barval.BarName));
            }
        }

    }

    class BrushCollection : ObservableCollection<Brush>
    {
        public BrushCollection()
        {
            Type _brush = typeof(Brushes);
            PropertyInfo[] props = _brush.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                Brush _color = (Brush)prop.GetValue(null, null);
                if (_color != Brushes.LightSteelBlue && _color != Brushes.White &&
                     _color != Brushes.WhiteSmoke && _color != Brushes.LightCyan &&
                     _color != Brushes.LightYellow && _color != Brushes.Linen)
                    Add(_color);
            }
        }
    }

    class Bar
    {

        public string BarName { set; get; }

        public int Value { set; get; }

    }

    class Record : INotifyPropertyChanged
    {
        public Brush Color { set; get; }

        public string Name { set; get; }

        private int _data;
        public int Data
        {
            set
            {
                if (_data != value)
                {
                    _data = value;

                }
            }
            get
            {
                return _data;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Record(int value, Brush color, string name)
        {
            Data = value;
            Color = color;
            Name = name;
        }

        protected void PropertyOnChange(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }
    }
}
