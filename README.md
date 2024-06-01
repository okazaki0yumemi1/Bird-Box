# Bird-Box

# About

This is a SQLite database and a Web API (and Swagger UI) in a small application. It should be used with BirdNET-Analyzer neural network (https://github.com/kahst/BirdNET-Analyzer) to listen & analyze birds on a local machine. 

For example, I have this app installed on an Orange Pi Zero 2 to analyze bird songs during spring, summer and autumn months.

All you need is a single-board PC (ARM64 or ARM) or pretty much any PC (x86_64) with Linux installed, a microphone and a reliable power source. 

# How to install

1. Download the latest release for Linux for ARM, ARM64 or x86_64, depends on your CPU architecture
2. Unzip, change directory to Bird-Box, then go to ``` https://github.com/kahst/BirdNET-Analyzer ``` and follow the instructions to install:
3. Install following packages:
   ```
   python3
   tensorflow or TFLite
   ffmpeg, resampy, librosa
   ```
5. Clone BirdNET Analyzer to Bird-Box folder via 'git clone' command.
6. Start the binary file via ./Bird-Box command.
7. Navigate to ``` localhost:5001 ``` - you should see list of connected audio devices. Click on the "Swagger Web UI" link on the left top corner to get to the Swagger UI. 

To start the recording, you need to create POST-request with settings for BirdNET, duration (in hours) and an input device. The most important options are: confidence (can be between 0 & 1, but I use 0.7) and sensitivity (1 is standart, 1.5 is max), also longtitude and latitude (your location, basically).

To process results, make a GET-request to ``` localhost:5001/api/results/process ```. After that you can check the database for results.

# How to use as a systemd service

1. Install Bird-Box, check if it works. Let's say, you have it installed in /home/your-username/Bird-Box, and the executable file (Bird-Box) is located in /home/your-username/Bird-Box/linux-arm64/Bird-Box
2. Ensure you have super user privileges. Then do the following: create a file called ```bird-box.service``` in ``` /etc/systemd/system``` and write the following inside:
   ```
   [Unit]
   Description=Bird-Box Service
   [Service]
   Type=simple
   WorkingDirectory=/home/your-username/Bird-Box/linux-arm64/
   ExecStart=/home/your-username/Bird-Box/linux-arm64/Bird-Box
   Restart=on-failure
   RestartSec=5
   [Install]
   WantedBy=multi-user.target
   ```
3. Try running ```sudo systemctl status bird-box```. You should be able to see service information, but the service itself will be disabled and not running.
4. If previous step is fine, i.e. you see a service info, then run
   ```
   sudo systemctl enable bird-box
   sudo systemctl start bird-box
   ```
5. Navigate to your server's IP address (you can see it by running ```hostname -I```) and port 5001, i.e. 192.168.88.200:5001, if your server ip is 192.168.88.200.

# Can I try it?
Sure. Go to birds.containercat.com (birds.containercat.com/api/swagger/index.html for Swagger Web UI) and try following endpoints:
   /api/results
   You can also check these to get results for specific day or to get a specific type of bird:
   /api/results/days/
   /api/results/birds/
