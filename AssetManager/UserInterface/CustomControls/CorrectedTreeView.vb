Public Class CorrectedTreeView
    Inherits TreeView

    Protected Overrides Sub WndProc(ByRef m As Message)
        ' Suppress WM_LBUTTONDBLCLK
        ' Fixes bug that has existed since Vista...  (╯°□°)╯︵ ┻━┻
        If m.Msg = &H203 Then
            m.Result = IntPtr.Zero
        Else
            MyBase.WndProc(m)
        End If
    End Sub

End Class