Namespace ServerInfo
    Module ServerInfoDeclarations
        Public ServerPinging As Boolean = False
        Public Const MySQLServerIP As String = "10.10.0.89"
        Private _currentDataBase As Databases = Databases.asset_manager

        Public Property CurrentDataBase As Databases
            Get
                Return _currentDataBase
            End Get
            Set(value As Databases)
                _currentDataBase = value
                NetworkInfo.SetCurrentDomain(_currentDataBase)
            End Set
        End Property

    End Module
End Namespace