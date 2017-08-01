Imports Microsoft.Toolkit.Uwp
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers

Module Cliente

    Public Async Sub Detectar(picker As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbRuta As TextBlock = pagina.FindName("tbSteamRuta")

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim botonRutaTexto As TextBlock = pagina.FindName("botonSteamRutaTexto")

        Dim carpeta As StorageFolder = Nothing

        If picker = True Then
            Dim carpetapicker As FolderPicker = New FolderPicker()

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

                botonRutaTexto.Text = recursos.GetString("Boton Cambiar")

                Dim botonBorrarCategorias As Button = pagina.FindName("botonBorrarCategorias")
                botonBorrarCategorias.IsEnabled = True
            Else
                botonRutaTexto.Text = recursos.GetString("Boton Añadir")
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

    Public Async Sub EscribirCategorias(boton As Button)

        boton.IsEnabled = False

        Dim boolFinal As Boolean = False
        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")
            Dim listaFinal As New List(Of CategoriaCliente)

            If Await helper.FileExistsAsync("listaJuegos") = True Then
                Dim listaJuegos As List(Of Juego) = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")

                For Each juego In listaJuegos
                    Dim listaCategoriasJuego As New List(Of String)

                    For Each categoria In listaCategorias
                        If categoria.Estado = True Then
                            If categoria.Nombre.Contains("/*1/") Then
                                listaCategoriasJuego.Add(GenerarScore(juego.Userscore, 0))
                            End If

                            If categoria.Nombre.Contains("/*2/") Then
                                listaCategoriasJuego.Add(GenerarScore(juego.Metascore, 1))
                            End If

                            If categoria.Nombre.Contains("/*3/") Then
                                listaCategoriasJuego.Add(juego.Año)
                            End If

                            For Each genero In juego.Generos
                                If categoria.Nombre = genero Then
                                    If listaCategoriasJuego.Count > 0 Then
                                        Dim boolAñadir As Boolean = False

                                        Dim i As Integer = 0
                                        While i < listaCategoriasJuego.Count
                                            If categoria.Nombre = listaCategoriasJuego(i) Then
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
                                If categoria.Nombre = categoria_ Then
                                    If listaCategoriasJuego.Count > 0 Then
                                        Dim boolAñadir As Boolean = False

                                        Dim i As Integer = 0
                                        While i < listaCategoriasJuego.Count
                                            If categoria.Nombre = listaCategoriasJuego(i) Then
                                                boolAñadir = True
                                            End If
                                            i += 1
                                        End While

                                        If boolAñadir = False Then
                                            listaCategoriasJuego.Add(categoria.Nombre)
                                        End If
                                    Else
                                        listaCategoriasJuego.Add(categoria.Nombre)
                                    End If
                                End If
                            Next

                            For Each tag In juego.Tags
                                If categoria.Nombre = tag Then
                                    If listaCategoriasJuego.Count > 0 Then
                                        Dim boolAñadir As Boolean = False

                                        Dim i As Integer = 0
                                        While i < listaCategoriasJuego.Count
                                            If categoria.Nombre = listaCategoriasJuego(i) Then
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
                                If categoria.Nombre = idioma Then
                                    If listaCategoriasJuego.Count > 0 Then
                                        Dim boolAñadir As Boolean = False

                                        Dim i As Integer = 0
                                        While i < listaCategoriasJuego.Count
                                            If categoria.Nombre = listaCategoriasJuego(i) Then
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
                        End If
                    Next

                    If listaCategoriasJuego.Count > 0 Then
                        listaFinal.Add(New CategoriaCliente(juego.ID, listaCategoriasJuego))
                    End If
                Next
            End If

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

        boton.IsEnabled = True

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
