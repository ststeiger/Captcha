
Public Class PasswordGenerator


    Public Shared Sub Test()
        ' http://stackoverflow.com/questions/5750203/how-to-write-unicode-characters-to-the-console
        System.Console.OutputEncoding = System.Text.Encoding.UTF8


        Dim man As New System.Web.SessionState.SessionIDManager()


        For i As Integer = 0 To 99
            Dim sessid As String = man.CreateSessionID(Nothing)
            Dim num As Integer = CountNumeric(sessid)
            System.Console.WriteLine(Convert.ToString(num.ToString() + ":  ") & sessid)


            Dim mySessId As String = RandomPassword(New SessionUidOptions())
            System.Console.WriteLine(mySessId)



            ' Dim pw As String = CreatePassword(8)
            ' Dim pw As String = RandomPassword(New PasswordOptions())


            'Dim pw As String = RandomPassword(New GenericPasswordOptions() With { _
            '     .PASSWORD_CHARS_LCASE = "æaàâäbcçdeéêëfgiïîjklmnoœôöpqrstuùûüüvwxyÿz", _
            '     .PASSWORD_CHARS_UCASE = "ÆAÀÂÄBCÇDEÉÊËFGHIÏÎJKLMNOŒÔÖPQRSTUÙÛÜÜVWXYŸZ", _
            '     .PASSWORD_CHARS_NUMERIC = "0123456789", _
            '     .PASSWORD_CHARS_SPECIAL = "*-+=_&!?$€£%{}()[]/\|.:;", _
            '     .NumberOfLowerCaseCharacters = 5, _
            '     .NumberOfUpperCaseCharacters = 1, _
            '     .NumberOfNumericCharacters = 3, _
            '     .NumberOfSpecialCharacters = 1 _
            '})

            'System.Console.WriteLine(pw)


            'Dim pw1 As String = RandomPassword(New CyrillicPasswordOptions())
            'System.Console.WriteLine(pw1)

            'Dim pw2 As String = RandomPassword(New SafePasswordOptions())
            'System.Console.WriteLine(pw2)

        Next i

    End Sub


    Public Shared Function CountNumeric(str As String) As Integer
        Dim ret As Integer = 0

        For i As Integer = 0 To str.Length - 1
            Dim c As Char = str(i)
            If c >= "0"c AndAlso c <= "9"c Then
                ret += 1
            End If
        Next

        Return ret
    End Function


    ' Randomly generate the desired number of lowercase, uppercase, numeric and special chars
    ' Add them to char array 
    ' Shuffle array randomly
    Public Shared Function RandomPassword(options As AbstractPasswordOptions) As String
        Dim maxLenLowerCaseChars As Integer = options.PASSWORD_CHARS_LCASE.Length
        Dim maxLenUpperCaseChars As Integer = options.PASSWORD_CHARS_UCASE.Length
        Dim maxLenNumericChars As Integer = options.PASSWORD_CHARS_NUMERIC.Length
        Dim maxLenSpecialChars As Integer = options.PASSWORD_CHARS_SPECIAL.Length


        Dim k As Integer = 0

        Dim pwChars As Char() = New Char(options.TotalPasswordLength - 1) {}

        Using rnd As Cryptography.CryptoRandom = options.RandomNumberGenerator

            For i As Integer = 0 To options.NumberOfLowerCaseCharacters - 1
                Dim c As Char = options.PASSWORD_CHARS_LCASE(rnd.[Next](0, maxLenLowerCaseChars))
                pwChars(k) = c
                k += 1
            Next


            For i As Integer = 0 To options.NumberOfUpperCaseCharacters - 1
                Dim c As Char = options.PASSWORD_CHARS_UCASE(rnd.[Next](0, maxLenUpperCaseChars))
                pwChars(k) = c
                k += 1
            Next


            For i As Integer = 0 To options.NumberOfNumericCharacters - 1
                Dim c As Char = options.PASSWORD_CHARS_NUMERIC(rnd.[Next](0, maxLenNumericChars))
                pwChars(k) = c
                k += 1
            Next


            For i As Integer = 0 To options.NumberOfSpecialCharacters - 1
                Dim c As Char = options.PASSWORD_CHARS_SPECIAL(rnd.[Next](0, maxLenSpecialChars))
                pwChars(k) = c
                k += 1
            Next

            Shuffle(pwChars, rnd)
        End Using ' rnd 

        Return New String(pwChars)
    End Function ' RandomPassword 


    Public Shared Sub Shuffle(Of T)(list As T())
        ' Random rnd = new Random();
        Using rnd As New Cryptography.CryptoRandom()
            Shuffle(list, rnd)
        End Using ' rnd 

    End Sub ' Shuffle 


    Public Shared Sub Shuffle(Of T)(list As T(), rnd As System.Random)
        Dim n As Integer = list.Length

        While n > 1
            Dim k As Integer = (rnd.[Next](0, n) Mod n)
            n -= 1
            Dim value As T = list(k)
            list(k) = list(n)
            list(n) = value
        End While

    End Sub ' Shuffle 


    Public Shared Sub Shuffle(Of T)(list As System.Collections.Generic.IList(Of T))
        ' Random rnd = new Random();
        Using rnd As New Cryptography.CryptoRandom()
            Shuffle(Of T)(list, rnd)
        End Using ' rnd

    End Sub ' Shuffle 


    Public Shared Sub Shuffle(Of T)(list As System.Collections.Generic.IList(Of T), rnd As System.Random)
        Dim n As Integer = list.Count

        While n > 1
            Dim k As Integer = (rnd.[Next](0, n) Mod n)
            n -= 1
            Dim value As T = list(k)
            list(k) = list(n)
            list(n) = value
        End While

    End Sub ' Shuffle 


    Public Shared Function CreatePassword(length As Integer) As String
        Dim res As New System.Text.StringBuilder()
        Const valid As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

        ' System.Random rnd = new System.Random();
        Using rnd As New Cryptography.CryptoRandom()
            ' cr.Next(10, 100);

            While 0 < System.Math.Max(System.Threading.Interlocked.Decrement(length), length + 1)
                res.Append(valid(rnd.[Next](valid.Length)))
            End While

        End Using ' rnd 

        Return res.ToString()
    End Function ' CreatePassword 


End Class ' PasswordGenerator
