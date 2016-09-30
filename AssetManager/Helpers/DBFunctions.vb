Option Explicit On
Imports MySql.Data.MySqlClient
Public Module DBFunctions

    Public Const strCommMessage As String = "Communicating..."
    Public Const strLoadingGridMessage As String = "Building Grid..."
    Public Const strCheckOut As String = "OUT"
    Public Const strCheckIn As String = "IN"
    Public strServerTime As String
    Public Function OpenConnections() As Boolean
        Dim Comm As New clsMySQL_Comms
        Try
            If GlobalConn.State <> ConnectionState.Open Then
                Asset.CloseConnection(GlobalConn)
                GlobalConn = Comm.NewConnection
            End If
            GlobalConn.Open()
            If GlobalConn.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        Catch ex As MySqlException
            Logger("ERROR:  MethodName=" & System.Reflection.MethodInfo.GetCurrentMethod().Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
            Return False
        End Try
    End Function
    Public Function CloseConnections()
        Try
            Asset.CloseConnection(GlobalConn)
            'GlobalConn.Close()
            'GlobalConn.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
        Return True
    End Function

    Public Function GetShortLocation(ByVal index As Integer) As String
        Try
            Return DeviceIndex.Locations(index).strShort
        Catch
            Return ""
        End Try
    End Function

    Public Function GetDBValue(ByVal CodeIndex() As Combo_Data, ByVal index As Integer) As Object
        Try
            If index > -1 Then
                Return CodeIndex(index).strShort
            End If
            Return Nothing
        Catch
            Return Nothing
        End Try
    End Function
    Public Function GetHumanValue(ByVal CodeIndex() As Combo_Data, ByVal ShortVal As String) As String
        For Each Code As Combo_Data In CodeIndex
            If Code.strShort = ShortVal Then Return Code.strLong
        Next
        Return Nothing
    End Function
    Public Function GetHumanValueFromIndex(ByVal CodeIndex() As Combo_Data, index As Integer) As String
        Return CodeIndex(index).strLong
    End Function
    Public Function GetDBValueFromHuman(ByVal CodeIndex() As Combo_Data, ByVal LongVal As String) As String
        For Each i As Combo_Data In CodeIndex
            If i.strLong = LongVal Then Return i.strShort
        Next
        Return Nothing
    End Function
    Public Function GetComboIndexFromShort(ByVal CodeIndex() As Combo_Data, ByVal ShortVal As String) As Integer
        For i As Integer = 0 To UBound(CodeIndex)
            If CodeIndex(i).strShort = ShortVal Then Return i
        Next
        Return Nothing
    End Function
    Public Function Get_MunisCode_From_AssetCode(AssetCode As String) As String
        Return Asset.Get_SQLValue("munis_codes", "asset_man_code", AssetCode, "munis_code")
    End Function
    Public Sub BuildIndexes()
        Logger("Building Indexes...")
        With Asset
            DeviceIndex.Locations = .BuildIndex(Attrib_Table.Device, Attrib_Type.Location)
            DeviceIndex.ChangeType = .BuildIndex(Attrib_Table.Device, Attrib_Type.ChangeType)
            DeviceIndex.EquipType = .BuildIndex(Attrib_Table.Device, Attrib_Type.EquipType)
            DeviceIndex.OSType = .BuildIndex(Attrib_Table.Device, Attrib_Type.OSType)
            DeviceIndex.StatusType = .BuildIndex(Attrib_Table.Device, Attrib_Type.StatusType)
            SibiIndex.StatusType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiStatusType)
            SibiIndex.ItemStatusType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiItemStatusType)
            SibiIndex.RequestType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiRequestType)
            SibiIndex.AttachFolder = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiAttachFolder)
        End With
        Logger("Building Indexes Done...")
    End Sub
    Public Function ConnectionReady() As Boolean
        Select Case GlobalConn.State
            Case ConnectionState.Closed
                Return False
            Case ConnectionState.Open
                Return True
            Case ConnectionState.Connecting
                Return False
            Case Else
                Return False
        End Select
    End Function


End Module
