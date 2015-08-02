'Imports System.ComponentModel
'Imports System.Media
Imports System.Windows.Media
Imports System.IO
Imports System.Windows.Threading

Class MainWindow
    Private mediaPath As String = "" '"C:\Users\home\Documents\Visual Studio 2010\Projects\PlayWaveTrials\Media"
    Private waveFids As New List(Of String)
    Private gx As Integer = 10
    Private gy As Integer = 10
    Private buttons As New List(Of Button)
    Private players As New Dictionary(Of String, WMPLib.WindowsMediaPlayer)
    'note: need to add reference to windows media player

    'Dim tcb As TimerCallback = AddressOf updateTick
    'Private update As New Timer(tcb, Nothing, 0, 1000)
    Private updateTimer As New DispatcherTimer()
    Private removeOnEnd As Boolean = True

    Private Sub updateTick()
        Dim remove_list As New List(Of String)
        For Each pk As String In players.Keys
            For Each b As Button In buttons
                If b.Content = pk Then
                    If players(pk).playState <> WMPLib.WMPPlayState.wmppsPlaying Then
                        b.Background = Brushes.LightBlue
                        If removeOnEnd Then
                            remove_list.Add(pk)
                        End If
                    Else
                        b.Background = Brushes.Pink
                    End If
                    Exit For
                End If
            Next
        Next
        For Each pk As String In remove_list
            players.Remove(pk)
        Next
    End Sub

    Private Sub windowKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        Select Case e.Key
            Case Key.A
                For Each pk As String In players.Keys
                    players(pk).controls.play()
                Next
            Case Key.L
                players.Clear()
                removeOnEnd = False
                For Each fid As String In waveFids
                    Dim player As New WMPLib.WindowsMediaPlayer
                    player.settings.autoStart = False
                    player.URL = fid
                    player.settings.balance = 100
                    players(Path.GetFileNameWithoutExtension(fid)) = player
                Next
        End Select
    End Sub

    Private Sub addButton(ByVal content As String)
        Dim btn As New Button
        Dim nm As String = Path.GetFileNameWithoutExtension(content)
        btn.Content = nm
        btn.HorizontalAlignment = HorizontalAlignment.Left
        btn.VerticalAlignment = VerticalAlignment.Top
        Dim btns As Integer = mainGrid.Children.Count
        If btns > 0 Then
            Dim last_btn As Button = mainGrid.Children(btns - 1)
            Dim w As Integer = last_btn.ActualWidth + 10
            gx += last_btn.Content.length * last_btn.FontSize / 2
            If gx + nm.Length * btn.FontSize > mainGrid.ActualWidth Then
                Dim h As Integer = last_btn.FontSize * 2
                gx = 0
                gy += h
            End If
        End If
        btn.Margin = New Thickness(gx, gy, 0, 0)
        AddHandler btn.Click, AddressOf playClick
        mainGrid.Children.Add(btn)
        buttons.Add(btn)
    End Sub

    Private Sub playClick(ByVal obj As Object, ByVal e As RoutedEventArgs)
        Dim btn As Button = obj
        For Each fid As String In waveFids
            If Path.GetFileNameWithoutExtension(fid) = btn.Content Then
                Dim player As New WMPLib.WindowsMediaPlayer
                player.URL = fid
                player.controls.play()
                players(btn.Content) = player
                Exit For
            End If
        Next
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim args As String() = Environment.GetCommandLineArgs()
        If args.Count > 1 Then
            mediaPath = args(1)
        Else
            mediaPath = Path.GetDirectoryName(args(0))
        End If
    End Sub

    Private Sub windowLoaded()
        Dim wav_fids = Directory.EnumerateFiles(mediaPath, "*.wav")

        For Each wav_fid As String In wav_fids
            waveFids.Add(wav_fid)
            addButton(Path.GetFileNameWithoutExtension(wav_fid))
        Next

        updateTimer.interval = TimeSpan.FromMilliseconds(1000)
        AddHandler updateTimer.Tick, AddressOf updateTick
        updateTimer.Start()
    End Sub

    Private Sub windowClosed()
        updateTimer.Stop()
        For Each pk As String In players.Keys
            players(pk).controls.stop()
            players(pk).close()
        Next
    End Sub

End Class
