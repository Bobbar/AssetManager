Imports System.Drawing.Text
Public NotInheritable Class SplashScreen
    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.FormBorderStyle = FormBorderStyle.None
        'Me.Height = 300
        'Me.Width = 400
        Dim p As New Drawing2D.GraphicsPath()
        p.StartFigure()
        p.AddArc(New Rectangle(0, 0, 40, 40), 180, 90)
        p.AddLine(40, 0, Me.Width - 40, 0)
        p.AddArc(New Rectangle(Me.Width - 40, 0, 40, 40), -90, 90)
        p.AddLine(Me.Width, 40, Me.Width, Me.Height - 40)
        p.AddArc(New Rectangle(Me.Width - 40, Me.Height - 40, 40, 40), 0, 90)
        p.AddLine(Me.Width - 40, Me.Height, 40, Me.Height)
        p.AddArc(New Rectangle(0, Me.Height - 40, 40, 40), 90, 90)
        p.CloseFigure()
        Me.Region = New Region(p)
        ' Me.BackColor = Color.Red
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)
        Version.Text = My.Application.Info.Version.ToString 'System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.MajorRevision, My.Application.Info.Version.Minor)
        'Copyright info
        Copyright.Text = My.Application.Info.Copyright
    End Sub
    Private Sub SplashScreen1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
    End Sub
End Class
