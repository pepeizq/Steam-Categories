Public Class Cuenta

    Public Property Usuario As String
    Public Property ID64 As String

    Public Sub New(ByVal usuario As String, ByVal id64 As String)
        Me.Usuario = usuario
        Me.ID64 = id64
    End Sub

End Class
