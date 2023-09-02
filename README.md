# Bird-Box

The following project is a WIP. 

The main goal of this app is to use it as a lightweight app to recognize bird species by voice.

Right now this application as a way to capture audio via USB microphone and pass it to the BirdNET Analyzer, and then to identify neural model output and store it in SQLite.

This ASP NET Core project consists of Web API + Swagger to control recording and neural model paramenetrs and to retreive data from BD via said web API. 

The current problems are:
- User is unable to select microphone, only first USB micro is being used.
- The BirdNET output porocessing is extremely slow and should be completely changed.
