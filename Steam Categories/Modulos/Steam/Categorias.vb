Imports Microsoft.Toolkit.Uwp

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

            Dim mostrarAviso As Boolean = True

            If Await helper.FileExistsAsync("actualizar") = True Then
                Dim actualizarModo As Boolean = Await helper.ReadFileAsync(Of Boolean)("actualizar")

                If actualizarModo = True Then
                    mostrarAviso = False
                End If
            End If

            If mostrarAviso = True Then
                Toast("Steam Categories", recursos.GetString("Cargado Si"))
            End If
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

        Dim listaCategorias As New List(Of Categoria)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count
                    For Each categoria_ In listaJuegos(i).Categorias
                        If Not categoria_ = Nothing Then
                            If listaCategorias.Count > 0 Then
                                Dim boolCategoria As Boolean = False

                                Dim j As Integer = 0
                                While j < listaCategorias.Count
                                    If categoria_ = listaCategorias(j).Nombre Then
                                        boolCategoria = True
                                    End If
                                    j += 1
                                End While

                                If boolCategoria = False Then
                                    listaCategorias.Add(New Categoria(categoria_, False, "categoria"))
                                End If
                            Else
                                listaCategorias.Add(New Categoria(categoria_, False, "categoria"))
                            End If
                        End If
                    Next
                    i += 1
                End While

                listaCategorias.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

                For Each categoria In listaCategorias
                    If categoria.Nombre.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = categoria
                        }

                        Dim cb As New CheckBox With {
                            .IsChecked = categoria.Estado,
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .IsHitTestVisible = False
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = categoria.Nombre,
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

        Dim listaGeneros As New List(Of Categoria)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count
                    For Each genero_ In listaJuegos(i).Generos
                        If Not genero_ = Nothing Then
                            If listaGeneros.Count > 0 Then
                                Dim boolGenero As Boolean = False

                                Dim j As Integer = 0
                                While j < listaGeneros.Count
                                    If genero_ = listaGeneros(j).Nombre Then
                                        boolGenero = True
                                    End If
                                    j += 1
                                End While

                                If boolGenero = False Then
                                    listaGeneros.Add(New Categoria(genero_, False, "genero"))
                                End If
                            Else
                                listaGeneros.Add(New Categoria(genero_, False, "genero"))
                            End If
                        End If
                    Next
                    i += 1
                End While

                listaGeneros.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

                For Each categoria In listaGeneros
                    If categoria.Nombre.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = categoria
                        }

                        Dim cb As New CheckBox With {
                            .IsChecked = categoria.Estado,
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .IsHitTestVisible = False
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = categoria.Nombre,
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

        Dim listaTags As New List(Of Categoria)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count
                    For Each tag_ In listaJuegos(i).Tags
                        If Not tag_ = Nothing Then
                            If listaTags.Count > 0 Then
                                Dim boolTag As Boolean = False

                                Dim j As Integer = 0
                                While j < listaTags.Count
                                    If tag_ = listaTags(j).Nombre Then
                                        boolTag = True
                                    End If
                                    j += 1
                                End While

                                If boolTag = False Then
                                    listaTags.Add(New Categoria(tag_, False, "tag"))
                                End If
                            Else
                                listaTags.Add(New Categoria(tag_, False, "tag"))
                            End If
                        End If
                    Next
                    i += 1
                End While

                listaTags.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

                For Each categoria In listaTags
                    If categoria.Nombre.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = categoria
                        }

                        Dim cb As New CheckBox With {
                            .IsChecked = categoria.Estado,
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .IsHitTestVisible = False
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = categoria.Nombre,
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

        Dim listaIdiomas As New List(Of Categoria)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count
                    For Each idioma_ In listaJuegos(i).Idiomas
                        If Not idioma_ = Nothing Then
                            If listaIdiomas.Count > 0 Then
                                Dim boolIdioma As Boolean = False

                                Dim j As Integer = 0
                                While j < listaIdiomas.Count
                                    If idioma_ = listaIdiomas(j).Nombre Then
                                        boolIdioma = True
                                    End If
                                    j += 1
                                End While

                                If boolIdioma = False Then
                                    listaIdiomas.Add(New Categoria(idioma_, False, "idioma"))
                                End If
                            Else
                                listaIdiomas.Add(New Categoria(idioma_, False, "idioma"))
                            End If
                        End If
                    Next
                    i += 1
                End While

                listaIdiomas.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

                For Each categoria In listaIdiomas
                    If categoria.Nombre.Length > 0 Then
                        Dim sp As New StackPanel With {
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Orientation = Orientation.Horizontal,
                            .Width = 200,
                            .Tag = categoria
                        }

                        Dim cb As New CheckBox With {
                            .IsChecked = categoria.Estado,
                            .MinWidth = 30,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .IsHitTestVisible = False
                        }

                        sp.Children.Add(cb)

                        Dim tb As New TextBlock With {
                            .Text = categoria.Nombre,
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

    Public Async Sub AñadirListaCategorias(sp As StackPanel)

        Dim listaCategorias As List(Of Categoria) = Nothing

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        If Await helper.FileExistsAsync("listaCategorias") = True Then
            listaCategorias = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")
        End If

        If listaCategorias Is Nothing Then
            listaCategorias = New List(Of Categoria)
        End If

        Dim categoria As Categoria = sp.Tag
        Dim cb As CheckBox = sp.Children.Item(0)

        If categoria.Estado = False Then
            categoria.Estado = True

            If listaCategorias.Count > 0 Then
                Dim boolCategoria As Boolean = False

                Dim j As Integer = 0
                While j < listaCategorias.Count
                    If categoria.Seccion = listaCategorias(j).Seccion Then
                        If categoria.Nombre = listaCategorias(j).Nombre Then
                            listaCategorias(j).Estado = True
                            boolCategoria = True
                        End If
                    End If
                    j += 1
                End While

                If boolCategoria = False Then
                    listaCategorias.Add(categoria)
                End If
            Else
                listaCategorias.Add(categoria)
            End If
        Else
            categoria.Estado = False

            Dim j As Integer = 0
            While j < listaCategorias.Count
                If categoria.Seccion = listaCategorias(j).Seccion Then
                    If categoria.Nombre = listaCategorias(j).Nombre Then
                        listaCategorias(j).Estado = False
                    End If
                End If
                j += 1
            End While
        End If

        cb.IsChecked = categoria.Estado

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim boton As Button = pagina.FindName("botonEscribirCategorias")
        Dim boolBoton As Boolean = False

        For Each item In listaCategorias
            If item.Estado = True Then
                boolBoton = True
            End If
        Next

        If boolBoton = True Then
            boton.IsEnabled = True
        Else
            boton.IsEnabled = False
        End If

        Try
            Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", listaCategorias)
        Catch ex As Exception

        End Try


    End Sub

    Public Async Sub Comprobar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")

            If Not listaCategorias Is Nothing Then
                For Each categoria In listaCategorias
                    If categoria.Estado = True Then
                        Dim cbUserscore As CheckBox = pagina.FindName("cbSeleccionUserscore")

                        If categoria.Nombre = ("/*1/" + cbUserscore.Content) Then
                            cbUserscore.IsChecked = True
                        End If

                        Dim cbMetascore As CheckBox = pagina.FindName("cbSeleccionMetascore")

                        If categoria.Nombre = ("/*2/" + cbMetascore.Content) Then
                            cbMetascore.IsChecked = True
                        End If

                        Dim cbAños As CheckBox = pagina.FindName("cbSeleccionAños")

                        If categoria.Nombre = ("/*3/" + cbAños.Content) Then
                            cbAños.IsChecked = True
                        End If

                        Dim gvCategorias As GridView = pagina.FindName("gvCategorias")

                        For Each sp As StackPanel In gvCategorias.Items
                            Dim categoria_ As Categoria = sp.Tag

                            If categoria_.Seccion = categoria.Seccion Then
                                If categoria_.Nombre = categoria.Nombre Then
                                    Dim cb As CheckBox = sp.Children.Item(0)
                                    cb.IsChecked = True
                                    MessageBox(cb.IsChecked.ToString)
                                End If
                            End If
                        Next

                        gvCategorias.UpdateLayout()

                        'Dim lista As List(Of Categoria) = gvCategorias.ItemsSource
                        'gvCategorias.ItemsSource = Nothing
                        'For Each item In lista
                        '    If item.Seccion = categoria.Seccion Then
                        '        If item.Nombre = categoria.Nombre Then
                        '            item.Estado = categoria.Estado
                        '        End If
                        '    End If
                        'Next
                        'gvCategorias.ItemsSource = lista
                    End If
                Next
            End If
        End If

    End Sub

End Module
