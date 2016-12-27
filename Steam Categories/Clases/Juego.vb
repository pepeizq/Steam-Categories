Public Class Juego

    Private _Titulo As String
    Private _ID As Integer
    Private _Userscore As String
    Private _Metascore As String
    Private _Año As String
    Private _Categorias As List(Of String)
    Private _Generos As List(Of String)
    Private _Tags As List(Of String)
    Private _Idiomas As List(Of String)

    Public Property Titulo() As String
        Get
            Return _Titulo
        End Get
        Set(ByVal valor As String)
            _Titulo = valor
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal valor As Integer)
            _ID = valor
        End Set
    End Property

    Public Property Userscore() As String
        Get
            Return _Userscore
        End Get
        Set(ByVal valor As String)
            _Userscore = valor
        End Set
    End Property

    Public Property Metascore() As String
        Get
            Return _Metascore
        End Get
        Set(ByVal valor As String)
            _Metascore = valor
        End Set
    End Property

    Public Property Año() As String
        Get
            Return _Año
        End Get
        Set(ByVal valor As String)
            _Año = valor
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

    Public Property Generos() As List(Of String)
        Get
            Return _Generos
        End Get
        Set(ByVal valor As List(Of String))
            _Generos = valor
        End Set
    End Property

    Public Property Tags() As List(Of String)
        Get
            Return _Tags
        End Get
        Set(ByVal valor As List(Of String))
            _Tags = valor
        End Set
    End Property

    Public Property Idiomas() As List(Of String)
        Get
            Return _Idiomas
        End Get
        Set(ByVal valor As List(Of String))
            _Idiomas = valor
        End Set
    End Property

    Public Sub New(ByVal titulo As String, ByVal id As Integer, ByVal userscore As String, ByVal metascore As String, ByVal año As String, ByVal categorias As List(Of String), ByVal generos As List(Of String), ByVal tags As List(Of String), ByVal idiomas As List(Of String))
        _Titulo = titulo
        _ID = id
        _Userscore = userscore
        _Metascore = metascore
        _Año = año
        _Categorias = categorias
        _Generos = generos
        _Tags = tags
        _Idiomas = idiomas
    End Sub

End Class
