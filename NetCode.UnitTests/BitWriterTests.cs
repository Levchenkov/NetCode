using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class BitWriterTests
{
    [Fact]
    public void GetArray_ValueIs5BitsWrite3Bits_ShouldBe3WrittenBits()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(3, 0b_10101);
        bitWriter.BitsCount.Should().Be(3);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_101);
    }

    [Fact]
    public void GetArray_ValueIs3BitsWrite4Bits_ShouldBe4WrittenBits()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(4, 0b_101);
        bitWriter.BitsCount.Should().Be(4);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_0101);
    }

    [Fact]
    public void GetArray_ValueIs5BitsWriteTwice3Bits_ShouldBe6WrittenBits()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(3, 0b_10101);
        bitWriter.WriteBits(3, 0b_10101);
        bitWriter.BitsCount.Should().Be(6);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_101_101);
    }

    [Fact]
    public void GetArray_ValueIs5BitsWriteTwice4Bits_ShouldBe8WrittenBits()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(4, 0b_10101);
        bitWriter.WriteBits(4, 0b_10101);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_0101_0101);
    }

    [Fact]
    public void GetArray_ValueIs5BitsWrite9BitsTotal_ShouldBe9WrittenBits()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(4, 0b_10101);
        bitWriter.WriteBits(5, 0b_10101);
        bitWriter.BitsCount.Should().Be(9);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);

        bitWriter.Array[0].Should().Be(0b_0101_0101);
        bitWriter.Array[1].Should().Be(0b_1);
    }

    [Fact]
    public void WriteUnAlignedData_Write1BitAndDontFlush_ExceptionExpected()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(1, 1u);

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(1);

        Func<byte[]> action = () => bitWriter.Array;

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WriteAlignedData_Write32BitsAndDontFlush_ShouldBeOk()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(32, 1u);

        bitWriter.BytesCount.Should().Be(4);
        bitWriter.BitsCount.Should().Be(32);

        Func<byte[]> action = () => bitWriter.Array;

        action.Should().NotThrow();
    }

    [Fact]
    public void WriteAlignedData_Write11BitsAndWrite21BitsAndDontFlush_ShouldBeOk()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(11, uint.MaxValue);
        bitWriter.WriteBits(21, uint.MaxValue);

        bitWriter.BytesCount.Should().Be(4);
        bitWriter.BitsCount.Should().Be(32);

        Func<byte[]> action = () => bitWriter.Array;

        action.Should().NotThrow();
    }

    [Fact]
    public void BytesCount_WriteByteAnd1BitAndDontFlush_BytesCountShouldBe2()
    {
        var bitWriter = new BitWriter();

        bitWriter.Write(byte.MaxValue);
        bitWriter.WriteBits(1, 1);

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(9);
    }

    [Fact]
    public void BytesCount_WriteByteAnd1BitAndFlush_BytesCountShouldBe2()
    {
        var bitWriter = new BitWriter();

        bitWriter.Write(byte.MaxValue);
        bitWriter.WriteBits(1, 1);
        bitWriter.BitsCount.Should().Be(9);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);
    }

    [Fact]
    public void Clear_WriteBitAndClear_BytesCountShouldBe0()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(1, 1);
        bitWriter.Clear();

        bitWriter.BytesCount.Should().Be(0);
        bitWriter.BitsCount.Should().Be(0);
    }

    [Fact]
    public void Clear_WriteIntAndClear_BytesCountShouldBe0()
    {
        var bitWriter = new BitWriter();

        bitWriter.Write(int.MaxValue);
        bitWriter.Clear();

        bitWriter.BytesCount.Should().Be(0);
        bitWriter.BitsCount.Should().Be(0);
    }

    [Fact]
    public void Flush_Write7Bits_ArrayShouldContain1Byte()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(7, 0b_111_1111);
        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(7);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_111_1111);
    }

    [Fact]
    public void Flush_Write9Bits_ArrayShouldContain2Byte()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(9, uint.MaxValue);
        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(9);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);

        bitWriter.Array[0].Should().Be(0b_1111_1111);
        bitWriter.Array[1].Should().Be(0b_1);
    }

    [Fact]
    public void Flush_Write15Bits_ArrayShouldContain2Byte()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(15, ushort.MaxValue);
        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(15);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);

        bitWriter.Array[0].Should().Be(0b_11111111);
        bitWriter.Array[1].Should().Be(0b_1111111);
    }

    [Fact]
    public void Flush_Write17Bits_ArrayShouldContain3Byte()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(17, uint.MaxValue);
        bitWriter.BytesCount.Should().Be(3);
        bitWriter.BitsCount.Should().Be(17);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(3);
        bitWriter.BitsCount.Should().Be(24);

        bitWriter.Array[0].Should().Be(0b_11111111);
        bitWriter.Array[1].Should().Be(0b_11111111);
        bitWriter.Array[2].Should().Be(0b1);
    }

    [Fact]
    public void Flush_Write31Bits_ArrayShouldContain4Byte()
    {
        var bitWriter = new BitWriter();

        bitWriter.WriteBits(31, uint.MaxValue);
        bitWriter.BytesCount.Should().Be(4);
        bitWriter.BitsCount.Should().Be(31);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(4);
        bitWriter.BitsCount.Should().Be(32);

        bitWriter.Array[0].Should().Be(0b_1111_1111);
        bitWriter.Array[1].Should().Be(0b_1111_1111);
        bitWriter.Array[2].Should().Be(0b_1111_1111);
        bitWriter.Array[3].Should().Be(0b_111_1111);
    }

    [Fact]
    public void DefaultCtor_WriteUInt_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        uint value = 0b_10101010_01010101_11001100_11110000;
        bitWriter.Write(value);

        bitWriter.BytesCount.Should().Be(4);
        bitWriter.BitsCount.Should().Be(32);

        bitWriter.Array[0].Should().Be(0b11110000);
        bitWriter.Array[1].Should().Be(0b11001100);
        bitWriter.Array[2].Should().Be(0b01010101);
        bitWriter.Array[3].Should().Be(0b10101010);
    }

    [Fact]
    public void DefaultCtor_WriteInt_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        int value = unchecked((int)0b_10101010_01010101_11001100_11110000);
        bitWriter.Write(value);

        bitWriter.BytesCount.Should().Be(4);
        bitWriter.BitsCount.Should().Be(32);

        bitWriter.Array[0].Should().Be(0b11110000);
        bitWriter.Array[1].Should().Be(0b11001100);
        bitWriter.Array[2].Should().Be(0b01010101);
        bitWriter.Array[3].Should().Be(0b10101010);
    }

    [Fact]
    public void DefaultCtor_WriteUShort_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        ushort value = 0b_10101010_01010101;
        bitWriter.Write(value);

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);

        bitWriter.Array[0].Should().Be(0b01010101);
        bitWriter.Array[1].Should().Be(0b10101010);
    }

    [Fact]
    public void DefaultCtor_WriteShort_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        short value = unchecked((short)0b_10101010_01010101);
        bitWriter.Write(value);

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);

        bitWriter.Array[0].Should().Be(0b01010101);
        bitWriter.Array[1].Should().Be(0b10101010);
    }

    [Fact]
    public void DefaultCtor_WriteByte_ArrayShouldContainByte()
    {
        var bitWriter = new BitWriter();

        byte value = 0b10101010;
        bitWriter.Write(value);

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);
        bitWriter.Array[0].Should().Be(0b10101010);
    }

    [Fact]
    public void DefaultCtor_WriteBitAndUInt_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        uint value = 0b_10101010_01010101_11001100_11110000;
        bitWriter.WriteBits(1, 1u);
        bitWriter.Write(value);
        bitWriter.BitsCount.Should().Be(33);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(5);
        bitWriter.BitsCount.Should().Be(40);

        bitWriter.Array[0].Should().Be(0b11100001);
        bitWriter.Array[1].Should().Be(0b10011001);
        bitWriter.Array[2].Should().Be(0b10101011);
        bitWriter.Array[3].Should().Be(0b01010100);
        bitWriter.Array[4].Should().Be(0b1);
    }

    [Fact]
    public void DefaultCtor_WriteBitAndInt_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        int value = unchecked((int)0b_10101010_01010101_11001100_11110000);
        bitWriter.WriteBits(1, 1);
        bitWriter.Write(value);
        bitWriter.BitsCount.Should().Be(33);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(5);
        bitWriter.BitsCount.Should().Be(40);

        bitWriter.Array[0].Should().Be(0b11100001);
        bitWriter.Array[1].Should().Be(0b10011001);
        bitWriter.Array[2].Should().Be(0b10101011);
        bitWriter.Array[3].Should().Be(0b01010100);
        bitWriter.Array[4].Should().Be(0b1);
    }

    [Fact]
    public void DefaultCtor_WriteBitAndUShort_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        ushort value = 0b_10101010_01010101;
        bitWriter.WriteBits(1, 1);
        bitWriter.Write(value);
        bitWriter.BitsCount.Should().Be(17);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(3);
        bitWriter.BitsCount.Should().Be(24);

        bitWriter.Array[0].Should().Be(0b10101011);
        bitWriter.Array[1].Should().Be(0b01010100);
        bitWriter.Array[2].Should().Be(0b1);
    }

    [Fact]
    public void DefaultCtor_WriteBitAndShort_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        short value = unchecked((short)0b_10101010_01010101);
        bitWriter.WriteBits(1, 1);
        bitWriter.Write(value);
        bitWriter.BitsCount.Should().Be(17);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(3);
        bitWriter.BitsCount.Should().Be(24);

        bitWriter.Array[0].Should().Be(0b10101011);
        bitWriter.Array[1].Should().Be(0b01010100);
        bitWriter.Array[2].Should().Be(0b1);
    }

    [Fact]
    public void DefaultCtor_WriteBitAndByte_ArrayShouldContainBytes()
    {
        var bitWriter = new BitWriter();

        byte value = 0b10101010;
        bitWriter.WriteBits(1, 1);
        bitWriter.Write(value);
        bitWriter.BitsCount.Should().Be(9);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
        bitWriter.BitsCount.Should().Be(16);

        bitWriter.Array[0].Should().Be(0b01010101);
        bitWriter.Array[1].Should().Be(0b1);
    }

    [Fact]
    public void WriteBits_ProvideNegativeBitCount_ExceptionExpected()
    {
        var bitWriter = new BitWriter();

        Action action = () => bitWriter.WriteBits(-1, 1);

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void WriteBits_ProvideZeroBitCount_ShouldBeOk()
    {
        var bitWriter = new BitWriter();

        Action action = () => bitWriter.WriteBits(0, 1);

        action.Should().NotThrow();
    }

    [Fact]
    public void WriteBits_ProvideMoreThan32BitsCount_ExceptionExpected()
    {
        var bitWriter = new BitWriter();

        Action action = () => bitWriter.WriteBits(33, 1);

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void WriteUInt_ArraySizeIs4BytesAndWriteUInt_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[4]);

        bitWriter.Write(uint.MaxValue);

        Action action = () => bitWriter.Write(uint.MaxValue);

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Flush_ArraySizeIs4AndWriteUIntAndWrite1Bit_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[4]);

        bitWriter.Write(uint.MaxValue);
        bitWriter.WriteBits(1, 1);

        Action action = () => bitWriter.Flush();

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Flush_ArraySizeIs4AndWrite1BitAndWriteUInt_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[4]);

        bitWriter.WriteBits(1, 1);
        bitWriter.Write(uint.MaxValue);

        Action action = () => bitWriter.Flush();

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Flush_ArraySizeIs1_Write1BitAndWriteByte_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[1]);

        bitWriter.WriteBits(1, 1);
        bitWriter.Write(byte.MaxValue);

        Action action = () => bitWriter.Flush();

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Flush_ArraySizeIs1_Write9Bits_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[1]);

        bitWriter.WriteBits(9, 1);

        Action action = () => bitWriter.Flush();

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void Flush_ArraySizeIs4_Write31BitAndWrite2Bits_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[4]);

        bitWriter.WriteBits(31, uint.MaxValue);
        bitWriter.WriteBits(2, uint.MaxValue);

        Action action = () => bitWriter.Flush();

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void SetArray_SetNonEmptyArrayAndWrite_ArrayShouldContainData()
    {
        var bitWriter = new BitWriter(Array.Empty<byte>());
        bitWriter.SetArray(new byte[2]);

        bitWriter.WriteBits(3, 0b_10101);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);

        bitWriter.Array[0].Should().Be(0b_101);
    }

    [Fact]
    public void WriteBool_SetTrue_ArrayShouldContainSetBit()
    {
        var bitWriter = new BitWriter(new byte[1]);

        bitWriter.Write(true);
        bitWriter.BitsCount.Should().Be(1);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_1);
    }

    [Fact]
    public void WriteBool_SetFalse_ArrayShouldContainEmptyBit()
    {
        var bitWriter = new BitWriter(new byte[1]);

        bitWriter.Write(false);
        bitWriter.BitsCount.Should().Be(1);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_0);
    }

    [Fact]
    public void SetAt_PositionIsNegative_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[1]);

        var action = () => bitWriter.SetAt(-1, true);

        action.Should().ThrowExactly<IndexOutOfRangeException>();
    }

    [Fact]
    public void SetAt_PositionIsBiggerThanArrayLength_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[1]);

        var action = () => bitWriter.SetAt(8, true);

        action.Should().ThrowExactly<IndexOutOfRangeException>();
    }

    [Fact]
    public void SetAt_SetTrueAt0BeforeWrite_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[2]);
        bitWriter.BitsCount.Should().Be(0);

        var action = () => bitWriter.SetAt(0, true);

        action.Should().ThrowExactly<NotSupportedException>();
    }

    [Fact]
    public void SetAt_PositionIsBiggerThanBitsWritten_ExceptionExpected()
    {
        var bitWriter = new BitWriter(new byte[2]);
        bitWriter.WriteBits(4, byte.MaxValue);
        bitWriter.BitsCount.Should().Be(4);

        var action = () => bitWriter.SetAt(4, true);

        action.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void SetAt_SetTrueAt0_BitShouldBeSet()
    {
        var bitWriter = new BitWriter(new byte[1]);
        bitWriter.WriteBits(1, 0);
        bitWriter.BitsCount.Should().Be(1);

        bitWriter.SetAt(0, true);
        bitWriter.BitsCount.Should().Be(1);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_0000_0001);
    }

    [Fact]
    public void SetAt_SetTrueAt1_BitShouldBeSet()
    {
        var bitWriter = new BitWriter(new byte[1]);
        bitWriter.WriteBits(2, 0);
        bitWriter.BitsCount.Should().Be(2);

        bitWriter.SetAt(1, true);
        bitWriter.BitsCount.Should().Be(2);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_0000_0010);
    }

    [Fact]
    public void SetAt_SetTrueAt7_BitShouldBeSet()
    {
        var bitWriter = new BitWriter(new byte[1]);
        bitWriter.Write((byte)0);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.SetAt(7, true);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_1000_0000);
    }

    [Fact]
    public void SetAt_SetFalse_BitShouldNotBeSet()
    {
        var bitWriter = new BitWriter(new byte[1]);
        bitWriter.Write((byte)0b_1111_1111);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.SetAt(4, false);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
        bitWriter.BitsCount.Should().Be(8);

        bitWriter.Array[0].Should().Be(0b_1110_1111);
    }

    [Fact]
    public void SetAt_SetTrueAtPositionBeforeBuffer_BitShouldBeSet()
    {
        var bitWriter = new BitWriter(new byte[9]);
        bitWriter.Write(0);
        bitWriter.Write(0);
        bitWriter.BitsCount.Should().Be(64);

        bitWriter.SetAt(1, true);
        bitWriter.SetAt(8, true);
        bitWriter.BitsCount.Should().Be(64);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(8);
        bitWriter.BitsCount.Should().Be(64);

        bitWriter.Array[0].Should().Be(0b_0000_0010); // bitPosition: 1
        bitWriter.Array[1].Should().Be(0b_0000_0001); // bitPosition: 8
    }

    [Fact]
    public void SetAt_SetFalseAtPositionBeforeBuffer_BitShouldNotBeSet()
    {
        var bitWriter = new BitWriter(new byte[9]);
        bitWriter.Write(uint.MaxValue);
        bitWriter.Write(uint.MaxValue);
        bitWriter.BitsCount.Should().Be(64);

        bitWriter.SetAt(1, false);
        bitWriter.SetAt(8, false);
        bitWriter.BitsCount.Should().Be(64);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(8);
        bitWriter.BitsCount.Should().Be(64);

        bitWriter.Array[0].Should().Be(0b_1111_1101); // bitPosition: 1
        bitWriter.Array[1].Should().Be(0b_1111_1110); // bitPosition: 8
    }

    [Fact]
    public void SetAt_SetFalseAtPositionInTheBuffer_BitShouldBeSet()
    {
        var bitWriter = new BitWriter(new byte[9]);
        bitWriter.Write(uint.MaxValue);
        bitWriter.Write(ushort.MaxValue);
        bitWriter.BitsCount.Should().Be(48);

        bitWriter.SetAt(1, false);
        bitWriter.SetAt(8, false);
        bitWriter.BitsCount.Should().Be(48);

        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(6);
        bitWriter.BitsCount.Should().Be(48);

        bitWriter.Array[0].Should().Be(0b_1111_1101); // bitPosition: 1
        bitWriter.Array[1].Should().Be(0b_1111_1110); // bitPosition: 8
    }

    [Fact]
    public void SetAt_SetAtBitCountPosition_ShouldSet()
    {
        var bitWriter = new BitWriter(new byte[9]);
        bitWriter.Write(true);
        bitWriter.Write(uint.MaxValue);

        bitWriter.SetAt(0, false);

        bitWriter.Flush();

        bitWriter.Array[0].Should().Be(0b_1111_1110);
        bitWriter.Array[1].Should().Be(0b_1111_1111);
        bitWriter.Array[2].Should().Be(0b_1111_1111);
        bitWriter.Array[3].Should().Be(0b_1111_1111);
        bitWriter.Array[4].Should().Be(0b_0000_0001);
    }

    [Fact]
    public void Position_ZeroOffset_ShouldBeValid()
    {
        var bitWriter = new BitWriter();
        bitWriter.SetArray(new byte[5], 0);

        bitWriter.Head.Bytes.Should().Be(0);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(0);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(0);

        bitWriter.WriteBits(1, 1);

        bitWriter.Head.Bytes.Should().Be(0);
        bitWriter.Head.Bits.Should().Be(1);
        bitWriter.Position.Bytes.Should().Be(0);
        bitWriter.Position.Bits.Should().Be(1);
        bitWriter.BitsPosition.Should().Be(1);

        bitWriter.WriteBits(6, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(0);
        bitWriter.Head.Bits.Should().Be(7);
        bitWriter.Position.Bytes.Should().Be(0);
        bitWriter.Position.Bits.Should().Be(7);
        bitWriter.BitsPosition.Should().Be(7);

        bitWriter.WriteBits(1, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(1);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(1);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(8);

        bitWriter.WriteBits(16 + 7, uint.MaxValue); // remains 1 bit in the buffer

        bitWriter.Head.Bytes.Should().Be(3);
        bitWriter.Head.Bits.Should().Be(7);
        bitWriter.Position.Bytes.Should().Be(3);
        bitWriter.Position.Bits.Should().Be(7);
        bitWriter.BitsPosition.Should().Be(31);

        bitWriter.WriteBits(1, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(4);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(4);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(32);

        bitWriter.WriteBits(1, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(4);
        bitWriter.Head.Bits.Should().Be(1);
        bitWriter.Position.Bytes.Should().Be(4);
        bitWriter.Position.Bits.Should().Be(1);
        bitWriter.BitsPosition.Should().Be(33);

        bitWriter.WriteBits(7, uint.MaxValue); // remains 0 bits in array

        bitWriter.Head.Bytes.Should().Be(5);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(5);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(40);
    }

    [Fact]
    public void Position_NonZeroOffset_ShouldBeValid()
    {
        var bitWriter = new BitWriter();
        bitWriter.SetArray(new byte[5], 1);

        bitWriter.Head.Bytes.Should().Be(1);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(0);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(0);

        bitWriter.WriteBits(1, 1);

        bitWriter.Head.Bytes.Should().Be(1);
        bitWriter.Head.Bits.Should().Be(1);
        bitWriter.Position.Bytes.Should().Be(0);
        bitWriter.Position.Bits.Should().Be(1);
        bitWriter.BitsPosition.Should().Be(1);

        bitWriter.WriteBits(6, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(1);
        bitWriter.Head.Bits.Should().Be(7);
        bitWriter.Position.Bytes.Should().Be(0);
        bitWriter.Position.Bits.Should().Be(7);
        bitWriter.BitsPosition.Should().Be(7);

        bitWriter.WriteBits(1, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(2);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(1);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(8);

        bitWriter.WriteBits(16 + 7, uint.MaxValue); // remains 1 bit in the buffer

        bitWriter.Head.Bytes.Should().Be(4);
        bitWriter.Head.Bits.Should().Be(7);
        bitWriter.Position.Bytes.Should().Be(3);
        bitWriter.Position.Bits.Should().Be(7);
        bitWriter.BitsPosition.Should().Be(31);

        bitWriter.WriteBits(1, uint.MaxValue);

        bitWriter.Head.Bytes.Should().Be(5);
        bitWriter.Head.Bits.Should().Be(0);
        bitWriter.Position.Bytes.Should().Be(4);
        bitWriter.Position.Bits.Should().Be(0);
        bitWriter.BitsPosition.Should().Be(32);
    }
}