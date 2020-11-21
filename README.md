# printly
Simple print-farm POC.

## API Reference

### System

GET /api/System/Info

#### Description

Gets information about the system, including version, uptime and available serial ports.

## Websockets Terminal

WS /terminal/{PortName}?baudrate={BaudRate}&parity={Parity}&databits={DataBits}&stopbits={StopBits}&handshake={HandShake}

BaudRate (optional) = Baudrate to use for the serial port connection, typically 1 of (300,600,1200,2400,4800,9600,14400,19200,38400,57600,115200,230400,460800)
Party (optional) = Party to use for the serial port connection, typically 1 of (none,odd,even,mark,space)
DataBits (optional) = Data bits to use for the serial port connection, typically 1 of (5,6,7,8,9)
StopBits (optional) = Stop bits to use for the serial port connection, typically 1 of (none,one,onepointfive,two)
Handshake (optional) = Handshake use for the serial port connection, typically 1 of (none,requesttosend,requesttosendxonxoff,xonxoff)

#### Description

Using a web sockets client, this interface can be used to establish direct serial port connection with devices connected to your Printly system.

https://chrome.google.com/webstore/detail/browser-websocket-client/mdmlhchldhfnfnkfmljgeinlffmdgkjo