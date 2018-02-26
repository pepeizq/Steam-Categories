Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI
Imports Windows.UI.Core

Module Interfaz

    Public Sub GenerarMenu()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lv As ListView = pagina.FindName("lvCategorias")

        Dim recursos As New Resources.ResourceLoader()

        Dim userscore As New Categoria2("Userscore", "Assets\Menu\categorias_userscore.PNG", 0)
        lv.Items.Add(GenerarMenuItem(userscore))

        Dim metascore As New Categoria2("Metascore", "Assets\Menu\categorias_metascore.PNG", 1)
        lv.Items.Add(GenerarMenuItem(metascore))

    End Sub

    Private Function GenerarMenuItem(categoria As Categoria2)

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
            .HorizontalContentAlignment = HorizontalAlignment.Stretch
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

    Public Sub Clickeo(spClickeado As StackPanel)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lv As ListView = pagina.FindName("lvCategorias")

        Dim categoriaClickeada As Categoria2 = spClickeado.Tag

        For Each lvitem As ListViewItem In lv.Items
            Dim sp As StackPanel = lvitem.Content

            Dim categoria As Categoria2 = sp.Tag

            If categoria.ID = categoriaClickeada.ID Then
                sp.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))

                Dim spImagen As StackPanel = pagina.FindName("spImagenCategoria")
                spImagen.Visibility = Visibility.Visible

                Dim imagen As ImageEx = pagina.FindName("imagenCategoriaEjemplo")
                imagen.Source = categoria.Imagen
            Else
                sp.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
            End If
        Next

    End Sub

End Module
