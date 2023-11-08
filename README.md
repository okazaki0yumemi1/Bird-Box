# Bird-Box

The following project is a WIP. 

The main goal of this app is to use it as a lightweight app to recognize bird species by their voices.

You can choose the microphone (input device) for recording, but expect a lot of bugs and messy stuff - I am really inexperienced software developer.

This ASP NET Core project consists of Web API + Swagger to control recording and neural model paramenetrs and to retreive data from BD via said web API. 

The current problems are:
- Lack of documentation
- Lack of tests with 2+ input devices
- General lack of good coding practices

How to use:

1. Download the latest release for Linux for ARM or x86_64
2. Unzip, change directory to Bird-Box, then go to https://github.com/kahst/BirdNET-Analyzer and follow the instructions to install:
   python3
   tensorflow or TFLite
   ffmpeg, resampy, librosa
3. Clone BirdNET Analyzer to Bird-Box.
4. Start the binary file via ./Bird-Box command.
5. Navigate to localhost:5001 - you should see list of connected audio devices. Click on the "Swagger Web UI" link on the left top corner to get to the web UI.