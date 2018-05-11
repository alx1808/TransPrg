Imports System.IO
Module Transit
    'Private Const DRIVESTR As String = "\\joachim01\drive_f"
    'Private Const DRIVESTR As String = "f:"
    Private Const CAMERA As String = "E:\BilderGesamt"
    Private Const CAMERA_VIA_DATE As String = "E:\Bilder_Nach_Datum"
    Private Const LOG_FILE_NAME As String = "E:\Bilder_Nach_Datum\Transit.log"

    'Private Const CAMERA As String = "C:\Users\a.ausweger\Documents\Div\FairPhone\Camera"
    'Private Const CAMERA_VIA_DATE As String = "C:\Users\a.ausweger\Documents\Div\FairPhone\Camera_Date"
    'Private Const LOG_FILE_NAME As String = "C:\Users\a.ausweger\Documents\Div\FairPhone\Transit.log"



    Sub Main()

        Dim fs As System.IO.FileStream = Nothing
        Dim w As StreamWriter = Nothing
        Try
            fs = New System.IO.FileStream(LOG_FILE_NAME, FileMode.OpenOrCreate, FileAccess.ReadWrite)
            w = New StreamWriter(fs) ' create a stream writer 
            w.BaseStream.Seek(0, SeekOrigin.End) ' set the file pointer to the end of file 
            w.Write("Log Entry : {0}", vbCrLf)
            w.WriteLine("{0} {1}{2}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), vbCrLf)

            MoveFiles("*.JPG", w)
            MoveFiles("*.AVI", w)
            MoveFiles("*.MOV", w)
            MoveFiles("*.MP4", w)

        Catch ex As Exception
            Console.Write(ex.Message & ", " & ex.StackTrace)
            w.Write(ex.Message & ", " & ex.StackTrace)
        Finally
            If Not w Is Nothing Then
                w.Flush()
                w.Close()
            End If
            If Not fs Is Nothing Then
                fs.Close()
            End If
        End Try
    End Sub
    Private Sub MoveFiles(ByVal sFilter As String, ByVal w As StreamWriter)
        Dim di As New DirectoryInfo(CAMERA)
        Dim fInfo As FileInfo() = di.GetFiles(sFilter)
        For Each fi As FileInfo In fInfo
            Dim dt As Date = fi.LastWriteTime



            Dim sNewPath As String = CAMERA_VIA_DATE & "\" & dt.Year & "\" & Format(dt, "MMMM") & "\" & dt.Day
            Console.WriteLine("Moving File to: " & sNewPath & "\" & fi.Name)

            If Not Directory.Exists(sNewPath) Then
                w.WriteLine("Creating Path {0}.", sNewPath)
                Directory.CreateDirectory(sNewPath)
            End If
            If File.Exists(sNewPath & "\" & fi.Name) Then
                w.WriteLine("File {0}\{1} already exists.", sNewPath, fi.Name)
            Else
                w.WriteLine("Moving File {0}\{1}.", sNewPath, fi.Name)
                fi.MoveTo(sNewPath & "\" & fi.Name)
            End If
        Next
    End Sub

End Module
