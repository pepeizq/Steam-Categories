Public Class Categoria2

    Public Property Nombre As String
    Public Property Imagen As String
    Public Property ID As Integer

    Public Sub New(ByVal nombre As String, ByVal imagen As String, ByVal id As Integer)
        Me.Nombre = nombre
        Me.Imagen = imagen
        Me.ID = id
    End Sub

End Class
