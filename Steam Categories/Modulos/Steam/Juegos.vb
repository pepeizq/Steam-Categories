Imports System.Net
Imports Microsoft.Toolkit.Uwp.Helpers

Module Juegos

    Public Async Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbBusquedaJuego As TextBox = pagina.FindName("tbBusquedaJuego")
        tbBusquedaJuego.IsEnabled = False

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")
        lvJuegos.Visibility = Visibility.Collapsed

        Dim gridMensaje As Grid = pagina.FindName("gridMensaje")
        gridMensaje.Visibility = Visibility.Collapsed

        Dim gridProgreso As Grid = pagina.FindName("gridProgreso")
        gridProgreso.Visibility = Visibility.Visible

        Dim gridProgreso2 As Grid = pagina.FindName("gridProgreso2")
        gridProgreso2.Visibility = Visibility.Visible

        Dim prProgreso As ProgressBar = pagina.FindName("prProgreso")
        prProgreso.IsIndeterminate = False

        Dim prProgreso2 As ProgressBar = pagina.FindName("prProgreso2")
        prProgreso2.IsIndeterminate = False

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")

        Dim tbProgreso2 As TextBlock = pagina.FindName("tbProgreso2")

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

        Dim tbCategoriasPersonalizadas As TextBox = pagina.FindName("tbCategoriasPersonalizadas")
        tbCategoriasPersonalizadas.IsEnabled = False

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

        Dim listaCustom As New List(Of String)

        If Await helper.FileExistsAsync("categoriascustom") = True Then
            Dim contenido As String = Await helper.ReadFileAsync(Of String)("categoriascustom")

            If Not contenido = Nothing Then
                Dim j As Integer = 0
                While j < 1000
                    If contenido.Contains(",") Then
                        Dim int19 As Integer = contenido.IndexOf(",")
                        Dim temp19 As String = contenido.Remove(0, int19 + 1)
                        Dim temp20 As String = contenido.Remove(int19, contenido.Length - int19)

                        If temp20.Trim.Length > 0 Then
                            listaCustom.Add(temp20.Trim)
                        End If

                        contenido = temp19
                    Else
                        If contenido.Trim.Length > 0 Then
                            listaCustom.Add(contenido.Trim)
                        End If
                        Exit While
                    End If
                    j += 1
                End While
            End If
        End If

        prProgreso.Value = 0
        prProgreso2.Value = 0
        tbProgreso.Text = String.Empty
        tbProgreso2.Text = String.Empty

        If Not listaJuegosID Is Nothing Then
            Dim i As Integer = 0

            While i < listaJuegosID.Count
                Dim boolAñadir As Boolean = False

                If Not listaJuegos Is Nothing Then
                    For Each juego In listaJuegos
                        Dim listaCustomFinal As New List(Of Categoria)

                        For Each item In listaCustom
                            listaCustomFinal.Add(New Categoria(item, False, juego.ID))
                        Next

                        juego.Custom = listaCustomFinal

                        If listaJuegosID(i) = juego.ID Then
                            boolAñadir = True
                        End If
                    Next
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

                            temp6 = Math.Round(Double.Parse(temp6), 0)

                            userscore = New Categoria(temp6.Trim, False, listaJuegosID(i))
                        End If

                        Dim listaTags As New List(Of Categoria)

                        If html.Contains(">store_tags</td>") Then
                            Dim temp7, temp8 As String
                            Dim int7, int8 As Integer

                            int7 = html.IndexOf(">store_tags</td>")
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

                                    listaTags.Add(New Categoria(temp10.Trim, False, listaJuegosID(i)))
                                Else
                                    Exit While
                                End If
                                j += 1
                            End While
                        End If

                        Dim listaCategorias As New List(Of Categoria)

                        If html.Contains(ChrW(34) + "header-thing header-thing-full" + ChrW(34)) Then
                            Dim temp11, temp12 As String
                            Dim int11, int12 As Integer

                            int11 = html.IndexOf(ChrW(34) + "header-thing header-thing-full" + ChrW(34))
                            temp11 = html.Remove(0, int11)

                            int12 = temp11.IndexOf("</div>")
                            temp12 = temp11.Remove(int12, temp11.Length - int12)

                            j = 0
                            While j < 50
                                If temp12.Contains("aria-label=") Then
                                    Dim temp13, temp14 As String
                                    Dim int13, int14 As Integer

                                    int13 = temp12.IndexOf("aria-label=")
                                    temp13 = temp12.Remove(0, int13 + 12)

                                    temp12 = temp13

                                    int14 = temp13.IndexOf(ChrW(34))
                                    temp14 = temp13.Remove(int14, temp13.Length - int14)

                                    temp14 = temp14.Trim
                                    temp14 = WebUtility.HtmlDecode(temp14)

                                    listaCategorias.Add(New Categoria(temp14.Trim, False, listaJuegosID(i)))
                                End If
                                j += 1
                            End While
                        End If

                        Dim listaGeneros As New List(Of Categoria)

                        If html.Contains(">Genres</td>") Then
                            Dim temp15, temp16 As String
                            Dim int15, int16 As Integer

                            int15 = html.IndexOf(">Genres</td>")
                            temp15 = html.Remove(0, int15 + 4)

                            int15 = temp15.IndexOf("<td>")
                            temp15 = temp15.Remove(0, int15 + 4)

                            int16 = temp15.IndexOf("</td>")
                            temp16 = temp15.Remove(int16, temp15.Length - int16)

                            If temp16.Trim.Length > 0 Then
                                temp16 = temp16.Trim
                                temp16 = WebUtility.HtmlDecode(temp16)

                                j = 0
                                While j < 50
                                    If temp16.Contains(",") Then
                                        Dim temp17, temp18 As String
                                        Dim int17 As Integer

                                        int17 = temp16.IndexOf(",")
                                        temp17 = temp16.Remove(0, int17 + 1)

                                        temp18 = temp16.Remove(int17, temp16.Length - int17)

                                        temp16 = temp17

                                        listaGeneros.Add(New Categoria(temp18.Trim, False, listaJuegosID(i)))
                                    Else
                                        listaGeneros.Add(New Categoria(temp16.Trim, False, listaJuegosID(i)))
                                        Exit While
                                    End If
                                    j += 1
                                End While
                            End If
                        End If

                        Dim listaCustomFinal As New List(Of Categoria)

                        For Each item In listaCustom
                            listaCustomFinal.Add(New Categoria(item, False, listaJuegosID(i)))
                        Next

                        Dim juego As New Juego(titulo, imagen, listaJuegosID(i), userscore, Nothing, listaCategorias, Nothing, listaGeneros, Nothing, listaTags, Nothing, listaCustomFinal, Nothing)

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
                prProgreso2.Value = (i / listaJuegosID.Count) * 100
                tbProgreso.Text = "(" + i.ToString + "/" + listaJuegosID.Count.ToString + ")"
                tbProgreso2.Text = "(" + i.ToString + "/" + listaJuegosID.Count.ToString + ")"
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

            Toast("Steam Categories", recursos.GetString("CategoriesLoaded"))

            tbBusquedaJuego.IsEnabled = True
            lvJuegos.Visibility = Visibility.Visible
            gridMensaje.Visibility = Visibility.Collapsed
        Else
            tbBusquedaJuego.IsEnabled = False
            lvJuegos.Visibility = Visibility.Collapsed
            gridMensaje.Visibility = Visibility.Visible
        End If

        gridProgreso.Visibility = Visibility.Collapsed
        gridProgreso2.Visibility = Visibility.Collapsed

        If Not listaJuegosID Is Nothing Then
            botonCargaCategorias.IsEnabled = True
        Else
            botonCargaCategorias.IsEnabled = False
        End If

        botonLimpiarTodo.IsEnabled = True
        botonSteamRuta.IsEnabled = True
        botonCuenta.IsEnabled = True
        tbSteamCuenta.IsEnabled = True
        tbCategoriasPersonalizadas.IsEnabled = True

    End Sub

End Module
