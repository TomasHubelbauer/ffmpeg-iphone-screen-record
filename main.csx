// Run using .NET Core:
// - Install libimobiledevice using Homebrew
// - Connect the iPhone to the Mac using USB and trust the laptop on the phone
// - Install the .NET SDK
// - Run `dotnet tool install -g dotnet-script`
// - Run `dotnet script main.csx` (on macOS)

using System;
using System.IO;
using System.Threading;

var frameCount = 5 * 24;

var ffmpegProcessStartInfo = new ProcessStartInfo();
ffmpegProcessStartInfo.FileName = "ffmpeg";
ffmpegProcessStartInfo.Arguments = "-y -f image2pipe -i - -profile:v baseline -level 3.0 out.mp4";

// Start ffmpeg in a separate process
ffmpegProcessStartInfo.UseShellExecute = false;
ffmpegProcessStartInfo.RedirectStandardInput = true;

var ffmpegProcess = Process.Start(ffmpegProcessStartInfo);

using (var memoryStream = new MemoryStream())
{
    for (var frame = 0; frame < frameCount; frame++)
    {
        // Capture the screenshot
        var idevicescreenshotProcessStartInfo = new ProcessStartInfo();
        idevicescreenshotProcessStartInfo.FileName = "idevicescreenshot";
        idevicescreenshotProcessStartInfo.Arguments = "frame.png";
        var idevicescreenshotProcess = Process.Start(idevicescreenshotProcessStartInfo);
        idevicescreenshotProcess.WaitForExit();

        // TODO: Detect frame.png not having been created (file missing / stdout of idevicescreenshot?)

        // Convert PNG to JPG for ffmpeg
        var sipsProcessStartInfo = new ProcessStartInfo();
        sipsProcessStartInfo.FileName = "sips";
        sipsProcessStartInfo.Arguments = "-s format jpeg frame.png --out frame.jpg";
        var sipsProcess = Process.Start(sipsProcessStartInfo);
        sipsProcess.WaitForExit();

        // TODO: Consider sending these to a parallel queue so this doesn't slow down screenshoting

        // Send the JPG frame bytes to FFMPEG
        var bytes = await File.ReadAllBytesAsync("frame.jpg");
        await ffmpegProcess.StandardInput.BaseStream.WriteAsync(bytes, 0, bytes.Length);
        await ffmpegProcess.StandardInput.FlushAsync();
    }
}

ffmpegProcess.StandardInput.Close();
ffmpegProcess.WaitForExit();

var ffprobeProcessStartInfo = new ProcessStartInfo();
ffprobeProcessStartInfo.FileName = "ffprobe";
ffprobeProcessStartInfo.Arguments = "-v error -select_streams v:0 -show_entries stream=nb_frames -of default=nokey=1:noprint_wrappers=1 out.mp4";

// Allow us to read the output so we can check the frame count
ffprobeProcessStartInfo.RedirectStandardOutput = true;

var ffprobeProcess = Process.Start(ffprobeProcessStartInfo);
ffprobeProcess.WaitForExit();

var frameCountCheck = await ffprobeProcess.StandardOutput.ReadToEndAsync();
if (int.Parse(frameCountCheck) != frameCount)
{
    throw new Exception($"The final frame count does not match! Actual: {frameCountCheck}, expected: {frameCount}");
} else {
    Console.WriteLine("Done! See out.mp4");
}
