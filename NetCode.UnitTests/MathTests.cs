using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class MathTests
{
    [Fact]
    public void BitsRequired()
    {
        Mathi.BitsRequired(0).Should().Be(1); // 1 bit: 0, 1
        Mathi.BitsRequired(1).Should().Be(1); // 1 bits: 0, 1
        Mathi.BitsRequired(2).Should().Be(2); // 2 bits: [0, 3]
        Mathi.BitsRequired(3).Should().Be(2); // 2 bits: [0, 3]
        Mathi.BitsRequired(4).Should().Be(3); // 3 bits: [0, 7]
        Mathi.BitsRequired(7).Should().Be(3); // 3 bits: [0, 7]
        Mathi.BitsRequired(8).Should().Be(4); // 3 bits: [0, 15]
        Mathi.BitsRequired(15).Should().Be(4); // 3 bits: [0, 15]
        Mathi.BitsRequired(16).Should().Be(5); // 3 bits: [0, 31]
        Mathi.BitsRequired(uint.MaxValue / 2).Should().Be(31);
        Mathi.BitsRequired(uint.MaxValue).Should().Be(32);
    }
}