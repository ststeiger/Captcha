
Namespace [Shared].Modules


    Public Class CookieAuthModule
        Implements System.Web.IHttpModule

        Public Shared IsTesting As Boolean
        Public Shared ThisModuleMustRun As Boolean
        Public Shared AllowedUrls As Hashtable

        Protected Shared ReadOnly m_bannedUserAgentsRegex As System.Text.RegularExpressions.Regex
        Protected Shared m_strRedirectURL As String = Nothing



        Shared Sub New()
            ThisModuleMustRun = False
            IsTesting = False

            Dim regex As String = "(libwww-perl|libcurl|sqlmap|msnbot|Java/|Purebot|Lipperhey|MaMa CaSpEr|Mail.Ru|gold crawler|MSIE 4.|MSIE 5.|MSIE 6.|MSIE 7.|MSIE 3.)"

            If Not String.IsNullOrEmpty(regex) Then
                m_bannedUserAgentsRegex = New System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase Or System.Text.RegularExpressions.RegexOptions.Compiled)
            End If


            If System.StringComparer.OrdinalIgnoreCase.Equals(System.Environment.UserDomainName, "CORR") Then
                IsTesting = True
            End If

            ' System.Web.Configuration.HttpCookiesSection.


            ThisModuleMustRun = SQL.ExecuteScalar(Of Boolean)("SELECT TOP 1 FC_Value FROM T_FMS_Configuration WHERE FC_KEY = 'UseJwtAuthModule' ")

            AllowedUrls = New Hashtable(System.StringComparer.OrdinalIgnoreCase)
            AllowedUrls.Add("ajax/Bocketiback.ashx", Nothing)
            AllowedUrls.Add("ajax/Bibedibabedibu.ashx", Nothing)


            AllowedUrls.Add("", Nothing)
            AllowedUrls.Add("default.aspx", Nothing)
            AllowedUrls.Add("w8/index.html", Nothing)
            AllowedUrls.Add("css/w8/Layout.ashx", Nothing)
            AllowedUrls.Add("js/w8/Script.ashx", Nothing)
            AllowedUrls.Add("w8/Loading.html", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/getConnection", Nothing)
            ' AllowedUrls.Add("images/w8/l_ss.png", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/getBasicLink", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/getFooter", Nothing)
            ' AllowedUrls.Add("images/Confirm/ie.png", Nothing)
            ' AllowedUrls.Add("images/Confirm/header_bg.jpg", Nothing)
            ' AllowedUrls.Add("images/Confirm/body_bg.jpg", Nothing)
            ' AllowedUrls.Add("images/Confirm/buttons.png", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/getVersion", Nothing)
            AllowedUrls.Add("ajax/login.ashx", Nothing)
            AllowedUrls.Add("ajax/saml.ashx", Nothing)

            AllowedUrls.Add("ajax/Portal.aspx/getTranslation", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/getNavigation", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/callError", Nothing)
            AllowedUrls.Add("ajax/Portal.aspx/useNavigation", Nothing)
            ' AllowedUrls.Add("images/w8/_rm.png", Nothing)

            AllowedUrls.Add("nouser.aspx", Nothing)
            AllowedUrls.Add("noaccess.aspx", Nothing)
            AllowedUrls.Add("noavailability.aspx", Nothing)
            AllowedUrls.Add("nodata.aspx", Nothing)
            AllowedUrls.Add("nomodulaccess.aspx", Nothing)

            If Not System.StringComparer.OrdinalIgnoreCase.Equals(System.Environment.UserDomainName, "COR") Then
                AllowedUrls.Add("COR/devtool.aspx", Nothing)
            End If

        End Sub


        Private Sub System_Web_IHttpModule_Dispose() Implements System.Web.IHttpModule.Dispose
            ' Throw New System.NotImplementedException()
            ' Nothing to dispose
        End Sub


        Private Sub System_Web_IHttpModule_Init(context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init

            If ThisModuleMustRun Then
                AddHandler context.BeginRequest, New System.EventHandler(AddressOf OnBeginRequest)
                AddHandler context.EndRequest, New System.EventHandler(AddressOf OnEndRequest)
                AddHandler context.PreRequestHandlerExecute, New System.EventHandler(AddressOf RedirectMatchedUserAgents)
            End If

        End Sub


        Public Shared Function IsAuthenticated(context As System.Web.HttpContext) As Boolean
            Try
                ' Dim ip As String = context.Request.UserHostAddress

                Dim urlRequiresAuthentication As Boolean = UrlNeedsAuthentication(context)
                If Not urlRequiresAuthentication Then
                    Return True
                End If

                If [Shared].Current.Basic.Benutzer.byCookie(context.Request).Exists() Then
                    Return True
                End If


                'Dim strShibSessionID As String = context.Request.Params("ShibSessionID")
                'If String.IsNullOrEmpty(strShibSessionID) Then
                '    Return True
                'End If

            Catch ex As Exception
                System.Console.WriteLine(ex.Message)
            End Try

            ' [Shared].Current.Basic.Benutzer.byCookie(Me.Request).BE_Language()
            Return False
        End Function


        Private Shared Function HasUrl(context As System.Web.HttpContext)
            If context Is Nothing OrElse context.Request Is Nothing OrElse context.Request.Url Is Nothing OrElse context.Request.Url.OriginalString Is Nothing Then
                Return False
            End If

            Return True
        End Function


        Public Shared Function UrlNeedsAuthentication(context As System.Web.HttpContext) As Boolean
            Try
                If Not HasUrl(context) Then
                    Return True
                End If


                Dim strAbsPath As String = context.Request.Url.AbsolutePath
                If String.IsNullOrEmpty(strAbsPath) Then
                    Return True
                End If

                If Not String.IsNullOrEmpty(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath) Then
                    strAbsPath = strAbsPath.Substring(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath.Length)
                End If

                If strAbsPath.StartsWith("/") Then
                    strAbsPath = strAbsPath.Substring(1)
                End If

                If strAbsPath.StartsWith("images") Then
                    Return False
                End If

                If AllowedUrls.ContainsKey(strAbsPath) Then
                    Return False
                End If

                Return True
            Catch ex As Exception

            End Try


            Return True
        End Function



        Public Class AuthError
            Public isAuthError As Boolean
            Public message As String


            Sub New()
                Me.New("No User")
            End Sub


            Sub New(strMessage As String)
                isAuthError = True
                message = strMessage
            End Sub


        End Class


        Public Sub DeleteCookie(cookieName As String)

            If (Not System.Web.HttpContext.Current.Request.Cookies(cookieName) Is Nothing) Then
                Dim thisCookie As System.Web.HttpCookie
                thisCookie = New System.Web.HttpCookie(cookieName)
                thisCookie.Expires = DateTime.Now.AddDays(-365)
                thisCookie.HttpOnly = False
                thisCookie.Secure = False
                System.Web.HttpContext.Current.Response.Cookies.Add(thisCookie)
            End If

        End Sub



        Private Shared Sub RedirectMatchedUserAgents(sender As Object, e As System.EventArgs)
            Dim app As System.Web.HttpApplication = TryCast(sender, System.Web.HttpApplication)

            If m_bannedUserAgentsRegex IsNot Nothing AndAlso app IsNot Nothing AndAlso app.Request IsNot Nothing AndAlso Not String.IsNullOrEmpty(app.Request.UserAgent) Then

                If m_bannedUserAgentsRegex.Match(app.Request.UserAgent).Success Then

                    'var cbAppContextBase = new HttpContextWrapper(app.Context);
                    'string strRedirectURL = System.Web.Mvc.UrlHelper.GenerateContentUrl("~/Ban/UserAgentBanned", cbAppContextBase);

                    ' m_strRedirectURL = uh.Action("UserAgentBanned", "Ban");

                    If m_strRedirectURL Is Nothing Then
                    End If '(m_strRedirectURL == null)

                    If Not System.StringComparer.OrdinalIgnoreCase.Equals(m_strRedirectURL, app.Request.Url.LocalPath) Then
                        app.Response.Redirect(m_strRedirectURL)
                    End If  '  (!StringComparer.OrdinalIgnoreCase.Equals(m_strRedirectURL, app.Request.Url.LocalPath))
                End If '  (m_bannedUserAgentsRegex.Match(app.Request.UserAgent).Success)
            End If '  (_bannedUserAgentsRegex != null && app != null && app.Request != null && !String.IsNullOrEmpty(app.Request.UserAgent))
        End Sub ' RedirectMatchedUserAgents



        Private Sub OnBeginRequest(sender As Object, e As System.EventArgs)



            If Not IsTesting AndAlso Not IsAuthenticated(System.Web.HttpContext.Current) Then
                System.Web.HttpContext.Current.Response.ClearHeaders()
                System.Web.HttpContext.Current.Response.ClearContent()

                Try
                    System.Web.HttpContext.Current.Response.AppendHeader("X-Cookie-Auth", "The system does not have a user token/cookie.")
                Catch
                End Try


                ' Dim AuthKeks As String = [Shared].Base.Benutzer.getCookie()
                ' Doesn't necessarely work, if path is set on cookie
                DeleteCookie("Abrakadabra")
                DeleteCookie("proc")
                DeleteCookie("proc2")


                'If HasUrl(System.Web.HttpContext.Current) Then

                '    If System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant().EndsWith(".ashx") Then
                '        Dim noUser As AuthError = New AuthError
                '        Dim strJsonResponse As String = JWT.PetaJson.Format(noUser, JWT.PetaJson.JsonOptions.DontWriteWhitespace)

                '        System.Web.HttpContext.Current.Request.ContentType = "application/json"
                '        System.Web.HttpContext.Current.Response.Write(strJsonResponse)
                '        System.Web.HttpContext.Current.Response.Flush()
                '        System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest()
                '        Return
                '    End If
                'End If


                If HasUrl(System.Web.HttpContext.Current) Then
                    If Not System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToLowerInvariant().EndsWith(".ashx") Then
                        Dim strRedirectTarget As String = System.Web.Hosting.HostingEnvironment.MapPath("~/nouser.aspx")

                        If System.IO.File.Exists(strRedirectTarget) Then
                            System.Web.HttpContext.Current.Server.Transfer("~/nouser.aspx?log=nobody")
                            System.Web.HttpContext.Current.Response.Flush()
                            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest()
                            Return
                        End If

                    End If
                End If


                ' System.Web.HttpContext.Current.Response.SuppressContent = True
                ' System.Web.HttpContext.Current.Response.StatusCode = System.Net.HttpStatusCode.Forbidden
                System.Web.HttpContext.Current.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized
                System.Web.HttpContext.Current.Response.StatusDescription = "AuthModule: Authentication failed."

                ' http://weblogs.asp.net/hajan/why-not-to-use-httpresponse-close-and-httpresponse-end
                ' System.Web.HttpContext.Current.Response.OutputStream.Close()
                System.Web.HttpContext.Current.Response.Flush()
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

        End Sub ' OnBeginRequest


        Private Sub OnEndRequest(sender As Object, e As System.EventArgs)

            If System.Web.HttpContext.Current IsNot Nothing AndAlso System.Web.HttpContext.Current.Response IsNot Nothing Then
                Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
                ' response.AddHeader("Content-Language", "*");
                ' response.AppendHeader("P3P", "CP=\\\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\\\"");
                Try

                    Try
                        ' Add bogus P3P policy, so cookie will be retained
                        response.AddHeader("P3P", "CP=\""IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\""")
                        ' response.AppendHeader("IE-SUCKS", "AND SSRS TOO")
                        ' response.AppendHeader("X-Frame-Options", "DENY")
                        ' response.AppendHeader("X-Frame-Options", "AllowAll")
                        response.AppendHeader("X-Frame-Options", "SAMEORIGIN")

                        ' https//gist.github.com/cemerson/3906944
                        response.AppendHeader("cache-control", "no-cache, no-store, must-revalidate, private, max-age=0")
                        ' response.AppendHeader("expires", "Tue, 01 Jan 1980 1:00:00 GMT, 0")
                        response.AppendHeader("expires", "0")
                        response.AppendHeader("pragma", "no-cache")

                        ' response.AppendHeader("Options", "SAMEORIGIN");
                    Catch ex As Exception

                    End Try


                    ' http://stackoverflow.com/questions/1442863/how-can-i-set-the-secure-flag-on-an-asp-net-session-cookie
                    For Each thisCookie As System.Web.HttpCookie In response.Cookies
                        ' TODO: FIX PORTAL !!!
                        ' thisCookie.HttpOnly = True

                        Dim ipString As String = System.Web.HttpContext.Current.Request.UserHostAddress

                        If Not IPv4Info.IsPrivateIp(ipString) Then
                            thisCookie.Secure = True
                        End If

                    Next thisCookie

                Catch ex As Exception
                    System.Console.WriteLine(ex.Message)
                End Try

            End If ' (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Response != null)
        End Sub ' context_EndRequest


    End Class ' CookieAuthModule : System.Web.IHttpModule


End Namespace ' [Shared]
