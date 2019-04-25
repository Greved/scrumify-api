using System;
using System.Linq;
using Greved.Core;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.DataAccess.Models;

namespace Scrumify.Api.Business.ReportDefinition
{
    public static class ReportDefinitionEntityConverter
    {
        public static DataAccess.Models.ReportDefinition ToStored(ReportDefinitionDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new DataAccess.Models.ReportDefinition
            {
                Id = dto.Id,
                Name = dto.Name,
                Items = dto.Items?.Select(ToStoredReportDefinitionItem).ToList(dto.Items.Count)
            };
        }

        private static ReportDefinitionItem ToStoredReportDefinitionItem(ReportDefinitionItemDto dto)
        {
            return new ReportDefinitionItem
            {
                Order = dto.Order,
                Group = ToStoredReportDefinitionQuestionGroup(dto.Group),
                Question = ToStored(dto.Question),
            };
        }

        private static ReportDefinitionQuestionGroup ToStoredReportDefinitionQuestionGroup(
            ReportDefinitionQuestionGroupDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new ReportDefinitionQuestionGroup
            {
                Id = dto.Id,
                Name = dto.Name,
                Questions = dto.Questions?.Select(ToStored).ToList(dto.Questions.Count),
            };
        }

        private static ReportDefinitionQuestion ToStored(ReportDefinitionQuestionDto question)
        {
            if (question == null)
            {
                return null;
            }

            return new ReportDefinitionQuestion
            {
                Id = question.Id,
                Text = question.Text,
                Type = ToStored(question.Type),
                Options = question.Options?.Select(option => new ReportDefinitionQuestionOption
                {
                    Id = option.Id,
                    Text = option.Text
                }).ToList(question.Options.Count)
            };
        }

        private static ReportDefinitionQuestionType ToStored(ReportDefinitionQuestionTypeDto dto)
        {
            switch (dto)
            {
                case ReportDefinitionQuestionTypeDto.Text:
                    return ReportDefinitionQuestionType.Text;
                case ReportDefinitionQuestionTypeDto.Poll:
                    return ReportDefinitionQuestionType.Poll;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dto), dto, null);
            }
        }
    }
}