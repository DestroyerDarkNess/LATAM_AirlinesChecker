Imports EO.Base
Imports EO.WebBrowser

Namespace API
    Public Class Auth : Implements IDisposable

        Private Property Execution_Watcher As Stopwatch = Nothing

        Public Property Engine As EO.WebEngine.Engine = Nothing

        Public Property UseRandomUserAgent As Boolean = False

        Public Property Secure As Boolean = True

        Public WithEvents WebViewHost As EO.WebBrowser.WebView = Nothing

        Public Property WebView As EO.WinForm.WebControl = Nothing

        Private Property InstanceText As String = String.Empty


        Private ChangeUserAgentJS As String = <![CDATA[

var ieUserAgents = [
				"Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0",
				"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
				"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)",
				"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/5.0)",
				"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/4.0; InfoPath.2; SV1; .NET CLR 2.0.50727; WOW64)",
				"Mozilla/5.0 (compatible; MSIE 10.0; Macintosh; Intel Mac OS X 10_7_3; Trident/6.0)",
				"Mozilla/4.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/5.0)",
				"Mozilla/1.22 (compatible; MSIE 10.0; Windows 3.1)",
				"Mozilla/5.0 (Windows; U; MSIE 9.0; WIndows NT 9.0; en-US))",
				"Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; Media Center PC 6.0; InfoPath.3; MS-RTC LM 8; Zune 4.7)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; Media Center PC 6.0; InfoPath.3; MS-RTC LM 8; Zune 4.7",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Zune 4.0; InfoPath.3; MS-RTC LM 8; .NET4.0C; .NET4.0E)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; chromeframe/12.0.742.112)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 2.0.50727; Media Center PC 6.0)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 2.0.50727; Media Center PC 6.0)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Zune 4.0; Tablet PC 2.0; InfoPath.3; .NET4.0C; .NET4.0E)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; yie8)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET CLR 1.1.4322; .NET4.0C; Tablet PC 2.0)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; FunWebProducts)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; chromeframe/13.0.782.215)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; chromeframe/11.0.696.57)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0) chromeframe/10.0.648.205",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/4.0; GTB7.4; InfoPath.1; SV1; .NET CLR 2.8.52393; WOW64; en-US)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.0; Trident/5.0; chromeframe/11.0.696.57)",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.0; Trident/4.0; GTB7.4; InfoPath.3; SV1; .NET CLR 3.1.76908; WOW64; en-US)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; GTB7.4; InfoPath.2; SV1; .NET CLR 3.3.69573; WOW64; en-US)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; InfoPath.1; SV1; .NET CLR 3.8.36217; WOW64; en-US)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; .NET CLR 2.7.58687; SLCC2; Media Center PC 5.0; Zune 3.4; Tablet PC 3.6; InfoPath.3)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.2; Trident/4.0; Media Center PC 4.0; SLCC1; .NET CLR 3.0.04320)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 1.1.4322)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.1; SLCC1; .NET CLR 1.1.4322)",
				"Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 5.0; Trident/4.0; InfoPath.1; SV1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 3.0.04506.30)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 5.0; Trident/4.0; FBSMTWB; .NET CLR 2.0.34861; .NET CLR 3.0.3746.3218; .NET CLR 3.5.33652; msn OptimizedIE8;ENUS)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.2; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; Media Center PC 6.0; InfoPath.2; MS-RTC LM 8)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; Media Center PC 6.0; InfoPath.2; MS-RTC LM 8",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 3.5.30729; .NET CLR 3.0.30729; MS-RTC LM 8)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; InfoPath.2)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Zune 3.0)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; msn OptimizedIE8;ZHCN)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MS-RTC LM 8; InfoPath.3; .NET4.0C; .NET4.0E) chromeframe/8.0.552.224",
				"Mozilla/4.0(compatible; MSIE 7.0b; Windows NT 6.0)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 6.0)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.0.04506.30)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; Media Center PC 3.0; .NET CLR 1.0.3705; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.1)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; FDM; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322; InfoPath.1)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322; Alexa Toolbar; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322; Alexa Toolbar)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.40607)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; .NET CLR 1.0.3705; Media Center PC 3.1; Alexa Toolbar; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
				"Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 6.0; en-US)",
				"Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 6.0; el-GR)",
				"Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 5.2)",
				"Mozilla/5.0 (MSIE 7.0; Macintosh; U; SunOS; X11; gu; SV1; InfoPath.2; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; c .NET CLR 3.0.04506; .NET CLR 3.5.30707; InfoPath.1; el-GR)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; c .NET CLR 3.0.04506; .NET CLR 3.5.30707; InfoPath.1; el-GR)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 6.0; fr-FR)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 6.0; en-US)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 5.2; WOW64; .NET CLR 2.0.50727)",
				"Mozilla/5.0 (compatible; MSIE 7.0; Windows 98; SpamBlockerUtility 6.3.91; SpamBlockerUtility 6.2.91; .NET CLR 4.1.89;GB)",
				"Mozilla/4.79 [en] (compatible; MSIE 7.0; Windows NT 5.0; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 1.1.4322; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648)",
				"Mozilla/4.0 (Windows; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (Mozilla/4.0; MSIE 7.0; Windows NT 5.1; FDM; SV1; .NET CLR 3.0.04506.30)",
				"Mozilla/4.0 (Mozilla/4.0; MSIE 7.0; Windows NT 5.1; FDM; SV1)",
				"Mozilla/4.0 (compatible;MSIE 7.0;Windows NT 6.0)",
				"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; Win64; x64; Trident/6.0; .NET4.0E; .NET4.0C)",
				"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; SLCC2; .NET CLR 2.0.50727; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 3.5.30729; .NET CLR 3.0.30729; MS-RTC LM 8)",
				"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MS-RTC LM 8; .NET4.0C; .NET4.0E; InfoPath.3)",
				"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)",
				"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; chromeframe/12.0.742.100)",
				"Mozilla/4.0 (compatible; MSIE 6.1; Windows XP; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)",
				"Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 6.0)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1; DigExt)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.0; YComp 5.0.2.6)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.0; YComp 5.0.0.0) (Compatible;  ;  ; Trident/4.0)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.0; YComp 5.0.0.0)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.0; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.0)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 4.0; .NET CLR 1.0.2914)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 4.0)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows 98; YComp 5.0.0.0)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows 98; Win 9x 4.90)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows 98)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.0; .NET CLR 1.0.3705)",
				"Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 4.0)",
				"Mozilla/5.0 (Windows; U; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)",
				"Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)",
				"Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4325)",
				"Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1)",
				"Mozilla/45.0 (compatible; MSIE 6.0; Windows NT 5.1)",
				"Mozilla/4.08 (compatible; MSIE 6.0; Windows NT 5.1)",
				"Mozilla/4.01 (compatible; MSIE 6.0; Windows NT 5.1)",
				"Mozilla/4.0 (X11; MSIE 6.0; i686; .NET CLR 1.1.4322; .NET CLR 2.0.50727; FDM)",
				"Mozilla/4.0 (Windows; MSIE 6.0; Windows NT 6.0)",
				"Mozilla/4.0 (Windows; MSIE 6.0; Windows NT 5.2)",
				"Mozilla/4.0 (Windows; MSIE 6.0; Windows NT 5.0)",
				"Mozilla/4.0 (Windows;  MSIE 6.0;  Windows NT 5.1;  SV1; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (MSIE 6.0; Windows NT 5.1)",
				"Mozilla/4.0 (MSIE 6.0; Windows NT 5.0)",
				"Mozilla/4.0 (compatible;MSIE 6.0;Windows 98;Q312461)",
				"Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; U; MSIE 6.0; Windows NT 5.1) (Compatible;  ;  ; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; U; MSIE 6.0; Windows NT 5.1)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; InfoPath.3; Tablet PC 2.0)",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; GTB6.5; QQDownload 534; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; SLCC2; .NET CLR 2.0.50727; Media Center PC 6.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729)",
				"Mozilla/4.0 (compatible; MSIE 5.5b1; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.50; Windows NT; SiteKiosk 4.9; SiteCoach 1.0)",
				"Mozilla/4.0 (compatible; MSIE 5.50; Windows NT; SiteKiosk 4.8; SiteCoach 1.0)",
				"Mozilla/4.0 (compatible; MSIE 5.50; Windows NT; SiteKiosk 4.8)",
				"Mozilla/4.0 (compatible; MSIE 5.50; Windows 98; SiteKiosk 4.8)",
				"Mozilla/4.0 (compatible; MSIE 5.50; Windows 95; SiteKiosk 4.8)",
				"Mozilla/4.0 (compatible;MSIE 5.5; Windows 98)",
				"Mozilla/4.0 (compatible; MSIE 6.0; MSIE 5.5; Windows NT 5.1)",
				"Mozilla/4.0 (compatible; MSIE 5.5;)",
				"Mozilla/4.0 (Compatible; MSIE 5.5; Windows NT5.0; Q312461; SV1; .NET CLR 1.1.4322; InfoPath.2)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT5)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 6.1; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 6.1; chromeframe/12.0.742.100; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30618)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.5)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.2; .NET CLR 1.1.4322; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; FDM)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.2; .NET CLR 1.1.4322) (Compatible;  ;  ; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.2; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.1; Trident/4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
				"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
				"Mozilla/4.0 (compatible; MSIE 5.23; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.22; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.21; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.2; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.2; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.17; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.17; Mac_PowerPC Mac OS; en)",
				"Mozilla/4.0 (compatible; MSIE 5.16; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.16; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.15; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.15; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.14; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.13; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.12; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.12; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.05; Windows NT 4.0)",
				"Mozilla/4.0 (compatible; MSIE 5.05; Windows NT 3.51)",
				"Mozilla/4.0 (compatible; MSIE 5.05; Windows 98; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT; YComp 5.0.0.0)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT; Hotbar 4.1.8.0)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT; DigExt)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT; .NET CLR 1.0.3705)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.2.6; MSIECrawler)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.2.6; Hotbar 4.2.8.0)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.2.6; Hotbar 3.0)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.2.6)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.2.4)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.0.0; Hotbar 4.1.8.0)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; YComp 5.0.0.0)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; Wanadoo 5.6)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; Wanadoo 5.3; Wanadoo 5.5)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; Wanadoo 5.1)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; SV1; .NET CLR 1.1.4322; .NET CLR 1.0.3705; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; SV1)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; Q312461; T312461)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; Q312461)",
				"Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0; MSIECrawler)",
				"Mozilla/4.0 (compatible; MSIE 5.0b1; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 5.00; Windows 98)",
				"Mozilla/4.0(compatible; MSIE 5.0; Windows 98; DigExt)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT;)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt; YComp 5.0.2.6)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt; YComp 5.0.2.5)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt; YComp 5.0.0.0)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt; Hotbar 4.1.8.0)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt; Hotbar 3.0)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt; .NET CLR 1.0.3705)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT; DigExt)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT 6.0; Trident/4.0; InfoPath.1; SV1; .NET CLR 3.0.04506.648; .NET4.0C; .NET4.0E)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT 5.9; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT 5.2; .NET CLR 1.1.4322)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows NT 5.0)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows 98;)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; YComp 5.0.2.4)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; Hotbar 3.0)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt; YComp 5.0.2.6; yplus 1.0)",
				"Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt; YComp 5.0.2.6)",
				"Mozilla/4.0 (compatible; MSIE 4.5; Windows NT 5.1; .NET CLR 2.0.40607)",
				"Mozilla/4.0 (compatible; MSIE 4.5; Windows 98; )",
				"Mozilla/4.0 (compatible; MSIE 4.5; Mac_PowerPC)",
				"Mozilla/4.0 (compatible; MSIE 4.5; Mac_PowerPC)",
				"Mozilla/4.0 PPC (compatible; MSIE 4.01; Windows CE; PPC; 240x320; Sprint:PPC-6700; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows NT)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows NT 5.0)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint;PPC-i830; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint; SCH-i830; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint:SPH-ip830w; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint:SPH-ip320; Smartphone; 176x220)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint:SCH-i830; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint:SCH-i320; Smartphone; 176x220)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Sprint:PPC-i830; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; Smartphone; 176x220)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; PPC; 240x320; Sprint:PPC-6700; PPC; 240x320)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; PPC; 240x320; PPC)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; PPC)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows CE)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows 98; Hotbar 3.0)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows 98; DigExt)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows 98)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Windows 95)",
				"Mozilla/4.0 (compatible; MSIE 4.01; Mac_PowerPC)",
				"Mozilla/4.0 WebTV/2.6 (compatible; MSIE 4.0)",
				"Mozilla/4.0 (compatible; MSIE 4.0; Windows NT)",
				"Mozilla/4.0 (compatible; MSIE 4.0; Windows 98 )",
				"Mozilla/4.0 (compatible; MSIE 4.0; Windows 95; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
				"Mozilla/4.0 (compatible; MSIE 4.0; Windows 95)",
				"Mozilla/4.0 (Compatible; MSIE 4.0)",
				"Mozilla/2.0 (compatible; MSIE 4.0; Windows 98)",
				"Mozilla/2.0 (compatible; MSIE 3.03; Windows 3.1)",
				"Mozilla/2.0 (compatible; MSIE 3.02; Windows 3.1)",
				"Mozilla/2.0 (compatible; MSIE 3.01; Windows 95)",
				"Mozilla/2.0 (compatible; MSIE 3.01; Windows 95)",
				"Mozilla/2.0 (compatible; MSIE 3.0B; Windows NT)",
				"Mozilla/3.0 (compatible; MSIE 3.0; Windows NT 5.0)",
				"Mozilla/2.0 (compatible; MSIE 3.0; Windows 95)",
				"Mozilla/2.0 (compatible; MSIE 3.0; Windows 3.1)",
				"Mozilla/4.0 (compatible; MSIE 2.0; Windows NT 5.0; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)",
				"Mozilla/1.22 (compatible; MSIE 2.0; Windows 95)",
				"Mozilla/1.22 (compatible; MSIE 2.0; Windows 3.1)"];

