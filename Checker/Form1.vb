Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PostToAPI()
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'Dim Form2Ex As New Form2
        'Form2Ex.Show()
        'LatamAirlines_Auth.WebViewHost.ShowDevTools(Form2Ex.Handle)
    End Sub



    Public Class PostData
        Public Property _csrf As String
        Public Property _intstate As String
        Public Property client_id As String
        Public Property connection As String
        Public Property nonce As String
        Public Property password As String
        Public Property redirect_uri As String
        Public Property response_type As String
        Public Property scope As String
        Public Property state As String
        Public Property tenant As String
        Public Property username As String
    End Class

    Public Async Sub PostToAPI()
        Dim client As New HttpClient()

        ' Set headers
        client.DefaultRequestHeaders.Add("Connection", "keep-alive")
        client.DefaultRequestHeaders.Add("sec-ch-ua", """Not_A Brand"";v=""8"", ""Chromium"";v=""120"", ""Microsoft Edge"";v=""120""")
        'client.DefaultRequestHeaders.Add("Content-Type", "application/json")
        client.DefaultRequestHeaders.Add("Auth0-Client", "eyJuYW1lIjoiYXV0aDAuanMtdWxwIiwidmVyc2lvbiI6IjkuMTguMCJ9")
        client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0")
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0")
        client.DefaultRequestHeaders.Add("sec-ch-ua-platform", """Windows""")
        client.DefaultRequestHeaders.Add("Accept", "*/*")
        client.DefaultRequestHeaders.Add("Origin", "https://accounts.latamairlines.com")
        client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin")
        client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors")
        client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty")
        client.DefaultRequestHeaders.Add("Referer", "https://accounts.latamairlines.com/login?state=hKFo2SBYQllwc2RZSGhHRWltLU01bDBhQzRXWXlnbnBycFlyWKFupWxvZ2luo3RpZNkgZC04d3h4UWp5aDJaZl80Nlc0SDZrNnJMaEZGZVBjUFajY2lk2SA4V1M3MFFoNllyTGRSaDM3bmpSOU9tMHFoUFYxTGVBNg&client=8WS70Qh6YrLdRh37njR9Om0qhPV1LeA6&protocol=oauth2&scope=openid%20email%20profile&lang=es&country=uy&originalCountry=uy&nonce=a18dd99f634e96e797d671b057026ad0&response_type=code&redirect_uri=https%3A%2F%2Fwww.latamairlines.com%2Fcallback")
        client.DefaultRequestHeaders.Add("Accept-Language", "es,es-ES;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6")

        ' Create PostData object
        Dim data As New PostData With {
        ._csrf = "STjQfkux-8t880iEZa4pKGZSuNxahpOKasAg",
        ._intstate = "deprecated",
        .client_id = "8WS70Qh6YrLdRh37njR9Om0qhPV1LeA6",
        .connection = "latamxp-prod-db",
        .nonce = "a18dd99f634e96e797d671b057026ad0",
        .password = "c2FsNTQ1NDU=",
        .redirect_uri = "https://www.latamairlines.com/callback",
        .response_type = "code",
        .scope = "openid email profile",
        .state = "hKFo2SBYQllwc2RZSGhHRWltLU01bDBhQzRXWXlnbnBycFlyWKFupWxvZ2luo3RpZNkgZC04d3h4UWp5aDJaZl80Nlc0SDZrNnJMaEZGZVBjUFajY2lk2SA4V1M3MFFoNllyTGRSaDM3bmpSOU9tMHFoUFYxTGVBNg",
        .tenant = "latam-xp-prod",
        .username = "s4lsalsoft@gmail.com|FE-20231217183219601-7377|2cd4e90774"
    }

        ' Convert PostData object to JSON
        Dim json As String = JsonConvert.SerializeObject(data)

        ' Create HttpContent from JSON
        Dim content As New StringContent(json, Encoding.UTF8, "application/json")

        ' Send POST request
        Dim response As HttpResponseMessage = Await client.PostAsync("https://accounts.latamairlines.com/usernamepassword/login", content)

        ' Get the response content
        Dim responseContent As String = Await response.Content.ReadAsStringAsync()

        ' Output the response content to the console (or handle it as needed)
        Console.WriteLine(responseContent)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub
End Class
