Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.UI.Core

Module Categorias

    Public Async Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvCategorias As ListView = pagina.FindName("lvCategorias")

        Dim gridProgreso As Grid = pagina.FindName("gridProgreso")
        gridProgreso.Visibility = Visibility.Visible

        Dim prProgreso As ProgressBar = pagina.FindName("prProgreso")
        prProgreso.IsIndeterminate = False

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")

        Dim botonCargaCategorias As Button = pagina.FindName("botonCargaCategorias")
        botonCargaCategorias.IsEnabled = False

        Dim botonLimpiarTodo As Button = pagina.FindName("botonLimpiarTodo")
        botonLimpiarTodo.IsEnabled = False

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
        End If

        If listaJuegos Is Nothing Then
            listaJuegos = New List(Of Juego)
        End If

        Dim actualizar As Boolean = False

        If Await helper.FileExistsAsync("actualizar") = True Then
            Try
                actualizar = Await helper.ReadFileAsync(Of Boolean)("actualizar")
            Catch ex As Exception

            End Try
        End If

        If Not listaJuegosID Is Nothing Then
            Dim i As Integer = 0

            For Each idJuego In listaJuegosID
                Dim boolAñadir As Boolean = False

                If actualizar = True Then
                    If Not listaJuegos Is Nothing Then
                        For Each juego In listaJuegos
                            If idJuego = juego.ID Then
                                boolAñadir = True
                            End If
                        Next
                    End If
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

                        Dim años As New List(Of Categoria)

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

                            If temp5.Contains("<") Then
                                Dim int6 As Integer

                                int6 = temp5.IndexOf("<")
                                temp5 = temp5.Remove(int6, temp5.Length - int6)
                            End If

                            años.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(2).Tag))
                        End If

                        Dim categorias As New List(Of Categoria)

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
                                categorias.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(3).Tag))
                            Else
                                Dim j As Integer = 0
                                While j < 100
                                    Dim temp6 As String
                                    Dim int6 As Integer

                                    If temp5.Contains(",") Then
                                        int6 = temp5.IndexOf(",")
                                        temp6 = temp5.Remove(int6, temp5.Length - int6)

                                        categorias.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(3).Tag))

                                        temp5 = temp5.Remove(0, int6 + 1)
                                    Else
                                        categorias.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(3).Tag))
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim generos As New List(Of Categoria)

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

                                generos.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(4).Tag))
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

                                        generos.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(4).Tag))

                                        temp5 = temp5.Remove(0, int6 + 1)
                                    Else
                                        If temp5.Contains("<") Then
                                            Dim int7, int8 As Integer

                                            int7 = temp5.IndexOf("<")
                                            int8 = temp5.IndexOf(">")
                                            temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                            temp5 = temp5.Replace("</a>", Nothing)
                                        End If

                                        generos.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(4).Tag))
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim tags As New List(Of Categoria)

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

                                tags.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(5).Tag))
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

                                        tags.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(5).Tag))

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

                                        tags.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(5).Tag))
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim idiomas As New List(Of Categoria)

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

                                idiomas.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(6).Tag))
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

                                        idiomas.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(6).Tag))

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

                                        idiomas.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(6).Tag))
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim juego As New Juego(titulo, listaJuegosID(i), userscore, metascore, años, categorias, generos, tags, idiomas)

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
            Dim gvAños As GridView = pagina.FindName("gvAños")
            Dim gvCategorias As GridView = pagina.FindName("gvCategorias")
            Dim gvGeneros As GridView = pagina.FindName("gvGeneros")
            Dim gvTags As GridView = pagina.FindName("gvTags")
            Dim gvIdiomas As GridView = pagina.FindName("gvIdiomas")

            Dim listaAños As New List(Of Categoria)
            Dim listaCategorias As New List(Of Categoria)
            Dim listaGeneros As New List(Of Categoria)
            Dim listaTags As New List(Of Categoria)
            Dim listaIdiomas As New List(Of Categoria)

            For Each juego In listaJuegos
                listaAños.AddRange(juego.Años)
                listaCategorias.AddRange(juego.Categorias)
                listaGeneros.AddRange(juego.Generos)
                listaTags.AddRange(juego.Tags)
                listaIdiomas.AddRange(juego.Idiomas)
            Next

            Interfaz.RellenarGridsCheckboxes(listaAños, gvAños, lvCategorias.Items(2).Tag)
            Interfaz.RellenarGridsCheckboxes(listaCategorias, gvCategorias, lvCategorias.Items(3).Tag)
            Interfaz.RellenarGridsCheckboxes(listaGeneros, gvGeneros, lvCategorias.Items(4).Tag)
            Interfaz.RellenarGridsCheckboxes(listaTags, gvTags, lvCategorias.Items(5).Tag)
            Interfaz.RellenarGridsCheckboxes(listaIdiomas, gvIdiomas, lvCategorias.Items(6).Tag)

            lvCategorias.IsEnabled = True

            Dim tb As TextBlock = pagina.FindName("tbMensajeCategorias")
            tb.Text = recursos.GetString("MessageCategories2")

            If actualizar = False Then
                Toast("Steam Categories", recursos.GetString("CategoriesLoaded"))
            Else
                Comprobar()
            End If
        Else
            Toast("Steam Categories", recursos.GetString("CategoriesNotLoaded"))
        End If

        gridProgreso.Visibility = Visibility.Collapsed
        botonCargaCategorias.IsEnabled = True
        botonLimpiarTodo.IsEnabled = True
        botonSteamRuta.IsEnabled = True
        botonCuenta.IsEnabled = True
        tbSteamCuenta.IsEnabled = True
        cbActualizar.IsEnabled = True

    End Sub

    Public Async Sub Comprobar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")

            Dim cbUserscore As CheckBox = pagina.FindName("cbSeleccionUserscore")
            Dim cbMetascore As CheckBox = pagina.FindName("cbSeleccionMetascore")

            Dim gvAños As GridView = pagina.FindName("gvAños")
            Dim gvCategorias As GridView = pagina.FindName("gvCategorias")
            Dim gvGeneros As GridView = pagina.FindName("gvGeneros")
            Dim gvTags As GridView = pagina.FindName("gvTags")
            Dim gvIdiomas As GridView = pagina.FindName("gvIdiomas")

            Dim boolBoton As Boolean = False
            Dim contadorTrue As Integer = 0

            If Not listaCategorias Is Nothing Then
                For Each categoria In listaCategorias
                    If categoria.Estado = True Then

                        boolBoton = True
                        contadorTrue += 1

                        If categoria.Nombre = ("/*1/Userscore") Then
                            cbUserscore.IsChecked = True
                        End If

                        If categoria.Nombre = ("/*2/Metascore") Then
                            cbMetascore.IsChecked = True
                        End If

                        For Each cb In gvAños.Items
                            Dim categoria_ As Categoria = cb.Tag

                            If categoria_.Maestro.ID = categoria.Maestro.ID Then
                                If categoria_.Nombre = categoria.Nombre Then
                                    cb.IsChecked = True
                                End If
                            End If
                        Next

                        For Each cb In gvCategorias.Items
                            Dim categoria_ As Categoria = cb.Tag

                            If categoria_.Maestro.ID = categoria.Maestro.ID Then
                                If categoria_.Nombre = categoria.Nombre Then
                                    cb.IsChecked = True
                                End If
                            End If
                        Next

                        For Each cb In gvGeneros.Items
                            Dim categoria_ As Categoria = cb.Tag

                            If categoria_.Maestro.ID = categoria.Maestro.ID Then
                                If categoria_.Nombre = categoria.Nombre Then
                                    cb.IsChecked = True
                                End If
                            End If
                        Next

                        For Each cb In gvTags.Items
                            Dim categoria_ As Categoria = cb.Tag

                            If categoria_.Maestro.ID = categoria.Maestro.ID Then
                                If categoria_.Nombre = categoria.Nombre Then
                                    cb.IsChecked = True
                                End If
                            End If
                        Next

                        For Each cb In gvIdiomas.Items
                            Dim categoria_ As Categoria = cb.Tag

                            If categoria_.Maestro.ID = categoria.Maestro.ID Then
                                If categoria_.Nombre = categoria.Nombre Then
                                    cb.IsChecked = True
                                End If
                            End If
                        Next
                    End If
                Next
            End If

            Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
            botonAñadir.IsEnabled = boolBoton

            Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
            botonLimpiar.IsEnabled = boolBoton

            Dim botonEliminar As Button = pagina.FindName("botonBorrarCategorias")
            botonEliminar.IsEnabled = boolBoton

            If boolBoton = True Then
                Cliente.EscribirCategorias()
            End If

            Dim tb As TextBlock = pagina.FindName("tbNumeroCategorias")

            If Not contadorTrue = 0 Then
                tb.Text = contadorTrue.ToString
            Else
                tb.Text = String.Empty
            End If
        End If

    End Sub

End Module