var firefoxUserAgents = [
				"Mozilla/5.0 (Windows NT 5.1; rv:31.0) Gecko/20100101 Firefox/31.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:29.0) Gecko/20120101 Firefox/29.0",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/29.0",
				"Mozilla/5.0 (X11; OpenBSD amd64; rv:28.0) Gecko/20100101 Firefox/28.0",
				"Mozilla/5.0 (X11; Linux x86_64; rv:28.0) Gecko/20100101  Firefox/28.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:27.3) Gecko/20130101 Firefox/27.3",
				"Mozilla/5.0 (Windows NT 6.2; Win64; x64; rv:27.0) Gecko/20121011 Firefox/27.0",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:25.0) Gecko/20100101 Firefox/25.0",
				"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:24.0) Gecko/20100101 Firefox/24.0",
				"Mozilla/5.0 (Windows NT 6.0; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:24.0) Gecko/20100101 Firefox/24.0",
				"Mozilla/5.0 (Windows NT 6.2; rv:22.0) Gecko/20130405 Firefox/23.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20130406 Firefox/23.0",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:23.0) Gecko/20131011 Firefox/23.0",
				"Mozilla/5.0 (Windows NT 6.2; rv:22.0) Gecko/20130405 Firefox/22.0",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:22.0) Gecko/20130328 Firefox/22.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:22.0) Gecko/20130405 Firefox/22.0",
				"Mozilla/5.0 (Windows NT 6.2; Win64; x64; rv:16.0.1) Gecko/20121011 Firefox/21.0.1",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:16.0.1) Gecko/20121011 Firefox/21.0.1",
				"Mozilla/5.0 (Windows NT 6.2; Win64; x64; rv:21.0.0) Gecko/20121011 Firefox/21.0.0",
				"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:21.0) Gecko/20130331 Firefox/21.0",
				"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (X11; Linux i686; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.2; rv:21.0) Gecko/20130326 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20130401 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20130331 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20130330 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20130401 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20130328 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:21.0) Gecko/20130401 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:21.0) Gecko/20130331 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 5.0; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:21.0) Gecko/20100101 Firefox/21.0",
				"Mozilla/5.0 (Windows NT 6.2; Win64; x64;) Gecko/20100101 Firefox/20.0",
				"Mozilla/5.0 (Windows x86; rv:19.0) Gecko/20100101 Firefox/19.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:6.0) Gecko/20100101 Firefox/19.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:14.0) Gecko/20100101 Firefox/18.0.1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0)  Gecko/20100101 Firefox/18.0",
				"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:17.0) Gecko/20100101 Firefox/17.0.6",
				"Mozilla/5.0 (X11; Ubuntu; Linux armv7l; rv:17.0) Gecko/20100101 Firefox/17.0",
				"Mozilla/6.0 (Windows NT 6.2; WOW64; rv:16.0.1) Gecko/20121011 Firefox/16.0.1",
				"Mozilla/5.0 (Windows NT 6.2; WOW64; rv:16.0.1) Gecko/20121011 Firefox/16.0.1",
				"Mozilla/5.0 (Windows NT 6.2; Win64; x64; rv:16.0.1) Gecko/20121011 Firefox/16.0.1",
				"Mozilla/5.0 (X11; NetBSD amd64; rv:16.0) Gecko/20121102 Firefox/16.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:15.0) Gecko/20120716 Firefox/15.0a2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.16) Gecko/20120427 Firefox/15.0a1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:15.0) Gecko/20120427 Firefox/15.0a1",
				"Mozilla/5.0 (Windows NT 6.2; WOW64; rv:15.0) Gecko/20120910144328 Firefox/15.0.2",
				"Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:15.0) Gecko/20100101 Firefox/15.0.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; rv:15.0) Gecko/20121011 Firefox/15.0.1",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:14.0) Gecko/20120405 Firefox/14.0a1",
				"Mozilla/5.0 (Windows NT 6.1; rv:14.0) Gecko/20120405 Firefox/14.0a1",
				"Mozilla/5.0 (Windows NT 5.1; rv:14.0) Gecko/20120405 Firefox/14.0a1",
				"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:14.0) Gecko/20100101 Firefox/14.0.1",
				"Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:14.0) Gecko/20100101 Firefox/14.0.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; WOW64; en-US; rv:2.0.4) Gecko/20120718 AskTbAVR-IDW/3.12.5.17700 Firefox/14.0.1",
				"Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20120403211507 Firefox/14.0.1",
				"Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/ 20120405 Firefox/14.0.1",
				"Mozilla/5.0 (Windows NT 6.0; rv:14.0) Gecko/20100101 Firefox/14.0.1",
				"Mozilla/5.0 (Windows NT 5.1; rv:15.0) Gecko/20100101 Firefox/13.0.1",
				"Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20120403211507 Firefox/12.0",
				"Mozilla/5.0 (Windows NT 6.1; de;rv:12.0) Gecko/20120403211507 Firefox/12.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:12.0) Gecko/20120403211507 Firefox/12.0",
				"Mozilla/5.0 (compatible; Windows; U; Windows NT 6.2; WOW64; en-US; rv:12.0) Gecko/20120403211507 Firefox/12.0",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.1.16) Gecko/20120421 Gecko Firefox/11.0",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.1.16) Gecko/20120421 Firefox/11.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko Firefox/11.0",
				"Mozilla/5.0 (Windows NT 6.1; U;WOW64; de;rv:11.0) Gecko Firefox/11.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:11.0) Gecko Firefox/11.0",
				"Mozilla/6.0 (Macintosh; I; Intel Mac OS X 11_7_9; de-LI; rv:1.9b4) Gecko/2012010317 Firefox/10.0a4",
				"Mozilla/5.0 (Macintosh; I; Intel Mac OS X 11_7_9; de-LI; rv:1.9b4) Gecko/2012010317 Firefox/10.0a4",
				"Mozilla/5.0 (X11; Mageia; Linux x86_64; rv:10.0.9) Gecko/20100101 Firefox/10.0.9",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:9.0a2) Gecko/20111101 Firefox/9.0a2",
				"Mozilla/5.0 (Windows NT 6.2; rv:9.0.1) Gecko/20100101 Firefox/9.0.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:9.0) Gecko/20100101 Firefox/9.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:8.0; en_us) Gecko/20100101 Firefox/8.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:6.0) Gecko/20100101 Firefox/7.0",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0a2) Gecko/20110613 Firefox/6.0a2",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0a2) Gecko/20110612 Firefox/6.0a2",
				"Mozilla/5.0 (X11; Linux i686; rv:6.0) Gecko/20100101 Firefox/6.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:6.0) Gecko/20110814 Firefox/6.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:6.0) Gecko/20100101 Firefox/6.0 FirePHP/0.6",
				"Mozilla/5.0 (Windows NT 5.0; WOW64; rv:6.0) Gecko/20100101 Firefox/6.0",
				"Mozilla/5.0 (X11; Linux i686 on x86_64; rv:5.0a2) Gecko/20110524 Firefox/5.0a2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.28) Gecko/20120306 Firefox/5.0.1",
				"Mozilla/5.0 (Windows NT 6.1; U; ru; rv:5.0.1.6) Gecko/20110501 Firefox/5.0.1 Firefox/5.0.1",
				"mozilla/3.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/5.0.1",
				"Mozilla/5.0 (X11; U; Linux i586; de; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (X11; U; Linux amd64; rv:5.0) Gecko/20100101 Firefox/5.0 (Debian)",
				"Mozilla/5.0 (X11; U; Linux amd64; en-US; rv:5.0) Gecko/20110619 Firefox/5.0",
				"Mozilla/5.0 (X11; Linux) Gecko Firefox/5.0",
				"Mozilla/5.0 (X11; Linux x86_64; rv:5.0) Gecko/20100101 Firefox/5.0 FirePHP/0.5",
				"Mozilla/5.0 (X11; Linux x86_64; rv:5.0) Gecko/20100101 Firefox/5.0 Firefox/5.0",
				"Mozilla/5.0 (X11; Linux x86_64) Gecko Firefox/5.0",
				"Mozilla/5.0 (X11; Linux ppc; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (X11; Linux AMD64) Gecko Firefox/5.0",
				"Mozilla/5.0 (X11; FreeBSD amd64; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 6.2; WOW64; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:5.0) Gecko/20110619 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:6.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 6.1.1; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 5.2; WOW64; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 5.1; U; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 5.0; WOW64; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (Windows NT 5.0; rv:5.0) Gecko/20100101 Firefox/5.0",
				"Mozilla/5.0 (X11; Linux x86_64; rv:2.2a1pre) Gecko/20110324 Firefox/4.2a1pre",
				"Mozilla/5.0 (X11; Linux x86_64; rv:2.2a1pre) Gecko/20100101 Firefox/4.2a1pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.2a1pre) Gecko/20110324 Firefox/4.2a1pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.2a1pre) Gecko/20110323 Firefox/4.2a1pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.2a1pre) Gecko/20110208 Firefox/4.2a1pre",
				"Mozilla/5.0 (X11; Linux x86_64; rv:2.0b9pre) Gecko/20110111 Firefox/4.0b9pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b9pre) Gecko/20101228 Firefox/4.0b9pre",
				"Mozilla/5.0 (Windows NT 5.1; rv:2.0b9pre) Gecko/20110105 Firefox/4.0b9pre",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b8pre) Gecko/20101114 Firefox/4.0b8pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b8pre) Gecko/20101213 Firefox/4.0b8pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b8pre) Gecko/20101128 Firefox/4.0b8pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b8pre) Gecko/20101114 Firefox/4.0b8pre",
				"Mozilla/5.0 (Windows NT 5.1; rv:2.0b8pre) Gecko/20101127 Firefox/4.0b8pre",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0b8) Gecko/20100101 Firefox/4.0b8",
				"Mozilla/4.0 (compatible;  Intel Mac OS X 10.6; rv:2.0b8) Gecko/20100101 Firefox/4.0b8)",
				"Mozilla/5.0 (Windows NT 6.1; rv:2.0b7pre) Gecko/20100921 Firefox/4.0b7pre",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b7) Gecko/20101111 Firefox/4.0b7",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b7) Gecko/20100101 Firefox/4.0b7",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b6pre) Gecko/20100903 Firefox/4.0b6pre",
				"Mozilla/5.0 (Windows NT 6.1; rv:2.0b6pre) Gecko/20100903 Firefox/4.0b6pre Firefox/4.0b6pre",
				"Mozilla/5.0 (X11; Linux x86_64; rv:2.0b4) Gecko/20100818 Firefox/4.0b4",
				"Mozilla/5.0 (X11; Linux i686; rv:2.0b3pre) Gecko/20100731 Firefox/4.0b3pre",
				"Mozilla/5.0 (Windows NT 5.2; rv:2.0b13pre) Gecko/20110304 Firefox/4.0b13pre",
				"Mozilla/5.0 (Windows NT 5.1; rv:2.0b13pre) Gecko/20110223 Firefox/4.0b13pre",
				"Mozilla/5.0 (X11; Linux i686; rv:2.0b12pre) Gecko/20110204 Firefox/4.0b12pre",
				"Mozilla/5.0 (X11; Linux i686; rv:2.0b12pre) Gecko/20100101 Firefox/4.0b12pre",
				"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b11pre) Gecko/20110128 Firefox/4.0b11pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b11pre) Gecko/20110131 Firefox/4.0b11pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b11pre) Gecko/20110129 Firefox/4.0b11pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b11pre) Gecko/20110128 Firefox/4.0b11pre",
				"Mozilla/5.0 (Windows NT 6.1; rv:2.0b11pre) Gecko/20110126 Firefox/4.0b11pre",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0b11pre) Gecko/20110126 Firefox/4.0b11pre",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0b10pre) Gecko/20110118 Firefox/4.0b10pre",
				"Mozilla/5.0 (Windows NT 6.1; rv:2.0b10pre) Gecko/20110113 Firefox/4.0b10pre",
				"Mozilla/5.0 (X11; Linux i686; rv:2.0b10) Gecko/20100101 Firefox/4.0b10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:2.0b10) Gecko/20110126 Firefox/4.0b10",
				"Mozilla/5.0 (Windows NT 6.1; rv:2.0b10) Gecko/20110126 Firefox/4.0b10",
				"Mozilla/5.0 (X11; U; Linux i686; ru; rv:1.9.1.3) Gecko/20091020 Ubuntu/10.04 (lucid) Firefox/4.0.1",
				"Mozilla/5.0 (X11; Linux x86_64; rv:2.0.1) Gecko/20110506 Firefox/4.0.1",
				"Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20110518 Firefox/4.0.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:2.0.1) Gecko/20110606 Firefox/4.0.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; pl-PL; rv:2.0) Gecko/20110307 Firefox/4.0",
				"Mozilla/5.0 (X11; U; Linux i686; en-GB; rv:2.0) Gecko/20110404 Fedora/16-dev Firefox/4.0",
				"Mozilla/5.0 (X11; Arch Linux i686; rv:2.0) Gecko/20110321 Firefox/4.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows NT 6.1; rv:2.0) Gecko/20110319 Firefox/4.0",
				"Mozilla/5.0 (Windows NT 6.1; rv:1.9) Gecko/20100101 Firefox/4.0",
				"Mozilla/5.0 (X11; U; Linux i686; pl-PL; rv:1.9.0.2) Gecko/20121223 Ubuntu/9.25 (jaunty) Firefox/3.8",
				"Mozilla/5.0 (X11; U; Linux i686; pl-PL; rv:1.9.0.2) Gecko/2008092313 Ubuntu/9.25 (jaunty) Firefox/3.8",
				"Mozilla/5.0 (X11; U; Linux i686; it-IT; rv:1.9.0.2) Gecko/2008092313 Ubuntu/9.25 (jaunty) Firefox/3.8",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.3) Gecko/20100401 Mozilla/5.0 (X11; U; Linux i686; it-IT; rv:1.9.0.2) Gecko/2008092313 Ubuntu/9.25 (jaunty) Firefox/3.8",
				"Mozilla/5.0 (X11; U; Linux i686; ru; rv:1.9.3a5pre) Gecko/20100526 Firefox/3.7a5pre",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2b5) Gecko/20091204 Firefox/3.6b5",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2b5) Gecko/20091204 Firefox/3.6b5",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; fr; rv:1.9.2b5) Gecko/20091204 Firefox/3.6b5",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.2) Gecko/20091218 Firefox 3.6b5",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; fr; rv:1.9.2b4) Gecko/20091124 Firefox/3.6b4 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2b4) Gecko/20091124 Firefox/3.6b4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2b1) Gecko/20091014 Firefox/3.6b1 GTB5",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2a1pre) Gecko/20090428 Firefox/3.6a1pre",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2a1pre) Gecko/20090405 Firefox/3.6a1pre",
				"Mozilla/5.0 (X11; U; Linux i686; ru-RU; rv:1.9.2a1pre) Gecko/20090405 Ubuntu/9.04 (jaunty) Firefox/3.6a1pre",
				"Mozilla/5.0 (Windows; Windows NT 5.1; es-ES; rv:1.9.2a1pre) Gecko/20090402 Firefox/3.6a1pre",
				"Mozilla/5.0 (Windows; Windows NT 5.1; en-US; rv:1.9.2a1pre) Gecko/20090402 Firefox/3.6a1pre",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; ja; rv:1.9.2a1pre) Gecko/20090402 Firefox/3.6a1pre (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.9) Gecko/20100915 Gentoo Firefox/3.6.9",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.9) Gecko/20100827 Red Hat/3.6.9-2.el6 Firefox/3.6.9",
				"Mozilla/5.0 (X11; U; FreeBSD i386; en-US; rv:1.9.2.9) Gecko/20100913 Firefox/3.6.9",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:1.9.2.9) Gecko/20100913 Firefox/3.6.9",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.9.2.9) Gecko/20100824 Firefox/3.6.9 ( .NET CLR 3.5.30729; .NET CLR 4.0.20506)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-GB; rv:1.9.2.9) Gecko/20100824 Firefox/3.6.9",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6;en-US; rv:1.9.2.9) Gecko/20100824 Firefox/3.6.9",
				"Mozilla/5.0 (X11; U; OpenBSD i386; en-US; rv:1.9.2.8) Gecko/20101230 Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.8) Gecko/20100804 Gentoo Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.8) Gecko/20100723 SUSE/3.6.8-0.1.1 Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux i686; zh-CN; rv:1.9.2.8) Gecko/20100722 Ubuntu/10.04 (lucid) Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux i686; ru; rv:1.9.2.8) Gecko/20100723 Ubuntu/10.04 (lucid) Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux i686; fi-FI; rv:1.9.2.8) Gecko/20100723 Ubuntu/10.04 (lucid) Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.8) Gecko/20100727 Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; Linux i686; de-DE; rv:1.9.2.8) Gecko/20100725 Gentoo Firefox/3.6.8",
				"Mozilla/5.0 (X11; U; FreeBSD i386; de-CH; rv:1.9.2.8) Gecko/20100729 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; pt-BR; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; it; rv:1.9.2.8) Gecko/20100722 AskTbADAP/3.9.1.14019 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; he; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; fr; rv:1.9.2.8) Gecko/20100722 Firefox 3.6.8 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 ( .NET CLR 3.5.30729; .NET4.0C)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.8) Gecko/20100722 Firefox 3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.3) Gecko/20121221 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-TW; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; tr; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.7) Gecko/20100809 Fedora/3.6.7-1.fc14 Firefox/3.6.7",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.7) Gecko/20100723 Fedora/3.6.7-1.fc13 Firefox/3.6.7",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.7) Gecko/20100726 CentOS/3.6-3.el5.centos Firefox/3.6.7",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; hu; rv:1.9.2.7) Gecko/20100713 Firefox/3.6.7 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; ru; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.7 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; pt-PT; rv:1.9.2.7) Gecko/20100713 Firefox/3.6.7 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.6) Gecko/20100628 Ubuntu/10.04 (lucid) Firefox/3.6.6 GTB7.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.6) Gecko/20100628 Ubuntu/10.04 (lucid) Firefox/3.6.6 GTB7.0",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.6) Gecko/20100628 Ubuntu/10.04 (lucid) Firefox/3.6.6 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.6) Gecko/20100628 Ubuntu/10.04 (lucid) Firefox/3.6.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; pt-PT; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; it; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; zh-CN; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; nl; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; de-AT; rv:1.9.1.8) Gecko/20100625 Firefox/3.6.6",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.4) Gecko/20100614 Ubuntu/10.04 (lucid) Firefox/3.6.4",
				"Mozilla/5.0 (X11; U; Linux i686; fa; rv:1.8.1.4) Gecko/20100527 Firefox/3.6.4",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.4) Gecko/20100625 Gentoo Firefox/3.6.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-TW; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.4) Gecko/20100513 Firefox/3.6.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ja; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; cs; rv:1.9.2.4) Gecko/20100513 Firefox/3.6.4 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; zh-CN; rv:1.9.2.4) Gecko/20100513 Firefox/3.6.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; ja; rv:1.9.2.4) Gecko/20100513 Firefox/3.6.4 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; fr; rv:1.9.2.4) Gecko/20100523 Firefox/3.6.4 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.4) Gecko/20100527 Firefox/3.6.4 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.4) Gecko/20100527 Firefox/3.6.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.4) Gecko/20100523 Firefox/3.6.4 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.4) Gecko/20100513 Firefox/3.6.4 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-CA; rv:1.9.2.4) Gecko/20100523 Firefox/3.6.4",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-TW; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4 GTB7.0 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.4) Gecko/20100513 Firefox/3.6.4 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.4) Gecko/20100503 Firefox/3.6.4 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; nb-NO; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; ko; rv:1.9.2.4) Gecko/20100523 Firefox/3.6.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.3pre) Gecko/20100405 Firefox/3.6.3plugin1 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; he; rv:1.9.1b4pre) Gecko/20100405 Firefox/3.6.3plugin1",
				"Mozilla/5.0 (X11; U; Linux x86_64; fr; rv:1.9.2.3) Gecko/20100403 Fedora/3.6.3-4.fc13 Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.3) Gecko/20100403 Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; de; rv:1.9.2.3) Gecko/20100401 SUSE/3.6.3-1.1 Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux i686; ko-KR; rv:1.9.2.3) Gecko/20100423 Ubuntu/10.04 (lucid) Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.3) Gecko/20100404 Ubuntu/10.04 (lucid) Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 GTB7.1",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.3) Gecko/20100423 Ubuntu/10.04 (lucid) Firefox/3.6.3",
				"Mozilla/5.0 (X11; U; Linux AMD64; en-US; rv:1.9.2.3) Gecko/20100403 Ubuntu/10.10 (maverick) Firefox/3.6.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; pl; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; it; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; hu; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; es-ES; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 GTB7.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; es-ES; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 GTB7.0 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; es-ES; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; cs; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ca; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; fr; rv:1.9.2.28) Gecko/20120306 Firefox/3.6.28",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.9.2.28) Gecko/20120306 AskTbSTC-SRS/3.13.1.18132 Firefox/3.6.28 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.28) Gecko/20120306 Firefox/3.6.28 ( .NET CLR 3.5.30729; .NET4.0C)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; ja; rv:1.9.2.25) Gecko/20111212 Firefox/3.6.25 ( .NET CLR 3.5.30729; .NET4.0C)",
				"Mozilla/5.0 (X11; U; Linux x86_64; it; rv:1.9.2.24) Gecko/20111101 SUSE/3.6.24-0.2.1 Firefox/3.6.24",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; fr; rv:1.9.2.23) Gecko/20110920 Firefox/3.6.23",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X 10.4; en-US; rv:1.9.2.22) Gecko/20110902 Firefox/3.6.22",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.5; it; rv:1.9.2.22) Gecko/20110902 Firefox/3.6.22",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.21) Gecko/20110830 Ubuntu/10.10 (maverick) Firefox/3.6.21",
				"Mozilla/5.0 (X11; U; OpenBSD i386; en-US; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20",
				"Mozilla/5.0 (X11; U; Linux x86_64; it; rv:1.9.2.20) Gecko/20110805 Ubuntu/10.04 (lucid) Firefox/3.6.20",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.20) Gecko/20110804 Red Hat/3.6-2.el5 Firefox/3.6.20",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; hu; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; de; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; hu; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.20) Gecko/20110803 AskTbFWV5/3.13.0.17701 Firefox/3.6.20 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; cs; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.5; en-US; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.20",
				"Mozilla/5.0 (X11; U; Linux i686; fr; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; fr; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2 GTB7.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.2) Gecko/20100316 AskTbSPC2/3.9.1.14019 Firefox/3.6.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; ru; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; pl; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2 GTB6 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2 ( .NET CLR 3.0.04506.648)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2 ( .NET CLR 3.0.04506.30)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.7; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.10pre) Gecko/20100902 Ubuntu/9.10 (karmic) Firefox/3.6.1pre",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; de; rv:1.9.2.20) Gecko/20110803 Firefox/3.6.19",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X 10.4; en-GB; rv:1.9.2.19) Gecko/20110707 Firefox/3.6.19",
				"Mozilla/5.0 (X11; U; Linux x86_64; ru; rv:1.9.2.18) Gecko/20110628 Ubuntu/10.10 (maverick) Firefox/3.6.18",
				"Mozilla/5.0 (X11; U; Linux i686; pl; rv:1.9.2.18) Gecko/20110614 Firefox/3.6.18 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (X11; U; Linux i686; en-GB; rv:1.9.2.18) Gecko/20110628 Ubuntu/10.10 (maverick) Firefox/3.6.18",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.18) Gecko/20110628 Ubuntu/10.10 (maverick) Firefox/3.6.18",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.18) Gecko/20110615 Ubuntu/10.10 (maverick) Firefox/3.6.18",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; pt-BR; rv:1.9.2.18) Gecko/20110614 Firefox/3.6.18 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ar; rv:1.9.2.18) Gecko/20110614 Firefox/3.6.18",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; pt-BR; rv:1.9.2.18) Gecko/20110614 Firefox/3.6.18 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.9.2.18) Gecko/20110614 Firefox/3.6.18 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (X11; U; Linux i686 (x86_64); en-GB; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17",
				"Mozilla/5.0 (X11; Linux i686 on x86_64; rv:5.0) Gecko/20100101 Firefox/3.6.17 Firefox/3.6.17",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; tr; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; sl; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; pt-BR; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; hu; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; fr; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (X11; U; Linux x86_64; ja-JP; rv:1.9.2.16) Gecko/20110323 Ubuntu/10.10 (maverick) Firefox/3.6.16",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.16) Gecko/20110323 Ubuntu/9.10 (karmic) Firefox/3.6.16 FirePHP/0.5",
				"Mozilla/5.0 (X11; U; Linux i686; en-GB; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; fr; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; pl; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; ko; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; fr; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en; rv:1.9.1.13) Gecko/20100914 Firefox/3.6.16",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.9.2.16) Gecko/20110319 AskTbUTR/3.11.3.15590 Firefox/3.6.16",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.16pre) Gecko/20110304 Ubuntu/10.10 (maverick) Firefox/3.6.15pre",
				"Mozilla/5.0 (X11; U; Linux i686; nl; rv:1.9.2.15) Gecko/20110303 Ubuntu/8.04 (hardy) Firefox/3.6.15",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.15) Gecko/20110303 Ubuntu/10.04 (lucid) Firefox/3.6.15 FirePHP/0.5",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.15) Gecko/20110330 CentOS/3.6-1.el5.centos Firefox/3.6.15",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; es-ES; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15 ( .NET CLR 3.5.30729; .NET4.0C) FirePHP/0.5",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.9.2.15) Gecko/20110303 AskTbBT4/3.11.3.15590 Firefox/3.6.15 ( .NET CLR 3.5.30729; .NET4.0C)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.14pre) Gecko/20110105 Firefox/3.6.14pre",
				"Mozilla/5.0 (X11; U; Linux armv7l; en-US; rv:1.9.2.14) Gecko/20110224 Firefox/3.6.14 MB860/Version.0.43.3.MB860.AmericaMovil.en.MX",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.14) Gecko/20110218 Firefox/3.6.14",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-AU; rv:1.9.2.14) Gecko/20110218 Firefox/3.6.14",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.9.2.14) Gecko/20110218 Firefox/3.6.14 GTB7.1 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.9.2.13) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; pl-PL; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.04 (lucid) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; nb-NO; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.04 (lucid) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; it; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.04 (lucid) Firefox/3.6.13 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux x86_64; fr; rv:1.9.2.13) Gecko/20110103 Fedora/3.6.13-1.fc14 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; fr; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.13) Gecko/20101223 Gentoo Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.13) Gecko/20101219 Gentoo Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.13) Gecko/20101206 Red Hat/3.6-3.el4 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.13) Gecko/20101206 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-NZ; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.10 (maverick) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-GB; rv:1.9.2.13) Gecko/20101206 Ubuntu/9.10 (karmic) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-GB; rv:1.9.2.13) Gecko/20101206 Red Hat/3.6-2.el5 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; da-DK; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.10 (maverick) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux MIPS32 1074Kf CPS QuadCore; en-US; rv:1.9.2.13) Gecko/20110103 Fedora/3.6.13-1.fc14 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux i686; ru; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.10 (maverick) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux i686; pt-BR; rv:1.9.2.13) Gecko/20101209 Fedora/3.6.13-1.fc13 Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux i686; es-ES; rv:1.9.2.13) Gecko/20101206 Ubuntu/9.10 (karmic) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.13) Gecko/20101209 CentOS/3.6-2.el5.centos Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.13) Gecko/20101206 Ubuntu/10.10 (maverick) Firefox/3.6.13",
				"Mozilla/5.0 (X11; U; NetBSD i386; en-US; rv:1.9.2.12) Gecko/20101030 Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux x86_64; es-MX; rv:1.9.2.12) Gecko/20101027 Ubuntu/10.04 (lucid) Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux x86_64; es-ES; rv:1.9.2.12) Gecko/20101027 Fedora/3.6.12-1.fc13 Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux x86_64; es-ES; rv:1.9.2.12) Gecko/20101026 SUSE/3.6.12-0.7.1 Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.12) Gecko/20101102 Gentoo Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.12) Gecko/20101102 Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux ppc; fr; rv:1.9.2.12) Gecko/20101027 Ubuntu/10.10 (maverick) Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux i686; ko-KR; rv:1.9.2.12) Gecko/20101027 Ubuntu/10.10 (maverick) Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.12) Gecko/20101114 Gentoo Firefox/3.6.12",
				"Mozilla/5.0 (X11; U; Linux i686; en-GB; rv:1.9.2.12) Gecko/20101027 Ubuntu/10.10 (maverick) Firefox/3.6.12 GTB7.1",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.12) Gecko/20101027 Fedora/3.6.12-1.fc13 Firefox/3.6.12",
				"Mozilla/5.0 (X11; FreeBSD x86_64; rv:2.0) Gecko/20100101 Firefox/3.6.12",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12 ( .NET CLR 3.5.30729; .NET4.0E)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; sv-SE; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12 (.NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; .NET CLR 3.5.21022)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12 GTB5",
				"Mozilla/5.0 (X11; U; Linux x86_64; ru; rv:1.9.2.11) Gecko/20101028 CentOS/3.6-2.el5.centos Firefox/3.6.11",
				"Mozilla/5.0 (X11; U; Linux armv7l; en-GB; rv:1.9.2.3pre) Gecko/20100723 Firefox/3.6.11",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; ru; rv:1.9.2.11) Gecko/20101012 Firefox/3.6.11",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2;  rv:1.9.2.11) Gecko/20101012 Firefox/3.6.11",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.9.2.11) Gecko/20101012 Firefox/3.6.11 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux x86_64; zh-CN; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; pt-BR; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; pl-PL; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10 GTB7.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; el-GR; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; de; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10 GTB7.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; cs-CZ; rv:1.9.2.10) Gecko/20100915 Ubuntu/10.04 (lucid) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; pl-PL; rv:1.9.2.10) Gecko/20100915 Ubuntu/10.04 (lucid) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; fr-FR; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; es-AR; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.10) Gecko/20100915 Ubuntu/9.04 (jaunty) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; en-GB; rv:1.9.2.11) Gecko/20101013 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; en-CA; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.10) Gecko/20100915 Ubuntu/9.10 (karmic) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.10) Gecko/20100915 Ubuntu/10.04 (lucid) Firefox/3.6.10",
				"Mozilla/5.0 (X11; U; Linux i686; de; rv:1.9.2.10) Gecko/20100914 SUSE/3.6.10-0.3.1 Firefox/3.6.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ro; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; nl; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10 ( .NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; fr; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.1) Gecko/20100122 firefox/3.6.1",
				"Mozilla/5.0(Windows; U; Windows NT 7.0; rv:1.9.2) Gecko/20100101 Firefox/3.6",
				"Mozilla/5.0(Windows; U; Windows NT 5.2; rv:1.9.2) Gecko/20100101 Firefox/3.6",
				"Mozilla/5.0 (X11; U; x86_64 Linux; en_GB, en_US; rv:1.9.2) Gecko/20100115 Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2) Gecko/20100222 Ubuntu/10.04 (lucid) Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.2) Gecko/20100130 Gentoo Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux x86_64; de; rv:1.9.2) Gecko/20100308 Ubuntu/10.04 (lucid) Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2.2pre) Gecko/20100312 Ubuntu/9.04 (jaunty) Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2) Gecko/20100128 Gentoo Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2) Gecko/20100115 Ubuntu/10.04 (lucid) Firefox/3.6",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.2) Gecko/20100115 Firefox/3.6 FirePHP/0.4",
				"Mozilla/5.0 (X11; Linux i686; rv:2.0) Gecko/20100101 Firefox/3.6",
				"Mozilla/5.0 (X11; FreeBSD i686) Firefox/3.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru-RU; rv:1.9.2) Gecko/20100105 MRA 5.6 (build 03278) Firefox/3.6 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; lt; rv:1.9.2) Gecko/20100115 Firefox/3.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.3a3pre) Gecko/20100306 Firefox3.6 (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.8) Gecko/20100806 Firefox/3.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.17) Gecko/20110420 Firefox/3.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.3) Gecko/20100401 Firefox/3.6;MEGAUPLOAD 1.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ar; rv:1.9.2) Gecko/20100115 Firefox/3.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; ru; rv:1.9.2) Gecko/20100115 Firefox/3.6",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1b5pre) Gecko/20090517 Firefox/3.5b4pre (.NET CLR 3.5.30729)",
				"Mozilla/5.0 (X11; U; Gentoo Linux x86_64; pl-PL) Gecko Firefox",
				"Mozilla/5.0 (X11; ; Linux x86_64; rv:1.8.1.6) Gecko/20070802 Firefox",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.9.0.6) Gecko/2009011913  Firefox",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; de-DE; rv:1.9.2.20) Gecko/20110803 Firefox",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X Mach-O; rv:1.8.1.16) Gecko/20080702 Firefox",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; en-US; rv:1.8.1.13) Gecko/20080313 Firefox"];

