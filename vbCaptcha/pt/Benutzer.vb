Namespace [Shared]
    Namespace Current
        Namespace Basic
            Public Class Benutzer
                Inherits Base.Benutzer

                ''' <summary>
                ''' Erstellt einen neuen Benutzer
                ''' </summary>
                ''' <remarks></remarks>
                Public Sub New()

                End Sub

                ''' <summary>
                ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_Hash].
                ''' </summary>
                ''' <param name="pHash">[BE_Hash]</param>
                ''' <remarks></remarks>
                Public Sub New(pHash As String)
                    MyBase.New(pHash)
                End Sub

                ''' <summary>
                ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_ID].
                ''' </summary>
                ''' <param name="pID">[BE_ID]</param>
                ''' <remarks></remarks>
                Public Sub New(pID As Integer)
                    MyBase.New(pID)
                End Sub

                ''' <summary>
                ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_User] und [BE_Passwort]
                ''' </summary>
                ''' <param name="pUser">[BE_User]</param>
                ''' <param name="pPassword">[BE_Passwort]</param>
                ''' <remarks></remarks>
                Public Sub New(pUser As String, pPassword As String)
                    MyBase.New(pUser, pPassword)
                End Sub

                Public Shared Shadows Function [Get](pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "proc") As String
                    Return Base.Benutzer.[Get](pCollection, pKey)
                End Function
            End Class
        End Namespace

        Namespace Portal
            Public Class Benutzer
                Inherits Base.Benutzer

                <Base.Serialise(True)>
                Public logoutLabel As String

                <Base.Serialise(True)>
                Public SSRS As String

                ''' <summary>
                ''' Erstellt einen neuen Benutzer
                ''' </summary>
                ''' <remarks></remarks>
                Public Sub New()

                End Sub

                ''' <summary>
                ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_Hash].
                ''' </summary>
                ''' <param name="pHash">[BE_Hash]</param>
                ''' <remarks></remarks>
                Public Sub New(pHash As String)
                    MyBase.New(pHash)
                End Sub

                ''' <summary>
                ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_User] und [BE_Passwort]
                ''' </summary>
                ''' <param name="pUser">[BE_User]</param>
                ''' <param name="pPassword">[BE_Passwort]</param>
                ''' <remarks></remarks>
                Public Sub New(pUser As String, pPassword As String)
                    MyBase.New(pUser, pPassword)
                End Sub

                Public Shared Shadows Function [Get](pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "BE_ID") As String
                    Return Base.Benutzer.[Get](pCollection, pKey)
                End Function
            End Class
        End Namespace
    End Namespace

    Namespace Base
        Public Class Benutzer
            Inherits T_Benutzer

            Private Shared _Key As String = "" 'Hokedipokedi
            Private Shared _Context As String = "Abrakadabra"
            Private Shared _Cookie As String = "" 'Simsalabim

            Public Sub New()

            End Sub

            ''' <summary>
            ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_Hash].
            ''' </summary>
            ''' <param name="pHash">[BE_Hash]</param>
            ''' <remarks></remarks>
            Public Sub New(pHash As String)
                Using tCommand As System.Data.IDbCommand = SQL.CreateCommand("select top 1 * from [T_Benutzer] where [BE_Hash] = @BE_Hash")
                    SQL.AddParameter(tCommand, "@BE_Hash", pHash)

                    Using tTable As System.Data.DataTable = SQL.GetDataTable(tCommand)
                        If Not tTable Is Nothing AndAlso tTable.Rows.Count = 1 Then
                            Me.byRow(tTable.Rows(0))
                        End If
                    End Using
                End Using
            End Sub

            ''' <summary>
            ''' Erstellt einen neuen Benutzer mit den Daten gemäss [BE_ID].
            ''' </summary>
            ''' <param name="pID">[BE_ID]</param>
            ''' <remarks></remarks>
            Public Sub New(pID As Integer)
                Using tCommand As System.Data.IDbCommand = SQL.CreateCommand("select top 1 * from [T_Benutzer] where [BE_ID] = @BE_ID")
                    SQL.AddParameter(tCommand, "@BE_ID", pID)

                    Using tTable As System.Data.DataTable = SQL.GetDataTable(tCommand)
                        If Not tTable Is Nothing AndAlso tTable.Rows.Count = 1 Then
                            Me.byRow(tTable.Rows(0))
                        End If
                    End Using
                End Using
            End Sub

            ''' <summary>
            '''  Erstellt einen neuen Benutzer mit den Daten gemäss [BE_User] und [BE_Passwort]
            ''' </summary>
            ''' <param name="pUser">[BE_User]</param>
            ''' <param name="pPassword">[BE_Passwort]</param>
            ''' <remarks></remarks>
            Public Sub New(pUser As String, pPassword As String)
                Using tCommand As System.Data.IDbCommand = SQL.CreateCommand("select top 1 * from [T_Benutzer] where [BE_User] = @BE_User and [BE_Passwort] = @BE_Passwort")
                    SQL.AddParameter(tCommand, "@BE_User", pUser)
                    SQL.AddParameter(tCommand, "@BE_Passwort", Crypt.Crypt(pPassword))

                    Using tTable As System.Data.DataTable = SQL.GetDataTable(tCommand)
                        If Not tTable Is Nothing AndAlso tTable.Rows.Count = 1 Then
                            Me.byRow(tTable.Rows(0))
                        End If
                    End Using
                End Using
            End Sub

            ''' <summary>
            ''' Erstellt einen neuen Benutzer mit den Daten der Datenzeile
            ''' </summary>
            ''' <param name="pRow"></param>
            ''' <remarks></remarks>
            Public Sub New(pRow As System.Data.DataRow)
                Me.byRow(pRow)
            End Sub

            ''' <summary>
            ''' Liest die Einstellungen aus der Datenbank (Schlüssel und Name)
            ''' </summary>
            ''' <remarks></remarks>
            Private Shared Sub _getSettings()
                Try
                    Using tCommand As System.Data.IDbCommand = SQL.CreateCommand("select top 1 isnull([MDT_JWT_Name], 'Simsalabim') as [MDT_JWT_Name], isnull([MDT_JWT_Key], 'Hokedipokedi') as [MDT_JWT_Key] from [T_AP_Ref_Mandant]")
                        Using tTable As Data.DataTable = SQL.GetDataTable(tCommand)
                            If Not tTable Is Nothing AndAlso tTable.Rows.Count = 1 Then
                                Benutzer._Key = tTable.Rows(0).Item("MDT_JWT_Key").ToString
                                Benutzer._Cookie = tTable.Rows(0).Item("MDT_JWT_Name").ToString
                            End If
                        End Using
                    End Using
                Catch
                    Benutzer._Key = "Hokedipokedi"
                    Benutzer._Cookie = "Simsalabim"
                End Try
            End Sub

            ''' <summary>
            ''' Gibt den Namen des Kekes zurück
            ''' </summary>
            ''' <returns>Gibt den Namen des Kekes zurück</returns>
            ''' <remarks></remarks>
            Private Shared Function _getCookie() As String
                If String.IsNullOrEmpty(Benutzer._Cookie) Then
                    Benutzer._getSettings()
                End If

                Return Benutzer._Cookie
            End Function

            ''' <summary>
            ''' Gibt den Schlüssel des Kekes zurück
            ''' </summary>
            ''' <returns>Gibt den Schlüssel des Kekes zurück</returns>
            ''' <remarks></remarks>
            Private Shared Function _getKey() As String
                If String.IsNullOrEmpty(Benutzer._Key) Then
                    Benutzer._getSettings()
                End If

                Return Benutzer._Key
            End Function

            ''' <summary>
            ''' Erstellt eine neue Instanz aus einem codierten Keks
            ''' </summary>
            ''' <param name="pCookie"></param>
            ''' <returns>T_Benutzer</returns>
            ''' <remarks></remarks>
            Public Shared Function byCookie(pCookie As String) As Benutzer
                Return byCookie(Of Benutzer)(pCookie)
            End Function

            Public Shared Function byCookie(Of T)(pCookie As String) As T
                Dim tCookie As String = JWT.JsonWebToken.Decode(pCookie, Benutzer._getKey())
                Return JWT.PetaJson.Parse(Of T)(tCookie, JWT.PetaJson.JsonOptions.None)
            End Function


            ''' <summary>
            ''' Erstellt eine neue Instanz aus einem codierten Keks
            ''' </summary>
            ''' <param name="pRequest"></param>
            ''' <returns>T_Benutzer</returns>
            ''' <remarks></remarks>
            Public Shared Function byCookie(pRequest As System.Web.HttpRequest) As Benutzer
                Return byCookie(Of Benutzer)(pRequest)
            End Function

            Public Shared Function byCookie(Of T)(pRequest As System.Web.HttpRequest) As T
                If Not pRequest.Cookies(Benutzer._Context) Is Nothing Then
                    Dim tCookie As String = pRequest.Cookies(Benutzer._Context)(Benutzer._getCookie())
                    Return byCookie(Of T)(tCookie)
                End If

                Return Activator.CreateInstance(Of T)()
                Return Nothing
            End Function

            ''' <summary>
            ''' Weist die Werte der Datenzeile an die Klassen-Felder zu
            ''' </summary>
            ''' <param name="pRow"></param>
            ''' <remarks></remarks>
            Protected Sub byRow(pRow As System.Data.DataRow)
                If Not pRow Is Nothing Then
                    With pRow
                        If .Table.Columns.Contains("BE_ID") Then Me.BE_ID = If(IsDBNull(.Item("BE_ID")), 0, CInt(.Item("BE_ID")))
                        If .Table.Columns.Contains("BE_Status") Then Me.BE_Status = If(IsDBNull(.Item("BE_Status")), 0, CInt(.Item("BE_Status")))
                        If .Table.Columns.Contains("BE_Augen") Then Me.BE_Augen = If(IsDBNull(.Item("BE_Augen")), 0, CInt(.Item("BE_Augen")))

                        If .Table.Columns.Contains("BE_Name") Then Me.BE_Name = .Item("BE_Name").ToString()
                        If .Table.Columns.Contains("BE_Vorname") Then Me.BE_Vorname = .Item("BE_Vorname").ToString()
                        If .Table.Columns.Contains("BE_User") Then Me.BE_User = .Item("BE_User").ToString()
                        If .Table.Columns.Contains("BE_Passwort") Then Me.BE_Passwort = .Item("BE_Passwort").ToString()
                        If .Table.Columns.Contains("BE_Language") Then Me.BE_Language = .Item("BE_Language").ToString()
                        If .Table.Columns.Contains("BE_Hash") Then Me.BE_Hash = .Item("BE_Hash").ToString()
                        If .Table.Columns.Contains("BE_Email") Then Me.BE_Email = .Item("BE_Email").ToString()
                        If .Table.Columns.Contains("BE_IsCOR_Hash") Then Me.BE_IsCOR_Hash = .Item("BE_IsCOR_Hash").ToString()
                        If .Table.Columns.Contains("_BE_Label") Then Me._BE_Label = .Item("_BE_Label").ToString()
                    End With
                End If
            End Sub

            ''' <summary>
            ''' Gibt den Namen des Kekes zurück
            ''' </summary>
            ''' <returns>Gibt den Namen des Kekes zurück</returns>
            ''' <remarks></remarks>
            Public Shared Function getCookie() As String
                Return Benutzer._getCookie
            End Function


            ''' <summary>
            ''' Erstellt einen codierten Keks
            ''' </summary>
            ''' <returns>Basis64 codierter Keks</returns>
            ''' <remarks></remarks>
            Public Function toCookie() As String
                Dim extraHeaders As New Dictionary(Of String, Object)
                extraHeaders("JTI") = System.Guid.NewGuid().ToString()



                '-- DECLARE @jti varchar(255) 
                '-- SET @jti = CAST(NEWID() AS varchar(36)) 

                Dim strSQL As String = "INSERT INTO T_SYS_JwtToken(JWT_UID, JWT_JTI, JWT_IAT, JWT_EXP, JWT_NBF) " + System.Environment.NewLine
                strSQL += "SELECT " + System.Environment.NewLine
                strSQL += "	 NEWID() AS JWT_UID -- uniqueidentifier" + System.Environment.NewLine
                strSQL += "	,@jti AS JWT_JTI -- varchar(255)" + System.Environment.NewLine
                strSQL += "	,CURRENT_TIMESTAMP AS JWT_IAT -- datetime" + System.Environment.NewLine
                strSQL += "	,CURRENT_TIMESTAMP + 1 AS JWT_EXP -- datetime" + System.Environment.NewLine
                strSQL += "	,CAST(NULL AS datetime) AS JWT_NBF-- datetime" + System.Environment.NewLine
                strSQL += ";" + System.Environment.NewLine

                Using cmd As System.Data.IDbCommand = SQL.CreateCommand(strSQL)
                    SQL.AddParameter(cmd, "jti", "value")

                    SQL.ExecuteNonQuery(cmd)
                End Using

                Return JWT.JsonWebToken.Encode(extraHeaders, Me, Benutzer._getKey(), JWT.JwtHashAlgorithm.HS256)
            End Function

            ''' <summary>
            ''' Schreibt einen codierten Keks in die Antwort
            ''' </summary>
            ''' <param name="pResponse"></param>
            ''' <returns>Basis64 codierter Keks</returns>
            ''' <remarks></remarks>
            Public Function toCookie(pResponse As System.Web.HttpResponse) As String
                Dim tCookieString As String = Me.toCookie

                Dim tCookie As System.Web.HttpCookie = pResponse.Cookies(Benutzer._Context)
                tCookie(Benutzer._getCookie()) = tCookieString
                tCookie.Expires = Now.AddDays(7)
                pResponse.Cookies.Add(tCookie)

                Return tCookieString
            End Function

            ''' <summary>
            ''' Vermutet, ob der Benutzer auf der Datenbank gefunden wurde
            ''' </summary>
            ''' <returns></returns>
            ''' <remarks>Keine effektive SQL-Abfrage</remarks>
            Public Function Exists() As Boolean
                Return Me.BE_ID > 0
            End Function

            Private Shared Function _getValueFromKey(pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "") As String
                Dim tValue As String = String.Empty
                Dim tKeys As New System.Collections.Generic.List(Of String)

                If Not String.IsNullOrEmpty(pKey) Then
                    tKeys.Add(pKey)
                Else
                    tKeys.AddRange({"proc", "BE_ID", "BE"})
                End If

                If Not pCollection Is Nothing AndAlso pCollection.Count > 0 Then
                    For Each tKey As String In pCollection.Keys
                        If tKeys.Contains(tKey) Then
                            tValue = pCollection.Item(tKey)
                            Exit For
                        End If
                    Next
                End If

                Return tValue
            End Function

            Protected Shared Function Cookie(pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "") As String
                Return Benutzer._getValueFromKey(pCollection, pKey)
            End Function

            Protected Shared Function Form(pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "") As String
                Return Benutzer._getValueFromKey(pCollection, pKey)
            End Function

            Protected Shared Function QueryString(pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "") As String
                Return Benutzer._getValueFromKey(pCollection, pKey)
            End Function

            Protected Shared Function Session(pCollection As System.Web.SessionState.HttpSessionState, Optional pKey As String = "") As String
                Dim tCollection As New System.Collections.Specialized.NameValueCollection()

                If Not pCollection Is Nothing AndAlso pCollection.Count > 0 Then
                    For Each tKey As String In pCollection.Keys
                        tCollection.Add(tKey, pCollection.Item(tKey))
                    Next
                End If

                Return Benutzer.Session(tCollection)
            End Function

            Protected Shared Function Session(pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "") As String
                Return Benutzer._getValueFromKey(pCollection, pKey)
            End Function

            Public Shared Function [Get](pCollection As System.Collections.Specialized.NameValueCollection, Optional pKey As String = "") As String
                Dim tV As String = Benutzer.Cookie(pCollection, pKey)
                If Not String.IsNullOrEmpty(tV) Then Return tV

                tV = Benutzer.Form(pCollection, pKey)
                If Not String.IsNullOrEmpty(tV) Then Return tV

                tV = Benutzer.QueryString(pCollection, pKey)
                If Not String.IsNullOrEmpty(tV) Then Return tV

                tV = Benutzer.Session(pCollection, pKey)
                If Not String.IsNullOrEmpty(tV) Then Return tV

                Return String.Empty
            End Function
        End Class

        Public Class T_Benutzer
            <Serialise(True)>
            Public BE_ID As Integer = 0

            <Serialise(True)>
            Public BE_Name As String

            <Serialise(True)>
            Public BE_Vorname As String

            <Serialise(True)>
            Public BE_User As String

            <Serialise(False)>
            Public BE_Passwort As String

            <Serialise(True)>
            Public BE_Language As String

            <Serialise(True)>
            Public BE_Hash As String

            <Serialise(False)>
            Public BE_Level As Integer 'Wird ned brucht

            <Serialise(False)>
            Public BE_isLDAPSync As Boolean 'Wird ned brucht

            <Serialise(False)>
            Public BE_Domaene As String 'Wird ned brucht

            <Serialise(False)>
            Public BE_Hide As Boolean 'Wird ned brucht

            <Serialise(False)>
            Public BE_Email As String

            <Serialise(False)>
            Public BE_TelNr As String 'Keni ned

            <Serialise(False)>
            Public BE_CurrencyID As Integer 'Wird ned brucht

            <Serialise(False)>
            Public BE_Status As Integer

            <Serialise(False)>
            Public BE_IsCOR_Hash As String

            <Serialise(False)>
            Public BE_IsGuest As Boolean 'Keni ned

            <Serialise(False)>
            Public BE_Augen As Integer

            <Serialise(True)>
            Public _BE_Label As String
        End Class

        ''todo: Gäbs scho
        ''Namespace JWT.PetaJson
        ''<AttributeUsage(AttributeTargets.[Property] Or AttributeTargets.Field)>
        ''Public Class JsonExcludeAttribute
        <AttributeUsage(AttributeTargets.Field)> _
        Public Class Serialise
            Inherits Attribute

#Region "Members"
            Private _Serialise As Boolean = True
#End Region

#Region "Property"
            Public Property Serialise As Boolean
                Get
                    Return Me._Serialise
                End Get
                Set(value As Boolean)
                    Me._Serialise = value
                End Set
            End Property
#End Region

#Region "Constructor"
            Public Sub New(pSerialise As Boolean)
                Me._Serialise = pSerialise
            End Sub
#End Region
        End Class
    End Namespace
End Namespace