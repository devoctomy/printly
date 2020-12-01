[![Build Status](https://travis-ci.org/devoctomy/printly.svg?branch=main)](https://travis-ci.org/devoctomy/printly)
[![codecov](https://codecov.io/gh/devoctomy/printly/branch/main/graph/badge.svg?token=1HHMS22045)](https://codecov.io/gh/devoctomy/printly)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/b20623c3dd714df698a87f4cd1020f5a)](https://www.codacy.com/gh/devoctomy/printly/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=devoctomy/printly&amp;utm_campaign=Badge_Grade)

# printly
Simple print-farm POC.

## dotnet 5 SDK (Installation on Raspberry Pi 4 Arm64)

https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-5.0.100-linux-arm64-binaries

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

-  BaudRate (optional) = Baudrate to use for the serial port connection, typically 1 of (300,600,1200,2400,4800,9600,14400,19200,38400,57600,115200,230400,460800)
-  Party (optional) = Party to use for the serial port connection, typically 1 of (none,odd,even,mark,space)
-  DataBits (optional) = Data bits to use for the serial port connection, typically 1 of (5,6,7,8,9)
-  StopBits (optional) = Stop bits to use for the serial port connection, typically 1 of (none,one,onepointfive,two)
-  Handshake (optional) = Handshake use for the serial port connection, typically 1 of (none,requesttosend,requesttosendxonxoff,xonxoff)

### Description

Using a web sockets client, this interface can be used to establish direct serial port connection with devices connected to your Printly system.

https://chrome.google.com/webstore/detail/browser-websocket-client/mdmlhchldhfnfnkfmljgeinlffmdgkjo