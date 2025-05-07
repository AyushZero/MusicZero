# MusicZero - Spotify Controller

A sleek, minimal Spotify controller that sits in your system tray and provides quick access to your music controls.

## Features

- 🎵 Control Spotify playback (play/pause, next, previous)
- 🎧 View current track information
- 📝 See upcoming tracks in queue
- 🖱️ Auto-hiding interface that expands on hover
- 🔄 Real-time playback progress
- 🎯 System tray integration for easy access
- 🎨 Modern, minimal UI design

## Requirements

- Windows 10 or later
- .NET 8.0 Runtime
- Spotify Desktop App installed and running
- Spotify Premium account (required for API access)

## Setup

1. Clone this repository
2. Open the solution in Visual Studio 2022 or later
3. Copy `appsettings.template.json` to `appsettings.json`
4. Get your Spotify API credentials:
   - Go to [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
   - Create a new application
   - Add `http://127.0.0.1:5000/callback` to your Redirect URIs
   - Copy your Client ID and Client Secret
   - Update `appsettings.json` with your credentials
5. Build and run the application
6. On first run, you'll be prompted to authorize the app with your Spotify account
7. After authorization, the app will minimize to your system tray

## Usage

- **Show/Hide**: Click the MusicZero icon in the system tray
- **Controls**:
  - Play/Pause: Click the play/pause button
  - Next Track: Click the next button
  - Previous Track: Click the previous button
- **Interface**:
  - Hover over the app to expand it
  - Move mouse away to auto-hide
  - Press ESC to minimize to tray
  - Drag the window to reposition

## System Tray Features

- Double-click the tray icon to show the window
- Right-click for options:
  - Show: Display the main window
  - Exit: Close the application

## Development

### Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 SDK
- Spotify Developer Account

### Spotify API Setup

1. Go to [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
2. Create a new application
3. Add `http://127.0.0.1:5000/callback` to your Redirect URIs
4. Copy your Client ID and Client Secret
5. Update the credentials in `MainWindow.xaml.cs`

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Built with WPF and .NET 8.0
- Uses [SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET) for Spotify integration 