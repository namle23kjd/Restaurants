using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Command.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Test",
                Category = "Italian",
                ContactEmail = "test@test.com",
                PostalCode = "12-123",
            };

            var validator = new CreateRestaurantCommandValidator();

            // act

            var result = validator.TestValidate(command);

            //aser
            result.ShouldNotHaveAnyValidationErrors();



        }


        [Fact()]
        public void Validator_ForValidCommand_ShouldHaveValidationErrors()
        {
            // arrange
            var command = new CreateRestaurantCommand()
            {
                Name = "Te",
                Category = "Ital",
                ContactEmail = "testest.com",
                PostalCode = "123123",
            };

            var validator = new CreateRestaurantCommandValidator();

            // act

            var result = validator.TestValidate(command);

            //aser
            result.ShouldHaveValidationErrorFor(c => c.Name);
            result.ShouldHaveValidationErrorFor(c => c.Category);
            result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

        [Theory()]
        [InlineData("Italian")]
        [InlineData("Mexico")]
        [InlineData("Japanese")]
        [InlineData("American")]
        [InlineData("Ididan")]
        public void Validator_ForValidCategory_SHouldNotHaveCalidationErrorsForCategoryProperty(string category)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand { Category = category };

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldNotHaveValidationErrorFor(c => c.Category);
        }

        [Theory()]
        [InlineData("102020")]
        [InlineData("10-2020")]
        [InlineData("10-20")]
        [InlineData("10-2")]
        [InlineData("10-")]
        public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
        {
            // arrange
            var validator = new CreateRestaurantCommandValidator();
            var command = new CreateRestaurantCommand { PostalCode = postalCode };

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }
            
    }
}