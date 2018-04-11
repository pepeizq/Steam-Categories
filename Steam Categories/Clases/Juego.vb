Public Class Juego

    Public Property Titulo As String
    Public Property Imagen As String
    Public Property ID As Integer
    Public Property Userscore As String
    Public Property Metascore As String
    Public Property Años As List(Of Categoria)
    Public Property Categorias As List(Of Categoria)
    Public Property Generos As List(Of Categoria)
    Public Property Tags As List(Of Categoria)
    Public Property Idiomas As List(Of Categoria)

    Public Sub New(ByVal titulo As String, ByVal imagen As String, ByVal id As Integer, ByVal userscore As String, ByVal metascore As String,
                   ByVal años As List(Of Categoria), ByVal categorias As List(Of Categoria), ByVal generos As List(Of Categoria),
                   ByVal tags As List(Of Categoria), ByVal idiomas As List(Of Categoria))
        Me.Titulo = titulo
        Me.Imagen = imagen
        Me.ID = id
        Me.Userscore = userscore
        Me.Metascore = metascore
        Me.Años = años
        Me.Categorias = categorias
        Me.Generos = generos
        Me.Tags = tags
        Me.Idiomas = idiomas
    End Sub

End Class
