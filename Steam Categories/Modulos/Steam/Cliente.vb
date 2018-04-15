Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers

Module Cliente

    Public Async Sub Detectar(picker As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbRuta As TextBlock = pagina.FindName("tbSteamRuta")

        Dim recursos As New Resources.ResourceLoader()
        Dim botonRutaTexto As TextBlock = pagina.FindName("botonSteamRutaTexto")

        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")
        botonBorrar.IsEnabled = False

        Dim carpeta As StorageFolder = Nothing

        If picker = True Then
            Dim carpetapicker As New FolderPicker()

            carpetapicker.FileTypeFilter.Add("*")
            carpetapicker.ViewMode = PickerViewMode.List

            carpeta = Await carpetapicker.PickSingleFolderAsync()
        Else
            Try
                carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
            Catch ex As Exception
                carpeta = Nothing
            End Try
        End If

        If Not carpeta Is Nothing Then
            Dim ejecutable As StorageFile = Nothing

            Try
                ejecutable = Await carpeta.GetFileAsync("Steam.exe")
            Catch ex As Exception

            End Try

            If Not ejecutable Is Nothing Then
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("SteamPath", carpeta)
                tbRuta.Text = carpeta.Path

                botonRutaTexto.Text = recursos.GetString("Change")
                botonBorrar.IsEnabled = True
            Else
                botonRutaTexto.Text = recursos.GetString("Add2")
                botonBorrar.IsEnabled = False
            End If
        End If

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

    Public Async Sub EscribirCategorias()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        botonAñadir.IsEnabled = False

        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        botonLimpiar.IsEnabled = False

        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")
        botonBorrar.IsEnabled = False

        Dim listaFinal As New List(Of CategoriaCliente)
        Dim boolFinal As Boolean = False

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            Dim listaFinalAñadir As New List(Of String)

            If Not juego.Userscore Is Nothing Then
                If juego.Userscore.Estado = True Then
                    listaFinalAñadir.Add(GenerarPuntuacion(juego.Userscore.Nombre, 0))
                End If
            End If

            If Not juego.Tags Is Nothing Then
                If juego.Tags.Count > 0 Then
                    For Each tag In juego.Tags
                        If tag.Estado = True Then
                            listaFinalAñadir.Add(tag.Nombre)
                        End If
                    Next
                End If
            End If

            If Not juego.Categorias Is Nothing Then
                If juego.Categorias.Count > 0 Then
                    For Each categoria In juego.Categorias
                        If categoria.Estado = True Then
                            listaFinalAñadir.Add(categoria.Nombre)
                        End If
                    Next
                End If
            End If

            If Not juego.Generos Is Nothing Then
                If juego.Generos.Count > 0 Then
                    For Each genero In juego.Generos
                        If genero.Estado = True Then
                            listaFinalAñadir.Add(genero.Nombre)
                        End If
                    Next
                End If
            End If

            If listaFinalAñadir.Count > 0 Then
                listaFinal.Add(New CategoriaCliente(juego.ID, listaFinalAñadir))
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

                        If listaFinal.Count > 0 Then
                            boolFinal = True
                        End If
                    End If
                End If
            End If
        End If

        Dim recursos As New Resources.ResourceLoader()
        Dim helper As New LocalObjectStorageHelper

        If boolFinal = True Then
            If Await helper.FileExistsAsync("actualizar") = True Then
                Dim actualizar As Boolean = False

                Try
                    actualizar = Await helper.ReadFileAsync(Of Boolean)("actualizar")
                Catch ex As Exception

                End Try

                If actualizar = True Then
                    Toast(recursos.GetString("CategoriesUpdated"), Nothing)
                Else
                    Toast(recursos.GetString("CategoriesAdded"), Nothing)
                End If
            Else
                Toast(recursos.GetString("CategoriesAdded"), Nothing)
            End If
        Else
            Toast(recursos.GetString("CategoriesNotAdded"), Nothing)
        End If

        botonAñadir.IsEnabled = True
        botonLimpiar.IsEnabled = True
        botonBorrar.IsEnabled = True

    End Sub

    Public Async Sub BorrarCategorias()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        botonAñadir.IsEnabled = False

        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        botonLimpiar.IsEnabled = False

        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")
        botonBorrar.IsEnabled = False

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

        Dim recursos As New Resources.ResourceLoader()

        If boolFinal = True Then
            Toast(recursos.GetString("DeleteCategoriesSteamYes"), Nothing)
        Else
            Toast(recursos.GetString("DeleteCategoriesSteamNo"), Nothing)
        End If

        botonAñadir.IsEnabled = True
        botonLimpiar.IsEnabled = True
        botonBorrar.IsEnabled = True

    End Sub

    Private Function GenerarPuntuacion(puntuacion As String, tipo As Integer)

        If Not puntuacion = Nothing Then
            Dim int As Integer = Integer.Parse(puntuacion)

            If int > 89 Then
                puntuacion = "> 89%"
            ElseIf int > 79 And int < 90 Then
                puntuacion = "> 79% & 90% <"
            ElseIf int > 69 And int < 80 Then
                puntuacion = "> 69% & 80% <"
            ElseIf int > 59 And int < 70 Then
                puntuacion = "> 59% & 70% <"
            ElseIf int > 49 And int < 60 Then
                puntuacion = "> 49% & 60% <"
            ElseIf int > 39 And int < 50 Then
                puntuacion = "> 39% & 50% <"
            ElseIf int > 29 And int < 40 Then
                puntuacion = "> 29% & 40% <"
            ElseIf int > 19 And int < 30 Then
                puntuacion = "> 19% & 30% <"
            ElseIf int < 20 Then
                puntuacion = "20% <"
            End If

            If Not puntuacion = Nothing Then
                If puntuacion.Contains("%") Then
                    If tipo = 0 Then
                        puntuacion = "Userscore " + puntuacion
                    ElseIf tipo = 1 Then
                        puntuacion = "Metascore " + puntuacion
                    End If
                Else
                    puntuacion = Nothing
                End If
            End If
        End If

        Return puntuacion
    End Function

End Module
