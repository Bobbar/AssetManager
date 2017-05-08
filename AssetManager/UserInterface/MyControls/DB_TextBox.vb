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

        ' This call is required by the designer.
        InitializeComponent()

        db_required = False

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Sub New(DataColumn As String, Optional Required As Boolean = False)

        ' This call is required by the designer.
        InitializeComponent()

        db_column = DataColumn
        db_required = Required
        ' Add any initialization after the InitializeComponent() call.

    End Sub

End Class
