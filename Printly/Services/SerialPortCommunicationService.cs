using Microsoft.Extensions.Logging;
using System;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortCommunicationService : ISerialPortCommunicationService
    {
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;
        public event EventHandler<SerialErrorReceivedEventArgs> ErrorReceived;

        private readonly ISerialPortFactory _serialPortFactory;
        private readonly ILogger<SerialPortCommunicationService> _logger;
        private string _portName = string.Empty;

        public ISerialPort SerialPort { get; private set; }
        public string PortName => _portName;
        public object State { get; set; }

        public SerialPortCommunicationService(
            ISerialPortFactory serialPortFactory,
            ILogger<SerialPortCommunicationService> logger)
        {
            _serialPortFactory = serialPortFactory;
            _logger = logger;
        }

        public bool Open(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout)
        {
            _logger.LogInformation($"Opening serial port '{portName}', using {baudRate} {dataBits} {parity} {stopBits}.");
            SerialPort = _serialPortFactory.Create(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);
            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialPort.ErrorReceived += SerialPort_ErrorReceived;
            SerialPort.Open();
            _portName = portName;
            return SerialPort.IsOpen;
        }

        public void Close()
        {
            if(SerialPort != null && SerialPort.IsOpen)
            {
                _logger.LogInformation($"Closing serial port '{SerialPort.PortName}'.");
                SerialPort.Close();
            }
        }

        private void SerialPort_DataReceived(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            _logger.LogDebug($"Received data on serial port '{SerialPort.PortName}'.");
            DataReceived?.Invoke(
                this,
                e);
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            _logger.LogDebug($"Received error on serial port '{SerialPort.PortName}'.");
            ErrorReceived?.Invoke(
                this,
                e);
        }

        public void Write(
            byte[] buffer,
            int offset,
            int count)
        {
            _logger.LogInformation($"Writing {count} bytes serial port '{SerialPort.PortName}'.");
            SerialPort.Write(
                buffer,
                offset,
                count);
        }
    }
}
