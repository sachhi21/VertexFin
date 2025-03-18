using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;

namespace VertexFin.Common.Library.Helper
{
   

    public static class GuidV7
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static Guid NewGuid()
        {
            // Get current timestamp in milliseconds since Unix epoch
            long unixTimeMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Convert timestamp to bytes (big-endian)
            byte[] timestampBytes = BitConverter.GetBytes(unixTimeMillis);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes); // Convert to big-endian
            }

            // Prepare 16-byte array for GUID
            byte[] guidBytes = new byte[16];

            // Fill first 6 bytes with timestamp
            Array.Copy(timestampBytes, 2, guidBytes, 0, 6); // Take last 6 bytes (big-endian)

            // Generate 10 random bytes
            byte[] randomBytes = new byte[10];
            Rng.GetBytes(randomBytes);

            // Fill remaining bytes with randomness
            Array.Copy(randomBytes, 0, guidBytes, 6, 10);

            // Set UUID version (v7 = 0b0111xxxx)
            guidBytes[6] &= 0x0F;
            guidBytes[6] |= 0x70; // Set version 7

            // Set UUID variant (RFC 4122: 0b10xxxxxx)
            guidBytes[8] &= 0x3F;
            guidBytes[8] |= 0x80;

            return new Guid(guidBytes);
        }
    }

   

}
