#! /bin/sh

echo "Installing python3-dev and python3-pip..."
apt-get install -y python3-dev python3-pip  || echo "Python3 install failed, aborting installation" && exit
pip3 install --upgrade pip && echo "Success!" || echo "pip3 upgrade command failed, continuing"
echo "Installing Python packages..."
pip3 install tflite-runtime && echo "Success!" || echo "TFLite install failed inside pip3 environment, installing tensorflow..." && pip3 install tensorflow  && echo "Tensorflow install was successful" || echo "Tensorflow install failed, aborting installation" && exit
pip3 install librosa resampy || echo "librosa and resampy install failed inside pip3 environment"

echo "Installing ffmpeg..."
apt-get install -y ffmpeg && echo "Success!" || echo "ffmpeg install failed"
echo "Installing audio capture packages: alsa and pulseaudio..."
apt-get install -y alsa-utils pulseaudio alsa-base && echo "Success!" || echo "Audio packages install failed" 
apt-get install -y dotnet-sdk-7.0 && echo "Dotnet sdk install was successfull" || echo "dotnet sdk 7.0 failed, aborting installation" && exit
echo "Creating directory for SQLite database"
mkdir ./Data || echo "Unable to create directory for database, please create folder "Data" in Bird-Box folder"
mkdir ./Recordings || echo "Unable to create directory for recording results, please create folder "Recordings" in Bird-Box folder"