using AutoMapper;
using ContosoUniversity.Blazor.Data;
using ContosoUniversity.Blazor.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoUniversity.Blazor.Pages.Courses
{
    public class IndexBase : ComponentBase
    {

        [Inject]
        protected IMediator Mediator { get; set; }

        public Result Data { get; private set; }

        protected async override Task OnInitializedAsync() => Data = await Mediator.Send(new Query());

        public record Query : IRequest<Result>
        {
        }

        public record Result
        {
            public List<Course> Courses { get; init; }

            public record Course
            {
                public int Id { get; init; }
                public string Title { get; init; }
                public int Credits { get; init; }
                public string DepartmentName { get; init; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Course, Result.Course>();
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly SchoolContext _db;
            private readonly IConfigurationProvider _configuration;

            public Handler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public Task<Result> Handle(Query message, CancellationToken token)
            {
                var courses = new List<Result.Course>
                {
                    new Result.Course()
                    { 
                        Id = 1,
                        Title = "English 101",
                        Credits = 3,
                        DepartmentName = "English"
                    }
                };

                //var courses = await _db.Courses
                //    .OrderBy(d => d.Id)
                //    .ProjectToListAsync<Result.Course>(_configuration);

                return Task.FromResult(new Result
                {
                    Courses = courses
                });
            }
        }
    }
}
