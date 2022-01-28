/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System.Windows.Forms;
using ExcelLibrary.SpreadSheet;

namespace AquaMate.Core.Export
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExcelExporter
    {
        public static void Generate(ListView listView, string fileName)
        {
            if (listView == null || string.IsNullOrEmpty(fileName)) return;

            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("First Sheet");

            try {
                for (int i = 0; i < listView.Columns.Count; i++) {
                    ColumnHeader columnHeader = listView.Columns[i];
                    worksheet.Cells[0, i] = new Cell(columnHeader.Text);
                }

                int num = listView.Items.Count;
                for (int i = 0; i < num; i++) {
                    ListViewItem item = listView.Items[i];
                    int row = i + 1;

                    for (int k = 0; k < item.SubItems.Count; k++) {
                        string val = item.SubItems[k].Text;
                        worksheet.Cells[row, k] = new Cell(val);
                    }
                }

                workbook.Worksheets.Add(worksheet);
            } finally {
                workbook.Save(fileName);
            }
        }
    }
}
