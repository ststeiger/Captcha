Public Class Form1



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim x As New CaptchaImage()
        'x.Text = ""
        x.TextLength = 5
        x.TextChars = ""
        x.

        Dim img As System.Drawing.Image = x.RenderImage()


    End Sub
End Class
