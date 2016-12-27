Imports Microsoft.Toolkit.Uwp

Module Listado

    Public Function GenerarCategorias(listaJuegos As List(Of Juego))

        Dim gridView As New GridView
        Dim listaCategorias As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each categoria_ In listaJuegos(i).Categorias
                        If listaCategorias.Count > 0 Then
                            Dim boolCategoria As Boolean = False

                            Dim j As Integer = 0
                            While j < listaCategorias.Count
                                If categoria_ = listaCategorias(j) Then
                                    boolCategoria = True
                                End If
                                j += 1
                            End While

                            If boolCategoria = False Then
                                listaCategorias.Add(categoria_)
                            End If
                        Else
                            listaCategorias.Add(categoria_)
                        End If
                    Next
                    i += 1
                End While

                listaCategorias.Sort()

                For Each categoria In listaCategorias
                    If categoria.Length > 0 Then
                        Dim cb As New CheckBox
                        cb.Content = categoria
                        cb.Margin = New Thickness(5, 5, 5, 5)
                        cb.Width = 200
                        cb.FontSize = 14

                        AddHandler cb.Checked, AddressOf cbChecked
                        AddHandler cb.Unchecked, AddressOf cbUnChecked

                        gridView.Items.Add(cb)
                    End If
                Next
            End If
        End If

        Return gridView

    End Function

    Public Function GenerarGeneros(listaJuegos As List(Of Juego))

        Dim gridView As New GridView
        Dim listaGeneros As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each genero_ In listaJuegos(i).Generos
                        If listaGeneros.Count > 0 Then
                            Dim boolGenero As Boolean = False

                            Dim j As Integer = 0
                            While j < listaGeneros.Count
                                If genero_ = listaGeneros(j) Then
                                    boolGenero = True
                                End If
                                j += 1
                            End While

                            If boolGenero = False Then
                                listaGeneros.Add(genero_)
                            End If
                        Else
                            listaGeneros.Add(genero_)
                        End If
                    Next
                    i += 1
                End While

                listaGeneros.Sort()

                For Each genero In listaGeneros
                    If genero.Length > 0 Then
                        Dim cb As New CheckBox
                        cb.Content = genero
                        cb.Margin = New Thickness(5, 5, 5, 5)
                        cb.Width = 200
                        cb.FontSize = 14

                        AddHandler cb.Checked, AddressOf cbChecked
                        AddHandler cb.Unchecked, AddressOf cbUnChecked

                        gridView.Items.Add(cb)
                    End If
                Next
            End If
        End If

        Return gridView

    End Function

    Public Function GenerarTags(listaJuegos As List(Of Juego))

        Dim gridView As New GridView
        Dim listaTags As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each tag_ In listaJuegos(i).Tags
                        If listaTags.Count > 0 Then
                            Dim boolTag As Boolean = False

                            Dim j As Integer = 0
                            While j < listaTags.Count
                                If tag_ = listaTags(j) Then
                                    boolTag = True
                                End If
                                j += 1
                            End While

                            If boolTag = False Then
                                listaTags.Add(tag_)
                            End If
                        Else
                            listaTags.Add(tag_)
                        End If
                    Next
                    i += 1
                End While

                listaTags.Sort()

                For Each tag In listaTags
                    If tag.Length > 0 Then
                        Dim cb As New CheckBox
                        cb.Content = tag
                        cb.Margin = New Thickness(5, 5, 5, 5)
                        cb.Width = 200
                        cb.FontSize = 14

                        AddHandler cb.Checked, AddressOf cbChecked
                        AddHandler cb.Unchecked, AddressOf cbUnChecked

                        gridView.Items.Add(cb)
                    End If
                Next
            End If
        End If

        Return gridView

    End Function

    Public Function GenerarIdiomas(listaJuegos As List(Of Juego))

        Dim gridView As New GridView
        Dim listaIdiomas As New List(Of String)

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                Dim i As Integer = 0
                While i < listaJuegos.Count

                    For Each idioma_ In listaJuegos(i).Idiomas
                        If listaIdiomas.Count > 0 Then
                            Dim boolIdioma As Boolean = False

                            Dim j As Integer = 0
                            While j < listaIdiomas.Count
                                If idioma_ = listaIdiomas(j) Then
                                    boolIdioma = True
                                End If
                                j += 1
                            End While

                            If boolIdioma = False Then
                                listaIdiomas.Add(idioma_)
                            End If
                        Else
                            listaIdiomas.Add(idioma_)
                        End If
                    Next
                    i += 1
                End While

                listaIdiomas.Sort()

                For Each idioma In listaIdiomas
                    If idioma.Length > 0 Then
                        Dim cb As New CheckBox
                        cb.Content = idioma
                        cb.Margin = New Thickness(5, 5, 5, 5)
                        cb.Width = 200
                        cb.FontSize = 14

                        AddHandler cb.Checked, AddressOf cbChecked
                        AddHandler cb.Unchecked, AddressOf cbUnChecked

                        gridView.Items.Add(cb)
                    End If
                Next
            End If
        End If

        Return gridView

    End Function

    Private Async Sub cbChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim cb As CheckBox = e.OriginalSource
        Dim categoria As String = TryCast(cb.Content, String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper
        Dim listaCategorias As List(Of String)

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            listaCategorias = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")
        Else
            listaCategorias = New List(Of String)
        End If

        If listaCategorias.Count > 0 Then
            Dim boolCategoria As Boolean = False

            Dim j As Integer = 0
            While j < listaCategorias.Count
                If categoria = listaCategorias(j) Then
                    boolCategoria = True
                End If
                j += 1
            End While

            If boolCategoria = False Then
                listaCategorias.Add(categoria)
            End If
        Else
            listaCategorias.Add(categoria)
        End If

        If listaCategorias.Count > 0 Then
            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content
            Dim boton As Button = pagina.FindName("buttonEscribirCategorias")
            boton.IsEnabled = True
        End If

        Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)

    End Sub

    Private Async Sub cbUnChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim cb As CheckBox = e.OriginalSource
        Dim categoria As String = TryCast(cb.Content, String)

        Dim helper As LocalObjectStorageHelper = New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("listaCategorias") = True Then
            Dim listaCategorias As List(Of String) = Await helper.ReadFileAsync(Of List(Of String))("listaCategorias")

            listaCategorias.Remove(categoria)

            If listaCategorias.Count = 0 Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content
                Dim boton As Button = pagina.FindName("buttonEscribirCategorias")
                boton.IsEnabled = False
            End If

            Await helper.SaveFileAsync(Of List(Of String))("listaCategorias", listaCategorias)
        End If

    End Sub

End Module
