using Microsoft.AspNetCore.Http;
using Printly.Exceptions;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortConnectionManager : ISerialPortConnectionManager
    {
        private readonly ISerialPortCommunicationServiceFactory _serialPortCommunicationServiceFactory;
        private readonly Dictionary<string, ISerialPortCommunicationService> _connectionCache;

        public SerialPortConnectionManager(ISerialPortCommunicationServiceFactory serialPortCommunicationServiceFactory)
        {
            _serialPortCommunicationServiceFactory = serialPortCommunicationServiceFactory;
            _connectionCache = new Dictionary<string, ISerialPortCommunicationService>();
        }

        private ISerialPortCommunicationService Get(string portName)
        {
            if(_connectionCache.ContainsKey(portName))
            {
                return _connectionCache[portName];
            }
            else
            {
                return null;
            }
        }

        public ISerialPortCommunicationService GetOrOpen(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout)
        {
            var connection = Get(portName);
            if(connection == null)
            {
                var newConnection = _serialPortCommunicationServiceFactory.Create();
                var isOpen = newConnection.Open(
                    portName,
                    baudRate,
                    parity,
                    dataBits,
                    stopBits,
                    handshake,
                    readTimeout,
                    writeTimeout);
                if(!isOpen)
                {
                    throw new SerialPortConnectionException("Failed to open connection to serial port.");
                }
                connection = newConnection;
                _connectionCache.Add(
                    portName,
                    connection);
            }

            return connection;
        }

        public ISerialPortCommunicationService GetOrOpen(HttpRequest httpRequest)
        {
            var pathParts = httpRequest.Path.ToString()[1..].Split("/");
            var portName = pathParts[1];

            return GetOrOpen(
                portName,
                int.Parse(GetQueryValueOrDefault(httpRequest.Query, "baudrate", "9600")),
                Enum.Parse<Parity>(GetQueryValueOrDefault(httpRequest.Query, "parity", Parity.None.ToString()), true),
                int.Parse(GetQueryValueOrDefault(httpRequest.Query, "databits", "8")),
                Enum.Parse<StopBits>(GetQueryValueOrDefault(httpRequest.Query, "stopbits", StopBits.One.ToString()), true),
                Enum.Parse<Handshake>(GetQueryValueOrDefault(httpRequest.Query, "handshake", Handshake.None.ToString()), true),
                new TimeSpan(1, 0, 0),
                new TimeSpan(1, 0, 0));
        }

        public bool Close(string portName)
        {
            if (_connectionCache.ContainsKey(portName))
            {
                _connectionCache[portName].Close();
                _connectionCache.Remove(portName);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string GetQueryValueOrDefault(
            IQueryCollection queryCollection,
            string name,
            string defaultValue)
        {
            if (queryCollection.TryGetValue(name, out var values))
            {
                return values[0];
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
