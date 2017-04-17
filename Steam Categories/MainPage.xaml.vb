Imports Microsoft.Toolkit.Uwp
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.System
Imports Windows.UI

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar

        barra.BackgroundColor = Colors.DarkBlue
        barra.ForegroundColor = Colors.White
        barra.InactiveForegroundColor = Colors.White
        barra.ButtonBackgroundColor = Colors.DarkBlue
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveForegroundColor = Colors.White

        '--------------------------------------------------------

        Dim recursos As Resources.ResourceLoader = New Resources.ResourceLoader()

        botonInicioTexto.Text = recursos.GetString("Boton Inicio")
        botonCategoriasTexto.Text = recursos.GetString("Categorias")
        botonConfigTexto.Text = recursos.GetString("Boton Config")

        commadBarTop.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right

        botonInicioVotarTexto.Text = recursos.GetString("Boton Votar")
        botonInicioCompartirTexto.Text = recursos.GetString("Boton Compartir")
        botonInicioContactoTexto.Text = recursos.GetString("Boton Contactar")
        botonInicioMasAppsTexto.Text = recursos.GetString("Boton Web")

        tbRSS.Text = recursos.GetString("RSS")

        tbJuegosCuentaMensaje.Text = recursos.GetString("Texto Juegos Cuenta")
        tbJuegosAppMensaje.Text = recursos.GetString("Texto Juegos App")

        buttonCargaCategoriasTexto.Text = recursos.GetString("Boton Carga Categorias")
        buttonCargaCategoriasTooltip.Text = recursos.GetString("Aviso Carga")
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

        tbConfig.Text = recursos.GetString("Boton Config")
        tbSteamConfigInstruccionesCliente.Text = recursos.GetString("Texto Steam Config Cliente")
        buttonSteamConfigPathTexto.Text = recursos.GetString("Boton Añadir")
        tbSteamConfigPath.Text = recursos.GetString("Texto Carpeta")
        tbSteamConfigInstruccionesCuenta.Text = recursos.GetString("Texto Steam Config Cuenta")

        '----------------------------------------------

        tbConsejoConfig.Text = recursos.GetString("Consejo Config")
        tbInicioGrid.Text = recursos.GetString("Grid Arranque")

        cbItemArranqueInicio.Content = recursos.GetString("Boton Inicio")
        cbItemArranqueCategorias.Content = recursos.GetString("Categorias")
        cbItemArranqueConfig.Content = recursos.GetString("Boton Config")

        If ApplicationData.Current.LocalSettings.Values("cbarranque") = Nothing Then
            cbArranque.SelectedIndex = 0
            ApplicationData.Current.LocalSettings.Values("cbarranque") = "0"
        Else
            cbArranque.SelectedIndex = ApplicationData.Current.LocalSettings.Values("cbarranque")

            If cbArranque.SelectedIndex = 0 Then
                GridVisibilidad(gridInicio, botonInicio, Nothing)
            ElseIf cbArranque.SelectedIndex = 1 Then
                GridVisibilidad(gridCategorias, botonCategorias, Nothing)
                GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)
            ElseIf cbArranque.SelectedIndex = 2 Then
                GridVisibilidad(Nothing, botonConfig, gridConfig)
            Else
                GridVisibilidad(gridInicio, botonInicio, Nothing)
            End If
        End If

        tbVersionApp.Text = "App " + SystemInformation.ApplicationVersion.Major.ToString + "." + SystemInformation.ApplicationVersion.Minor.ToString + "." + SystemInformation.ApplicationVersion.Build.ToString + "." + SystemInformation.ApplicationVersion.Revision.ToString
        tbVersionWindows.Text = "Windows " + SystemInformation.OperatingSystemVersion.Major.ToString + "." + SystemInformation.OperatingSystemVersion.Minor.ToString + "." + SystemInformation.OperatingSystemVersion.Build.ToString + "." + SystemInformation.OperatingSystemVersion.Revision.ToString

        '--------------------------------------------------------

        Try
            RSS.Generar()
        Catch ex As Exception

        End Try

        Steam.ArranqueCliente(False)
        Steam.ArranqueCuenta(tbSteamConfigCuenta, prSteamConfigCuenta)

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

            If listaJuegos.Count > 0 Then
                GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

                gridSeleccionCategorias.Children.Add(Listado.GenerarCategorias(listaJuegos))
                gridSeleccionGeneros.Children.Add(Listado.GenerarGeneros(listaJuegos))
                gridSeleccionTags.Children.Add(Listado.GenerarTags(listaJuegos))
                gridSeleccionIdiomas.Children.Add(Listado.GenerarIdiomas(listaJuegos))

                tbJuegosApp.Text = listaJuegos.Count.ToString
            End If
        End If

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")

            listaCategorias.RemoveRange(0, listaCategorias.Count)

            Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)
        End If

    End Sub

    Private Sub GridVisibilidad(grid As Grid, boton As AppBarButton, sp As StackPanel)

        gridInicio.Visibility = Visibility.Collapsed
        gridCategorias.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridWeb.Visibility = Visibility.Collapsed

        If Not sp Is Nothing Then
            sp.Visibility = Visibility.Visible
        Else
            grid.Visibility = Visibility.Visible
        End If

        botonInicio.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonInicio.BorderThickness = New Thickness(0, 0, 0, 0)
        botonCategorias.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonCategorias.BorderThickness = New Thickness(0, 0, 0, 0)
        botonConfig.BorderBrush = New SolidColorBrush(Colors.Transparent)
        botonConfig.BorderThickness = New Thickness(0, 0, 0, 0)

        If Not boton Is Nothing Then
            boton.BorderBrush = New SolidColorBrush(Colors.White)
            boton.BorderThickness = New Thickness(0, 2, 0, 0)
        End If

    End Sub

    Private Sub BotonInicio_Click(sender As Object, e As RoutedEventArgs) Handles botonInicio.Click

        GridVisibilidad(gridInicio, botonInicio, Nothing)

    End Sub

    Private Sub BotonTilesSteam_Click(sender As Object, e As RoutedEventArgs) Handles botonCategorias.Click

        GridVisibilidad(gridCategorias, botonCategorias, Nothing)
        GridSeleccionVisibilidad(gridSeleccionUserscore, buttonSeleccionUserscore)

    End Sub

    Private Sub BotonConfig_Click(sender As Object, e As RoutedEventArgs) Handles botonConfig.Click

        GridVisibilidad(Nothing, botonConfig, gridConfig)

    End Sub

    Private Async Sub BotonInicioVotar_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioVotar.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

    End Sub

    Private Sub BotonInicioCompartir_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioCompartir.Click

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf MainPage_DataRequested
        DataTransferManager.ShowShareUI()

    End Sub

    Private Sub MainPage_DataRequested(sender As DataTransferManager, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request
        request.Data.SetText("Download: https://www.microsoft.com/store/apps/9p54scg1n6bm")
        request.Data.Properties.Title = "Steam Categories"
        request.Data.Properties.Description = "Add categories for your Steam games"

    End Sub

    Private Sub BotonInicioContacto_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioContacto.Click

        GridVisibilidad(gridWeb, Nothing, Nothing)

    End Sub

    Private Sub BotonInicioMasApps_Click(sender As Object, e As RoutedEventArgs) Handles botonInicioMasApps.Click

        If spMasApps.Visibility = Visibility.Visible Then
            spMasApps.Visibility = Visibility.Collapsed
        Else
            spMasApps.Visibility = Visibility.Visible
        End If

    End Sub

    Private Async Sub BotonAppSteamTiles_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamTiles.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh51sb3"))

    End Sub

    Private Async Sub BotonAppSteamDeals_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamDeals.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9p7836m1tw15"))

    End Sub

    Private Async Sub BotonAppSteamBridge_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamBridge.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh441c9"))

    End Sub

    Private Async Sub BotonAppSteamSkins_Click(sender As Object, e As RoutedEventArgs) Handles botonAppSteamSkins.Click

        Await Launcher.LaunchUriAsync(New Uri("ms-windows-store://pdp/?productid=9nblggh55b7f"))

    End Sub

    Private Async Sub LvRSSUpdates_ItemClick(sender As Object, e As ItemClickEventArgs) Handles lvRSSUpdates.ItemClick

        Dim feed As FeedRSS = e.ClickedItem
        Await Launcher.LaunchUriAsync(feed.Enlace)

    End Sub

    Private Sub CbArranque_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbArranque.SelectionChanged

        ApplicationData.Current.LocalSettings.Values("cbarranque") = cbArranque.SelectedIndex

    End Sub

    Private Async Sub BotonSocialTwitter_Click(sender As Object, e As RoutedEventArgs) Handles botonSocialTwitter.Click

        Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/pepeizqapps"))

    End Sub

    Private Async Sub BotonSocialGitHub_Click(sender As Object, e As RoutedEventArgs) Handles botonSocialGitHub.Click

        Await Launcher.LaunchUriAsync(New Uri("https://github.com/pepeizq"))

    End Sub

    Private Async Sub BotonSocialPaypal_Click(sender As Object, e As RoutedEventArgs) Handles botonSocialPaypal.Click

        Await Launcher.LaunchUriAsync(New Uri("https://paypal.me/pepeizq/1"))

    End Sub

    '--------------------------------------------------------------

    Private Sub ButtonSteamConfigPath_Click(sender As Object, e As RoutedEventArgs) Handles buttonSteamConfigPath.Click

        Steam.ArranqueCliente(True)

    End Sub

    Private Sub TbSteamConfigCuenta_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbSteamConfigCuenta.TextChanged

        Steam.ArranqueCuenta(tbSteamConfigCuenta, prSteamConfigCuenta)

    End Sub

    '--------------------------------------------------------------

    Dim listaJuegos As List(Of Juego)

    Private Async Sub ButtonCargaCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonCargaCategorias.Click

        gridProgreso.Visibility = Visibility.Visible
        prProgreso.IsIndeterminate = False
        buttonCargaCategorias.IsEnabled = False
        buttonBorrarCategorias.IsEnabled = False
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
        buttonSteamConfigPath.IsEnabled = True
        tbSteamConfigCuenta.IsEnabled = True

    End Sub

    Private Sub ButtonEscribirCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonEscribirCategorias.Click

        Steam.EscribirCategorias(listaJuegos, buttonEscribirCategorias)

    End Sub

    Private Sub ButtonBorrarCategorias_Click(sender As Object, e As RoutedEventArgs) Handles buttonBorrarCategorias.Click

        Steam.BorrarCategorias(buttonBorrarCategorias)

    End Sub

    '--------------------------------------------------------------

    Private Async Sub GridSeleccionVisibilidad(grid As Grid, boton As Button)

        If Not boton Is Nothing Then
            buttonSeleccionUserscore.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionUserscore.BorderBrush = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionMetascore.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionMetascore.BorderBrush = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionAños.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionAños.BorderBrush = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionCategorias.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionCategorias.BorderBrush = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionGeneros.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionGeneros.BorderBrush = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionTags.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionTags.BorderBrush = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionIdiomas.Background = New SolidColorBrush(Colors.Transparent)
            buttonSeleccionIdiomas.BorderBrush = New SolidColorBrush(Colors.Transparent)

            boton.Background = New SolidColorBrush(Colors.DarkBlue)
            boton.BorderBrush = New SolidColorBrush(Colors.White)
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

            If listaJuegos.Count = 0 Then
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

        cbSeleccionChecked("/*/userscore/*/")

    End Sub

    Private Sub CbSeleccionUserscore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionUserscore.Unchecked

        cbSeleccionUnChecked("/*/userscore/*/")

    End Sub

    Private Sub CbSeleccionMetascore_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Checked

        cbSeleccionChecked("/*/metascore/*/")

    End Sub

    Private Sub CbSeleccionMetascore_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionMetascore.Unchecked

        cbSeleccionUnChecked("/*/metascore/*/")

    End Sub

    Private Sub CbSeleccionAños_Checked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Checked

        cbSeleccionChecked("/*/años/*/")

    End Sub

    Private Sub CbSeleccionAños_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbSeleccionAños.Unchecked

        cbSeleccionUnChecked("/*/años/*/")

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


End Class
