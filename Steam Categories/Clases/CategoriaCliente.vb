Public Class CategoriaCliente

    Public Property ID As Integer
    Public Property Categorias As List(Of String)

    Public Sub New(ByVal id As Integer, ByVal categorias As List(Of String))
        Me.ID = id
        Me.Categorias = categorias
    End Sub

End Class
