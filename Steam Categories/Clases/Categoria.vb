Public Class Categoria

    Public Property Nombre As String
    Public Property Estado As Boolean
    Public Property Seccion As String

    Public Sub New(ByVal nombre As String, ByVal estado As Boolean, ByVal seccion As String)
        Me.Nombre = nombre
        Me.Estado = estado
        Me.Seccion = seccion
    End Sub

End Class
