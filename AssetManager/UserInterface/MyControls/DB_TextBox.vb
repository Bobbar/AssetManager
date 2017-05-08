Public Class DB_TextBox
    Inherits TextBox
    Private db_column As String
    Private db_required As Boolean
    Public Property DataColumn As String
        Get
            Return db_column
        End Get
        Set(value As String)
            db_column = value
        End Set
    End Property
    Public Property Required As Boolean
        Get
            Return db_required
        End Get
        Set(value As Boolean)
            db_required = value
        End Set
    End Property
    Sub New()
        MyBase.New
        ' This call is required by the designer.
        InitializeComponent()

        db_required = False
        db_column = ""
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub SetDBProps(ByVal DataColumn As String, Optional Required As Boolean = False)
        db_column = DataColumn
        db_required = Required
    End Sub
End Class
