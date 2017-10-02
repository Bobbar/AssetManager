
Public Enum EntryType
    Sibi
    Device
End Enum

Public Enum PdfFormType
    InputForm
    TransferForm
    DisposeForm
End Enum

Public Enum LiveBoxType
    DynamicSearch
    InstaLoad
    SelectValue
    UserSelect
End Enum

Public Enum FindDevType
    AssetTag
    Serial
End Enum

Public Enum CommandArgs
    TESTDB
End Enum

Public Enum Databases
    test_db
    asset_manager
End Enum

Public Class Subnets

    Private subnetList As New List(Of SubnetInfo)() From
        {New SubnetInfo("10.10.80.0", "Admin"),
        New SubnetInfo("10.10.81.0", "OC"),
        New SubnetInfo("10.10.82.0", "DiscoverU"),
        New SubnetInfo("10.10.83.0", "FRS"),
        New SubnetInfo("10.10.84.0", "PRO"),
        New SubnetInfo("10.10.85.0", "Art & Clay")}

    Public Shared ReadOnly Property IPtoSubnet(ip As String) As String
        Get
            Dim subs As New Subnets
            Dim subnetString = ip.Substring(0, 8) & ".0"
            Return subs.subnetList.Find(Function(i) i.IP = subnetString).SubnetName
        End Get
    End Property

    Private Structure SubnetInfo
        Public IP As String
        Public SubnetName As String
        Sub New(ip As String, subnetName As String)
            Me.IP = ip
            Me.SubnetName = subnetName
        End Sub
    End Structure
End Class
