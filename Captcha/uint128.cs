﻿
using System.Runtime.CompilerServices;


namespace Captcha
{

    // https://raw.githubusercontent.com/somdoron/NaCl.net/master/nacl.net/UInt128.cs
    // http://www.codeproject.com/Tips/784635/UInt-Bit-Operations
    // http://www.codeproject.com/Tips/785014/UInt-Division-Modulus
    public struct UInt128
        : System.IComparable
        , System.IComparable<UInt128>
        , System.Collections.Generic.IComparer<UInt128>
        , System.IEquatable<UInt128>
    {

        // http://stackoverflow.com/questions/11656241/how-to-print-uint128-t-number-using-gcc#answer-11659521
        public override string ToString()
        {
            if (this.Low == 0 && this.High == 0)
                return "0";

            uint[] buf = new uint[40];
            const ulong ONE = 1;
            const ulong ZERO = 0;

            uint i, j, m = 39;
            for (i = 64; i-- > 0; )
            {
                ulong carry = (this.High & (ONE << (int)i));
                carry = carry != 0 ? ONE : ZERO; // ToBool

                for (j = 39; j-- > m + 1 || carry != 0; )
                {
                    ulong d = 2 * buf[j] + carry;
                    carry = d > 9 ? ONE : ZERO;
                    buf[j] = carry != 0 ? (char)(d - 10) : (char)d;
                } // Next j 
                m = j;
            } // Next i 

            for (i = 64; i-- > 0; )
            {
                ulong carry = (this.Low & (ONE << (int)i));
                carry = carry != 0 ? ONE : ZERO; // ToBool

                for (j = 39; j-- > m + 1 || carry != 0; )
                {
                    ulong d = 2 * buf[j] + carry;
                    carry = d > 9 ? ONE : ZERO;
                    buf[j] = carry != 0 ? (char)(d - 10) : (char)d;
                } // Next j 
                m = j;
            } // Next i 

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            bool hasFirstNonNull = false;
            for (int k = 0; k < buf.Length - 1; ++k)
            {

                if (hasFirstNonNull || buf[k] != 0)
                {
                    hasFirstNonNull = true;
                    sb.Append(buf[k].ToString());
                } // End if(hasFirstNonNull || buf[k] != 0)

            } // Next k 

            string s = sb.ToString();
            sb.Length = 0;
            sb = null;
            return s;
        }


        public string ToIpV6()
        {
            string ipString = "";

            //we display in total 4 parts for every long
            ulong crtLong = this.Low;
            for (int i = 0; i < 4; ++i)
            {
                ipString = (crtLong & 0xFFFF).ToString("x04") + (ipString == string.Empty ? "" : ":" + ipString);
                crtLong = crtLong >> 16;
            } // Next j 

            crtLong = this.High;
            for (int i = 0; i < 4; ++i)
            {
                ipString = (crtLong & 0xFFFF).ToString("x04") + (ipString == string.Empty ? "" : ":" + ipString);
                crtLong = crtLong >> 16;
            } // Next j 

            return ipString;
        } // End Function ToIpV6 
        

        public bool Equals(UInt128 other)
        {
            return this.High == other.High && this.Low == other.Low;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
                return false;

            return obj is UInt128 && this.Equals((UInt128)obj);
        }


        public int Compare(UInt128 x, UInt128 y)
        {
            if (x > y) return -1;
            if (x == y) return 0;
            return 1;
        }


        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1; // https://msdn.microsoft.com/en-us/library/system.icomparable.compareto(v=vs.110).aspx

            System.Type t = obj.GetType();

            if (object.ReferenceEquals(t, typeof(UInt128)))
            {
                UInt128 ui = (UInt128)obj;
                return this.Compare(this, ui);
            }


            if (object.ReferenceEquals(t, typeof(System.DBNull)))
                return 1;

            ulong? lowerPart = obj as ulong?;
            if (!lowerPart.HasValue)
                return 1;

            UInt128 compareTarget = new UInt128(0, lowerPart.Value);
            return this.Compare(this, compareTarget);
        }


