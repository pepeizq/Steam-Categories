﻿Imports Microsoft.Toolkit.Uwp

Module Categorias

    Public Async Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridProgreso As Grid = pagina.FindName("gridProgreso")
        gridProgreso.Visibility = Visibility.Visible

        Dim prProgreso As ProgressBar = pagina.FindName("prProgreso")
        prProgreso.IsIndeterminate = False

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")

        Dim botonCargaCategorias As Button = pagina.FindName("botonCargaCategorias")
        botonCargaCategorias.IsEnabled = False

        Dim botonBorrarCategorias As Button = pagina.FindName("botonBorrarCategorias")
        botonBorrarCategorias.IsEnabled = False

        Dim botonBorrarCategoriasApp As Button = pagina.FindName("botonBorrarCategoriasApp")
        botonBorrarCategoriasApp.IsEnabled = False

        Dim botonSteamRuta As Button = pagina.FindName("botonSteamRuta")
        botonSteamRuta.IsEnabled = False

        Dim botonCuenta As Button = pagina.FindName("botonSteamCuenta")
        botonCuenta.IsEnabled = False

        Dim tbSteamCuenta As TextBox = pagina.FindName("tbSteamCuenta")
        tbSteamCuenta.IsEnabled = False

        Dim cbActualizar As CheckBox = pagina.FindName("cbActualizarListaJuegos")
        cbActualizar.IsEnabled = False

        Dim tbJuegosApp As TextBlock = pagina.FindName("tbJuegosApp")

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        Dim listaJuegosID As List(Of String) = Nothing

        If Await helper.FileExistsAsync("listaJuegosID") = True Then
            listaJuegosID = Await helper.ReadFileAsync(Of List(Of String))("listaJuegosID")
        End If

        Dim listaJuegos As List(Of Juego) = Nothing

        If Await helper.FileExistsAsync("listaJuegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")
        Else
            listaJuegos = New List(Of Juego)
        End If

        Dim actualizar As Boolean = False

        If Await helper.FileExistsAsync("actualizar") = True Then
            actualizar = Await helper.ReadFileAsync(Of Boolean)("actualizar")
        End If

        If Not listaJuegosID Is Nothing Then
            Dim i As Integer = 0

            For Each idJuego In listaJuegosID
                Dim boolAñadir As Boolean = False

                If actualizar = True Then
                    For Each juego In listaJuegos
                        If idJuego = juego.ID Then
                            boolAñadir = True
                        End If
                    Next
                End If

                If boolAñadir = False Then
                    Dim html As String = Await Decompiladores.HttpClient(New Uri("http://steamspy.com/app/" + idJuego))

                    If Not html = Nothing Then
                        Dim titulo As String = Nothing

                        Dim temp, temp2, temp3 As String
                        Dim int, int2, int3 As Integer

                        int = html.IndexOf("<div class=" + ChrW(34) + "p-r-30")
                        temp = html.Remove(0, int)

                        int2 = temp.IndexOf("<div>")
                        temp2 = temp.Remove(0, int2 + 5)

                        int3 = temp2.IndexOf("</div>")
                        temp3 = temp2.Remove(int3, temp2.Length - int3)

                        temp3 = temp3.Replace("<h3>", Nothing)
                        temp3 = temp3.Replace("</h3>", Nothing)
                        temp3 = temp3.Trim

                        titulo = temp3

                        Dim userscore As String = Nothing

                        If html.Contains("<strong>Userscore:</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Userscore:</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("%")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Userscore:", Nothing)
                            temp5 = temp5.Trim

                            userscore = temp5
                        End If

                        Dim metascore As String = Nothing

                        If html.Contains("<strong>Metascore:</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Metascore:</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("%")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Metascore:", Nothing)
                            temp5 = temp5.Trim

                            metascore = temp5
                        End If

                        Dim año As String = Nothing

                        If html.Contains("<strong>Release date</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Release date</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("<br>")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Release date", Nothing)
                            temp5 = temp5.Replace(":", Nothing)

                            If temp5.Contains(",") Then
                                Dim int6 As Integer

                                int6 = temp5.IndexOf(",")
                                temp5 = temp5.Remove(0, int6 + 1)
                            End If

                            If temp5.Contains("19") Then
                                Dim int6 As Integer

                                int6 = temp5.IndexOf("19")
                                temp5 = temp5.Remove(0, int6)

                                If (temp5.Length - (int6 + 4)) > 0 Then
                                    temp5 = temp5.Remove(int6 + 4, temp5.Length - (int6 + 4))
                                End If
                            End If

                            If temp5.Contains("20") Then
                                Dim int6 As Integer

                                int6 = temp5.IndexOf("20")
                                temp5 = temp5.Remove(0, int6)

                                If (temp5.Length - (int6 + 4)) > 0 Then
                                    temp5 = temp5.Remove(int6 + 4, temp5.Length - (int6 + 4))
                                End If
                            End If

                            temp5 = temp5.Trim

                            año = temp5
                        End If

                        Dim categorias As New List(Of String)

                        If html.Contains("<strong>Category:</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Category:</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("<br>")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Category:", Nothing)
                            temp5 = temp5.Trim

                            If Not temp5.Contains(",") Then
                                categorias.Add(temp5)
                            Else
                                Dim j As Integer = 0
                                While j < 100
                                    Dim temp6 As String
                                    Dim int6 As Integer

                                    If temp5.Contains(",") Then
                                        int6 = temp5.IndexOf(",")
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        categorias.Add(temp6.Trim)

                                        temp5 = temp5.Remove(0, int6 + 1)
                                    Else
                                        categorias.Add(temp5.Trim)
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim generos As New List(Of String)

                        If html.Contains("<strong>Genre:</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Genre:</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("<br>")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Genre:", Nothing)
                            temp5 = temp5.Trim

                            If Not temp5.Contains(",") Then
                                If temp5.Contains("<") Then
                                    Dim int7, int8 As Integer

                                    int7 = temp5.IndexOf("<")
                                    int8 = temp5.IndexOf(">")
                                    temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                    temp5 = temp5.Replace("</a>", Nothing)
                                End If

                                generos.Add(temp5)
                            Else
                                Dim j As Integer = 0
                                While j < 100
                                    Dim temp6 As String
                                    Dim int6 As Integer

                                    If temp5.Contains(",") Then
                                        int6 = temp5.IndexOf(",")
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        If temp6.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp6.IndexOf("<")
                                            int8 = temp6.IndexOf(">")
                                            temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                            temp6 = temp6.Replace("</a>", Nothing)
                                        End If

                                        generos.Add(temp6.Trim)

                                        temp5 = temp5.Remove(0, int6 + 1)
                                    Else
                                        If temp5.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp5.IndexOf("<")
                                            int8 = temp5.IndexOf(">")
                                            temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                            temp5 = temp5.Replace("</a>", Nothing)
                                        End If

                                        generos.Add(temp5.Trim)
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim tags As New List(Of String)

                        If html.Contains("<strong>Tags:</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Tags:</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("<br>")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Tags:", Nothing)
                            temp5 = temp5.Trim

                            If Not temp5.Contains(",") Then
                                If temp5.Contains("<") Then
                                    Dim int7, int8 As Integer

                                    int7 = temp5.IndexOf("<")
                                    int8 = temp5.IndexOf(">")
                                    temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                    temp5 = temp5.Replace("</a>", Nothing)
                                End If

                                If temp5.Contains("(") Then
                                    Dim int7, int8 As Integer

                                    int7 = temp5.IndexOf("(")
                                    int8 = temp5.IndexOf(")")
                                    temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                End If

                                tags.Add(temp5)
                            Else
                                Dim j As Integer = 0
                                While j < 100
                                    Dim temp6 As String
                                    Dim int6 As Integer

                                    If temp5.Contains(",") Then
                                        int6 = temp5.IndexOf(",")
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        If temp6.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp6.IndexOf("<")
                                            int8 = temp6.IndexOf(">")
                                            temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                            temp6 = temp6.Replace("</a>", Nothing)
                                        End If

                                        If temp6.Contains("(") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp6.IndexOf("(")
                                            int8 = temp6.IndexOf(")")
                                            temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                        End If

                                        tags.Add(temp6.Trim)

                                        temp5 = temp5.Remove(0, int6 + 1)
                                    Else
                                        If temp5.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp5.IndexOf("<")
                                            int8 = temp5.IndexOf(">")
                                            temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                            temp5 = temp5.Replace("</a>", Nothing)
                                        End If

                                        If temp5.Contains("(") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp5.IndexOf("(")
                                            int8 = temp5.IndexOf(")")
                                            temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                        End If

                                        tags.Add(temp5.Trim)
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim idiomas As New List(Of String)

                        If html.Contains("<strong>Languages:</strong>") Then
                            Dim temp4, temp5 As String
                            Dim int4, int5 As Integer

                            int4 = html.IndexOf("<strong>Languages:</strong>")
                            temp4 = html.Remove(0, int4)

                            int5 = temp4.IndexOf("<br>")
                            temp5 = temp4.Remove(int5, temp4.Length - int5)

                            temp5 = temp5.Replace("<strong>", Nothing)
                            temp5 = temp5.Replace("</strong>", Nothing)
                            temp5 = temp5.Replace("Languages:", Nothing)
                            temp5 = temp5.Replace("[b]*[/b]", Nothing)
                            temp5 = temp5.Replace(";", Nothing)
                            temp5 = temp5.Trim

                            If Not temp5.Contains(",") Then
                                If temp5.Contains("<") Then
                                    Dim int7, int8 As Integer

                                    int7 = temp5.IndexOf("<")
                                    int8 = temp5.IndexOf(">")
                                    temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                    temp5 = temp5.Replace("</a>", Nothing)
                                End If

                                If temp5.Contains("(") Then
                                    Dim int7, int8 As Integer

                                    int7 = temp5.IndexOf("(")
                                    int8 = temp5.IndexOf(")")
                                    temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                End If

                                idiomas.Add(temp5.Trim)
                            Else
                                Dim j As Integer = 0
                                While j < 100
                                    Dim temp6 As String
                                    Dim int6 As Integer

                                    If temp5.Contains(",") Then
                                        int6 = temp5.IndexOf(",")
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        If temp6.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp6.IndexOf("<")
                                            int8 = temp6.IndexOf(">")
                                            temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                            temp6 = temp6.Replace("</a>", Nothing)
                                        End If

                                        If temp6.Contains("(") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp6.IndexOf("(")
                                            int8 = temp6.IndexOf(")")
                                            temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                        End If

                                        idiomas.Add(temp6.Trim)

                                        temp5 = temp5.Remove(0, int6 + 1)
                                    Else
                                        If temp5.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp5.IndexOf("<")
                                            int8 = temp5.IndexOf(">")
                                            temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                            temp5 = temp5.Replace("</a>", Nothing)
                                        End If

                                        If temp5.Contains("(") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp5.IndexOf("(")
                                            int8 = temp5.IndexOf(")")
                                            temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                        End If

                                        idiomas.Add(temp5.Trim)
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim juego As New Juego(titulo, listaJuegosID(i), userscore, metascore, año, categorias, generos, tags, idiomas)

                        listaJuegos.Add(juego)
                    End If
                End If

                prProgreso.Value = (i / listaJuegosID.Count) * 100
                tbProgreso.Text = "(" + i.ToString + "/" + listaJuegosID.Count.ToString + ")"
                i += 1
            Next
        End If

        tbJuegosApp.Text = listaJuegos.Count.ToString

        Await helper.SaveFileAsync(Of List(Of Juego))("listaJuegos", listaJuegos)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        If listaJuegos.Count > 0 Then
            GenerarCategorias(listaJuegos)
            GenerarGeneros(listaJuegos)
            GenerarTags(listaJuegos)
            GenerarIdiomas(listaJuegos)

            Toast("Steam Categories", recursos.GetString("Cargado Si"))
        Else
            Toast("Steam Categories", recursos.GetString("Cargado No"))
        End If

        gridProgreso.Visibility = Visibility.Collapsed
        botonCargaCategorias.IsEnabled = True
        botonBorrarCategorias.IsEnabled = True
        botonBorrarCategoriasApp.IsEnabled = True
        botonSteamRuta.IsEnabled = True
        botonCuenta.IsEnabled = True
        tbSteamCuenta.IsEnabled = True
        cbActualizar.IsEnabled = True

    End Sub

    Public Sub GenerarCategorias(listaJuegos As List(Of Juego))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gv As GridView = pagina.FindName("gvCategorias")
        gv.Items.Clear()

        AddHandler gv.ItemClick, AddressOf GvItemClick

        Dim listaCategorias As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each categoria_ In listaJuegos(i).Categorias
                        If listaCategorias.Count > 0 Then
                            Dim boolCategoria As Boolean = False

                            Dim j As Integer = 0
                            While j < listaCategorias.Count
                                If categoria_ = listaCategorias(j) Then
                                    boolCategoria = True
                                End If
                                j += 1
                            End While

                            If boolCategoria = False Then
                                listaCategorias.Add(categoria_)
                            End If
                        Else
                            listaCategorias.Add(categoria_)
                        End If
                    Next
                    i += 1
                End While

                listaCategorias.Sort()

                For Each categoria In listaCategorias
                    If categoria.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = categoria
                        }

                        Dim cb As New CheckBox With {
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = categoria,
                            .TextWrapping = TextWrapping.Wrap,
                            .FontSize = 14,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .MaxWidth = 165
                        }

                        sp.Children.Add(tb)

                        gv.Items.Add(sp)
                    End If
                Next
            End If
        End If

    End Sub

    Public Sub GenerarGeneros(listaJuegos As List(Of Juego))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gv As GridView = pagina.FindName("gvGeneros")
        gv.Items.Clear()

        AddHandler gv.ItemClick, AddressOf GvItemClick

        Dim listaGeneros As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each genero_ In listaJuegos(i).Generos
                        If listaGeneros.Count > 0 Then
                            Dim boolGenero As Boolean = False

                            Dim j As Integer = 0
                            While j < listaGeneros.Count
                                If genero_ = listaGeneros(j) Then
                                    boolGenero = True
                                End If
                                j += 1
                            End While

                            If boolGenero = False Then
                                listaGeneros.Add(genero_)
                            End If
                        Else
                            listaGeneros.Add(genero_)
                        End If
                    Next
                    i += 1
                End While

                listaGeneros.Sort()

                For Each genero In listaGeneros
                    If genero.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = genero
                        }

                        Dim cb As New CheckBox With {
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = genero,
                            .TextWrapping = TextWrapping.Wrap,
                            .FontSize = 14,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .MaxWidth = 165
                        }

                        sp.Children.Add(tb)

                        gv.Items.Add(sp)
                    End If
                Next
            End If
        End If

    End Sub

    Public Sub GenerarTags(listaJuegos As List(Of Juego))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gv As GridView = pagina.FindName("gvTags")
        gv.Items.Clear()

        AddHandler gv.ItemClick, AddressOf GvItemClick

        Dim listaTags As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each tag_ In listaJuegos(i).Tags
                        If listaTags.Count > 0 Then
                            Dim boolTag As Boolean = False

                            Dim j As Integer = 0
                            While j < listaTags.Count
                                If tag_ = listaTags(j) Then
                                    boolTag = True
                                End If
                                j += 1
                            End While

                            If boolTag = False Then
                                listaTags.Add(tag_)
                            End If
                        Else
                            listaTags.Add(tag_)
                        End If
                    Next
                    i += 1
                End While

                listaTags.Sort()

                For Each tag In listaTags
                    If tag.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = tag
                        }

                        Dim cb As New CheckBox With {
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = tag,
                            .TextWrapping = TextWrapping.Wrap,
                            .FontSize = 14,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .MaxWidth = 165
                        }

                        sp.Children.Add(tb)

                        gv.Items.Add(sp)
                    End If
                Next
            End If
        End If

    End Sub

    Public Sub GenerarIdiomas(listaJuegos As List(Of Juego))

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gv As GridView = pagina.FindName("gvIdiomas")
        gv.Items.Clear()

        AddHandler gv.ItemClick, AddressOf GvItemClick

        Dim listaIdiomas As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each idioma_ In listaJuegos(i).Idiomas
                        If listaIdiomas.Count > 0 Then
                            Dim boolIdioma As Boolean = False

                            Dim j As Integer = 0
                            While j < listaIdiomas.Count
                                If idioma_ = listaIdiomas(j) Then
                                    boolIdioma = True
                                End If
                                j += 1
                            End While

                            If boolIdioma = False Then
                                listaIdiomas.Add(idioma_)
                            End If
                        Else
                            listaIdiomas.Add(idioma_)
                        End If
                    Next
                    i += 1
                End While

                listaIdiomas.Sort()

                For Each idioma In listaIdiomas
                    If idioma.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = idioma
                        }

                        Dim cb As New CheckBox With {
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = idioma,
                            .TextWrapping = TextWrapping.Wrap,
                            .FontSize = 14,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .MaxWidth = 165
                        }

                        sp.Children.Add(tb)

                        gv.Items.Add(sp)
                    End If
                Next
            End If
        End If

    End Sub

    Private Async Sub GvItemClick(sender As Object, e As ItemClickEventArgs)

        Dim sp As StackPanel = e.ClickedItem
        Dim categoria As String = sp.Tag

        Dim cb As CheckBox = sp.Children.Item(0)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim listaCategorias As List(Of String)

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            listaCategorias = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")
        Else
            listaCategorias = New List(Of String)
        End If

        If cb.IsChecked = False Then
            If listaCategorias.Count > 0 Then
                Dim boolCategoria As Boolean = False

                Dim j As Integer = 0
                While j < listaCategorias.Count
                    If categoria = listaCategorias(j) Then
                        boolCategoria = True
                    End If
                    j += 1
                End While

                If boolCategoria = False Then
                    listaCategorias.Add(categoria)
                End If
            Else
                listaCategorias.Add(categoria)
            End If

            cb.IsChecked = True
        Else
            listaCategorias.Remove(categoria)
            cb.IsChecked = False
        End If

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim boton As Button = pagina.FindName("botonEscribirCategorias")

        If listaCategorias.Count > 0 Then
            boton.IsEnabled = True
        Else
            boton.IsEnabled = False
        End If

        Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)

    End Sub

End Module