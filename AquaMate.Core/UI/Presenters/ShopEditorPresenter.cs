/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IShopEditorView : IEditorView<Shop>
    {
        ITextBox NameField { get; }
        ITextBox AddressField { get; }
        ITextBox TelephoneField { get; }
        ITextBox WebSiteField { get; }
        ITextBox EmailField { get; }
        ITextBox NoteField { get; }
    }


    public class ShopEditorPresenter : EditorPresenter<IModel, Shop, IShopEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "ShopEditorPresenter");


        public ShopEditorPresenter(IShopEditorView view) : base(view)
        {
        }

        public override void UpdateView()
        {
            if (fRecord != null) {
                fView.NameField.Text = fRecord.Name;
                fView.AddressField.Text = fRecord.Address;
                fView.TelephoneField.Text = fRecord.Telephone;
                fView.WebSiteField.Text = fRecord.WebSite;
                fView.EmailField.Text = fRecord.Email;
                fView.NoteField.Text = fRecord.Note;
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.Name = fView.NameField.Text;
                fRecord.Address = fView.AddressField.Text;
                fRecord.Telephone = fView.TelephoneField.Text;
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
