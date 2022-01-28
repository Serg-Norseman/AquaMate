/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IBrandEditorView : IEditorView<Brand>
    {
        ITextBox NameField { get; }
        IComboBox CountryCombo { get; }
        ITextBox WebSiteField { get; }
        ITextBox EmailField { get; }
        ITextBox NoteField { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class BrandEditorPresenter : EditorPresenter<IModel, Brand, IBrandEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "BrandEditorPresenter");


        public BrandEditorPresenter(IBrandEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.NameField.Text = fRecord.Name;

                fView.CountryCombo.AddRange(fModel.QueryBrandCountries(), true);
                fView.CountryCombo.Text = fRecord.Country;

                fView.WebSiteField.Text = fRecord.WebSite;
                fView.EmailField.Text = fRecord.Email;
                fView.NoteField.Text = fRecord.Note;
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Name = fView.NameField.Text;
                fRecord.Country = fView.CountryCombo.Text;
                fRecord.WebSite = fView.WebSiteField.Text;
                fRecord.Email = fView.EmailField.Text;
                fRecord.Note = fView.NoteField.Text;

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }
    }
}
