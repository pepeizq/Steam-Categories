Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI
Imports Windows.UI.Core

Module Interfaz

    Public Sub GenerarMenu()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lv As ListView = pagina.FindName("lvCategorias")

        Dim recursos As New Resources.ResourceLoader()

        Dim userscore As New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")
        lv.Items.Add(GenerarMenuItem(userscore))

        Dim metascore As New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")
        lv.Items.Add(GenerarMenuItem(metascore))

        Dim años As New CategoriaMaestro(recursos.GetString("Years"), "Assets\Menu\categorias_años.PNG", 2, "gridCategoriasAños")
        lv.Items.Add(GenerarMenuItem(años))

        Dim categorias As New CategoriaMaestro(recursos.GetString("Categories"), "Assets\Menu\categorias_categorias.PNG", 3, "gridCategoriasCategorias")
        lv.Items.Add(GenerarMenuItem(categorias))

        Dim generos As New CategoriaMaestro(recursos.GetString("Genres"), "Assets\Menu\categorias_generos.PNG", 4, "gridCategoriasGeneros")
        lv.Items.Add(GenerarMenuItem(generos))

        Dim etiquetas As New CategoriaMaestro(recursos.GetString("Tags"), "Assets\Menu\categorias_etiquetas.PNG", 5, "gridCategoriasTags")
        lv.Items.Add(GenerarMenuItem(etiquetas))

        Dim idiomas As New CategoriaMaestro(recursos.GetString("Languages"), "Assets\Menu\categorias_idiomas.PNG", 6, "gridCategoriasIdiomas")
        lv.Items.Add(GenerarMenuItem(idiomas))

    End Sub

    Private Function GenerarMenuItem(categoria As CategoriaMaestro)

        Dim tb As New TextBlock With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Padding = New Thickness(15, 15, 15, 15),
            .Text = categoria.Nombre,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim sp As New StackPanel With {
            .Orientation = Orientation.Vertical,
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Tag = categoria
        }

        sp.Children.Add(tb)

        Dim lvitem As New ListViewItem With {
            .Content = sp,
            .Padding = New Thickness(0, 0, 0, 0),
            .HorizontalContentAlignment = HorizontalAlignment.Stretch,
            .MinWidth = 150,
            .Tag = categoria
        }

        AddHandler lvitem.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler lvitem.PointerExited, AddressOf UsuarioSaleBoton

        Return lvitem

    End Function

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    Public Sub ClickeoBarraIzquierda(spClickeado As StackPanel)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim categoriaClickeada As CategoriaMaestro = spClickeado.Tag

        Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + categoriaClickeada.Nombre

        Dim categorias As Grid = pagina.FindName("gridCategoriasMaestro")

        For Each grid As Grid In categorias.Children
            If grid.Name = categoriaClickeada.GridNombre Then
                grid.Visibility = Visibility.Visible
            Else
                grid.Visibility = Visibility.Collapsed
            End If
        Next

        Dim lv As ListView = pagina.FindName("lvCategorias")
        For Each lvitem As ListViewItem In lv.Items
            Dim sp As StackPanel = lvitem.Content

            Dim categoria As CategoriaMaestro = sp.Tag

            If categoria.ID = categoriaClickeada.ID Then
                sp.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            Else
                sp.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
            End If
        Next

    End Sub

    Public Sub RellenarGridsCheckboxes(listaCategoriasJuego As List(Of Categoria), gv As GridView, maestro As CategoriaMaestro)

        gv.Items.Clear()

        Dim lista As New List(Of Categoria)

        If Not listaCategoriasJuego Is Nothing Then
            If listaCategoriasJuego.Count > 0 Then
                For Each categoria In listaCategoriasJuego
                    If Not categoria Is Nothing Then
                        If lista.Count > 0 Then
                            Dim boolCategoria As Boolean = False

                            Dim j As Integer = 0
                            While j < lista.Count
                                If categoria.Nombre = lista(j).Nombre Then
                                    boolCategoria = True
                                End If
                                j += 1
                            End While

                            If boolCategoria = False Then
                                'lista.Add(New Categoria(categoria.Nombre, False))
                            End If
                        Else
                            'lista.Add(New Categoria(categoria.Nombre, False))
                        End If
                    End If
                Next

                lista.Sort(Function(x, y) x.Nombre.CompareTo(y.Nombre))

                For Each categoria In lista
                    If categoria.Nombre.Length > 0 Then
                        Dim tb As New TextBlock With {
                            .Text = categoria.Nombre,
                            .TextWrapping = TextWrapping.Wrap,
                            .FontSize = 14,
                            .VerticalAlignment = VerticalAlignment.Center
                        }

                        Dim cb As New CheckBox With {
                            .IsChecked = categoria.Estado,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .HorizontalAlignment = HorizontalAlignment.Stretch,
                            .Content = tb,
                            .Tag = categoria,
                            .Width = 200,
                            .Padding = New Thickness(5, 5, 5, 5)
                        }

                        AddHandler cb.Checked, AddressOf UsuarioClickeaCaja
                        AddHandler cb.Unchecked, AddressOf UsuarioClickeaCaja
                        AddHandler cb.PointerEntered, AddressOf UsuarioEntraBoton
                        AddHandler cb.PointerExited, AddressOf UsuarioSaleBoton

                        gv.Items.Add(cb)
                    End If
                Next
            End If
        End If

    End Sub

    Public Async Sub UsuarioClickeaCaja(sender As Object, e As RoutedEventArgs)

        Dim cb As CheckBox = e.OriginalSource

        Dim listaCategorias As List(Of Categoria) = Nothing

        Dim helper As New LocalObjectStorageHelper
        If Await helper.FileExistsAsync("listaCategorias") = True Then
            listaCategorias = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")
        End If

        If listaCategorias Is Nothing Then
            listaCategorias = New List(Of Categoria)
        End If

        Dim categoria As Categoria = cb.Tag

        If cb.IsChecked = True Then
            If Not categoria Is Nothing Then
                categoria.Estado = True

                If listaCategorias.Count > 0 Then
                    Dim boolCategoria As Boolean = False

                    'Dim j As Integer = 0
                    'While j < listaCategorias.Count
                    '    If categoria.Maestro.ID = listaCategorias(j).Maestro.ID Then
                    '        If categoria.Nombre = listaCategorias(j).Nombre Then
                    '            listaCategorias(j).Estado = True
                    '            boolCategoria = True
                    '        End If
                    '    End If
                    '    j += 1
                    'End While

                    If boolCategoria = False Then
                        listaCategorias.Add(categoria)
                    End If
                Else
                    listaCategorias.Add(categoria)
                End If
            End If
        Else
            If Not categoria Is Nothing Then
                categoria.Estado = False

                'Dim j As Integer = 0
                'While j < listaCategorias.Count
                '    If categoria.Maestro.ID = listaCategorias(j).Maestro.ID Then
                '        If categoria.Nombre = listaCategorias(j).Nombre Then
                '            listaCategorias(j).Estado = False
                '        End If
                '    End If
                '    j += 1
                'End While
            End If
        End If

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim boolBoton As Boolean = False
        Dim contadorTrue As Integer = 0

        For Each item In listaCategorias
            If item.Estado = True Then
                boolBoton = True
                contadorTrue += 1
            End If
        Next

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        botonAñadir.IsEnabled = boolBoton

        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        botonLimpiar.IsEnabled = boolBoton

        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")
        botonBorrar.IsEnabled = boolBoton

        Dim tb As TextBlock = pagina.FindName("tbNumeroCategorias")

        If Not contadorTrue = 0 Then
            tb.Text = " (" + contadorTrue.ToString + ")"
        Else
            tb.Text = String.Empty
        End If

        Try
            Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", listaCategorias)
        Catch ex As Exception

        End Try

    End Sub




    Public Function AñadirJuegoLista(juego As Juego)

        Dim recursos As New Resources.ResourceLoader()

        Dim grid As New Grid With {
            .Tag = juego,
            .Padding = New Thickness(10, 3, 10, 3),
            .Name = "grid" + juego.ID.ToString
        }

        Dim color1 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#e0e0e0"),
            .Offset = 0.5
        }

        Dim color2 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#d6d6d6"),
            .Offset = 1.0
        }

        Dim coleccion As New GradientStopCollection From {
            color1,
            color2
        }

        Dim brush As New LinearGradientBrush With {
            .StartPoint = New Point(0.5, 0),
            .EndPoint = New Point(0.5, 1),
            .GradientStops = coleccion
        }

        grid.Background = brush

        Dim col1 As New ColumnDefinition
        Dim col2 As New ColumnDefinition
        Dim col3 As New ColumnDefinition
        Dim col4 As New ColumnDefinition

        col1.Width = New GridLength(1, GridUnitType.Auto)
        col2.Width = New GridLength(1, GridUnitType.Auto)
        col3.Width = New GridLength(1, GridUnitType.Star)
        col4.Width = New GridLength(1, GridUnitType.Auto)

        grid.ColumnDefinitions.Add(col1)
        grid.ColumnDefinitions.Add(col2)
        grid.ColumnDefinitions.Add(col3)
        grid.ColumnDefinitions.Add(col4)

        Dim tbNumCategorias As New TextBlock With {
            .Text = "(0)",
            .Margin = New Thickness(5, 0, 10, 0),
            .VerticalAlignment = VerticalAlignment.Center,
            .Foreground = New SolidColorBrush(Colors.Black),
            .Name = "tbNumCategorias" + juego.ID.ToString
        }

        tbNumCategorias.SetValue(Grid.ColumnProperty, 0)
        grid.Children.Add(tbNumCategorias)

        Dim borde As New Border With {
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .BorderThickness = New Thickness(1, 1, 1, 1),
            .Margin = New Thickness(4, 4, 10, 4)
        }

        Dim imagenJuego As New ImageEx With {
            .Stretch = Stretch.Uniform,
            .IsCacheEnabled = True
        }

        Try
            imagenJuego.Source = New BitmapImage(New Uri(juego.Imagen))
        Catch ex As Exception

        End Try

        borde.Child = imagenJuego
        borde.SetValue(Grid.ColumnProperty, 1)
        grid.Children.Add(borde)

        Dim tbTitulo As New TextBlock With {
            .Text = juego.Titulo,
            .VerticalAlignment = VerticalAlignment.Center,
            .Margin = New Thickness(0, 0, 10, 0),
            .Foreground = New SolidColorBrush(Colors.Black)
        }

        tbTitulo.SetValue(Grid.ColumnProperty, 2)
        grid.Children.Add(tbTitulo)

        '-------------------------------------------------------------

        Dim sp As New StackPanel With {
            .Orientation = Orientation.Horizontal
        }

        If Not juego.Userscore Is Nothing Then
            Dim spUserScore As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim iconoUserscore As New FontAwesome.UWP.FontAwesome With {
                .VerticalAlignment = VerticalAlignment.Center
            }

            Dim tbUserscore As New TextBlock With {
                .Text = juego.Userscore.Nombre + "%",
                .VerticalAlignment = VerticalAlignment.Center,
                .Margin = New Thickness(5, 0, 0, 0)
            }

            If juego.Userscore.Nombre > 74 Then
                iconoUserscore.Icon = FontAwesomeIcon.ThumbsOutlineUp
                iconoUserscore.Foreground = New SolidColorBrush(Colors.Green)
                tbUserscore.Foreground = New SolidColorBrush(Colors.Green)
            ElseIf juego.Userscore.Nombre > 49 And juego.Userscore.Nombre < 75 Then
                iconoUserscore.Icon = FontAwesomeIcon.HandRockOutline
                iconoUserscore.Foreground = New SolidColorBrush(Colors.Goldenrod)
                tbUserscore.Foreground = New SolidColorBrush(Colors.Goldenrod)
            ElseIf juego.Userscore.Nombre < 50 Then
                iconoUserscore.Icon = FontAwesomeIcon.ThumbsOutlineDown
                iconoUserscore.Foreground = New SolidColorBrush(Colors.Red)
                tbUserscore.Foreground = New SolidColorBrush(Colors.Red)
            End If

            spUserScore.Children.Add(iconoUserscore)
            spUserScore.Children.Add(tbUserscore)

            Dim cbUserscore As New CheckBox With {
                .MinWidth = 0,
                .Margin = New Thickness(5, 0, 5, 0),
                .Content = spUserScore,
                .VerticalAlignment = VerticalAlignment.Center,
                .Name = "cbUserscore" + juego.ID.ToString,
                .Tag = juego.Userscore
            }

            AddHandler cbUserscore.Checked, AddressOf UsuarioClickeaUserscore
            AddHandler cbUserscore.Unchecked, AddressOf UsuarioClickeaUserscore
            AddHandler cbUserscore.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler cbUserscore.PointerExited, AddressOf UsuarioSaleBoton

            sp.Children.Add(cbUserscore)
        End If

        If Not juego.Tags Is Nothing Then
            If juego.Tags.Count > 0 Then
                Dim spBoton As New StackPanel With {
                    .Orientation = Orientation.Horizontal
                }

                Dim tbBoton As New TextBlock With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .Text = recursos.GetString("Tags"),
                    .VerticalAlignment = VerticalAlignment.Center
                }

                spBoton.Children.Add(tbBoton)

                Dim iconoBoton As New FontAwesome.UWP.FontAwesome With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .Icon = FontAwesomeIcon.AngleDown,
                    .Margin = New Thickness(5, 0, 0, 0)
                }

                spBoton.Children.Add(iconoBoton)

                Dim menu As New MenuFlyout

                Dim i As Integer = 0
                For Each tag In juego.Tags
                    Dim item As New ToggleMenuFlyoutItem With {
                        .Text = tag.Nombre,
                        .Name = "etiqueta" + juego.ID.ToString + tag.Nombre,
                        .Tag = juego.Tags(i)
                    }

                    AddHandler item.Click, AddressOf UsuarioClickeaEtiqueta
                    AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
                    AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

                    menu.Items.Add(item)
                    i += 1
                Next

                Dim boton As New Button With {
                    .Margin = New Thickness(20, 0, 0, 0),
                    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
                    .Content = spBoton,
                    .Flyout = menu
                }

                AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

                sp.Children.Add(boton)
            End If
        End If

        sp.SetValue(Grid.ColumnProperty, 3)
        grid.Children.Add(sp)

        Return grid

    End Function

    Public Sub UsuarioClickeaUserscore(sender As Object, e As RoutedEventArgs)

        Dim cb As CheckBox = sender
        Dim categoria As Categoria = cb.Tag
        categoria.Estado = cb.IsChecked
        cb.Tag = categoria

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Userscore Is Nothing Then
                Dim cbUserscore As CheckBox = pagina.FindName("cbUserscore" + juego.ID.ToString)

                If Not cbUserscore Is Nothing Then
                    If Not cbUserscore.IsChecked = categoria.Estado Then
                        If categoria.Nombre > 89 Then
                            If juego.Userscore.Nombre > 89 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 79 And categoria.Nombre < 90 Then
                            If juego.Userscore.Nombre > 79 And juego.Userscore.Nombre < 90 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 69 And categoria.Nombre < 80 Then
                            If juego.Userscore.Nombre > 69 And juego.Userscore.Nombre < 80 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 59 And categoria.Nombre < 70 Then
                            If juego.Userscore.Nombre > 59 And juego.Userscore.Nombre < 70 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 49 And categoria.Nombre < 60 Then
                            If juego.Userscore.Nombre > 49 And juego.Userscore.Nombre < 60 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 39 And categoria.Nombre < 50 Then
                            If juego.Userscore.Nombre > 39 And juego.Userscore.Nombre < 50 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 29 And categoria.Nombre < 40 Then
                            If juego.Userscore.Nombre > 29 And juego.Userscore.Nombre < 40 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 19 And categoria.Nombre < 30 Then
                            If juego.Userscore.Nombre > 19 And juego.Userscore.Nombre < 30 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre > 9 And categoria.Nombre < 20 Then
                            If juego.Userscore.Nombre > 9 And juego.Userscore.Nombre < 20 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        ElseIf categoria.Nombre < 10 Then
                            If juego.Userscore.Nombre < 10 Then
                                cbUserscore.IsChecked = categoria.Estado
                            End If
                        End If
                    End If
                End If
            End If
        Next

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Userscore Is Nothing Then
                If categoria.IDJuego = juego.ID Then
                    Dim tbNumCategorias As TextBlock = pagina.FindName("tbNumCategorias" + juego.ID.ToString)
                    Dim tempNumCategorias As String = tbNumCategorias.Text
                    tempNumCategorias = tempNumCategorias.Replace("(", Nothing)
                    tempNumCategorias = tempNumCategorias.Replace(")", Nothing)
                    Dim numCategorias As Integer = Integer.Parse(tempNumCategorias)

                    Dim cbUserscore As CheckBox = pagina.FindName("cbUserscore" + juego.ID.ToString)

                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                    Else
                        If numCategorias > 0 Then
                            numCategorias -= 1
                        End If
                    End If

                    tbNumCategorias.Text = "(" + numCategorias.ToString + ")"
                End If
            End If
        Next

    End Sub

    Public Sub UsuarioClickeaEtiqueta(sender As Object, e As RoutedEventArgs)

        Dim toggle As ToggleMenuFlyoutItem = sender
        Dim categoria As Categoria = toggle.Tag
        categoria.Estado = toggle.IsChecked
        toggle.Tag = categoria

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Tags Is Nothing Then
                If juego.Tags.Count > 0 Then
                    For Each tag In juego.Tags
                        Dim toggleEtiqueta As ToggleMenuFlyoutItem = pagina.FindName("etiqueta" + juego.ID.ToString + tag.Nombre)

                        If Not toggleEtiqueta Is Nothing Then
                            If Not toggleEtiqueta.IsChecked = categoria.Estado Then
                                toggleEtiqueta.IsChecked = categoria.Estado
                            End If
                        End If
                    Next
                End If

            End If
        Next

    End Sub

End Module
