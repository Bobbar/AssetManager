Imports MySql.Data.MySqlClient
Public Module MyStructures
    Public Structure ConnectionData
        Public DBConnection As MySqlConnection
        Public ConnectionID As String
    End Structure
    Public Structure Combo_Data
        Public strLong As String
        Public strShort As String
        Public strID As String
    End Structure
    Public Structure Device_Info
        Public strAssetTag As String
        Public strDescription As String
        Public strEqType As String
        Public strSerial As String
        Public strLocation As String
        Public strCurrentUser As String
        Public strFiscalYear As String
        Public dtPurchaseDate As Date 'As Object
        Public strReplaceYear As String
        Public strOSVersion As String
        Public strGUID As String
        Public strPO As String
        Public strStatus As String
        Public strNote As String
        Public bolTrackable As Boolean
        Public strSibiLink As String
        Public Tracking As Track_Info
        Public Historical As Hist_Info
    End Structure
    Public Structure Request_Info
        Public strUID As String
        Public strUser As String
        Public strDescription As String
        Public dtDateStamp As Object
        Public dtNeedBy As Object
        Public strStatus As String
        Public strType As String
        Public strPO As String
        Public strRequisitionNumber As String
        Public strReplaceAsset As String
        Public strReplaceSerial As String
        Public strRequestNumber As String
        Public strRTNumber As String
        Public RequstItems As DataTable
    End Structure
    Public Structure Hist_Info
        Public strChangeType As String
        Public strHistUID As String
        Public strNote As String
        Public strActionUser As String
        Public dtActionDateTime As Date
    End Structure
    Public Structure Track_Info
        Public strCheckOutTime As String
        Public strDueBackTime As String
        Public strCheckInTime As String
        Public strCheckOutUser As String
        Public strCheckInUser As String
        Public strUseLocation As String
        Public strUseReason As String
        Public bolCheckedOut As Boolean
    End Structure
    Public Structure Access_Info
        Public strModule As String
        Public intLevel As Integer
        Public strDesc As String
    End Structure
    Public Structure Update_Info
        Public strNote As String
        Public strChangeType As String
    End Structure
    Public Structure User_Info
        Public strUsername As String
        Public strFullname As String
        'Public bolIsAdmin As Boolean
        Public intAccessLevel As Integer
        Public strUID As String
    End Structure
    Public Structure Emp_Info
        Public Number As String
        Public Name As String
        Public UID As String
    End Structure
End Module
