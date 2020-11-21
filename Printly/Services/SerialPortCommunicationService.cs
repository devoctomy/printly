using System;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortCommunicationService : ISerialPortCommunicationService
    {
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        private SerialPort _serialPort;

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
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = stopBits;
            _serialPort.Handshake = handshake;
            _serialPort.ReadTimeout = (int)readTimeout.TotalMilliseconds;
            _serialPort.WriteTimeout = (int)writeTimeout.TotalMilliseconds;
            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.Open();
            return _serialPort.IsOpen;
        }

        public void Close()
        {
            if(_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        private void _serialPort_DataReceived(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            DataReceived?.Invoke(
                _serialPort,
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