var chromeUserAgents = [
				"Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.67 Safari/537.36",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.67 Safari/537.36",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1944.0 Safari/537.36",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.47 Safari/537.36",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36 Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10",
				"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1664.3 Safari/537.36",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1664.3 Safari/537.36",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1623.0 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.17 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.62 Safari/537.36",
				"Mozilla/5.0 (X11; CrOS i686 4319.74.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.57 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.2 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1468.0 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1467.0 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1464.0 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1500.55 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.90 Safari/537.36",
				"Mozilla/5.0 (X11; NetBSD) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36",
				"Mozilla/5.0 (X11; CrOS i686 3912.101.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.60 Safari/537.17",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1309.0 Safari/537.17",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.15 (KHTML, like Gecko) Chrome/24.0.1295.0 Safari/537.15",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.14 (KHTML, like Gecko) Chrome/24.0.1292.0 Safari/537.14",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1290.1 Safari/537.13",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1290.1 Safari/537.13",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1290.1 Safari/537.13",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_4) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1290.1 Safari/537.13",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.13 (KHTML, like Gecko) Chrome/24.0.1284.0 Safari/537.13",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.6 Safari/537.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.6 Safari/537.11",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.26 Safari/537.11",
				"Mozilla/5.0 (Windows NT 6.0) yi; AppleWebKit/345667.12221 (KHTML, like Gecko) Chrome/23.0.1271.26 Safari/453667.1221",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.17 Safari/537.11",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.4 (KHTML, like Gecko) Chrome/22.0.1229.94 Safari/537.4",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_0) AppleWebKit/537.4 (KHTML, like Gecko) Chrome/22.0.1229.79 Safari/537.4",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.2 (KHTML, like Gecko) Chrome/22.0.1216.0 Safari/537.2",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/22.0.1207.1 Safari/537.1",
				"Mozilla/5.0 (X11; CrOS i686 2268.111.0) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.57 Safari/536.11",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1090.0 Safari/536.6",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/19.77.34.5 Safari/537.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.9 Safari/536.5",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.36 Safari/536.5",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_0) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1062.0 Safari/536.3",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1062.0 Safari/536.3",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.0 Safari/536.3",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.22 (KHTML, like Gecko) Chrome/19.0.1047.0 Safari/535.22",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.21 (KHTML, like Gecko) Chrome/19.0.1042.0 Safari/535.21",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.21 (KHTML, like Gecko) Chrome/19.0.1041.0 Safari/535.21",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.20 (KHTML, like Gecko) Chrome/19.0.1036.7 Safari/535.20",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/18.6.872.0 Safari/535.2 UNTRUSTED/1.0 3gpp-gba UNTRUSTED/1.0",
				"Mozilla/5.0 (Macintosh; AMD Mac OS X 10_8_2) AppleWebKit/535.22 (KHTML, like Gecko) Chrome/18.6.872",
				"Mozilla/5.0 (X11; CrOS i686 1660.57.0) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.46 Safari/535.19",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.45 Safari/535.19",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.45 Safari/535.19",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.45 Safari/535.19",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Safari/535.19",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_8) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.151 Safari/535.19",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.19 (KHTML, like Gecko) Ubuntu/11.10 Chromium/18.0.1025.142 Chrome/18.0.1025.142 Safari/535.19",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.11 Safari/535.19",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_8) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.11 (KHTML, like Gecko) Ubuntu/11.10 Chromium/17.0.963.65 Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.11 (KHTML, like Gecko) Ubuntu/11.04 Chromium/17.0.963.65 Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.11 (KHTML, like Gecko) Ubuntu/10.10 Chromium/17.0.963.65 Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.11 (KHTML, like Gecko) Ubuntu/11.10 Chromium/17.0.963.65 Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (X11; FreeBSD amd64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_4) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.65 Safari/535.11",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.11 (KHTML, like Gecko) Ubuntu/11.04 Chromium/17.0.963.56 Chrome/17.0.963.56 Safari/535.11",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.12 Safari/535.11",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/17.0.940.0 Safari/535.8",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.77 Safari/535.7ad-imcjapan-syosyaman-xkgi3lqg03!wgz",
				"Mozilla/5.0 (X11; CrOS i686 1193.158.0) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.75 Safari/535.7",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.75 Safari/535.7",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.75 Safari/535.7",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.7xs5D9rRDFpg2g",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.8 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.8",
				"Mozilla/5.0 (Windows NT 5.2; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.7",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.36 Safari/535.7",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.36 Safari/535.7",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.36 Safari/535.7",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.6 (KHTML, like Gecko) Chrome/16.0.897.0 Safari/535.6",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.54 Safari/535.2",
				"Mozilla/5.0 (X11; FreeBSD i386) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.2 (KHTML, like Gecko) Ubuntu/11.10 Chromium/15.0.874.120 Chrome/15.0.874.120 Safari/535.2",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.120 Safari/535.2",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.872.0 Safari/535.2",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.2 (KHTML, like Gecko) Ubuntu/11.04 Chromium/15.0.871.0 Chrome/15.0.871.0 Safari/535.2",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.864.0 Safari/535.2",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.861.0 Safari/535.2",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.861.0 Safari/535.2",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.861.0 Safari/535.2",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.860.0 Safari/535.2",
				"Chrome/15.0.860.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/15.0.860.0",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.186 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.834.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/11.04 Chromium/14.0.825.0 Chrome/14.0.825.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.824.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.815.10913 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.815.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/11.04 Chromium/14.0.814.0 Chrome/14.0.814.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.814.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/10.04 Chromium/14.0.813.0 Chrome/14.0.813.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.813.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.813.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.813.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.813.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.812.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.811.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.810.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.810.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.809.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/10.10 Chromium/14.0.808.0 Chrome/14.0.808.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/10.04 Chromium/14.0.808.0 Chrome/14.0.808.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/10.04 Chromium/14.0.804.0 Chrome/14.0.804.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.803.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/11.04 Chromium/14.0.803.0 Chrome/14.0.803.0 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.803.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.803.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.803.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_8) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.803.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.801.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_8) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.801.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.794.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.794.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.792.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.792.0 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.792.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; PPC Mac OS X 10_6_7) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.790.0 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.790.0 Safari/535.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1) AppleWebKit/526.3 (KHTML, like Gecko) Chrome/14.0.564.21 Safari/526.3",
				"Mozilla/5.0 (X11; CrOS i686 13.587.48) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.43 Safari/535.1",
				"Mozilla/5.0 Slackware/13.37 (X11; U; Linux x86_64; en-US) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41",
				"Mozilla/5.0 ArchLinux (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Ubuntu/11.04 Chromium/13.0.782.41 Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.2; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_3) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.41 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_3) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.32 Safari/535.1",
				"Mozilla/5.0 (X11; Linux amd64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.24 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.24 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.24 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.220 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.220 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.220 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.215 Safari/535.1",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.215 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.215 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.215 Safari/535.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.20 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.20 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.20 Safari/535.1",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.20 Safari/535.1",
				"Mozilla/5.0 (X11; CrOS i686 0.13.587) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.14 Safari/535.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.107 Safari/535.1",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.107 Safari/535.1",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.1 Safari/535.1",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.36 (KHTML, like Gecko) Chrome/13.0.766.0 Safari/534.36",
				"Mozilla/5.0 (X11; Linux amd64) AppleWebKit/534.36 (KHTML, like Gecko) Chrome/13.0.766.0 Safari/534.36",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.35 (KHTML, like Gecko) Ubuntu/10.10 Chromium/13.0.764.0 Chrome/13.0.764.0 Safari/534.35",
				"Mozilla/5.0 (X11; CrOS i686 0.13.507) AppleWebKit/534.35 (KHTML, like Gecko) Chrome/13.0.763.0 Safari/534.35",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.33 (KHTML, like Gecko) Ubuntu/9.10 Chromium/13.0.752.0 Chrome/13.0.752.0 Safari/534.33",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_8) AppleWebKit/534.31 (KHTML, like Gecko) Chrome/13.0.748.0 Safari/534.31",
				"Mozilla/5.0 (Windows NT 6.1; en-US) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.750.0 Safari/534.30",
				"Mozilla/5.0 (X11; CrOS i686 12.433.109) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.93 Safari/534.30",
				"Mozilla/5.0 (X11; CrOS i686 12.0.742.91) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.93 Safari/534.30",
				"Mozilla/5.0 Slackware/13.37 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/12.0.742.91",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.91 Chromium/12.0.742.91 Safari/534.30",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.68 Safari/534.30",
				"Mozilla/5.0 ArchLinux (X11; U; Linux x86_64; en-US) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.60 Safari/534.30",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.53 Safari/534.30",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.113 Safari/534.30",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/11.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/11.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (Windows NT 7.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (Windows 8) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_6) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_4) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.112 Safari/534.30",
				"Mozilla/5.0 (X11; CrOS i686 12.433.216) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.105 Safari/534.30",
				"Mozilla/5.0 ArchLinux (X11; U; Linux x86_64; en-US) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30",
				"Mozilla/5.0 ArchLinux (X11; U; Linux x86_64; en-US) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Slackware/Chrome/12.0.742.100 Safari/534.30",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_4) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.724.100 Safari/534.30",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.25 (KHTML, like Gecko) Chrome/12.0.706.0 Safari/534.25",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.25 (KHTML, like Gecko) Chrome/12.0.704.0 Safari/534.25",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.703.0 Chrome/12.0.703.0 Safari/534.24",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.24 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.702.0 Chrome/12.0.702.0 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/12.0.702.0 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/12.0.702.0 Safari/534.24",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.700.3 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.699.0 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.699.0 Safari/534.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_6) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.698.0 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.697.0 Safari/534.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.68 Safari/534.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.68 Safari/534.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_8) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.68 Safari/534.24",
				"Mozilla/5.0 Slackware/13.37 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/11.0.696.50",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.43 Safari/534.24",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.34 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.34 Safari/534.24",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.3 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.3 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.0) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.3 Safari/534.24",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.14 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.12 Safari/534.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_6) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.12 Safari/534.24",
				"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko) Ubuntu/10.04 Chromium/11.0.696.0 Chrome/11.0.696.0 Safari/534.24",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.0 Safari/534.24",
				"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.694.0 Safari/534.24",
				"Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.23 (KHTML, like Gecko) Chrome/11.0.686.3 Safari/534.23",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.21 (KHTML, like Gecko) Chrome/11.0.682.0 Safari/534.21",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.21 (KHTML, like Gecko) Chrome/11.0.678.0 Safari/534.21",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_7_0; en-US) AppleWebKit/534.21 (KHTML, like Gecko) Chrome/11.0.678.0 Safari/534.21",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20",
				"Mozilla/5.0 (Windows NT) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.669.0 Safari/534.20",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.19 (KHTML, like Gecko) Chrome/11.0.661.0 Safari/534.19",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.18 (KHTML, like Gecko) Chrome/11.0.661.0 Safari/534.18",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; en-US) AppleWebKit/534.18 (KHTML, like Gecko) Chrome/11.0.660.0 Safari/534.18",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/11.0.655.0 Safari/534.17",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/11.0.655.0 Safari/534.17",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/11.0.654.0 Safari/534.17",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/11.0.652.0 Safari/534.17",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/10.0.649.0 Safari/534.17",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; de-DE) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/10.0.649.0 Safari/534.17",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.82 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux armv7l; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204 Safari/534.16",
				"Mozilla/5.0 (X11; U; FreeBSD x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204 Safari/534.16",
				"Mozilla/5.0 (X11; U; FreeBSD i386; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_5; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.204",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.134 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.134 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.134 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.134 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.648.133 Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.648.133 Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_2; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.648.127 Chrome/10.0.648.127 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.127 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.127 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.127 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.11 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru-RU; AppleWebKit/534.16; KHTML; like Gecko; Chrome/10.0.648.11;Safari/534.16)",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; ru-RU) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.11 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.11 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.648.0 Chrome/10.0.648.0 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.648.0 Chrome/10.0.648.0 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.0 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.642.0 Chrome/10.0.642.0 Safari/534.16",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_5; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.639.0 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.638.0 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux i686 (x86_64); en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.634.0 Safari/534.16",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.634.0 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.16 SUSE/10.0.626.0 (KHTML, like Gecko) Chrome/10.0.626.0 Safari/534.16",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Chrome/10.0.613.0 Safari/534.15",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.613.0 Chrome/10.0.613.0 Safari/534.15",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Ubuntu/10.04 Chromium/10.0.612.3 Chrome/10.0.612.3 Safari/534.15",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Chrome/10.0.612.1 Safari/534.15",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.611.0 Chrome/10.0.611.0 Safari/534.15",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/10.0.602.0 Safari/534.14",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/10.0.601.0 Safari/534.14",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/10.0.601.0 Safari/534.14",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/540.0 (KHTML,like Gecko) Chrome/9.1.0.0 Safari/540.0",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/540.0 (KHTML, like Gecko) Ubuntu/10.10 Chrome/9.1.0.0 Safari/540.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.601.0 Safari/534.14",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Ubuntu/10.10 Chromium/9.0.600.0 Chrome/9.0.600.0 Safari/534.14",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.600.0 Safari/534.14",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.599.0 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-CA) AppleWebKit/534.13 (KHTML like Gecko) Chrome/9.0.597.98 Safari/534.13",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.84 Safari/534.13",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.44 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.19 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.15 Safari/534.13",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_5; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.15 Safari/534.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.107 Safari/534.13 v1333515017.9196",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US)  AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_5; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.596.0 Safari/534.13",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Ubuntu/10.04 Chromium/9.0.595.0 Chrome/9.0.595.0 Safari/534.13",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Ubuntu/9.10 Chromium/9.0.592.0 Chrome/9.0.592.0 Safari/534.13",
				"Mozilla/5.0 (X11; U; Windows NT 6; en-US) AppleWebKit/534.12 (KHTML, like Gecko) Chrome/9.0.587.0 Safari/534.12",
				"Mozilla/5.0 (Windows  U  Windows NT 5.1  en-US) AppleWebKit/534.12 (KHTML, like Gecko) Chrome/9.0.583.0 Safari/534.12",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.12 (KHTML, like Gecko) Chrome/9.0.579.0 Safari/534.12",
				"Mozilla/5.0 (X11; U; Linux i686 (x86_64); en-US) AppleWebKit/534.12 (KHTML, like Gecko) Chrome/9.0.576.0 Safari/534.12",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/540.0 (KHTML, like Gecko) Ubuntu/10.10 Chrome/8.1.0.0 Safari/540.0",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.558.0 Safari/534.10",
				"Mozilla/5.0 (X11; U; CrOS i686 0.9.130; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.344 Safari/534.10",
				"Mozilla/5.0 (X11; U; CrOS i686 0.9.128; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.343 Safari/534.10",
				"Mozilla/5.0 (X11; U; CrOS i686 0.9.128; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.341 Safari/534.10",
				"Mozilla/5.0 (X11; U; CrOS i686 0.9.128; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.339 Safari/534.10",
				"Mozilla/5.0 (X11; U; CrOS i686 0.9.128; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.339",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Ubuntu/10.10 Chromium/8.0.552.237 Chrome/8.0.552.237 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; de-DE) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/533.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.215 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.215 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.215 Safari/534.10",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.210 Safari/534.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.200 Safari/534.10",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.551.0 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.548.0 Safari/534.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.544.0 Safari/534.10",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:1.9.1.15) Gecko/20101027 Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.540.0 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.540.0 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; de-DE) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.540.0 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.540.0 Safari/534.10",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.9 (KHTML, like Gecko) Chrome/7.0.531.0 Safari/534.9",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.8 (KHTML, like Gecko) Chrome/7.0.521.0 Safari/534.8",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.517.24 Safari/534.7",
				"Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.514.0 Safari/534.7",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.514.0 Safari/534.7",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.514.0 Safari/534.7",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.500.0 Safari/534.6",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.498.0 Safari/534.6",
				"Mozilla/5.0 (ipad Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.6 (KHTML, like Gecko) Chrome/7.0.498.0 Safari/534.6",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/7.0.0 Safari/700.13",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.4 (KHTML, like Gecko) Chrome/6.0.481.0 Safari/534.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.472.63 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.472.53 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.472.33 Safari/534.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.470.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.464.0 Safari/534.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.464.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.463.0 Safari/534.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.462.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.462.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.461.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.461.0 Safari/534.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.461.0 Safari/534.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.460.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.460.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.460.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.459.0 Safari/534.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.1 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.1 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.1 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.1 Safari/534.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.1 Safari/534.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.458.0 Safari/534.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.457.0 Safari/534.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.456.0 Safari/534.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.2 (KHTML, like Gecko) Chrome/6.0.454.0 Safari/534.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/534.2 (KHTML, like Gecko) Chrome/6.0.454.0 Safari/534.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.2 (KHTML, like Gecko) Chrome/6.0.453.1 Safari/534.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/534.2 (KHTML, like Gecko) Chrome/6.0.453.1 Safari/534.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/534.2 (KHTML, like Gecko) Chrome/6.0.453.1 Safari/534.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.2 (KHTML, like Gecko) Chrome/6.0.451.0 Safari/534.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.1 SUSE/6.0.428.0 (KHTML, like Gecko) Chrome/6.0.428.0 Safari/534.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.428.0 Safari/534.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.428.0 Safari/534.1",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.428.0 Safari/534.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.427.0 Safari/534.1",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.422.0 Safari/534.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.417.0 Safari/534.1",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.416.0 Safari/534.1",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/534.1 (KHTML, like Gecko) Chrome/6.0.414.0 Safari/534.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.9 (KHTML, like Gecko) Chrome/6.0.400.0 Safari/533.9",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.8 (KHTML, like Gecko) Chrome/6.0.397.0 Safari/533.8",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/6.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.999 Safari/533.4",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_2; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_0; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_6; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.86 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.86 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_0; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.86 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_2; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.127 Safari/533.4",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.126 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; fr-FR) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.126 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.125 Safari/533.4",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.370.0 Safari/533.4",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.368.0 Safari/533.4",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.366.2 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.366.0 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_2; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.366.0 Safari/533.4",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.363.0 Safari/533.3",
				"Mozilla/5.0 (X11; U; OpenBSD i386; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.359.0 Safari/533.3",
				"Mozilla/5.0 (X11; U; x86_64 Linux; en_GB, en_US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.358.0 Safari/533.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.358.0 Safari/533.3",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.358.0 Safari/533.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.357.0 Safari/533.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.356.0 Safari/533.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.355.0 Safari/533.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.354.0 Safari/533.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.354.0 Safari/533.3",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.353.0 Safari/533.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/5.0.353.0 Safari/533.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_2; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.343.0 Safari/533.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.343.0 Safari/533.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_7_0; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.7 Safari/533.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_4; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.7 Safari/533.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.5 Safari/533.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.3 Safari/533.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.3 Safari/533.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.2 Safari/533.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.1 Safari/533.2",
				"Mozilla/5.0 (X11; U; Linux i586; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.1 Safari/533.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.1 Safari/533.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/533.1 (KHTML, like Gecko) Chrome/5.0.335.0 Safari/533.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/533.16 (KHTML, like Gecko) Chrome/5.0.335.0 Safari/533.16",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.310.0 Safari/532.9",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.309.0 Safari/532.9",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.308.0 Safari/532.9",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_0; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.307.11 Safari/532.9",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.307.1 Safari/532.9",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.1.249.1025 Safari/532.5",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/532.8 (KHTML, like Gecko) Chrome/4.0.302.2 Safari/532.8",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.8 (KHTML, like Gecko) Chrome/4.0.288.1 Safari/532.8",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.8 (KHTML, like Gecko) Chrome/4.0.277.0 Safari/532.8",
				"Mozilla/5.0 (X11; U; Slackware Linux x86_64; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.30 Safari/532.5",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; it-IT) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.25 Safari/532.5",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.0 Safari/532.5",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_8; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.0 Safari/532.5",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.246.0 Safari/532.5",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.4 (KHTML, like Gecko) Chrome/4.0.241.0 Safari/532.4",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.4 (KHTML, like Gecko) Chrome/4.0.237.0 Safari/532.4 Debian",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.3 (KHTML, like Gecko) Chrome/4.0.227.0 Safari/532.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.3 (KHTML, like Gecko) Chrome/4.0.224.2 Safari/532.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.3 (KHTML, like Gecko) Chrome/4.0.223.5 Safari/532.3",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.4 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.3 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; de-DE) Chrome/4.0.223.3 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.2 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.2 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.2 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.2 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.1 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.1 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.1 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.223.0 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.8 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.7 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.6 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.6 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.6 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.5 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.5 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.5 Safari/532.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.5 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.4 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.4 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.4 Safari/532.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.4 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.3 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.3 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.3 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.2 Safari/532.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.2 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.12 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.12 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.12 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.1 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.0 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.8 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686 (x86_64); en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.8 Safari/532.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.8 Safari/532.2",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_8; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.8 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.7 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.6 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.6 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.6 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.3 Safari/532.2",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.221.0 Safari/532.2",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.220.1 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.6 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.5 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.5 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.4 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.3 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.3 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.3 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.0 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.1 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.1 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.1 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.1 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.1 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.1 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.0 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.0 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.0 Safari/532.1",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.213.0 Safari/532.1",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_0; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.212.1 Safari/532.1",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_7; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.212.1 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.0",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.1",
				"Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.0",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.212.0 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.7 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.7 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.4 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.4 Safari/532.0",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.4 Safari/532.0",
				"Mozilla/5.0 (X11; U; Linux x86_64; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.2 Safari/532.0"];

