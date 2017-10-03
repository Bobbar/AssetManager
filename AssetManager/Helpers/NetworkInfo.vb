Namespace NetworkInfo
    Public Module Subnet
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
    End Module
End Namespace