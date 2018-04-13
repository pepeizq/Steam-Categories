Public Class Categoria

    Public Property Nombre As String
    Public Property Estado As Boolean
    Public Property IDJuego As Integer

    Public Sub New(ByVal nombre As String, ByVal estado As Boolean, ByVal idjuego As Integer)
        Me.Nombre = nombre
        Me.Estado = estado
        Me.IDJuego = idjuego
    End Sub

End Class
