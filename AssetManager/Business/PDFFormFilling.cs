using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using MyDialogLib;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager
{
    public class PdfFormFilling
    {
        private ExtendedForm ParentForm;
        private DeviceObject CurrentDevice = new DeviceObject();
        private AdvancedDialog CurrentDialog;
        private string UnitPriceTxtName = "txtUnitPrice";

        public PdfFormFilling(ExtendedForm parentForm, DeviceObject deviceInfo, PdfFormType pdfType)
        {
            this.ParentForm = parentForm;
            CurrentDevice = deviceInfo;
            FillForm(pdfType);
        }

        public void ListFieldNames()
        {
            PdfReader pdfReader = new PdfReader(Properties.Resources.Exh_K_02_Asset_Disposal_Form);
            StringBuilder sb = new StringBuilder();
            // var de = new KeyValuePair<string, AcroFields.Item>();

            foreach (KeyValuePair<string, AcroFields.Item> de in pdfReader.AcroFields.Fields)
            {
                sb.Append(de.Key.ToString() + Environment.NewLine);
            }
            Debug.Print(sb.ToString());
        }

        private string GetUnitPrice()
        {
            using (AdvancedDialog NewDialog = new AdvancedDialog(ParentForm))
            {
                CurrentDialog = NewDialog;
                NewDialog.Text = "Input Unit Price";
                NewDialog.AddTextBox(UnitPriceTxtName, "Enter Unit Price:");
                NewDialog.AddButton("cmdReqSelect", "Select From Req.", PriceFromMunis);
                NewDialog.ShowDialog();
                if (NewDialog.DialogResult == DialogResult.OK)
                {
                    return NewDialog.GetControlValue(UnitPriceTxtName).ToString();
                }
            }

            return string.Empty;
        }

        private async void PriceFromMunis()
        {
            try
            {
                OtherFunctions.Message("Please Double-Click a MUNIS line item on the following window.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Input Needed");
                var SelectedPrice = await GlobalInstances.MunisFunc.NewMunisReqSearch(GlobalInstances.MunisFunc.GetReqNumberFromPO(CurrentDevice.PO), GlobalInstances.MunisFunc.GetFYFromPO(CurrentDevice.PO), ParentForm, true);
                decimal decPrice = Convert.ToDecimal(SelectedPrice);
                var SelectedUnitPrice = decPrice.ToString("C");
                CurrentDialog.SetControlValue(UnitPriceTxtName, SelectedUnitPrice);
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void FillForm(PdfFormType Type)
        {
            try
            {
                Directory.CreateDirectory(Paths.DownloadPath);
                string strTimeStamp = DateTime.Now.ToString("_hhmmss");
                string newFile = Paths.DownloadPath + CurrentDevice.Description + strTimeStamp + ".pdf";

                if (Type == PdfFormType.InputForm)
                {
                    using (PdfReader pdfReader = new PdfReader(Properties.Resources.Exh_K_01_Asset_Input_Formnew))
                    {
                        using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                        {
                            AcroFields pdfFormFields = InputFormFields(CurrentDevice, pdfStamper);
                            pdfStamper.FormFlattening = FlattenPrompt();
                        }
                    }

                }
                else if (Type == PdfFormType.TransferForm)
                {
                    using (PdfReader pdfReader = new PdfReader(Properties.Resources.Exh_K_03_Asset_Transfer_Form))
                    {
                        using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                        {
                            AcroFields pdfFormFields = TransferFormFields(CurrentDevice, pdfStamper);
                            pdfStamper.FormFlattening = FlattenPrompt();
                        }
                    }

                }
                else if (Type == PdfFormType.DisposeForm)
                {
                    using (PdfReader pdfReader = new PdfReader(Properties.Resources.Exh_K_02_Asset_Disposal_Form))
                    {
                        using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create)))
                        {
                            AcroFields pdfFormFields = DisposalFormFields(CurrentDevice, pdfStamper);
                            pdfStamper.FormFlattening = FlattenPrompt();
                        }
                    }

                }

                Process.Start(newFile);
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private bool FlattenPrompt()
        {
            var blah = OtherFunctions.Message("Select 'Yes' to save the PDF as an editable form. Select 'No' to save the PDF as a flattened, ready to print document.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "PDF Type");
            if (blah == DialogResult.Yes)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private AcroFields DisposalFormFields(DeviceObject Device, PdfStamper pdfStamper)
        {
            AcroFields tmpFields = pdfStamper.AcroFields;
            using (AdvancedDialog newDialog = new AdvancedDialog(ParentForm, true))
            {
                newDialog.Text = "Additional Input Required";

                #region Section2

                newDialog.AddLabel("Reason for asset disposal-please check one:", true);
                newDialog.AddCheckBox("chkAuction", "Prep for public auction:");
                newDialog.AddCheckBox("chkObsolete", "Functional obsolescence:");
                newDialog.AddCheckBox("chkTradeIn", "Trade-in or exchange:");
                newDialog.AddCheckBox("chkDamaged", "Asset is damaged beyond repair:");
                newDialog.AddCheckBox("chkScrap", "Sold as scrap, not at a public sale:");
                newDialog.AddCheckBox("chkParts", "Used for parts:");
                newDialog.AddCheckBox("chkOther", "Other:");
                newDialog.AddRichTextBox("rtbOther", "If Other, Please explain:");

                #endregion

                #region Section3

                newDialog.AddLabel("Method of asset disposal-please check one:", true);
                newDialog.AddCheckBox("chkHand", "Hand carried by:");
                newDialog.AddRichTextBox("rtbHand", "");
                newDialog.AddCheckBox("chkCarrier", "Carrier company:");
                newDialog.AddRichTextBox("rtbCarrier", "");
                newDialog.AddCheckBox("chkShipping", "Shipping receipt number:");
                newDialog.AddRichTextBox("rtbShipping", "");
                newDialog.AddCheckBox("chkDisposed", "Disposed of on premises:");
                newDialog.AddRichTextBox("rtbDisposed", "");
                newDialog.AddCheckBox("chkOtherMethod", "Other. Please explain:");
                newDialog.AddRichTextBox("rtpOtherMethod", "");

                #endregion

                #region Section4

                newDialog.AddTextBox("txtSaleAmount", "List the amount of proceeds from the sale of the disposed asset, if any.");
                newDialog.AddLabel("If the asset item was traded, provide the following information for the asset BEGING ACQUIRED:", true);
                newDialog.AddTextBox("txtAssetTag", "Asset/Tag Number:");
                newDialog.AddTextBox("txtSerial", "Serial Number:");
                newDialog.AddTextBox("txtDescription", "Description:");

                #endregion

                newDialog.ShowDialog();
                if (newDialog.DialogResult != DialogResult.OK)
                {
                    return null;
                }
                tmpFields.SetField("topmostSubform[0].Page1[0].AssetTag_number[0]", Device.AssetTag);
                tmpFields.SetField("topmostSubform[0].Page1[0].Mfg_serial_number_1[0]", Device.Serial);
                tmpFields.SetField("topmostSubform[0].Page1[0].Mfg_serial_number_2[0]", Device.Description);
                tmpFields.SetField("topmostSubform[0].Page1[0].Mfg_serial_number_3[0]", "FCBDD");
                tmpFields.SetField("topmostSubform[0].Page1[0].County_s_possession[0]", DateTime.Now.ToString("MM/dd/yyyy"));

                #region Section 2

                tmpFields.SetField("topmostSubform[0].Page1[0].Preparation_for_public_auction[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkAuction"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Functional_obsolescence[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkObsolete"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Trade-in_or_exchange[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkTradeIn"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Asset_is_damaged_beyond_repair[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkDamaged"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Sold_as_scrap__not_at_a_public_sale[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkScrap"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Used_for_parts[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkParts"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].undefined[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkOther"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Other__Please_explain_2[0]", newDialog.GetControlValue("rtbOther").ToString());

                #endregion

                #region Section 3

                tmpFields.SetField("topmostSubform[0].Page1[0].Method_of_asset_disposal_please_check_one[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkHand"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Hand_carried_by[0]", newDialog.GetControlValue("rtbHand").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0]._1[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkCarrier"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Carrier_company[0]", newDialog.GetControlValue("rtbCarrier").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0]._2[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkShipping"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Shipping_receipt_number[0]", newDialog.GetControlValue("rtbShipping").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0]._3[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkDisposed"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Disposed_of_on_premises[0]", newDialog.GetControlValue("rtbDisposed").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0]._4[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkOtherMethod"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Other__Please_explain_3[0]", newDialog.GetControlValue("rtpOtherMethod").ToString());

                #endregion

                #region Section 4

                tmpFields.SetField("topmostSubform[0].Page1[0].List_the_amount_of_proceeds_from_the_sale_of_the_disposed_asset__if_any[0]", newDialog.GetControlValue("txtSaleAmount").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0].AssetTag_number_2[0]", newDialog.GetControlValue("txtAssetTag").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0].Serial_number[0]", newDialog.GetControlValue("txtSerial").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", newDialog.GetControlValue("txtDescription").ToString());
                tmpFields.SetField("topmostSubform[0].Page1[0].Department_1[0]", "FCBDD");
                tmpFields.SetField("topmostSubform[0].Page1[0].Date[0]", DateTime.Now.ToString("MM/dd/yyyy"));

                #endregion

            }

            return tmpFields;
        }

        private AcroFields InputFormFields(DeviceObject Device, PdfStamper pdfStamper)
        {
            AcroFields tmpFields = pdfStamper.AcroFields;
            string strUnitPrice = GetUnitPrice();
            if (string.IsNullOrEmpty(strUnitPrice) || ReferenceEquals(strUnitPrice, null))
            {
                return null;
            }
            tmpFields.SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD");
            // .SetField("topmostSubform[0].Page1[0].Asterisked_items_____must_be_completed_by_the_department[0]", CurrentDevice.strAssetTag)
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined[0]", Device.Serial);
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_2[0]",GlobalInstances.MunisFunc.GetVendorNameFromPO(Device.PO));
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_3[0]", Device.Description);
            //.SetField("topmostSubform[0].Page1[0]._1[0]", "6")
            // .SetField("topmostSubform[0].Page1[0]._2[0]", "7")
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_4[0]", Device.PO);
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_5[0]",GlobalInstances.AssetFunc.GetMunisCodeFromAssetCode(Device.Location));
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_6[0]", "5200");
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_7[0]",GlobalInstances.AssetFunc.GetMunisCodeFromAssetCode(Device.EquipmentType));
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_8[0]", "GP");
            //.SetField("topmostSubform[0].Page1[0].undefined_9[0]", "13")
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_10[0]", "1");
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_11[0]", strUnitPrice);
            tmpFields.SetField("topmostSubform[0].Page1[0].undefined_12[0]", Device.PurchaseDate.ToString("MM/dd/yyyy"));
            tmpFields.SetField("topmostSubform[0].Page1[0].Date[0]", DateTime.Now.ToString("MM/dd/yyyy"));
            return tmpFields;
        }

        private AcroFields TransferFormFields(DeviceObject Device, PdfStamper pdfStamper)
        {
            AcroFields tmpFields = pdfStamper.AcroFields;
            using (AdvancedDialog newDialog = new AdvancedDialog(ParentForm))
            {
                newDialog.Text = "Additional Input Required";
                ComboBox cmbFrom = new ComboBox();
                AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.Locations, cmbFrom);
                newDialog.AddCustomControl("cmbFromLoc", "Transfer FROM:", (Control)cmbFrom);
                ComboBox cmbTo = new ComboBox();
                AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.Locations, cmbTo);
                newDialog.AddCustomControl("cmbToLoc", "Transfer TO:", (Control)cmbTo);
                newDialog.AddLabel("Reason For Transfer-Check One:", true);
                newDialog.AddCheckBox("chkBetterU", "Better Use of asset:");
                newDialog.AddCheckBox("chkTradeIn", "Trade-in or exchange:");
                newDialog.AddCheckBox("chkExcess", "Excess assets:");
                newDialog.AddCheckBox("chkOther", "Other:");
                newDialog.AddRichTextBox("rtbOther", "If Other, Please explain:");
                newDialog.ShowDialog();
                if (newDialog.DialogResult != DialogResult.OK)
                {
                    return null;
                }
                tmpFields.SetField("topmostSubform[0].Page1[0].AssetTag_number[0]", Device.AssetTag);
                tmpFields.SetField("topmostSubform[0].Page1[0].Serial_number[0]", Device.Serial);
                tmpFields.SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.Description);
                tmpFields.SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD - 5200");
                tmpFields.SetField("topmostSubform[0].Page1[0].Location[0]", AttribIndexFunctions.GetDisplayValueFromIndex(GlobalInstances.DeviceAttribute.Locations, System.Convert.ToInt32(newDialog.GetControlValue("cmbFromLoc"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Department_2[0]", "FCBDD - 5200");
                tmpFields.SetField("topmostSubform[0].Page1[0].Location_2[0]", AttribIndexFunctions.GetDisplayValueFromIndex(GlobalInstances.DeviceAttribute.Locations, System.Convert.ToInt32(newDialog.GetControlValue("cmbToLoc"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Better_utilization_of_assets[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkBetterU"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Trade-in_or_exchange_with_Other_Departments[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkTradeIn"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Excess_assets[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkExcess"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].undefined[0]", CheckValueToString(System.Convert.ToBoolean(newDialog.GetControlValue("chkOther"))));
                tmpFields.SetField("topmostSubform[0].Page1[0].Other__Please_explain_1[0]", newDialog.GetControlValue("rtbOther").ToString());
                //key
                //topmostSubform[0].Page1[0].AssetTag_number[0]
                //topmostSubform[0].Page1[0].Serial_number[0]
                //topmostSubform[0].Page1[0].Description_of_asset[0]
                //topmostSubform[0].Page1[0].Department[0]
                //topmostSubform[0].Page1[0].Location[0]
                //topmostSubform[0].Page1[0].Department_2[0]
                //topmostSubform[0].Page1[0].Location_2[0]
                //topmostSubform[0].Page1[0].Better_utilization_of_assets[0]
                //topmostSubform[0].Page1[0].Trade-in_or_exchange_with_Other_Departments[0]
                //topmostSubform[0].Page1[0].Excess_assets[0]
                //topmostSubform[0].Page1[0].undefined[0]
                //topmostSubform[0].Page1[0].Other__Please_explain_1[0]
                //topmostSubform[0].Page1[0].Other__Please_explain_2[0]
                //topmostSubform[0].Page1[0].Method_of_Delivery_or_Shipping_Please_Check_One[0]
                //topmostSubform[0].Page1[0].Hand-carried_by[0]
                //topmostSubform[0].Page1[0].undefined_2[0]
                //topmostSubform[0].Page1[0].Carrier_company[0]
                //topmostSubform[0].Page1[0].US_Mail[0]
                //topmostSubform[0].Page1[0].Shipping_receipt_number[0]
                //topmostSubform[0].Page1[0].Date_of_shipment_or_transfer[0]
                //topmostSubform[0].Page1[0].Signature_of_SENDING_official[0]
                //topmostSubform[0].Page1[0].Department_3[0]
                //topmostSubform[0].Page1[0].Date[0]
                //topmostSubform[0].Page1[0].Signature_of_RECEIVING_official[0]
                //topmostSubform[0].Page1[0].Department_4[0]
                //topmostSubform[0].Page1[0].Date_2[0]
                //topmostSubform[0].Page1[0].PrintButton1[0]
            }

            return tmpFields;
        }

        private string CheckValueToString(bool Checked)
        {
            if (@Checked)
            {
                return "X";
            }
            else
            {
                return "";
            }
        }

    }
}