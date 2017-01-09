
Imports System.Drawing


Public Class Captcha3D
    Inherits AbstractCaptcha


    Public Overrides Property Image() As Image

        Get
            Return Generate(Me.Text)
        End Get
        Set(value As Image)
        End Set
    End Property

    ' End Property Image

    Private Shared Function Generate(captchaText As String) As System.Drawing.Image
        Dim coord As Double()() = Nothing
        Dim image2d_x As Integer = 0
        Dim image2d_y As Integer = 0


        Dim bevel As Double = 3
        Dim fontSize As Single = 20.0F
        Dim fontFamily As String = "Arial"
        ' fontFamily = "Algerian";



        ' Calculate projection matrix
        ' new double[] { 0, -200, 250 },
        Dim T As Double() = MathHelpers.cameraTransform(New Double() {MathHelpers.rand(-90, 90), -200, MathHelpers.rand(150, 250)}, New Double() {0, 0, 0})


        'MathHelpers.viewingTransform(15, 30, 3000)
        T = MathHelpers.matrixProduct(T, MathHelpers.viewingTransform(60, 300, 3000))


        ' string fontFile = FileHelper.MapProjectPath("Img/3DCaptcha.ttf");
        ' fontFile = Helpers.MapProjectPath("Img/ALGER.ttf");

        ' using (System.Drawing.Font font = GetCustomFont(fontFile, fontSize))
        Using font As New System.Drawing.Font(fontFamily, fontSize, System.Drawing.FontStyle.Bold)
            Using bmp As New System.Drawing.Bitmap(1, 1)
                Using g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bmp)
                    Dim size As System.Drawing.SizeF = g.MeasureString(captchaText, font, New System.Drawing.PointF(10, 10), System.Drawing.StringFormat.GenericTypographic)
                    image2d_x = CInt(size.Width * 1.1) + 5
                    image2d_y = CInt(size.Height) + 5
                    ' End Using g 
                End Using
            End Using
            ' End Using bmp 
            Using image2d As New System.Drawing.Bitmap(image2d_x, image2d_y)

                Using g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(image2d)
                    g.Clear(System.Drawing.Color.Black)
                    g.DrawString(captchaText, font, System.Drawing.Brushes.White, New System.Drawing.PointF(1, -1))
                    ' image2d.Save(FileHelper.MapProjectPath("Img/ftstring.png"), System.Drawing.Imaging.ImageFormat.Png);


                    coord = New Double(image2d_x * image2d_y - 1)() {}
                    ' { image2d_x * image2d_y };
                    ' Calculate coordinates
                    Dim count As Integer = 0
                    For y As Integer = 0 To image2d_y - 1 Step 2
                        For x As Integer = 0 To image2d_x - 1
                            ' Calculate x1, y1, x2, y2
                            Dim xc As Double = x - image2d_x / 2.0
                            Dim zc As Double = y - image2d_y / 2.0

                            Dim yc As Double = -(image2d.GetPixel(x, y).ToArgb() And &HFF) / 256.0 * bevel
                            Dim xyz As Double() = New Double() {xc, yc, zc, 1}
                            xyz = MathHelpers.vectorProduct(xyz, T)

                            coord(count) = xyz
                            count += 1
                            ' Next x 
                        Next
                        ' Next y
                    Next
                    ' End Using g
                End Using
                ' End Using image2d
            End Using
        End Using
        ' End Using font


        ' Create 3d image
        Dim image3d_x As Integer = 256
        'image3d_y = image3d_x / 1.618;
        Dim image3d_y As Integer = image3d_x * 9 / 16

        ' image3d_x = 256 * 4;
        ' image3d_y = (int)(image3d_x * 0.05);


        ' using (
        Dim image3d As New System.Drawing.Bitmap(image3d_x, image3d_y)
        ')
        If True Then
            Using g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(image3d)
                'g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias

                ' g.Clear(System.Drawing.Color.White);

                Dim rect As New System.Drawing.Rectangle(0, 0, image3d.Width, image3d.Height)

                ', System.Drawing.Color.Black
                Dim hatchBrush As New System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.LightDownwardDiagonal, System.Drawing.Color.Blue, System.Drawing.Color.Lavender)

                g.FillRectangle(hatchBrush, rect)




                Dim count As Integer = 0
                Dim scale As Double = 1.75 - image2d_x / 400.0

                For y As Integer = 0 To image2d_y - 1
                    For x As Integer = 0 To image2d_x - 1
                        If x > 0 Then
                            If coord(count - 1) Is Nothing Then
                                Continue For
                            End If

                            Dim x0 As Double = coord(count - 1)(0) * scale + image3d_x / 2.0
                            Dim y0 As Double = coord(count - 1)(1) * scale + image3d_y / 2.0
                            Dim x1 As Double = coord(count)(0) * scale + image3d_x / 2.0
                            Dim y1 As Double = coord(count)(1) * scale + image3d_y / 2.0

                            'using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Blue, 0.25f)) { g.DrawLine(pen, (int)x0, (int)y0, (int)x1, (int)y1); }
                            ' g.DrawLine(System.Drawing.Pens.White, (int)x0, (int)y0, (int)x1, (int)y1);
                            g.DrawLine(System.Drawing.Pens.Blue, CInt(x0), CInt(y0), CInt(x1), CInt(y1))
                        End If '  (x > 0) 

                        count += 1
                    Next x

                Next y
            End Using '  g


            ' using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            ' {
            '      image3d.Save(ms, format);
            '      imageBytes = ms.ToArray();
            ' } // End Using ms 

        End If
        ' End using image3d 
        Return image3d
    End Function ' Generate 


End Class ' Captcha3d
