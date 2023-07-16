FROM ubuntu:22.04 AS base
RUN apt-get update -y && apt-get upgrade -y
RUN apt-get install -y python3-dev python3-pip
RUN pip3 install --upgrade pip
RUN pip3 install tflite-runtime
RUN pip3 install librosa resampy
RUN apt-get install -y --no-install-recommends ffmpeg
RUN apt-get install -y alsa-utils libsndfile1-dev pulseaudio alsa-base
RUN apt-get install -y dotnet-sdk-7.0
RUN pulseaudio --start

FROM base AS build-env
RUN mkdir app
WORKDIR /app
RUN mkdir Data
RUN mkdir Recordings
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
#RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
#USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bird-Box.csproj", "./"]
RUN dotnet restore "Bird-Box.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Bird-Box.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bird-Box.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "Bird-Box.dll"]
