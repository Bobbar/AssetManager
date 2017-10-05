Imports System.DirectoryServices
Public Class ActiveDirectoryWrapper
    Private _hostname As String
    Private _searchResults As SearchResult
    ' Public Property FoundInAD As Boolean = False
    Sub New(hostname As String)
        _hostname = hostname
    End Sub

    ''' <summary>
    ''' Executes the Active Directory search and populates search results. Returns true if results were found.
    ''' </summary>
    ''' <returns></returns>
    Public Function LoadResults() As Boolean
        _searchResults = ReturnSearchResult()
        If _searchResults IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Asynchronously executes the Active Directory search and populates search results. Returns true if results were found.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function LoadResultsAsync() As Task(Of Boolean)
        Return Await Task.Run(Function()
                                  _searchResults = ReturnSearchResult()
                                  If _searchResults IsNot Nothing Then
                                      Return True
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    ''' <summary>
    ''' Get the OU path of a domain computer. 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDeviceOU() As String
        Return ParsePath(GetDistinguishedName())
    End Function

    ''' <summary>
    ''' Queries Active Directory for a hostname and returns the Distinguished Name.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDistinguishedName() As String
        Dim directorySearchResult = _searchResults
        If directorySearchResult IsNot Nothing Then
            If directorySearchResult.Properties("distinguishedName").Count > 0 Then
                Return directorySearchResult.Properties("distinguishedName")(0).ToString()
            End If
        End If
        Return String.Empty
    End Function

    Private Function ReturnSearchResult() As SearchResult
        Try
            Using rootDSE = New DirectoryEntry("LDAP://" & NetworkInfo.CurrentDomain & "/RootDSE")
                If ServerInfo.CurrentDataBase = Databases.vintondd Then
                    SecurityTools.VerifyAdminCreds("Credentials for Vinton AD")
                    rootDSE.Username = SecurityTools.AdminCreds.UserName
                    rootDSE.Password = SecurityTools.AdminCreds.Password
                End If

                Dim defaultNamingContext = rootDSE.Properties("defaultNamingContext").Value.ToString()
                Dim domainRootADsPath = [String].Format("LDAP://" & NetworkInfo.CurrentDomain & "/{0}", defaultNamingContext)

                Using searchRoot = New DirectoryEntry(domainRootADsPath)
                    If ServerInfo.CurrentDataBase = Databases.vintondd Then
                        searchRoot.Username = SecurityTools.AdminCreds.UserName
                        searchRoot.Password = SecurityTools.AdminCreds.Password
                    End If
                    Dim filter = "(&(objectCategory=computer)(name=" + _hostname + "))"
                    Using directorySearch = New DirectorySearcher(searchRoot, filter)
                        Dim directorySearchResult = directorySearch.FindOne()
                        If directorySearchResult IsNot Nothing Then
                            Return directorySearch.FindOne()
                        End If
                        Return Nothing
                    End Using
                End Using
            End Using
        Catch
            Return Nothing
        End Try
    End Function

    Public Function GetListOfAttribsAndValues() As List(Of String)
        Dim directorySearchResult = _searchResults
        Dim AttribList As New List(Of String)
        For i = 0 To directorySearchResult.Properties.Count - 1
            AttribList.Add(directorySearchResult.Properties.PropertyNames(i).ToString & " = " & directorySearchResult.Properties(directorySearchResult.Properties.PropertyNames(i).ToString)(0).ToString) ' & directorySearchResult.Properties.Values.)
        Next
        Return AttribList
    End Function

    Public Function GetAttributeValue(attribName As String) As String
        Dim directorySearchResult = _searchResults
        If directorySearchResult IsNot Nothing Then
            If directorySearchResult.Properties(attribName).Count > 0 Then
                Return AttribValueToString(directorySearchResult.Properties(attribName)(0))
            End If
        End If
        Return String.Empty
    End Function

    Private Function AttribValueToString(value As Object) As String
        Try
            Select Case value.GetType
                Case GetType(Int64)
                    Dim longTime = CType(value, Int64)
                    Return Date.FromFileTime(longTime).ToString
                Case Else
                    Return value.ToString
            End Select
        Catch ex As ArgumentOutOfRangeException
            Return value.ToString
        End Try
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
