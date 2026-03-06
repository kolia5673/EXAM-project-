using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ShopServer.Models;

namespace ShopServer.Services
{
    public static class ExcelLoader
    {
        public static List<Product> LoadFromXlsx(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Файл Excel не знайдено", path);

            using var fs = File.OpenRead(path);
            IWorkbook workbook = new XSSFWorkbook(fs);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null) throw new Exception("В Excel немає аркушів");

            var headerRow = sheet.GetRow(0) ?? throw new Exception("В Excel відсутній заголовок");
            int cols = headerRow.LastCellNum;
            int colName = -1, colPrice = -1, colQty = -1;
            for (int c = 0; c < cols; c++)
            {
                var cell = headerRow.GetCell(c);
                if (cell == null) continue;
                string h = cell.ToString().Trim();
                if (string.Equals(h, "Найменування", StringComparison.InvariantCultureIgnoreCase)) colName = c;
                if (string.Equals(h, "Ціна", StringComparison.InvariantCultureIgnoreCase)) colPrice = c;
                if (string.Equals(h, "Кількість", StringComparison.InvariantCultureIgnoreCase)) colQty = c;
            }
            if (colName == -1 || colPrice == -1 || colQty == -1)
                throw new Exception("В Excel відсутні потрібні стовпці: Найменування, Ціна, Кількість");

            var result = new List<Product>();
            for (int r = 1; r <= sheet.LastRowNum; r++)
            {
                var row = sheet.GetRow(r);
                if (row == null) continue;
                var nameCell = row.GetCell(colName);
                if (nameCell == null) continue;
                string name = nameCell.ToString().Trim();
                if (string.IsNullOrEmpty(name)) continue;

                decimal price = 0;
                int qty = 0;
                try
                {
                    var pc = row.GetCell(colPrice);
                    if (pc != null)
                    {
                        if (pc.CellType == CellType.Numeric) price = Convert.ToDecimal(pc.NumericCellValue);
                        else decimal.TryParse(pc.ToString(), out price);
                    }
                }
                catch { price = 0; }

                try
                {
                    var qc = row.GetCell(colQty);
                    if (qc != null)
                    {
                        if (qc.CellType == CellType.Numeric) qty = Convert.ToInt32(qc.NumericCellValue);
                        else int.TryParse(qc.ToString(), out qty);
                    }
                }
                catch { qty = 0; }

                result.Add(new Product { Найменування = name, Ціна = price, Кількість = qty });
            }

            return result;
        }
    }
}