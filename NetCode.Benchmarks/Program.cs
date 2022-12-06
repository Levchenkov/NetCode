// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using NetCode.Benchmarks;

var switcher = new BenchmarkSwitcher(new[]
{
    typeof(BitReader_ReadBits_Benchmark),
    typeof(BitReader_ReadByte_Benchmark),
    typeof(BitReader_ReadInt_Benchmark),
    typeof(BitWriter_WriteBits_Benchmark),
    typeof(BitWriter_WriteByte_Benchmark),
    typeof(BitWriter_WriteInt_Benchmark),
    typeof(ByteReaderBenchmark),
    typeof(ByteWriterBenchmark),
});

switcher.Run(args);