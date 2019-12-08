# FFMPEG Record iPhone Screen Using `idevicescreenshot`s

This repository contains a .NET script which invokes the `idevicescreenshot`
utility from `libimobiledevice` taking screenshots of a connected iPhone
screen (~3 seconds each) and pipes the screenshots to FFMPEG where they
get stiched to an MP4 video playable on an iPhone.

Run using `dotnet script main.csx`

## Dependencies

- .NET Core
- `dotnet-script` global tool
- SIPS (PNG to JPG conversion)
- `libimobiledevice` (for `idevicescreenshot`)

## To-Do

### Use the .NET bindings for ilibmobiledevice to make this work on Windows
