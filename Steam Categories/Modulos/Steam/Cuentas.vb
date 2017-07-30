Imports Microsoft.Toolkit.Uwp

Module Cuentas

    Public Async Sub Detectar()

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim cuenta As Cuenta = Nothing

        If Await helper.FileExistsAsync("cuenta") = True Then
            cuenta = Await helper.ReadFileAsync(Of Cuenta)("cuenta")
        End If

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonCuenta As Button = pagina.FindName("botonSteamCuenta")
        botonCuenta.IsEnabled = False

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim botonCuentaTexto As TextBlock = pagina.FindName("botonSteamCuentaTexto")

        Dim tb As TextBox = pagina.FindName("tbSteamCuenta")
        tb.IsEnabled = False

        Dim usuario As String = tb.Text

        Dim pr As ProgressRing = pagina.FindName("prSteamCuenta")
        pr.Visibility = Visibility.Visible

        Dim busquedaID As Boolean = False

        If cuenta Is Nothing Then
            If Not usuario = Nothing Then
                busquedaID = True
            End If
        Else
            If Not usuario = Nothing Then
                usuario = usuario.Replace("http://steamcommunity.com/id/", Nothing)
                usuario = usuario.Replace("https://steamcommunity.com/id/", Nothing)

                If usuario.LastIndexOf("/") = (usuario.Length - 1) Then
                    usuario = usuario.Remove(usuario.Length - 1, 1)
                End If

                If Not cuenta.Usuario = usuario Then
                    busquedaID = True
                End If
            End If
        End If

        If busquedaID = True Then
            usuario = usuario.Replace("http://steamcommunity.com/id/", Nothing)
            usuario = usuario.Replace("https://steamcommunity.com/id/", Nothing)

            If usuario.LastIndexOf("/") = (usuario.Length - 1) Then
                usuario = usuario.Remove(usuario.Length - 1, 1)
            End If

            Dim id64 As String = Nothing
            Dim htmlID As String = Await Decompiladores.HttpClient(New Uri("https://api.steampowered.com/ISteamUser/ResolveVanityURL/v1/?key=488AE837ADDDA0201B51693B28F1B389&vanityurl=" + usuario))

            If Not htmlID = Nothing Then
                If htmlID.Contains("steamid") Then
                    Dim temp, temp2 As String
                    Dim int, int2 As Integer

                    int = htmlID.IndexOf("steamid" + ChrW(34))
                    temp = htmlID.Remove(0, int)

                    int2 = temp.IndexOf(":")
                    temp2 = temp.Remove(0, int2 + 1)

                    int2 = temp2.IndexOf(ChrW(34))
                    temp2 = temp2.Remove(0, int2 + 1)

                    int2 = temp2.IndexOf(ChrW(34))
                    temp2 = temp2.Remove(int2, temp2.Length - int2)

                    id64 = temp2.Trim
                End If

                If id64 = Nothing Then
                    id64 = usuario
                End If

                cuenta = New Cuenta(usuario, id64)

                Try
                    Await helper.SaveFileAsync(Of Cuenta)("cuenta", cuenta)
                Catch ex As Exception

                End Try
            End If
        End If

        If Not cuenta Is Nothing Then
            tb.Text = cuenta.Usuario

            Dim htmlJuegos As String = Await Decompiladores.HttpClient(New Uri("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=488AE837ADDDA0201B51693B28F1B389&steamid=" + cuenta.ID64 + "&format=json"))

            If Not htmlJuegos = Nothing Then
                If htmlJuegos.Contains("game_count") Then
                    Dim temp, temp2 As String
                    Dim int, int2 As Integer

                    int = htmlJuegos.IndexOf("game_count")
                    temp = htmlJuegos.Remove(0, int)

                    int2 = temp.IndexOf(",")

                    If Not int2 = -1 Then
                        temp2 = temp.Remove(int2, temp.Length - int2)

                        temp2 = temp2.Replace("game_count", Nothing)
                        temp2 = temp2.Replace(ChrW(34), Nothing)
                        temp2 = temp2.Replace(":", Nothing)
                        temp2 = temp2.Replace(vbNullChar, Nothing)
                        temp2 = temp2.Trim

                        Dim tbCuenta As TextBlock = pagina.FindName("tbJuegosCuenta")

                        tbCuenta.Text = temp2

                        Dim botonCategorias As Button = pagina.FindName("botonCargaCategorias")

                        If tbCuenta.Text.Length > 0 Then
                            botonCategorias.IsEnabled = True

                            Dim listaJuegosID As New List(Of String)

                            Dim i As Integer = 0

                            While i < temp2
                                If htmlJuegos.Contains(ChrW(34) + "appid" + ChrW(34)) Then
                                    Dim temp3, temp4 As String
                                    Dim int3, int4 As Integer

                                    int3 = htmlJuegos.IndexOf(ChrW(34) + "appid" + ChrW(34))
                                    temp3 = htmlJuegos.Remove(0, int3 + 7)

                                    htmlJuegos = temp3

                                    int4 = temp3.IndexOf(",")
                                    temp4 = temp3.Remove(int4, temp3.Length - int4)

                                    temp4 = temp4.Replace(":", Nothing)
                                    temp4 = temp4.Trim

                                    listaJuegosID.Add(temp4)
                                End If
                                i += 1
                            End While

                            Try
                                Await helper.SaveFileAsync(Of List(Of String))("listaJuegosID", listaJuegosID)

                                botonCuentaTexto.Text = recursos.GetString("Boton Cambiar")
                            Catch ex As Exception

                            End Try
                        Else
                            botonCategorias.IsEnabled = False
                            botonCuentaTexto.Text = recursos.GetString("Boton Añadir")
                        End If
                    End If
                End If
            End If
        End If

        botonCuenta.IsEnabled = True
        tb.IsEnabled = True
        pr.Visibility = Visibility.Collapsed

    End Sub

End Module
