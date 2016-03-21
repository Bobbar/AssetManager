<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ResultGrid = New System.Windows.Forms.DataGridView()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGridHistory = New System.Windows.Forms.DataGridView()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 58)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(885, 599)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button2)
        Me.TabPage1.Controls.Add(Me.ResultGrid)
        Me.TabPage1.Controls.Add(Me.cmdSearch)
        Me.TabPage1.Controls.Add(Me.txtAssetTag)
        Me.TabPage1.Controls.Add(Me.txtSerial)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(877, 573)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Search"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(425, 191)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(69, 56)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ResultGrid
        '
        Me.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ResultGrid.Location = New System.Drawing.Point(36, 303)
        Me.ResultGrid.Name = "ResultGrid"
        Me.ResultGrid.Size = New System.Drawing.Size(808, 251)
        Me.ResultGrid.TabIndex = 3
        '
        'cmdSearch
        '
        Me.cmdSearch.Location = New System.Drawing.Point(567, 48)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(110, 56)
        Me.cmdSearch.TabIndex = 2
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Location = New System.Drawing.Point(44, 95)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(95, 20)
        Me.txtAssetTag.TabIndex = 1
        Me.txtAssetTag.Text = "%ASSET_TAG%"
        '
        'txtSerial
        '
        Me.txtSerial.Location = New System.Drawing.Point(44, 57)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(96, 20)
        Me.txtSerial.TabIndex = 0
        Me.txtSerial.Text = "%SERIAL%"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.DataGridHistory)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(877, 573)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "View"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(877, 573)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Manage"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(14, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(78, 27)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "New"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridHistory
        '
        Me.DataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridHistory.Location = New System.Drawing.Point(23, 253)
        Me.DataGridHistory.Name = "DataGridHistory"
        Me.DataGridHistory.Size = New System.Drawing.Size(837, 280)
        Me.DataGridHistory.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(925, 669)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents cmdSearch As Button
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents Button1 As Button
    Friend WithEvents ResultGrid As DataGridView
    Friend WithEvents Button2 As Button
    Friend WithEvents DataGridHistory As DataGridView
End Class
