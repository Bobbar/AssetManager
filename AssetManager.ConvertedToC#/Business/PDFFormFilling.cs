using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using MyDialogLib;

public class PdfFormFilling
{
    private ExtendedForm ParentForm;
    private DeviceObject CurrentDevice = new DeviceObject();
    private AdvancedDialog CurrentDialog;

    private string UnitPriceTxtName = "txtUnitPrice";
    PdfFormFilling(ExtendedForm parentForm, DeviceObject deviceInfo, PdfFormType pdfType)
    {
        this.ParentForm = parentForm;
        CurrentDevice = deviceInfo;
        FillForm(pdfType);
    }

    public void ListFieldNames()
    {
        PdfReader pdfReader = new PdfReader(My.Resources.Exh_K_02_Asset_Disposal_Form);
        StringBuilder sb = new StringBuilder();
        KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de = new KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item>();
        //DictionaryEntry
        foreach (de in pdfReader.AcroFields.Fields)
        {
            sb.Append(de.Key.ToString() + Environment.NewLine);
        }
        Debug.Print(sb.ToString());
    }

    [CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Req")]
    private string GetUnitPrice()
    {
        using (AdvancedDialog NewDialog = new AdvancedDialog(ParentForm))
        {
            CurrentDialog = NewDialog;
            // ERROR: Not supported in C#: WithStatement

        }
        return string.Empty;
    }

    private async void PriceFromMunis()
    {
        try
        {
            Message("Please Double-Click a MUNIS line item on the following window.", vbOKOnly + vbInformation, "Input Needed");
            object SelectedPrice = await MunisFunc.NewMunisReqSearch(MunisFunc.GetReqNumberFromPO(CurrentDevice.PO), MunisFunc.GetFYFromPO(CurrentDevice.PO), ParentForm, true);
            decimal decPrice = Convert.ToDecimal(SelectedPrice);
            object SelectedUnitPrice = decPrice.ToString("C");
            CurrentDialog.SetControlValue(UnitPriceTxtName, SelectedUnitPrice);
        }
        catch (Exception ex)
        {
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
        }
    }

    private void FillForm(PdfFormType Type)
    {
        try
        {
            Directory.CreateDirectory(Paths.DownloadPath);
            string strTimeStamp = Now.ToString("_hhmmss");
            string newFile = Paths.DownloadPath + CurrentDevice.Description + strTimeStamp + ".pdf";

            switch (Type)
            {
                case PdfFormType.InputForm:
                    using (PdfReader pdfReader = new PdfReader(My.Resources.Exh_K_01_Asset_Input_Formnew)object pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create))) {
                        AcroFields pdfFormFields = InputFormFields(CurrentDevice, pdfStamper);
                        pdfStamper.FormFlattening = FlattenPrompt();

                    }

                case PdfFormType.TransferForm:
                    using (PdfReader pdfReader = new PdfReader(My.Resources.Exh_K_03_Asset_Transfer_Form)object pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create))) {
                        AcroFields pdfFormFields = TransferFormFields(CurrentDevice, pdfStamper);
                        pdfStamper.FormFlattening = FlattenPrompt();
                    }

                case PdfFormType.DisposeForm:
                    using (PdfReader pdfReader = new PdfReader(My.Resources.Exh_K_02_Asset_Disposal_Form)object pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create))) {
                        AcroFields pdfFormFields = DisposalFormFields(CurrentDevice, pdfStamper);
                        pdfStamper.FormFlattening = FlattenPrompt();
                    }

            }

            Process.Start(newFile);
        }
        catch (Exception ex)
        {
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
        }
    }

    private bool FlattenPrompt()
    {
        object blah = Message("Select 'Yes' to save the PDF as an editable form. Select 'No' to save the PDF as a flattened, ready to print document.", vbQuestion + vbYesNo, "PDF Type");
        if (blah == vbYes)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private AcroFields DisposalFormFields(DeviceObject Device, ref PdfStamper pdfStamper)
    {
        AcroFields tmpFields = pdfStamper.AcroFields;
        using (AdvancedDialog newDialog = new AdvancedDialog(ParentForm, true))
        {

# Region "Section2"


# End Region

# Region "Section3"


# End Region

# Region "Section4"


# End Region

            // ERROR: Not supported in C#: WithStatement

            if (newDialog.DialogResult != DialogResult.OK)
                return null;

# Region "Section 2"


# End Region

# Region "Section 3"


# End Region

# Region "Section 4"


# End Region

            // ERROR: Not supported in C#: WithStatement

        }
        return tmpFields;
    }

    private AcroFields InputFormFields(DeviceObject Device, ref PdfStamper pdfStamper)
    {
        AcroFields tmpFields = pdfStamper.AcroFields;
        string strUnitPrice = GetUnitPrice();
        if (strUnitPrice == "" | strUnitPrice == null)
        {
            return null;
        }
        // .SetField("topmostSubform[0].Page1[0].Asterisked_items_____must_be_completed_by_the_department[0]", CurrentDevice.strAssetTag)
        //.SetField("topmostSubform[0].Page1[0]._1[0]", "6")
        // .SetField("topmostSubform[0].Page1[0]._2[0]", "7")
        //.SetField("topmostSubform[0].Page1[0].undefined_9[0]", "13")
        // ERROR: Not supported in C#: WithStatement

        return tmpFields;
    }

    private AcroFields TransferFormFields(DeviceObject Device, ref PdfStamper pdfStamper)
    {
        AcroFields tmpFields = pdfStamper.AcroFields;
        using (AdvancedDialog newDialog = new AdvancedDialog(ParentForm))
        {
            // ERROR: Not supported in C#: WithStatement

            if (newDialog.DialogResult != DialogResult.OK)
                return null;
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
            // ERROR: Not supported in C#: WithStatement

        }
        return tmpFields;
    }

    private string CheckValueToString(bool Checked)
    {
        if (Checked)
        {
            return "X";
        }
        else
        {
            return "";
        }
    }

}
