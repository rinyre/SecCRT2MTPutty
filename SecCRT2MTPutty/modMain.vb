Imports System
Imports System.IO
Imports System.Xml
'Imports org.jivesoftware.util

Module ImportSecureCRT2MTPutty

    Class Server
        'Creating servers as a datastruct for simplicity of storage/retrieval
        Public Sub New(ByVal DisplayName As String, ByVal ServerName As String, ByVal Port As Integer, ByVal UserName As String)
            Me.DisplayName = DisplayName
            Me.ServerName = ServerName
            Me.Port = Port
            Me.UserName = UserName
        End Sub
        Public DisplayName As String
        Public ServerName As String
        Public Port As Integer
        Public UserName As String
    End Class

    Function ExpVar(ByVal Path As String)
        'Just to shorten for typing
        Return Environment.ExpandEnvironmentVariables(Path)
    End Function

    'Function decrypt(ByVal encpass As String) ' Unused presently
    '    '' Unused. Taken from Python code at http://www.synacktiv.com/ressources/VanDyke_SecureCRT_decrypt.py
    '    'from Crypto.Cipher import Blowfish
    '    '        import argparse
    '    '        import re

    '    '        def decrypt(password)
    '    '	c1 = Blowfish.new('5F B0 45 A2 94 17 D9 16 C6 C6 A2 FF 06 41 82 B7'.replace(' ','').decode('hex'), Blowfish.MODE_CBC, '\x00'*8)
    '    '	c2 = Blowfish.new('24 A6 3D DE 5B D3 B3 82 9C 7E 06 F4 08 16 AA 07'.replace(' ','').decode('hex'), Blowfish.MODE_CBC, '\x00'*8)
    '    '	padded = c1.decrypt(c2.decrypt(password.decode('hex'))[4:-4])
    '    '	p = ''
    '    '	while padded[:2] != '\x00\x00' :
    '    '		p += padded[:2]
    '    '		padded = padded[2:]
    '    '	return p.decode('UTF-16')
    '    'REGEX_PASWORD = re.compile(ur'S:"Password"=u([0-9a-f]+)')
    '    'def password(x) :
    '    '    m = REGEX_PASWORD.search(x)
    '    '    If m Then
    '    '        Return decrypt(m.group(1))
    '    '    Return '???'   

    '    Dim hexkey1 As Array = "5F B0 45 A2 94 17 D9 16 C6 C6 A2 FF 06 41 82 B7".Split(" ")
    '    Dim hexkey2 As Array = "24 A6 3D DE 5B D3 B3 82 9C 7E 06 F4 08 16 AA 07".Split(" ")
    '    Dim key1 = ""
    '    Dim key2 = ""
    '    For Each keypart In hexkey1
    '        key1 += Convert.ToChar(Convert.ToInt16(keypart, 16))
    '    Next
    '    For Each keypart In hexkey2
    '        key2 += Convert.ToChar(Convert.ToInt16(keypart, 16))
    '    Next
    '    Dim c1 As Blowfish = New Blowfish(key1)
    '    Dim c2 As Blowfish = New Blowfish(key2)
    '    Dim step1 = c2.decryptString(encpass)
    '    Dim padded = c1.decryptString(step1.Substring(4, step1.Length - 5))
    '    c1.hashCode()
    '    Dim p = ""
    '    While padded.Substring(0, 2) IsNot vbNullChar + vbNullChar
    '        p += padded.Substring(0, 2)
    '        padded = padded.Substring(2)
    '    End While
    '    Return Convert.ToString(p.Cast(Of String))
    'End Function

    Sub Main()
        Dim inipath As String = ExpVar("%AppData%\VanDyke\Config\Sessions")
        ' Import items
        Dim di As New DirectoryInfo(inipath)
        Dim fiArr As FileInfo() = di.GetFiles()
        Dim fri As FileInfo
        Dim Servers As New List(Of Server)


        For Each fri In fiArr
            If (Not fri.Name = "__FolderData__.ini") And fri.Extension.ToLower = ".ini" Then
                Dim sr As StreamReader = New StreamReader(fri.FullName)
                Dim strLine As String = String.Empty
                Dim Password As String = ""
                Try
                    Dim Hostname As String = ""
                    Dim Username As String = ""
                    Dim Port As Integer = 22

                    Dim Displayname As String = fri.Name().Substring(0, fri.Name.IndexOf(".ini"))
                    Do While sr.Peek() >= 0
                        strLine = String.Empty
                        strLine = sr.ReadLine

                        If strLine.Contains("""Hostname""") Then
                            Hostname = strLine.Substring(strLine.IndexOf("=") + 1).Replace(vbNewLine, "")
                        ElseIf strLine.Contains("""Username""") Then
                            Username = strLine.Substring(strLine.IndexOf("=") + 1).Replace(vbNewLine, "")
                        ElseIf strLine.Contains("""[SSH2] Port""") Then
                            Port = Convert.ToInt32(strLine.Substring(strLine.IndexOf("=") + 1).Replace(vbNewLine, ""), 16)
                            'ElseIf strLine.Contains("S:""Password""") Then
                            '    Password = strLine.Substring(strLine.IndexOf("=") + 2).Replace(vbNewLine, "")
                        End If
                    Loop
                    If Hostname IsNot "" Then
                        Console.WriteLine("Processed " + fri.Name + " for username " + Username)
                        Servers.Add(New Server(Displayname, Hostname, Port, Username))
                    End If
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)

                End Try
                'If Password IsNot "" Then
                '    Debug.WriteLine(decrypt(Password))
                'End If
            End If
        Next

        Directory.CreateDirectory(ExpVar("%AppData%\TTYPlus"))
        Dim OutXMLFile As String = ExpVar("%AppData%\TTYPlus\mtputty.xml")
        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        Using writer As XmlWriter = XmlWriter.Create(OutXMLFile, settings)
            writer.WriteStartDocument()
            writer.WriteStartElement("MTPutty")
            writer.WriteAttributeString("version", "1.0")
            writer.WriteStartElement("Servers")
            writer.WriteStartElement("Putty")
            writer.WriteStartElement("Node")
            writer.WriteAttributeString("Type", "0")
            writer.WriteAttributeString("Expanded", "1")
            writer.WriteElementString("DisplayName", "Imported SecureCRT Sessions")
            Dim server As Server
            For Each server In Servers
                writer.WriteStartElement("Node")
                writer.WriteAttributeString("Type", "1")
                writer.WriteElementString("SavedSession", "Default Settings")
                writer.WriteElementString("DisplayName", server.DisplayName.ToString)
                writer.WriteElementString("ServerName", server.ServerName.ToString)
                writer.WriteElementString("PuttyConType", 4)
                writer.WriteElementString("Port", server.Port)
                writer.WriteElementString("UserName", server.UserName.ToString)
                writer.WriteElementString("Password", "")
                writer.WriteElementString("PasswordDelay", 5000)
                writer.WriteElementString("CLParams", server.ServerName.ToString + " -ssh -l " + server.UserName.ToString + " -P " + server.Port.ToString)
                writer.WriteElementString("ScriptDelay", 5000)
                writer.WriteEndElement()
                Console.WriteLine("Created node for " + server.DisplayName.ToString)
            Next
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndDocument()

        End Using
        Console.WriteLine("Finished!")

    End Sub

End Module
