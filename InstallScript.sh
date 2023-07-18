#! /bin/sh
echo "Creating directory for SQLite database"
mkdir ./Data || echo "Unable to create directory for database, please create folder "Data" in Bird-Box folder"
mkdir ./Recordings || echo "Unable to create directory for recording results, please create folder "Recordings" in Bird-Box folder"
echo "You also have to download BirdNET-Analyzer model and put it in the same folder as ./Bird-Box file (same as "Data" and "Recording" folders)"
