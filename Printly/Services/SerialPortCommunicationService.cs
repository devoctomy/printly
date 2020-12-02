using System;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortCommunicationService : ISerialPortCommunicationService
    {
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;
        public event EventHandler<SerialErrorReceivedEventArgs> ErrorReceived;

        private readonly ISerialPortFactory _serialPortFactory;
        private string _portName = string.Empty;

        public ISerialPort SerialPort { get; private set; }
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
                SerialPort.Close();
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
            SerialPort.Write(
                buffer,
                offset,
                count);
        }
    }
}
