/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface INutritionEditorView : IEditorView<Nutrition>
    {
        ITextBox NameField { get; }
        IComboBox BrandCombo { get; }
        ITextBox AmountField { get; }
        ITextBox NoteField { get; }
        IComboBox StateCombo { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class NutritionEditorPresenter : EditorPresenter<IModel, Nutrition, INutritionEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "NutritionEditorPresenter");


        public NutritionEditorPresenter(INutritionEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.NameField.Text = fRecord.Name;

                fView.BrandCombo.AddRange(fModel.QueryNutritionBrands(), true);
                fView.BrandCombo.Text = fRecord.Brand;

                fView.AmountField.SetDecimalVal(fRecord.Amount);
                fView.NoteField.Text = fRecord.Note;
                SetState(fRecord.State);
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Name = fView.NameField.Text;
                fRecord.Brand = fView.BrandCombo.Text;
                fRecord.Amount = (float)fView.AmountField.GetDecimalVal();
                fRecord.Note = fView.NoteField.Text;
                fRecord.State = fView.StateCombo.GetSelectedTag<ItemState>();

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }

        private void SetState(ItemState value)
        {
            var namesList = ALData.GetItemStateNamesList(ItemType.Nutrition);
            fView.StateCombo.AddRange(namesList, false);
            fView.StateCombo.SetSelectedTag<ItemState>(value);
        }
    }
}
