using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace AM2315package.AM2315
{
    public class AM2315sensor
    {
        private Int32 _addr;
        private I2cDevice _dev;
        public float humidity;
        public float temperature;
        public string message;

        public AM2315sensor(Int32 addr)
        {
            _addr = addr;
        }
        public async Task<int> Connect()
        {
            string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            I2cConnectionSettings devSettings = new I2cConnectionSettings(_addr);
            devSettings.BusSpeed = I2cBusSpeed.FastMode;
            devSettings.SharingMode = I2cSharingMode.Shared;
            _dev = await I2cDevice.FromIdAsync(devices[0].Id, devSettings);
            int ret = _dev != null ? 1 : -1;

            return ret;
        }

        public async Task<float[]> ReadData()
        {
            byte[] buffer = new byte[1];
            byte[] readbuffer = new byte[8];
            float[] returnVals = new float[2];
            try
            {
                buffer[0] = 0x03;
                _dev.Write(buffer);
                buffer[0] = 0;
                _dev.Write(buffer);
                buffer[0] = 4;
                _dev.Write(buffer);
                await Task.Delay(10);
                _dev.Read(readbuffer);
                if (readbuffer[0]!=0x03 || readbuffer[1]!=4)
                {
                    throw new Exception("error in reply")
                };
                humidity = readbuffer[2];
                humidity *= 256;
                humidity += readbuffer[3];
                humidity /= 10;
                temperature = readbuffer[4] & 0x7F;
                temperature *= 256;
                temperature += readbuffer[5];
                temperature /= 10;
                if ((readbuffer[4] >> 7)>0)
                {
                    temperature = -temperature;
                }
                returnVals[0] = humidity;
                returnVals[1] = temperature;
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return returnVals;
        }
    }
}
