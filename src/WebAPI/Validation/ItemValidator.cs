using FluentValidation;
using WebAPI.Models;

namespace WebAPI.Validation
{
    public class ItemValidator : AbstractValidator<ItemRequestModel>
    {
        private readonly string[] allowedFileExtensions = [".jpg", ".jpeg", ".png", ".svg"];
        public ItemValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("Text must be filled");
            RuleFor(x => x.File).Must(x => allowedFileExtensions.Contains(Path.GetExtension(x.FileName))).WithMessage("Unsupported extension");
        }
    }
}
