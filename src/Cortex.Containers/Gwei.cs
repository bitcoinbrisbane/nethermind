﻿using System;

namespace Cortex.Containers
{
    public struct Gwei : IEquatable<Gwei>
    {
        private readonly ulong _value;

        public Gwei(ulong value)
        {
            _value = value;
        }

        public static explicit operator Gwei(ulong value) => new Gwei(value);

        public static explicit operator ulong(Gwei slot) => slot._value;

        public static Gwei Min(Gwei val1, Gwei val2)
        {
            return new Gwei(Math.Min(val1._value, val2._value));
        }

        public static Gwei operator -(Gwei left, Gwei right)
        {
            return new Gwei(left._value - right._value);
        }

        public static bool operator !=(Gwei left, Gwei right)
        {
            return !(left == right);
        }

        public static Gwei operator %(Gwei left, Gwei right)
        {
            return new Gwei(left._value % right._value);
        }

        public static Gwei operator +(Gwei left, Gwei right)
        {
            return new Gwei(left._value + right._value);
        }

        public static bool operator ==(Gwei left, Gwei right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is Gwei slot && Equals(slot);
        }

        public bool Equals(Gwei other)
        {
            return _value == other._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
