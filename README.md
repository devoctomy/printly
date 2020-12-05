[![Build Status](https://travis-ci.com/devoctomy/printly.svg?branch=main)](https://travis-ci.com/devoctomy/printly)
[![codecov](https://codecov.io/gh/devoctomy/printly/branch/main/graph/badge.svg?token=1HHMS22045)](https://codecov.io/gh/devoctomy/printly)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/b20623c3dd714df698a87f4cd1020f5a)](https://www.codacy.com/gh/devoctomy/printly/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=devoctomy/printly&amp;utm_campaign=Badge_Grade)

# printly
Simple print-farm POC.

## Ubuntu on Raspberry Pi 4

Most of my testing has been done using a Raspberry Pi 4 and Ubuntu 20.04.1 Raspberry Pi image.  I found that occasionally the device would just stop booting, getting stuck on the rainbow screen, I understand only 2 things can cause this.

1. Dodgy SD card
2. Dodgy installation

In my circumstance I'm pretty sure that the SD card is fine, as reformatting it and flashing Ubuntu to it again would get everything back to scratch, so something was happening to my installation.  I believe this may be caused by Automatic Updates running on the Pi whilst I pull the plug on it. As I want to be able to pull the plug and have it recover every time, I have disabled Automatic Updates in Ubuntu via information on the following page,

[How to Handle Automatic Updates in Ubuntu](https://itsfoss.com/auto-updates-ubuntu/#:~:text=The%20reason%20is%20that%20Ubuntu,via%20the%20Software%20Updater%20tool.)

> DO THIS AT YOUR OWN RISK

## Installing Docker (Ubuntu)

Please refer to the following page from Docker,

[Official Docker instructions on installation of Docker](https://docs.docker.com/engine/install/ubuntu/)

> Please note that when adding the repository, there are several to choose from, you want "arm64" for the Raspberry Pi 4 and az 64 bit os.

At the time of writing, my installation went as follows,

```bash
sudo apt-get update
sudo apt-get install apt-transport-https ca-certificates curl gnupg-agent software-properties-common
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
sudo apt-key fingerprint 0EBFCD88
sudo add-apt-repository "deb [arch=arm64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
sudo apt-get update
sudo apt-get install docker-ce docker-ce-cli containerd.io
```

To verify the installation

```bash
docker --version
```

> Docker version 19.03.14, build 5eb3275

The following command allows docker to be run as non-administrator,

```bash
sudo groupadd docker
sudo usermod -aG docker $USER
```

> DO THIS AT YOUR OWN RISK
> Please note, you may need to restart your Pi for user groups to be applied.

## Installing Docker Compose

Please refer to the following page from Docker,

[Official Docker instructions on installation of Docker Compose](https://docs.docker.com/compose/install/)

I found that I was able to install this using apt-get,

```bash
sudo apt-get install docker-compose
```

You may also need to create an external network using the following command (required for docker-compose deployment),

```bash
docker network create -d bridge my-network
```

## dotnet 5 SDK (Installation on Raspberry Pi 4 Arm64)

Please refer to the following page from Microsoft,

[Official Microsoft instructions on manual instalation of dotnet 5](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-5.0.100-linux-arm64-binaries)

You may find that upon rebooting, the system forgets where dotnet is, you can resolve this by adding the path upon system startup by adding the following lines to the bottom of '~/.bashrc',

```bash
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet
```

## Deploying via Docker Compose

Once you have the docker engine, docker compose, and dotnet 5 successfull installed (verify using the following commands, each command should return their respective version number, dotnet should be >= 5)

```bash
docker --version
docker-compose --version
dotnet --version
```

Clone the printly source code,

```bash
cd ~/
git clone https://github.com/devoctomy/printly
```

Build and deploy printly

```bash
cd ~/printly/Docker
docker-compose up
```

Printly requires access to the host systems serial ports, and as it is being run in a container, you will need to add additional priviledges for it to be able to access them, please refer to the following article,

[How to access serial devices in docker](https://www.losant.com/blog/how-to-access-serial-devices-in-docker)

At the time of writing, I done the following,

```bash
sudo nano /etc/udev/rules.d/99-serial.rules
```

And added following line

```bash
KERNEL=="ttyUSB[0-9]*",MODE="0666"
```

The printly service has already been configured for everything else.  To verify that the service has access to the serial ports, after running 'docker-compose up', access the System Info API in a browser,

```bash
http://{IP Address of host}:5000/api/System/Info
```

You should see something like,

```javascript
{
    "success": true,
    "value": {
        "systemId": "2a136ad2-f1b9-4df3-8e02-74b69641a134",
        "startedAt": "2020-12-05T10:02:13.7967741Z",
        "serialPorts": ["/dev/ttyS0", "/dev/ttyAMA0"]
    },
    "error": null
}
```

As you can see, 2 serial ports have been discovered.

### Rebuilding / Deploying via Docker Compose

At some point you may want to remove everything from docker, rebuild and redeploy, you can do this via the following command

```bash
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker system prune -a
```

> Please note, after doing so you will need to recreate the network.

## Debugging Printly

To debug just Printly service within Visual Studio, you will need to have a running instance of Mongo.
this can be done using docker with the following command,

```powershell
docker run -p 27017:27017 mongo:latest
```

This will make your connection string as follows,

```javascript
"PRINTLY_MongoDbStorageConnectionString": "mongodb://localhost:27017"
```

## API Reference

### System

**GET /api/System/Info**

Gets information about the system, including version, uptime and available serial ports.

## Websockets Terminal

**WS /terminal/{PortName}?baudrate={BaudRate}&parity={Parity}&databits={DataBits}&stopbits={StopBits}&handshake={HandShake}**

-  BaudRate (optional) Baudrate to use for the serial port connection, typically 1 of (300,600,1200,2400,4800,9600,14400,19200,38400,57600,115200,230400,460800)
-  Party (optional) Party to use for the serial port connection, typically 1 of (none,odd,even,mark,space)
-  DataBits (optional) Data bits to use for the serial port connection, typically 1 of (5,6,7,8,9)
-  StopBits (optional) Stop bits to use for the serial port connection, typically 1 of (none,one,onepointfive,two)
-  Handshake (optional) Handshake use for the serial port connection, typically 1 of (none,requesttosend,requesttosendxonxoff,xonxoff)

### Description

Using a web sockets client, this interface can be used to establish direct serial port connection with devices connected to your Printly system.

https://chrome.google.com/webstore/detail/browser-websocket-client/mdmlhchldhfnfnkfmljgeinlffmdgkjo