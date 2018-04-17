Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI
Imports Windows.UI.Core

Module Interfaz

    Public Function AñadirJuegoLista(juego As Juego)

        Dim recursos As New Resources.ResourceLoader()

        Dim grid As New Grid With {
            .Tag = juego,
            .Padding = New Thickness(10, 3, 30, 3),
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

            If juego.Userscore.Nombre > 69 Then
                iconoUserscore.Icon = FontAwesomeIcon.ThumbsOutlineUp
                iconoUserscore.Foreground = New SolidColorBrush(Colors.Green)
                tbUserscore.Foreground = New SolidColorBrush(Colors.Green)
            ElseIf juego.Userscore.Nombre > 49 And juego.Userscore.Nombre < 70 Then
                Dim componer As New CompositeTransform With {
                    .Rotation = 90
                }

                iconoUserscore.RenderTransform = componer
                iconoUserscore.RenderTransformOrigin = New Point(0.5, 0.5)

                iconoUserscore.Icon = FontAwesomeIcon.HandRockOutline
                iconoUserscore.Foreground = New SolidColorBrush(Colors.Chocolate)
                tbUserscore.Foreground = New SolidColorBrush(Colors.Chocolate)
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

            juego.CbUserscore = cbUserscore
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
                Dim listaItems As New List(Of ToggleMenuFlyoutItem)

                Dim i As Integer = 0
                For Each tag In juego.Tags
                    Dim item As New ToggleMenuFlyoutItem With {
                        .Text = tag.Nombre,
                        .Name = "etiqueta" + juego.ID.ToString + tag.Nombre.Replace(" ", Nothing),
                        .Tag = juego.Tags(i)
                    }

                    AddHandler item.Click, AddressOf UsuarioClickeaEtiqueta
                    AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
                    AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

                    listaItems.Add(item)
                    menu.Items.Add(item)
                    i += 1
                Next

                juego.CbTags = listaItems

                Dim boton As New Button With {
                    .Margin = New Thickness(20, 0, 0, 0),
                    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
                    .Content = spBoton,
                    .Flyout = menu,
                    .Name = "botonEtiquetas" + juego.ID.ToString
                }

                AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

                sp.Children.Add(boton)
            End If
        End If

        If Not juego.Categorias Is Nothing Then
            If juego.Categorias.Count > 0 Then
                Dim spBoton As New StackPanel With {
                    .Orientation = Orientation.Horizontal
                }

                Dim tbBoton As New TextBlock With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .Text = recursos.GetString("Categories"),
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
                Dim listaItems As New List(Of ToggleMenuFlyoutItem)

                Dim i As Integer = 0
                For Each categoria In juego.Categorias
                    Dim item As New ToggleMenuFlyoutItem With {
                        .Text = categoria.Nombre,
                        .Name = "categoria" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing),
                        .Tag = juego.Categorias(i)
                    }

                    AddHandler item.Click, AddressOf UsuarioClickeaCategoria
                    AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
                    AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

                    listaItems.Add(item)
                    menu.Items.Add(item)
                    i += 1
                Next

                juego.CbCategorias = listaItems

                Dim boton As New Button With {
                    .Margin = New Thickness(20, 0, 0, 0),
                    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
                    .Content = spBoton,
                    .Flyout = menu,
                    .Name = "botonCategorias" + juego.ID.ToString
                }

                AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

                sp.Children.Add(boton)
            End If
        End If

        If Not juego.Generos Is Nothing Then
            If juego.Generos.Count > 0 Then
                Dim spBoton As New StackPanel With {
                    .Orientation = Orientation.Horizontal
                }

                Dim tbBoton As New TextBlock With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .Text = recursos.GetString("Genres"),
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
                Dim listaItems As New List(Of ToggleMenuFlyoutItem)

                Dim i As Integer = 0
                For Each genero In juego.Generos
                    Dim item As New ToggleMenuFlyoutItem With {
                        .Text = genero.Nombre,
                        .Name = "genero" + juego.ID.ToString + genero.Nombre.Replace(" ", Nothing),
                        .Tag = juego.Generos(i)
                    }

                    AddHandler item.Click, AddressOf UsuarioClickeaGenero
                    AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
                    AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

                    listaItems.Add(item)
                    menu.Items.Add(item)
                    i += 1
                Next

                juego.CbGeneros = listaItems

                Dim boton As New Button With {
                    .Margin = New Thickness(20, 0, 0, 0),
                    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
                    .Content = spBoton,
                    .Flyout = menu,
                    .Name = "botonGeneros" + juego.ID.ToString
                }

                AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

                sp.Children.Add(boton)
            End If
        End If

        If Not juego.Custom Is Nothing Then
            If juego.Custom.Count > 0 Then
                Dim spBoton As New StackPanel With {
                    .Orientation = Orientation.Horizontal
                }

                Dim tbBoton As New TextBlock With {
                    .Foreground = New SolidColorBrush(Colors.White),
                    .Text = recursos.GetString("Customs"),
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
                Dim listaItems As New List(Of ToggleMenuFlyoutItem)

                Dim i As Integer = 0
                For Each custom In juego.Custom
                    Dim item As New ToggleMenuFlyoutItem With {
                        .Text = custom.Nombre,
                        .Name = "custom" + juego.ID.ToString + custom.Nombre.Replace(" ", Nothing),
                        .Tag = juego.Custom(i)
                    }

                    AddHandler item.Click, AddressOf UsuarioClickeaCustom
                    AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
                    AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

                    listaItems.Add(item)
                    menu.Items.Add(item)
                    i += 1
                Next

                juego.CbCustom = listaItems

                Dim boton As New Button With {
                    .Margin = New Thickness(20, 0, 0, 0),
                    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
                    .Content = spBoton,
                    .Flyout = menu,
                    .Name = "botonCustoms" + juego.ID.ToString
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
                Dim cbUserscore As CheckBox = juego.CbUserscore

                If Not cbUserscore Is Nothing Then
                    If Not cbUserscore.IsChecked = categoria.Estado Then
                        RemoveHandler cbUserscore.Checked, AddressOf UsuarioClickeaUserscore
                        RemoveHandler cbUserscore.Unchecked, AddressOf UsuarioClickeaUserscore

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

                        Dim categoriaItem As Categoria = cbUserscore.Tag
                        categoriaItem.Estado = cbUserscore.IsChecked
                        cbUserscore.Tag = categoriaItem

                        AddHandler cbUserscore.Checked, AddressOf UsuarioClickeaUserscore
                        AddHandler cbUserscore.Unchecked, AddressOf UsuarioClickeaUserscore
                    End If
                End If
            End If
        Next

        Dim tbNumeroCategorias As TextBlock = pagina.FindName("tbNumeroCategorias")
        Dim numCategoriasTotales As Integer = tbNumeroCategorias.Text

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Userscore Is Nothing Then
                Dim tbNumCategorias As TextBlock = juegoGrid.Children(0)
                Dim tempNumCategorias As String = tbNumCategorias.Text
                tempNumCategorias = tempNumCategorias.Replace("(", Nothing)
                tempNumCategorias = tempNumCategorias.Replace(")", Nothing)
                Dim numCategorias As Integer = Integer.Parse(tempNumCategorias)

                Dim cbUserscore As CheckBox = juego.CbUserscore

                If categoria.Nombre > 89 And juego.Userscore.Nombre > 89 Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 79 And categoria.Nombre < 90) And (juego.Userscore.Nombre > 79 And juego.Userscore.Nombre < 90) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 69 And categoria.Nombre < 80) And (juego.Userscore.Nombre > 69 And juego.Userscore.Nombre < 80) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 59 And categoria.Nombre < 70) And (juego.Userscore.Nombre > 59 And juego.Userscore.Nombre < 70) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 49 And categoria.Nombre < 60) And (juego.Userscore.Nombre > 49 And juego.Userscore.Nombre < 60) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 39 And categoria.Nombre < 50) And (juego.Userscore.Nombre > 39 And juego.Userscore.Nombre < 50) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 29 And categoria.Nombre < 40) And (juego.Userscore.Nombre > 29 And juego.Userscore.Nombre < 40) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 19 And categoria.Nombre < 30) And (juego.Userscore.Nombre > 19 And juego.Userscore.Nombre < 30) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf (categoria.Nombre > 9 And categoria.Nombre < 20) And (juego.Userscore.Nombre > 9 And juego.Userscore.Nombre < 20) Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                ElseIf categoria.Nombre < 10 And juego.Userscore.Nombre < 10 Then
                    If cbUserscore.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                End If

                tbNumCategorias.Text = "(" + numCategorias.ToString + ")"
            End If
        Next

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")

        If numCategoriasTotales > 0 Then
            If botonBorrar.IsEnabled = True Then
                botonAñadir.IsEnabled = True
            End If

            botonLimpiar.IsEnabled = True
        Else
            botonAñadir.IsEnabled = False
            botonLimpiar.IsEnabled = False
        End If

        tbNumeroCategorias.Text = numCategoriasTotales.ToString

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
                If Not juego.CbTags Is Nothing Then
                    For Each item As ToggleMenuFlyoutItem In juego.CbTags
                        If item.Name = "etiqueta" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                            item.IsChecked = categoria.Estado

                            Dim categoriaItem As Categoria = item.Tag
                            categoriaItem.Estado = item.IsChecked
                            item.Tag = categoriaItem
                        End If
                    Next
                End If
            End If
        Next

        Dim tbNumeroCategorias As TextBlock = pagina.FindName("tbNumeroCategorias")
        Dim numCategoriasTotales As Integer = tbNumeroCategorias.Text

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Tags Is Nothing Then
                If Not juego.CbTags Is Nothing Then
                    Dim tbNumCategorias As TextBlock = juegoGrid.Children(0)
                    Dim tempNumCategorias As String = tbNumCategorias.Text
                    tempNumCategorias = tempNumCategorias.Replace("(", Nothing)
                    tempNumCategorias = tempNumCategorias.Replace(")", Nothing)
                    Dim numCategorias As Integer = Integer.Parse(tempNumCategorias)

                    For Each item As ToggleMenuFlyoutItem In juego.CbTags
                        If item.Name = "etiqueta" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                            If item.IsChecked = True Then
                                numCategorias += 1
                                numCategoriasTotales += 1
                            Else
                                numCategorias -= 1
                                numCategoriasTotales -= 1
                            End If
                        End If
                    Next

                    If Not tbNumCategorias Is Nothing Then
                        tbNumCategorias.Text = "(" + numCategorias.ToString + ")"
                    End If
                End If
            End If
        Next

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")

        If numCategoriasTotales > 0 Then
            If botonBorrar.IsEnabled = True Then
                botonAñadir.IsEnabled = True
            End If

            botonLimpiar.IsEnabled = True
        Else
            botonAñadir.IsEnabled = False
            botonLimpiar.IsEnabled = False
        End If

        tbNumeroCategorias.Text = numCategoriasTotales.ToString

    End Sub

    Public Sub UsuarioClickeaCategoria(sender As Object, e As RoutedEventArgs)

        Dim toggle As ToggleMenuFlyoutItem = sender
        Dim categoria As Categoria = toggle.Tag
        categoria.Estado = toggle.IsChecked
        toggle.Tag = categoria

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Categorias Is Nothing Then
                If Not juego.CbCategorias Is Nothing Then
                    For Each item As ToggleMenuFlyoutItem In juego.CbCategorias
                        If item.Name = "categoria" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                            item.IsChecked = categoria.Estado

                            Dim categoriaItem As Categoria = item.Tag
                            categoriaItem.Estado = item.IsChecked
                            item.Tag = categoriaItem
                        End If
                    Next
                End If
            End If
        Next

        Dim tbNumeroCategorias As TextBlock = pagina.FindName("tbNumeroCategorias")
        Dim numCategoriasTotales As Integer = tbNumeroCategorias.Text

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Categorias Is Nothing Then
                If Not juego.CbCategorias Is Nothing Then
                    Dim tbNumCategorias As TextBlock = juegoGrid.Children(0)
                    Dim tempNumCategorias As String = tbNumCategorias.Text
                    tempNumCategorias = tempNumCategorias.Replace("(", Nothing)
                    tempNumCategorias = tempNumCategorias.Replace(")", Nothing)
                    Dim numCategorias As Integer = Integer.Parse(tempNumCategorias)

                    For Each item As ToggleMenuFlyoutItem In juego.CbCategorias
                        If item.Name = "categoria" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                            If item.IsChecked = True Then
                                numCategorias += 1
                                numCategoriasTotales += 1
                            Else
                                numCategorias -= 1
                                numCategoriasTotales -= 1
                            End If
                        End If
                    Next

                    If Not tbNumCategorias Is Nothing Then
                        tbNumCategorias.Text = "(" + numCategorias.ToString + ")"
                    End If
                End If
            End If
        Next

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")

        If numCategoriasTotales > 0 Then
            If botonBorrar.IsEnabled = True Then
                botonAñadir.IsEnabled = True
            End If

            botonLimpiar.IsEnabled = True
        Else
            botonAñadir.IsEnabled = False
            botonLimpiar.IsEnabled = False
        End If

        tbNumeroCategorias.Text = numCategoriasTotales.ToString

    End Sub

    Public Sub UsuarioClickeaGenero(sender As Object, e As RoutedEventArgs)

        Dim toggle As ToggleMenuFlyoutItem = sender
        Dim categoria As Categoria = toggle.Tag
        categoria.Estado = toggle.IsChecked
        toggle.Tag = categoria

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvJuegos As ListView = pagina.FindName("lvJuegos")

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Generos Is Nothing Then
                If Not juego.CbGeneros Is Nothing Then
                    For Each item As ToggleMenuFlyoutItem In juego.CbGeneros
                        If item.Name = "genero" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                            item.IsChecked = categoria.Estado

                            Dim categoriaItem As Categoria = item.Tag
                            categoriaItem.Estado = item.IsChecked
                            item.Tag = categoriaItem
                        End If
                    Next
                End If
            End If
        Next

        Dim tbNumeroCategorias As TextBlock = pagina.FindName("tbNumeroCategorias")
        Dim numCategoriasTotales As Integer = tbNumeroCategorias.Text

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Generos Is Nothing Then
                If Not juego.CbGeneros Is Nothing Then
                    Dim tbNumCategorias As TextBlock = juegoGrid.Children(0)
                    Dim tempNumCategorias As String = tbNumCategorias.Text
                    tempNumCategorias = tempNumCategorias.Replace("(", Nothing)
                    tempNumCategorias = tempNumCategorias.Replace(")", Nothing)
                    Dim numCategorias As Integer = Integer.Parse(tempNumCategorias)

                    For Each item As ToggleMenuFlyoutItem In juego.CbGeneros
                        If item.Name = "genero" + juego.ID.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                            If item.IsChecked = True Then
                                numCategorias += 1
                                numCategoriasTotales += 1
                            Else
                                numCategorias -= 1
                                numCategoriasTotales -= 1
                            End If
                        End If
                    Next

                    If Not tbNumCategorias Is Nothing Then
                        tbNumCategorias.Text = "(" + numCategorias.ToString + ")"
                    End If
                End If
            End If
        Next

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")

        If numCategoriasTotales > 0 Then
            If botonBorrar.IsEnabled = True Then
                botonAñadir.IsEnabled = True
            End If

            botonLimpiar.IsEnabled = True
        Else
            botonAñadir.IsEnabled = False
            botonLimpiar.IsEnabled = False
        End If

        tbNumeroCategorias.Text = numCategoriasTotales.ToString

    End Sub

    Public Sub UsuarioClickeaCustom(sender As Object, e As RoutedEventArgs)

        Dim toggle As ToggleMenuFlyoutItem = sender
        Dim categoria As Categoria = toggle.Tag
        categoria.Estado = toggle.IsChecked
        toggle.Tag = categoria

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbNumeroCategorias As TextBlock = pagina.FindName("tbNumeroCategorias")
        Dim numCategoriasTotales As Integer = tbNumeroCategorias.Text

        Dim tbNumCategorias As TextBlock = pagina.FindName("tbNumCategorias" + categoria.IDJuego.ToString)
        Dim tempNumCategorias As String = tbNumCategorias.Text
        tempNumCategorias = tempNumCategorias.Replace("(", Nothing)
        tempNumCategorias = tempNumCategorias.Replace(")", Nothing)
        Dim numCategorias As Integer = Integer.Parse(tempNumCategorias)

        Dim boton As Button = pagina.FindName("botonCustoms" + categoria.IDJuego.ToString)

        If Not boton Is Nothing Then
            Dim menu As MenuFlyout = boton.Flyout

            For Each item As ToggleMenuFlyoutItem In menu.Items
                If item.Name = "custom" + categoria.IDJuego.ToString + categoria.Nombre.Replace(" ", Nothing) Then
                    If item.IsChecked = True Then
                        numCategorias += 1
                        numCategoriasTotales += 1
                    Else
                        numCategorias -= 1
                        numCategoriasTotales -= 1
                    End If
                End If
            Next
        End If

        If Not tbNumCategorias Is Nothing Then
            tbNumCategorias.Text = "(" + numCategorias.ToString + ")"
        End If

        Dim botonAñadir As Button = pagina.FindName("botonAñadirCategorias")
        Dim botonLimpiar As Button = pagina.FindName("botonLimpiarSeleccion")
        Dim botonBorrar As Button = pagina.FindName("botonBorrarCategorias")

        If numCategoriasTotales > 0 Then
            If botonBorrar.IsEnabled = True Then
                botonAñadir.IsEnabled = True
            End If

            botonLimpiar.IsEnabled = True
        Else
            botonAñadir.IsEnabled = False
            botonLimpiar.IsEnabled = False
        End If

        tbNumeroCategorias.Text = numCategoriasTotales.ToString

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
