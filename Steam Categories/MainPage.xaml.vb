Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
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

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        MasCosas.Generar()

        Dim recursos As New Resources.ResourceLoader()

        GridVisibilidad(gridCategorias, recursos.GetString("Categories"))
        nvPrincipal.IsPaneOpen = False

        Cuentas.Detectar()
        Juegos.Cargar()
        Cliente.Detectar(False)

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

    Private Sub TbBusquedaJuego_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbBusquedaJuego.TextChanged

        If tbBusquedaJuego.Text.Trim.Length > 0 Then
            For Each juegoGrid As Grid In lvJuegos.Items
                Dim item As ListViewItem = lvJuegos.ContainerFromItem(juegoGrid)

                If Not item Is Nothing Then
                    Dim juego As Juego = juegoGrid.Tag

                    If juego.Titulo.ToLower.Contains(tbBusquedaJuego.Text.ToLower.Trim) Then
                        item.Visibility = Visibility.Visible
                    Else
                        item.Visibility = Visibility.Collapsed
                    End If
                End If
            Next
        Else
            For Each juegoGrid As Grid In lvJuegos.Items
                Dim item As ListViewItem = lvJuegos.ContainerFromItem(juegoGrid)

                If Not item Is Nothing Then
                    item.Visibility = Visibility.Visible
                End If
            Next
        End If

    End Sub

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

        Cuentas.Detectar()

    End Sub

    Private Sub BotonCargaCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonCargaCategorias.Click

        Juegos.Cargar()

    End Sub

    Private Async Sub BotonLimpiarTodo_Click(sender As Object, e As RoutedEventArgs) Handles botonLimpiarTodo.Click

        Dim recursos As New Resources.ResourceLoader()
        Dim helper As New LocalObjectStorageHelper

        Await helper.SaveFileAsync(Of List(Of Juego))("listaJuegos", New List(Of Juego))

        tbBusquedaJuego.Text = String.Empty
        botonAñadirCategorias.IsEnabled = False
        botonLimpiarSeleccion.IsEnabled = False
        botonBorrarCategorias.IsEnabled = False

        Try
            StorageApplicationPermissions.FutureAccessList.Remove("SteamPath")
        Catch ex As Exception

        End Try

        botonSteamRutaTexto.Text = recursos.GetString("Add2")
        tbSteamRuta.Text = String.Empty

        Await helper.SaveFileAsync(Of Cuenta)("cuenta2", Nothing)
        Await helper.SaveFileAsync(Of List(Of String))("listaJuegosID", Nothing)
        botonSteamCuentaTexto.Text = recursos.GetString("Add2")
        tbSteamCuenta.Text = String.Empty

        botonCargaCategorias.IsEnabled = False
        tbJuegosCuenta.Text = 0
        tbJuegosApp.Text = 0

    End Sub

End Class