var safariUserAgents = [
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/419 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/418.9 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/418.9.1 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fi-fi) AppleWebKit/418.8 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es-es) AppleWebKit/418.8 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es) AppleWebKit/419 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en_CA) AppleWebKit/419 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/419 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/418.9 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/418.8 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/419 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/418.9.1 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/418.9 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; tr-tr) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; sv-se) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; sv-se) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nl-nl) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nl-nl) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.9.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nl-nl) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nb-no) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nb-no) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.9.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/417.9 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.9.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/417.9 (KHTML, like Gecko) Safari/417.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/418 (KHTML, like Gecko) Safari/417.9.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nl-nl) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nl-nl) AppleWebKit/416.11 (KHTML, like Gecko) Safari/416.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nl-nl) AppleWebKit/416.11 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; nb-no) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ja-jp) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/416.11 (KHTML, like Gecko) Safari/416.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/416.12 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/416.11 (KHTML, like Gecko) Safari/416.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/416.11 (KHTML, like Gecko) Safari/416.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-ca) AppleWebKit/416.11 (KHTML, like Gecko) Safari/416.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/416.11 (KHTML, like Gecko) Safari/416.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/416.11 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/416.12 (KHTML, like Gecko) Safari/416.13",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ja-jp) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412.7 (KHTML, like Gecko) Safari/412.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS; pl-pl) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS; en-en) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/412.6 (KHTML, like Gecko) Safari/412.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/412.6 (KHTML, like Gecko) Safari/412.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es-ES) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en_US) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/412.6 (KHTML, like Gecko) Safari/412.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/412 (KHTML, like Gecko) Safari/412 Privoxy/3.0",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/412.6.2 (KHTML, like Gecko) Safari/412.2.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/412.6.2 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/412.6 (KHTML, like Gecko) Safari/412.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412.6.2 (KHTML, like Gecko) Safari/412.2.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412.6 (KHTML, like Gecko) Safari/412.2_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412.6 (KHTML, like Gecko) Safari/412.2",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412.6 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/412 (KHTML, like Gecko) Safari/412",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; sv-se) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.8.1 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.8.1 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.8 (KHTML, like Gecko) Safari/312.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; sv-se) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; sv-se) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ja-jp) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.5 (KHTML, like Gecko) Safari/312.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/312.5 (KHTML, like Gecko) Safari/312.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es-es) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.5 (KHTML, like Gecko) Safari/312.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/125",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.5.1 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.5 (KHTML, like Gecko) Safari/312.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.5.2 (KHTML, like Gecko) Safari/312.3.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.1.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/312.1 (KHTML, like Gecko) Safari/125",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-ch) AppleWebKit/312.1.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-ca) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/312.1 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.1.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.1.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312.3.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-ch) AppleWebKit/312.1 (KHTML, like Gecko) Safari/312",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/125.5.6 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.11",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-ch) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-ch) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.11",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/125.5.7 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/125.5.6 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.11",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5.7 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5.6 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.5.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.11",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.5.7 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.5.6 (KHTML, like Gecko) Safari/125.12_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.5.6 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.12_Adobe",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.5.5 (KHTML, like Gecko) Safari/125.12",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ja-jp) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/125.5 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en_CA) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-au) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.5 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.4 (KHTML, like Gecko) Safari/100",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.4 (KHTML, like Gecko) Safari/125.9",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; es-es) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.7",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-gb) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.2 (KHTML, like Gecko) Safari/85.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.7",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en)  AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/125.2 (KHTML, like Gecko) Safari/125.7",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; it-it) AppleWebKit/124 (KHTML, like Gecko) Safari/125.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/124 (KHTML, like Gecko) Safari/125",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/124 (KHTML, like Gecko) Safari/125",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/124 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/124 (KHTML, like Gecko) Safari/125.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/124 (KHTML, like Gecko) Safari/125",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/85.8.2 (KHTML, like Gecko) Safari/85.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-gb) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/85.8.2 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85.8.1",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/85.8.5 (KHTML, like Gecko) Safari/85",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/85.8.2 (KHTML, like Gecko) Safari/85.8",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; sv-se) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ja-jp) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr-fr) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fr) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en-us) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.5",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.7",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/85.7 (KHTML, like Gecko) Safari/85.5",
				"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.3) Gecko/2008092816 Mobile Safari 1.1.3",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN) AppleWebKit/533+ (KHTML, like Gecko)",
				"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/528.8 (KHTML, like Gecko)",
				"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.34 (KHTML, like Gecko) Dooble/1.40 Safari/534.34",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; fi-fi) AppleWebKit/420+ (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/312.8.1 (KHTML, like Gecko) Safari/312.6",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/419.2 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-de) AppleWebKit/418.9.1 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-ch) AppleWebKit/85 (KHTML, like Gecko) Safari/85",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; de-CH) AppleWebKit/419.2 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; da-dk) AppleWebKit/522+ (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; PPC Mac OS X 10_5_6; en-us) AppleWebKit/528.16 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; it-IT) AppleWebKit/521.25 (KHTML, like Gecko) Safari/521.24",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; en-us) AppleWebKit/419.2.1 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; en) AppleWebKit/522.11.1 (KHTML, like Gecko) Safari/419.3",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; en) AppleWebKit/521.32.1 (KHTML, like Gecko) Safari/521.32.1",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X; en) AppleWebKit (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; es-es) AppleWebKit/531.22.7 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_6; en-us) AppleWebKit/528.16 (KHTML, like Gecko)",
				"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_5_5; it-it) AppleWebKit/525.18 (KHTML, like Gecko)"];

