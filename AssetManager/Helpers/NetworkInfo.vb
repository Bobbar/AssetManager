Namespace NetworkInfo

    Public Module NetInfo
        Private _currentDomain As String = "core.co.fairfield.oh.us"
        Public ReadOnly Property CurrentDomain As String
            Get
                Return _currentDomain
            End Get
        End Property

        Private DomainNames As New Dictionary(Of Databases, String)() From
        {
            {Databases.test_db, "core.co.fairfield.oh.us"},
            {Databases.asset_manager, "core.co.fairfield.oh.us"},
            {Databases.vintondd, "vinton.local"}
        }

        Private SubnetLocations As New Dictionary(Of String, String)() From
        {
            {"10.10.80.0", "Admin"},
            {"10.10.81.0", "OC"},
            {"10.10.82.0", "DiscoverU"},
            {"10.10.83.0", "FRS"},
            {"10.10.84.0", "PRO"},
            {"10.10.85.0", "Art & Clay"}
        }

        Public Function LocationOfIP(ip As String) As String
            Dim Subnet = ip.Substring(0, 8) & ".0"
            If SubnetLocations.ContainsKey(Subnet) Then
                Return SubnetLocations(Subnet)
            Else
                Return String.Empty
            End If
        End Function

        Public Function SetCurrentDomain(database As Databases) As String
            _currentDomain = DomainNames(database)
            SecurityTools.ClearAdminCreds()
            If database = Databases.vintondd Then
                SecurityTools.VerifyAdminCreds("Credentials for Vinton AD")
            End If
            Return DomainNames(database)
        End Function

    End Module
End Namespace