# FFMPEG Record iPhone Screen Using `idevicescreenshot`s

[**WEB**](https://tomashubelbauer.github.io/ffmpeg-iphone-screen-record)

This repository contains a .NET script which invokes the `idevicescreenshot`
utility from `libimobiledevice` taking screenshots of a connected iPhone
screen (~3 seconds each) and pipes the screenshots to FFMPEG where they
get stiched to an MP4 video playable on an iPhone.

Run using `dotnet script main.csx`

## Why not just use the screen recorder on the iPhone?

You should, I did this because it is IMO a neat hack and that's all.
Since the frame rate of this solution is so poor, it's not an alternative.

One thing in it's favor though is that it doesn't have the red bar on top
when recording since it just stitches screenshots together. :-)

## Dependencies

- .NET Core
- `dotnet-script` global tool
- SIPS (PNG to JPG conversion)
- `libimobiledevice` (for `idevicescreenshot`)

## To-Do

### Use the .NET bindings for ilibmobiledevice to make this work on Windows
