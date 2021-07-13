using FluentValidation;

namespace WebApi.DTOs.Directors.POST
{
    public class DirectorInputPostDTO
    {
        public string Name { get; set; }
    }

    public class DirectorInputPostDTOValidator : AbstractValidator<DirectorInputPostDTO>
    {
        public DirectorInputPostDTOValidator()
        {
            RuleFor(director => director.Name).NotNull().NotEmpty();
            RuleFor(director => director.Name).Length(2, 100).WithMessage("Lenght {TotalLength} is invalid.");
        }
    }
}