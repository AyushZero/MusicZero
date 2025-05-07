using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Threading.Tasks;
using System.Timers;

namespace MusicZero
{
    public partial class MainWindow : Window
    {
        private SpotifyClient _spotify;
        private EmbedIOAuthServer _server;
        private string _clientId = "ef99f899190c443ebd365f5260e67ca7"; // You'll need to replace this with your Spotify Client ID
        private string _clientSecret = "11f0d05bd4d941deb668a35487edb143"; // You'll need to replace this with your Spotify Client Secret
        private Timer _updateTimer;
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
                Scope = new[] { Scopes.UserReadPlaybackState, Scopes.UserModifyPlaybackState }
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
            _updateTimer = new Timer(1000);
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
                        PlayPauseButton.Content = playback.IsPlaying ? "⏸" : "▶";
                    });
                }
            }
            catch (Exception)
            {
                // Handle any errors silently
            }
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
    }
} 