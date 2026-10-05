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
    public interface ITransferEditorView : IEditorView<Transfer>
    {
        ITextBox NameField { get; }
        IComboBox SourceCombo { get; }
        IComboBox TargetCombo { get; }
        IDateTimeBox DateField { get; }
        IComboBox TypeCombo { get; }
        ITextBox CauseField { get; }
        ITextBox QuantityField { get; }
        ITextBox UnitPriceField { get; }
        IComboBox ShopCombo { get; }
    }


    public class TransferEditorPresenter : EditorPresenter<IModel, Transfer, ITransferEditorView>
    {
        private readonly ILogger fLogger = LogManager.GetLogger(ALCore.LOG_FILE, ALCore.LOG_LEVEL, "TransferEditorPresenter");


        public TransferEditorPresenter(ITransferEditorView view) : base(view)
        {
            var transferTypesList = ALData.GetNamesList<TransferType>(ALData.TransferTypes);
            fView.TypeCombo.AddRange(transferTypesList, true);
        }

        public override void UpdateView()
        {
            if (fModel != null) {
                fView.ShopCombo.AddRange(fModel.QueryTransferShops(), true);
            }

            if (fRecord != null) {
                string itName = fModel.GetRecordName(fRecord.ItemType, fRecord.ItemId);
                fView.NameField.Text = itName;

                if (fRecord.ItemType != ItemType.Aquarium) {
                    if (fRecord.IsNewRecord) {
                        IList<Transfer> lastTransfers = fModel.QueryLastTransfers(fRecord.ItemId, (int)fRecord.ItemType);
                        if (lastTransfers.Count > 0) {
                            fRecord.SourceId = lastTransfers[0].TargetId;
                        }
                    }

                    var aquariumsList = fModel.QueryAquariumsList(!fRecord.IsNewRecord);

                    fView.SourceCombo.AddRange(aquariumsList);
                    fView.SourceCombo.SetSelectedTag(fRecord.SourceId, false);

                    fView.TargetCombo.AddRange(aquariumsList);
                    fView.TargetCombo.SetSelectedTag(fRecord.TargetId, false);
                } else {
                    fView.SourceCombo.Enabled = false;
                    fView.TargetCombo.Enabled = false;
                }

                if (!ALCore.IsZeroDate(fRecord.Timestamp)) {
                    fView.DateField.Value = fRecord.Timestamp;
                }

                fView.TypeCombo.SetSelectedTag(fRecord.Type);
                fView.CauseField.Text = fRecord.Cause;
                fView.QuantityField.SetDecimalVal(fRecord.Quantity);

                if (fRecord.Type == TransferType.Purchase || fRecord.Type == TransferType.Sale) {
                    fView.UnitPriceField.SetDecimalVal(fRecord.UnitPrice);
                    fView.ShopCombo.Text = fRecord.Shop;
                }
            }
        }

        public override bool ApplyChanges()
        {
            try {
                fRecord.SourceId = fView.SourceCombo.GetSelectedTag<int>();
                fRecord.TargetId = fView.TargetCombo.GetSelectedTag<int>();
                fRecord.Timestamp = fView.DateField.Value;
                fRecord.Type = fView.TypeCombo.GetSelectedTag<TransferType>();
                fRecord.Cause = fView.CauseField.Text;
                fRecord.Quantity = (float)fView.QuantityField.GetDecimalVal();

                if (fRecord.Type == TransferType.Purchase || fRecord.Type == TransferType.Sale) {
                    fRecord.UnitPrice = (float)fView.UnitPriceField.GetDecimalVal();
                    fRecord.Shop = fView.ShopCombo.Text;
                }

                return true;
            } catch (Exception ex) {
                fLogger.WriteError("ApplyChanges()", ex);
                return false;
            }
        }

        public void ChangeSelectedType()
        {
            TransferType transferType = fView.TypeCombo.GetSelectedTag<TransferType>();

            bool ps = transferType == TransferType.Purchase || transferType == TransferType.Sale;

            fView.UnitPriceField.Enabled = ps;
            fView.ShopCombo.Enabled = ps;

            if (ps) {
                fView.UnitPriceField.SetDecimalVal(fRecord.UnitPrice);
                fView.ShopCombo.Text = fRecord.Shop;
            } else {
                fView.UnitPriceField.SetDecimalVal(0.0f);
                fView.ShopCombo.Text = "";
            }
        }
    }
}