var operaUserAgents = [
				"Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14",
				"Mozilla/5.0 (Windows NT 6.0; rv:2.0) Gecko/20100101 Firefox/4.0 Opera 12.14",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.0) Opera 12.14",
				"Opera/12.80 (Windows NT 5.1; U; en) Presto/2.10.289 Version/12.02",
				"Opera/9.80 (Windows NT 6.1; U; es-ES) Presto/2.9.181 Version/12.00",
				"Opera/9.80 (Windows NT 5.1; U; zh-sg) Presto/2.9.181 Version/12.00",
				"Opera/12.0(Windows NT 5.2;U;en)Presto/22.9.168 Version/12.00",
				"Opera/12.0(Windows NT 5.1;U;en)Presto/22.9.168 Version/12.00",
				"Mozilla/5.0 (Windows NT 5.1) Gecko/20100101 Firefox/14.0 Opera/12.0",
				"Opera/9.80 (Windows NT 6.1; WOW64; U; pt) Presto/2.10.229 Version/11.62",
				"Opera/9.80 (Windows NT 6.0; U; pl) Presto/2.10.229 Version/11.62",
				"Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; fr) Presto/2.9.168 Version/11.52",
				"Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; de) Presto/2.9.168 Version/11.52",
				"Opera/9.80 (Windows NT 5.1; U; en) Presto/2.9.168 Version/11.51",
				"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; de) Opera 11.51",
				"Opera/9.80 (X11; Linux x86_64; U; fr) Presto/2.9.168 Version/11.50",
				"Opera/9.80 (X11; Linux i686; U; hu) Presto/2.9.168 Version/11.50",
				"Opera/9.80 (X11; Linux i686; U; ru) Presto/2.8.131 Version/11.11",
				"Opera/9.80 (X11; Linux i686; U; es-ES) Presto/2.8.131 Version/11.11",
				"Mozilla/5.0 (Windows NT 5.1; U; en; rv:1.8.1) Gecko/20061208 Firefox/5.0 Opera 11.11",
				"Opera/9.80 (X11; Linux x86_64; U; bg) Presto/2.8.131 Version/11.10",
				"Opera/9.80 (Windows NT 6.0; U; en) Presto/2.8.99 Version/11.10",
				"Opera/9.80 (Windows NT 5.1; U; zh-tw) Presto/2.8.131 Version/11.10",
				"Opera/9.80 (Windows NT 6.1; Opera Tablet/15165; U; en) Presto/2.8.149 Version/11.1",
				"Opera/9.80 (X11; Linux x86_64; U; Ubuntu/10.10 (maverick); pl) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (X11; Linux i686; U; ja) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (X11; Linux i686; U; fr) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 6.1; U; zh-tw) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 6.1; U; sv) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 6.1; U; en-US) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 6.1; U; cs) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 6.0; U; pl) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 5.2; U; ru) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 5.1; U;) Presto/2.7.62 Version/11.01",
				"Opera/9.80 (Windows NT 5.1; U; cs) Presto/2.7.62 Version/11.01",
				"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101213 Opera/9.80 (Windows NT 6.1; U; zh-tw) Presto/2.7.62 Version/11.01",
				"Mozilla/5.0 (Windows NT 6.1; U; nl; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 11.01",
				"Mozilla/5.0 (Windows NT 6.1; U; de; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 11.01",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; de) Opera 11.01",
				"Opera/9.80 (X11; Linux x86_64; U; pl) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (X11; Linux i686; U; it) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.6.37 Version/11.00",
				"Opera/9.80 (Windows NT 6.1; U; pl) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 6.1; U; ko) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 6.1; U; fi) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 6.1; U; en-GB) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 6.1 x64; U; en) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 6.0; U; en) Presto/2.7.39 Version/11.00",
				"Opera/9.80 (Windows NT 5.1; U; ru) Presto/2.7.39 Version/11.00",
				"Opera/9.80 (Windows NT 5.1; U; MRA 5.5 (build 02842); ru) Presto/2.7.62 Version/11.00",
				"Opera/9.80 (Windows NT 5.1; U; it) Presto/2.7.62 Version/11.00",
				"Mozilla/5.0 (Windows NT 6.0; U; ja; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 11.00",
				"Mozilla/5.0 (Windows NT 5.1; U; pl; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 11.00",
				"Mozilla/5.0 (Windows NT 5.1; U; de; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 11.00",
				"Mozilla/4.0 (compatible; MSIE 8.0; X11; Linux x86_64; pl) Opera 11.00",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; fr) Opera 11.00",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; ja) Opera 11.00",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; en) Opera 11.00",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; pl) Opera 11.00",
				"Opera/9.80 (Windows NT 6.1; U; pl) Presto/2.6.31 Version/10.70",
				"Mozilla/5.0 (Windows NT 5.2; U; ru; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.70",
				"Mozilla/5.0 (Windows NT 5.1; U; zh-cn; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.70",
				"Opera/9.80 (Windows NT 5.2; U; zh-cn) Presto/2.6.30 Version/10.63",
				"Opera/9.80 (Windows NT 5.2; U; en) Presto/2.6.30 Version/10.63",
				"Opera/9.80 (Windows NT 5.1; U; MRA 5.6 (build 03278); ru) Presto/2.6.30 Version/10.63",
				"Opera/9.80 (Windows NT 5.1; U; pl) Presto/2.6.30 Version/10.62",
				"Mozilla/5.0 (X11; Linux x86_64; U; de; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.62",
				"Mozilla/4.0 (compatible; MSIE 8.0; X11; Linux x86_64; de) Opera 10.62",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; en) Opera 10.62",
				"Opera/9.80 (X11; Linux i686; U; pl) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (X11; Linux i686; U; es-ES) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (Windows NT 6.0; U; it) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (Windows NT 5.2; U; ru) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (Windows 98; U; de) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (Macintosh; Intel Mac OS X; U; nl) Presto/2.6.30 Version/10.61",
				"Opera/9.80 (X11; Linux i686; U; en) Presto/2.5.27 Version/10.60",
				"Opera/9.80 (Windows NT 6.0; U; nl) Presto/2.6.30 Version/10.60",
				"Opera/10.60 (Windows NT 5.1; U; zh-cn) Presto/2.6.30 Version/10.60",
				"Opera/10.60 (Windows NT 5.1; U; en-US) Presto/2.6.30 Version/10.60",
				"Opera/9.80 (X11; Linux i686; U; it) Presto/2.5.24 Version/10.54",
				"Opera/9.80 (X11; Linux i686; U; en-GB) Presto/2.5.24 Version/10.53",
				"Mozilla/5.0 (Windows NT 5.1; U; zh-cn; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.53",
				"Mozilla/5.0 (Windows NT 5.1; U; Firefox/5.0; en; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.53",
				"Mozilla/5.0 (Windows NT 5.1; U; Firefox/4.5; en; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.53",
				"Mozilla/5.0 (Windows NT 5.1; U; Firefox/3.5; en; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.53",
				"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; ko) Opera 10.53",
				"Opera/9.80 (Windows NT 6.1; U; fr) Presto/2.5.24 Version/10.52",
				"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.5.22 Version/10.51",
				"Opera/9.80 (Windows NT 6.0; U; cs) Presto/2.5.22 Version/10.51",
				"Opera/9.80 (Windows NT 5.2; U; ru) Presto/2.5.22 Version/10.51",
				"Opera/9.80 (Linux i686; U; en) Presto/2.5.22 Version/10.51",
				"Mozilla/5.0 (Windows NT 6.1; U; en-GB; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.51",
				"Mozilla/5.0 (Linux i686; U; en; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 Opera 10.51",
				"Mozilla/4.0 (compatible; MSIE 8.0; Linux i686; en) Opera 10.51",
				"Opera/9.80 (Windows NT 6.1; U; zh-tw) Presto/2.5.22 Version/10.50",
				"Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.5.22 Version/10.50",
				"Opera/9.80 (Windows NT 6.1; U; sk) Presto/2.6.22 Version/10.50",
				"Opera/9.80 (Windows NT 6.1; U; ja) Presto/2.5.22 Version/10.50",
				"Opera/9.80 (Windows NT 6.0; U; zh-cn) Presto/2.5.22 Version/10.50",
				"Opera/9.80 (Windows NT 5.1; U; sk) Presto/2.5.22 Version/10.50",
				"Opera/9.80 (Windows NT 5.1; U; ru) Presto/2.5.22 Version/10.50",
				"Opera/10.50 (Windows NT 6.1; U; en-GB) Presto/2.2.2",
				"Opera/9.80 (S60; SymbOS; Opera Tablet/9174; U; en) Presto/2.7.81 Version/10.5",
				"Opera/9.80 (X11; U; Linux i686; en-US; rv:1.9.2.3) Presto/2.2.15 Version/10.10",
				"Opera/9.80 (X11; Linux x86_64; U; it) Presto/2.2.15 Version/10.10",
				"Opera/9.80 (Windows NT 6.1; U; de) Presto/2.2.15 Version/10.10",
				"Opera/9.80 (Windows NT 6.0; U; Gecko/20100115; pl) Presto/2.2.15 Version/10.10",
				"Opera/9.80 (Windows NT 6.0; U; en) Presto/2.2.15 Version/10.10",
				"Opera/9.80 (Windows NT 5.1; U; de) Presto/2.2.15 Version/10.10",
				"Opera/9.80 (Windows NT 5.1; U; cs) Presto/2.2.15 Version/10.10",
				"Mozilla/5.0 (Windows NT 6.0; U; tr; rv:1.8.1) Gecko/20061208 Firefox/2.0.0 Opera 10.10",
				"Mozilla/4.0 (compatible; MSIE 6.0; X11; Linux i686; de) Opera 10.10",
				"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 6.0; tr) Opera 10.10",
				"Opera/9.80 (X11; Linux x86_64; U; en-GB) Presto/2.2.15 Version/10.01",
				"Opera/9.80 (X11; Linux x86_64; U; en) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux x86_64; U; de) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; ru) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; pt-BR) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; pl) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; nb) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; en-GB) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; en) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; Debian; pl) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (X11; Linux i686; U; de) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.1; U; zh-cn) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.1; U; fi) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.1; U; de) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.1; U; cs) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.0; U; en) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 6.0; U; de) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 5.2; U; en) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 5.1; U; zh-cn) Presto/2.2.15 Version/10.00",
				"Opera/9.80 (Windows NT 5.1; U; ru) Presto/2.2.15 Version/10.00",
				"Opera/9.99 (X11; U; sk)",
				"Opera/9.99 (Windows NT 5.1; U; pl) Presto/9.9.9",
				"Opera/9.80 (J2ME/MIDP; Opera Mini/5.0 (Windows; U; Windows NT 5.1; en) AppleWebKit/886; U; en) Presto/2.4.15",
				"Opera/9.70 (Linux ppc64 ; U; en) Presto/2.2.1",
				"Opera/9.70 (Linux i686 ; U; zh-cn) Presto/2.2.0",
				"Opera/9.70 (Linux i686 ; U; en-us) Presto/2.2.0",
				"Opera/9.70 (Linux i686 ; U; en) Presto/2.2.1",
				"Opera/9.70 (Linux i686 ; U; en) Presto/2.2.0",
				"Opera/9.70 (Linux i686 ; U; ; en) Presto/2.2.1",
				"Opera/9.70 (Linux i686 ; U;  ; en) Presto/2.2.1",
				"Mozilla/5.0 (Linux i686 ; U; en; rv:1.8.1) Gecko/20061208 Firefox/2.0.0 Opera 9.70",
				"Mozilla/4.0 (compatible; MSIE 6.0; Linux i686 ; en) Opera 9.70",
				"HTC_HD2_T8585 Opera/9.70 (Windows NT 5.1; U; de)",
				"Opera 9.7 (Windows NT 5.2; U; en)",
				"Opera/9.64(Windows NT 5.1; U; en) Presto/2.1.1",
				"Opera/9.64 (X11; Linux x86_64; U; pl) Presto/2.1.1",
				"Opera/9.64 (X11; Linux x86_64; U; hr) Presto/2.1.1",
				"Opera/9.64 (X11; Linux x86_64; U; en-GB) Presto/2.1.1",
				"Opera/9.64 (X11; Linux x86_64; U; en) Presto/2.1.1",
				"Opera/9.64 (X11; Linux x86_64; U; de) Presto/2.1.1",
				"Opera/9.64 (X11; Linux x86_64; U; cs) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; tr) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; sv) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; pl) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; nb) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; Linux Mint; nb) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; Linux Mint; it) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; en) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; de) Presto/2.1.1",
				"Opera/9.64 (X11; Linux i686; U; da) Presto/2.1.1",
				"Opera/9.64 (Windows NT 6.1; U; MRA 5.5 (build 02842); ru) Presto/2.1.1",
				"Opera/9.64 (Windows NT 6.1; U; de) Presto/2.1.1",
				"Opera/9.64 (Windows NT 6.0; U; zh-cn) Presto/2.1.1",
				"Opera/9.64 (Windows NT 6.0; U; pl) Presto/2.1.1",
				"Opera/9.63 (X11; Linux x86_64; U; ru) Presto/2.1.1",
                "Opera/9.63 (X11; Linux x86_64; U; cs) Presto/2.1.1"];
    
   
   const words = [
    "acrobat",
    "africa",
    "alaska",
    "albert",
    "albino",
    "album",
    "alcohol",
    "alex",
    "alpha",
    "amadeus",
    "amanda",
    "amazon",
    "america",
    "analog",
    "animal",
    "antenna",
    "antonio",
    "apollo",
    "april",
    "aroma",
    "artist",
    "aspirin",
    "athlete",
    "atlas",
    "banana",
    "bandit",
    "banjo",
    "bikini",
    "bingo",
    "bonus",
    "camera",
    "canada",
    "carbon",
    "casino",
    "catalog",
    "cinema",
    "citizen",
    "cobra",
    "comet",
    "compact",
    "complex",
    "context",
    "credit",
    "critic",
    "crystal",
    "culture",
    "david",
    "delta",
    "dialog",
    "diploma",
    "doctor",
    "domino",
    "dragon",
    "drama",
    "extra",
    "fabric",
    "final",
    "focus",
    "forum",
    "galaxy",
    "gallery",
    "global",
    "harmony",
    "hotel",
    "humor",
    "index",
    "japan",
    "kilo",
    "lemon",
    "liter",
    "lotus",
    "mango",
    "melon",
    "menu",
    "meter",
    "metro",
    "mineral",
    "model",
    "music",
    "object",
    "piano",
    "pirate",
    "plastic",
    "radio",
    "report",
    "signal",
    "sport",
    "studio",
    "subject",
    "super",
    "tango",
    "taxi",
    "tempo",
    "tennis",
    "textile",
    "tokyo",
    "total",
    "tourist",
    "video",
    "visa",
    "academy",
    "alfred",
    "atlanta",
    "atomic",
    "barbara",
    "bazaar",
    "brother",
    "budget",
    "cabaret",
    "cadet",
    "candle",
    "capsule",
    "caviar",
    "channel",
    "chapter",
    "circle",
    "cobalt",
    "comrade",
    "condor",
    "crimson",
    "cyclone",
    "darwin",
    "declare",
    "denver",
    "desert",
    "divide",
    "dolby",
    "domain",
    "double",
    "eagle",
    "echo",
    "eclipse",
    "editor",
    "educate",
    "edward",
    "effect",
    "electra",
    "emerald",
    "emotion",
    "empire",
    "eternal",
    "evening",
    "exhibit",
    "expand",
    "explore",
    "extreme",
    "ferrari",
    "forget",
    "freedom",
    "friday",
    "fuji",
    "galileo",
    "genesis",
    "gravity",
    "habitat",
    "hamlet",
    "harlem",
    "helium",
    "holiday",
    "hunter",
    "ibiza",
    "iceberg",
    "imagine",
    "infant",
    "isotope",
    "jackson",
    "jamaica",
    "jasmine",
    "java",
    "jessica",
    "kitchen",
    "lazarus",
    "letter",
    "license",
    "lithium",
    "loyal",
    "lucky",
    "magenta",
    "manual",
    "marble",
    "maxwell",
    "mayor",
    "monarch",
    "monday",
    "money",
    "morning",
    "mother",
    "mystery",
    "native",
    "nectar",
    "nelson",
    "network",
    "nikita",
    "nobel",
    "nobody",
    "nominal",
    "norway",
    "nothing",
    "number",
    "october",
    "office",
    "oliver",
    "opinion",
    "option",
    "order",
    "outside",
    "package",
    "pandora",
    "panther",
    "papa",
    "pattern",
    "pedro",
    "pencil",
    "people",
    "phantom",
    "philips",
    "pioneer",
    "pluto",
    "podium",
    "portal",
    "potato",
    "process",
    "proxy",
    "pupil",
    "python",
    "quality",
    "quarter",
    "quiet",
    "rabbit",
    "radical",
    "radius",
    "rainbow",
    "ramirez",
    "ravioli",
    "raymond",
    "respect",
    "respond",
    "result",
    "resume",
    "richard",
    "river",
    "roger",
    "roman",
    "rondo",
    "sabrina",
    "salary",
    "salsa",
    "sample",
    "samuel",
    "saturn",
    "savage",
    "scarlet",
    "scorpio",
    "sector",
    "serpent",
    "shampoo",
    "sharon",
    "silence",
    "simple",
    "society",
    "sonar",
    "sonata",
    "soprano",
    "sparta",
    "spider",
    "sponsor",
    "abraham",
    "action",
    "active",
    "actor",
    "adam",
    "address",
    "admiral",
    "adrian",
    "agenda",
    "agent",
    "airline",
    "airport",
    "alabama",
    "aladdin",
    "alarm",
    "algebra",
    "alibi",
    "alice",
    "alien",
    "almond",
    "alpine",
    "amber",
    "amigo",
    "ammonia",
    "analyze",
    "anatomy",
    "angel",
    "annual",
    "answer",
    "apple",
    "archive",
    "arctic",
    "arena",
    "arizona",
    "armada",
    "arnold",
    "arsenal",
    "arthur",
    "asia",
    "aspect",
    "athena",
    "audio",
    "august",
    "austria",
    "avenue",
    "average",
    "axiom",
    "aztec",
    "bagel",
    "baker",
    "balance",
    "ballad",
    "ballet",
    "bambino",
    "bamboo",
    "baron",
    "basic",
    "basket",
    "battery",
    "belgium",
    "benefit",
    "berlin",
    "bermuda",
    "bernard",
    "bicycle",
    "binary",
    "biology",
    "bishop",
    "blitz",
    "block",
    "blonde",
    "bonjour",
    "boris",
    "boston",
    "bottle",
    "boxer",
    "brandy",
    "bravo",
    "brazil",
    "bridge",
    "british",
    "bronze",
    "brown",
    "bruce",
    "bruno",
    "brush",
    "burger",
    "burma",
    "cabinet",
    "cactus",
    "cafe",
    "cairo",
    "calypso",
    "camel",
    "campus",
    "canal",
    "cannon",
    "canoe",
    "cantina",
    "canvas",
    "canyon",
    "capital",
    "caramel",
    "caravan",
    "career",
    "cargo",
    "carlo",
    "carol",
    "carpet",
    "cartel",
    "cartoon",
    "castle",
    "castro",
    "cecilia",
    "cement",
    "center",
    "century",
    "ceramic",
    "chamber",
    "chance",
    "change",
    "chaos",
    "charlie",
    "charm",
    "charter",
    "cheese",
    "chef",
    "chemist",
    "cherry",
    "chess",
    "chicago",
    "chicken",
    "chief",
    "china",
    "cigar",
    "circus",
    "city",
    "clara",
    "classic",
    "claudia",
    "clean",
    "client",
    "climax",
    "clinic",
    "clock",
    "club",
    "cockpit",
    "coconut",
    "cola",
    "collect",
    "colombo",
    "colony",
    "color",
    "combat",
    "comedy",
    "command",
    "company",
    "concert",
    "connect",
    "consul",
    "contact",
    "contour",
    "control",
    "convert",
    "copy",
    "corner",
    "corona",
    "correct",
    "cosmos",
    "couple",
    "courage",
    "cowboy",
    "craft",
    "crash",
    "cricket",
    "crown",
    "cuba",
    "dallas",
    "dance",
    "daniel",
    "decade",
    "decimal",
    "degree",
    "delete",
    "deliver",
    "delphi",
    "deluxe",
    "demand",
    "demo",
    "denmark",
    "derby",
    "design",
    "detect",
    "develop",
    "diagram",
    "diamond",
    "diana",
    "diego",
    "diesel",
    "diet",
    "digital",
    "dilemma",
    "direct",
    "disco",
    "disney",
    "distant",
    "dollar",
    "dolphin",
    "donald",
    "drink",
    "driver",
    "dublin",
    "duet",
    "dynamic",
    "earth",
    "east",
    "ecology",
    "economy",
    "edgar",
    "egypt",
    "elastic",
    "elegant",
    "element",
    "elite",
    "elvis",
    "email",
    "empty",
    "energy",
    "engine",
    "english",
    "episode",
    "equator",
    "escape",
    "escort",
    "ethnic",
    "europe",
    "everest",
    "evident",
    "exact",
    "example",
    "exit",
    "exotic",
    "export",
    "express",
    "factor",
    "falcon",
    "family",
    "fantasy",
    "fashion",
    "fiber",
    "fiction",
    "fidel",
    "fiesta",
    "figure",
    "film",
    "filter",
    "finance",
    "finish",
    "finland",
    "first",
    "flag",
    "flash",
    "florida",
    "flower",
    "fluid",
    "flute",
    "folio",
    "ford",
    "forest",
    "formal",
    "formula",
    "fortune",
    "forward",
    "fragile",
    "france",
    "frank",
    "fresh",
    "friend",
    "frozen",
    "future",
    "gabriel",
    "gamma",
    "garage",
    "garcia",
    "garden",
    "garlic",
    "gemini",
    "general",
    "genetic",
    "genius",
    "germany",
    "gloria",
    "gold",
    "golf",
    "gondola",
    "gong",
    "good",
    "gordon",
    "gorilla",
    "grand",
    "granite",
    "graph",
    "green",
    "group",
    "guide",
    "guitar",
    "guru",
    "hand",
    "happy",
    "harbor",
    "harvard",
    "havana",
    "hawaii",
    "helena",
    "hello",
    "henry",
    "hilton",
    "history",
    "horizon",
    "house",
    "human",
    "icon",
    "idea",
    "igloo",
    "igor",
    "image",
    "impact",
    "import",
    "india",
    "indigo",
    "input",
    "insect",
    "instant",
    "iris",
    "italian",
    "jacket",
    "jacob",
    "jaguar",
    "janet",
    "jargon",
    "jazz",
    "jeep",
    "john",
    "joker",
    "jordan",
    "judo",
    "jumbo",
    "june",
    "jungle",
    "junior",
    "jupiter",
    "karate",
    "karma",
    "kayak",
    "kermit",
    "king",
    "koala",
    "korea",
    "labor",
    "lady",
    "lagoon",
    "laptop",
    "laser",
    "latin",
    "lava",
    "lecture",
    "left",
    "legal",
    "level",
    "lexicon",
    "liberal",
    "libra",
    "lily",
    "limbo",
    "limit",
    "linda",
    "linear",
    "lion",
    "liquid",
    "little",
    "llama",
    "lobby",
    "lobster",
    "local",
    "logic",
    "logo",
    "lola",
    "london",
    "lucas",
    "lunar",
    "machine",
    "macro",
    "madam",
    "madonna",
    "madrid",
    "maestro",
    "magic",
    "magnet",
    "magnum",
    "mailbox",
    "major",
    "mama",
    "mambo",
    "manager",
    "manila",
    "marco",
    "marina",
    "market",
    "mars",
    "martin",
    "marvin",
    "mary",
    "master",
    "matrix",
    "maximum",
    "media",
    "medical",
    "mega",
    "melody",
    "memo",
    "mental",
    "mentor",
    "mercury",
    "message",
    "metal",
    "meteor",
    "method",
    "mexico",
    "miami",
    "micro",
    "milk",
    "million",
    "minimum",
    "minus",
    "minute",
    "miracle",
    "mirage",
    "miranda",
    "mister",
    "mixer",
    "mobile",
    "modem",
    "modern",
    "modular",
    "moment",
    "monaco",
    "monica",
    "monitor",
    "mono",
    "monster",
    "montana",
    "morgan",
    "motel",
    "motif",
    "motor",
    "mozart",
    "multi",
    "museum",
    "mustang",
    "natural",
    "neon",
    "nepal",
    "neptune",
    "nerve",
    "neutral",
    "nevada",
    "news",
    "next",
    "ninja",
    "nirvana",
    "normal",
    "nova",
    "novel",
    "nuclear",
    "numeric",
    "nylon",
    "oasis",
    "observe",
    "ocean",
    "octopus",
    "olivia",
    "olympic",
    "omega",
    "opera",
    "optic",
    "optimal",
    "orange",
    "orbit",
    "organic",
    "orient",
    "origin",
    "orlando",
    "oscar",
    "oxford",
    "oxygen",
    "ozone",
    "pablo",
    "pacific",
    "pagoda",
    "palace",
    "pamela",
    "panama",
    "pancake",
    "panda",
    "panel",
    "panic",
    "paradox",
    "pardon",
    "paris",
    "parker",
    "parking",
    "parody",
    "partner",
    "passage",
    "passive",
    "pasta",
    "pastel",
    "patent",
    "patient",
    "patriot",
    "patrol",
    "pegasus",
    "pelican",
    "penguin",
    "pepper",
    "percent",
    "perfect",
    "perfume",
    "period",
    "permit",
    "person",
    "peru",
    "phone",
    "photo",
    "picasso",
    "picnic",
    "picture",
    "pigment",
    "pilgrim",
    "pilot",
    "pixel",
    "pizza",
    "planet",
    "plasma",
    "plaza",
    "pocket",
    "poem",
    "poetic",
    "poker",
    "polaris",
    "police",
    "politic",
    "polo",
    "polygon",
    "pony",
    "popcorn",
    "popular",
    "postage",
    "precise",
    "prefix",
    "premium",
    "present",
    "price",
    "prince",
    "printer",
    "prism",
    "private",
    "prize",
    "product",
    "profile",
    "program",
    "project",
    "protect",
    "proton",
    "public",
    "pulse",
    "puma",
    "pump",
    "pyramid",
    "queen",
    "radar",
    "ralph",
    "random",
    "rapid",
    "rebel",
    "record",
    "recycle",
    "reflex",
    "reform",
    "regard",
    "regular",
    "relax",
    "reptile",
    "reverse",
    "ricardo",
    "right",
    "ringo",
    "risk",
    "ritual",
    "robert",
    "robot",
    "rocket",
    "rodeo",
    "romeo",
    "royal",
    "russian",
    "safari",
    "salad",
    "salami",
    "salmon",
    "salon",
    "salute",
    "samba",
    "sandra",
    "santana",
    "sardine",
    "school",
    "scoop",
    "scratch",
    "screen",
    "script",
    "scroll",
    "second",
    "secret",
    "section",
    "segment",
    "select",
    "seminar",
    "senator",
    "senior",
    "sensor",
    "serial",
    "service",
    "shadow",
    "sharp",
    "sheriff",
    "shock",
    "short",
    "shrink",
    "sierra",
    "silicon",
    "silk",
    "silver",
    "similar",
    "simon",
    "single",
    "siren",
    "slang",
    "slogan",
    "smart",
    "smoke",
    "snake",
    "social",
    "soda",
    "solar",
    "solid",
    "solo",
    "sonic",
    "source",
    "soviet",
    "special",
    "speed",
    "sphere",
    "spiral",
    "spirit",
    "spring",
    "static",
    "status",
    "stereo",
    "stone",
    "stop",
    "street",
    "strong",
    "student",
    "style",
    "sultan",
    "susan",
    "sushi",
    "suzuki",
    "switch",
    "symbol",
    "system",
    "tactic",
    "tahiti",
    "talent",
    "tarzan",
    "telex",
    "texas",
    "theory",
    "thermos",
    "tiger",
    "titanic",
    "tomato",
    "topic",
    "tornado",
    "toronto",
    "torpedo",
    "totem",
    "tractor",
    "traffic",
    "transit",
    "trapeze",
    "travel",
    "tribal",
    "trick",
    "trident",
    "trilogy",
    "tripod",
    "tropic",
    "trumpet",
    "tulip",
    "tuna",
    "turbo",
    "twist",
    "ultra",
    "uniform",
    "union",
    "uranium",
    "vacuum",
    "valid",
    "vampire",
    "vanilla",
    "vatican",
    "velvet",
    "ventura",
    "venus",
    "vertigo",
    "veteran",
    "victor",
    "vienna",
    "viking",
    "village",
    "vincent",
    "violet",
    "violin",
    "virtual",
    "virus",
    "vision",
    "visitor",
    "visual",
    "vitamin",
    "viva",
    "vocal",
    "vodka",
    "volcano",
    "voltage",
    "volume",
    "voyage",
    "water",
    "weekend",
    "welcome",
    "western",
    "window",
    "winter",
    "wizard",
    "wolf",
    "world",
    "xray",
    "yankee",
    "yoga",
    "yogurt",
    "yoyo",
    "zebra",
    "zero",
    "zigzag",
    "zipper",
    "zodiac",
    "zoom",
    "acid",
    "adios",
    "agatha",
    "alamo",
    "alert",
    "almanac",
    "aloha",
    "andrea",
    "anita",
    "arcade",
    "aurora",
    "avalon",
    "baby",
    "baggage",
    "balloon",
    "bank",
    "basil",
    "begin",
    "biscuit",
    "blue",
    "bombay",
    "botanic",
    "brain",
    "brenda",
    "brigade",
    "cable",
    "calibre",
    "carmen",
    "cello",
    "celtic",
    "chariot",
    "chrome",
    "citrus",
    "civil",
    "cloud",
    "combine",
    "common",
    "cool",
    "copper",
    "coral",
    "crater",
    "cubic",
    "cupid",
    "cycle",
    "depend",
    "door",
    "dream",
    "dynasty",
    "edison",
    "edition",
    "enigma",
    "equal",
    "eric",
    "event",
    "evita",
    "exodus",
    "extend",
    "famous",
    "farmer",
    "food",
    "fossil",
    "frog",
    "fruit",
    "geneva",
    "gentle",
    "george",
    "giant",
    "gilbert",
    "gossip",
    "gram",
    "greek",
    "grille",
    "hammer",
    "harvest",
    "hazard",
    "heaven",
    "herbert",
    "heroic",
    "hexagon",
    "husband",
    "immune",
    "inca",
    "inch",
    "initial",
    "isabel",
    "ivory",
    "jason",
    "jerome",
    "joel",
    "joshua",
    "journal",
    "judge",
    "juliet",
    "jump",
    "justice",
    "kimono",
    "kinetic",
    "leonid",
    "leopard",
    "lima",
    "maze",
    "medusa",
    "member",
    "memphis",
    "michael",
    "miguel",
    "milan",
    "mile",
    "miller",
    "mimic",
    "mimosa",
    "mission",
    "monkey",
    "moral",
    "moses",
    "mouse",
    "nancy",
    "natasha",
    "nebula",
    "nickel",
    "nina",
    "noise",
    "orchid",
    "oregano",
    "origami",
    "orinoco",
    "orion",
    "othello",
    "paper",
    "paprika",
    "prelude",
    "prepare",
    "pretend",
    "promise",
    "prosper",
    "provide",
    "puzzle",
    "remote",
    "repair",
    "reply",
    "rival",
    "riviera",
    "robin",
    "rose",
    "rover",
    "rudolf",
    "saga",
    "sahara",
    "scholar",
    "shelter",
    "ship",
    "shoe",
    "sigma",
    "sister",
    "sleep",
    "smile",
    "spain",
    "spark",
    "split",
    "spray",
    "square",
    "stadium",
    "star",
    "storm",
    "story",
    "strange",
    "stretch",
    "stuart",
    "subway",
    "sugar",
    "sulfur",
    "summer",
    "survive",
    "sweet",
    "swim",
    "table",
    "taboo",
    "target",
    "teacher",
    "telecom",
    "temple",
    "tibet",
    "ticket",
    "tina",
    "today",
    "toga",
    "tommy",
    "tower",
    "trivial",
    "tunnel",
    "turtle",
    "twin",
    "uncle",
    "unicorn",
    "unique",
    "update",
    "valery",
    "vega",
    "version",
    "voodoo",
    "warning",
    "william",
    "wonder",
    "year",
    "yellow",
    "young",
    "absent",
    "absorb",
    "absurd",
    "accent",
    "alfonso",
    "alias",
    "ambient",
    "anagram",
    "andy",
    "anvil",
    "appear",
    "apropos",
    "archer",
    "ariel",
    "armor",
    "arrow",
    "austin",
    "avatar",
    "axis",
    "baboon",
    "bahama",
    "bali",
    "balsa",
    "barcode",
    "bazooka",
    "beach",
    "beast",
    "beatles",
    "beauty",
    "before",
    "benny",
    "betty",
    "between",
    "beyond",
    "billy",
    "bison",
    "blast",
    "bless",
    "bogart",
    "bonanza",
    "book",
    "border",
    "brave",
    "bread",
    "break",
    "broken",
    "bucket",
    "buenos",
    "buffalo",
    "bundle",
    "button",
    "buzzer",
    "byte",
    "caesar",
    "camilla",
    "canary",
    "candid",
    "carrot",
    "cave",
    "chant",
    "child",
    "choice",
    "chris",
    "cipher",
    "clarion",
    "clark",
    "clever",
    "cliff",
    "clone",
    "conan",
    "conduct",
    "congo",
    "costume",
    "cotton",
    "cover",
    "crack",
    "current",
    "danube",
    "data",
    "decide",
    "deposit",
    "desire",
    "detail",
    "dexter",
    "dinner",
    "donor",
    "druid",
    "drum",
    "easy",
    "eddie",
    "enjoy",
    "enrico",
    "epoxy",
    "erosion",
    "except",
    "exile",
    "explain",
    "fame",
    "fast",
    "father",
    "felix",
    "field",
    "fiona",
    "fire",
    "fish",
    "flame",
    "flex",
    "flipper",
    "float",
    "flood",
    "floor",
    "forbid",
    "forever",
    "fractal",
    "frame",
    "freddie",
    "front",
    "fuel",
    "gallop",
    "game",
    "garbo",
    "gate",
    "gelatin",
    "gibson",
    "ginger",
    "giraffe",
    "gizmo",
    "glass",
    "goblin",
    "gopher",
    "grace",
    "gray",
    "gregory",
    "grid",
    "griffin",
    "ground",
    "guest",
    "gustav",
    "gyro",
    "hair",
    "halt",
    "harris",
    "heart",
    "heavy",
    "herman",
    "hippie",
    "hobby",
    "honey",
    "hope",
    "horse",
    "hostel",
    "hydro",
    "imitate",
    "info",
    "ingrid",
    "inside",
    "invent",
    "invest",
    "invite",
    "ivan",
    "james",
    "jester",
    "jimmy",
    "join",
    "joseph",
    "juice",
    "julius",
    "july",
    "kansas",
    "karl",
    "kevin",
    "kiwi",
    "ladder",
    "lake",
    "laura",
    "learn",
    "legacy",
    "legend",
    "lesson",
    "life",
    "light",
    "list",
    "locate",
    "lopez",
    "lorenzo",
    "love",
    "lunch",
    "malta",
    "mammal",
    "margin",
    "margo",
    "marion",
    "mask",
    "match",
    "mayday",
    "meaning",
    "mercy",
    "middle",
    "mike",
    "mirror",
    "modest",
    "morph",
    "morris",
    "mystic",
    "nadia",
    "nato",
    "navy",
    "needle",
    "neuron",
    "never",
    "newton",
    "nice",
    "night",
    "nissan",
    "nitro",
    "nixon",
    "north",
    "oberon",
    "octavia",
    "ohio",
    "olga",
    "open",
    "opus",
    "orca",
    "oval",
    "owner",
    "page",
    "paint",
    "palma",
    "parent",
    "parlor",
    "parole",
    "paul",
    "peace",
    "pearl",
    "perform",
    "phoenix",
    "phrase",
    "pierre",
    "pinball",
    "place",
    "plate",
    "plato",
    "plume",
    "pogo",
    "point",
    "polka",
    "poncho",
    "powder",
    "prague",
    "press",
    "presto",
    "pretty",
    "prime",
    "promo",
    "quest",
    "quick",
    "quiz",
    "quota",
    "race",
    "rachel",
    "raja",
    "ranger",
    "region",
    "remark",
    "rent",
    "reward",
    "rhino",
    "ribbon",
    "rider",
    "road",
    "rodent",
    "round",
    "rubber",
    "ruby",
    "rufus",
    "sabine",
    "saddle",
    "sailor",
    "saint",
    "salt",
    "scale",
    "scuba",
    "season",
    "secure",
    "shake",
    "shallow",
    "shannon",
    "shave",
    "shelf",
    "sherman",
    "shine",
    "shirt",
    "side",
    "sinatra",
    "sincere",
    "size",
    "slalom",
    "slow",
    "small",
    "snow",
    "sofia",
    "song",
    "sound",
    "south",
    "speech",
    "spell",
    "spend",
    "spoon",
    "stage",
    "stamp",
    "stand",
    "state",
    "stella",
    "stick",
    "sting",
    "stock",
    "store",
    "sunday",
    "sunset",
    "support",
    "supreme",
    "sweden",
    "swing",
    "tape",
    "tavern",
    "think",
    "thomas",
    "tictac",
    "time",
    "toast",
    "tobacco",
    "tonight",
    "torch",
    "torso",
    "touch",
    "toyota",
    "trade",
    "tribune",
    "trinity",
    "triton",
    "truck",
    "trust",
    "type",
    "under",
    "unit",
    "urban",
    "urgent",
    "user",
    "value",
    "vendor",
    "venice",
    "verona",
    "vibrate",
    "virgo",
    "visible",
    "vista",
    "vital",
    "voice",
    "vortex",
    "waiter",
    "watch",
    "wave",
    "weather",
    "wedding",
    "wheel",
    "whiskey",
    "wisdom",
    "android",
    "annex",
    "armani",
    "cake",
    "confide",
    "deal",
    "define",
    "dispute",
    "genuine",
    "idiom",
    "impress",
    "include",
    "ironic",
    "null",
    "nurse",
    "obscure",
    "prefer",
    "prodigy",
    "ego",
    "fax",
    "jet",
    "job",
    "rio",
    "ski",
    "yes"
];     

