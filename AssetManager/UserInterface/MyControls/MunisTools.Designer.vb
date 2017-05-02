<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MunisToolsMenu

    Inherits System.Windows.Forms.ToolStripDropDownButton

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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MunisTools = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsmDeviceLookUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmUserOrgObLookup = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmPOLookUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmReqNumLookUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmOrgObLookup = New System.Windows.Forms.ToolStripMenuItem()

        'Me.SuspendLayout()
        '
        'MunisTools
        '
        Me.MunisTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmUserOrgObLookup, Me.tsmPOLookUp, Me.tsmReqNumLookUp, Me.tsmDeviceLookUp, Me.tsmOrgObLookup})
        Me.MunisTools.Image = Global.AssetManager.My.Resources.Resources.Find
        Me.MunisTools.Name = "MunisTools"
        Me.MunisTools.Size = New System.Drawing.Size(87, 29)
        Me.MunisTools.Text = "MUNIS Tools"
        Me.AutoSize = True

        '
        'tsmDeviceLookUp
        '
        Me.tsmDeviceLookUp.Name = "tsmDeviceLookUp"
        Me.tsmDeviceLookUp.Size = New System.Drawing.Size(186, 22)
        Me.tsmDeviceLookUp.Text = "Device Lookup"
        '
        'tsmUserOrgObLookup
        '
        Me.tsmUserOrgObLookup.Name = "tsmUserOrgObLookup"
        Me.tsmUserOrgObLookup.Size = New System.Drawing.Size(186, 22)
        Me.tsmUserOrgObLookup.Text = "User Lookup"
        '
        'tsmUserOrgObLookup
        '
        Me.tsmOrgObLookup.Name = "tsmOrgObLookup"
        Me.tsmOrgObLookup.Size = New System.Drawing.Size(186, 22)
        Me.tsmOrgObLookup.Text = "Org/Obj Lookup"
        '
        'tsmPOLookUp
        '
        Me.tsmPOLookUp.Name = "tsmPOLookUp"
        Me.tsmPOLookUp.Size = New System.Drawing.Size(186, 22)
        Me.tsmPOLookUp.Text = "PO Lookup"
        '
        'tsmReqNumLookUp
        '
        Me.tsmReqNumLookUp.Name = "tsmReqNumLookUp"
        Me.tsmReqNumLookUp.Size = New System.Drawing.Size(186, 22)
        Me.tsmReqNumLookUp.Text = "Requisition # Lookup"
        '
        'MunisToolsMenu
        '
        Me.Name = "MunisToolsMenu"
        Me.Size = New System.Drawing.Size(248, 138)
        'Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MunisTools As ToolStripDropDownButton
    Friend WithEvents tsmDeviceLookUp As ToolStripMenuItem
    Friend WithEvents tsmUserOrgObLookup As ToolStripMenuItem
    Friend WithEvents tsmPOLookUp As ToolStripMenuItem
    Friend WithEvents tsmReqNumLookUp As ToolStripMenuItem
    Friend WithEvents tsmOrgObLookup As ToolStripMenuItem
End Class
