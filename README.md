# Bird-Box

The following project is a WIP. 

The main goal of this app is to use it as a lightweight app to recognize bird species by voice.

Right now this application as a way to capture audio via USB microphone and pass it to the BirdNET Analyzer, and then to identify neural model output and store it in SQLite.

This ASP NET Core project consists of Web API + Swagger to control recording and neural model paramenetrs and to retreive data from BD via said web API. 

The current problems are:
- User is unable to select microphone, only first USB micro is being used.
- The BirdNET output processing is slow and should be changed somehow.

How to use:

1. Download the latest release for Linux for ARM or x86_64
2. Unzip, change directory to Bird-Box, then go to https://github.com/kahst/BirdNET-Analyzer and follow the instructions to install:
   python3
   tensorflow or TFLite
   ffmpeg, resampy, librosa
3. Clone BirdNET Analyzer to Bird-Box.
4. Start the binary file via ./Bird-Box command.
5. Navigate to localhost:5001 - you should see Swagger web page with endpoints.
