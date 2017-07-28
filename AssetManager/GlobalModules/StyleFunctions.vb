Module StyleFunctions
    Public GridStyles As DataGridViewCellStyle
    Public GridFont As Font = New Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

    Public Function SetBarColor(UID As String) As Color
        Dim hash As Integer = UID.GetHashCode
        Dim r, g, b As Integer
        r = (hash And &HFF0000) >> 16
        g = (hash And &HFF00) >> 8
        b = hash And &HFF
        Return Color.FromArgb(r, g, b)
    End Function

    Public Function GetFontColor(color As Color) As Color 'get contrasting font color
        Dim d As Integer = 0
        Dim a As Double
        a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255
        If a < 0.5 Then
            d = 0
        Else
            d = 255
        End If
        Return Color.FromArgb(d, d, d)
    End Function

    Public Sub SetGridStyle(Grid As DataGridView)
        Grid.BackgroundColor = DefGridBC
        Grid.DefaultCellStyle = GridStyles
        Grid.DefaultCellStyle.Font = GridFont
    End Sub

    Public Sub HighlightRow(ByRef Grid As DataGridView, Theme As GridTheme, Row As Integer)
        Try
            Dim BackColor As Color = Theme.BackColor 'DefGridBC
            Dim SelectColor As Color = Theme.CellSelectColor 'DefGridSelCol
            Dim c1 As Color = Theme.RowHighlightColor 'colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In Grid.Rows(Row).Cells
                    Dim c2 As Color = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B)
                    Dim BlendColor As Color
                    BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                    CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                    CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                    CInt((CInt(c1.B) + CInt(c2.B)) / 2))
                    cell.Style.SelectionBackColor = BlendColor
                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                    BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                    CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                    CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                    CInt((CInt(c1.B) + CInt(c2.B)) / 2))
                    cell.Style.BackColor = BlendColor
                Next
            End If
        Catch
        End Try
    End Sub

    Public Sub LeaveRow(ByRef Grid As DataGridView, Theme As GridTheme, Row As Integer)
        Dim BackColor As Color = Theme.BackColor
        Dim SelectColor As Color = Theme.CellSelectColor
        If Row > -1 Then
            For Each cell As DataGridViewCell In Grid.Rows(Row).Cells
                cell.Style.SelectionBackColor = SelectColor
                cell.Style.BackColor = BackColor
            Next
        End If
    End Sub

End Module