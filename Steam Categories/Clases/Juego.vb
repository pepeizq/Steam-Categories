Public Class Juego

    Public Property Titulo As String
    Public Property ID As Integer
    Public Property Userscore As String
    Public Property Metascore As String
    Public Property Año As String
    Public Property Categorias As List(Of String)
    Public Property Generos As List(Of String)
    Public Property Tags As List(Of String)
    Public Property Idiomas As List(Of String)

    Public Sub New(ByVal titulo As String, ByVal id As Integer, ByVal userscore As String, ByVal metascore As String,
                   ByVal año As String, ByVal categorias As List(Of String), ByVal generos As List(Of String),
                   ByVal tags As List(Of String), ByVal idiomas As List(Of String))
        Me.Titulo = titulo
        Me.ID = id
        Me.Userscore = userscore
        Me.Metascore = metascore
        Me.Año = año
        Me.Categorias = categorias
        Me.Generos = generos
        Me.Tags = tags
        Me.Idiomas = idiomas
    End Sub

End Class
