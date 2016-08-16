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
        ' Dim pdfTemplate As String = "c:\Temp\PDF\fw4.pdf"

        ' title the form
        'Me.Text += " - " + pdfTemplate

        ' create a new PDF reader based on the PDF template document
        Dim pdfReader As PdfReader = New PdfReader(My.Resources.Exh_K_01_Asset_Input_Formnew)

        ' create and populate a string builder with each of the 
        ' field names available in the subject PDF
        Dim sb As New StringBuilder()

        Dim de As New KeyValuePair(Of String, iTextSharp.text.pdf.AcroFields.Item) 'DictionaryEntry
        For Each de In pdfReader.AcroFields.Fields
            sb.Append(de.Key.ToString() + Environment.NewLine)
        Next

        '  pdfReader.AcroFields.Item

        ' Write the string builder's content to the form's textbox
        Debug.Print(sb.ToString())
        'textBox1.SelectionStart = 0
    End Sub

    Public Sub FillForm()
        Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
        Dim pdfTemplate As String = "c:\Temp\PDF\fw4.pdf"
        Dim newFile As String = strTempPath & "Final_fw4.pdf"

        Dim pdfReader As New PdfReader(My.Resources.Exh_K_01_Asset_Input_Formnew)
        Dim pdfStamper As New PdfStamper(pdfReader, New FileStream(
            newFile, FileMode.Create))

        Dim pdfFormFields As AcroFields = pdfStamper.AcroFields

        ' set form pdfFormFields

        ' The first worksheet and W-4 form
        pdfFormFields.SetField("topmostSubform[0].Page1[0].Department[0]", "1")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].Asterisked_items_____must_be_completed_by_the_department[0]", "2")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined[0]", "3")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_2[0]", "4")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_3[0]", "5")
        pdfFormFields.SetField("topmostSubform[0].Page1[0]._1[0]", "6")
        pdfFormFields.SetField("topmostSubform[0].Page1[0]._2[0]", "7")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_4[0]", "8")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_5[0]", "9")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_6[0]", "10")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_7[0]", "11")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_8[0]", "12")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_9[0]", "13")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_10[0]", "14")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_11[0]", "15")
        pdfFormFields.SetField("topmostSubform[0].Page1[0].undefined_12[0]", "16")


        'pdfFormFields.SetField("f1_03(0)", "1")
        'pdfFormFields.SetField("f1_04(0)", "8")
        'pdfFormFields.SetField("f1_05(0)", "0")
        'pdfFormFields.SetField("f1_06(0)", "1")
        'pdfFormFields.SetField("f1_07(0)", "16")
        'pdfFormFields.SetField("f1_08(0)", "28")
        'pdfFormFields.SetField("f1_09(0)", "Franklin A.")
        'pdfFormFields.SetField("f1_10(0)", "Benefield")
        'pdfFormFields.SetField("f1_11(0)", "532")
        'pdfFormFields.SetField("f1_12(0)", "12")
        'pdfFormFields.SetField("f1_13(0)", "1234")


        ' report by reading values from completed PDF
        'Dim sTmp As String = "W-4 Completed for " +
        'pdfFormFields.GetField("f1_09(0)") + " " +
        'pdfFormFields.GetField("f1_10(0)")
        ' MessageBox.Show(sTmp, "Finished")

        ' flatten the form to remove editting options, set it to false
        ' to leave the form open to subsequent manual edits
        pdfStamper.FormFlattening = True

        ' close the pdf
        pdfStamper.Close()

    End Sub



End Module
