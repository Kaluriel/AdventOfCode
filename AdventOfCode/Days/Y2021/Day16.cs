using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day16 : DayBase2021
    {
        class Packet
        {
            public int Version { get; set; }
            public int Type { get; set; }
            public ulong Value { get; set; }

            public List<Packet> SubPackets { get; } = new List<Packet>();

            public int SumOfVersions()
            {
                return SubPackets.Sum(x => x.SumOfVersions()) + Version;
            }

            public ulong Evaluate()
            {
                ulong ret = Type switch
                {
                    0 => SubPackets.Aggregate((ulong)0, (x, y) => x + y.Evaluate()),
                    1 => SubPackets.Aggregate((ulong)1, (x, y) => x * y.Evaluate()),
                    2 => SubPackets.Min(x => x.Evaluate()),
                    3 => SubPackets.Max(x => x.Evaluate()),
                    4 => Value,
                    5 => (ulong)((SubPackets[0].Evaluate() > SubPackets[1].Evaluate()) ? 1 : 0),
                    6 => (ulong)((SubPackets[0].Evaluate() < SubPackets[1].Evaluate()) ? 1 : 0),
                    7 => (ulong)((SubPackets[0].Evaluate() == SubPackets[1].Evaluate()) ? 1 : 0),
                    _ => 0,
                };

                return ret;
            }
        }

        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var binary = Source.ToCharArray()
                               .Select(c => c.ToString())
                               .Select(h => Convert.ToInt32(h, 16))
                               .Select(d => Convert.ToString(d, 2).PadLeft(4, '0'))
                               .Aggregate(string.Empty, (x, y) => x + y);

            Console.WriteLine(binary);

            int index = 0;
            var packet = ReadPacket(0, binary, ref index);

            //
            return Task.FromResult<object>(
                packet.SumOfVersions()
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var binary = Source.ToCharArray()
                               .Select(c => c.ToString())
                               .Select(h => Convert.ToInt32(h, 16))
                               .Select(d => Convert.ToString(d, 2).PadLeft(4, '0'))
                               .Aggregate(string.Empty, (x, y) => x + y);

            Console.WriteLine(binary);

            int index = 0;
            var packet = ReadPacket(0, binary, ref index);

            //
            return Task.FromResult<object>(
                packet.Evaluate()
            );
        }

        static Packet ReadPacket(int indentCount, string binary, ref int index)
        {
            var ret = new Packet();

            Console.WriteLine();

            ret.Version = ReadInt32(binary, ref index, 3);
            Console_WriteLine(indentCount, "Version: " + ret.Version);

            ret.Type = ReadInt32(binary, ref index, 3);
            Console_WriteLine(indentCount, "Type: " + ret.Type);

            if (ret.Type == 4)
            {
                ret.Value = ReadLiteralNumber(binary, ref index);
                Console_WriteLine(indentCount, "Value: " + ret.Value);
            }
            else
            {
                int lengthTypeID = ReadInt32(binary, ref index, 1);
                Console_WriteLine(indentCount, "Length Type ID: " + lengthTypeID);

                if (lengthTypeID == 0)
                {
                    int totalLengthInBits = ReadInt32(binary, ref index, 15);
                    Console_WriteLine(indentCount, "Total Length In Bits: " + totalLengthInBits);

                    int subPacketEnd = index + totalLengthInBits;
                    while (index < subPacketEnd)
                    {
                        var subPacket = ReadPacket(indentCount + 1, binary, ref index);
                        ret.SubPackets.Add(subPacket);
                    }
                }
                else
                {
                    int subPacketCount = ReadInt32(binary, ref index, 11);
                    Console_WriteLine(indentCount, "Sub Packet Count: " + subPacketCount);

                    for (int i = 0; i < subPacketCount; ++i)
                    {
                        var subPacket = ReadPacket(indentCount + 1, binary, ref index);
                        ret.SubPackets.Add(subPacket);
                    }
                }
            }

            return ret;
        }

        static ulong ReadLiteralNumber(string binary, ref int index)
        {
            string literalBits = string.Empty;

            do
            {
                string bits = ReadBinary(binary, ref index, 5);
                literalBits += bits[1..5];
            }
            while (binary[index - 5] == '1');

            return Convert.ToUInt64(literalBits, 2);
        }

        static int ReadInt32(string binary, ref int index, int length)
        {
            string bits = ReadBinary(binary, ref index, length);
            return Convert.ToInt32(bits, 2);
        }

        static ulong ReadUInt64(string binary, ref int index, int length)
        {
            string bits = ReadBinary(binary, ref index, length);
            return Convert.ToUInt64(bits, 2);
        }

        static string ReadBinary(string binary, ref int index, int length)
        {
            string ret = binary[index..(index + length)];
            index += length;
            return ret;
        }

        static void Console_WriteLine(int indentCount, string str)
        {
            Console.WriteLine(
                Enumerable.Repeat("\t", indentCount).Aggregate(str, (x, y) => y + x)
            );
        }
    }
}