        public int CompareTo(UInt128 other)
        {
            if (other > this) return -1;
            if (this == other) return 0;
            return 1;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt128(ulong n)
        {
            Low = n;
            High = 0;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt128(ulong high, ulong low)
        {
            Low = low;
            High = high;
        }

        public ulong High;
        public ulong Low;



        public static UInt128 MaxValue
        {
            get
            {
                return new UInt128(ulong.MaxValue, ulong.MaxValue);
            }
        }

        public static UInt128 MinValue
        {
            get
            {
                return new UInt128(ulong.MinValue, ulong.MinValue);
            }
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(UInt128 value)
        {
            return value.Low;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UInt128(ulong value)
        {
            return new UInt128(value);
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMultiplyUInt64(ref ulong left, ref ulong right)
        {
            ulong a32 = left >> 32;
            ulong a00 = left & 0xffffffffu;

            ulong b32 = right >> 32;
            ulong b00 = right & 0xffffffffu;

            ulong high = a32 * b32;
            ulong low = a00 * b00;

            ulong addLow = (a32 * b00 + a00 * b32);
            ulong addHigh = addLow >> 32;
            addLow = addLow << 32;

            ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
            high += addHigh + c;
            low += addLow;

            High = high;
            Low = low;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddMultiplyUInt64(ref ulong left, ref ulong right)
        {
            ulong a32 = left >> 32;
            ulong a00 = left & 0xffffffffu;

            ulong b32 = right >> 32;
            ulong b00 = right & 0xffffffffu;

            ulong high = a32 * b32;
            ulong low = a00 * b00;

            ulong addLow = (a32 * b00 + a00 * b32);
            ulong addHigh = addLow >> 32;
            addLow = addLow << 32;

            ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
            high += addHigh + c;
            low += addLow;

            c = (((Low & low) & 1) + (Low >> 1) + (low >> 1)) >> 63;
            High = High + high + c;
            Low = Low + low;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 MultiplyUInt64(ulong left, ulong right)
        {
            ulong a32 = left >> 32;
            ulong a00 = left & 0xffffffffu;

            ulong b32 = right >> 32;
            ulong b00 = right & 0xffffffffu;

            ulong high = a32 * b32;

            ulong low = a00 * b00;

            ulong addLow = (a32 * b00 + a00 * b32);
            ulong addHigh = addLow >> 32;
            addLow = addLow << 32;

            ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
            high += addHigh + c;
            low += addLow;

            return new UInt128(high, low);
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(UInt128 n)
        {
            ulong c = (((Low & n.Low) & 1) + (Low >> 1) + (n.Low >> 1)) >> 63;
            High = High + n.High + c;
            Low = Low + n.Low;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ulong n)
        {
            ulong c = (((Low & n) & 1) + (Low >> 1) + (n >> 1)) >> 63;
            High = High + c;
            Low = Low + n;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UInt128 lhs, UInt128 rhs)
        {
            return (lhs.High == rhs.High && lhs.Low == rhs.Low);
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UInt128 lhs, uint rhs)
        {
            if (lhs.High != 0)
                return false;

            return (lhs.Low == rhs);
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UInt128 lhs, ulong rhs)
        {
            if (lhs.High != 0)
                return false;

            return (lhs.Low == rhs);
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UInt128 lhs, UInt128 rhs)
        {
            return (lhs.High != rhs.High || lhs.Low != rhs.Low);
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UInt128 lhs, uint rhs)
        {
            if (lhs.High != 0)
                return true;

            return (lhs.Low != rhs);
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UInt128 lhs, ulong rhs)
        {
            if (lhs.High != 0)
                return true;

            return (lhs.Low != rhs);
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High < rhs.High)
                return true;

            if (lhs.High == rhs.High && lhs.Low < rhs.Low)
                return true;

            return false;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High > rhs.High)
                return true;

            if (lhs.High == rhs.High && lhs.Low > rhs.Low)
                return true;

            return false;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High < rhs.High)
                return true;

            if (lhs.High == rhs.High)
            {
                if (lhs.Low <= rhs.Low)
                    return true;
            }

            return false;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High > rhs.High)
                return true;

            if (lhs.High == rhs.High)
            {
                if (lhs.Low >= rhs.Low)
                    return true;
            }

            return false;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt128 left, ulong right)
        {
            unchecked
            {
                ulong a96 = left.High >> 32;
                ulong a64 = left.High & 0xffffffffu;
                ulong a32 = left.Low >> 32;
                ulong a00 = left.Low & 0xffffffffu;

                ulong b32 = right >> 32;
                ulong b00 = right & 0xffffffffu;

                // multiply [a96 .. a00] x [b96 .. b00]
                // terms higher than c96 disappear off the high side
                // terms c96 and c64 are safe to ignore carry bit
                ulong c96 = a96 * b00 + a64 * b32;
                ulong c64 = a64 * b00 + a32 * b32;

                ulong high = (c96 << 32) + c64;
                ulong low = a00 * b00;

                ulong addLow = (a32 * b00 + a00 * b32);
                ulong addHigh = addLow >> 32;
                addLow = addLow << 32;

                ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
                high = high + addHigh + c;
                low = low + addLow;

                return new UInt128(high, low);
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 left, ulong right)
        {
            ulong c = (((left.Low & right) & 1) + (left.Low >> 1) + (right >> 1)) >> 63;
            ulong high = left.High + c;
            ulong low = left.Low + right;

            return new UInt128(high, low);
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 left, UInt128 right)
        {
            ulong c = (((left.Low & right.Low) & 1) + (left.Low >> 1) + (right.Low >> 1)) >> 63;
            ulong high = left.High + right.High + c;
            ulong low = left.Low + right.Low;

            return new UInt128(high, low);
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator >>(UInt128 value, int shift)
        {
            UInt128 shifted = new UInt128();

            if (shift > 63)
            {
                shifted.Low = value.High >> (shift - 64);
                shifted.High = 0;
            }
            else
            {
                shifted.High = value.High >> shift;
                shifted.Low = (value.High << (64 - shift)) | (value.Low >> shift);
            }
            return shifted;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator <<(UInt128 value, int shift)
        {
            UInt128 shifted = new UInt128();

            if (shift > 63)
            {
                shifted.High = value.Low << (shift - 64);
                shifted.Low = 0;
            }
            else
            {
                ulong ul = value.Low >> (64 - shift);
                shifted.High = ul | (value.High << shift);
                shifted.Low = value.Low << shift;
            }
            return shifted;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator &(UInt128 left, ulong right)
        {
            UInt128 result = left;
            result.High = 0;
            result.Low &= right;

            return result;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator &(UInt128 left, UInt128 right)
        {
            UInt128 result = left;
            result.High &= right.High;
            result.Low &= right.Low;

            return result;
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator |(UInt128 left, UInt128 right)
        {
            UInt128 result = left;
            result.High |= right.High;
            result.Low |= right.Low;
            return result;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator ^(UInt128 left, UInt128 right)
        {
            UInt128 result = left;
            result.High ^= right.High;
            result.Low ^= right.Low;
            return result;
        }


        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator ~(UInt128 left)
        {
            UInt128 result = left;
            result.High = ~result.High;
            result.Low = ~result.Low;
            return result;
        }

    }
}