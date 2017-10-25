using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AntiInfoScraper
{
    public class SecureRandom : RandomNumberGenerator
    {
        private readonly RandomNumberGenerator rng;

        public SecureRandom()
        {
            this.rng = new RNGCryptoServiceProvider();
        }
        public int Next()
        {
            var data = new byte[sizeof(int)];
            rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
        }
        public string String(int len, string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            StringBuilder sb = new StringBuilder();
            string letter = "";
            while (sb.Length != len)
            {
                while (letter == "" || !charset.Contains(letter))
                {
                    if (sb.Length == len)
                        return sb.ToString();

                    byte[] oneByte = new byte[1];
                    rng.GetBytes(oneByte);
                    char c = (char)oneByte[0];
                    if (char.IsDigit(c) || char.IsLetter(c))
                        letter = c.ToString();
                }
                sb.Append(letter[0]);
                letter = "";
            }
            return sb.ToString();
        }
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return (int)Math.Floor((minValue + ((double)maxValue - minValue) * NextDouble()));
        }

        public double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            rng.GetBytes(data);
            var randUint = BitConverter.ToUInt32(data, 0);
            return randUint / (uint.MaxValue + 1.0);
        }
        public double NextDouble(double minimum, double maximum)
        {
            return NextDouble() * (maximum - minimum) + minimum;
        }
        public override void GetBytes(byte[] data)
        {
            rng.GetBytes(data);
        }
        public override void GetNonZeroBytes(byte[] data)
        {
            rng.GetNonZeroBytes(data);
        }
    }
}
