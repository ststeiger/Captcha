
Public MustInherit Class AbstractCaptcha

    Private m_guid As System.Nullable(Of System.Guid)
    Private m_text As String
    Private m_imageFormat As System.Drawing.Imaging.ImageFormat
    Private m_MimeType As String


    Public Overridable Property UID() As System.Guid
        Get
            If Me.m_guid.HasValue Then
                Return Me.m_guid.Value
            End If

            Me.m_guid = System.Guid.NewGuid()
            Return Me.m_guid.Value
        End Get
        Set(value As System.Guid)
            Me.m_guid = value
        End Set
    End Property


    Public Overridable Property Text() As String
        Get
            If String.IsNullOrEmpty(Me.m_text) Then
                Me.m_text = Me.RandomToken
            End If

            Return Me.m_text
        End Get
        Set(value As String)
            Me.m_text = value
        End Set
    End Property


    Public Overridable ReadOnly Property RandomToken() As String
        Get
            Dim r As New System.Random()
            Dim s As String = ""
            For j As Integer = 0 To 4
                Dim i As Integer = r.[Next](3)
                Dim ch As Integer
                Select Case i
                    Case 1
                        ch = r.[Next](0, 9)
                        s = s & ch.ToString()
                        Exit Select
                    Case 2
                        ch = r.[Next](65, 90)
                        s = s & System.Convert.ToChar(ch).ToString()
                        Exit Select
                    Case 3
                        ch = r.[Next](97, 122)
                        s = s & System.Convert.ToChar(ch).ToString()
                        Exit Select
                    Case Else
                        ch = r.[Next](97, 122)
                        s = s & System.Convert.ToChar(ch).ToString()
                        Exit Select
                End Select
                r.NextDouble()
                r.[Next](100, 1999)
            Next
            Return s
        End Get
    End Property ' RandomToken 

    Public Overridable Property Format() As System.Drawing.Imaging.ImageFormat
        Get
            If m_imageFormat Is Nothing Then
                m_imageFormat = System.Drawing.Imaging.ImageFormat.Png
            End If

            Return m_imageFormat
        End Get
        Set(value As System.Drawing.Imaging.ImageFormat)
            Me.m_imageFormat = value
        End Set
    End Property


    Public Overridable ReadOnly Property MimeType() As String
        Get
            If Not String.IsNullOrEmpty(m_MimeType) Then
                Return Me.m_MimeType
            End If

            Dim formatUID As System.Guid = Me.Format.Guid

            Dim codecs As System.Drawing.Imaging.ImageCodecInfo() = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
            For i As Integer = 0 To codecs.Length - 1
                If codecs(i).FormatID = formatUID Then
                    Me.m_MimeType = codecs(i).MimeType
                    Return Me.m_MimeType

                End If
            Next

            Me.m_MimeType = "image/unknown"
            Return Me.m_MimeType
        End Get
    End Property


    Public MustOverride Property Image() As System.Drawing.Image


    Public Overridable ReadOnly Property ImageBytes() As Byte()
        Get
            Dim imageBytes__1 As Byte() = Nothing

            Using ms As New System.IO.MemoryStream()
                Me.Image.Save(ms, Me.Format)
                imageBytes__1 = ms.ToArray()
            End Using

            Return imageBytes__1
        End Get
    End Property


    Public Overridable ReadOnly Property Base64() As String
        Get
            Return System.Convert.ToBase64String(Me.ImageBytes)
        End Get
    End Property


End Class ' AbstractCaptcha 
