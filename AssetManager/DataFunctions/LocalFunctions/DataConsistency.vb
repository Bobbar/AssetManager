Imports System.Text.RegularExpressions

Module DataConsistency
    Public ReadOnly strDBDateTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Public ReadOnly strDBDateFormat As String = "yyyy-MM-dd"
    Public ReadOnly dtDefaultDate As Date = DateTime.Parse("1/1/0001 12:00:00 AM")

    Public Function NoNull(DBVal As Object) As String
        Try
            Return IIf(IsDBNull(DBVal), "", DBVal.ToString).ToString
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Trims, removes LF and CR chars and returns a DBNull if string is empty.
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    Public Function CleanDBValue(Value As String) As Object
        Dim CleanString As String = Regex.Replace(Trim(Value), "[/\r?\n|\r]+", String.Empty)
        Return IIf(CleanString = String.Empty, DBNull.Value, CleanString)
    End Function

    Public Function ValidPhoneNumber(PhoneNum As String) As Boolean
        If Trim(PhoneNum) <> "" Then
            Const nDigits As Integer = 10
            Dim fPhoneNum As String = ""
            Dim NumArray() As Char = PhoneNum.ToCharArray()
            For Each num As Char In NumArray
                If Char.IsDigit(num) Then fPhoneNum += num.ToString
            Next
            If Len(fPhoneNum) <> nDigits Then
                Return False
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function

    Public Function YearFromDate(dtDate As Date) As String
        Return dtDate.Year.ToString
    End Function

End Module