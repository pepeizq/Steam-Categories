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
            .MinWidth = 150
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

End Module
