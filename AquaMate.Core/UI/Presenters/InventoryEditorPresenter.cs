/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IInventoryEditorView : IEditorView<Inventory>
    {
        ITextBox NameField { get; }
        IComboBox BrandCombo { get; }
        IComboBox TypeCombo { get; }
        ITextBox NoteField { get; }
        IComboBox StateCombo { get; }
        IPropertyGrid PropsGrid { get; }
    }


    public class InventoryEditorPresenter : EditorPresenter<IModel, Inventory, IInventoryEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "InventoryEditorPresenter");


        public InventoryEditorPresenter(IInventoryEditorView view) : base(view)
        {
            var inventoryTypesList = ALData.GetNamesList<InventoryType>(ALData.InventoryTypes);
            fView.TypeCombo.AddRange(inventoryTypesList, true);
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.NameField.Text = fRecord.Name;

                fView.BrandCombo.AddRange(fModel.QueryInventoryBrands(), true);
                fView.BrandCombo.Text = fRecord.Brand;

                fView.TypeCombo.SetSelectedTag(fRecord.Type);
                fView.NoteField.Text = fRecord.Note;

                SetState(ALCore.GetItemType(fRecord.Type), fRecord.State);
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Name = fView.NameField.Text;
                fRecord.Brand = fView.BrandCombo.Text;
                fRecord.Type = fView.TypeCombo.GetSelectedTag<InventoryType>();
                fRecord.Note = fView.NoteField.Text;
                fRecord.State = fView.StateCombo.GetSelectedTag<ItemState>();

                fRecord.Properties = fView.PropsGrid.SelectedObject as IInventoryProperties;

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }

        private void SetState(ItemType itemType, ItemState itemState)
        {
            var namesList = ALData.GetItemStateNamesList(itemType);
            fView.StateCombo.AddRange(namesList, false);
            fView.StateCombo.SetSelectedTag<ItemState>(itemState);
        }

        public void ChangeSelectedType()
        {
            InventoryType invType = fView.TypeCombo.GetSelectedTag<InventoryType>();

            SetState(ALCore.GetItemType(invType), fRecord.State);

            if (invType >= 0) {
            }

            IInventoryProperties props = fRecord.GetProperties(invType, fRecord.RawProperties);
            if (props != null) {
                props.SetPropNames();
            }
            fView.PropsGrid.SelectedObject = props;
        }
    }
}
