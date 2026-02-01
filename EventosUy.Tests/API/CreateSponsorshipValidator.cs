using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Domain.Enumerates;
using FluentValidation.TestHelper;

namespace EventosUy.Tests.API
{
    public class CreateSponsorshipValidator
    {
        private readonly SponsorshipInsertValidator _validator;

        public CreateSponsorshipValidator()
        {
            _validator = new SponsorshipInsertValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Name_Is_Invalid(string name) 
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: name,
                    tier: SponsorshipTier.BRONZE,
                    amount: 1_000m,
                    institutionId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    voucherName: "voucher",
                    voucherCode: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate( model );

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Amount_Is_Invalid(decimal amount)
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: "name",
                    tier: SponsorshipTier.BRONZE,
                    amount: amount,
                    institutionId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    voucherName: "voucher",
                    voucherCode: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [Fact]
        public void Should_Have_Error_When_Institution_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: "name",
                    tier: SponsorshipTier.BRONZE,
                    amount: 0m,
                    institutionId: Guid.Empty,
                    registerTypeId: Guid.NewGuid(),
                    voucherName: "voucher",
                    voucherCode: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Institution);
        }

        [Fact]
        public void Should_Have_Error_When_Register_Type_Is_Invalid()
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: "name",
                    tier: SponsorshipTier.BRONZE,
                    amount: 0m,
                    institutionId: Guid.NewGuid(),
                    registerTypeId: Guid.Empty,
                    voucherName: "voucher",
                    voucherCode: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RegisterType);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Have_Error_When_Voucher_Name_Is_Invalid(string name)
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: "name",
                    tier: SponsorshipTier.BRONZE,
                    amount: 1_000m,
                    institutionId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    voucherName: name,
                    voucherCode: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("CODE123")]
        public void Should_Have_Error_When_Voucher_Code_Is_Invalid(string code)
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: "name",
                    tier: SponsorshipTier.BRONZE,
                    amount: 1_000m,
                    institutionId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    voucherName: "voucher",
                    voucherCode: code
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.VoucherCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            // Arrange
            var model = new DTInsertSponsorship(
                    name: "name",
                    tier: SponsorshipTier.BRONZE,
                    amount: 1_000m,
                    institutionId: Guid.NewGuid(),
                    registerTypeId: Guid.NewGuid(),
                    voucherName: "voucher",
                    voucherCode: "codeCODE"
                );

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
