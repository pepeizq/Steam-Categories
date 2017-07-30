Imports Microsoft.Services.Store.Engagement
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

        botonConfigCategoriasTexto.Text = recursos.GetString("Categorias")
        tbSteamConfigInstruccionesCliente.Text = recursos.GetString("Texto Steam Config Cliente")
        botonSteamRutaTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamRuta.Text = recursos.GetString("Texto Carpeta")
        botonSteamCuentaTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigInstruccionesCuenta.Text = recursos.GetString("Texto Steam Config Cuenta")
        botonCargaCategoriasTexto.Text = recursos.GetString("Boton Carga Categorias")
        tbCargaCategoriasAviso.Text = recursos.GetString("Aviso Carga")
        tbJuegosCuentaMensaje.Text = recursos.GetString("Texto Juegos Cuenta")
        tbJuegosAppMensaje.Text = recursos.GetString("Texto Juegos App")
        tbBorrarCategoriasAppTexto.Text = recursos.GetString("Boton Borrar Categorias App")

        '----------------------------------------------

        Cliente.Detectar(False)
        Cuentas.Detectar()
        GridVisibilidad(gridCategorias, botonCategorias, recursos.GetString("Categorias"))
        GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)

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

        If Await helper.FileExistsAsync("listaJuegos") = True Then
            Dim listaJuegos As List(Of Juego) = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")

            If Not listaJuegos Is Nothing Then
                If listaJuegos.Count > 0 Then
                    GridSeleccionVisibilidad(gridSeleccionUserscore, botonSeleccionUserscore)

                    Categorias.GenerarCategorias(listaJuegos)
                    Categorias.GenerarGeneros(listaJuegos)
                    Categorias.GenerarTags(listaJuegos)
                    Categorias.GenerarIdiomas(listaJuegos)

                    tbJuegosApp.Text = listaJuegos.Count.ToString
                End If
            End If
        End If

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")

            listaCategorias.RemoveRange(0, listaCategorias.Count)

            Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)
        End If

        If Await helper.FileExistsAsync("actualizar") = True Then
            Dim actualizar As Boolean = Await helper.ReadFileAsync(Of Boolean)("actualizar")
            cbActualizarListaJuegos.IsChecked = actualizar
        End If

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

        Cliente.EscribirCategorias(botonEscribirCategorias)

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
            Dim listaJuegos As List(Of Juego) = Await helper.ReadFileAsync(Of List(Of Juego))("listaJuegos")

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

        CbSeleccionChecked("/*/userscore/*/")

    End Sub

    Private Sub CbSeleccionUserscore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionUserscore.Unchecked

        CbSeleccionUnChecked("/*/userscore/*/")

    End Sub

    Private Sub CbSeleccionMetascore_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Checked

        CbSeleccionChecked("/*/metascore/*/")

    End Sub

    Private Sub CbSeleccionMetascore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Unchecked

        CbSeleccionUnChecked("/*/metascore/*/")

    End Sub

    Private Sub CbSeleccionAños_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Checked

        CbSeleccionChecked("/*/años/*/")

    End Sub

    Private Sub CbSeleccionAños_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Unchecked

        CbSeleccionUnChecked("/*/años/*/")

    End Sub

    Private Async Sub CbSeleccionChecked(categoria As String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim listaCategorias As List(Of String)

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            listaCategorias = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")
        Else
            listaCategorias = New List(Of String)
        End If

        listaCategorias.Add(categoria)

        If listaCategorias.Count > 0 Then
            botonEscribirCategorias.IsEnabled = True
        End If

        Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)

    End Sub

    Private Async Sub CbSeleccionUnChecked(categoria As String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")

            listaCategorias.Remove(categoria)

            If listaCategorias.Count = 0 Then
                botonEscribirCategorias.IsEnabled = False
            End If

            Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)
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

    'CONFIG--------------------------------------------------------------

    Private Sub GridVisibilidadConfig(grid As Grid, boton As Button)

        gridConfigCategorias.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

        botonConfigCategorias.Background = New SolidColorBrush(Colors.Transparent)

        boton.Background = New SolidColorBrush(Colors.DarkBlue)

    End Sub

    Private Sub BotonSteamRuta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamRuta.Click

        Cliente.Detectar(True)

    End Sub

    Private Sub BotonSteamCuenta_Click(sender As Object, e As RoutedEventArgs) Handles botonSteamCuenta.Click

        Cuentas.Detectar()

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

    Private Async Sub BotonBorrarCategoriasApp_Click(sender As Object, e As RoutedEventArgs) Handles botonBorrarCategoriasApp.Click

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Await helper.SaveFileAsync(Of List(Of Juego))("listaJuegos", New List(Of Juego))

        gvCategorias.Items.Clear()
        gvGeneros.Items.Clear()
        gvTags.Items.Clear()
        gvIdiomas.Items.Clear()

        tbJuegosApp.Text = 0

    End Sub

End Class
