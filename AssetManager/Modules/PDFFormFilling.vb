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















            'pdfFormFields.SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD")
            '' pdfFormFields.SetField("topmostSubform[0].Page1[0].Asterisked_items_____must_be_completed_by_the_department[0]", CurrentDevice.strAssetTag)
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined[0]", Device.strSerial)
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_2[0]", Munis.Get_VendorName_From_PO(Device.strPO))
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_3[0]", Device.strDescription)
            ''pdfFormFields.SetField("topmostSubform[0].Page1[0]._1[0]", "6") 
            '' pdfFormFields.SetField("topmostSubform[0].Page1[0]._2[0]", "7")
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_4[0]", Device.strPO)
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_5[0]", Get_MunisCode_From_AssetCode(Device.strLocation))
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_6[0]", "5200")
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_7[0]", Get_MunisCode_From_AssetCode(Device.strEqType))
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_8[0]", "GP")
            ''pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_9[0]", "13")
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_10[0]", "1")
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_11[0]", strUnitPrice)
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_12[0]", Device.dtPurchaseDate.ToString("MM/dd/yyyy"))
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].Date[0]", Now.ToString("MM/dd/yyyy"))
            PdfStamper.FormFlattening = True
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
        Dim cmbFromLoc As New ComboBox
        cmbFromLoc.Name = "cmbFromLoc"
        cmbFromLoc.Tag = "Transfer FROM:"
        FillComboBox(Locations, cmbFromLoc)
        newDialog.AddControl(cmbFromLoc)

        Dim cmbToLoc As New ComboBox
        cmbToLoc.Name = "cmbToLoc"
        cmbToLoc.Tag = "Transfer TO:"
        FillComboBox(Locations, cmbToLoc)
        newDialog.AddControl(cmbToLoc)

        Dim lbl As New Label
        lbl.Text = "Reason For Transfer-Check One:"
        newDialog.AddControl(lbl)

        Dim chkBetterU As New CheckBox
        chkBetterU.Name = "chkBetterU"
        chkBetterU.Tag = "Better Use of asset:"
        newDialog.AddControl(chkBetterU)

        Dim chkTradeIn As New CheckBox
        chkTradeIn.Name = "chkTradeIn"
        chkTradeIn.Tag = "Trade-in or exchange:"
        newDialog.AddControl(chkTradeIn)

        Dim chkExcess As New CheckBox
        chkExcess.Name = "chkExcess"
        chkExcess.Tag = "Excess assets:"
        newDialog.AddControl(chkExcess)

        Dim chkOther As New CheckBox
        chkOther.Name = "chkOther"
        chkOther.Tag = "Other:"
        newDialog.AddControl(chkOther)


        Dim rtbOther As New RichTextBox
        rtbOther.Name = "rtbOther"
        rtbOther.Tag = "If Other, Please explain:"
        newDialog.AddControl(rtbOther)

        newDialog.ShowDialog()

        With tmpFields
            .SetField("topmostSubform[0].Page1[0].AssetTag_number[0]", Device.strAssetTag)
            .SetField("topmostSubform[0].Page1[0].Serial_number[0]", Device.strSerial)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)


            .SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD - 5200")
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)
            .SetField("topmostSubform[0].Page1[0].Description_of_asset[0]", Device.strDescription)




        End With

        Return tmpFields

    End Function
End Module
