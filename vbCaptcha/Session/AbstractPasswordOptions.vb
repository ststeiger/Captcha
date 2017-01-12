

Namespace Captcha


    Public Class AbstractPasswordOptions

        Private m_Seed As Cryptography.CryptoRandom

        Public Overridable Property RandomNumberGenerator() As Cryptography.CryptoRandom
            Get
                If m_Seed IsNot Nothing AndAlso m_Seed.Disposed Then
                    m_Seed = Nothing
                End If

                If m_Seed Is Nothing Then
                    m_Seed = New Cryptography.CryptoRandom()
                End If

                Return Me.m_Seed
            End Get
            Set
                Me.m_Seed = Value
            End Set
        End Property


        Public Overridable Property PASSWORD_CHARS_LCASE() As String
            Get
                Return m_PASSWORD_CHARS_LCASE
            End Get
            Set
                m_PASSWORD_CHARS_LCASE = Value
            End Set
        End Property

        Private m_PASSWORD_CHARS_LCASE As String

        Public Overridable Property PASSWORD_CHARS_UCASE() As String
            Get
                Return m_PASSWORD_CHARS_UCASE
            End Get
            Set
                m_PASSWORD_CHARS_UCASE = Value
            End Set
        End Property


        Private m_PASSWORD_CHARS_UCASE As String

        Public Overridable Property PASSWORD_CHARS_NUMERIC() As String
            Get
                Return m_PASSWORD_CHARS_NUMERIC
            End Get
            Set
                m_PASSWORD_CHARS_NUMERIC = Value
            End Set
        End Property


        Private m_PASSWORD_CHARS_NUMERIC As String

        Public Overridable Property PASSWORD_CHARS_SPECIAL() As String
            Get
                Return m_PASSWORD_CHARS_SPECIAL
            End Get
            Set
                m_PASSWORD_CHARS_SPECIAL = Value
            End Set
        End Property


        Private m_PASSWORD_CHARS_SPECIAL As String

        Public Overridable Property NumberOfLowerCaseCharacters() As Integer
            Get
                Return m_NumberOfLowerCaseCharacters
            End Get
            Set
                m_NumberOfLowerCaseCharacters = Value
            End Set
        End Property


        Private m_NumberOfLowerCaseCharacters As Integer

        Public Overridable Property NumberOfUpperCaseCharacters() As Integer
            Get
                Return m_NumberOfUpperCaseCharacters
            End Get
            Set
                m_NumberOfUpperCaseCharacters = Value
            End Set
        End Property


        Private m_NumberOfUpperCaseCharacters As Integer

        Public Overridable Property NumberOfNumericCharacters() As Integer
            Get
                Return m_NumberOfNumericCharacters
            End Get
            Set
                m_NumberOfNumericCharacters = Value
            End Set
        End Property


        Private m_NumberOfNumericCharacters As Integer

        Public Overridable Property NumberOfSpecialCharacters() As Integer
            Get
                Return m_NumberOfSpecialCharacters
            End Get
            Set
                m_NumberOfSpecialCharacters = Value
            End Set
        End Property


        Private m_NumberOfSpecialCharacters As Integer

        Public Overridable ReadOnly Property TotalPasswordLength() As Integer
            Get
                Return Me.NumberOfLowerCaseCharacters + Me.NumberOfUpperCaseCharacters + Me.NumberOfNumericCharacters + Me.NumberOfSpecialCharacters
            End Get
        End Property

    End Class


    Public Class GenericPasswordOptions
        Inherits AbstractPasswordOptions

    End Class
    ' End Class GenericPasswordOptions 

    Public Class SafePasswordOptions
        Inherits AbstractPasswordOptions

        Public Sub New()
            Me.PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz"
            Me.PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ"
            Me.PASSWORD_CHARS_NUMERIC = "23456789"
            Me.PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}"

            Me.NumberOfLowerCaseCharacters = 5
            Me.NumberOfUpperCaseCharacters = 1
            Me.NumberOfNumericCharacters = 1
            Me.NumberOfSpecialCharacters = 1
        End Sub
        ' End Constructor 
    End Class
    ' End Class SafePasswordOptions 

    Public Class EnglishPasswordOptions
        Inherits AbstractPasswordOptions

        Public Sub New()
            ' this.PASSWORD_CHARS_LCASE = "abcdefghijkmnopqrstwxyz";
            Me.PASSWORD_CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz"

            ' this.PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            Me.PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

            ' this.PASSWORD_CHARS_NUMERIC = "23456789";
            Me.PASSWORD_CHARS_NUMERIC = "0123456789"

            ' this.PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
            Me.PASSWORD_CHARS_SPECIAL = "*-+=_&!?$%{}()[]/\|.:;"


            Me.NumberOfLowerCaseCharacters = 5
            Me.NumberOfUpperCaseCharacters = 1
            Me.NumberOfNumericCharacters = 3
            Me.NumberOfSpecialCharacters = 1
        End Sub
        ' End Constructor 
    End Class
    ' End Class EnglishPasswordOptions 

    Public Class CyrillicPasswordOptions
        Inherits AbstractPasswordOptions

        Public Sub New()
            ' this.PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            Me.PASSWORD_CHARS_LCASE = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"

            ' this.PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            Me.PASSWORD_CHARS_UCASE = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"

            ' this.PASSWORD_CHARS_NUMERIC = "23456789";
            Me.PASSWORD_CHARS_NUMERIC = "0123456789"

            ' this.PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
            Me.PASSWORD_CHARS_SPECIAL = "*-+=_&!?$%{}()[]/\|.:;"


            Me.NumberOfLowerCaseCharacters = 5
            Me.NumberOfUpperCaseCharacters = 1
            Me.NumberOfNumericCharacters = 3
            Me.NumberOfSpecialCharacters = 1
        End Sub
        ' End Constructor 
    End Class
    ' End Class CyrillicPasswordOptions 

    Public Class GermanPasswordOptions
        Inherits AbstractPasswordOptions

        Public Sub New()
            ' PASSWORD_CHARS_LCASE = "aäbcdefgiïjklmnoöpqrsßtuüvwxyz";
            ' PASSWORD_CHARS_UCASE = "AÄBCDEFGHIÏJKLMNOÖPQRSTUÜVWXYZ";

            PASSWORD_CHARS_LCASE = "aäbcdefghijklmnoöpqrsßtuüvwxyz"
            PASSWORD_CHARS_UCASE = "AÄBCDEFGHIJKLMNOÖPQRSTUÜVWXYZ"

            PASSWORD_CHARS_NUMERIC = "0123456789"
            PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\|.:;"
            NumberOfLowerCaseCharacters = 5
            NumberOfUpperCaseCharacters = 1
            NumberOfNumericCharacters = 3
            NumberOfSpecialCharacters = 1
        End Sub
        ' End Constructor 
    End Class
    ' End Class GermanPasswordOptions 

    Public Class SwissPasswordOptions
        Inherits AbstractPasswordOptions

        Public Sub New()
            PASSWORD_CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz"
            PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            PASSWORD_CHARS_NUMERIC = "0123456789"
            PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\|.:;"
            NumberOfLowerCaseCharacters = 5
            NumberOfUpperCaseCharacters = 1
            NumberOfNumericCharacters = 3
            NumberOfSpecialCharacters = 1
        End Sub
        ' End Constructor 
    End Class
    ' End Class SwissPasswordOptions 

    Public Class SessionUidOptions
        Inherits AbstractPasswordOptions

        Public Sub New()
            Me.PASSWORD_CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz"
            Me.PASSWORD_CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Me.PASSWORD_CHARS_NUMERIC = "0123456789"
            Me.PASSWORD_CHARS_SPECIAL = ""


            Me.NumberOfNumericCharacters = Me.RandomNumberGenerator.[Next](0, 10)
            Me.NumberOfLowerCaseCharacters = 24 - Me.NumberOfNumericCharacters
            Me.NumberOfUpperCaseCharacters = 0
            Me.NumberOfSpecialCharacters = 0
        End Sub
        ' End Constructor 
    End Class
    ' End Class EnglishPasswordOptions 

End Namespace
' End Namespace Captcha 
