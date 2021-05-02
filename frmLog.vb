Imports System.IO
Imports System.Windows.Forms

Public Class frmLog

    Private _Root As DirectoryInfo = Nothing
    Public Property Root As DirectoryInfo
        Get
            Return _Root
        End Get
        Set(value As DirectoryInfo)
            _Root = value
        End Set
    End Property
    Private Sub frmLog_Load(sender As Object, e As EventArgs) Handles Me.Load
        With RichTextBox1
            .Focus()

        End With
    End Sub

#Region "Date/Folder properties"

    Public Property [Date] As Date
        Get
            Return DateTimePicker1.Value
        End Get
        Set(value As Date)
            DateTimePicker1.Value = value
        End Set
    End Property

    Private ReadOnly Property Folder As DirectoryInfo
        Get
            With DateTimePicker1.Value
                Return New DirectoryInfo(
                    Path.Combine(
                       _Root.FullName,
                        .ToString("yyyy-MM")
                    )
                )
            End With
        End Get
    End Property

    Private ReadOnly Property Filename As FileInfo
        Get
            With DateTimePicker1.Value
                Return New FileInfo(
                    Path.Combine(
                        Folder.FullName,
                        String.Format(
                            "{0}.txt",
                            .ToString("yyMMdd")
                        )
                    )
                )
            End With
        End Get

    End Property

#End Region

#Region "File Watcher"

    Private Sub FileSystemWatcher1_Changed(sender As Object, e As FileSystemEventArgs) Handles FileSystemWatcher1.Changed

        If String.Compare(e.FullPath, Filename.FullName, True) = 0 Then

            Dim l As Integer = 0
            Dim f As Boolean = False

            Do
                Try
                    Using SR As New StreamReader(Filename.FullName)
                        While Not SR.EndOfStream
                            Dim str = SR.ReadLine
                            With RichTextBox1
                                If l >= UBound(.Lines) Then
                                    .AppendText(str & Environment.NewLine)
                                    .SelectionStart = .Text.Length
                                    .ScrollToCaret()

                                End If
                            End With

                            l += 1
                        End While

                    End Using
                    f = True

                Catch ex As Exception
                    Threading.Thread.Sleep(100)
                    f = False

                End Try

            Loop Until f

        End If

    End Sub

    Private Sub FileSystemWatcher1_Created(sender As Object, e As FileSystemEventArgs) Handles FileSystemWatcher1.Created

        With RichTextBox1
            If String.Compare(e.FullPath, Filename.FullName, True) = 0 Then
                readFile()
                .SelectionStart = .Text.Length
                .ScrollToCaret()

            End If
        End With

    End Sub

#End Region

#Region "Controls"

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged

        If Not Root Is Nothing Then

            With FileSystemWatcher1
                .EnableRaisingEvents = True
                .Path = Filename.Directory.FullName
                .IncludeSubdirectories = False
            End With

            With RichTextBox1
                If Filename.Exists Then
                    Dim l As Integer = 0
                    readFile()

                Else
                    .Clear()

                End If

                .SelectionStart = .Text.Length
                .ScrollToCaret()
                .Focus()

            End With

        End If

    End Sub

#End Region

#Region "Search"

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        If SearchText.Text.Length > 0 Then

            With RichTextBox1

                Dim ind As Integer
                Dim s As Integer = .SelectionStart
                Dim l As Integer = .SelectionLength

                .SelectionStart = .SelectionStart + .SelectionLength
                Dim finds As New RichTextBoxFinds
                ind = .Find(SearchText.Text, .SelectionStart, finds)

                If ind >= 0 Then
                    .SelectionStart = ind
                    .SelectionLength = SearchText.Text.Length

                Else
                    .SelectionStart = .Text.Length
                    .SelectionLength = 0

                End If

                .ScrollToCaret()
                .Focus()

            End With

        End If

    End Sub

    Private Sub RichTextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles RichTextBox1.KeyUp, Me.KeyUp, DateTimePicker1.KeyUp
        If e.KeyData = Keys.F Or e.Control Then
            SearchText.Focus()

        ElseIf e.KeyData = Keys.F3 Then
            btnFind_Click(Me, New EventArgs)

        End If

    End Sub

    Private Sub SearchText_KeyDown(sender As Object, e As KeyEventArgs) Handles SearchText.KeyDown
        Select Case e.KeyData
            Case Keys.Enter
                btnFind_Click(Me, New EventArgs)

        End Select

    End Sub

#End Region

    Sub readFile()

        Dim f As Boolean = False
        With RichTextBox1
            Do
                Try
                    Using SR As New StreamReader(Filename.FullName)
                        .Text = SR.ReadToEnd

                    End Using
                    f = False

                Catch ex As Exception
                    Threading.Thread.Sleep(100)
                    f = True

                End Try
            Loop Until Not f

        End With

    End Sub

    Private Sub frmLog_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        With Me.RichTextBox1
            .Width = (Me.Width - 3)
            .Height = Me.Height - 40
        End With

    End Sub
End Class
