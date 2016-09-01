Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.xml
Imports System.IO
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Module PDFFormFilling
    Public NotInheritable Class FormType
        Public Const InputForm As String = "INPUT"
        Public Const TransferForm As String = "TRANSFER"
    End Class
    Public Sub ListFieldNames()
        Dim pdfReader As PdfReader = New PdfReader(My.Resources.Exh_K_03_Asset_Transfer_Form)
        Dim sb As New StringBuilder()
        Dim de As New KeyValuePair(Of String, iTextSharp.text.pdf.AcroFields.Item) 'DictionaryEntry
        For Each de In pdfReader.AcroFields.Fields
            sb.Append(de.Key.ToString() + Environment.NewLine)
        Next
        Debug.Print(sb.ToString())
    End Sub
    Private Function GetUnitPrice(Device As Device_Info) As String
        Message("Please Double-Click a MUNIS line item on the following window.", vbOKOnly + vbInformation, "Input Needed")
        Dim f As New View_Munis
        f.Text = "Select a Line Item"
        f.HideFixedAssetGrid()
        f.LoadDevice(Device)
        f.LoadMunisRequisitionGridByReqNo(Munis.Get_ReqNumber_From_PO(Device.strPO), Munis.Get_FY_From_PO(Device.strPO))
        f.ShowDialog(View)
        If f.DialogResult = DialogResult.OK Then
            Return f.UnitPrice
        Else
            Return Nothing
        End If
    End Function
    Public Sub FillForm(Device As Device_Info, Type As String)
        Try
            Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
            Dim strTimeStamp As String = Now.ToString("_hhmmss")
            Dim newFile As String = strTempPath & Device.strDescription & strTimeStamp & ".pdf"
            Dim pdfStamper As PdfStamper
            Select Case Type
                Case FormType.InputForm
                    Dim pdfReader As New PdfReader(My.Resources.Exh_K_01_Asset_Input_Formnew)
                    pdfStamper = New PdfStamper(pdfReader, New FileStream(newFile, FileMode.Create))
                    Dim pdfFormFields As AcroFields = InputFormFields(Device, pdfStamper) 'pdfStamper.AcroFields
                Case FormType.TransferForm
                    Dim pdfReader As New PdfReader(My.Resources.Exh_K_03_Asset_Transfer_Form)
                    pdfStamper = New PdfStamper(pdfReader, New FileStream(newFile, FileMode.Create))
                    Dim pdfFormFields As AcroFields = TransferFormFields(Device, pdfStamper)
            End Select
            pdfStamper.FormFlattening = True
            ' close the pdf
            pdfStamper.Close()
            Process.Start(newFile)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Function InputFormFields(Device As Device_Info, ByRef pdfStamper As PdfStamper) As AcroFields
        Dim tmpFields As AcroFields = pdfStamper.AcroFields
        Dim strUnitPrice As String = GetUnitPrice(Device)
        If strUnitPrice = "" Or IsNothing(strUnitPrice) Then
            Exit Function
        End If
        With tmpFields
            .SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD")
            ' .SetField("topmostSubform[0].Page1[0].Asterisked_items_____must_be_completed_by_the_department[0]", CurrentDevice.strAssetTag)
            .SetField("topmostSubform[0].Page1[0].undefined[0]", Device.strSerial)
            .SetField("topmostSubform[0].Page1[0].undefined_2[0]", Munis.Get_VendorName_From_PO(Device.strPO))
            .SetField("topmostSubform[0].Page1[0].undefined_3[0]", Device.strDescription)
            '.SetField("topmostSubform[0].Page1[0]._1[0]", "6") 
            ' .SetField("topmostSubform[0].Page1[0]._2[0]", "7")
            .SetField("topmostSubform[0].Page1[0].undefined_4[0]", Device.strPO)
            .SetField("topmostSubform[0].Page1[0].undefined_5[0]", Get_MunisCode_From_AssetCode(Device.strLocation))
            .SetField("topmostSubform[0].Page1[0].undefined_6[0]", "5200")
            .SetField("topmostSubform[0].Page1[0].undefined_7[0]", Get_MunisCode_From_AssetCode(Device.strEqType))
            .SetField("topmostSubform[0].Page1[0].undefined_8[0]", "GP")
            '.SetField("topmostSubform[0].Page1[0].undefined_9[0]", "13")
            .SetField("topmostSubform[0].Page1[0].undefined_10[0]", "1")
            .SetField("topmostSubform[0].Page1[0].undefined_11[0]", strUnitPrice)
            .SetField("topmostSubform[0].Page1[0].undefined_12[0]", Device.dtPurchaseDate.ToString("MM/dd/yyyy"))
            .SetField("topmostSubform[0].Page1[0].Date[0]", Now.ToString("MM/dd/yyyy"))
        End With
        Return tmpFields
    End Function
    Private Function TransferFormFields(Device As Device_Info, ByRef pdfStamper As PdfStamper) As AcroFields
        Dim tmpFields As AcroFields = pdfStamper.AcroFields
        Dim newDialog As New MyDialog
        With newDialog
            .Text = "Additional Input Required"
            .AddComboBox("cmbFromLoc", "Transfer FROM:", DeviceIndex.Locations)
            .AddComboBox("cmbToLoc", "Transfer TO:", DeviceIndex.Locations)
            .AddLabel("Reason For Transfer-Check One:", True)
            .AddCheckBox("chkBetterU", "Better Use of asset:")
            .AddCheckBox("chkTradeIn", "Trade-in or exchange:")
            .AddCheckBox("chkExcess", "Excess assets:")
            .AddCheckBox("chkOther", "Other:")
            .AddRichTextBox("rtbOther", "If Other, Please explain:")
            .ShowDialog()
        End With
        If newDialog.DialogResult <> DialogResult.OK Then Return Nothing
        With tmpFields
            .SetField("topmostSubform[0].Page1[0].AssetTag_number[0]", Device.strAssetTag)
            .SetField("topmostSubform[0].Page1[0].Serial_number[0]", Device.strSerial)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)
            .SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD - 5200")
            .SetField("topmostSubform[0].Page1[0].Location[0]", GetHumanValueFromIndex(DeviceIndex.Locations, newDialog.GetControlValue("cmbFromLoc")))
            .SetField("topmostSubform[0].Page1[0].Department_2[0]", "FCBDD - 5200")
            .SetField("topmostSubform[0].Page1[0].Location_2[0]", GetHumanValueFromIndex(DeviceIndex.Locations, newDialog.GetControlValue("cmbToLoc")))
            .SetField("topmostSubform[0].Page1[0].Better_utilization_of_assets[0]", CheckValueToString(newDialog.GetControlValue("chkBetterU")))
            .SetField("topmostSubform[0].Page1[0].Trade-in_or_exchange_with_Other_Departments[0]", CheckValueToString(newDialog.GetControlValue("chkTradeIn")))
            .SetField("topmostSubform[0].Page1[0].Excess_assets[0]", CheckValueToString(newDialog.GetControlValue("chkExcess")))
            .SetField("topmostSubform[0].Page1[0].undefined[0]", CheckValueToString(newDialog.GetControlValue("chkOther")))
            .SetField("topmostSubform[0].Page1[0].Other__Please_explain_1[0]", newDialog.GetControlValue("rtbOther"))
            'key
            'topmostSubform[0].Page1[0].AssetTag_number[0]
            'topmostSubform[0].Page1[0].Serial_number[0]
            'topmostSubform[0].Page1[0].Description_of_asset[0]
            'topmostSubform[0].Page1[0].Department[0]
            'topmostSubform[0].Page1[0].Location[0]
            'topmostSubform[0].Page1[0].Department_2[0]
            'topmostSubform[0].Page1[0].Location_2[0]
            'topmostSubform[0].Page1[0].Better_utilization_of_assets[0]
            'topmostSubform[0].Page1[0].Trade-in_or_exchange_with_Other_Departments[0]
            'topmostSubform[0].Page1[0].Excess_assets[0]
            'topmostSubform[0].Page1[0].undefined[0]
            'topmostSubform[0].Page1[0].Other__Please_explain_1[0]
            'topmostSubform[0].Page1[0].Other__Please_explain_2[0]
            'topmostSubform[0].Page1[0].Method_of_Delivery_or_Shipping_Please_Check_One[0]
            'topmostSubform[0].Page1[0].Hand-carried_by[0]
            'topmostSubform[0].Page1[0].undefined_2[0]
            'topmostSubform[0].Page1[0].Carrier_company[0]
            'topmostSubform[0].Page1[0].US_Mail[0]
            'topmostSubform[0].Page1[0].Shipping_receipt_number[0]
            'topmostSubform[0].Page1[0].Date_of_shipment_or_transfer[0]
            'topmostSubform[0].Page1[0].Signature_of_SENDING_official[0]
            'topmostSubform[0].Page1[0].Department_3[0]
            'topmostSubform[0].Page1[0].Date[0]
            'topmostSubform[0].Page1[0].Signature_of_RECEIVING_official[0]
            'topmostSubform[0].Page1[0].Department_4[0]
            'topmostSubform[0].Page1[0].Date_2[0]
            'topmostSubform[0].Page1[0].PrintButton1[0]
        End With
        newDialog.Dispose()
        Return tmpFields
    End Function
    Private Function CheckValueToString(CheckValue As CheckState) As String
        If CheckValue = CheckState.Checked Then
            Return "X"
        Else
            Return ""
        End If
    End Function
End Module
