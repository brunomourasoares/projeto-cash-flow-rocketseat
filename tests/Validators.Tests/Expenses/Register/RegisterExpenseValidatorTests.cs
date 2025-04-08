using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpensiveJsonBuilder.Builder();
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        //Assert.True(result.IsValid);
        result.IsValid.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_Invalid(decimal amount)
    {
        // Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpensiveJsonBuilder.Builder();
        request.Amount = amount;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUT_MUST_BE_GREATER_THAN_ZERO);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title) 
    {
        // Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpensiveJsonBuilder.Builder();
        request.Title = title;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED);
    }
    
    [Fact]
    public void Error_Date_Future() 
    {
        // Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpensiveJsonBuilder.Builder();
        request.Date = DateTime.UtcNow.AddDays(1);
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE);
    }
    
    [Fact]
    public void Error_Payment_Type_Invalid() 
    {
        // Arrange
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpensiveJsonBuilder.Builder();
        request.PaymentType = (PaymentType)700;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_PAYMENT_TYPE);
    }
}