# MusicZero

A sleek, always-on-top music controller for Spotify built with WPF (.NET 8.0). MusicZero provides a minimal interface for controlling your Spotify playback while staying out of your way.

## Features

- Always-on-top window
- Real-time track information display
- Playback controls (play/pause, next, previous)
- Progress bar with time display
- Up next track information
- Draggable interface
- Modern, minimal design

## Requirements

- Windows 10 or later
- .NET 8.0 SDK
- Spotify Premium account
- Spotify Developer credentials

## Setup

1. Clone the repository:
```bash
git clone https://github.com/yourusername/MusicZero.git
cd MusicZero
```

2. Create a Spotify Developer application:
   - Go to [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
   - Create a new application
   - Add `http://127.0.0.1:5000/callback` to the Redirect URIs
   - Copy the Client ID and Client Secret

3. Update the credentials:
   - Open `MainWindow.xaml.cs`
   - Replace `YOUR_CLIENT_ID` with your Spotify Client ID
   - Replace `YOUR_CLIENT_SECRET` with your Spotify Client Secret

4. Build and run:
```bash
dotnet build
dotnet run
```

## Usage

- The application will open a browser window for Spotify authentication
- After authentication, the controller will appear at the top of your screen
- Drag the window to reposition it
- Use the playback controls to control your Spotify playback

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. 