''' <summary>
''' Data mapper for classes tagged with <see cref="DataColumnNameAttribute"/>
''' </summary>
Public Class DataMapping

    Public Sub MapClassProperties(obj As Object, data As Object)
        If TypeOf data Is DataTable Then
            Dim row = DirectCast(data, DataTable).Rows(0)
            MapProperty(obj, row)
        ElseIf TypeOf data Is DataRow Then
            MapProperty(obj, DirectCast(data, DataRow))
        Else
            Throw New Exception("Invalid data object type.")
        End If
    End Sub

    Public Sub MapClassProperties(obj As Object, row As DataRow)
        MapProperty(obj, row)
    End Sub

    Public Sub MapClassProperties(obj As Object, data As DataTable)
        Dim row = data.Rows(0)
        MapProperty(obj, row)
    End Sub
    ''' <summary>
    ''' Uses reflection to recursively populate/map class properties that are marked with a <see cref="DataColumnNameAttribute"/>.
    ''' </summary>
    ''' <param name="obj">Object to be populated.</param>
    ''' <param name="row">DataRow with columns matching the <see cref="DataColumnNameAttribute"/> in the objects properties.</param>
    Private Sub MapProperty(obj As Object, row As DataRow)
        'Collect list of all properties in the object class.
        Dim Props As List(Of Reflection.PropertyInfo) = (obj.GetType.GetProperties().ToList)

        'Iterate through the properties.
        For Each prop In Props

            'Check if the property contains a target attribute.
            If prop.GetCustomAttributes(GetType(DataColumnNameAttribute), True).Length > 0 Then

                'Get the column name attached to the property.
                Dim propColumn = DirectCast(prop.GetCustomAttributes(False)(0), DataColumnNameAttribute).ColumnName

                'Make sure the DataTable contains a matching column name.
                If row.Table.Columns.Contains(propColumn) Then

                    'Check the type of the propery and set its value accordingly.
                    Select Case prop.PropertyType
                        Case GetType(String)
                            prop.SetValue(obj, row(propColumn).ToString, Nothing)

                        Case GetType(DateTime)
                            Dim pDate As DateTime
                            If DateTime.TryParse(NoNull(row(propColumn).ToString), pDate) Then
                                prop.SetValue(obj, pDate)
                            Else
                                prop.SetValue(obj, Nothing)
                            End If

                        Case GetType(Boolean)
                            prop.SetValue(obj, CBool(row(propColumn)))

                        Case Else
                            'Throw an error if type is unexpected.
                            Debug.Print(prop.PropertyType.ToString)
                            Throw New Exception("Unexpected property type.")
                    End Select
                End If

            Else 'If the property does not contain a target attribute, check to see if it is a nested class inheriting the DataStructure base class.

                If GetType(DataMapping).IsAssignableFrom(prop.PropertyType) Then
                    'Recurse with nested DataStructure properties.
                    MapProperty(prop.GetValue(obj, Nothing), row)
                End If
            End If
        Next
    End Sub

End Class
