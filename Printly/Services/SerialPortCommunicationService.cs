using System;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortCommunicationService : ISerialPortCommunicationService
    {
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;
        public event EventHandler<SerialErrorReceivedEventArgs> ErrorReceived;

        private readonly ISerialPortFactory _serialPortFactory;
        private ISerialPort _serialPort;
        private string _portName = string.Empty;

        public string PortName => _portName;

        public object State { get; set; }

        public SerialPortCommunicationService(ISerialPortFactory serialPortFactory)
        {
            _serialPortFactory = serialPortFactory;
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
            _serialPort = _serialPortFactory.Create(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.ErrorReceived += SerialPort_ErrorReceived;
            _serialPort.Open();
            _portName = portName;
            return _serialPort.IsOpen;
        }

        public void Close()
        {
            if(_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        private void SerialPort_DataReceived(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            DataReceived?.Invoke(
                this,
                e);
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ErrorReceived?.Invoke(
                this,
                e);
        }

        public void Write(
            byte[] buffer,
            int offset,
            int count)
        {
            _serialPort.Write(
                buffer,
                offset,
                count);
        }
    }
}
