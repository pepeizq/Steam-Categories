Imports Microsoft.Services.Store.Engagement
Imports Microsoft.Toolkit.Uwp
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.System
Imports Windows.UI

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

        'botonEscribirCategoriasTexto.Text = recursos.GetString("Boton Escribir Categorias")
        'botonBorrarCategoriasTexto.Text = recursos.GetString("Boton Borrar Categorias")
        'tbAvisoSteamCerrado.Text = recursos.GetString("Aviso Steam")

        'expanderUserscore.Header = recursos.GetString("Como Funciona")
        'tbSeleccionUserscoreInfo.Text = recursos.GetString("Texto Seleccion Userscore")

        'expanderMetascore.Header = recursos.GetString("Como Funciona")
        'tbSeleccionMetascoreInfo.Text = recursos.GetString("Texto Seleccion Metascore")

        'expanderAños.Header = recursos.GetString("Como Funciona")
        'botonSeleccionAños.Content = recursos.GetString("Año Lanzamiento")
        'cbSeleccionAños.Content = recursos.GetString("Año Lanzamiento")
        'tbSeleccionAñosInfo.Text = recursos.GetString("Texto Seleccion Años")

        'botonSeleccionCategorias.Content = recursos.GetString("Categorias")
        'botonSeleccionGeneros.Content = recursos.GetString("Generos")
        'botonSeleccionTags.Content = recursos.GetString("Tags")
        'botonSeleccionIdiomas.Content = recursos.GetString("Idiomas")

        'tbNoJuegos.Text = recursos.GetString("No Juegos")

        'botonConfigCategoriasTexto.Text = recursos.GetString("App")
        'botonConfigCategoriasPersonalizarTexto.Text = recursos.GetString("Personalizar")
        'tbSteamConfigInstruccionesCliente.Text = recursos.GetString("Texto Steam Config Cliente")
        'botonSteamRutaTexto.Text = recursos.GetString("Boton Añadir")
        'tbSteamRuta.Text = recursos.GetString("Texto Carpeta")
        'botonSteamCuentaTexto.Text = recursos.GetString("Boton Añadir")
        'tbSteamConfigInstruccionesCuenta.Text = recursos.GetString("Texto Steam Config Cuenta")
        'botonCargaCategoriasTexto.Text = recursos.GetString("Boton Carga Categorias")
        'tbCargaCategoriasAviso.Text = recursos.GetString("Aviso Carga")
        'tbJuegosCuentaMensaje.Text = recursos.GetString("Texto Juegos Cuenta")
        'tbJuegosAppMensaje.Text = recursos.GetString("Texto Juegos App")
        'cbActualizarListaJuegos.Content = recursos.GetString("Modo Actualizar")
        'tbActualizarListaJuegos.Text = recursos.GetString("Modo Actualizar Tooltip")
        'tbLimpiarSeleccionCategoriasTexto.Text = recursos.GetString("Boton Limpiar Seleccion")
        'tbBorrarCategoriasAppTexto.Text = recursos.GetString("Boton Borrar Categorias App")

        '--------------------------------------------------------

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
                        tbMensajeCategorias.text = recursos.GetString("MessageCategories2")
                    End If

                    'GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)
                    tbJuegosApp.Text = listaJuegos.Count.ToString
                End If
            End If
        End If

        Dim actualizar As Boolean = False

        If Await helper.FileExistsAsync("actualizar") = True Then
            actualizar = Await helper.ReadFileAsync(Of Boolean)("actualizar")
            cbActualizarListaJuegos.IsChecked = actualizar
        End If

        If actualizar = False Then
            If Await helper.FileExistsAsync("listaCategorias") = True Then
                Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", New List(Of Categoria))
            End If

            Categorias.GenerarCategorias(listaJuegos)
            Categorias.GenerarGeneros(listaJuegos)
            Categorias.GenerarTags(listaJuegos)
            Categorias.GenerarIdiomas(listaJuegos)
        Else
            Categorias.Cargar()
        End If

        Cuentas.Detectar(actualizar)
        Cliente.Detectar(False)
        PersonalizarConfig()

    End Sub

    Private Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = "Steam Categories (" + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString + ") - " + tag

        gridCategorias.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    'CATEGORIAS--------------------------------------------------------------

    Private Sub LvCategoriasItemClick(sender As Object, args As ItemClickEventArgs)

        If panelMensajeCategorias.Visibility = Visibility.Visible Then
            panelMensajeCategorias.Visibility = Visibility.Collapsed
        End If

        botonCategoriasUserscore.Background = New SolidColorBrush(Colors.SteelBlue)
        botonCategoriasMetascore.Background = New SolidColorBrush(Colors.SteelBlue)
        botonCategoriasAños.Background = New SolidColorBrush(Colors.SteelBlue)
        botonCategoriasCategorias.Background = New SolidColorBrush(Colors.SteelBlue)
        botonCategoriasGeneros.Background = New SolidColorBrush(Colors.SteelBlue)
        botonCategoriasTags.Background = New SolidColorBrush(Colors.SteelBlue)
        botonCategoriasIdiomas.Background = New SolidColorBrush(Colors.SteelBlue)

        gridCategoriasUserscore.Visibility = Visibility.Collapsed
        gridCategoriasMetascore.Visibility = Visibility.Collapsed
        gridCategoriasAños.Visibility = Visibility.Collapsed
        gridCategoriasCategorias.Visibility = Visibility.Collapsed
        gridCategoriasGeneros.Visibility = Visibility.Collapsed
        gridCategoriasTags.Visibility = Visibility.Collapsed
        gridCategoriasIdiomas.Visibility = Visibility.Collapsed

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            botonCategoriasUserscore.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasUserscore.Visibility = Visibility.Visible

        ElseIf sp.Tag.ToString = 1 Then

            botonCategoriasMetascore.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasMetascore.Visibility = Visibility.Visible

        ElseIf sp.Tag.ToString = 2 Then

            botonCategoriasAños.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasAños.Visibility = Visibility.Visible

        ElseIf sp.Tag.ToString = 3 Then

            botonCategoriasCategorias.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasCategorias.Visibility = Visibility.Visible

        ElseIf sp.Tag.ToString = 4 Then

            botonCategoriasGeneros.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasGeneros.Visibility = Visibility.Visible

        ElseIf sp.Tag.ToString = 5 Then

            botonCategoriasTags.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasTags.Visibility = Visibility.Visible

        ElseIf sp.Tag.ToString = 6 Then

            botonCategoriasIdiomas.Background = New SolidColorBrush(Colors.DarkBlue)
            gridCategoriasIdiomas.Visibility = Visibility.Visible

        End If

    End Sub

    Private Sub BotonEscribirCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonEscribirCategorias.Click

        Cliente.EscribirCategorias()

    End Sub

    Private Sub BotonEscribirCategorias_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles botonEscribirCategorias.PointerEntered

        panelAviso.HorizontalAlignment = HorizontalAlignment.Left
        panelAviso.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonEscribirCategorias_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles botonEscribirCategorias.PointerExited

        panelAviso.Visibility = Visibility.Collapsed

    End Sub

    Private Sub BotonBorrarCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarCategorias.Click

        Cliente.BorrarCategorias(botonBorrarCategorias)

    End Sub

    Private Sub BotonBorrarCategorias_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles botonBorrarCategorias.PointerEntered

        panelAviso.HorizontalAlignment = HorizontalAlignment.Right
        panelAviso.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonBorrarCategorias_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles botonBorrarCategorias.PointerExited

        panelAviso.Visibility = Visibility.Collapsed

    End Sub

    '--------------------------------------------------------------

    Private Async Sub GridSeleccionVisibilidad(grid As Grid, boton As Button)


        gridNoJuegosCargados.Visibility = Visibility.Collapsed

        Dim noCategorias As Boolean = False
        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaJuegos") = True Then
            Dim listaJuegos As List(Of Juego) = Nothing

            Try
                listaJuegos = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")
            Catch ex As Exception

            End Try

            If Not listaJuegos Is Nothing Then
                If listaJuegos.Count = 0 Then
                    noCategorias = True
                End If
            Else
                noCategorias = True
            End If
        Else
            noCategorias = True
        End If

        If noCategorias = False Then
            grid.Visibility = Visibility.Visible
        Else
            gridNoJuegosCargados.Visibility = Visibility.Visible
        End If

    End Sub



    '--------------------------------------------------------------

    Private Sub CbSeleccionUserscore_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionUserscore.Checked

        CbSeleccionChecked("/*1/Userscore")

    End Sub

    Private Sub CbSeleccionUserscore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionUserscore.Unchecked

        CbSeleccionUnChecked("/*1/Userscore")

    End Sub

    Private Sub CbSeleccionMetascore_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Checked

        CbSeleccionChecked("/*2/Metascore")

    End Sub

    Private Sub CbSeleccionMetascore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Unchecked

        CbSeleccionUnChecked("/*2/Metascore")

    End Sub

    Private Sub CbSeleccionAños_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Checked

        CbSeleccionChecked("/*3/Years")

    End Sub

    Private Sub CbSeleccionAños_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Unchecked

        CbSeleccionUnChecked("/*3/Years")

    End Sub

    Private Async Sub CbSeleccionChecked(categoria As String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim listaCategorias As List(Of Categoria) = Nothing

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            listaCategorias = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")
        End If

        If listaCategorias Is Nothing Then
            listaCategorias = New List(Of Categoria)
        End If

        Dim boolCategoria As Boolean = False

        Dim j As Integer = 0
        While j < listaCategorias.Count
            If categoria = listaCategorias(j).Nombre Then
                listaCategorias(j).Estado = True
                boolCategoria = True
            End If
            j += 1
        End While

        If boolCategoria = False Then
            listaCategorias.Add(New Categoria(categoria, True, "principal"))
        End If

        Dim boolBoton As Boolean = False
        Dim contadorTrue As Integer = 0

        For Each item In listaCategorias
            If item.Estado = True Then
                boolBoton = True
                contadorTrue += 1
            End If
        Next

        If boolBoton = True Then
            botonEscribirCategorias.IsEnabled = True
        Else
            botonEscribirCategorias.IsEnabled = False
        End If

        If Not contadorTrue = 0 Then
            tbNumeroCategorias.Text = contadorTrue.ToString
        Else
            tbNumeroCategorias.Text = String.Empty
        End If

        Try
            Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", listaCategorias)
        Catch ex As Exception

        End Try

    End Sub

    Private Async Sub CbSeleccionUnChecked(categoria As String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of Categoria) = Await helper.ReadFileAsync(Of List(Of Categoria))("listaCategorias")

            If listaCategorias Is Nothing Then
                listaCategorias = New List(Of Categoria)
            End If

            For Each categoria_ In listaCategorias
                If categoria = categoria_.Nombre Then
                    categoria_.Estado = False
                End If
            Next

            Dim boolBoton As Boolean = False
            Dim contadorTrue As Integer = 0

            For Each item In listaCategorias
                If item.Estado = True Then
                    boolBoton = True
                    contadorTrue += 1
                End If
            Next

            If boolBoton = True Then
                botonEscribirCategorias.IsEnabled = True
            Else
                botonEscribirCategorias.IsEnabled = False
            End If

            If Not contadorTrue = 0 Then
                tbNumeroCategorias.Text = contadorTrue.ToString
            Else
                tbNumeroCategorias.Text = String.Empty
            End If

            Try
                Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", listaCategorias)
            Catch ex As Exception

            End Try
        End If

    End Sub

    Private Sub GvCategorias_ItemClick(sender As Object, e As ItemClickEventArgs) Handles gvCategorias.ItemClick

        Dim sp As StackPanel = e.ClickedItem
        Categorias.AñadirListaCategorias(sp)

    End Sub

    Private Sub GvGeneros_ItemClick(sender As Object, e As ItemClickEventArgs) Handles gvGeneros.ItemClick

        Dim sp As StackPanel = e.ClickedItem
        Categorias.AñadirListaCategorias(sp)

    End Sub

    Private Sub GvTags_ItemClick(sender As Object, e As ItemClickEventArgs) Handles gvTags.ItemClick

        Dim sp As StackPanel = e.ClickedItem
        Categorias.AñadirListaCategorias(sp)

    End Sub

    Private Sub GvIdiomas_ItemClick(sender As Object, e As ItemClickEventArgs) Handles gvIdiomas.ItemClick

        Dim sp As StackPanel = e.ClickedItem
        Categorias.AñadirListaCategorias(sp)

    End Sub

    'CONFIG--------------------------------------------------------------

    Private Sub BotonSteamRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamRuta.Click

        Cliente.Detectar(True)

    End Sub

    Private Sub BotonSteamCuenta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamCuenta.Click

        Cuentas.Detectar(False)

    End Sub

    Private Sub BotonCargaCategorias_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles botonCargaCategorias.PointerEntered

        panelAvisoCargaCategorias.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonCargaCategorias_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles botonCargaCategorias.PointerExited

        panelAvisoCargaCategorias.Visibility = Visibility.Collapsed

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

    Private Async Sub BotonLimpiarSeleccionCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonLimpiarSeleccionCategorias.Click

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of List(Of Categoria))("listaCategorias", New List(Of Categoria))

        cbSeleccionUserscore.IsChecked = False
        cbSeleccionMetascore.IsChecked = False
        cbSeleccionAños.IsChecked = False

        For Each sp In gvCategorias.Items
            Dim cb As CheckBox = sp.Children.Item(0)
            cb.IsChecked = False
        Next

        For Each sp In gvGeneros.Items
            Dim cb As CheckBox = sp.Children.Item(0)
            cb.IsChecked = False
        Next

        For Each sp In gvTags.Items
            Dim cb As CheckBox = sp.Children.Item(0)
            cb.IsChecked = False
        Next

        For Each sp In gvIdiomas.Items
            Dim cb As CheckBox = sp.Children.Item(0)
            cb.IsChecked = False
        Next

        tbNumeroCategorias.Text = String.Empty
        botonEscribirCategorias.IsEnabled = False

    End Sub

    Private Async Sub BotonBorrarCategoriasApp_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarCategoriasApp.Click

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of List(Of Juego))("listaJuegos", New List(Of Juego))

        gvCategorias.Items.Clear()
        gvGeneros.Items.Clear()
        gvTags.Items.Clear()
        gvIdiomas.Items.Clear()

        tbJuegosApp.Text = 0
        lvCategorias.IsEnabled = False

    End Sub

    Private Async Sub PersonalizarConfig()

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        Dim userscore As List(Of Categoria) = Nothing
        Dim metascore As List(Of Categoria) = Nothing

        If Await helper.FileExistsAsync("userscore") = True Then
            userscore = Await helper.ReadFileAsync(Of List(Of Categoria))("userscore")
        End If

        If Await helper.FileExistsAsync("metascore") = True Then
            metascore = Await helper.ReadFileAsync(Of List(Of Categoria))("metascore")
        End If

        If userscore Is Nothing Then
            userscore = New List(Of Categoria) From {
                New Categoria("9", True, "userscore"),
                New Categoria("8", True, "userscore"),
                New Categoria("7", True, "userscore"),
                New Categoria("6", True, "userscore"),
                New Categoria("5", False, "userscore"),
                New Categoria("4", False, "userscore"),
                New Categoria("3", False, "userscore"),
                New Categoria("2", False, "userscore"),
                New Categoria("1", False, "userscore")
            }
        End If

        If metascore Is Nothing Then
            metascore = New List(Of Categoria) From {
                New Categoria("9", True, "metascore"),
                New Categoria("8", True, "metascore"),
                New Categoria("7", True, "metascore"),
                New Categoria("6", True, "metascore"),
                New Categoria("5", True, "metascore"),
                New Categoria("4", True, "metascore"),
                New Categoria("3", True, "metascore"),
                New Categoria("2", True, "metascore"),
                New Categoria("1", True, "metascore")
            }
        End If

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

        PersonalizarPuntuaciones("9", cbPersonalizarUserscore9.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore9_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore9.Unchecked

        PersonalizarPuntuaciones("9", cbPersonalizarUserscore9.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore8_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore8.Checked

        PersonalizarPuntuaciones("8", cbPersonalizarUserscore8.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore8_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore8.Unchecked

        PersonalizarPuntuaciones("8", cbPersonalizarUserscore8.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore7_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore7.Checked

        PersonalizarPuntuaciones("7", cbPersonalizarUserscore7.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore7_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore7.Unchecked

        PersonalizarPuntuaciones("7", cbPersonalizarUserscore7.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore6_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore6.Checked

        PersonalizarPuntuaciones("6", cbPersonalizarUserscore6.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore6_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore6.Unchecked

        PersonalizarPuntuaciones("6", cbPersonalizarUserscore6.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore5_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore5.Checked

        PersonalizarPuntuaciones("5", cbPersonalizarUserscore5.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore5_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore5.Unchecked

        PersonalizarPuntuaciones("5", cbPersonalizarUserscore5.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore4_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore4.Checked

        PersonalizarPuntuaciones("4", cbPersonalizarUserscore4.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore4_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore4.Unchecked

        PersonalizarPuntuaciones("4", cbPersonalizarUserscore4.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore3_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore3.Checked

        PersonalizarPuntuaciones("3", cbPersonalizarUserscore3.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore3_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore3.Unchecked

        PersonalizarPuntuaciones("3", cbPersonalizarUserscore3.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore2_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore2.Checked

        PersonalizarPuntuaciones("2", cbPersonalizarUserscore2.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore2_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore2.Unchecked

        PersonalizarPuntuaciones("2", cbPersonalizarUserscore2.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore1_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore1.Checked

        PersonalizarPuntuaciones("1", cbPersonalizarUserscore1.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarUserscore1_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarUserscore1.Unchecked

        PersonalizarPuntuaciones("1", cbPersonalizarUserscore1.IsChecked, "userscore")

    End Sub

    Private Sub CbPersonalizarMetascore9_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore9.Checked

        PersonalizarPuntuaciones("9", cbPersonalizarMetascore9.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore9_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore9.Unchecked

        PersonalizarPuntuaciones("9", cbPersonalizarMetascore9.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore8_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore8.Checked

        PersonalizarPuntuaciones("8", cbPersonalizarMetascore8.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore8_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore8.Unchecked

        PersonalizarPuntuaciones("8", cbPersonalizarMetascore8.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore7_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore7.Checked

        PersonalizarPuntuaciones("7", cbPersonalizarMetascore7.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore7_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore7.Unchecked

        PersonalizarPuntuaciones("7", cbPersonalizarMetascore7.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore6_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore6.Checked

        PersonalizarPuntuaciones("6", cbPersonalizarMetascore6.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore6_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore6.Unchecked

        PersonalizarPuntuaciones("6", cbPersonalizarMetascore6.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore5_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore5.Checked

        PersonalizarPuntuaciones("5", cbPersonalizarMetascore5.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore5_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore5.Unchecked

        PersonalizarPuntuaciones("5", cbPersonalizarMetascore5.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore4_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore4.Checked

        PersonalizarPuntuaciones("4", cbPersonalizarMetascore4.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore4_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore4.Unchecked

        PersonalizarPuntuaciones("4", cbPersonalizarMetascore4.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore3_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore3.Checked

        PersonalizarPuntuaciones("3", cbPersonalizarMetascore3.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore3_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore3.Unchecked

        PersonalizarPuntuaciones("3", cbPersonalizarMetascore3.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore2_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore2.Checked

        PersonalizarPuntuaciones("2", cbPersonalizarMetascore2.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore2_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore2.Unchecked

        PersonalizarPuntuaciones("2", cbPersonalizarMetascore2.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore1_Checked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore1.Checked

        PersonalizarPuntuaciones("1", cbPersonalizarMetascore1.IsChecked, "metascore")

    End Sub

    Private Sub CbPersonalizarMetascore1_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbPersonalizarMetascore1.Unchecked

        PersonalizarPuntuaciones("1", cbPersonalizarMetascore1.IsChecked, "metascore")

    End Sub

    'MASCOSAS-----------------------------------------

    Private Async Sub LvMasCosasItemClick(sender As Object, args As ItemClickEventArgs)

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

        ElseIf sp.Tag.ToString = 1 Then

            wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/"))

        ElseIf sp.Tag.ToString = 2 Then

            wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/contact/"))

        ElseIf sp.Tag.ToString = 3 Then

            If StoreServicesFeedbackLauncher.IsSupported = True Then
                Dim ejecutador As StoreServicesFeedbackLauncher = StoreServicesFeedbackLauncher.GetDefault()
                Await ejecutador.LaunchAsync()
            Else
                wvMasCosas.Navigate(New Uri("https://pepeizqapps.com/contact/"))
            End If

        ElseIf sp.Tag.ToString = 4 Then

            wvMasCosas.Navigate(New Uri("https://poeditor.com/join/project/YaZAR0uIW4"))

        ElseIf sp.Tag.ToString = 5 Then

            wvMasCosas.Navigate(New Uri("https://github.com/pepeizq/Steam-Categories"))

        End If

    End Sub

End Class
