Public Class ProgressCounter

#Region "Fields"

    Private _currentTick As Integer
    Private _progBytesMoved As Integer
    Private _progTotalBytes As Integer
    Private _speedBytesMoved As Integer
    Private _speedThroughput As Double
    Private _startTick As Integer

#End Region

#Region "Constructors"

    Sub New()
        _progBytesMoved = 0
        _progTotalBytes = 0
        _speedBytesMoved = 0
        _currentTick = 0
        _startTick = 0
        _speedThroughput = 0
    End Sub

#End Region

#Region "Properties"

    Public Property BytesMoved As Integer
        Get
            Return _progBytesMoved
        End Get
        Set(value As Integer)
            _speedBytesMoved += value
            _progBytesMoved += value
        End Set
    End Property

    Public Property BytesToTransfer As Integer
        Get
            Return _progTotalBytes
        End Get
        Set(value As Integer)
            _progTotalBytes = value
        End Set
    End Property

    Public ReadOnly Property Percent As Integer
        Get
            If _progTotalBytes > 0 Then
                Return CInt((_progBytesMoved / _progTotalBytes) * 100)
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property Throughput As Double
        Get
            Return _speedThroughput
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub ResetProgress()
        _progBytesMoved = 0
    End Sub

    Public Sub Tick()
        _currentTick = Environment.TickCount
        If _startTick > 0 Then
            If _speedBytesMoved > 0 Then
                Dim elapTime = _currentTick - _startTick
                _speedThroughput = Math.Round((_speedBytesMoved / elapTime) / 1000, 2)
            End If
        Else
            _startTick = _currentTick
        End If
    End Sub

#End Region

End Class