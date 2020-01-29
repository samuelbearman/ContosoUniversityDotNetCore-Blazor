using AutoMapper;
using ContosoUniversity_Blazor.Data;
using ContosoUniversity_Blazor.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoUniversity_Blazor.Pages.Courses
{
    public class CreateBase : ComponentBase
    {
        [Inject]
        IMediator _mediator { get; set; }

        [Inject]
        NavigationManager _navigationManager { get; set; }

        public Command Data { get; set; } = new Command();

        public async Task Create_Click()
        {
            var result = await _mediator.Send(Data);

            _navigationManager.NavigateTo($"/Courses/{result}");
        }

        public class Command : IRequest<int>
        {
            [IgnoreMap]
            public int Number { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            public Department Department { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() =>
                CreateMap<Command, Course>(MemberList.Source);
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly SchoolContext _db;
            private readonly IMapper _mapper;

            public Handler(SchoolContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command message, CancellationToken token)
            {
                var course = _mapper.Map<Command, Course>(message);
                course.Id = message.Number;

                _db.Courses.Add(course);

                await _db.SaveChangesAsync(token);

                return course.Id;
            }
        }
    }
}
