using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Application.Behaviors;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(sc =>
            {
                sc.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                sc.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}