Option Explicit On
Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Public Module DBFunctions
    Public Const strCommMessage As String = "Communicating..."
    Public Const strLoadingGridMessage As String = "Building Grid..."
    Public Const strCheckOut As String = "OUT"
    Public Const strCheckIn As String = "IN"
    Public strServerTime As String = Now.ToString
    Public bolServerPinging As Boolean = True
    Public Function GetShortLocation(ByVal index As Integer) As String
        Try
            Return DeviceIndex.Locations(index).strShort
        Catch
            Return ""
        End Try
    End Function
    Public Function GetDBValue(ByVal CodeIndex() As Combo_Data, ByVal index As Integer) As String
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
    End Sub
    Public Function NoNull(DBVal As Object) As String
        Try
            Return IIf(IsDBNull(DBVal), "", DBVal.ToString).ToString
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' Trims, removes LF and CR chars and returns a DBNull if string is empty.
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    Public Function CleanDBValue(Value As String) As Object
        Dim CleanString As String = Regex.Replace(Trim(Value), "[/\r?\n|\r]+", String.Empty)
        Return IIf(CleanString = String.Empty, DBNull.Value, CleanString)
    End Function
End Module
