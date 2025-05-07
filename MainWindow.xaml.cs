using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MusicZero
{
    public partial class MainWindow : Window
    {
        private SpotifyClient? _spotify;
        private EmbedIOAuthServer? _server;
        private System.Timers.Timer? _updateTimer;
        private string _clientId = "ef99f899190c443ebd365f5260e67ca7"; // Get this from https://developer.spotify.com/dashboard
        private string _clientSecret = "11f0d05bd4d941deb668a35487edb143"; // Get this from https://developer.spotify.com/dashboard
        private const string REDIRECT_URI = "http://127.0.0.1:5000/callback";

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpotify();
        }

        private async void InitializeSpotify()
        {
            _server = new EmbedIOAuthServer(new Uri(REDIRECT_URI), 5000);
            await _server.Start();

            _server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await _server.Stop();
                var config = SpotifyClientConfig.CreateDefault();
                var tokenResponse = await new OAuthClient().RequestToken(
                    new AuthorizationCodeTokenRequest(
                        _clientId, _clientSecret, response.Code, new Uri(REDIRECT_URI)
                    )
                );

                _spotify = new SpotifyClient(tokenResponse.AccessToken);
                StartUpdateTimer();
            };

            var loginRequest = new LoginRequest(_server.BaseUri, _clientId, LoginRequest.ResponseType.Code)
            {
                Scope = new[] { 
                    Scopes.UserReadPlaybackState, 
                    Scopes.UserModifyPlaybackState,
                    Scopes.UserReadCurrentlyPlaying,
                    Scopes.UserReadPrivate,
                    Scopes.PlaylistReadPrivate,
                    Scopes.PlaylistReadCollaborative
                }
            };

            var uri = loginRequest.ToUri();
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = uri.ToString(),
                UseShellExecute = true
            });
        }

        private void StartUpdateTimer()
        {
            _updateTimer = new System.Timers.Timer(1000);
            _updateTimer.Elapsed += async (sender, e) => await UpdatePlaybackInfo();
            _updateTimer.Start();
        }

        private async Task UpdatePlaybackInfo()
        {
            if (_spotify == null) return;

            try
            {
                var playback = await _spotify.Player.GetCurrentPlayback();
                if (playback?.Item is FullTrack track)
                {
                    Dispatcher.Invoke(() =>
                    {
                        TitleText.Text = track.Name;
                        ArtistText.Text = string.Join(", ", track.Artists.Select(a => a.Name));
                        PlayPauseIcon.Data = playback.IsPlaying 
                            ? Geometry.Parse("M6 19h4V5H6v14zm8-14v14h4V5h-4z")  // Pause icon
                            : Geometry.Parse("M8 5v14l11-7z");  // Play icon

                        // Update progress
                        ProgressBar.Maximum = track.DurationMs;
                        ProgressBar.Value = playback.ProgressMs;

                        // Get up next track
                        UpdateUpNext();
                    });
                }
            }
            catch (Exception)
            {
                // Handle any errors silently
            }
        }

        private async void UpdateUpNext()
        {
            if (_spotify == null) return;

            try
            {
                var queue = await _spotify.Player.GetQueue();
                if (queue?.Queue?.Count > 0)
                {
                    // Get the first track that isn't the current one
                    var nextTrack = queue.Queue.FirstOrDefault(t => t is FullTrack);
                    if (nextTrack is FullTrack track)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            UpNextText.Text = $"{track.Name} - {string.Join(", ", track.Artists.Select(a => a.Name))}";
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            UpNextText.Text = "Nothing";
                        });
                    }
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        UpNextText.Text = "Nothing";
                    });
                }
            }
            catch
            {
                Dispatcher.Invoke(() =>
                {
                    UpNextText.Text = "Nothing";
                });
            }
        }

        private string FormatTime(int milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            return $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}";
        }

        private async void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_spotify == null) return;

            try
            {
                var playback = await _spotify.Player.GetCurrentPlayback();
                if (playback?.IsPlaying == true)
                {
                    await _spotify.Player.PausePlayback();
                }
                else
                {
                    await _spotify.Player.ResumePlayback();
                }
            }
            catch (Exception)
            {
                // Handle any errors silently
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_spotify == null) return;

            try
            {
                await _spotify.Player.SkipNext();
            }
            catch (Exception)
            {
                // Handle any errors silently
            }
        }

        private async void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_spotify == null) return;

            try
            {
                await _spotify.Player.SkipPrevious();
            }
            catch (Exception)
            {
                // Handle any errors silently
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Height = 30;
            ContentPanel.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Height = 2;
            ContentPanel.Visibility = Visibility.Collapsed;
        }
    }
} 