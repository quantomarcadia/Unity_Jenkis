FROM ubuntu:22.04

RUN apt-get update && apt-get install -y libglu1 libxcursor1 libxinerama1 && rm -rf /var/lib/apt/lists/*

COPY ./Builds/LinuxServer/ /app/

WORKDIR /app


RUN chmod +x ./MyServer.x86_64


ENTRYPOINT ["./MyServer.x86_64", "-batchmode", "-nographics"]