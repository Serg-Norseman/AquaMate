/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Windows.Forms;
using AquaMate.UI.Components;
using BSLib.Design;
using BSLib.Design.Graphics;
using BSLib.Design.Handlers;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public class ZListItem : ListViewItem, IListItem
    {
        protected object fValue;

        public object Data { get; set; }

        public ZListItem()
        {
            fValue = null;
        }

        public ZListItem(object itemValue, object data)
        {
            fValue = itemValue;
            Text = ToString();
            Data = data;
        }

        public override string ToString()
        {
            return (fValue == null) ? string.Empty : fValue.ToString();
        }

        public int CompareTo(object obj)
        {
            ZListItem otherItem = obj as ZListItem;
            if (otherItem == null) {
                return -1;
            }

            //IComparable cv1 = fValue as IComparable;
            //IComparable cv2 = otherItem.fValue as IComparable;

            IComparable cv1 = this.Text as IComparable;
            IComparable cv2 = otherItem.Text as IComparable;

            int compRes;
            if (cv1 != null && cv2 != null)
            {
                compRes = cv1.CompareTo(cv2);
            }
            else if (cv1 != null)
            {
                compRes = -1;
            }
            else if (cv2 != null)
            {
                compRes = 1;
            }
            else {
                compRes = 0;
            }
            return compRes;
        }

        public void AddSubItem(object itemValue)
        {
            ZListSubItem subItem = new ZListSubItem(itemValue);
            SubItems.Add(subItem);
        }

        public void SetBackColor(IColor color)
        {
            var colorHandler = color as ColorHandler;
            if (colorHandler != null) {
                BackColor = colorHandler.Handle;
            }
        }

        public void SetForeColor(IColor color)
        {
            var colorHandler = color as ColorHandler;
            if (colorHandler != null) {
                ForeColor = colorHandler.Handle;
            }
        }

        public void SetFont(IFont font)
        {
            var fontHandler = font as FontHandler;
            if (fontHandler != null) {
                base.Font = fontHandler.Handle;
            }
        }
    }


    public class ZListSubItem : ListViewItem.ListViewSubItem, IComparable
    {
        protected object fValue;

        public ZListSubItem(object itemValue)
        {
            fValue = itemValue;
            Text = ToString();
        }

        public override string ToString()
        {
            return (fValue == null) ? string.Empty : fValue.ToString();
        }

        public int CompareTo(object obj)
        {
            ZListSubItem otherItem = obj as ZListSubItem;
            if (otherItem == null) {
                return -1;
            }

            if (fValue is string && otherItem.fValue is string) {
                return ZListView.StrCompareEx((string)fValue, (string)otherItem.fValue);
            }

            IComparable cv1 = fValue as IComparable;
            IComparable cv2 = otherItem.fValue as IComparable;

            int compRes;
            if (cv1 != null && cv2 != null)
            {
                compRes = cv1.CompareTo(cv2);
            }
            else if (cv1 != null)
            {
                compRes = -1;
            }
            else if (cv2 != null)
            {
                compRes = 1;
            }
            else {
                compRes = 0;
            }
            return compRes;
        }
    }

    public sealed class ZListViewItems : IListViewItems
    {
        private readonly ZListView fListView;

        public IListItem this[int index]
        {
            get { return (IListItem)fListView.Items[index]; }
        }

        public int Count
        {
            get { return fListView.Items.Count; }
        }

        public ZListViewItems(ZListView listView)
        {
            fListView = listView;
        }
    }

    public sealed class ZListViewHandler : BaseControlHandler<ZListView, ZListViewHandler>, IListView
    {
        private ZListViewItems fItems;

        public ZListViewHandler(ZListView control) : base(control)
        {
            fItems = new ZListViewItems(control);
        }

        public IListViewItems Items
        {
            get { return fItems; }
        }

        public int SortColumn
        {
            get { return Control.SortColumn; }
            set { Control.SortColumn = value; }
        }

        public int SelectedIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddColumn(string caption, int width, bool autoSize)
        {
        }

        public void AddColumn(string caption, int width, bool autoSize, BSDTypes.HorizontalAlignment textAlign)
        {
            Control.Columns.Add(caption, width, (HorizontalAlignment)textAlign);
        }

        public IListItem AddItem(object rowData, params object[] columnValues)
        {
            string[] strValues = Array.ConvertAll<object, string>(columnValues, Convert.ToString);
            return Control.AddItemEx<ZListItem>(rowData, strValues) as IListItem;
        }

        public void BeginUpdate()
        {
            Control.BeginUpdate();
        }

        public void EndUpdate()
        {
            Control.EndUpdate();
        }

        public void Clear()
        {
            Control.Clear();
        }

        public void ClearColumns()
        {
            Control.Columns.Clear();
        }

        public void ClearItems()
        {
            Control.Items.Clear();
        }

        public void DeleteRecord(object data)
        {
        }

        public object GetSelectedData()
        {
            return null;
        }

        public void SelectItem(object rowData)
        {
        }

        public void SetColumnCaption(int index, string caption)
        {
        }

        public void SetSortColumn(int sortColumn, bool checkOrder = true)
        {
            Control.SortColumn = sortColumn;
        }

        public void Sort(int sortColumn, BSLib.Design.BSDTypes.SortOrder sortOrder)
        {
            Control.Sort(sortColumn, (System.Windows.Forms.SortOrder)sortOrder);
        }

        public void UpdateContents(bool columnsChanged = false)
        {
        }

        public void AddCheckedColumn(string caption, int width, bool autoSize = false)
        {
        }

        public IListItem AddItem(object rowData, bool isChecked, params object[] columnValues)
        {
            return null;
        }
    }
}
