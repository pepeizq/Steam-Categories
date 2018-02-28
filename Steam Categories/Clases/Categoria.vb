Public Class Categoria

    Public Property Nombre As String
    Public Property Estado As Boolean
    Public Property Maestro As CategoriaMaestro

    Public Sub New(ByVal nombre As String, ByVal estado As Boolean, ByVal maestro As CategoriaMaestro)
        Me.Nombre = nombre
        Me.Estado = estado
        Me.Maestro = maestro
    End Sub

End Class
