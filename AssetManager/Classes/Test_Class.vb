Imports System.Collections.Generic
Imports System.Threading
Public Class Test_Class
    Sub New(ByRef ValueToUpdate As Integer)
        ReturnNumbers(ValueToUpdate)

    End Sub


    Private Sub ReturnNumbers(ByRef Value As Integer)
        For i As Integer = 0 To 20

            Value = i
            Thread.Sleep(250)
            Application.DoEvents()
        Next


    End Sub
End Class
