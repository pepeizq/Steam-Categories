Public Class Cuenta

    Public Property Usuario As String
    Public Property ID64 As String
    Public Property Nombre As String
    Public Property Avatar As String

    Public Sub New(ByVal usuario As String, ByVal id64 As String, ByVal nombre As String, ByVal avatar As String)
        Me.Usuario = usuario
        Me.ID64 = id64
        Me.Nombre = nombre
        Me.Avatar = avatar
    End Sub

End Class
