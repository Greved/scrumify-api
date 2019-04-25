using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greved.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrumify.Api.Business.ReportDefinition.Save;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.Api.Client.Models.ReportDefinition.List;
using Scrumify.DataAccess;

namespace Scrumify.Api.Controllers
{
    [Route("api/report-definition")]
    public class ReportDefinitionController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IReportDefinitionRepository reportDefinitionRepository;

        public ReportDefinitionController(IMediator mediator, IReportDefinitionRepository reportDefinitionRepository)
        {
            this.mediator = mediator;
            this.reportDefinitionRepository = reportDefinitionRepository;
        }

        // GET: api/report-definition
        [HttpGet]
        public async Task<IEnumerable<ReportDefinitionListItemDto>> Get(CancellationToken cancellationToken)
        {
            var storedListItems = await reportDefinitionRepository.ReadAsync(cancellationToken);
            var clientListItems = storedListItems
                .Select(x => new ReportDefinitionListItemDto{Id = x.Id, Name = x.Name})
                .ToList(storedListItems.Count);
            return clientListItems;
        }

        // PUT: api/report-definition
        [HttpPut]
        public async Task<ActionResult<string>> Save([FromBody]ReportDefinitionDto reportDefinition, CancellationToken cancellationToken)
        {
            var command = new SaveReportDefinitionCommand(reportDefinition);
            var definitionId = await mediator.Send(command, cancellationToken);
            return Ok(definitionId);
        }
    }
}