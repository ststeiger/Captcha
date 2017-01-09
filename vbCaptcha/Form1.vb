
Public Class Form1


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim x As New CaptchaImage()
        'x.Text = ""
        x.TextLength = 5
        x.TextChars = "abcdef"

        Dim img As System.Drawing.Image = x.RenderImage()
        Me.PictureBox1.Image = img


        Dim ac As AbstractCaptcha = New Captcha3D()
        Me.PictureBox1.Image = ac.Image

        MsgBox(ac.Image.Size.Width)
        MsgBox(ac.Image.Size.Height)


    End Sub


End Class
