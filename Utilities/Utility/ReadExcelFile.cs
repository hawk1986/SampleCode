using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab

namespace Utilities.Utility
{
    public class ReadExcelFile
    {
        public static List<LostAndFoundExcelModel> getExcelFile(string path)
        {
            var temp = new List<LostAndFoundExcelModel>();
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                var _temp = new LostAndFoundExcelModel();
                var cell1 = string.Empty;
                var cell2 = string.Empty;
                var cell3 = string.Empty;
                var cell4 = string.Empty;
                for (int j = 1; j <= colCount; j++)
                {
                    if (i > 1)
                    {

                        //new line
                        if (j == 1)
                            Console.Write("\r\n");

                        //write the value to the console
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

                        var cell = xlRange.Cells[i, j];
                        if (j == 1)
                            cell1 = cell.text;
                        if (j == 2)
                            cell2 = cell.text;
                        if (j == 3)
                            cell3 = cell.text;
                        if (j == 4)
                            cell4 = cell.text;

                    }
                }
                if (!string.IsNullOrWhiteSpace(cell1))
                {
                    _temp.RegNo = cell1;
                    _temp.Date = cell2;
                    _temp.Item = cell3;
                    _temp.Location = cell4;
                    temp.Add(_temp);
                }
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            return temp;
        }

        public class LostAndFoundExcelModel
        {
            public string RegNo { get; set; }
            public string Date { get; set; }
            public string Item { get; set; }
            public string Location { get; set; }
        }
    }
}