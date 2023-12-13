# Bird-Box

# About

This is a SQLite database and a Web API (and Swagger UI) in a small application. It should be used with BirdNET-Analyzer neural network (https://github.com/kahst/BirdNET-Analyzer) to listen & analyze birds on a local machine. 

For example, I have this app installed on an Orange Pi Zero 2 to analyze bird songs during spring, summer and autumn months.

All you need is a single-board PC (armv8) or pretty much any PC (x86_64) with Linux installed, a microphone and a reliable power source. 

# How to install

1. Download the latest release for Linux for ARM or x86_64, depends on your CPU architecture
2. Unzip, change directory to Bird-Box, then go to https://github.com/kahst/BirdNET-Analyzer and follow the instructions to install:
   python3
   tensorflow or TFLite
   ffmpeg, resampy, librosa
3. Clone BirdNET Analyzer to Bird-Box folder via 'git clone' command.
4. Start the binary file via ./Bird-Box command.
5. Navigate to localhost:5001 - you should see list of connected audio devices. Click on the "Swagger Web UI" link on the left top corner to get to the Swagger UI. 

To start the recording, you need to create POST-request with settings for BirdNET, duration (in hours) and an input device. The most important options are: confidence (can be between 0 & 1, but I use 0.7) and sensitivity (1 is standart, 1.5 is max), also longtitude and latitude (your location, basically).

To process results, make a GET-request to "localhost:5001/api/results/process". After that you can check the database for results.