/*
* Returns a random string with length 'length'
* containing characters in the 'charset' string.
*/
function randomString(length, charset) {
	var string = "";

	for (var i = 0; i < length; i++) {
		string += charset[randomNumber(0, charset.length)];
	}

	return string;
}

/*
* Returns a random boolean value.
* Either 'true' or 'false'.
*/
function randomBoolean() {
    return randomNumber(0, 2) ? true : false;
}

/*
* Returns a random number between 'min' (inclusive)
* and 'max' (exclusive). Lowest possible value is 'min'.
* Largest possible value is 'max - 1'.
*/
function randomNumber(min, max) {
	return Math.floor(Math.random() * (max - min) + min);
}

const languages = [
"af-ZA", // Afrikaans - South Africa
"sq-AL", // Albanian - Albania
"ar-DZ", // Arabic - Algeria
"ar-BH", // Arabic - Bahrain
"ar-EG", // Arabic - Egypt
"ar-IQ", // Arabic - Iraq
"ar-JO", // Arabic - Jordan
"ar-KW", // Arabic - Kuwait
"ar-LB", // Arabic - Lebanon
"ar-LY", // Arabic - Libya
"ar-MA", // Arabic - Morocco
"ar-OM", // Arabic - Oman
"ar-QA", // Arabic - Qatar
"ar-SA", // Arabic - Saudi Arabia
"ar-SY", // Arabic - Syria
"ar-TN", // Arabic - Tunisia
"ar-AE", // Arabic - United Arab Emirates
"ar-YE", // Arabic - Yemen
"hy-AM", // Armenian - Armenia
//"Cy-az-AZ", // Azeri (Cyrillic) - Azerbaijan
//"Lt-az-AZ", // Azeri (Latin) - Azerbaijan
"eu-ES", // Basque - Basque
"be-BY", // Belarusian - Belarus
"bg-BG", // Bulgarian - Bulgaria
"ca-ES", // Catalan - Catalan
"zh-CN", // Chinese - China
"zh-HK", // Chinese - Hong Kong SAR
"zh-MO", // Chinese - Macau SAR
"zh-SG", // Chinese - Singapore
"zh-TW", // Chinese - Taiwan
"zh-CHS", // Chinese (Simplified)
"zh-CHT", // Chinese (Traditional)
"hr-HR", // Croatian - Croatia
"cs-CZ", // Czech - Czech Republic
"da-DK", // Danish - Denmark
"div-MV", // Dhivehi - Maldives
"nl-BE", // Dutch - Belgium
"nl-NL", // Dutch - The Netherlands
"en-AU", // English - Australia
"en-BZ", // English - Belize
"en-CA", // English - Canada
"en-CB", // English - Caribbean
"en-IE", // English - Ireland
"en-JM", // English - Jamaica
"en-NZ", // English - New Zealand
"en-PH", // English - Philippines
"en-ZA", // English - South Africa
"en-TT", // English - Trinidad and Tobago
"en-GB", // English - United Kingdom
"en-US", // English - United States
"en-ZW", // English - Zimbabwe
"et-EE", // Estonian - Estonia
"fo-FO", // Faroese - Faroe Islands
"fa-IR", // Farsi - Iran
"fi-FI", // Finnish - Finland
"fr-BE", // French - Belgium
"fr-CA", // French - Canada
"fr-FR", // French - France
"fr-LU", // French - Luxembourg
"fr-MC", // French - Monaco
"fr-CH", // French - Switzerland
"gl-ES", // Galician - Galician
"ka-GE", // Georgian - Georgia
"de-AT", // German - Austria
"de-DE", // German - Germany
"de-LI", // German - Liechtenstein
"de-LU", // German - Luxembourg
"de-CH", // German - Switzerland
"el-GR", // Greek - Greece
"gu-IN", // Gujarati - India
"he-IL", // Hebrew - Israel
"hi-IN", // Hindi - India
"hu-HU", // Hungarian - Hungary
"is-IS", // Icelandic - Iceland
"id-ID", // Indonesian - Indonesia
"it-IT", // Italian - Italy
"it-CH", // Italian - Switzerland
"ja-JP", // Japanese - Japan
"kn-IN", // Kannada - India
"kk-KZ", // Kazakh - Kazakhstan
"kok-IN", // Konkani - India
"ko-KR", // Korean - Korea
"ky-KZ", // Kyrgyz - Kazakhstan
"lv-LV", // Latvian - Latvia
"lt-LT", // Lithuanian - Lithuania
"mk-MK", // Macedonian (FYROM)	0x042F", // MKD
"ms-BN", // Malay - Brunei
"ms-MY", // Malay - Malaysia
"mr-IN", // Marathi - India
"mn-MN", // Mongolian - Mongolia
"nb-NO", // Norwegian (Bokmål) - Norway
"nn-NO", // Norwegian (Nynorsk) - Norway
"pl-PL", // Polish - Poland
"pt-BR", // Portuguese - Brazil
"pt-PT", // Portuguese - Portugal
"pa-IN", // Punjabi - India
"ro-RO", // Romanian - Romania
"ru-RU", // Russian - Russia
"sa-IN", // Sanskrit - India
//"Cy-sr-SP", // Serbian (Cyrillic) - Serbia
//"Lt-sr-SP", // Serbian (Latin) - Serbia
"sk-SK", // Slovak - Slovakia
"sl-SI", // Slovenian - Slovenia
"es-AR", // Spanish - Argentina
"es-BO", // Spanish - Bolivia
"es-CL", // Spanish - Chile
"es-CO", // Spanish - Colombia
"es-CR", // Spanish - Costa Rica
"es-DO", // Spanish - Dominican Republic
"es-EC", // Spanish - Ecuador
"es-SV", // Spanish - El Salvador
"es-GT", // Spanish - Guatemala
"es-HN", // Spanish - Honduras
"es-MX", // Spanish - Mexico
"es-NI", // Spanish - Nicaragua
"es-PA", // Spanish - Panama
"es-PY", // Spanish - Paraguay
"es-PE", // Spanish - Peru
"es-PR", // Spanish - Puerto Rico
"es-ES", // Spanish - Spain
"es-UY", // Spanish - Uruguay
"es-VE", // Spanish - Venezuela
"sw-KE", // Swahili - Kenya
"sv-FI", // Swedish - Finland
"sv-SE", // Swedish - Sweden
"syr-SY", // Syriac - Syria
"ta-IN", // Tamil - India
"tt-RU", // Tatar - Russia
"te-IN", // Telugu - India
"th-TH", // Thai - Thailand
"tr-TR", // Turkish - Turkey
"uk-UA", // Ukrainian - Ukraine
"ur-PK", // Urdu - Pakistan
//"Cy-uz-UZ", // Uzbek (Cyrillic) - Uzbekistan
//"Lt-uz-UZ", // Uzbek (Latin) - Uzbekistan
"vi-VN"]; // Vietnamese - Vietnam

      function fakeReferrer(origin) {
        Math.seedrandom(origin);

        const firstWord = words[randomNumber(0, words.length)];
        const secondWord = words[randomNumber(0, words.length)];
        const thirdWord = words[randomNumber(0, words.length)];
        const firstDirName = words[randomNumber(0, words.length)];
        const secondDirName = words[randomNumber(0, words.length)];
        const thirdDirName = words[randomNumber(0, words.length)];

        const protocols = ["https", "http"];

        const protocol = protocols[randomNumber(0, protocols.length)];

        const tlds = ["com", "net", "org", "gov", "info", "xxx"];

        const tld = tlds[randomNumber(0, tlds.length)];

        const referrer = protocol + "://www." + firstWord + secondWord + thirdWord + "." + tld + "/" + firstDirName + "/" + secondDirName + "/" + thirdDirName;

        return referrer;
    }

    const origin = window.location.hostname;

    const fakeReferrerValue = fakeReferrer(origin);

    Object.defineProperties(window.document, {
        referrer: {
            configurable: false,
            enumerable: true,
            get: function getReferrer() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Document.referrer");

                return fakeReferrerValue;
            }
        }
    });

      function fakeActiveVRDisplays(origin) {       Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeAppCodeName(origin) {            Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeAppName(origin) {                Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }

    // TODO: Is always returning "5.0" safe?
    function fakeAppVersion(origin) {
        return "5.0";
    }
    function fakeBattery(origin) {                Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeConnection(origin) {             Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeGeoLocation(origin) {            Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeHardwareConcurrency(origin) {    Math.seedrandom(origin); return randomNumber(0, 9); }
    function fakeJavaEnabled(origin) {            Math.seedrandom(origin); return randomBoolean(); }
    function fakeLanguage(origin) {
        Math.seedrandom(origin);

        const language = languages[randomNumber(0, languages.length)];

        const languageParts = language.split("-");

        switch (languageParts.length) {
            case 2:
                return languageParts[0] + "-" + languageParts[1];
            case 3:
                // TODO: How to display Cyrillic or Latin versions of the same language???
        }
    }
    function fakeLanguages(origin) {
        Math.seedrandom(origin);

        const language = languages[randomNumber(0, languages.length)];

        const languageParts = language.split("-");

        switch (languageParts.length) {
            case 2:
                return languageParts[0] + "-" + languageParts[1] + "," + languageParts[0];
            case 3:
                // TODO: How to display Cyrillic or Latin versions of the same language???
        }
    }
    function fakeMimeTypes(origin) {              Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeOnLine(origin) {                 Math.seedrandom(origin); return randomBoolean(); }
    function fakeOscpu(origin) {                  Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakePermissions(origin) {            Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakePlatform(origin) {               Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakePlugins(origin) {                Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeProduct(origin) {                Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeServiceWorker(origin) {          Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeStorage(origin) {                Math.seedrandom(origin); return randomString(32, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
    function fakeUserAgent(origin) {
        Math.seedrandom(origin);
        const randomBrowser = randomNumber(0, 5);
        switch (randomBrowser) {
            case 0:
                return ieUserAgents[randomNumber(0, ieUserAgents.length)];
            case 1:
                return firefoxUserAgents[randomNumber(0, firefoxUserAgents.length)];
            case 2:
                return chromeUserAgents[randomNumber(0, chromeUserAgents.length)];
            case 3:
                return safariUserAgents[randomNumber(0, safariUserAgents.length)];
            case 4:
                return operaUserAgents[randomNumber(0, operaUserAgents.length)];
        }
    }

    const origin = window.location.hostname;

    const fakeActiveVRDisplaysValue       = fakeActiveVRDisplays(origin);
    const fakeAppCodeNameValue            = fakeAppCodeName(origin);
    const fakeAppNameValue                = fakeAppName(origin);
    const fakeAppVersionValue             = fakeAppVersion(origin);
    const fakeBatteryValue                = fakeBattery(origin);
    const fakeConnectionValue             = fakeConnection(origin);
    const fakeGeoLocationValue            = fakeGeoLocation(origin);
    const fakeHardwareConcurrencyValue    = fakeHardwareConcurrency(origin);
    const fakeJavaEnabledValue            = fakeJavaEnabled(origin);
    const fakeLanguageValue               = fakeLanguage(origin);
    const fakeLanguagesValue              = fakeLanguages(origin);
    const fakeMimeTypesValue              = fakeMimeTypes(origin);
    const fakeOnLineValue                 = fakeOnLine(origin);
    const fakeOscpuValue                  = fakeOscpu(origin);
    const fakePermissionsValue            = fakePermissions(origin);
    const fakePlatformValue               = fakePlatform(origin);
    const fakePluginsValue                = fakePlugins(origin);
    const fakeProductValue                = fakeProduct(origin);
    const fakeServiceWorkerValue          = fakeServiceWorker(origin);
    const fakeStorageValue                = fakeStorage(origin);
    const fakeUserAgentValue              = fakeUserAgent(origin);

    Object.defineProperties(window.navigator, {
        /*
        activeVRDisplays: {
            configurable: false,
            enumerable: true,
            get: function getActiveVRDisplays() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.activeVRDisplays");

                return fakeActiveVRDisplaysValue;
            }
        },
        appCodeName: {
            configurable: false,
            enumerable: true,
            get: function getAppCodeName() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.appCodeName");

                return fakeAppCodeNameValue;
            }
        },
        appName: {
            configurable: false,
            enumerable: true,
            get: function getAppName() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.appName");

                return fakeAppNameValue;
            }
        },
        */

        appVersion: {
            configurable: false,
            enumerable: true,
            get: function getAppVersion() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.appVersion");

                return fakeAppVersionValue;
            }
        },

        // TODO: This is getBattery() now
        /*
        battery: {
            configurable: false,
            enumerable: true,
            get: function getBattery() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.battery");

                return fakeBatteryValue;
            }
        },
        connection: {
            configurable: false,
            enumerable: true,
            get: function getConnection() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.connection");

                return fakeConnectionValue;
            }
        },
        geolocation: {
            configurable: false,
            enumerable: true,
            get: function getGeoLocation() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.geolocation");

                return fakeGeoLocationValue;
            }
        },
        */

        hardwareConcurrency: {
            configurable: false,
            enumerable: true,
            get: function getHardwareConcurrency() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.hardwareConcurrency");

                return fakeHardwareConcurrencyValue;
            }
        },

        // TODO: javaEnabled() is a method, not a property
        /*
        javaEnabled: {
            configurable: false,
            enumerable: true,
            get: function getJavaEnabled() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.javaEnabled");

                return fakeJavaEnabledValue;
            }
        },
        */

        language: {
            configurable: false,
            enumerable: true,
            get: function getLanguage() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.language");

                return fakeLanguageValue;
            }
        },
        languages: {
            configurable: false,
            enumerable: true,
            get: function getLanguages() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.languages");

                return fakeLanguagesValue;
            }
        },
        mimeTypes: {
            configurable: false,
            enumerable: true,
            get: function getMimeTypes() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.mimeTypes");

                return fakeMimeTypesValue;
            }
        },

        /*
        onLine: {
            configurable: false,
            enumerable: true,
            get: function getOnLine() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.onLine");

                return fakeOnLineValue;
            }
        },
        */

        oscpu: {
            configurable: false,
            enumerable: true,
            get: function getOscpu() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.oscpu");

                return fakeOscpuValue;
            }
        },

        /*
        permissions: {
            configurable: false,
            enumerable: true,
            get: function getPermissions() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.permissions");

                return fakePermissionsValue;
            }
        },
        */

        platform: {
            configurable: false,
            enumerable: true,
            get: function getPlatform() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.platform");

                return fakePlatformValue;
            }
        },
        plugins: {
            configurable: false,
            enumerable: true,
            get: function getPlugins() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.plugins");

                return fakePluginsValue;
            }
        },

        /*
        product: {
            configurable: false,
            enumerable: true,
            get: function getProduct() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.product");

                return fakeProductValue;
            }
        },
        serviceWorker: {
            configurable: false,
            enumerable: true,
            get: function getServiceWorker() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.serviceWorker");

                return fakeServiceWorkerValue;
            }
        },
        storage: {
            configurable: false,
            enumerable: true,
            get: function getStorage() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.storage");

                return fakeStorageValue;
            }
        },
        */

        userAgent: {
            configurable: false,
            enumerable: true,
            get: function getUserAgent() {
                console.log("[ALERT] " + window.location.hostname + " accessed property Navigator.userAgent");

                return fakeUserAgentValue;
            }
        }
            });

              function fakeLength(origin) { Math.seedrandom(origin); return randomNumber(0, 256); }

    const origin = window.location.hostname;

    const fakeLengthValue = fakeLength(origin);

    Object.defineProperties(window.history, {
        length: {
            configurable: false,
            enumerable: true,
            get: function getLength() {
                console.log("[ALERT] " + window.location.hostname + " accessed property History.length");

                return fakeLengthValue;
            }
        }
    });

    
    function fakeCharging(origin) {           Math.seedrandom(origin); return randomBoolean(); }
    function fakeChargingTime(origin) {       Math.seedrandom(origin); return randomNumber(0, 9999); }
    function fakeDischargingTime(origin) {    Math.seedrandom(origin); return randomNumber(0, 9999); }
    function fakeLevel(origin) {              Math.seedrandom(origin); return randomNumber(0, 2); }

    const origin = window.location.hostname;

    const fakeChargingValue        = fakeCharging(origin);
    const fakeChargingTimeValue    = fakeChargingTime(origin);
    const fakeDischargingValue     = fakeDischargingTime(origin);
    const fakeLevelValue           = fakeLevel(origin);

    Object.defineProperties(BatteryManager.prototype, {
        charging: {
            configurable: false,
            enumerable: true,
            get: function getCharging() {
                console.log("[ALERT] " + window.location.hostname + " accessed property BatteryManager.charging");

                return fakeChargingValue;
            }
        },

        // TODO: This value could be infinity, if device is not charging
        chargingTime: {
            configurable: false,
            enumerable: true,
            get: function getChargingTime() {
                console.log("[ALERT] " + window.location.hostname + " accessed property BatteryManager.chargingTime");

                return fakeChargingTimeValue;
            }
        },

        // TODO: This value could be infinity, if device is charging
        dischargingTime: {
            configurable: false,
            enumerable: true,
            get: function getDischargingTime() {
                console.log("[ALERT] " + window.location.hostname + " accessed property BatteryManager.dischargingTime");

                return fakeDischargingValue;
            }
        },

        // TODO: This could return any decimal number between 0 and 1, not just 0 or 1
        level: {
            configurable: false,
            enumerable: true,
            get: function getLevel() {
                console.log("[ALERT] " + window.location.hostname + " accessed property BatteryManager.level");

                return fakeLevelValue;
            }
        }
    });

    function randomAcceptHeader(origin) {
	return "NotYetImplemented";
}
function randomAcceptCharsetHeader(origin) {
	return "NotYetImplemented";
}
function randomAcceptEncodingHeader(origin) {
	return "NotYetImplemented";
}
function randomAcceptLanguageHeader(origin) {
    Math.seedrandom(origin);

    const language = languages[randomNumber(0, languages.length)];

    const languageParts = language.split("-");

    switch (languageParts.length) {
        case 2:
            return languageParts[0] + "-" + languageParts[1] + ";q=0.8," + languageParts[0] + ";q=0.6";
        case 3:
            // TODO: How to display Cyrillic or Latin versions of the same language???
    }
}
function randomAuthorizationHeader(origin) {
	return "NotYetImplemented";
}
function randomExpectHeader(origin) {
	return "NotYetImplemented";
}
function randomFromHeader(origin) {
	return "NotYetImplemented";
}
function randomHostHeader(origin) {
	return "NotYetImplemented";
}
function randomIfMatchHeader(origin) {
	return "NotYetImplemented";
}
function randomIfModifiedSinceHeader(origin) {
	return "NotYetImplemented";
}
function randomIfNoneMatchHeader(origin) {
	return "NotYetImplemented";
}
function randomIfRangeHeader(origin) {
	return "NotYetImplemented";
}
function randomIfUnmodifiedSinceHeader(origin) {
	return "NotYetImplemented";
}
function randomMaxForwardsHeader(origin) {
	return "NotYetImplemented";
}
function randomProxyAuthorizationHeader(origin) {
	return "NotYetImplemented";
}
function randomRangeHeader(origin) {
	return "NotYetImplemented";
}
function randomRefererHeader(origin) {
    Math.seedrandom(origin);

    const firstWord = words[randomNumber(0, words.length)];
    const secondWord = words[randomNumber(0, words.length)];
    const thirdWord = words[randomNumber(0, words.length)];
    const firstDirName = words[randomNumber(0, words.length)];
    const secondDirName = words[randomNumber(0, words.length)];
    const thirdDirName = words[randomNumber(0, words.length)];

    const protocols = ["https", "http"];

    const protocol = protocols[randomNumber(0, protocols.length)];

    const tlds = ["com", "net", "org", "gov", "info", "xxx"];

    const tld = tlds[randomNumber(0, tlds.length)];

    const referrer = protocol + "://www." + firstWord + secondWord + thirdWord + "." + tld + "/" + firstDirName + "/" + secondDirName + "/" + thirdDirName;

    return referrer;
}
function randomTEHeader(origin) {
	return "NotYetImplemented";
}
function randomUserAgentHeader(origin) {
    Math.seedrandom(origin);
    const randomBrowser = randomNumber(0, 5);
    switch (randomBrowser) {
        case 0:
            return ieUserAgents[randomNumber(0, ieUserAgents.length)];
        case 1:
            return firefoxUserAgents[randomNumber(0, firefoxUserAgents.length)];
        case 2:
            return chromeUserAgents[randomNumber(0, chromeUserAgents.length)];
        case 3:
            return safariUserAgents[randomNumber(0, safariUserAgents.length)];
        case 4:
            return operaUserAgents[randomNumber(0, operaUserAgents.length)];
    }
}

function rewriteHttpHeaders(e) {
    var serverUrl = new URL(e.url);
    var origin = serverUrl.hostname;

	for (var header of e.requestHeaders) {
		if (header.name.toLowerCase() === "accept") {
		}
		else if (header.name.toLowerCase() === "accept-charset") {
		}
		else if (header.name.toLowerCase() === "accept-encoding") {
		}
		else if (header.name.toLowerCase() === "accept-language") {
            header.value = randomAcceptLanguageHeader(origin);
		}
		else if (header.name.toLowerCase() === "authorization") {
		}
		else if (header.name.toLowerCase() === "expect") {
		}
		else if (header.name.toLowerCase() === "from") {
		}
		else if (header.name.toLowerCase() === "host") {
		}
		else if (header.name.toLowerCase() === "if-match") {
		}
		else if (header.name.toLowerCase() === "if-modified-since") {
		}
		else if (header.name.toLowerCase() === "if-none-match") {
		}
		else if (header.name.toLowerCase() === "if-range") {
		}
		else if (header.name.toLowerCase() === "if-unmodified-since") {
		}
		else if (header.name.toLowerCase() === "max-forwards") {
		}
		else if (header.name.toLowerCase() === "proxy-authorization") {
		}
		else if (header.name.toLowerCase() === "range") {
		}
		else if (header.name.toLowerCase() === "referer") {
			header.value = randomRefererHeader(origin);
		}
		else if (header.name.toLowerCase() === "te") {
		}
		else if (header.name.toLowerCase() === "user-agent") {
			header.value = randomUserAgentHeader(origin);
		}
	}

	return {requestHeaders: e.requestHeaders};
}

chrome.webRequest.onBeforeSendHeaders.addListener(rewriteHttpHeaders, {urls: ["<all_urls>"]}, ["blocking", "requestHeaders"]);
]]>.Value

        Public Sub New(ByVal Instance As String, Optional ByVal Proxy As ProxyInfo = Nothing)
            Dim CachePath As String = "LatamAirlines" & "\" & Program.ID & "\" & Instance

            Try
                If IO.Directory.Exists(CachePath) = True Then
                    IO.Directory.Delete(CachePath, True)
                End If
            Catch ex As Exception

            End Try

            Engine = EO.WebEngine.Engine.Create(Instance)
            InstanceText = Instance

            If Execution_Watcher Is Nothing Then Execution_Watcher = New Stopwatch

            If Proxy IsNot Nothing Then
                Engine.Options.Proxy = Proxy
            End If

            Engine.Options.DisableBuiltInPlugIns = True
            Engine.Options.CachePath = CachePath
            Engine.Options.ExtraCommandLineArgs = "privet-ipv6-only"
            Engine.Options.SkipWaitForLayerActivation = True
            Engine.Options.DisableWMPointer = True
            Engine.Options.BypassUserGestureCheck = True
            Engine.Options.DisableGPU = True

            WebViewHost = New EO.WebBrowser.WebView() With {.Engine = Engine}
            WebView = New EO.WinForm.WebControl With {.WebView = WebViewHost, .Dock = DockStyle.Fill}

            AddHandler WebViewHost.BeforeDownload, Sub(sender As Object, e As BeforeDownloadEventArgs)
                                                       e.ShowDialog = False
                                                   End Sub

            AddHandler WebViewHost.CertificateError, Sub(sender As Object, e As CertificateErrorEventArgs)
                                                         e.Continue()
                                                     End Sub

        End Sub

        Public Async Function LoadLoginPageAsync() As Task(Of NavigationTask)
            If UseRandomUserAgent = True Then
                WebViewHost.CustomUserAgent = UserAgentGenerator.GetRandomUserAgent

                If Program.Settings.ReadIni("Config", "JSPatchs", False) = True Then WebViewHost.JSInitCode = ChangeUserAgentJS

                Console.WriteLine("CustomUserAgent: " & WebViewHost.CustomUserAgent)
            End If

            If Secure = False Then
                Console.WriteLine("Wait")

                WebViewHost.LoadUrl(Models.URLValues.Login)
                For i As Integer = 0 To 60

                    If IsPrimaryButtonVisible() = True AndAlso DomAvailable() = True Then
                        Exit For
                    End If

                    If LoadFailed = True Then
                        Exit For
                    End If

                    Threading.Thread.Sleep(1000)
                    Application.DoEvents()

                    'i -= 1
                Next
                Console.WriteLine("No Wait")
                Return Nothing
            End If

            Return WebViewHost.LoadUrl(Models.URLValues.Login)
        End Function

        Private ErrorMessage As String = String.Empty
        Private LoadFailed As Boolean = False

        Private Sub WebViewHost_LoadFailed(sender As Object, e As LoadFailedEventArgs) Handles WebViewHost.LoadFailed
            ErrorMessage = e.ErrorMessage
            Console.WriteLine("Error Code: " & e.ErrorCode & "   Message: " & e.ErrorMessage)
            LoadFailed = True
        End Sub

        Public Function CheckAlive(ByVal Navigate As NavigationTask) As Models.NavigationResult
            Try

                If Secure = True Then
                    Dim Result As Models.NavigationResult = New Models.NavigationResult(Navigate.HttpStatusCode, Navigate.ErrorCode)

                    If Not Navigate.HttpStatusCode = 200 Then
                        If Navigate.Exception IsNot Nothing Then Result.Message = Navigate.Exception.Message.ToString
                        Return Result
                    Else

                        Dim Doc As EO.WebBrowser.DOM.Document = WebViewHost.GetDOMWindow().document
                        Dim ContinueButton As DOM.Element = Doc.getElementById("primary-button")

                        If ContinueButton.Item("type").ToString.ToLower = "submit" Then
                            Result.IsAlive = True
                        End If

                        Return Result

                    End If
                Else
                    Dim Result As Models.NavigationResult = New Models.NavigationResult("", "") With {.IsAlive = True}

                    For i As Integer = 0 To 2
                        If LoadFailed = True Then
                            Result.IsAlive = False
                            LoadFailed = False
                            Threading.Thread.Sleep(500)
                            Exit For
                        End If

                        Try
                            Dim Doc As EO.WebBrowser.DOM.Document = WebViewHost.GetDOMWindow().document
                            If Doc IsNot Nothing Then Exit For
                        Catch ex As Exception : End Try

                        Application.DoEvents()
                        Threading.Thread.Sleep(1000)
                        i -= 1
                    Next

                    Return Result
                End If

            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Enum WebStates As Integer
            Starting = -1
            InUserWriting = 0
            LoadingUser = 1
            UserCheckPoint = 2
            InPasswordWriting = 3
            LoadingPassword = 4
            PasswordCheckPoint = 5
            Finalization = 6
        End Enum

        Public Async Function Login(ByVal Account As Models.Account, Optional ByVal Timeout_Seconds As Integer = 59) As Task(Of Models.AuthResult)
            WebViewHost.StopLoad()

            Execution_Watcher.Start()

            Dim CurrentState As WebStates = WebStates.Starting

            Console.WriteLine("OnLoading")

            Dim Result As Models.AuthResult = Nothing

            If Program.Settings.ReadIni("Config", "JSPatchs", False) = True Then WebViewHost.EvalScript(ChangeUserAgentJS)

            AddHandler WebViewHost.NeedCredentials, Sub(ByVal sender As Object, ByVal e As NeedCredentialsEventArgs)
                                                        Result = New Models.AuthResult(False, "Need Proxy Credentials") With {.Status = CurrentState}
                                                        'e.[Continue]("username", "password")
                                                    End Sub

            Dim Navigate As NavigationTask = Await LoadLoginPageAsync()

            Dim Asynctask As New Task(New System.Action(Sub()

                                                            Console.WriteLine("OnStart")

                                                            Dim NavigationResult As API.Models.NavigationResult = CheckAlive(Navigate)

                                                            If Program.Settings.ReadIni("Config", "JSPatchs", False) = True Then WebViewHost.EvalScript(ChangeUserAgentJS)
                                                            Console.WriteLine("CustomUserAgent: " & WebViewHost.CustomUserAgent)
                                                            If NavigationResult IsNot Nothing AndAlso NavigationResult.IsAlive = True Then
                                                                Engine.CookieManager.DeleteCookies()
                                                                CurrentState = WebStates.InUserWriting

                                                                Dim Doc As EO.WebBrowser.DOM.Document = WebViewHost.GetDOMWindow().document

                                                                WebViewHost.EvalScript("document.getElementById('form-input--alias').focus();")


                                                                For Each StChar As Char In Account.Username.ToCharArray
                                                                    WebViewHost.SendChar(StChar)
                                                                Next

                                                                Threading.Thread.Sleep(2000)

                                                                WebViewHost.EvalScript("document.getElementById('primary-button').click();")

                                                                CurrentState = WebStates.LoadingUser

                                                                Threading.Thread.Sleep(2000)

                                                                For i As Integer = 0 To 2
                                                                    Console.WriteLine("esperando....")
                                                                    If Result IsNot Nothing Then Exit Sub
                                                                    Try
                                                                        If IsInvalidLogin() = True Then Exit For

                                                                        Try
                                                                            Dim title As String = WebViewHost.EvalScript("document.title;")
                                                                            If String.Equals(title, "Access Denied", StringComparison.OrdinalIgnoreCase) Then
                                                                                Exit For
                                                                            End If
                                                                        Catch ex As Exception : End Try

                                                                        Dim primaryButton As Object = WebViewHost.EvalScript("document.getElementById('primary-button');")

                                                                        If primaryButton IsNot Nothing Then
                                                                            Dim IndexValue = WebViewHost.EvalScript("document.getElementById('primary-button').getAttribute('tabindex')").ToString
                                                                            If IndexValue = "0" Then
                                                                                Exit For
                                                                            End If
                                                                        Else
                                                                            Exit For
                                                                        End If

                                                                    Catch ex As Exception : Exit For : End Try

                                                                    Threading.Thread.Sleep(500)
                                                                    Application.DoEvents()
                                                                    i -= 1
                                                                Next

                                                                Console.WriteLine("Continue....")

                                                                CurrentState = WebStates.UserCheckPoint


                                                                If Secure = True Then
                                                                    Do While (WebViewHost.IsLoading)
                                                                        If Result IsNot Nothing Then Exit Sub
                                                                        Application.DoEvents()
                                                                    Loop
                                                                End If

                                                                Console.WriteLine("Continue  2....")

                                                                Dim UserLoginError As String = IsUserError()

                                                                If Not UserLoginError = String.Empty Then

                                                                    Result = New Models.AuthResult(False, UserLoginError) With {.Status = CurrentState}

                                                                ElseIf IsInvalidLogin() = True Then

                                                                    Result = New Models.AuthResult(False, "Invalid Login") With {.Status = CurrentState, .IsUserValid = False, .RequiredProxyChanged = True}

                                                                Else

                                                                    'Do While (IsPasswordInputVisible() = False) : Application.DoEvents() : Loop

                                                                    For i As Integer = 0 To 60
                                                                        If Result IsNot Nothing Then Exit Sub
                                                                        If IsPasswordInputVisible() = True Then
                                                                            Exit For
                                                                        End If

                                                                        Try
                                                                            Dim title As String = WebViewHost.EvalScript("document.title;")
                                                                            If String.Equals(title, "Access Denied", StringComparison.OrdinalIgnoreCase) Then
                                                                                Exit For
                                                                            End If
                                                                        Catch ex As Exception : End Try

                                                                        System.Threading.Thread.Sleep(1000)
                                                                        Application.DoEvents()
                                                                    Next

                                                                    If IsPasswordInputVisible() = True Then
                                                                        CurrentState = WebStates.InPasswordWriting
                                                                        WebViewHost.EvalScript("document.getElementById('form-input--password').focus();")

                                                                        For Each StChar As Char In Account.Password.ToCharArray
                                                                            WebViewHost.SendChar(StChar)
                                                                        Next

                                                                        Threading.Thread.Sleep(2000)

                                                                        CurrentState = WebStates.LoadingPassword
                                                                        WebViewHost.EvalScript("document.getElementById('primary-button').click();")


                                                                        Threading.Thread.Sleep(2000)

                                                                        For i As Integer = 0 To 2

                                                                            If Result IsNot Nothing Then Exit Sub

                                                                            Try
                                                                                Dim primaryButton As Object = WebViewHost.EvalScript("document.getElementById('primary-button');")

                                                                                If primaryButton IsNot Nothing Then
                                                                                    Dim IndexValue = WebViewHost.EvalScript("document.getElementById('primary-button').getAttribute('tabindex')").ToString
                                                                                    If IndexValue = "0" Then
                                                                                        Exit For
                                                                                    End If
                                                                                Else
                                                                                    Exit For
                                                                                End If
                                                                            Catch ex As Exception : Exit For : End Try

                                                                            Threading.Thread.Sleep(500)
                                                                            Application.DoEvents()
                                                                            i -= 1
                                                                        Next

                                                                        CurrentState = WebStates.PasswordCheckPoint

                                                                        Dim PasswordError As String = IsPasswordError()

                                                                        If PasswordError = String.Empty Then

                                                                            CurrentState = WebStates.Finalization

                                                                            Result = New Models.AuthResult(True, "The account is valid") With {.Status = CurrentState, .IsUserValid = True}
                                                                        Else
                                                                            Result = New Models.AuthResult(False, PasswordError) With {.Status = CurrentState, .IsUserValid = True}
                                                                        End If

                                                                    Else

                                                                        If ErrorMessage = String.Empty Then ErrorMessage = " The website could Not be loaded."
                                                                        Result = New Models.AuthResult(False, ErrorMessage) With {.Status = -1}

                                                                    End If


                                                                End If

                                                                CurrentState = WebStates.Finalization

                                                            Else

                                                                If ErrorMessage = String.Empty Then ErrorMessage = " The website could Not be loaded."
                                                                Result = New Models.AuthResult(False, ErrorMessage) With {.Status = CurrentState}

                                                            End If

                                                        End Sub), TaskCreationOptions.LongRunning)

            If Secure = True Then
                Navigate.OnDone(Sub()
                                    Asynctask.Start()
                                End Sub)
            Else
                Asynctask.Start()
            End If


            For i As Integer = 0 To 2

                If Result Is Nothing Then
                    If Execution_Watcher.IsRunning Then
                        'If CurrentState = WebStates.Starting And IsPrimaryButtonVisible() = False Then
                        '    If Execution_Watcher.Elapsed.Seconds > 30 Then
                        '        Result = New Models.AuthResult(False, " WebPage Dont Loaded") With {.Status = -1, .IsUserValid = False, .RequiredProxyChanged = True}
                        '        Console.WriteLine("Reset 1")
                        '        Exit Do
                        '    End If
                        'Else

                        'End If
                        Console.Title = "Account: " & Account.Username & " Timeleft : " & Execution_Watcher.Elapsed.Seconds & " Seconds"
                        If Execution_Watcher.Elapsed.Seconds >= Timeout_Seconds Then
                            Result = New Models.AuthResult(False, "System Timeout") With {.Status = -1, .IsUserValid = False, .RequiredProxyChanged = True}
                            Console.WriteLine("Reset 2")
                            Exit For
                        End If
                    End If

                Else
                    Exit For
                End If
                Application.DoEvents()
                i -= 1
            Next

            Try
                Execution_Watcher.Stop()
                Asynctask.Dispose()
                WebViewHost.StopLoad()
                WebViewHost.StopAllFindSessions()
            Catch ex As Exception

            End Try

            Console.WriteLine("User: " & Account.Username & " End: " & Result.ErrorMessage)

            Return Result
        End Function

        Private Function IsUserError() As String
            Try
                Dim StatusLabel As DOM.Element = WebViewHost.GetDOMWindow().document.getElementById("form-input--alias-helper-text")

                If StatusLabel Is Nothing Then


                    Dim AlertError As DOM.Element = WebViewHost.GetDOMWindow().document.getElementById("form-alert--error-usersearch")
                    If AlertError IsNot Nothing Then
                        Return AlertError.innerText
                    Else
                        Return String.Empty
                    End If

                Else
                    Return StatusLabel.innerText
                End If
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Private Function IsInvalidLogin() As Boolean
            Try
                Return (WebViewHost.EvalScript("document.getElementsByClassName('sc-AxirZ dOjtAe').length") > 0)
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function IsUserInputVisible() As Boolean
            Try
                Return (WebViewHost.EvalScript("document.getElementById('form-input--alias');") IsNot Nothing)
            Catch ex As Exception
                Return False
            End Try
        End Function


        Private Function IsPrimaryButtonVisible() As Boolean
            Try

                Dim IndexValue = WebViewHost.EvalScript("document.getElementById('primary-button').getAttribute('tabindex')").ToString
                Return (IndexValue = "0")
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function DomAvailable() As Boolean
            Try
                Dim Doc As EO.WebBrowser.DOM.Document = WebViewHost.GetDOMWindow().document
                Return (String.IsNullOrEmpty(Doc.title) = False)
            Catch ex As Exception
                Return False
            End Try
        End Function


        Private Function IsPasswordInputVisible() As Boolean
            Try
                Return (WebViewHost.GetDOMWindow().document.getElementById("form-input--password") IsNot Nothing)
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function IsPasswordError() As String
            Try
                Dim StatusLabel As DOM.Element = WebViewHost.GetDOMWindow().document.getElementById("form-input--password-helper-text")

                If StatusLabel Is Nothing Then


                    Dim AlertError As DOM.Element = WebViewHost.GetDOMWindow().document.getElementById("form-alert--error-login")
                    If AlertError IsNot Nothing Then
                        Return AlertError.innerText
                    Else
                        Return String.Empty
                    End If

                Else
                    Return StatusLabel.innerText
                End If
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Private Sub WebViewHost_NewWindow(sender As Object, e As NewWindowEventArgs) Handles WebViewHost.NewWindow
            e.Accepted = False
        End Sub



#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Try
                        Engine.CookieManager.DeleteCookies()
                        Engine.Stop(True)
                        WebView.Dispose()
                        WebViewHost.Dispose()
                    Catch ex As Exception : End Try

                    Try
                        Dim FolderPath As String = IO.Path.Combine(IO.Path.GetTempPath, WebViewHost.Engine.Options.CachePath)
                        If System.IO.Directory.Exists(FolderPath) Then System.IO.Directory.Delete(FolderPath, True)
                    Catch ex As Exception : End Try
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class
End Namespace

