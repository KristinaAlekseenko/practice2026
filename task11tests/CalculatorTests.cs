using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests
{
    [Fact]
    public void Calculator_Add_ShouldReturnCorrectSum()
    {
        var calc = CalculatorGenerator.CreateCalculator();
        var wrapper = new CalculatorWrapper(calc);

        int result = wrapper.Add(5, 3);

        Assert.Equal(8, result);
    }

    [Fact]
    public void Calculator_Minus_ShouldReturnCorrectDifference()
    {
        var calc = CalculatorGenerator.CreateCalculator();
        var wrapper = new CalculatorWrapper(calc);

        int result = wrapper.Minus(10, 4);

        Assert.Equal(6, result);
    }

    [Fact]
    public void Calculator_Mul_ShouldReturnCorrectProduct()
    {
        var calc = CalculatorGenerator.CreateCalculator();
        var wrapper = new CalculatorWrapper(calc);

        int result = wrapper.Mul(3, 4);

        Assert.Equal(12, result);
    }

    [Fact]
    public void Calculator_Div_ShouldReturnCorrectQuotient()
    {
        var calc = CalculatorGenerator.CreateCalculator();
        var wrapper = new CalculatorWrapper(calc);

        int result = wrapper.Div(15, 3);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Calculator_Div_ShouldThrowOnDivisionByZero()
    {
        var calc = CalculatorGenerator.CreateCalculator();
        var wrapper = new CalculatorWrapper(calc);

        Assert.Throws<System.DivideByZeroException>(() => wrapper.Div(5, 0));
    }

    [Fact]
    public void Calculator_CreateCalculator_ShouldNotReturnNull()
    {
        var calc = CalculatorGenerator.CreateCalculator();

        Assert.NotNull(calc);
    }
}