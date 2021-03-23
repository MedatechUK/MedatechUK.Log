Imports System.IO
Imports System.Reflection

Namespace Logging

    Public Enum eLogLocation
        CurrentyWorkingDirectory
        GetEntryAssemblyDirectory

    End Enum

    Public Module Events

        Public logHandlerDelegate As EventHandler

#Region "Log command"

        Public Sub Log(ByRef Sender As Object, str As String, ParamArray args() As String)
            logHandlerDelegate.Invoke(Sender, New LogArgs(str, args))
        End Sub

        Public Sub Log(str As String, ParamArray args() As String)
            logHandlerDelegate.Invoke(Nothing, New LogArgs(str, args))
        End Sub

        Public Sub Log(ByRef Sender As Object, ex As Exception)
            logHandlerDelegate.Invoke(Sender, New LogArgs(ex))
        End Sub

        Public Sub Log(ex As Exception)
            logHandlerDelegate.Invoke(Nothing, New LogArgs(ex))
        End Sub

#End Region

        Private _logLoc As eLogLocation = eLogLocation.GetEntryAssemblyDirectory
        Public Property LogLocation As eLogLocation
            Get
                Return _logLoc
            End Get
            Set(value As eLogLocation)
                _logLoc = value
            End Set
        End Property

        Private Function LogFolder() As DirectoryInfo
            Dim root As String
            Select Case _logLoc
                Case eLogLocation.CurrentyWorkingDirectory
                    root = Directory.GetCurrentDirectory

                Case eLogLocation.GetEntryAssemblyDirectory
                    root = New FileInfo((Assembly.GetEntryAssembly.Location)).Directory.FullName

                Case Else
                    Throw New NotImplementedException

            End Select

            Return New DirectoryInfo(
            Path.Combine(
               root,
                String.Format(
                    "log\{0}",
                    Now.ToString("yyyy-MM")
                )
            )
        )

        End Function

        Public Function currentlog() As FileInfo
            With LogFolder()
                If Not .Exists Then .Create()
                Return New FileInfo(
                Path.Combine(
                    .FullName,
                    String.Format(
                        "{0}.txt",
                        Now.ToString("yyMMdd")
                    )
                )
            )

            End With

        End Function

        Public Sub logHandler(sender As Object, e As LogArgs)
            Using log As New StreamWriter(currentlog.FullName, True)
                If Not sender Is Nothing Then
                    log.WriteLine("{0}> {1} {2}", Format(Now, "HH:mm:ss"), sender.ToString, e.Message)

                Else
                    log.WriteLine("{0}> {1}", Format(Now, "HH:mm:ss"), e.Message)

                End If

            End Using

            Console.WriteLine(e.Message)

        End Sub

    End Module

End Namespace
