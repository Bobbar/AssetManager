Imports MySql.Data.MySqlClient
Module modDBScript
    Private DeviceList As DataTable
    Private Munis As New clsMunis_Comms
    Private MySQLDB As New clsMySQL_Comms

    Public Sub GetAndSetEmpNums()
        Dim strQRY As String = "SELECT * FROM devices"

        DeviceList = MySQLDB.Return_SQLTable(strQRY)

        For Each r As DataRow In DeviceList.Rows
            Dim EmpInfo As Emp_Info = GetEmpInfo(r.Item("dev_cur_user"))

            Debug.Print(r.Item("dev_cur_user") & " -- " & EmpInfo.Number)

            If EmpInfo.Number <> "" Then
                If Not IsInDB(EmpInfo.Number) Then
                    AddEmp(EmpInfo)

                End If
                Asset.Update_SQLValue("devices", "dev_cur_user_emp_num", EmpInfo.Number, "dev_UID", (r.Item("dev_UID")))
            End If




        Next

        Debug.Print("----------DONE-----------")

    End Sub
    Public Sub SetEmpNames()
        Dim strQRY As String = "SELECT * FROM employees"
        Dim EmpList As DataTable = MySQLDB.Return_SQLTable(strQRY)


        For Each r As DataRow In EmpList.Rows

            Debug.Print(r.Item("emp_name"))
            Asset.Update_SQLValue("devices", "dev_cur_user", r.Item("emp_name"), "dev_cur_user_emp_num", r.Item("emp_number"))


        Next

        Debug.Print("----------DONE-----------")

    End Sub
    Private Function IsInDB(EmpNum As String) As Boolean
        Dim EmpName As String = Asset.Get_SQLValue("employees", "emp_number", EmpNum, "emp_name")
        If EmpName <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub AddEmp(EmpInfo As Emp_Info)
        Dim UID As String = Guid.NewGuid.ToString
        Dim strQRY As String = "INSERT INTO employees
(emp_name,
emp_number,
emp_UID)
VALUES
(@emp_name,
@emp_number,
@emp_UID)"

        Dim cmd As MySqlCommand = MySQLDB.Return_SQLCommand(strQRY)
        cmd.Parameters.AddWithValue("@emp_name", EmpInfo.Name)
        cmd.Parameters.AddWithValue("@emp_number", EmpInfo.Number)
        cmd.Parameters.AddWithValue("@emp_UID", UID)
        cmd.ExecuteNonQuery()
        cmd.Dispose()



    End Sub
    Private Function GetEmpInfo(EmpName As String) As Emp_Info
        Dim tmpInfo As Emp_Info

        Dim SplitName() As String, FirstName As String, LastName As String

        SplitName = Split(EmpName, " ")
        If SplitName.Count > 1 Then 'if there is an expected split of first/last name
            FirstName = UCase(SplitName(0))
            LastName = UCase(SplitName(1))


            Dim strQRY As String = "SELECT TOP 10 a_employee_number,a_name_last,a_name_first FROM pr_employee_master WHERE a_name_last LIKE '" & LastName & "'"

            Dim results As DataTable = Munis.Return_MSSQLTable(strQRY)
            If IsNothing(results) Then Return Nothing

            If results.Rows.Count > 0 Then 'if theres hits with last name

                If results.Rows.Count > 1 Then 'if there MORE THAN ONE HIT

                    For Each r As DataRow In results.Rows 'check for match with first name
                        Dim ChkFirstName As String = r.Item("a_name_first")
                        If ChkFirstName.Contains(FirstName) Then
                            tmpInfo.Name = r.Item("a_name_first") & " " & r.Item("a_name_last")
                            tmpInfo.Number = r.Item("a_employee_number")
                            Return tmpInfo 'r.Item("a_employee_number")
                        End If
                    Next


                    Dim CmbIndex(results.Rows.Count - 1) As Combo_Data 'NO MATCHES on first name, prompt for correct emp

                    For Each r As DataRow In results.Rows


                        CmbIndex(results.Rows.IndexOf(r)).strLong = r.Item("a_name_first") & " " & r.Item("a_name_last")
                        CmbIndex(results.Rows.IndexOf(r)).strShort = r.Item("a_employee_number")


                    Next


                    Dim NewDialog As New MyDialog
                    With NewDialog
                        .Text = "Select User"
                        .AddTextBox("txtInName", "Search Name:", EmpName)
                        .AddComboBox("cmbEmpNum", "Select Emp", CmbIndex)
                        .ShowDialog()
                        If .DialogResult = DialogResult.OK Then
                            tmpInfo.Name = GetHumanValueFromIndex(CmbIndex, NewDialog.GetControlValue("cmbEmpNum"))
                            tmpInfo.Number = GetDBValue(CmbIndex, NewDialog.GetControlValue("cmbEmpNum"))
                            Return tmpInfo 'GetDBValue(CmbIndex, NewDialog.GetControlValue("cmbEmpNum"))
                        End If

                    End With

                Else  'if ONLY ONE result, return emp num

                    tmpInfo.Name = results.Rows(0).Item("a_name_first") & " " & results.Rows(0).Item("a_name_last")
                    tmpInfo.Number = results.Rows(0).Item("a_employee_number")
                    Return tmpInfo 'results.Rows(0).Item("a_employee_number")


                End If





            End If


        End If



        Return Nothing




    End Function





End Module
