﻿Imports Microsoft.Toolkit.Uwp.Helpers
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

                botonRutaTexto.Text = recursos.GetString("Change")
            Else
                botonRutaTexto.Text = recursos.GetString("Add2")
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

        Dim lvComandos As ListView = pagina.FindName("lvCategoriasComandos")
        lvComandos.IsEnabled = False

        Dim boolFinal As Boolean = False
        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")
            Dim listaFinal As New List(Of CategoriaCliente)

            If Await helper.FileExistsAsync("listaJuegos") = True Then
                Dim listaJuegos As List(Of Juego) = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")

                Dim userscore As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("userscore")
                Dim metascore As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("metascore")

                For Each juego In listaJuegos
                    Dim listaCategoriasJuego As New List(Of String)

                    For Each categoria In listaCategorias
                        If categoria.Estado = True Then
                            If categoria.Nombre.Contains("/*1/") Then
                                Dim puntuacion As String = GenerarPuntuacion(userscore, juego.Userscore, 0)

                                If Not puntuacion = Nothing Then
                                    listaCategoriasJuego.Add(puntuacion)
                                End If
                            End If

                            If categoria.Nombre.Contains("/*2/") Then
                                Dim puntuacion As String = GenerarPuntuacion(metascore, juego.Metascore, 1)

                                If Not puntuacion = Nothing Then
                                    listaCategoriasJuego.Add(puntuacion)
                                End If
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

                            If listaFinal.Count > 0 Then
                                boolFinal = True
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        If boolFinal = True Then
            If Await helper.FileExistsAsync("actualizar") = True Then
                Dim actualizar As Boolean = False

                Try
                    actualizar = Await helper.ReadFileAsync(Of Boolean)("actualizar")
                Catch ex As Exception

                End Try

                If actualizar = True Then
                    Toast("Steam Categories", recursos.GetString("CategoriesUpdated"))
                Else
                    Toast("Steam Categories", recursos.GetString("CategoriesAdded"))
                End If
            Else
                Toast("Steam Categories", recursos.GetString("CategoriesAdded"))
            End If
        Else
            Toast("Steam Categories", recursos.GetString("CategoriesNotAdded"))
        End If

        lvComandos.IsEnabled = True

    End Sub

    Public Async Sub BorrarCategorias()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvComandos As ListView = pagina.FindName("lvCategoriasComandos")
        lvComandos.IsEnabled = False

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
            Toast("Steam Categories", recursos.GetString("DeleteCategoriesSteamYes"))
        Else
            Toast("Steam Categories", recursos.GetString("DeleteCategoriesSteamNo"))
        End If

        lvComandos.IsEnabled = True

    End Sub

    Private Function GenerarPuntuacion(lista As List(Of Categoria), puntuacion As String, tipo As Integer)

        If Not puntuacion = Nothing Then
            Dim int As Integer = Integer.Parse(puntuacion)

            For Each item In lista
                If int > 90 Then
                    If item.Nombre = "9" Then
                        If item.Estado = True Then
                            puntuacion = "> 90%"
                        End If
                    End If
                ElseIf int > 80 And int < 91 Then
                    If item.Nombre = "8" Then
                        If item.Estado = True Then
                            puntuacion = "> 80% & 91% <"
                        End If
                    End If
                ElseIf int > 70 And int < 81 Then
                    If item.Nombre = "7" Then
                        If item.Estado = True Then
                            puntuacion = "> 70% & 81% <"
                        End If
                    End If
                ElseIf int > 60 And int < 71 Then
                    If item.Nombre = "6" Then
                        If item.Estado = True Then
                            puntuacion = "> 60% & 71% <"
                        End If
                    End If
                ElseIf int > 50 And int < 61 Then
                    If item.Nombre = "5" Then
                        If item.Estado = True Then
                            puntuacion = "> 50% & 61% <"
                        End If
                    End If
                ElseIf int > 40 And int < 51 Then
                    If item.Nombre = "4" Then
                        If item.Estado = True Then
                            puntuacion = "> 40% & 51% <"
                        End If
                    End If
                ElseIf int > 30 And int < 41 Then
                    If item.Nombre = "3" Then
                        If item.Estado = True Then
                            puntuacion = "> 30% & 41% <"
                        End If
                    End If
                ElseIf int > 20 And int < 31 Then
                    If item.Nombre = "2" Then
                        If item.Estado = True Then
                            puntuacion = "> 20% & 31% <"
                        End If
                    End If
                ElseIf int < 21 Then
                    If item.Estado = True Then
                        puntuacion = "21% <"
                    End If
                End If
            Next

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
