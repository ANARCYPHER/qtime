﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Quantum.Simulation.Core;
using System;
using System.Numerics;

namespace Microsoft.Quantum.Convert
{
    public partial class IntAsDouble
    {
        public class Native : IntAsDouble
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<long, double> __Body__ => (arg) => System.Convert.ToDouble(arg);
        }
    }

    public partial class IntAsBigInt
    {
        public class Native : IntAsBigInt
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<long, BigInteger> __Body__ => (arg) => new BigInteger(arg);
        }
    }


    public partial class MaybeBigIntAsInt
    {
        public class Native : MaybeBigIntAsInt
        {
            static (long, bool) MaybeBigIntAsIntFunc(BigInteger x)
            {
                if (x > long.MaxValue || x < long.MinValue)
                {
                    return (0, false);
                }
                else
                {
                    return ((long)x, true);
                }
            }

            public Native(IOperationFactory m) : base(m) { }
            public override Func<BigInteger, (long, bool)> __Body__ => MaybeBigIntAsIntFunc;
        }
    }


    public partial class BigIntAsBoolArray
    {
        public class Native : BigIntAsBoolArray
        {
            static IQArray<bool> BigIntAsBoolArrayFunc(BigInteger x)
            {
                var bytes = x.ToByteArray();
                long n = bytes.LongLength * 8;
                var array = new bool[n];
                for (int i = 0; i < bytes.Length; i++)
                {
                    var b = bytes[i];
                    array[(8 * i) + 0] = (b & 0x01) != 0;
                    array[(8 * i) + 1] = (b & 0x02) != 0;
                    array[(8 * i) + 2] = (b & 0x04) != 0;
                    array[(8 * i) + 3] = (b & 0x08) != 0;
                    array[(8 * i) + 4] = (b & 0x10) != 0;
                    array[(8 * i) + 5] = (b & 0x20) != 0;
                    array[(8 * i) + 6] = (b & 0x40) != 0;
                    array[(8 * i) + 7] = (b & 0x80) != 0;
                }
                return new QArray<bool>(array);
            }

            public Native(IOperationFactory m) : base(m) { }
            public override Func<BigInteger, IQArray<bool>> __Body__ => BigIntAsBoolArrayFunc;
        }
    }

    public partial class BoolArrayAsBigInt
    {
        public class Native : BoolArrayAsBigInt
        {
            static BigInteger BoolArrayAsBigIntFunc(IQArray<bool> x)
            {
                long fullBytes = x.Length / 8;
                long leftOver = x.Length % 8;
                long totalBytes = fullBytes + (leftOver > 0 ? 1 : 0);
                var array = new byte[totalBytes];
                for (int i = 0; i < fullBytes; i++)
                {
                    array[i] += (byte)(x[(8 * i) + 0] ? 0x01 : 0);
                    array[i] += (byte)(x[(8 * i) + 1] ? 0x02 : 0);
                    array[i] += (byte)(x[(8 * i) + 2] ? 0x04 : 0);
                    array[i] += (byte)(x[(8 * i) + 3] ? 0x08 : 0);
                    array[i] += (byte)(x[(8 * i) + 4] ? 0x10 : 0);
                    array[i] += (byte)(x[(8 * i) + 5] ? 0x20 : 0);
                    array[i] += (byte)(x[(8 * i) + 6] ? 0x40 : 0);
                    array[i] += (byte)(x[(8 * i) + 7] ? 0x80 : 0);
                }
                long last = fullBytes * 8;
                if (leftOver > 0) array[fullBytes] += (byte)(x[last + 0] ? 0x01 : 0);
                if (leftOver > 1) array[fullBytes] += (byte)(x[last + 1] ? 0x02 : 0);
                if (leftOver > 2) array[fullBytes] += (byte)(x[last + 2] ? 0x04 : 0);
                if (leftOver > 3) array[fullBytes] += (byte)(x[last + 3] ? 0x08 : 0);
                if (leftOver > 4) array[fullBytes] += (byte)(x[last + 4] ? 0x10 : 0);
                if (leftOver > 5) array[fullBytes] += (byte)(x[last + 5] ? 0x20 : 0);
                if (leftOver > 6) array[fullBytes] += (byte)(x[last + 6] ? 0x40 : 0);
                return new BigInteger(array);
            }

            public Native(IOperationFactory m) : base(m) { }
            public override Func<IQArray<bool>, BigInteger> __Body__ => BoolArrayAsBigIntFunc;
        }
    }

    public partial class BoolAsString
    {
        public class Native : BoolAsString
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<bool, string> __Body__ => (arg) => System.Convert.ToString(arg);
        }
    }

    public partial class DoubleAsString
    {
        public class Native : DoubleAsString
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<double, string> __Body__ => (arg) => System.Convert.ToString(arg);
        }
    }

    public partial class DoubleAsStringWithFormat
    {
        public class Native : DoubleAsStringWithFormat
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<(double, string), string> __Body__ => (arg) => arg.Item1.ToString(arg.Item2);
        }
    }

    public partial class IntAsString
    {
        public class Native : IntAsString
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<long, string> __Body__ => (arg) => System.Convert.ToString(arg);
        }
    }

    public partial class IntAsStringWithFormat
    {
        public class Native : IntAsStringWithFormat
        {
            public Native(IOperationFactory m) : base(m) { }
            public override Func<(long, string), string> __Body__ => (arg) => arg.Item1.ToString(arg.Item2);
        }
    }

}
