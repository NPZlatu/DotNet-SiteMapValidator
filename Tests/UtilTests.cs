using Xunit;
namespace Tests;

public class UtilTest
{
    [Fact]
    public void DumpObject_ShouldSerializeAndPrintObject()
    {
        // Arrange
        var obj = new { Name = "Test", Value = 123 };
        var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        Util.Dump(obj);

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("{\"Name\":\"Test\",\"Value\":123}", output);
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("123456")]
    [InlineData("")]
    [InlineData("SpecialCharacters!@#")]
    public void DumpObject_ShouldSerializeAndPrintString(string input)
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        Util.Dump(input);

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal(input, output);
    }

    [Fact]
    public void DumpObject_ShouldPrintNull_WhenInputObjectIsNull()
    {
        // Arrange
        object input = null;
        var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        Util.Dump(input);

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("null", output);
    }

}
