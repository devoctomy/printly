﻿using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortConnectionManager : ISerialPortConnectionManager
    {
        private readonly Dictionary<string, ISerialPortCommunicationService> _connectionCache;

        public SerialPortConnectionManager()
        {
            _connectionCache = new Dictionary<string, ISerialPortCommunicationService>();
        }

        public ISerialPortCommunicationService Get(string portName)
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
                var newConnection = new SerialPortCommunicationService();
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
                    throw new Exception("Failed to open connection");
                }
                connection = newConnection;
                _connectionCache.Add(
                    portName,
                    connection);
            }

            return connection;
        }

        public void Close(string portName)
        {
            if (_connectionCache.ContainsKey(portName))
            {
                _connectionCache[portName].Close();
                _connectionCache.Remove(portName);
            }
        }
    }
}