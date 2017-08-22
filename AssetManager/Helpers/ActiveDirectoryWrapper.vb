Imports System.DirectoryServices
Public Class ActiveDirectoryWrapper

    ''' <summary>
    ''' Asynchronously get the OU path of a domain computer. 
    ''' </summary>
    ''' <param name="hostname"></param>
    ''' <returns></returns>
    Public Async Function GetDeviceOU(hostname As String) As Task(Of String)
        Return Await Task.Run(Function()
                                  Return ParsePath(GetDistinguishedName(hostname))
                              End Function)
    End Function

    ''' <summary>
    ''' Queries Active Directory for a hostname and returns the Distinguished Name.
    ''' </summary>
    ''' <param name="hostname"></param>
    ''' <returns></returns>
    Public Function GetDistinguishedName(hostname As String) As String
        Using rootDSE = New DirectoryEntry("LDAP://RootDSE")
            Dim defaultNamingContext = rootDSE.Properties("defaultNamingContext").Value.ToString()
            Dim domainRootADsPath = [String].Format("LDAP://{0}", defaultNamingContext)
            Using searchRoot = New DirectoryEntry(domainRootADsPath)
                Dim filter = "(&(objectCategory=computer)(name=" + hostname + "))"
                Using directorySearch = New DirectorySearcher(searchRoot, filter)
                    Dim directorySearchResult = directorySearch.FindOne()
                    If directorySearchResult IsNot Nothing Then
                        If directorySearchResult.Properties("distinguishedName").Count > 0 Then
                            Return directorySearchResult.Properties("distinguishedName")(0).ToString()
                        End If
                    End If
                    Return String.Empty
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Parses a Distinguished Name string into a shorter more readable string.
    ''' </summary>
    ''' <param name="distName"></param>
    ''' <returns></returns>
    Private Function ParsePath(distName As String) As String
        'Replace unwanted OU= text and split the string by commas.
        Dim Elements As List(Of String) = Split(Replace(distName, "OU=", ""), ",").ToList
        'OU Name of the topmost desired OU.
        Dim RootOUName As String = "FCAccounts"
        'Find the index of the RootOUName using a lambda expression.
        Dim RootIndex = Elements.FindIndex(Function(e) e.Contains(RootOUName))
        Dim Path As String = ""
        'Iterate through the elements starting at 1 to skip the device name, stop at the RootIndex. 
        For i = 1 To RootIndex
            'Concant the user friendly path string.
            Path += Elements(i) & "/"
        Next
        Return Path
    End Function

End Class
