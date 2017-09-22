
Public Class GridTheme

    Sub New(highlightCol As Color, cellSelCol As Color, backCol As Color)
        RowHighlightColor = highlightCol
        CellSelectColor = cellSelCol
        BackColor = backCol
    End Sub

    Sub New()

    End Sub

    Public Property RowHighlightColor As Color
    Public Property CellSelectColor As Color
    Public Property BackColor As Color
End Class
