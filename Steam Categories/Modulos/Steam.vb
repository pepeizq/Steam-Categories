Imports Microsoft.Toolkit.Uwp
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers

Module Steam

    Public Async Sub ArranqueCliente(tbConfigPath As TextBlock, buttonConfigPath As TextBlock, picker As Boolean)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim carpeta As StorageFolder = Nothing

        Try
            If picker = True Then
                Dim carpetapicker As FolderPicker = New FolderPicker()

                carpetapicker.FileTypeFilter.Add("*")
                carpetapicker.ViewMode = PickerViewMode.List

                carpeta = Await carpetapicker.PickSingleFolderAsync()
            Else
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
            End If

            If Not carpeta Is Nothing Then
                Dim ejecutable As StorageFile = Nothing

                Try
                    ejecutable = Await carpeta.GetFileAsync("Steam.exe")
                Catch ex As Exception

                End Try

                If Not ejecutable Is Nothing Then
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("SteamPath", carpeta)
                    tbConfigPath.Text = carpeta.Path
                    buttonConfigPath.Text = recursos.GetString("Boton Cambiar")
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Dim WithEvents wb As WebView
    Dim tb_ As TextBox
    Dim pr_ As ProgressRing
    Dim cuenta As String
    Dim steamID As String

    Public Sub ArranqueCuenta(tb As TextBox, pr As ProgressRing)

        tb.IsEnabled = False
        tb_ = tb

        pr.Visibility = Visibility.Visible
        pr_ = pr

        If tb.Text = Nothing Then
            tb.Text = "http://steamcommunity.com/id/pepeizq/"
        End If

        cuenta = tb.Text

        wb = New WebView
        wb.Navigate(New Uri(cuenta))

    End Sub

    Private Async Sub wb_NavigationCompleted() Handles wb.NavigationCompleted

        Dim lista As New List(Of String)
        lista.Add("document.documentElement.outerHTML;")
        Dim argumentos As IEnumerable(Of String) = lista
        Dim html As String = Nothing

        Try
            html = Await wb.InvokeScriptAsync("eval", argumentos)
        Catch ex As Exception

        End Try

        If Not html = Nothing Then
            If html.Contains(ChrW(34) + "steamid" + ChrW(34)) Then
                Dim temp, temp2 As String
                Dim int, int2 As Integer

                int = html.IndexOf(ChrW(34) + "steamid" + ChrW(34))
                temp = html.Remove(0, int)

                int2 = temp.IndexOf(",")
                temp2 = temp.Remove(int2, temp.Length - int2)

                temp2 = temp2.Replace("steamid", "")
                temp2 = temp2.Replace(":", "")
                temp2 = temp2.Replace(ChrW(34), "")

                steamID = temp2.Trim
            End If

            If html.Contains("image_src") Then
                Dim temp, temp2, temp3 As String
                Dim int, int2, int3 As Integer

                int = html.IndexOf("image_src")
                temp = html.Remove(0, int + 10)

                int2 = temp.IndexOf(ChrW(34))
                temp2 = temp.Remove(0, int2 + 1)

                int3 = temp2.IndexOf(ChrW(34))
                temp3 = temp2.Remove(int3, temp2.Length - int3)

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content
                Dim imagen As ImageEx = pagina.FindName("imageCuenta")

                imagen.Source = New BitmapImage(New Uri(temp3, UriKind.Absolute))
            End If

            If html.Contains(ChrW(34) + "personaname" + ChrW(34)) Then
                Dim temp, temp2 As String
                Dim int, int2 As Integer

                int = html.IndexOf(ChrW(34) + "personaname" + ChrW(34))
                temp = html.Remove(0, int)

                int2 = temp.IndexOf(",")
                temp2 = temp.Remove(int2, temp.Length - int2)

                temp2 = temp2.Replace("personaname", "")
                temp2 = temp2.Replace(":", "")
                temp2 = temp2.Replace(ChrW(34), "")

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content
                Dim tbNombre As TextBlock = pagina.FindName("tbCuentaNombre")

                tbNombre.Text = temp2.Trim
            End If
        End If

        html = Await Decompiladores.HttpClient(New Uri("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=488AE837ADDDA0201B51693B28F1B389&steamid=" + steamID + "&format=json"))

        If Not html = Nothing Then
            If html.Contains("game_count") Then
                Dim temp, temp2 As String
                Dim int, int2 As Integer

                int = html.IndexOf("game_count")
                temp = html.Remove(0, int)

                int2 = temp.IndexOf(",")
                temp2 = temp.Remove(int2, temp.Length - int2)

                temp2 = temp2.Replace("game_count", Nothing)
                temp2 = temp2.Replace(ChrW(34), Nothing)
                temp2 = temp2.Replace(":", Nothing)
                temp2 = temp2.Replace(vbNullChar, Nothing)
                temp2 = temp2.Trim

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content
                Dim tb As TextBlock = pagina.FindName("tbJuegosCuenta")

                tb.Text = temp2

                Dim button As Button = pagina.FindName("buttonCargaCategorias")

                If tb.Text.Length > 0 Then
                    button.IsEnabled = True

                    Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

                    Dim listaJuegosID As New List(Of String)

                    Dim i As Integer = 0

                    While i < temp2
                        If html.Contains(ChrW(34) + "appid" + ChrW(34)) Then
                            Dim temp3, temp4 As String
                            Dim int3, int4 As Integer

                            int3 = html.IndexOf(ChrW(34) + "appid" + ChrW(34))
                            temp3 = html.Remove(0, int3 + 7)

                            html = temp3

                            int4 = temp3.IndexOf(",")
                            temp4 = temp3.Remove(int4, temp3.Length - int4)

                            temp4 = temp4.Replace(":", Nothing)
                            temp4 = temp4.Trim

                            listaJuegosID.Add(temp4)
                        End If
                        i += 1
                    End While

                    Await helper.SaveFileAsync(Of List(Of String))("listaJuegosID", listaJuegosID)
                Else
                    button.IsEnabled = False
                End If
            End If
        End If

        If Not steamID = Nothing Then
            Dim opciones As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            opciones.Values("steamID") = steamID
        End If

        tb_.IsEnabled = True
        pr_.Visibility = Visibility.Collapsed

    End Sub

    Public Async Sub EscribirCategorias(listaJuegos As List(Of Juego), button As Button)

        button.IsEnabled = False

        Dim boolFinal As Boolean = False
        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")
            Dim listaFinal As New List(Of JuegoFinal)

            For Each juego In listaJuegos
                Dim listaCategoriasJuego As New List(Of String)

                For Each categoria In listaCategorias
                    If categoria = "/*/userscore/*/" Then
                        listaCategoriasJuego.Add(GenerarScore(juego.Userscore, 0))
                    End If

                    If categoria = "/*/metascore/*/" Then
                        listaCategoriasJuego.Add(GenerarScore(juego.Metascore, 1))
                    End If

                    If categoria = "/*/años/*/" Then
                        listaCategoriasJuego.Add(juego.Año)
                    End If

                    For Each genero In juego.Generos
                        If categoria = genero Then
                            If listaCategoriasJuego.Count > 0 Then
                                Dim boolAñadir As Boolean = False

                                Dim i As Integer = 0
                                While i < listaCategoriasJuego.Count
                                    If categoria = listaCategoriasJuego(i) Then
                                        boolAñadir = True
                                    End If
                                    i += 1
                                End While

                                If boolAñadir = False Then
                                    listaCategoriasJuego.Add(genero)
                                End If
                            Else
                                listaCategoriasJuego.Add(genero)
                            End If
                        End If
                    Next

                    For Each categoria_ In juego.Categorias
                        If categoria = categoria_ Then
                            If listaCategoriasJuego.Count > 0 Then
                                Dim boolAñadir As Boolean = False

                                Dim i As Integer = 0
                                While i < listaCategoriasJuego.Count
                                    If categoria = listaCategoriasJuego(i) Then
                                        boolAñadir = True
                                    End If
                                    i += 1
                                End While

                                If boolAñadir = False Then
                                    listaCategoriasJuego.Add(categoria)
                                End If
                            Else
                                listaCategoriasJuego.Add(categoria)
                            End If
                        End If
                    Next

                    For Each tag In juego.Tags
                        If categoria = tag Then
                            If listaCategoriasJuego.Count > 0 Then
                                Dim boolAñadir As Boolean = False

                                Dim i As Integer = 0
                                While i < listaCategoriasJuego.Count
                                    If categoria = listaCategoriasJuego(i) Then
                                        boolAñadir = True
                                    End If
                                    i += 1
                                End While

                                If boolAñadir = False Then
                                    listaCategoriasJuego.Add(tag)
                                End If
                            Else
                                listaCategoriasJuego.Add(tag)
                            End If
                        End If
                    Next

                    For Each idioma In juego.Idiomas
                        If categoria = idioma Then
                            If listaCategoriasJuego.Count > 0 Then
                                Dim boolAñadir As Boolean = False

                                Dim i As Integer = 0
                                While i < listaCategoriasJuego.Count
                                    If categoria = listaCategoriasJuego(i) Then
                                        boolAñadir = True
                                    End If
                                    i += 1
                                End While

                                If boolAñadir = False Then
                                    listaCategoriasJuego.Add(idioma)
                                End If
                            Else
                                listaCategoriasJuego.Add(idioma)
                            End If
                        End If
                    Next
                Next

                If listaCategoriasJuego.Count > 0 Then
                    listaFinal.Add(New JuegoFinal(juego.ID, listaCategoriasJuego))
                End If
            Next

            Dim usuarioID As String = Await GenerarID()
            Dim carpeta As StorageFolder = Nothing

            Try
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
            Catch ex As Exception

            End Try

            If Not carpeta Is Nothing Then
                If Not usuarioID = Nothing Then
                    Dim sharedconfig As StorageFile = Nothing

                    Try
                        sharedconfig = Await carpeta.GetFileAsync("userdata\" + usuarioID + "\7\remote\sharedconfig.vdf")
                    Catch ex As Exception

                    End Try

                    If Not sharedconfig Is Nothing Then
                        Dim lineas As String = Nothing

                        Try
                            lineas = Await StorageFileHelper.ReadTextFromFileAsync(carpeta, "userdata\" + usuarioID + "\7\remote\sharedconfig.vdf")
                        Catch ex As Exception

                        End Try

                        If Not lineas = Nothing Then
                            Dim temp, temp2, temp3 As String
                            Dim int, int2, int3 As Integer

                            int = lineas.IndexOf(ChrW(34) + "Apps" + ChrW(34))

                            If int = -1 Then
                                int = lineas.IndexOf(ChrW(34) + "apps" + ChrW(34))
                            End If

                            temp = lineas.Remove(0, int)

                            int2 = temp.IndexOf("{")
                            temp2 = temp.Remove(0, int2 + 1)

                            int3 = temp2.LastIndexOf("}")
                            temp3 = temp2.Remove(int3, temp2.Length - int3)

                            lineas = lineas.Remove(int + int2 + 1, int3)
                            lineas = lineas.Insert(int + int2 + 1, vbCrLf + "}" + vbCrLf + "}" + vbCrLf + "}" + vbCrLf + "}")

                            '---------------------------------

                            int = lineas.IndexOf(ChrW(34) + "Apps" + ChrW(34))

                            If int = -1 Then
                                int = lineas.IndexOf(ChrW(34) + "apps" + ChrW(34))
                            End If

                            temp = lineas.Remove(0, int)

                            int2 = temp.IndexOf("{")

                            For Each juego In listaFinal
                                Dim listaFinalCategorias As List(Of String) = juego.Categorias

                                If listaFinalCategorias.Count > 0 Then
                                    Dim lineasAñadirJuego As String = Nothing

                                    lineasAñadirJuego = lineasAñadirJuego + vbCrLf + ChrW(34) + juego.ID.ToString + ChrW(34) + vbCrLf + "{" + ChrW(34) + "tags" + ChrW(34) + vbCrLf + "{ " + vbCrLf

                                    Dim i As Integer = 0
                                    While i < listaFinalCategorias.Count
                                        lineasAñadirJuego = lineasAñadirJuego + ChrW(34) + i.ToString + ChrW(34) + " " + ChrW(34) + listaFinalCategorias(i) + ChrW(34)

                                        If Not i = listaFinalCategorias.Count - 1 Then
                                            lineasAñadirJuego = lineasAñadirJuego + vbCrLf
                                        End If

                                        i += 1
                                    End While

                                    lineasAñadirJuego = lineasAñadirJuego + vbCrLf + "}" + vbCrLf + "}"

                                    lineas = lineas.Insert(int + int2 + 1, lineasAñadirJuego)
                                End If
                            Next

                            Await FileIO.WriteTextAsync(sharedconfig, lineas)
                            boolFinal = True
                        End If
                    End If
                End If
            End If
        End If

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        If boolFinal = True Then
            Toast("Steam Categories", recursos.GetString("Categorias Si"))
        Else
            Toast("Steam Categories", recursos.GetString("Categorias No"))
        End If

        button.IsEnabled = True

    End Sub

    Public Async Sub BorrarCategorias(button As Button)

        button.IsEnabled = False

        Dim boolFinal As Boolean = False
        Dim usuarioID As String = Await GenerarID()
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            If Not usuarioID = Nothing Then
                Dim sharedconfig As StorageFile = Nothing

                Try
                    sharedconfig = Await carpeta.GetFileAsync("userdata\" + usuarioID + "\7\remote\sharedconfig.vdf")
                Catch ex As Exception

                End Try

                If Not sharedconfig Is Nothing Then
                    Dim lineas As String = Nothing

                    lineas = Await StorageFileHelper.ReadTextFromFileAsync(carpeta, "userdata\" + usuarioID + "\7\remote\sharedconfig.vdf")

                    If Not lineas = Nothing Then
                        Dim temp, temp2, temp3 As String
                        Dim int, int2, int3 As Integer

                        int = lineas.IndexOf(ChrW(34) + "Apps" + ChrW(34))

                        If int = -1 Then
                            int = lineas.IndexOf(ChrW(34) + "apps" + ChrW(34))
                        End If

                        temp = lineas.Remove(0, int)

                        int2 = temp.IndexOf("{")
                        temp2 = temp.Remove(0, int2 + 1)

                        int3 = temp2.LastIndexOf("}")
                        temp3 = temp2.Remove(int3, temp2.Length - int3)

                        lineas = lineas.Remove(int + int2 + 1, int3)
                        lineas = lineas.Insert(int + int2 + 1, vbCrLf + "}" + vbCrLf + "}" + vbCrLf + "}" + vbCrLf + "}")
                    End If

                    Await FileIO.WriteTextAsync(sharedconfig, lineas)
                    boolFinal = True
                End If
            End If
        End If

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        If boolFinal = True Then
            Toast("Steam Categories", recursos.GetString("Borrado Si"))
        Else
            Toast("Steam Categories", recursos.GetString("Borrado No"))
        End If

        button.IsEnabled = True

    End Sub

    Private Async Function GenerarID() As Task(Of String)

        Dim usuarioID As String = Nothing
        Dim carpeta As StorageFolder = Nothing

        Try
            carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
        Catch ex As Exception

        End Try

        If Not carpeta Is Nothing Then
            Dim listaSubcarpetas As IReadOnlyList(Of StorageFolder) = Await carpeta.GetFoldersAsync()

            For Each subcarpeta As StorageFolder In listaSubcarpetas
                If subcarpeta.Name.ToLower = "logs" Then
                    Dim conexiones As StorageFile = Nothing

                    Try
                        conexiones = Await subcarpeta.GetFileAsync("connection_log.txt")
                    Catch ex As Exception

                    End Try

                    If Not conexiones Is Nothing Then
                        Dim lineas As String = Nothing

                        Try
                            lineas = Await FileIO.ReadTextAsync(conexiones)
                        Catch ex As Exception
                        End Try

                        If Not lineas = Nothing Then
                            If lineas.Contains("SetSteamID") Then
                                Dim temp, temp2, temp3 As String
                                Dim int, int2, int3 As Integer

                                int = lineas.LastIndexOf("SetSteamID")
                                temp = lineas.Remove(0, int + 5)

                                Dim i As Integer = 0
                                While i < 100
                                    If temp.Contains("[U:1:") Then
                                        int2 = temp.IndexOf("[U:1:")
                                        temp2 = temp.Remove(0, int2 + 5)

                                        temp = temp2

                                        int3 = temp2.IndexOf("]")
                                        temp3 = temp2.Remove(int3, temp2.Length - int3)

                                        usuarioID = temp3.Trim

                                        If usuarioID.Length > 1 Then
                                            Exit While
                                        End If
                                    End If
                                    i += 1
                                End While
                            End If
                        End If
                    End If
                End If
            Next
        End If

        Return usuarioID
    End Function

    Private Function GenerarScore(score As String, tipo As Integer)

        If Not score = Nothing Then
            Dim int As Integer = Integer.Parse(score)

            If int > 90 Then
                score = "> 90%"
            ElseIf int > 80 And int < 91 Then
                score = "> 80% & 91% <"
            ElseIf int > 70 And int < 81 Then
                score = "> 70% & 81% <"
            ElseIf int > 60 And int < 71 Then
                score = "> 60% & 71% <"
            ElseIf int > 50 And int < 61 Then
                score = "> 50% & 61% <"
            ElseIf int > 40 And int < 51 Then
                score = "> 40% & 51% <"
            ElseIf int > 30 And int < 41 Then
                score = "> 30% & 41% <"
            ElseIf int > 20 And int < 31 Then
                score = "> 20% & 31% <"
            ElseIf int < 21 Then
                score = "21% <"
            End If

            If tipo = 0 Then
                score = "Userscore " + score
            ElseIf tipo = 1 Then
                score = "Metascore " + score
            End If
        End If

        Return score
    End Function

End Module
