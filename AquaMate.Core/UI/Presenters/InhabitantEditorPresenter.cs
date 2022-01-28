/*
 *  This file is part of the "AquaMate".
 *  Copyright (C) 2019-2022 by Sergey V. Zhdanovskih.
 *  This program is licensed under the GNU General Public License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using AquaMate.Core;
using AquaMate.Core.Model;
using AquaMate.Core.Types;
using AquaMate.Logging;
using BSLib;
using BSLib.Design.MVP.Controls;

namespace AquaMate.UI
{
    public interface IInhabitantEditorView : IEditorView<Inhabitant>
    {
        IComboBox SpeciesCombo { get; }
        ITextBox NameField { get; }
        ITextBox NoteField { get; }
        ILabel SexLabel { get; }
        IComboBox SexCombo { get; }
        IComboBox StateCombo { get; }

        void SetImage(ItemType itemType, int itemId);
    }


    /// <summary>
    /// 
    /// </summary>
    public class InhabitantEditorPresenter : EditorPresenter<IModel, Inhabitant, IInhabitantEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "InhabitantEditorPresenter");


        public InhabitantEditorPresenter(IInhabitantEditorView view) : base(view)
        {
            var sexNamesList = ALData.GetNamesList<Sex>(ALData.SexNames);
            fView.SexCombo.AddRange(sexNamesList, false);
        }

        public override void UpdateView()
        {
            var speciesList = fModel.QuerySpecies();
            var species = speciesList.FirstOrDefault(sp => sp.Id == fRecord.SpeciesId);
            var speciesNames = ALData.GetEntityNamesList(speciesList);
            fView.SpeciesCombo.AddRange(speciesNames);
            fView.SpeciesCombo.SetSelectedTag(fRecord.SpeciesId);

            fView.NameField.Text = fRecord.Name;
            fView.NoteField.Text = fRecord.Note;
            fView.SexCombo.SetSelectedTag(fRecord.Sex);

            ItemType itemType;

            if (species != null) {
                itemType = ALCore.GetItemType(species.Type);
                ItemState itemState;
                DateTime exclusionDate;
                fModel.GetItemState(fRecord.Id, itemType, out itemState, out exclusionDate);
                bool noTransferState = (itemState == ItemState.Unknown);
                fView.StateCombo.Enabled = noTransferState;

                if (!noTransferState) {
                    SetState(itemType, fRecord.State);
                }
            } else {
                itemType = ItemType.None;
                fView.StateCombo.Clear();
            }

            fView.SetImage(itemType, fRecord.Id);
        }

        private void SetState(ItemType itemType, ItemState itemState)
        {
            var namesList = ALData.GetItemStateNamesList(itemType);
            fView.StateCombo.AddRange(namesList, false);
            fView.StateCombo.SetSelectedTag(fRecord.State);
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.SpeciesId = fView.SpeciesCombo.GetSelectedTag<int>();
                fRecord.Name = fView.NameField.Text;
                fRecord.Note = fView.NoteField.Text;
                fRecord.Sex = fView.SexCombo.GetSelectedTag<Sex>();
                fRecord.State = fView.StateCombo.GetSelectedTag<ItemState>();

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }

        public void ChangeSelectedSpecies()
        {
            int speciesId = fView.SpeciesCombo.GetSelectedTag<int>();
            Species species = fModel.GetRecord<Species>(speciesId);

            bool itemIsNull = (species == null);

            if (!itemIsNull) {
                SetState(ALCore.GetItemType(species.Type), fRecord.State);
            }

            bool hasSex = (!itemIsNull && ALCore.IsAnimal(species.Type));
            fView.SexLabel.Enabled = hasSex;
            fView.SexCombo.Enabled = hasSex;
        }
    }
}
