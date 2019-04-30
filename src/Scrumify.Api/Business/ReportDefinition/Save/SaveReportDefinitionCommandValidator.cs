using FluentValidation;
using FluentValidation.Results;
using MongoDB.Bson;

namespace Scrumify.Api.Business.ReportDefinition.Save
{
    public class SaveReportDefinitionCommandValidator: AbstractValidator<SaveReportDefinitionCommand>
    {
        public SaveReportDefinitionCommandValidator()
        {
            RuleFor(command => command.ReportDefinition).NotNull().WithMessage("Definition shouldn't be null");
            When(command => command.ReportDefinition != null, () =>
            {
                RuleFor(command => command.ReportDefinition.Name).NotEmpty().WithMessage("Definition's name shouldn't be empty");
                RuleFor(command => command.ReportDefinition.Id).Must(id => ObjectId.TryParse(id, out _)).WithMessage("Definition's Id should be in ObjectId format");
                RuleFor(command => command.ReportDefinition.Items.Count).GreaterThan(0).WithMessage("Definition should have at least one item");
            });
        }
    }
}