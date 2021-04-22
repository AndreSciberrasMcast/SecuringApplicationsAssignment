using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.Services;
using SecuringApplicationsAssignment.Data.Context;
using SecuringApplicationsAssignment.Data.Repositories;
using SecuringApplicationsAssignment.Domain.Interfaces;
namespace SecuringApplicationsAssignment.IOC
{
    public class DependancyContainer
    {
        public static void RegisterServices(IServiceCollection services, string connectionString)
        {

            services.AddDbContext<MyDatabaseContext>(options =>
                options.UseSqlServer(
                    connectionString
                   ));

            services.AddScoped<IAssignmentsRepository, AssignmentsRepository>();
            services.AddScoped<IAssignmentsService, AssignmentsService>();

            services.AddScoped<ISubmissionsRepository, SubmissionsRepository>();

        }
    }
}
