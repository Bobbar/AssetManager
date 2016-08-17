Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.xml
Imports System.IO
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Module PDFFormFilling
    Public Sub ListFieldNames()
        Dim pdfReader As PdfReader = New PdfReader(My.Resources.Exh_K_01_Asset_Input_Formnew)
        Dim sb As New StringBuilder()
        Dim de As New KeyValuePair(Of String, iTextSharp.text.pdf.AcroFields.Item) 'DictionaryEntry
        For Each de In pdfReader.AcroFields.Fields
            sb.Append(de.Key.ToString() + Environment.NewLine)
        Next
        Debug.Print(sb.ToString())
    End Sub
    Private Function GetUnitPrice() As String
        Dim f As New View_Munis
        f.Text = "Double-Click a line item from the requisition."
        f.HideFixedAssetGrid()
        f.LoadMunisRequisitionGridByReqNo(Munis_GetReqNumberFromPO(CurrentDevice.strPO), Munis_GetFYFromPO(CurrentDevice.strPO))
        f.ShowDialog(View)
        If f.DialogResult = DialogResult.OK Then
            Return f.UnitPrice
        Else
            Return Nothing
        End If
    End Function
    Public Sub FillForm()
        Try
            Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
            Dim strTimeStamp As String = Now.ToString("_hhmmss")
            Dim newFile As String = strTempPath & CurrentDevice.strDescription & strTimeStamp & ".pdf"
            Dim strUnitPrice As String = GetUnitPrice()
            If strUnitPrice = "" Or IsNothing(strUnitPrice) Then
                Exit Sub
            End If
            Dim pdfReader As New PdfReader(My.Resources.Exh_K_01_Asset_Input_Formnew)
            Dim pdfStamper As New PdfStamper(pdfReader, New FileStream(
                newFile, FileMode.Create))
            Dim pdfFormFields As AcroFields = pdfStamper.AcroFields
            pdfFormFields.SetField("topmostSubform[0].Page1[0].Department[0]", "FCBDD")
            ' pdfFormFields.SetField("topmostSubform[0].Page1[0].Asterisked_items_____must_be_completed_by_the_department[0]", CurrentDevice.strAssetTag)
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined[0]", CurrentDevice.strSerial)
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_2[0]", Munis_Get_VendorName_From_PO(CurrentDevice.strPO))
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_3[0]", CurrentDevice.strDescription)
            'pdfFormFields.SetField("topmostSubform[0].Page1[0]._1[0]", "6") 
            ' pdfFormFields.SetField("topmostSubform[0].Page1[0]._2[0]", "7")
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_4[0]", CurrentDevice.strPO)
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_5[0]", Get_MunisCode_From_AssetCode(CurrentDevice.strLocation))
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_6[0]", "5200")
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_7[0]", Get_MunisCode_From_AssetCode(CurrentDevice.strEqType))
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_8[0]", "GP")
            'pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_9[0]", "13")
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_10[0]", "1")
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_11[0]", strUnitPrice)
            pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_12[0]", CurrentDevice.dtPurchaseDate.ToString("MM/dd/yyyy"))
            pdfFormFields.SetField("topmostSubform[0].Page1[0].Date[0]", Now.ToString("MM/dd/yyyy"))
            pdfStamper.FormFlattening = True
            ' close the pdf
            pdfStamper.Close()
            Process.Start(newFile)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
End Module
