Public Class Form1



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim x As New CaptchaImage()
        'x.Text = ""
        x.TextLength = 5
        x.TextChars = "abcdef"
        
        Dim img As System.Drawing.Image = x.RenderImage()
        Me.PictureBox1.Image = img

    End Sub


End Class
