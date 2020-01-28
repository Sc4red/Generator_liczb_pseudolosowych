using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;



namespace Projekt
{
    static class Global
    {
        public static int liczba_probek = 0;
        public static double[] values;
        public static double zakres_od= 0;
        public static double zakres_do = 0;
          public static double x = DateTimeOffset.Now.ToUnixTimeSeconds();
       // public static double x = 187;
        public static DataTable SeriaDanych = new DataTable();
        public static DataTable Dystrybuanta = new DataTable();

        public static void CreateFile(DataTable reportdata, string fileName, string excelTemplate)
        {
            var data = new ExelData
            {
                ReportData = reportdata
            };

            new ExcelWriter(excelTemplate).Export(data, fileName);
        }
        public static List<double> listaX = new List<double>();

        public static List<double> listaY = new List<double>();

        public static void OnExcelCell(char Column, int RowNumber, string value)
        {
    


            if (RowNumber <= 20)
            {
                try
                {
                    if (Column == 'C') Global.listaX.Add(double.Parse(value));
                    else if (Column == 'E') Global.listaY.Add(double.Parse(value));

                }
                catch
                {

                    //  Console.Write(Column + ": " + value.PadRight(40).Substring(0, 40) + "  ");
                    return;
                }
                return;
            }
           
            
        }
    }
}
