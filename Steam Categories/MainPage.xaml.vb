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

        buttonEscribirCategoriasTexto.Text = recursos.GetString("Boton Escribir Categorias")
        buttonBorrarCategoriasTexto.Text = recursos.GetString("Boton Borrar Categorias")
        tbAvisoSteamCerrado.Text = recursos.GetString("Aviso Steam")

        expanderUserscore.Header = recursos.GetString("Como Funciona")
        tbSeleccionUserscoreInfo.Text = recursos.GetString("Texto Seleccion Userscore")

        expanderMetascore.Header = recursos.GetString("Como Funciona")
        tbSeleccionMetascoreInfo.Text = recursos.GetString("Texto Seleccion Metascore")

        expanderAños.Header = recursos.GetString("Como Funciona")
        buttonSeleccionAños.Content = recursos.GetString("Año Lanzamiento")
        cbSeleccionAños.Content = recursos.GetString("Año Lanzamiento")
        tbSeleccionAñosInfo.Text = recursos.GetString("Texto Seleccion Años")

        buttonSeleccionCategorias.Content = recursos.GetString("Categorias")
        buttonSeleccionGeneros.Content = recursos.GetString("Generos")
        buttonSeleccionTags.Content = recursos.GetString("Tags")
        buttonSeleccionIdiomas.Content = recursos.GetString("Idiomas")

        tbNoJuegos.Text = recursos.GetString("No Juegos")

        botonConfigCategoriasTexto.Text = recursos.GetString("Categorias")
        tbSteamConfigInstruccionesCliente.Text = recursos.GetString("Texto Steam Config Cliente")
        buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        tbSteamConfigInstruccionesCuenta.Text = recursos.GetString("Texto Steam Config Cuenta")
        buttonCargaCategoriasTexto.Text = recursos.GetString("Boton Carga Categorias")
        tbCargaCategoriasAviso.Text = recursos.GetString("Aviso Carga")
        tbJuegosCuentaMensaje.Text = recursos.GetString("Texto Juegos Cuenta")
        tbJuegosAppMensaje.Text = recursos.GetString("Texto Juegos App")
        tbBorrarCategoriasAppTexto.Text = recursos.GetString("Boton Borrar Categorias App")

        '----------------------------------------------

        Steam.ArranqueCliente(False)
        Steam.ArranqueCuenta()
        GridVisibilidad(gridCategorias, botonCategorias, recursos.GetString("Categorias"))
        GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

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

        If Await helper.FileExistsAsync("lista") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Juego))("lista")

            If Not listaJuegos Is Nothing Then
                If listaJuegos.Count > 0 Then
                    GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

                    gridSeleccionCategorias.Children.Add(Listado.GenerarCategorias(listaJuegos))
                    gridSeleccionGeneros.Children.Add(Listado.GenerarGeneros(listaJuegos))
                    gridSeleccionTags.Children.Add(Listado.GenerarTags(listaJuegos))
                    gridSeleccionIdiomas.Children.Add(Listado.GenerarIdiomas(listaJuegos))

                    tbJuegosApp.Text = listaJuegos.Count.ToString
                End If
            End If
        End If

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")

            listaCategorias.RemoveRange(0, listaCategorias.Count)

            Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)
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
        GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

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

    Private Sub ButtonEscribirCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonEscribirCategorias.Click

        Steam.EscribirCategorias(listaJuegos, buttonEscribirCategorias)

    End Sub

    Private Sub ButtonEscribirCategorias_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles buttonEscribirCategorias.PointerEntered

        panelAviso.HorizontalAlignment = HorizontalAlignment.Left
        panelAviso.Visibility = Visibility.Visible

    End Sub

    Private Sub ButtonEscribirCategorias_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles buttonEscribirCategorias.PointerExited

        panelAviso.Visibility = Visibility.Collapsed

    End Sub

    Private Sub ButtonBorrarCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonBorrarCategorias.Click

        Steam.BorrarCategorias(buttonBorrarCategorias)

    End Sub

    Private Sub ButtonBorrarCategorias_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles buttonBorrarCategorias.PointerEntered

        panelAviso.HorizontalAlignment = HorizontalAlignment.Right
        panelAviso.Visibility = Visibility.Visible

    End Sub

    Private Sub ButtonBorrarCategorias_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles buttonBorrarCategorias.PointerExited

        panelAviso.Visibility = Visibility.Collapsed

    End Sub

    '--------------------------------------------------------------

    Private Async Sub GridSeleccionVisibilidad(grid As Grid, boton As Button)

        If Not boton Is Nothing Then
            buttonSeleccionUserscore.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionMetascore.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionAños.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionCategorias.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionGeneros.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionTags.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionIdiomas.Background = New SolidColorBrush(Colors.Transparent)

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

        If Await helper.FileExistsAsync("lista") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Juego))("lista")

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

    Private Sub ButtonSeleccionUserscore_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionUserscore.Click

        GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

    End Sub

    Private Sub ButtonSeleccionMetascore_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionMetascore.Click

        GridSeleccionVisibilidad(gridSeleccionMetascore, buttonSeleccionMetascore)

    End Sub

    Private Sub ButtonSeleccionAños_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionAños.Click

        GridSeleccionVisibilidad(gridSeleccionAños, buttonSeleccionAños)

    End Sub

    Private Sub ButtonSeleccionCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionCategorias.Click

        GridSeleccionVisibilidad(gridSeleccionCategorias, buttonSeleccionCategorias)

    End Sub

    Private Sub ButtonSeleccionGeneros_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionGeneros.Click

        GridSeleccionVisibilidad(gridSeleccionGeneros, buttonSeleccionGeneros)

    End Sub

    Private Sub ButtonSeleccionTags_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionTags.Click

        GridSeleccionVisibilidad(gridSeleccionTags, buttonSeleccionTags)

    End Sub

    Private Sub ButtonSeleccionIdiomas_Click(sender As Object, e As RoutedEventArgs) Handles buttonSeleccionIdiomas.Click

        GridSeleccionVisibilidad(gridSeleccionIdiomas, buttonSeleccionIdiomas)

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
            buttonEscribirCategorias.IsEnabled = True
        End If

        Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)

    End Sub

    Private Async Sub CbSeleccionUnChecked(categoria As String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")

            listaCategorias.Remove(categoria)

            If listaCategorias.Count = 0 Then
                buttonEscribirCategorias.IsEnabled = False
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

    Private Sub ButtonSteamConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonSteamConfigPath.Click

        Steam.ArranqueCliente(True)

    End Sub

    Private Sub TbSteamConfigCuenta_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbSteamConfigCuenta.TextChanged

        Steam.ArranqueCuenta()

    End Sub

    Private Sub ButtonCargaCategorias_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles buttonCargaCategorias.PointerEntered

        panelAvisoCargaCategorias.Visibility = Visibility.Visible

    End Sub

    Private Sub ButtonCargaCategorias_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles buttonCargaCategorias.PointerExited

        panelAvisoCargaCategorias.Visibility = Visibility.Collapsed

    End Sub

    Dim listaJuegos As List(Of Juego)

    Private Async Sub ButtonCargaCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonCargaCategorias.Click

        gridProgreso.Visibility = Visibility.Visible
        prProgreso.IsIndeterminate = False
        buttonCargaCategorias.IsEnabled = False
        buttonBorrarCategorias.IsEnabled = False
        buttonBorrarCategoriasApp.IsEnabled = False
        buttonSteamConfigPath.IsEnabled = False
        tbSteamConfigCuenta.IsEnabled = False

        Dim listaJuegosID As List(Of String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaJuegosID") = True Then
            listaJuegosID = Await helper.ReadFileAsync(Of List(Of String))("listaJuegosID")
        Else
            listaJuegosID = Nothing
        End If

        If Not listaJuegosID Is Nothing Then
            Dim tope As Integer = listaJuegosID.Count

            listaJuegos = New List(Of Juego)

            Dim i As Integer = 0
            While i < tope
                Dim html As String = Await Decompiladores.HttpClient(New Uri("http://steamspy.com/app/" + listaJuegosID(i)))

                If Not html = Nothing Then
                    Dim titulo As String = Nothing
                    Dim userscore As String = Nothing
                    Dim metascore As String = Nothing
                    Dim año As String = Nothing
                    Dim categorias As New List(Of String)
                    Dim generos As New List(Of String)
                    Dim tags As New List(Of String)
                    Dim idiomas As New List(Of String)

                    Dim temp, temp2, temp3 As String
                    Dim int, int2, int3 As Integer

                    int = html.IndexOf("<div class=" + ChrW(34) + "p-r-30")
                    temp = html.Remove(0, int)

                    int2 = temp.IndexOf("<div>")
                    temp2 = temp.Remove(0, int2 + 5)

                    int3 = temp2.IndexOf("</div>")
                    temp3 = temp2.Remove(int3, temp2.Length - int3)

                    temp3 = temp3.Replace("<h3>", Nothing)
                    temp3 = temp3.Replace("</h3>", Nothing)
                    temp3 = temp3.Trim

                    titulo = temp3

                    If html.Contains("<strong>Userscore:</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Userscore:</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("%")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Userscore:", Nothing)
                        temp5 = temp5.Trim

                        userscore = temp5
                    End If

                    If html.Contains("<strong>Metascore:</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Metascore:</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("%")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Metascore:", Nothing)
                        temp5 = temp5.Trim

                        metascore = temp5
                    End If

                    If html.Contains("<strong>Release date</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Release date</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("<br>")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Release date", Nothing)
                        temp5 = temp5.Replace(":", Nothing)

                        If temp5.Contains(",") Then
                            Dim int6 As Integer

                            int6 = temp5.IndexOf(",")
                            temp5 = temp5.Remove(0, int6 + 1)
                        End If

                        If temp5.Contains("19") Then
                            Dim int6 As Integer

                            int6 = temp5.IndexOf("19")
                            temp5 = temp5.Remove(0, int6)

                            If (temp5.Length - (int6 + 4)) > 0 Then
                                temp5 = temp5.Remove(int6 + 4, temp5.Length - (int6 + 4))
                            End If
                        End If

                        If temp5.Contains("20") Then
                            Dim int6 As Integer

                            int6 = temp5.IndexOf("20")
                            temp5 = temp5.Remove(0, int6)

                            If (temp5.Length - (int6 + 4)) > 0 Then
                                temp5 = temp5.Remove(int6 + 4, temp5.Length - (int6 + 4))
                            End If
                        End If

                        temp5 = temp5.Trim

                        año = temp5
                    End If

                    If html.Contains("<strong>Category:</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Category:</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("<br>")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Category:", Nothing)
                        temp5 = temp5.Trim

                        If Not temp5.Contains(",") Then
                            categorias.Add(temp5)
                        Else
                            Dim j As Integer = 0
                            While j < 100
                                Dim temp6 As String
                                Dim int6 As Integer

                                If temp5.Contains(",") Then
                                    int6 = temp5.IndexOf(",")
                                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                                    categorias.Add(temp6.Trim)

                                    temp5 = temp5.Remove(0, int6 + 1)
                                Else
                                    categorias.Add(temp5.Trim)
                                    Exit While
                                End If
                                j += 1
                            End While
                        End If
                    End If

                    If html.Contains("<strong>Genre:</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Genre:</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("<br>")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Genre:", Nothing)
                        temp5 = temp5.Trim

                        If Not temp5.Contains(",") Then
                            If temp5.Contains("<") Then
                                Dim int7, int8 As Integer

                                int7 = temp5.IndexOf("<")
                                int8 = temp5.IndexOf(">")
                                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                temp5 = temp5.Replace("</a>", Nothing)
                            End If

                            generos.Add(temp5)
                        Else
                            Dim j As Integer = 0
                            While j < 100
                                Dim temp6 As String
                                Dim int6 As Integer

                                If temp5.Contains(",") Then
                                    int6 = temp5.IndexOf(",")
                                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                                    If temp6.Contains("<") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp6.IndexOf("<")
                                        int8 = temp6.IndexOf(">")
                                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                        temp6 = temp6.Replace("</a>", Nothing)
                                    End If

                                    generos.Add(temp6.Trim)

                                    temp5 = temp5.Remove(0, int6 + 1)
                                Else
                                    If temp5.Contains("<") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp5.IndexOf("<")
                                        int8 = temp5.IndexOf(">")
                                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                        temp5 = temp5.Replace("</a>", Nothing)
                                    End If

                                    generos.Add(temp5.Trim)
                                    Exit While
                                End If
                                j += 1
                            End While
                        End If
                    End If

                    If html.Contains("<strong>Tags:</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Tags:</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("<br>")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Tags:", Nothing)
                        temp5 = temp5.Trim

                        If Not temp5.Contains(",") Then
                            If temp5.Contains("<") Then
                                Dim int7, int8 As Integer

                                int7 = temp5.IndexOf("<")
                                int8 = temp5.IndexOf(">")
                                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                temp5 = temp5.Replace("</a>", Nothing)
                            End If

                            If temp5.Contains("(") Then
                                Dim int7, int8 As Integer

                                int7 = temp5.IndexOf("(")
                                int8 = temp5.IndexOf(")")
                                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                            End If

                            tags.Add(temp5)
                        Else
                            Dim j As Integer = 0
                            While j < 100
                                Dim temp6 As String
                                Dim int6 As Integer

                                If temp5.Contains(",") Then
                                    int6 = temp5.IndexOf(",")
                                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                                    If temp6.Contains("<") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp6.IndexOf("<")
                                        int8 = temp6.IndexOf(">")
                                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                        temp6 = temp6.Replace("</a>", Nothing)
                                    End If

                                    If temp6.Contains("(") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp6.IndexOf("(")
                                        int8 = temp6.IndexOf(")")
                                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                    End If

                                    tags.Add(temp6.Trim)

                                    temp5 = temp5.Remove(0, int6 + 1)
                                Else
                                    If temp5.Contains("<") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp5.IndexOf("<")
                                        int8 = temp5.IndexOf(">")
                                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                        temp5 = temp5.Replace("</a>", Nothing)
                                    End If

                                    If temp5.Contains("(") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp5.IndexOf("(")
                                        int8 = temp5.IndexOf(")")
                                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                    End If

                                    tags.Add(temp5.Trim)
                                    Exit While
                                End If
                                j += 1
                            End While
                        End If
                    End If

                    If html.Contains("<strong>Languages:</strong>") Then
                        Dim temp4, temp5 As String
                        Dim int4, int5 As Integer

                        int4 = html.IndexOf("<strong>Languages:</strong>")
                        temp4 = html.Remove(0, int4)

                        int5 = temp4.IndexOf("<br>")
                        temp5 = temp4.Remove(int5, temp4.Length - int5)

                        temp5 = temp5.Replace("<strong>", Nothing)
                        temp5 = temp5.Replace("</strong>", Nothing)
                        temp5 = temp5.Replace("Languages:", Nothing)
                        temp5 = temp5.Replace("[b]*[/b]", Nothing)
                        temp5 = temp5.Replace(";", Nothing)
                        temp5 = temp5.Trim

                        If Not temp5.Contains(",") Then
                            If temp5.Contains("<") Then
                                Dim int7, int8 As Integer

                                int7 = temp5.IndexOf("<")
                                int8 = temp5.IndexOf(">")
                                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                temp5 = temp5.Replace("</a>", Nothing)
                            End If

                            If temp5.Contains("(") Then
                                Dim int7, int8 As Integer

                                int7 = temp5.IndexOf("(")
                                int8 = temp5.IndexOf(")")
                                temp5 = temp5.Remove(int7, int8 - int7 + 1)
                            End If

                            idiomas.Add(temp5.Trim)
                        Else
                            Dim j As Integer = 0
                            While j < 100
                                Dim temp6 As String
                                Dim int6 As Integer

                                If temp5.Contains(",") Then
                                    int6 = temp5.IndexOf(",")
                                    temp6 = temp5.Remove(int6, temp5.Length - int6)

                                    If temp6.Contains("<") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp6.IndexOf("<")
                                        int8 = temp6.IndexOf(">")
                                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                        temp6 = temp6.Replace("</a>", Nothing)
                                    End If

                                    If temp6.Contains("(") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp6.IndexOf("(")
                                        int8 = temp6.IndexOf(")")
                                        temp6 = temp6.Remove(int7, int8 - int7 + 1)
                                    End If

                                    idiomas.Add(temp6.Trim)

                                    temp5 = temp5.Remove(0, int6 + 1)
                                Else
                                    If temp5.Contains("<") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp5.IndexOf("<")
                                        int8 = temp5.IndexOf(">")
                                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                        temp5 = temp5.Replace("</a>", Nothing)
                                    End If

                                    If temp5.Contains("(") Then
                                        Dim int7, int8 As Integer

                                        int7 = temp5.IndexOf("(")
                                        int8 = temp5.IndexOf(")")
                                        temp5 = temp5.Remove(int7, int8 - int7 + 1)
                                    End If

                                    idiomas.Add(temp5.Trim)
                                    Exit While
                                End If
                                j += 1
                            End While
                        End If
                    End If

                    Dim juego As New Juego(titulo, listaJuegosID(i), userscore, metascore, año, categorias, generos, tags, idiomas)

                    listaJuegos.Add(juego)
                End If

                prProgreso.Value = (i / tope) * 100
                tbProgreso.Text = "(" + i.ToString + "/" + tope.ToString + ")"

                i += 1
            End While
        End If

        tbJuegosApp.Text = listaJuegos.Count.ToString

        Await helper.SaveFileAsync(Of List(Of Juego))("lista", listaJuegos)

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        If listaJuegos.Count > 0 Then
            GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

            gridSeleccionCategorias.Children.Clear()
            gridSeleccionGeneros.Children.Clear()
            gridSeleccionTags.Children.Clear()
            gridSeleccionIdiomas.Children.Clear()

            gridSeleccionCategorias.Children.Add(Listado.GenerarCategorias(listaJuegos))
            gridSeleccionGeneros.Children.Add(Listado.GenerarGeneros(listaJuegos))
            gridSeleccionTags.Children.Add(Listado.GenerarTags(listaJuegos))
            gridSeleccionIdiomas.Children.Add(Listado.GenerarIdiomas(listaJuegos))

            Toast("Steam Categories", recursos.GetString("Cargado Si"))
        Else
            Toast("Steam Categories", recursos.GetString("Cargado No"))
        End If

        gridProgreso.Visibility = Visibility.Collapsed
        buttonCargaCategorias.IsEnabled = True
        buttonBorrarCategorias.IsEnabled = True
        buttonBorrarCategoriasApp.IsEnabled = True
        buttonSteamConfigPath.IsEnabled = True
        tbSteamConfigCuenta.IsEnabled = True

    End Sub

    Private Async Sub ButtonBorrarCategoriasApp_Click(sender As Object, e As RoutedEventArgs) Handles buttonBorrarCategoriasApp.Click

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        Dim lista As New List(Of Juego)
        Await helper.SaveFileAsync(Of List(Of Juego))("lista", lista)

        gridSeleccionCategorias.Children.Clear()
        gridSeleccionGeneros.Children.Clear()
        gridSeleccionTags.Children.Clear()
        gridSeleccionIdiomas.Children.Clear()

        tbJuegosApp.Text = 0

    End Sub
End Class
