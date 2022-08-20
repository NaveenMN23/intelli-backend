using ClosedXML.Excel;
using System.Data;

namespace IntelliCRMAPIService.BL
{
    public class ExcelConverter
    {
        public async Task<DataTable> ConvertToDataTable(Stream stream)
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable();
                using (XLWorkbook workBook = new XLWorkbook(stream))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;
                            if (row.FirstCellUsed() != null)
                            {
                                foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return dt;
            }

            return dt;
        }
    }
}
