Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.UI
Imports Windows.UI.Core

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Categories"), New SymbolIcon(Symbol.Home), 0))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Config"), New SymbolIcon(Symbol.Setting), 1))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("MoreThings"), New SymbolIcon(Symbol.More), 2))

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        Dim item As TextBlock = args.InvokedItem

        If item.Text = recursos.GetString("Categories") Then
            GridVisibilidad(gridCategorias, item.Text)
        ElseIf item.Text = recursos.GetString("Config") Then
            GridVisibilidad(gridConfig, item.Text)
        ElseIf item.Text = recursos.GetString("MoreThings") Then
            GridVisibilidad(gridMasCosas, item.Text)

            Dim sv As ScrollViewer = gridMasCosas.Children(0)
            Dim gridRelleno As Grid = sv.Content
            Dim sp As StackPanel = gridRelleno.Children(0)
            Dim lv As ListView = sp.Children(0)

            MasCosas.Navegar(lv, "2", "https://pepeizqapps.com/")
        End If

    End Sub

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveBackgroundColor = Colors.Transparent

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        GridVisibilidad(gridCategorias, recursos.GetString("Categories"))
        nvPrincipal.IsPaneOpen = False

        Interfaz.GenerarMenu()
        MasCosas.Generar()

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
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
                        lvCategorias.IsEnabled = True
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
            End If
        Else
            Categorias.Cargar()
        End If

        Cuentas.Detectar(actualizar)
        Cliente.Detectar(False)
        PersonalizarConfigUserMetascore(actualizar)

        '--------------------------------------------------------

        AddHandler botonPopupUserscore.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupUserscore.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonPopupMetascore.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupMetascore.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonPopupAños.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupAños.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonPopupCategorias.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupCategorias.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonPopupGeneros.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupGeneros.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonPopupTags.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupTags.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonPopupIdiomas.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonPopupIdiomas.PointerExited, AddressOf UsuarioSaleBoton

        AddHandler botonAñadirCategorias.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonAñadirCategorias.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonLimpiarSeleccion.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonLimpiarSeleccion.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonBorrarCategorias.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonBorrarCategorias.PointerExited, AddressOf UsuarioSaleBoton

        AddHandler botonSteamRuta.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonSteamRuta.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonSteamCuenta.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonSteamCuenta.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonCargaCategorias.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonCargaCategorias.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler cbActualizarListaJuegos.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbActualizarListaJuegos.PointerExited, AddressOf UsuarioSaleBoton
        AddHandler botonLimpiarTodo.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonLimpiarTodo.PointerExited, AddressOf UsuarioSaleBoton

        '--------------------------------------------------------

        cbPersonalizarUserscore1.Tag = New Categoria(cbPersonalizarUserscore1.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore1.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore1.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore2.Tag = New Categoria(cbPersonalizarUserscore2.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore2.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore2.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore3.Tag = New Categoria(cbPersonalizarUserscore3.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore3.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore3.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore4.Tag = New Categoria(cbPersonalizarUserscore4.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore4.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore4.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore5.Tag = New Categoria(cbPersonalizarUserscore5.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore5.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore5.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore6.Tag = New Categoria(cbPersonalizarUserscore6.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore6.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore6.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore7.Tag = New Categoria(cbPersonalizarUserscore7.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore7.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore7.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore8.Tag = New Categoria(cbPersonalizarUserscore8.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore8.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore8.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarUserscore9.Tag = New Categoria(cbPersonalizarUserscore9.Content, False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
        AddHandler cbPersonalizarUserscore9.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarUserscore9.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore1.Tag = New Categoria(cbPersonalizarMetascore1.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore1.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore1.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore2.Tag = New Categoria(cbPersonalizarMetascore2.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore2.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore2.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore3.Tag = New Categoria(cbPersonalizarMetascore3.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore3.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore3.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore4.Tag = New Categoria(cbPersonalizarMetascore4.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore4.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore4.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore5.Tag = New Categoria(cbPersonalizarMetascore5.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore5.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore5.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore6.Tag = New Categoria(cbPersonalizarMetascore6.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore6.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore6.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore7.Tag = New Categoria(cbPersonalizarMetascore7.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore7.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore7.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore8.Tag = New Categoria(cbPersonalizarMetascore8.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore8.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore8.PointerExited, AddressOf UsuarioSaleBoton

        cbPersonalizarMetascore9.Tag = New Categoria(cbPersonalizarMetascore9.Content, False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
        AddHandler cbPersonalizarMetascore9.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler cbPersonalizarMetascore9.PointerExited, AddressOf UsuarioSaleBoton

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
                                                                       gridMasCosas.Background = App.Current.Resources("GridAcrilico")
                                                                   Else
                                                                       gridConfig.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfigCategories.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                       gridMasCosas.Background = New SolidColorBrush(Colors.LightGray)
                                                                   End If
                                                               End Sub)

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + tag

        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    'CATEGORIAS--------------------------------------------------------------

    Private Sub LvCategoriasItemClick(sender As Object, e As ItemClickEventArgs)

        If panelMensajeCategorias.Visibility = Visibility.Visible Then
            panelMensajeCategorias.Visibility = Visibility.Collapsed
        End If

        Interfaz.ClickeoBarraIzquierda(e.ClickedItem)

    End Sub

    Private Sub BotonAñadirCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirCategorias.Click

        Cliente.EscribirCategorias()

    End Sub

    Private Async Sub BotonLimpiarSeleccion_Click(sender As Object, e As RoutedEventArgs) Handles botonLimpiarSeleccion.Click

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", New List(Of Categoria))

        For Each cb As CheckBox In gvUserscore.Items
            cb.IsChecked = False
        Next

        For Each cb As CheckBox In gvMetascore.Items
            cb.IsChecked = False
        Next

        For Each cb As CheckBox In gvAños.Items
            cb.IsChecked = False
        Next

        For Each cb As CheckBox In gvCategorias.Items
            cb.IsChecked = False
        Next

        For Each cb As CheckBox In gvGeneros.Items
            cb.IsChecked = False
        Next

        For Each cb As CheckBox In gvTags.Items
            cb.IsChecked = False
        Next

        For Each cb As CheckBox In gvIdiomas.Items
            cb.IsChecked = False
        Next

        tbNumeroCategorias.Text = String.Empty
        botonAñadirCategorias.IsEnabled = False
        botonLimpiarSeleccion.IsEnabled = False
        botonBorrarCategorias.IsEnabled = False

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

        Categorias.Cargar()

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

        lvCategorias.IsEnabled = False
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
            userscore = New List(Of Categoria) From {
                New Categoria("9", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("8", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("7", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("6", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("5", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("4", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("3", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("2", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore")),
                New Categoria("1", False, New CategoriaMaestro("Userscore", "Assets\Menu\categorias_userscore.PNG", 0, "gridCategoriasUserscore"))
            }
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
            metascore = New List(Of Categoria) From {
                New Categoria("9", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("8", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("7", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("6", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("5", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("4", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("3", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("2", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore")),
                New Categoria("1", False, New CategoriaMaestro("Metascore", "Assets\Menu\categorias_metascore.PNG", 1, "gridCategoriasMetascore"))
            }
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

    Private Sub CbPersonalizarUserscore9_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore9.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("9", cbPersonalizarUserscore9.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore9_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore9.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("9", cbPersonalizarUserscore9.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore8_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore8.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("8", cbPersonalizarUserscore8.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore8_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore8.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("8", cbPersonalizarUserscore8.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore7_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore7.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("7", cbPersonalizarUserscore7.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore7_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore7.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("7", cbPersonalizarUserscore7.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore6_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore6.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("6", cbPersonalizarUserscore6.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore6_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore6.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("6", cbPersonalizarUserscore6.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore5_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore5.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("5", cbPersonalizarUserscore5.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore5_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore5.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("5", cbPersonalizarUserscore5.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore4_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore4.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("4", cbPersonalizarUserscore4.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore4_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore4.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("4", cbPersonalizarUserscore4.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore3_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore3.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("3", cbPersonalizarUserscore3.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore3_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore3.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("3", cbPersonalizarUserscore3.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore2_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore2.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("2", cbPersonalizarUserscore2.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore2_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore2.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("2", cbPersonalizarUserscore2.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore1_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore1.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("1", cbPersonalizarUserscore1.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore1_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore1.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("1", cbPersonalizarUserscore1.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarMetascore9_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore9.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("9", cbPersonalizarMetascore9.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore9_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore9.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("9", cbPersonalizarMetascore9.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore8_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore8.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("8", cbPersonalizarMetascore8.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore8_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore8.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("8", cbPersonalizarMetascore8.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore7_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore7.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("7", cbPersonalizarMetascore7.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore7_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore7.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("7", cbPersonalizarMetascore7.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore6_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore6.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("6", cbPersonalizarMetascore6.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore6_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore6.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("6", cbPersonalizarMetascore6.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore5_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore5.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("5", cbPersonalizarMetascore5.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore5_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore5.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("5", cbPersonalizarMetascore5.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore4_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore4.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("4", cbPersonalizarMetascore4.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore4_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore4.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("4", cbPersonalizarMetascore4.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore3_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore3.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("3", cbPersonalizarMetascore3.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore3_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore3.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("3", cbPersonalizarMetascore3.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore2_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore2.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("2", cbPersonalizarMetascore2.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore2_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore2.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("2", cbPersonalizarMetascore2.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore1_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore1.Checked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("1", cbPersonalizarMetascore1.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore1_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore1.Unchecked

        Interfaz.UsuarioClickeaCaja(sender, e)
        PersonalizarPuntuaciones("1", cbPersonalizarMetascore1.IsChecked, "metascore")

    End Sub

End Class
