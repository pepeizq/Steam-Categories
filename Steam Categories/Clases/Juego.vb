Public Class Juego

    Public Property Titulo As String
    Public Property Imagen As String
    Public Property ID As Integer
    Public Property Userscore As Categoria
    Public Property CbUserscore As CheckBox
    Public Property Categorias As List(Of Categoria)
    Public Property CbCategorias As List(Of ToggleMenuFlyoutItem)
    Public Property Generos As List(Of Categoria)
    Public Property CbGeneros As List(Of ToggleMenuFlyoutItem)
    Public Property Tags As List(Of Categoria)
    Public Property CbTags As List(Of ToggleMenuFlyoutItem)
    Public Property Custom As List(Of Categoria)
    Public Property CbCustom As List(Of ToggleMenuFlyoutItem)

    Public Sub New(ByVal titulo As String, ByVal imagen As String, ByVal id As Integer, ByVal userscore As Categoria, ByVal cbuserscore As CheckBox,
                   ByVal categorias As List(Of Categoria), ByVal cbcategorias As List(Of ToggleMenuFlyoutItem),
                   ByVal generos As List(Of Categoria), ByVal cbgeneros As List(Of ToggleMenuFlyoutItem),
                   ByVal tags As List(Of Categoria), ByVal cbtags As List(Of ToggleMenuFlyoutItem),
                   ByVal custom As List(Of Categoria), ByVal cbcustom As List(Of ToggleMenuFlyoutItem))
        Me.Titulo = titulo
        Me.Imagen = imagen
        Me.ID = id
        Me.Userscore = userscore
        Me.CbUserscore = cbuserscore
        Me.Categorias = categorias
        Me.CbCategorias = cbcategorias
        Me.Generos = generos
        Me.CbGeneros = cbgeneros
        Me.Tags = tags
        Me.CbTags = cbtags
        Me.Custom = custom
        Me.CbCustom = cbcustom
    End Sub

End Class
