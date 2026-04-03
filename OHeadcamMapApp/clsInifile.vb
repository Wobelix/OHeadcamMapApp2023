Imports System.Text
Public Class clsInifile

    Public IniFolder As String = ""
    Public inifile As String
    Public Language As String
    Public Encoding_local As Encoding


    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String,
                ByVal lpKeyName As String,
                ByVal lpDefault As String,
                ByVal lpReturnedString As StringBuilder,
                ByVal nSize As Integer,
                ByVal lpFileName As String) As Integer
    Private Declare Auto Function WritePrivateProfileString Lib "kernel32" (ByVal lpAppName As String, ByVal lpKeyName As String,
                                                                            ByVal lpString As String, ByVal lpFileName As String) As Boolean
    Public Sub SetINIfile(FilenameWithPath As String)
        inifile = FilenameWithPath
    End Sub
    Public Function getINItext(key As String) As String
        Dim fileReader, tmp As String, i1 As Integer
        'SetINI = WritePrivateProfileString(Section, Key, Value, inifile)
        'If Section = "" Then
        fileReader = My.Computer.FileSystem.ReadAllText(inifile, Encoding_local)
        tmp = fileReader.Substring(fileReader.IndexOf(key))
        i1 = tmp.IndexOf(vbCrLf)
        getINItext = tmp.Substring(0, i1)
    End Function
    Public Function SetINI(ByVal Key As String, ByVal Value As String) As Boolean
        Dim fileReader, tmp, tmp2 As String
        'SetINI = WritePrivateProfileString(Section, Key, Value, inifile)
        'If Section = "" Then
        fileReader = My.Computer.FileSystem.ReadAllText(inifile, Encoding_local)
        tmp = Key + " = " + Value
        tmp2 = fileReader.Replace(getINItext(Key), tmp)

        My.Computer.FileSystem.WriteAllText(inifile, tmp2, False, Encoding_local)

        Return True
        'End If
    End Function
    Public Function GetINI(ByVal Key As String) As String
        Dim txt As String, i As Integer
        txt = getINItext(Key)
        i = txt.IndexOf("=") + 1
        GetINI = txt.Substring(i).Trim

        'Dim strbTmp As StringBuilder = New StringBuilder(255)
        'Dim fileReader As String, res As Boolean
        'If Section = "" Then
        'fileReader = "[]" + vbCrLf + My.Computer.FileSystem.ReadAllText(inifile)
        'My.Computer.FileSystem.WriteAllText(inifile, fileReader, False)
        'End If
        'GetPrivateProfileString(Section, Key, "", strbTmp, strbTmp.Capacity, inifile)
        'GetINI = strbTmp.ToString()
        'If Section = "" Then
        ' fileReader = My.Computer.FileSystem.ReadAllText(inifile).Replace("[]" + vbCrLf, "")
        ' My.Computer.FileSystem.WriteAllText(inifile, fileReader, False)
        ' End If
    End Function
    Public Function Create_INIFile(ByVal strPath As String, ByVal strFileName As String) As Boolean
        If Dir(strPath & "\" & strFileName) <> "" Then
            Return False
        End If

        Try

            Using sw As System.IO.StreamWriter = New System.IO.StreamWriter(strPath & "\" & strFileName, False, Encoding_local)
                sw.WriteLine(vbCrLf)
                sw.Flush()
                sw.Close()
            End Using


        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Sub SetLanguage(lang As String)
        Language = lang

        Encoding_local = New UTF8Encoding(False)

    End Sub



End Class
