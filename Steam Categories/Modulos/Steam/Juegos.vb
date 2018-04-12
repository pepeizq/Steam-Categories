Imports System.Net
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.UI.Core

Module Juegos

    Public Async Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")
        lvJuegos.IsEnabled = False

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

        Dim helper As New LocalObjectStorageHelper

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

            While i < 40 ' listaJuegosID.Count
                Dim boolAñadir As Boolean = False

                If actualizar = True Then
                    If Not listaJuegos Is Nothing Then
                        For Each juego In listaJuegos
                            If listaJuegosID(i) = juego.ID Then
                                boolAñadir = True
                            End If
                        Next
                    End If
                End If

                If boolAñadir = False Then
                    Dim html As String = Await Decompiladores.HttpClient(New Uri("https://steamdb.info/app/" + listaJuegosID(i) + "/info/"))

                    If Not html = Nothing Then
                        Dim temp, temp2 As String
                        Dim int, int2 As Integer

                        int = html.IndexOf(">Name</")
                        temp = html.Remove(0, int + 5)

                        int2 = temp.IndexOf("</tr>")
                        temp2 = temp.Remove(int2, temp.Length - int2)

                        Dim j As Integer = 0
                        While j < 5
                            If temp2.Contains("<") Then
                                int = temp2.IndexOf("<")
                                int2 = temp2.IndexOf(">") + 1
                                temp2 = temp2.Remove(int, int2 - int)
                            End If
                            j += 1
                        End While

                        temp2 = temp2.Trim
                        temp2 = WebUtility.HtmlDecode(temp2)

                        Dim titulo As String = temp2

                        Dim imagen As String = Nothing

                        If html.Contains(">logo_small</") Then
                            Dim temp3, temp4 As String
                            Dim int3, int4 As Integer

                            int3 = html.IndexOf(">logo_small</")
                            temp3 = html.Remove(0, int3)

                            int4 = temp3.IndexOf("href=")
                            temp4 = temp3.Remove(0, int4 + 6)

                            int4 = temp4.IndexOf(ChrW(34))
                            temp4 = temp4.Remove(int4, temp4.Length - int4)

                            imagen = temp4.Trim
                        End If

                        Dim userscore As Categoria = Nothing

                        If html.Contains(ChrW(34) + "ratingValue" + ChrW(34)) Then
                            Dim temp5, temp6 As String
                            Dim int5, int6 As Integer

                            int5 = html.IndexOf(ChrW(34) + "ratingValue" + ChrW(34))
                            temp5 = html.Remove(0, int5)

                            int5 = temp5.IndexOf("content=")
                            temp5 = temp5.Remove(0, int5 + 9)

                            int6 = temp5.IndexOf(ChrW(34))
                            temp6 = temp5.Remove(int6, temp5.Length - int6)

                            temp6 = Math.Round(Double.Parse(temp6.Replace(".", ",")), 0)

                            userscore = New Categoria(temp6.Trim, False)
                        End If

                        Dim listaTags As New List(Of Categoria)

                        If html.Contains(">store_tags<") Then
                            Dim temp7, temp8 As String
                            Dim int7, int8 As Integer

                            int7 = html.IndexOf(">store_tags<")
                            temp7 = html.Remove(0, int7)

                            int8 = temp7.IndexOf("</tr>")
                            temp8 = temp7.Remove(int8, temp7.Length - int8)

                            j = 0
                            While j < 50
                                If temp8.Contains("<a href=") Then
                                    Dim temp9, temp10 As String
                                    Dim int9, int10 As Integer

                                    int9 = temp8.IndexOf("<a href=")
                                    temp9 = temp8.Remove(0, int9 + 5)

                                    temp8 = temp9

                                    int9 = temp9.IndexOf(">")
                                    temp9 = temp9.Remove(0, int9 + 1)

                                    int10 = temp9.IndexOf("<")
                                    temp10 = temp9.Remove(int10, temp9.Length - int10)

                                    temp10 = temp10.Trim
                                    temp10 = WebUtility.HtmlDecode(temp10)

                                    listaTags.Add(New Categoria(temp10.Trim, False))
                                Else
                                    Exit While
                                End If
                                j += 1
                            End While
                        End If




                        '    Dim metascore As String = Nothing

                        '    If html.Contains("<strong>Metascore:</strong>") Then
                        '        Dim temp4, temp5 As String
                        '        Dim int4, int5 As Integer

                        '        int4 = html.IndexOf("<strong>Metascore:</strong>")
                        '        temp4 = html.Remove(0, int4)

                        '        int5 = temp4.IndexOf("%")
                        '        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        '        temp5 = temp5.Replace("<strong>", Nothing)
                        '        temp5 = temp5.Replace("</strong>", Nothing)
                        '        temp5 = temp5.Replace("Metascore:", Nothing)
                        '        temp5 = temp5.Trim

                        '        metascore = temp5
                        '    End If

                        '    Dim años As New List(Of Categoria)

                        '    If html.Contains("<strong>Release date</strong>") Then
                        '        Dim temp4, temp5 As String
                        '        Dim int4, int5 As Integer

                        '        int4 = html.IndexOf("<strong>Release date</strong>")
                        '        temp4 = html.Remove(0, int4)

                        '        int5 = temp4.IndexOf("<br>")
                        '        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        '        temp5 = temp5.Replace("<strong>", Nothing)
                        '        temp5 = temp5.Replace("</strong>", Nothing)
                        '        temp5 = temp5.Replace("Release date", Nothing)
                        '        temp5 = temp5.Replace(":", Nothing)

                        '        If temp5.Contains(",") Then
                        '            Dim int6 As Integer

                        '            int6 = temp5.IndexOf(",")
                        '            temp5 = temp5.Remove(0, int6 + 1)
                        '        End If

                        '        If temp5.Contains("19") Then
                        '            Dim int6 As Integer

                        '            int6 = temp5.IndexOf("19")
                        '            temp5 = temp5.Remove(0, int6)

                        '            If (temp5.Length - (int6 + 4)) > 0 Then
                        '                temp5 = temp5.Remove(int6 + 4, temp5.Length - (int6 + 4))
                        '            End If
                        '        End If

                        '        If temp5.Contains("20") Then
                        '            Dim int6 As Integer

                        '            int6 = temp5.IndexOf("20")
                        '            temp5 = temp5.Remove(0, int6)

                        '            If (temp5.Length - (int6 + 4)) > 0 Then
                        '                temp5 = temp5.Remove(int6 + 4, temp5.Length - (int6 + 4))
                        '            End If
                        '        End If

                        '        If temp5.Contains("<") Then
                        '            Dim int6 As Integer

                        '            int6 = temp5.IndexOf("<")
                        '            temp5 = temp5.Remove(int6, temp5.Length - int6)
                        '        End If

                        '        años.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(2).Tag))
                        '    End If

                        '    Dim categorias As New List(Of Categoria)

                        '    If html.Contains("<strong>Category:</strong>") Then
                        '        Dim temp4, temp5 As String
                        '        Dim int4, int5 As Integer

                        '        int4 = html.IndexOf("<strong>Category:</strong>")
                        '        temp4 = html.Remove(0, int4)

                        '        int5 = temp4.IndexOf("<br>")
                        '        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        '        temp5 = temp5.Replace("<strong>", Nothing)
                        '        temp5 = temp5.Replace("</strong>", Nothing)
                        '        temp5 = temp5.Replace("Category:", Nothing)
                        '        temp5 = temp5.Trim

                        '        If Not temp5.Contains(",") Then
                        '            categorias.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(3).Tag))
                        '        Else
                        '            Dim j As Integer = 0
                        '            While j < 100
                        '                Dim temp6 As String
                        '                Dim int6 As Integer

                        '                If temp5.Contains(",") Then
                        '                    int6 = temp5.IndexOf(",")
                        '                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                        '                    categorias.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(3).Tag))

                        '                    temp5 = temp5.Remove(0, int6 + 1)
                        '                Else
                        '                    categorias.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(3).Tag))
                        '                    Exit While
                        '                End If
                        '                j += 1
                        '            End While
                        '        End If
                        '    End If

                        '    Dim generos As New List(Of Categoria)

                        '    If html.Contains("<strong>Genre:</strong>") Then
                        '        Dim temp4, temp5 As String
                        '        Dim int4, int5 As Integer

                        '        int4 = html.IndexOf("<strong>Genre:</strong>")
                        '        temp4 = html.Remove(0, int4)

                        '        int5 = temp4.IndexOf("<br>")
                        '        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        '        temp5 = temp5.Replace("<strong>", Nothing)
                        '        temp5 = temp5.Replace("</strong>", Nothing)
                        '        temp5 = temp5.Replace("Genre:", Nothing)
                        '        temp5 = temp5.Trim

                        '        If Not temp5.Contains(",") Then
                        '            If temp5.Contains("<") Then
                        '                Dim int7, int8 As Integer

                        '                int7 = temp5.IndexOf("<")
                        '                int8 = temp5.IndexOf(">")
                        '                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                temp5 = temp5.Replace("</a>", Nothing)
                        '            End If

                        '            generos.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(4).Tag))
                        '        Else
                        '            Dim j As Integer = 0
                        '            While j < 100
                        '                Dim temp6 As String
                        '                Dim int6 As Integer

                        '                If temp5.Contains(",") Then
                        '                    int6 = temp5.IndexOf(",")
                        '                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                        '                    If temp6.Contains("<") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp6.IndexOf("<")
                        '                        int8 = temp6.IndexOf(">")
                        '                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                        '                        temp6 = temp6.Replace("</a>", Nothing)
                        '                    End If

                        '                    generos.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(4).Tag))

                        '                    temp5 = temp5.Remove(0, int6 + 1)
                        '                Else
                        '                    If temp5.Contains("<") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp5.IndexOf("<")
                        '                        int8 = temp5.IndexOf(">")
                        '                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                        temp5 = temp5.Replace("</a>", Nothing)
                        '                    End If

                        '                    generos.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(4).Tag))
                        '                    Exit While
                        '                End If
                        '                j += 1
                        '            End While
                        '        End If
                        '    End If

                        '    Dim tags As New List(Of Categoria)

                        '    If html.Contains("<strong>Tags:</strong>") Then
                        '        Dim temp4, temp5 As String
                        '        Dim int4, int5 As Integer

                        '        int4 = html.IndexOf("<strong>Tags:</strong>")
                        '        temp4 = html.Remove(0, int4)

                        '        int5 = temp4.IndexOf("<br>")
                        '        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        '        temp5 = temp5.Replace("<strong>", Nothing)
                        '        temp5 = temp5.Replace("</strong>", Nothing)
                        '        temp5 = temp5.Replace("Tags:", Nothing)
                        '        temp5 = temp5.Trim

                        '        If Not temp5.Contains(",") Then
                        '            If temp5.Contains("<") Then
                        '                Dim int7, int8 As Integer

                        '                int7 = temp5.IndexOf("<")
                        '                int8 = temp5.IndexOf(">")
                        '                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                temp5 = temp5.Replace("</a>", Nothing)
                        '            End If

                        '            If temp5.Contains("(") Then
                        '                Dim int7, int8 As Integer

                        '                int7 = temp5.IndexOf("(")
                        '                int8 = temp5.IndexOf(")")
                        '                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '            End If

                        '            tags.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(5).Tag))
                        '        Else
                        '            Dim j As Integer = 0
                        '            While j < 100
                        '                Dim temp6 As String
                        '                Dim int6 As Integer

                        '                If temp5.Contains(",") Then
                        '                    int6 = temp5.IndexOf(",")
                        '                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                        '                    If temp6.Contains("<") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp6.IndexOf("<")
                        '                        int8 = temp6.IndexOf(">")
                        '                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                        '                        temp6 = temp6.Replace("</a>", Nothing)
                        '                    End If

                        '                    If temp6.Contains("(") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp6.IndexOf("(")
                        '                        int8 = temp6.IndexOf(")")
                        '                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                        '                    End If

                        '                    tags.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(5).Tag))

                        '                    temp5 = temp5.Remove(0, int6 + 1)
                        '                Else
                        '                    If temp5.Contains("<") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp5.IndexOf("<")
                        '                        int8 = temp5.IndexOf(">")
                        '                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                        temp5 = temp5.Replace("</a>", Nothing)
                        '                    End If

                        '                    If temp5.Contains("(") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp5.IndexOf("(")
                        '                        int8 = temp5.IndexOf(")")
                        '                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                    End If

                        '                    tags.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(5).Tag))
                        '                    Exit While
                        '                End If
                        '                j += 1
                        '            End While
                        '        End If
                        '    End If

                        '    Dim idiomas As New List(Of Categoria)

                        '    If html.Contains("<strong>Languages:</strong>") Then
                        '        Dim temp4, temp5 As String
                        '        Dim int4, int5 As Integer

                        '        int4 = html.IndexOf("<strong>Languages:</strong>")
                        '        temp4 = html.Remove(0, int4)

                        '        int5 = temp4.IndexOf("<br>")
                        '        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        '        temp5 = temp5.Replace("<strong>", Nothing)
                        '        temp5 = temp5.Replace("</strong>", Nothing)
                        '        temp5 = temp5.Replace("Languages:", Nothing)
                        '        temp5 = temp5.Replace("[b]*[/b]", Nothing)
                        '        temp5 = temp5.Replace(";", Nothing)
                        '        temp5 = temp5.Trim

                        '        If Not temp5.Contains(",") Then
                        '            If temp5.Contains("<") Then
                        '                Dim int7, int8 As Integer

                        '                int7 = temp5.IndexOf("<")
                        '                int8 = temp5.IndexOf(">")
                        '                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                temp5 = temp5.Replace("</a>", Nothing)
                        '            End If

                        '            If temp5.Contains("(") Then
                        '                Dim int7, int8 As Integer

                        '                int7 = temp5.IndexOf("(")
                        '                int8 = temp5.IndexOf(")")
                        '                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '            End If

                        '            idiomas.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(6).Tag))
                        '        Else
                        '            Dim j As Integer = 0
                        '            While j < 100
                        '                Dim temp6 As String
                        '                Dim int6 As Integer

                        '                If temp5.Contains(",") Then
                        '                    int6 = temp5.IndexOf(",")
                        '                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                        '                    If temp6.Contains("<") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp6.IndexOf("<")
                        '                        int8 = temp6.IndexOf(">")
                        '                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                        '                        temp6 = temp6.Replace("</a>", Nothing)
                        '                    End If

                        '                    If temp6.Contains("(") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp6.IndexOf("(")
                        '                        int8 = temp6.IndexOf(")")
                        '                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                        '                    End If

                        '                    idiomas.Add(New Categoria(temp6.Trim, False, lvCategorias.Items(6).Tag))

                        '                    temp5 = temp5.Remove(0, int6 + 1)
                        '                Else
                        '                    If temp5.Contains("<") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp5.IndexOf("<")
                        '                        int8 = temp5.IndexOf(">")
                        '                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                        temp5 = temp5.Replace("</a>", Nothing)
                        '                    End If

                        '                    If temp5.Contains("(") Then
                        '                        Dim int7, int8 As Integer

                        '                        int7 = temp5.IndexOf("(")
                        '                        int8 = temp5.IndexOf(")")
                        '                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                        '                    End If

                        '                    idiomas.Add(New Categoria(temp5.Trim, False, lvCategorias.Items(6).Tag))
                        '                    Exit While
                        '                End If
                        '                j += 1
                        '            End While
                        '        End If
                        '    End If

                        Dim juego As New Juego(titulo, imagen, listaJuegosID(i), userscore, Nothing, Nothing, Nothing, Nothing, listaTags, Nothing)

                        Dim idBool As Boolean = False
                        Dim k As Integer = 0
                        While k < listaJuegos.Count
                            If listaJuegos(k).ID = juego.ID Then
                                idBool = True
                            End If
                            k += 1
                        End While

                        If idBool = False Then
                            listaJuegos.Add(juego)
                        End If
                    End If
                End If

                prProgreso.Value = (i / listaJuegosID.Count) * 100
                tbProgreso.Text = "(" + i.ToString + "/" + listaJuegosID.Count.ToString + ")"
                i += 1
            End While
        End If

        tbJuegosApp.Text = listaJuegos.Count.ToString

        Await helper.SaveFileAsync(Of List(Of Juego))("listaJuegos", listaJuegos)

        Dim recursos As New Resources.ResourceLoader()

        If listaJuegos.Count > 0 Then
            listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

            lvJuegos.Items.Clear()

            For Each juego In listaJuegos
                lvJuegos.Items.Add(Interfaz.AñadirJuegoLista(juego))
            Next

            If actualizar = False Then
                Toast("Steam Categories", recursos.GetString("CategoriesLoaded"))
            Else
                Comprobar()
            End If
        Else
            Toast("Steam Categories", recursos.GetString("CategoriesNotLoaded"))
        End If

        lvJuegos.IsEnabled = True
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

                            'If categoria_.Maestro.ID = categoria.Maestro.ID Then
                            '    If categoria_.Nombre = categoria.Nombre Then
                            '        cb.IsChecked = True
                            '    End If
                            'End If
                        Next

                        For Each cb In gvCategorias.Items
                            Dim categoria_ As Categoria = cb.Tag

                            'If categoria_.Maestro.ID = categoria.Maestro.ID Then
                            '    If categoria_.Nombre = categoria.Nombre Then
                            '        cb.IsChecked = True
                            '    End If
                            'End If
                        Next

                        For Each cb In gvGeneros.Items
                            Dim categoria_ As Categoria = cb.Tag

                            'If categoria_.Maestro.ID = categoria.Maestro.ID Then
                            '    If categoria_.Nombre = categoria.Nombre Then
                            '        cb.IsChecked = True
                            '    End If
                            'End If
                        Next

                        For Each cb In gvTags.Items
                            Dim categoria_ As Categoria = cb.Tag

                            'If categoria_.Maestro.ID = categoria.Maestro.ID Then
                            '    If categoria_.Nombre = categoria.Nombre Then
                            '        cb.IsChecked = True
                            '    End If
                            'End If
                        Next

                        For Each cb In gvIdiomas.Items
                            Dim categoria_ As Categoria = cb.Tag

                            'If categoria_.Maestro.ID = categoria.Maestro.ID Then
                            '    If categoria_.Nombre = categoria.Nombre Then
                            '        cb.IsChecked = True
                            '    End If
                            'End If
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
