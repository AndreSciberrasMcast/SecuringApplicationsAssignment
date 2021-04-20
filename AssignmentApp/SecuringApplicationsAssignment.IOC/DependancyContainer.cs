using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace SecuringApplicationsAssignment.IOC
{
    public class DependancyContainer
    {
        public static void RegisterServices(IServiceCollection services, string connectionString)
        {
            /*
            services.AddDbContext<TaskExchangeDBContext>(options =>
                options.UseSqlServer(
                   connectionString));
            )*/

           
        }
    }
}
