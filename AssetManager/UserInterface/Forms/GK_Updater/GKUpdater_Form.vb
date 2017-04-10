Public Class GKUpdater_Form
    Private MyUpdates As New List(Of GK_Progress_Fragment)
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Show()

    End Sub

    Public Sub AddUpdate(ByRef Updater As GK_Updater)

        Updater_Table.RowStyles.Clear()
        Dim NewProgCtl As New GK_Progress_Fragment(Updater, MyUpdates.Count + 1)
        Updater_Table.Controls.Add(NewProgCtl)
        MyUpdates.Add(NewProgCtl)

    End Sub
End Class