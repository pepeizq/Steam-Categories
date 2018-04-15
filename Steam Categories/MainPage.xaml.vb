Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Categories"), FontAwesomeIcon.Home, 0))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Config"), FontAwesomeIcon.Cog, 1))

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        Dim item As TextBlock = args.InvokedItem

        If Not item Is Nothing Then
            If item.Text = recursos.GetString("Categories") Then
                GridVisibilidad(gridCategorias, item.Text)
            ElseIf item.Text = recursos.GetString("Config") Then
                GridVisibilidad(gridConfig, item.Text)
            End If
        End If

    End Sub

    Private Sub Nv_ItemFlyout(sender As NavigationViewItem, args As TappedRoutedEventArgs)

        FlyoutBase.ShowAttachedFlyout(sender)

    End Sub

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        MasCosas.Generar()

        Dim recursos As New Resources.ResourceLoader()

        GridVisibilidad(gridCategorias, recursos.GetString("Categories"))
        nvPrincipal.IsPaneOpen = False

        Dim helper As New LocalObjectStorageHelper
        Dim listaJuegos As List(Of Juego) = Nothing

        If Await helper.FileExistsAsync("listaJuegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")

            If Not listaJuegos Is Nothing Then
                If listaJuegos.Count > 0 Then
                    Dim carpeta As StorageFolder = Nothing

                    Try
                        carpeta = Await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("SteamPath")
                    Catch ex As Exception

                    End Try

                    If Not carpeta Is Nothing Then
                        'lvCategorias.IsEnabled = True
                        tbMensajeCategorias.Text = recursos.GetString("MessageCategories2")
                    End If

                    tbJuegosApp.Text = listaJuegos.Count.ToString
                End If
            End If
        End If

        Dim actualizar As Boolean = False

        If Await helper.FileExistsAsync("actualizar") = True Then
            Try
                actualizar = Await helper.ReadFileAsync(Of Boolean)("actualizar")
            Catch ex As Exception

            End Try

            cbActualizarListaJuegos.IsChecked = actualizar
        End If

        If actualizar = False Then
            If Await helper.FileExistsAsync("listaCategorias") = True Then
                Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", New List(Of Categoria))
            End If

            If Not listaJuegos Is Nothing Then
                Juegos.Cargar()

                Dim listaAños As New List(Of Categoria)
                Dim listaCategorias As New List(Of Categoria)
                Dim listaGeneros As New List(Of Categoria)
                Dim listaTags As New List(Of Categoria)
                Dim listaIdiomas As New List(Of Categoria)

                For Each juego In listaJuegos
                    'listaAños.AddRange(juego.Años)
                    'listaCategorias.AddRange(juego.Categorias)
                    'listaGeneros.AddRange(juego.Generos)
                    'listaTags.AddRange(juego.Tags)
                    'listaIdiomas.AddRange(juego.Idiomas)
                Next

                'Interfaz.RellenarGridsCheckboxes(listaAños, gvAños, lvCategorias.Items(2).Tag)
                'Interfaz.RellenarGridsCheckboxes(listaCategorias, gvCategorias, lvCategorias.Items(3).Tag)
                'Interfaz.RellenarGridsCheckboxes(listaGeneros, gvGeneros, lvCategorias.Items(4).Tag)
                'Interfaz.RellenarGridsCheckboxes(listaTags, gvTags, lvCategorias.Items(5).Tag)
                'Interfaz.RellenarGridsCheckboxes(listaIdiomas, gvIdiomas, lvCategorias.Items(6).Tag)
            End If
        Else
            Juegos.Cargar()
        End If

        Cuentas.Detectar(actualizar)
        Cliente.Detectar(False)
        PersonalizarConfigUserMetascore(actualizar)

        '--------------------------------------------------------

        'cbPersonalizarUserscore1.Tag = New Categoria(cbPersonalizarUserscore1.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore2.Tag = New Categoria(cbPersonalizarUserscore2.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore3.Tag = New Categoria(cbPersonalizarUserscore3.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore4.Tag = New Categoria(cbPersonalizarUserscore4.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore5.Tag = New Categoria(cbPersonalizarUserscore5.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore6.Tag = New Categoria(cbPersonalizarUserscore6.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore7.Tag = New Categoria(cbPersonalizarUserscore7.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore8.Tag = New Categoria(cbPersonalizarUserscore8.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        'cbPersonalizarUserscore9.Tag = New Categoria(cbPersonalizarUserscore9.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))

        'cbPersonalizarMetascore1.Tag = New Categoria(cbPersonalizarMetascore1.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore2.Tag = New Categoria(cbPersonalizarMetascore2.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore3.Tag = New Categoria(cbPersonalizarMetascore3.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore4.Tag = New Categoria(cbPersonalizarMetascore4.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore5.Tag = New Categoria(cbPersonalizarMetascore5.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore6.Tag = New Categoria(cbPersonalizarMetascore6.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore7.Tag = New Categoria(cbPersonalizarMetascore7.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore8.Tag = New Categoria(cbPersonalizarMetascore8.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        'cbPersonalizarMetascore9.Tag = New Categoria(cbPersonalizarMetascore9.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))

        '--------------------------------------------------------

        Dim transpariencia As New UISettings
        TransparienciaEfectosFinal(transpariencia.AdvancedEffectsEnabled)
        AddHandler transpariencia.AdvancedEffectsEnabledChanged, AddressOf TransparienciaEfectosCambia

    End Sub

    Private Sub TransparienciaEfectosCambia(sender As UISettings, e As Object)

        TransparienciaEfectosFinal(sender.AdvancedEffectsEnabled)

    End Sub

    Private Async Sub TransparienciaEfectosFinal(estado As Boolean)

        Await Dispatcher.RunAsync(CoreDispatcherPriority.High, Sub()
                                                                   If estado = True Then
                                                                       gridConfig.Background = App.Current.Resources("GridAcrilico")
                                                                       gridConfigCategories.Background = App.Current.Resources("GridTituloBackground")
                                                                   Else
                                                                       gridConfig.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfigCategories.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                   End If
                                                               End Sub)

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + tag

        gridConfig.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    'CATEGORIAS--------------------------------------------------------------

    Private Sub BotonAñadirCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirCategorias.Click

        Cliente.EscribirCategorias()

    End Sub

    Private Async Sub BotonLimpiarSeleccion_Click(sender As Object, e As RoutedEventArgs) Handles botonLimpiarSeleccion.Click

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim helper As New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", New List(Of Categoria))

        For Each juegoGrid As Grid In lvJuegos.Items
            Dim juego As Juego = juegoGrid.Tag

            If Not juego.Userscore Is Nothing Then
                Dim cbUserscore As CheckBox = pagina.FindName("cbUserscore" + juego.ID.ToString)

                If Not cbUserscore Is Nothing Then
                    cbUserscore.IsChecked = False

                    Dim categoria As Categoria = cbUserscore.Tag
                    categoria.Estado = cbUserscore.IsChecked
                    cbUserscore.Tag = categoria
                End If
            End If

            If Not juego.Tags Is Nothing Then
                Dim boton As Button = pagina.FindName("botonEtiquetas" + juego.ID.ToString)

                If Not boton Is Nothing Then
                    Dim menu As MenuFlyout = boton.Flyout

                    For Each item As ToggleMenuFlyoutItem In menu.Items
                        item.IsChecked = False

                        Dim categoria As Categoria = item.Tag
                        categoria.Estado = item.IsChecked
                        item.Tag = categoria
                    Next
                End If
            End If

            If Not juego.Categorias Is Nothing Then
                Dim boton As Button = pagina.FindName("botonCategorias" + juego.ID.ToString)

                If Not boton Is Nothing Then
                    Dim menu As MenuFlyout = boton.Flyout

                    For Each item As ToggleMenuFlyoutItem In menu.Items
                        item.IsChecked = False

                        Dim categoria As Categoria = item.Tag
                        categoria.Estado = item.IsChecked
                        item.Tag = categoria
                    Next
                End If
            End If

            If Not juego.Generos Is Nothing Then
                Dim boton As Button = pagina.FindName("botonGeneros" + juego.ID.ToString)

                If Not boton Is Nothing Then
                    Dim menu As MenuFlyout = boton.Flyout

                    For Each item As ToggleMenuFlyoutItem In menu.Items
                        item.IsChecked = False

                        Dim categoria As Categoria = item.Tag
                        categoria.Estado = item.IsChecked
                        item.Tag = categoria
                    Next
                End If
            End If

            Dim tb As TextBlock = juegoGrid.Children(0)
            tb.Text = "(0)"
        Next

        tbNumeroCategorias.Text = "0"
        botonAñadirCategorias.IsEnabled = False
        botonLimpiarSeleccion.IsEnabled = False

    End Sub

    Private Sub BotonBorrarCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarCategorias.Click

        Cliente.BorrarCategorias()

    End Sub

    'CONFIG--------------------------------------------------------------

    Private Sub BotonSteamRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamRuta.Click

        Cliente.Detectar(True)

    End Sub

    Private Sub BotonSteamCuenta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamCuenta.Click

        Cuentas.Detectar(False)

    End Sub

    Private Sub BotonCargaCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonCargaCategorias.Click

        Juegos.Cargar()

    End Sub

    Private Async Sub CbActualizarListaJuegos_Checked(sender As Object, e As RoutedEventArgs) Handles cbActualizarListaJuegos.Checked

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of Boolean)("actualizar", True)
        cbActualizarListaJuegos.IsChecked = True

    End Sub

    Private Async Sub CbActualizarListaJuegos_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbActualizarListaJuegos.Unchecked

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of Boolean)("actualizar", False)
        cbActualizarListaJuegos.IsChecked = False

    End Sub

    Private Async Sub BotonLimpiarTodo_Click(sender As Object, e As RoutedEventArgs) Handles botonLimpiarTodo.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        Await helper.SaveFileAsync(Of List(Of Juego))("listaJuegos", New List(Of Juego))

        PersonalizarConfigUserMetascore(False)

        gvAños.Items.Clear()
        gvCategorias.Items.Clear()
        gvGeneros.Items.Clear()
        gvTags.Items.Clear()
        gvIdiomas.Items.Clear()

        botonAñadirCategorias.IsEnabled = False
        botonLimpiarSeleccion.IsEnabled = False
        botonBorrarCategorias.IsEnabled = False

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("SteamPath")
        Catch ex As Exception

        End Try

        botonSteamRutaTexto.Text = recursos.GetString("Add2")
        tbSteamRuta.Text = String.Empty

        Await helper.SaveFileAsync(Of Cuenta)("cuenta", Nothing)
        Await helper.SaveFileAsync(Of List(Of String))("listaJuegosID", Nothing)
        botonSteamCuentaTexto.Text = recursos.GetString("Add2")
        tbSteamCuenta.Text = String.Empty

        botonCargaCategorias.IsEnabled = False
        tbJuegosCuenta.Text = 0
        tbJuegosApp.Text = 0
        cbActualizarListaJuegos.IsChecked = False

    End Sub

    Private Async Sub PersonalizarConfigUserMetascore(actualizacion As Boolean)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        Dim userscore As List(Of Categoria) = Nothing
        Dim metascore As List(Of Categoria) = Nothing

        If Await helper.FileExistsAsync("userscore") = True Then
            userscore = Await helper.ReadFileAsync(Of List(Of Categoria))("userscore")
        End If

        If Await helper.FileExistsAsync("metascore") = True Then
            metascore = Await helper.ReadFileAsync(Of List(Of Categoria))("metascore")
        End If

        Dim generarUserscore As Boolean = False

        If userscore Is Nothing Then
            generarUserscore = True
        End If

        If actualizacion = False Then
            generarUserscore = True
        Else
            For Each score In userscore
                If score.Nombre = "9" Then
                    cbPersonalizarUserscore9.IsChecked = score.Estado
                End If

                If score.Nombre = "8" Then
                    cbPersonalizarUserscore8.IsChecked = score.Estado
                End If

                If score.Nombre = "7" Then
                    cbPersonalizarUserscore7.IsChecked = score.Estado
                End If

                If score.Nombre = "6" Then
                    cbPersonalizarUserscore6.IsChecked = score.Estado
                End If

                If score.Nombre = "5" Then
                    cbPersonalizarUserscore5.IsChecked = score.Estado
                End If

                If score.Nombre = "4" Then
                    cbPersonalizarUserscore4.IsChecked = score.Estado
                End If

                If score.Nombre = "3" Then
                    cbPersonalizarUserscore3.IsChecked = score.Estado
                End If

                If score.Nombre = "2" Then
                    cbPersonalizarUserscore2.IsChecked = score.Estado
                End If

                If score.Nombre = "1" Then
                    cbPersonalizarUserscore1.IsChecked = score.Estado
                End If
            Next
        End If

        If generarUserscore = True Then
            'userscore = New List(Of Categoria) From {
            '    New Categoria("9", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("8", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("7", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("6", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("5", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("4", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("3", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("2", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
            '    New Categoria("1", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
            '}
        End If

        Dim generarMetascore As Boolean = False

        If metascore Is Nothing Then
            generarMetascore = True
        End If

        If actualizacion = False Then
            generarMetascore = True
        Else
            For Each score In metascore
                If score.Nombre = "9" Then
                    cbPersonalizarMetascore9.IsChecked = score.Estado
                End If

                If score.Nombre = "8" Then
                    cbPersonalizarMetascore8.IsChecked = score.Estado
                End If

                If score.Nombre = "7" Then
                    cbPersonalizarMetascore7.IsChecked = score.Estado
                End If

                If score.Nombre = "6" Then
                    cbPersonalizarMetascore6.IsChecked = score.Estado
                End If

                If score.Nombre = "5" Then
                    cbPersonalizarMetascore5.IsChecked = score.Estado
                End If

                If score.Nombre = "4" Then
                    cbPersonalizarMetascore4.IsChecked = score.Estado
                End If

                If score.Nombre = "3" Then
                    cbPersonalizarMetascore3.IsChecked = score.Estado
                End If

                If score.Nombre = "2" Then
                    cbPersonalizarMetascore2.IsChecked = score.Estado
                End If

                If score.Nombre = "1" Then
                    cbPersonalizarMetascore1.IsChecked = score.Estado
                End If
            Next
        End If

        If generarMetascore = True Then
            'metascore = New List(Of Categoria) From {
            '    New Categoria("9", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("8", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("7", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("6", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("5", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("4", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("3", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("2", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
            '    New Categoria("1", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
            '}
        End If

        Try
            Await helper.SaveFileAsync(Of List(Of Categoria))("userscore", userscore)
        Catch ex As Exception

        End Try

        Try
            Await helper.SaveFileAsync(Of List(Of Categoria))("metascore", metascore)
        Catch ex As Exception

        End Try

    End Sub

    Private Async Sub PersonalizarPuntuaciones(numero As String, estado As Boolean, opcion As String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim lista As List(Of Categoria) = Nothing

        If Await helper.FileExistsAsync(opcion) = True Then
            Try
                lista = Await helper.ReadFileAsync(Of List(Of Categoria))(opcion)
            Catch ex As Exception

            End Try
        End If

        If Not lista Is Nothing Then
            For Each item In lista
                If item.Nombre = numero Then
                    item.Estado = estado
                End If
            Next

            Try
                Await helper.SaveFileAsync(Of List(Of Categoria))(opcion, lista)
            Catch ex As Exception

            End Try
        End If

    End Sub

End Class
