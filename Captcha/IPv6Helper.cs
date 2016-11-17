
namespace Captcha
{
    // http://www.codeproject.com/Tips/784635/UInt-Bit-Operations
    // https://github.com/somdoron/NaCl.net/blob/master/nacl.net/UInt128.cs


    class IPv6Helper
    {


        public static void Test()
        {
            // 2607:f0d0:1002:51::4
            // 2607:f0d0:1002:0051:0000:0000:----:0004
            // 2607:f0d0:1002:0051:0000:0000:0000:0004

            // 2001:200:180:8000::
            // 2001:200:180:8000:0:0:0:0
            CIDR2IpRange("2001:200:180:8000::/49");
            ulong[] longs = IPToLong("2607:f0d0:1002:0051:0000:0000:0000:0004");


            string cidr = IPrange2CIDR("2001:0200:0180:8000:0000:0000:0000:0000", "2001:0200:0180:ffff:ffff:ffff:ffff:ffff");
            System.Console.WriteLine(cidr);

            System.Console.WriteLine(longs);
            string origPi = longToIP(longs);
            System.Console.WriteLine(origPi);
            System.Console.WriteLine(longs);
        }


        // https://social.msdn.microsoft.com/Forums/en-US/9509ecd0-d908-4f56-aca2-7703bdc53fdb/ipv6-address-compress-expand?forum=netfxnetcom
        
        public static string ExpandIP(string address)
        {
            System.Net.IPAddress addr = System.Net.IPAddress.Parse(address);
            
            if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                byte[] bytes = addr.GetAddressBytes();

                System.Diagnostics.Debug.Assert(bytes.Length == 16, "bytes.Length == 16");
                System.Text.StringBuilder bldr = new System.Text.StringBuilder();
                for (int i = 0; i < 16; i += 2)
                {
                    bldr.AppendFormat("{0:x2}{1:x2}:", bytes[i], bytes[i + 1]);
                }
                bldr.Length = bldr.Length - 1;//last colon
                string uncompressed = bldr.ToString();
                bldr.Length = 0;
                bldr = null;

                return uncompressed;
            }

            throw new System.ArgumentException("Not an IPv6-address");
        }


