﻿using AutoMapper;
using ContosoUniversity_Blazor.Data;
using ContosoUniversity_Blazor.Extensions;
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
    public class IndexBase : ComponentBase
    {
        private readonly IMediator _mediator;
        public IndexBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Result Data { get; private set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync() => Data = await _mediator.Send(new Query());

        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Course> Courses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public int Credits { get; set; }
                public string DepartmentName { get; set; }
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

            public async Task<Result> Handle(Query message, CancellationToken token)
            {
                var courses = await _db.Courses
                    .OrderBy(d => d.Id)
                    .ProjectToListAsync<Result.Course>(_configuration);

                return new Result
                {
                    Courses = courses
                };
            }
        }
    }
}
