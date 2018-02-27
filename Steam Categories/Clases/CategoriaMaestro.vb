Public Class CategoriaMaestro

    Public Property Nombre As String
    Public Property Imagen As String
    Public Property ID As Integer
    Public Property GridNombre As String

    Public Sub New(ByVal nombre As String, ByVal imagen As String, ByVal id As Integer, ByVal gridNombre As String)
        Me.Nombre = nombre
        Me.Imagen = imagen
        Me.ID = id
        Me.GridNombre = gridNombre
    End Sub

End Class
