Namespace Logging

    Public MustInherit Class Logable
        Public Property logHandler As EventHandler
            Get
                If Not logHandlerDelegate Is Nothing Then
                    Return logHandlerDelegate
                Else
                    Return Logging.Events.logHandlerDelegate
                End If
            End Get
            Set(value As EventHandler)
                logHandlerDelegate = value
            End Set
        End Property

    End Class

    Public MustInherit Class LogableDictionary
        Inherits Dictionary(Of String, String)
        Public Property logHandler As EventHandler
            Get
                If Not logHandlerDelegate Is Nothing Then
                    Return logHandlerDelegate
                Else
                    Return Logging.Events.logHandlerDelegate
                End If
            End Get
            Set(value As EventHandler)
                logHandlerDelegate = value
            End Set
        End Property

    End Class

    Public MustInherit Class LogableService
        Inherits System.ServiceProcess.ServiceBase
        Public Property logHandler As EventHandler
            Get
                If Not logHandlerDelegate Is Nothing Then
                    Return logHandlerDelegate
                Else
                    Return Logging.Events.logHandlerDelegate
                End If
            End Get
            Set(value As EventHandler)
                logHandlerDelegate = value
            End Set
        End Property
    End Class

End Namespace
