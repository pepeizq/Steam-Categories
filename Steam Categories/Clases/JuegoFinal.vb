Public Class JuegoFinal

    Private _ID As Integer
    Private _Categorias As List(Of String)

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal valor As Integer)
            _ID = valor
        End Set
    End Property

    Public Property Categorias() As List(Of String)
        Get
            Return _Categorias
        End Get
        Set(ByVal valor As List(Of String))
            _Categorias = valor
        End Set
    End Property

    Public Sub New(ByVal id As Integer, ByVal categorias As List(Of String))
        _ID = id
        _Categorias = categorias
    End Sub

End Class
