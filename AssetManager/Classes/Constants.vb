Module Constants
    Public Const strDBDateTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Public Const strDBDateFormat As String = "yyyy-MM-dd"
    Public dtDefaultDate As Date = DateTime.Parse("1/1/0001 12:00:00 AM")
    Public ReadOnly intReplacementSched As Integer = 4
    Public ReadOnly Property strLocalUser As String = Environment.UserName
End Module
