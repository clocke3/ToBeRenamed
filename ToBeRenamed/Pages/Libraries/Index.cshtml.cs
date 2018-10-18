﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Libraries
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<LibraryDto> Libraries { get; set; }

        public async Task OnGet()
        {
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));
            Libraries = await _mediator.Send(new GetLibrariesForUser(userDto.Id));
        }
    }
}