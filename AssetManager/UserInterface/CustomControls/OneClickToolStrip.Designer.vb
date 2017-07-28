<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OneClickToolStrip
    Inherits System.Windows.Forms.ToolStrip

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Const WM_LBUTTONDOWN As UInteger = &H201
    Const WM_LBUTTONUP As UInteger = &H202
    Private Shared down As Boolean = False

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_LBUTTONUP AndAlso Not down Then
            m.Msg = CInt(WM_LBUTTONDOWN)
            MyBase.WndProc(m)
            m.Msg = CInt(WM_LBUTTONUP)
        End If

        If m.Msg = WM_LBUTTONDOWN Then
            down = True
        End If
        If m.Msg = WM_LBUTTONUP Then
            down = False
        End If

        MyBase.WndProc(m)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        ' Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    End Sub

End Class
