Imports System.Deployment.Application

Public Class SplashScreenForm

    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.FormBorderStyle = FormBorderStyle.None
        Using p As New Drawing2D.GraphicsPath()
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
        End Using
        If ApplicationDeployment.IsNetworkDeployed Then
            Version.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString
        Else
            Version.Text = "Debug"
        End If
        Copyright.Text = My.Application.Info.Copyright
    End Sub

    Overloads Sub Hide()
        Me.Dispose()
    End Sub

    Public Sub SetStatus(Text As String)
        lblStatus.Text = Text
        Me.Refresh()
    End Sub

End Class