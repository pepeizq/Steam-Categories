Public Class Categoria

    Public Property Nombre As String
    Public Property Estado As Boolean

    Public Sub New(ByVal nombre As String, ByVal estado As Boolean)
        Me.Nombre = nombre
        Me.Estado = estado
    End Sub

End Class
