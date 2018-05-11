Imports System.IO
Module UpdateFotos

    Sub Main()
        UpdateFiles("*.JPG")
        UpdateFiles("*.AVI")

    End Sub
    Private Sub UpdateFiles(ByVal sFilter As String)
        Try
            Dim SearchDI As New DirectoryInfo("F:\Minolta\Fotos")
            Dim di As New DirectoryInfo("F:\Minolta\fotos\transfer")
            Dim fInfo As FileInfo() = di.GetFiles(sFilter)
            For Each fi As FileInfo In fInfo
                Dim sOutName As String = ""
                If SearchRekFile(SearchDI, fi.Name, sOutName) Then
                    Console.WriteLine("File found: " & sOutName)
                Else
                    Dim dt As Date = fi.LastWriteTime
                    Dim sNewPath As String = "F:\Minolta\fotos\" & dt.Year & "\" & Format(dt, "MMMM") & "\" & dt.Day
                    Console.WriteLine("Moving File to: " & sNewPath & "\" & fi.Name)

                    If Not Directory.Exists(sNewPath) Then
                        Directory.CreateDirectory(sNewPath)
                    End If
                    If Not File.Exists(sNewPath & "\" & fi.Name) Then
                        fi.MoveTo(sNewPath & "\" & fi.Name)
                    End If
                End If

            Next
        Catch ex As Exception
            MsgBox(ex.StackTrace)
        End Try

    End Sub
    Private Function SearchRekFile(ByRef di As DirectoryInfo, ByVal FileName As String, ByRef OutName As String) As Boolean
        Dim fInfo As FileInfo() = di.GetFiles()
        For Each fi As FileInfo In fInfo
            If String.Compare(fi.Name, FileName, True) = 0 Then
                ' found
                OutName = di.FullName & "\" & fi.Name
                Return True
            End If
        Next
        For Each di2 As DirectoryInfo In di.GetDirectories()
            If SearchRekFile(di2, FileName, OutName) Then
                Return True
            End If
        Next
    End Function

End Module
