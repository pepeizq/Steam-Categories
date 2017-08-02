﻿Imports Microsoft.Services.Store.Engagement
Imports Microsoft.Toolkit.Uwp
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Acrilico.Generar(gridTopAcrilico)
        Acrilico.Generar(gridMenuAcrilico)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonPressedBackgroundColor = Colors.DarkBlue
        barra.ButtonInactiveBackgroundColor = Colors.Transparent
        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        botonCategoriasTexto.Text = recursos.GetString("Categorias")
        botonConfigTexto.Text = recursos.GetString("Boton Config")
        botonVotarTexto.Text = recursos.GetString("Boton Votar")
        botonMasCosasTexto.Text = recursos.GetString("Boton Cosas")

        botonMasAppsTexto.Text = recursos.GetString("Boton Web")
        botonContactoTexto.Text = recursos.GetString("Boton Contacto")
        botonReportarTexto.Text = recursos.GetString("Boton Reportar")
        botonCodigoFuenteTexto.Text = recursos.GetString("Boton Codigo Fuente")

        botonEscribirCategoriasTexto.Text = recursos.GetString("Boton Escribir Categorias")
        botonBorrarCategoriasTexto.Text = recursos.GetString("Boton Borrar Categorias")
        tbAvisoSteamCerrado.Text = recursos.GetString("Aviso Steam")

        expanderUserscore.Header = recursos.GetString("Como Funciona")
        tbSeleccionUserscoreInfo.Text = recursos.GetString("Texto Seleccion Userscore")

        expanderMetascore.Header = recursos.GetString("Como Funciona")
        tbSeleccionMetascoreInfo.Text = recursos.GetString("Texto Seleccion Metascore")

        expanderAños.Header = recursos.GetString("Como Funciona")
        botonSeleccionAños.Content = recursos.GetString("Año Lanzamiento")
        cbSeleccionAños.Content = recursos.GetString("Año Lanzamiento")
        tbSeleccionAñosInfo.Text = recursos.GetString("Texto Seleccion Años")

        botonSeleccionCategorias.Content = recursos.GetString("Categorias")
        botonSeleccionGeneros.Content = recursos.GetString("Generos")
        botonSeleccionTags.Content = recursos.GetString("Tags")
        botonSeleccionIdiomas.Content = recursos.GetString("Idiomas")

        tbNoJuegos.Text = recursos.GetString("No Juegos")

        botonConfigCategoriasTexto.Text = recursos.GetString("App")
        botonConfigCategoriasPersonalizarTexto.Text = recursos.GetString("Personalizar")
        tbSteamConfigInstruccionesCliente.Text = recursos.GetString("Texto Steam Config Cliente")
        botonSteamRutaTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamRuta.Text = recursos.GetString("Texto Carpeta")
        botonSteamCuentaTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigInstruccionesCuenta.Text = recursos.GetString("Texto Steam Config Cuenta")
        botonCargaCategoriasTexto.Text = recursos.GetString("Boton Carga Categorias")
        tbCargaCategoriasAviso.Text = recursos.GetString("Aviso Carga")
        tbJuegosCuentaMensaje.Text = recursos.GetString("Texto Juegos Cuenta")
        tbJuegosAppMensaje.Text = recursos.GetString("Texto Juegos App")
        cbActualizarListaJuegos.Content = recursos.GetString("Modo Actualizar")
        tbActualizarListaJuegos.Text = recursos.GetString("Modo Actualizar Tooltip")
        tbLimpiarSeleccionCategoriasTexto.Text = recursos.GetString("Boton Limpiar Seleccion")
        tbBorrarCategoriasAppTexto.Text = recursos.GetString("Boton Borrar Categorias App")

        '--------------------------------------------------------

        If ApplicationData.Current.LocalSettings.Values("expanderUserscore") = Nothing Then
            ApplicationData.Current.LocalSettings.Values("expanderUserscore") = "on"
            expanderUserscore.IsExpanded = True
        Else
            If ApplicationData.Current.LocalSettings.Values("expanderUserscore") = "on" Then
                expanderUserscore.IsExpanded = True
            Else
                expanderUserscore.IsExpanded = False
            End If
        End If

        If ApplicationData.Current.LocalSettings.Values("expanderMetascore") = Nothing Then
            ApplicationData.Current.LocalSettings.Values("expanderMetascore") = "on"
            expanderMetascore.IsExpanded = True
        Else
            If ApplicationData.Current.LocalSettings.Values("expanderMetascore") = "on" Then
                expanderMetascore.IsExpanded = True
            Else
                expanderMetascore.IsExpanded = False
            End If
        End If

        If ApplicationData.Current.LocalSettings.Values("expanderAños") = Nothing Then
            ApplicationData.Current.LocalSettings.Values("expanderAños") = "on"
            expanderAños.IsExpanded = True
        Else
            If ApplicationData.Current.LocalSettings.Values("expanderAños") = "on" Then
                expanderAños.IsExpanded = True
            Else
                expanderAños.IsExpanded = False
            End If
        End If

        '--------------------------------------------------------

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim listaJuegos As List(Of Juego) = Nothing

        If Await helper.FileExistsAsync("listaJuegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")

            If Not listaJuegos Is Nothing Then
                If listaJuegos.Count > 0 Then
                    GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)
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

        GridVisibilidad(gridCategorias, botonCategorias, recursos.GetString("Categorias"))
        GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)

    End Sub

    Private Sub GridVisibilidad(grid As Grid, boton As Button, seccion As String)

        tbTitulo.Text = "Steam Categories (" + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString + ") - " + seccion

        gridCategorias.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonCategorias.Background = New SolidColorBrush(Colors.Transparent)
        botonConfig.Background = New SolidColorBrush(Colors.Transparent)

        If Not boton Is Nothing Then
            boton.Background = New SolidColorBrush(Colors.SteelBlue)
        End If

    End Sub

    Private Sub BotonCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonCategorias.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        GridVisibilidad(gridCategorias, botonCategorias, recursos.GetString("Categorias"))
        GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)

    End Sub

    Private Sub BotonConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonConfig.Click

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()
        GridVisibilidad(gridConfig, botonConfig, recursos.GetString("Boton Config"))
        GridVisibilidadConfig(gridConfigCategorias, botonConfigCategorias)

    End Sub

    Private Async Sub BotonVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Private Sub BotonMasCosas_Click(sender As Object, e As RoutedEventArgs) Handles botonMasCosas.Click

        If popupMasCosas.IsOpen = True Then
            botonMasCosas.Background = New SolidColorBrush(Colors.Transparent)
            popupMasCosas.IsOpen = False
        Else
            botonMasCosas.Background = New SolidColorBrush(Colors.SteelBlue)
            popupMasCosas.IsOpen = True
        End If

    End Sub

    Private Sub PopupMasCosas_LayoutUpdated(sender As Object, e As Object) Handles popupMasCosas.LayoutUpdated

        popupMasCosas.Height = spMasCosas.ActualHeight

    End Sub

    Private Async Sub BotonMasApps_Click(sender As Object, e As RoutedEventArgs) Handles botonMasApps.Click

        Await Launcher.LaunchUriAsync(New Uri("https://pepeizqapps.com/"))

    End Sub

    Private Async Sub BotonContacto_Click(sender As Object, e As RoutedEventArgs) Handles botonContacto.Click

        Await Launcher.LaunchUriAsync(New Uri("https://pepeizqapps.com/contact/"))

    End Sub

    Private Async Sub BotonReportar_Click(sender As Object, e As RoutedEventArgs) Handles botonReportar.Click

        If StoreServicesFeedbackLauncher.IsSupported = True Then
            Dim ejecutador As StoreServicesFeedbackLauncher = StoreServicesFeedbackLauncher.GetDefault()
            Await ejecutador.LaunchAsync()
        Else
            Await Launcher.LaunchUriAsync(New Uri("https://pepeizqapps.com/contact/"))
        End If

    End Sub

    Private Async Sub BotonCodigoFuente_Click(sender As Object, e As RoutedEventArgs) Handles botonCodigoFuente.Click

        Await Launcher.LaunchUriAsync(New Uri("https://github.com/pepeizq/Steam-Categories"))

    End Sub

    'CATEGORIAS--------------------------------------------------------------

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

        If Not boton Is Nothing Then
            botonSeleccionUserscore.Background = New SolidColorBrush(Colors.Transparent)
            botonSeleccionMetascore.Background = New SolidColorBrush(Colors.Transparent)
            botonSeleccionAños.Background = New SolidColorBrush(Colors.Transparent)
            botonSeleccionCategorias.Background = New SolidColorBrush(Colors.Transparent)
            botonSeleccionGeneros.Background = New SolidColorBrush(Colors.Transparent)
            botonSeleccionTags.Background = New SolidColorBrush(Colors.Transparent)
            botonSeleccionIdiomas.Background = New SolidColorBrush(Colors.Transparent)

            boton.Background = New SolidColorBrush(Colors.DarkBlue)
        End If

        gridSeleccionUserscore.Visibility = Visibility.Collapsed
        gridSeleccionMetascore.Visibility = Visibility.Collapsed
        gridSeleccionAños.Visibility = Visibility.Collapsed
        gridSeleccionCategorias.Visibility = Visibility.Collapsed
        gridSeleccionGeneros.Visibility = Visibility.Collapsed
        gridSeleccionTags.Visibility = Visibility.Collapsed
        gridSeleccionIdiomas.Visibility = Visibility.Collapsed
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

    Private Sub BotonSeleccionUserscore_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionUserscore.Click

        GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)

    End Sub

    Private Sub BotonSeleccionMetascore_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionMetascore.Click

        GridSeleccionVisibilidad(gridSeleccionMetascore, botonSeleccionMetascore)

    End Sub

    Private Sub BotonSeleccionAños_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionAños.Click

        GridSeleccionVisibilidad(gridSeleccionAños, botonSeleccionAños)

    End Sub

    Private Sub BotonSeleccionCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionCategorias.Click

        GridSeleccionVisibilidad(gridSeleccionCategorias, botonSeleccionCategorias)

    End Sub

    Private Sub BotonSeleccionGeneros_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionGeneros.Click

        GridSeleccionVisibilidad(gridSeleccionGeneros, botonSeleccionGeneros)

    End Sub

    Private Sub BotonSeleccionTags_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionTags.Click

        GridSeleccionVisibilidad(gridSeleccionTags, botonSeleccionTags)

    End Sub

    Private Sub BotonSeleccionIdiomas_Click(sender As Object, e As RoutedEventArgs) Handles botonSeleccionIdiomas.Click

        GridSeleccionVisibilidad(gridSeleccionIdiomas, botonSeleccionIdiomas)

    End Sub

    '--------------------------------------------------------------

    Private Sub CbSeleccionUserscore_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionUserscore.Checked

        CbSeleccionChecked("/*1/" + cbSeleccionUserscore.Content)

    End Sub

    Private Sub CbSeleccionUserscore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionUserscore.Unchecked

        CbSeleccionUnChecked("/*1/" + cbSeleccionUserscore.Content)

    End Sub

    Private Sub CbSeleccionMetascore_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Checked

        CbSeleccionChecked("/*2/" + cbSeleccionMetascore.Content)

    End Sub

    Private Sub CbSeleccionMetascore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Unchecked

        CbSeleccionUnChecked("/*2/" + cbSeleccionMetascore.Content)

    End Sub

    Private Sub CbSeleccionAños_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Checked

        CbSeleccionChecked("/*3/" + cbSeleccionAños.Content)

    End Sub

    Private Sub CbSeleccionAños_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Unchecked

        CbSeleccionUnChecked("/*3/" + cbSeleccionAños.Content)

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

    Private Sub ExpanderUserscore_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles expanderUserscore.SizeChanged

        If expanderUserscore.IsExpanded = True Then
            ApplicationData.Current.LocalSettings.Values("expanderUserscore") = "on"
        Else
            ApplicationData.Current.LocalSettings.Values("expanderUserscore") = "off"
        End If

    End Sub

    Private Sub ExpanderMetascore_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles expanderMetascore.SizeChanged

        If expanderMetascore.IsExpanded = True Then
            ApplicationData.Current.LocalSettings.Values("expanderMetascore") = "on"
        Else
            ApplicationData.Current.LocalSettings.Values("expanderMetascore") = "off"
        End If

    End Sub

    Private Sub ExpanderAños_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles expanderAños.SizeChanged

        If expanderAños.IsExpanded = True Then
            ApplicationData.Current.LocalSettings.Values("expanderAños") = "on"
        Else
            ApplicationData.Current.LocalSettings.Values("expanderAños") = "off"
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

    Private Sub GridVisibilidadConfig(grid As Grid, boton As Button)

        gridConfigCategorias.Visibility = Visibility.Collapsed
        gridConfigCategoriasPersonalizar.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonConfigCategorias.Background = New SolidColorBrush(Colors.Transparent)
        botonConfigCategoriasPersonalizar.Background = New SolidColorBrush(Colors.Transparent)

        boton.Background = New SolidColorBrush(Colors.DarkBlue)

    End Sub

    Private Sub BotonConfigCategorias_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigCategorias.Click

        GridVisibilidadConfig(gridConfigCategorias, botonConfigCategorias)

    End Sub

    Private Sub BotonConfigCategoriasPersonalizar_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigCategoriasPersonalizar.Click

        GridVisibilidadConfig(gridConfigCategoriasPersonalizar, botonConfigCategoriasPersonalizar)

    End Sub

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
                cbPersonalizarmetascore9.IsChecked = score.Estado
            End If

            If score.Nombre = "8" Then
                cbPersonalizarmetascore8.IsChecked = score.Estado
            End If

            If score.Nombre = "7" Then
                cbPersonalizarmetascore7.IsChecked = score.Estado
            End If

            If score.Nombre = "6" Then
                cbPersonalizarmetascore6.IsChecked = score.Estado
            End If

            If score.Nombre = "5" Then
                cbPersonalizarmetascore5.IsChecked = score.Estado
            End If

            If score.Nombre = "4" Then
                cbPersonalizarmetascore4.IsChecked = score.Estado
            End If

            If score.Nombre = "3" Then
                cbPersonalizarmetascore3.IsChecked = score.Estado
            End If

            If score.Nombre = "2" Then
                cbPersonalizarmetascore2.IsChecked = score.Estado
            End If

            If score.Nombre = "1" Then
                cbPersonalizarmetascore1.IsChecked = score.Estado
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

End Class
