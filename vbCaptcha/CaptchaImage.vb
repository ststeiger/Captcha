Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging


' http://www.codeproject.com/Articles/8751/A-CAPTCHA-Server-Control-for-ASP-NET


''' <summary>
''' CAPTCHA image generation class
''' </summary>
''' <remarks>
''' Adapted from the excellent code at 
''' http://www.codeproject.com/aspnet/CaptchaImage.asp
'''
''' Jeff Atwood
''' http://www.codinghorror.com/
''' </remarks>
Public Class CaptchaImage

    Private m_height As Integer
    Private m_width As Integer
    Private m_rand As Random
    Private m_generatedAt As DateTime
    Private m_randomText As String
    Private m_randomTextLength As Integer
    Private m_randomTextChars As String
    Private m_fontFamilyName As String
    Private m_fontWarp As FontWarpFactor
    Private m_backgroundNoise As BackgroundNoiseLevel
    Private m_lineNoise As LineNoiseLevel
    Private m_guid As String
    Private m_fontWhitelist As String


    ''' <summary>
    ''' Amount of random font warping to apply to rendered text
    ''' </summary>
    Public Enum FontWarpFactor
        None
        Low
        Medium
        High
        Extreme
    End Enum

    ''' <summary>
    ''' Amount of background noise to add to rendered image
    ''' </summary>
    Public Enum BackgroundNoiseLevel
        None
        Low
        Medium
        High
        Extreme
    End Enum

    ''' <summary>
    ''' Amount of curved line noise to add to rendered image
    ''' </summary>
    Public Enum LineNoiseLevel
        None
        Low
        Medium
        High
        Extreme
    End Enum





    ''' <summary>
    ''' Returns a GUID that uniquely identifies this Captcha
    ''' </summary>
    Public ReadOnly Property UniqueId() As String
        Get
            Return m_guid
        End Get
    End Property


    ''' <summary>
    ''' Returns the date and time this image was last rendered
    ''' </summary>
    Public ReadOnly Property RenderedAt() As DateTime
        Get
            Return m_generatedAt
        End Get
    End Property


    ''' <summary>
    ''' Font family to use when drawing the Captcha text. If no font is provided, a random font will be chosen from the font whitelist for each character.
    ''' </summary>
    Public Property Font() As String
        Get
            Return m_fontFamilyName
        End Get
        Set(ByVal Value As String)
            Try
                Dim font1 As Font = New Font(Value, 12.0!)
                m_fontFamilyName = Value
                font1.Dispose()
            Catch ex As Exception
                m_fontFamilyName = Drawing.FontFamily.GenericSerif.Name
            End Try
        End Set
    End Property


    ''' <summary>
    ''' Amount of random warping to apply to the Captcha text.
    ''' </summary>
    Public Property FontWarp() As FontWarpFactor
        Get
            Return m_fontWarp
        End Get
        Set(ByVal Value As FontWarpFactor)
            m_fontWarp = Value
        End Set
    End Property


    ''' <summary>
    ''' Amount of background noise to apply to the Captcha image.
    ''' </summary>
    Public Property BackgroundNoise() As BackgroundNoiseLevel
        Get
            Return m_backgroundNoise
        End Get
        Set(ByVal Value As BackgroundNoiseLevel)
            m_backgroundNoise = Value
        End Set
    End Property


    Public Property LineNoise() As LineNoiseLevel
        Get
            Return m_lineNoise
        End Get
        Set(ByVal value As LineNoiseLevel)
            m_lineNoise = value
        End Set
    End Property


    ''' <summary>
    ''' A string of valid characters to use in the Captcha text. 
    ''' A random character will be selected from this string for each character.
    ''' </summary>
    Public Property TextChars() As String
        Get
            Return m_randomTextChars
        End Get
        Set(ByVal Value As String)
            m_randomTextChars = Value
            m_randomText = GenerateRandomText()
        End Set
    End Property


    ''' <summary>
    ''' Number of characters to use in the Captcha text. 
    ''' </summary>
    Public Property TextLength() As Integer
        Get
            Return m_randomTextLength
        End Get
        Set(ByVal Value As Integer)
            m_randomTextLength = Value
            m_randomText = GenerateRandomText()
        End Set
    End Property


    ''' <summary>
    ''' Returns the randomly generated Captcha text.
    ''' </summary>
    Public ReadOnly Property [Text]() As String
        Get
            Return m_randomText
        End Get
    End Property


    ''' <summary>
    ''' Width of Captcha image to generate, in pixels 
    ''' </summary>
    Public Property Width() As Integer
        Get
            Return m_width
        End Get
        Set(ByVal Value As Integer)
            If (Value <= 60) Then
                Throw New ArgumentOutOfRangeException("width", Value, "width must be greater than 60.")
            End If
            m_width = Value
        End Set
    End Property


    ''' <summary>
    ''' Height of Captcha image to generate, in pixels 
    ''' </summary>
    Public Property Height() As Integer
        Get
            Return m_height
        End Get
        Set(ByVal Value As Integer)
            If Value <= 30 Then
                Throw New ArgumentOutOfRangeException("height", Value, "height must be greater than 30.")
            End If
            m_height = Value
        End Set
    End Property


    ''' <summary>
    ''' A semicolon-delimited list of valid fonts to use when no font is provided.
    ''' </summary>
    Public Property FontWhitelist() As String
        Get
            Return m_fontWhitelist
        End Get
        Set(ByVal value As String)
            m_fontWhitelist = value
        End Set
    End Property



    Public Sub New()
        m_rand = New Random
        m_fontWarp = FontWarpFactor.Low
        m_backgroundNoise = BackgroundNoiseLevel.Low
        m_lineNoise = LineNoiseLevel.None
        m_width = 180
        m_height = 50
        m_randomTextLength = 5
        m_randomTextChars = "ACDEFGHJKLNPQRTUVXYZ2346789"
        m_fontFamilyName = ""
        ' -- a list of known good fonts in on both Windows XP and Windows Server 2003
        m_fontWhitelist = _
            "arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;" & _
            "georgia;lucida console;lucida sans unicode;mangal;microsoft sans serif;palatino linotype;" & _
            "sylfaen;tahoma;times new roman;trebuchet ms;verdana"
        m_randomText = GenerateRandomText()
        m_generatedAt = DateTime.Now
        m_guid = Guid.NewGuid.ToString()
    End Sub


    ''' <summary>
    ''' Forces a new Captcha image to be generated using current property value settings.
    ''' </summary>
    Public Function RenderImage() As Bitmap
        Return GenerateImagePrivate()
    End Function


    ''' <summary>
    ''' Returns a random font family from the font whitelist
    ''' </summary>
    Private Function RandomFontFamily() As String
        Static ff() As String
        '-- small optimization so we don't have to split for each char
        If ff Is Nothing Then
            ff = m_fontWhitelist.Split(";"c)
        End If
        Return ff(m_rand.Next(0, ff.Length))
    End Function


    ''' <summary>
    ''' generate random text for the CAPTCHA
    ''' </summary>
    Private Function GenerateRandomText() As String
        Dim sb As New System.Text.StringBuilder(m_randomTextLength)
        Dim maxLength As Integer = m_randomTextChars.Length
        For n As Integer = 0 To m_randomTextLength - 1
            sb.Append(m_randomTextChars.Substring(m_rand.Next(maxLength), 1))
        Next
        Return sb.ToString
    End Function


    ''' <summary>
    ''' Returns a random point within the specified x and y ranges
    ''' </summary>
    Private Function RandomPoint(ByVal xmin As Integer, ByVal xmax As Integer, ByRef ymin As Integer, ByRef ymax As Integer) As PointF
        Return New PointF(m_rand.Next(xmin, xmax), m_rand.Next(ymin, ymax))
    End Function


    ''' <summary>
    ''' Returns a random point within the specified rectangle
    ''' </summary>
    Private Function RandomPoint(ByVal rect As Rectangle) As PointF
        Return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom)
    End Function


    ''' <summary>
    ''' Returns a GraphicsPath containing the specified string and font
    ''' </summary>
    Private Function TextPath(ByVal s As String, ByVal f As Font, ByVal r As Rectangle) As GraphicsPath
        Dim sf As StringFormat = New StringFormat
        sf.Alignment = StringAlignment.Near
        sf.LineAlignment = StringAlignment.Near
        Dim gp As GraphicsPath = New GraphicsPath
        gp.AddString(s, f.FontFamily, CType(f.Style, Integer), f.Size, r, sf)
        Return gp
    End Function


    ''' <summary>
    ''' Returns the CAPTCHA font in an appropriate size 
    ''' </summary>
    Private Function GetFont() As Font
        Dim fsize As Single
        Dim fname As String = m_fontFamilyName

        If fname = "" Then
            fname = RandomFontFamily()
        End If

        Select Case Me.FontWarp
            Case FontWarpFactor.None
                fsize = Convert.ToInt32(m_height * 0.7)
            Case FontWarpFactor.Low
                fsize = Convert.ToInt32(m_height * 0.8)
            Case FontWarpFactor.Medium
                fsize = Convert.ToInt32(m_height * 0.85)
            Case FontWarpFactor.High
                fsize = Convert.ToInt32(m_height * 0.9)
            Case FontWarpFactor.Extreme
                fsize = Convert.ToInt32(m_height * 0.95)
        End Select

        Return New Font(fname, fsize, FontStyle.Bold)
    End Function


    ''' <summary>
    ''' Renders the CAPTCHA image
    ''' </summary>
    Private Function GenerateImagePrivate() As Bitmap
        Dim fnt As Font = Nothing
        Dim rect As Rectangle
        Dim br As Brush
        Dim bmp As Bitmap = New Bitmap(m_width, m_height, PixelFormat.Format32bppArgb)
        Dim gr As Graphics = Graphics.FromImage(bmp)
        gr.SmoothingMode = SmoothingMode.AntiAlias

        '-- fill an empty white rectangle
        rect = New Rectangle(0, 0, m_width, m_height)
        br = New SolidBrush(Color.White)
        gr.FillRectangle(br, rect)

        Dim charOffset As Integer = 0
        Dim charWidth As Double = m_width / m_randomTextLength
        Dim rectChar As Rectangle

        For Each c As Char In m_randomText
            '-- establish font and draw area
            fnt = GetFont()
            rectChar = New Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), m_height)

            '-- warp the character
            Dim gp As GraphicsPath = TextPath(c, fnt, rectChar)
            WarpText(gp, rectChar)

            '-- draw the character
            br = New SolidBrush(Color.Black)
            gr.FillPath(br, gp)

            charOffset += 1
        Next

        AddNoise(gr, rect)
        AddLine(gr, rect)

        '-- clean up unmanaged resources
        fnt.Dispose()
        br.Dispose()
        gr.Dispose()

        Return bmp
    End Function


    ''' <summary>
    ''' Warp the provided text GraphicsPath by a variable amount
    ''' </summary>
    Private Sub WarpText(ByVal textPath As GraphicsPath, ByVal rect As Rectangle)
        Dim WarpDivisor As Single
        Dim RangeModifier As Single

        Select Case m_fontWarp
            Case FontWarpFactor.None
                Return
            Case FontWarpFactor.Low
                WarpDivisor = 6
                RangeModifier = 1
            Case FontWarpFactor.Medium
                WarpDivisor = 5
                RangeModifier = 1.3
            Case FontWarpFactor.High
                WarpDivisor = 4.5
                RangeModifier = 1.4
            Case FontWarpFactor.Extreme
                WarpDivisor = 4
                RangeModifier = 1.5
        End Select

        Dim rectF As RectangleF
        rectF = New RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height)

        Dim hrange As Integer = Convert.ToInt32(rect.Height / WarpDivisor)
        Dim wrange As Integer = Convert.ToInt32(rect.Width / WarpDivisor)
        Dim left As Integer = rect.Left - Convert.ToInt32(wrange * RangeModifier)
        Dim top As Integer = rect.Top - Convert.ToInt32(hrange * RangeModifier)
        Dim width As Integer = rect.Left + rect.Width + Convert.ToInt32(wrange * RangeModifier)
        Dim height As Integer = rect.Top + rect.Height + Convert.ToInt32(hrange * RangeModifier)

        If left < 0 Then left = 0
        If top < 0 Then top = 0
        If width > Me.Width Then width = Me.Width
        If height > Me.Height Then height = Me.Height

        Dim leftTop As PointF = RandomPoint(left, left + wrange, top, top + hrange)
        Dim rightTop As PointF = RandomPoint(width - wrange, width, top, top + hrange)
        Dim leftBottom As PointF = RandomPoint(left, left + wrange, height - hrange, height)
        Dim rightBottom As PointF = RandomPoint(width - wrange, width, height - hrange, height)

        Dim points As PointF() = New PointF() {leftTop, rightTop, leftBottom, rightBottom}
        Dim m As New Matrix
        m.Translate(0, 0)
        textPath.Warp(points, rectF, m, WarpMode.Perspective, 0)
    End Sub


    ''' <summary>
    ''' Add a variable level of graphic noise to the image
    ''' </summary>
    Private Sub AddNoise(ByVal graphics1 As Graphics, ByVal rect As Rectangle)
        Dim density As Integer
        Dim size As Integer

        Select Case m_backgroundNoise
            Case BackgroundNoiseLevel.None
                Return
            Case BackgroundNoiseLevel.Low
                density = 30
                size = 40
            Case BackgroundNoiseLevel.Medium
                density = 18
                size = 40
            Case BackgroundNoiseLevel.High
                density = 16
                size = 39
            Case BackgroundNoiseLevel.Extreme
                density = 12
                size = 38
        End Select

        Dim br As New SolidBrush(Color.Black)
        Dim max As Integer = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size)

        For i As Integer = 0 To Convert.ToInt32((rect.Width * rect.Height) / density)
            graphics1.FillEllipse(br, m_rand.Next(rect.Width), m_rand.Next(rect.Height), _
                m_rand.Next(max), m_rand.Next(max))
        Next
        br.Dispose()
    End Sub


    ''' <summary>
    ''' Add variable level of curved lines to the image
    ''' </summary>
    Private Sub AddLine(ByVal graphics1 As Graphics, ByVal rect As Rectangle)

        Dim length As Integer
        Dim width As Single
        Dim linecount As Integer

        Select Case m_lineNoise
            Case LineNoiseLevel.None
                Return
            Case LineNoiseLevel.Low
                length = 4
                width = Convert.ToSingle(m_height / 31.25) ' 1.6
                linecount = 1
            Case LineNoiseLevel.Medium
                length = 5
                width = Convert.ToSingle(m_height / 27.7777) ' 1.8
                linecount = 1
            Case LineNoiseLevel.High
                length = 3
                width = Convert.ToSingle(m_height / 25) ' 2.0
                linecount = 2
            Case LineNoiseLevel.Extreme
                length = 3
                width = Convert.ToSingle(m_height / 22.7272) ' 2.2
                linecount = 3
        End Select

        Dim pf(length) As PointF
        Dim p As New Pen(Color.Black, width)

        For l As Integer = 1 To linecount
            For i As Integer = 0 To length
                pf(i) = RandomPoint(rect)
            Next
            graphics1.DrawCurve(p, pf, 1.75)
        Next l

        p.Dispose()
    End Sub


End Class
