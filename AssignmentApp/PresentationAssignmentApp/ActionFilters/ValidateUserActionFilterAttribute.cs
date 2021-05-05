using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PresentationAssignmentApp.Controllers;
using PresentationAssignmentApp.Helpers;
using SecuringApplicationsAssignment.Application.Interfaces;
using System;

namespace PresentationAssignmentApp.ActionFilters
{
    public class ValidateUserActionFilterAttribute : ActionFilterAttribute
    {  

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AssignmentsController controller = (AssignmentsController)context.Controller;
            ILogger logger = controller.GetLogger();

            try
            {
                

                var id = Guid.Parse(CryptographicHelper.SymmetricDecrypt(context.ActionArguments["id"].ToString()));
                var loggedInUser = context.HttpContext.User.Identity.Name;

                IAssignmentsService assignmentsService = (IAssignmentsService)context.HttpContext.RequestServices.GetService(typeof(IAssignmentsService));


                if (loggedInUser != assignmentsService.GetSubmission(id).Member.Email && !context.HttpContext.User.IsInRole("Teacher"))
                {
                    logger.LogInformation(loggedInUser + " tried to access submission with id " + id + ". Access was denied");
                    
                    context.Result = new UnauthorizedObjectResult("Access Denied");
                }

            }
            catch (Exception ex)
            {
                logger.LogInformation("Bad request when user tried to access a file");
                context.Result = new BadRequestObjectResult("Bad Request");
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {


            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
        
    }
}
