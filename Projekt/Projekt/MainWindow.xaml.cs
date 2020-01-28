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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Drawing;



namespace Projekt
{


    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BitmapImage bitmapImage = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)+"\\obrazek.jpg"));
            ImageQr.Source = bitmapImage;
            Global.Dystrybuanta.Columns.Add("Wylosowana",typeof(int));
            Global.Dystrybuanta.Columns.Add("Ile",typeof(int));
            Global.SeriaDanych.Columns.Add("Id",typeof(int));
            Global.SeriaDanych.Columns.Add("Liczba",typeof(int));
      

        }
        Generator gen = new Generator();
    
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
     
        }
        public bool pierwszy = true;
        double wartosc = 0;
        int i = 0;

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            i++;
            wartosc = gen.wyznacz(double.Parse(Generator1TextBoxOd.Text.ToString()), double.Parse(Generator1TextBoxDo.Text.ToString()));
            TextBox1.Text +="  " + i.ToString()+ ")"  + wartosc.ToString();

            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;


            ImageQr.Source = metoda.ToWpfImage(qrcode.Draw(wartosc.ToString(), (50)));

          
        }



        private void ButtonQRGen_Click(object sender, RoutedEventArgs e)
        {

           
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;


            ImageQr.Source = metoda.ToWpfImage(qrcode.Draw(TextBoxLinkQR.Text, (50)));



        }

        private void WlButton_Click(object sender, RoutedEventArgs e)
        {
            Wykres wyk = new Wykres();
              wyk.ShowDialog();
        }

        private void WykButt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.liczba_probek = Int32.Parse(Liczba_probek_textbox.Text.ToString());
                Global.zakres_od = Int32.Parse(Zakres_od_textbox.Text.ToString());
                Global.zakres_do = Int32.Parse(Zakres_do_textbox.Text.ToString());
                Window3 wyk = new Window3();
                wyk.ShowDialog();
            }catch(Exception ex)
            {
                MessageBox.Show("Wypelnij wszystkie pola" + ex.Message);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

          
                try
                {
                    var saveFileDialog = new SaveFileDialog()
                    {
                        Filter = "Image Files (*.bmp, *.png, *.jpg)|*.bmp;*.png;*.jpg"
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {

                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)ImageQr.Source));
                        using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                            encoder.Save(stream);
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
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

            Window4 wyk = new Window4("tak");
            wyk.ShowDialog();

        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private void ButtonOxy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.liczba_probek = Int32.Parse(Liczba_probek_textbox.Text.ToString());
                Global.zakres_od = Int32.Parse(Zakres_od_textbox.Text.ToString());
                Global.zakres_do = Int32.Parse(Zakres_do_textbox.Text.ToString());
                Window4 wyk = new Window4();
                wyk.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wypelnij wszystkie pola" + ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.liczba_probek = Int32.Parse(Liczba_probek_textbox.Text.ToString());
                Global.zakres_od = Int32.Parse(Zakres_od_textbox.Text.ToString());
                Global.zakres_do = Int32.Parse(Zakres_do_textbox.Text.ToString());
                Wykres wyk = new Wykres();
                wyk.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wypelnij wszystkie pola" + ex.Message);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                MessagingToolkit.QRCode.Codec.QRCodeDecoder dekoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
                TextBoxLinkQR.Text =  dekoder.Decode(new MessagingToolkit.QRCode.Codec.Data.QRCodeBitmapImage(BitmapImage2Bitmap(bitmapImage)));
                ImageQr.Source = bitmapImage;
            }
        }
    }


    static class metoda
    {
        public static BitmapImage ToWpfImage(this System.Drawing.Image img)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage ix = new BitmapImage();
            ix.BeginInit();
            ix.CacheOption = BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit();
            return ix;
        }
    }
    class Generator
    {     
        public bool pierwszy = true;
        public long _x; //Poprzednio wyloswana liczba
        public long y;  //czas pobrany z systemu

        int wsp_q = 127773;
        int wsp_r = 2836;
        int zakres = 2147483647;


        //public Int64 wyznacz(long a, long b)
        //{
        //    //Int64 x;

        //    // if (pierwszy)   //losowanie pierwszej liczby
        //    // {
        //    //     //y = DateTimeOffset.Now.ToUnixTimeSeconds();
        //    //     y = 16807;
        //    //     x = (y + 1) % (b - a + 1) + a; ;
        //    //     _x = x;
        //    //     pierwszy = false;
        //    //     return x;
        //    // }   //losowanie kolejnych
        //    // else { x = (y * _x + 1) % (b - a + 1) + a; _x = x; return x; }




           

        //}

        double rozk_jed()
        {
            int h = (int)((Global.x) / wsp_q);                                              //H =ziarno / 127773
            (Global.x) = 16807 * ((Global.x) - wsp_q * h) - wsp_r * h;                      //X = 16807 * ((ziarno - (127773 *H)) - (2836*H)
            if ((Global.x) < 0) (Global.x) += zakres;                                       // (ziarno<0) ziarno+2147483647
            return (double)Global.x / (double)zakres;                                       // ziarno/zakres
        }


      public  int wyznacz(double start, double koniec)
        {
            double b = start + (koniec - start) * rozk_jed();

            return (int)Math.Round(b);
           // return (int)b;
        }

    }


    

}
