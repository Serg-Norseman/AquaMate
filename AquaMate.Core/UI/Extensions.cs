/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using BSLib;
using BSLib.Design.Graphics;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        public static double GetDecimalVal(this ITextBox textBox, double defaultValue = 0.0d)
        {
            string strVal = textBox.Text;
            return ConvertHelper.ParseFloat(strVal, defaultValue, true);
        }

        public static void SetDecimalVal(this ITextBox textBox, double value, int decimalDigits = 2, bool hideZero = false)
        {
            if (double.IsNaN(value) || (value == 0.0d && hideZero)) {
                textBox.Text = string.Empty;
                return;
            }

            string fmt = "0.".PadRight(2 + decimalDigits, '0');
            string strVal = value.ToString(fmt, ALCore.SQLITE_NFI);
            textBox.Text = strVal;
        }


        public static DateTime GetCheckedDate(this IDateTimeBox textBox)
        {
            return textBox.Checked ? textBox.Value : new DateTime(0);
        }

        public static void SetCheckedDate(this IDateTimeBox textBox, DateTime value)
        {
            textBox.Checked = !ALCore.IsZeroDate(value);
            if (textBox.Checked) {
                textBox.Value = value;
            }
        }


        public static void SetForeColor(this IListItem listItem, int color)
        {
            if (listItem == null) return;

            var colorHandler = AppHost.GfxProvider.CreateColor(color);
            listItem.SetForeColor(colorHandler);
        }

        public static void SetBackColor(this IListItem listItem, int color)
        {
            if (listItem == null) return;

            var colorHandler = AppHost.GfxProvider.CreateColor(color);
            listItem.SetBackColor(colorHandler);
        }
    }
}