        // http://snipplr.com/view/84723/
        public static void lala()
        {
            string strIP = "2404:6800:4001:805::1006";
            System.Net.IPAddress address;
            // System.Numerics.BigInteger ipnum;

            if (System.Net.IPAddress.TryParse(strIP, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();

                if (System.BitConverter.IsLittleEndian)
                {
                    System.Collections.Generic.List<byte> byteList = new System.Collections.Generic.List<byte>(addrBytes);
                    byteList.Reverse();
                    addrBytes = byteList.ToArray();
                }

                ulong ip1 = System.BitConverter.ToUInt64(addrBytes, 8);
                ulong ip2 = System.BitConverter.ToUInt64(addrBytes, 0);
                /*
                if (addrBytes.Length > 8)
                {
                    //IPv6
                    ipnum = System.BitConverter.ToUInt64(addrBytes, 8);
                    ipnum <<= 64;
                    ipnum += System.BitConverter.ToUInt64(addrBytes, 0);
                }
                else
                {
                    //IPv4
                    ipnum = System.BitConverter.ToUInt32(addrBytes, 0);
                }
                */
            }
        }


        // https://www.digitalocean.com/community/tutorials/understanding-ip-addresses-subnets-and-cidr-notation-for-networking
        // https://www.ultratools.com/tools/ipv6CIDRToRangeResult?ipAddress=2001%3A200%3A180%3A8000%3A%3A%2F49
        public static void CIDR2IpRange(string IP)
        {
            
            string[] parts = IP.Split('/');
            /*
            uint ipnum = (System.Convert.ToUInt32(parts[0]) << 24) |
                (System.Convert.ToUInt32(parts[1]) << 16) |
                (System.Convert.ToUInt32(parts[2]) << 8) |
                System.Convert.ToUInt32(parts[3]);
            */
            string addr = parts[0];
            addr = ExpandIP(addr);
            int maskbits = System.Convert.ToInt32(parts[1]);
            System.Console.WriteLine(addr);
            System.Console.WriteLine(maskbits);



            ulong[] longs = IPToLong(addr);
            UInt128 ipnum = new UInt128(longs[1], longs[0]);

            //uint mask = uint.MaxValue;
            UInt128 mask = UInt128.MaxValue;
            mask <<= (128 - maskbits);

            
            UInt128 ipstart = ipnum & mask;
            UInt128 ipend = ipnum | (mask ^ UInt128.MaxValue);

            string fromRange = longToIP(new ulong[] { ipstart.Low, ipstart.High });
            string toRange = longToIP(new ulong[] { ipend.Low, ipend.High });
            System.Console.WriteLine(fromRange);
            System.Console.WriteLine(toRange);

            System.Console.WriteLine(fromRange + " - " + toRange);
        }


        public static string IPrange2CIDR(string ip1, string ip2)
        {
            ulong[] longs = IPToLong(ip1);
            UInt128 startAddr = new UInt128(longs[1], longs[0]);
            longs = IPToLong(ip2);
            UInt128 endAddr = new UInt128(longs[1], longs[0]);


            // uint startAddr = 0xc0a80001; // 192.168.0.1
            // uint endAddr = 0xc0a800fe;   // 192.168.0.254
            // uint startAddr = System.BitConverter.ToUInt32(System.Net.IPAddress.Parse(ip1).GetAddressBytes(), 0);
            // uint endAddr = System.BitConverter.ToUInt32(System.Net.IPAddress.Parse(ip2).GetAddressBytes(), 0);

            if (startAddr > endAddr)
            {
                UInt128 temp = startAddr;
                startAddr = endAddr;
                endAddr = temp;
            }

            // uint diff = endAddr - startAddr -1;
            // int bits =  32 - (int)System.Math.Ceiling(System.Math.Log10(diff) / System.Math.Log10(2));
            // return ip1 + "/" + bits;

            UInt128 diffs = startAddr ^ endAddr;

            // Now count the number of consecutive zero bits starting at the most significant
            int bits = 128;
            // int mask = 0;

            // We keep shifting diffs right until it's zero (i.e. we've shifted all the non-zero bits off)
            while (diffs != 0)
            {
                diffs >>= 1;
                bits--; // Every time we shift, that's one fewer consecutive zero bits in the prefix
                // Accumulate a mask which will have zeros in the consecutive zeros of the prefix and ones elsewhere
                // mask = (mask << 1) | 1;
            }
            
            return (ip1 + "/" + bits);
        }


        public static ulong[] IPToLong(string addr)
        {
            string[] addrArray = addr.Split(':');//a IPv6 adress is of form 2607:f0d0:1002:0051:0000:0000:0000:0004
            ulong[] num = new ulong[addrArray.Length];

            for (int i = 0; i < addrArray.Length; i++)
            {
                // num[i] = Long.parseLong(addrArray[i], 16);
                num[i] = ulong.Parse(addrArray[i], System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
            }

            ulong long1 = num[0];
            for (int i = 1; i < 4; i++)
            {
                long1 = (long1 << 16) + num[i];
            }

            ulong long2 = num[4];
            for (int i = 5; i < 8; i++)
            {
                long2 = (long2 << 16) + num[i];
            }

            ulong[] longs = { long2, long1 };
            return longs;
        }


        public static string longToIP(ulong[] ip)
        {
            string ipString = "";

            //for every long: it should be two of them
            for (int i = 0; i < 2; ++i)
            {
                ulong crtLong = ip[i]; // Warning: ip[i] is call by reference...

                //we display in total 4 parts for every long
                for (int j = 0; j < 4; j++)
                {
                    ipString = (crtLong & 0xFFFF).ToString("x04") + (ipString == string.Empty ? "" : ":" + ipString);
                    crtLong = crtLong >> 16;
                } // Next j 

            } // Next i 

            return ipString;
        } // End Function 


    } // End Class 


} // End Namespace 
