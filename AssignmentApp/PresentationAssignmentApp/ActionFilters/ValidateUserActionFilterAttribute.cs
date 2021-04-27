using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecuringApplicationsAssignment.Application.Interfaces;
using System;

namespace PresentationAssignmentApp.ActionFilters
{
    public class ValidateUserActionFilterAttribute : ActionFilterAttribute
    {  

        public override void OnActionExecuting(ActionExecutingContext context)
        {


            try
            {
                var id = new Guid(context.ActionArguments["id"].ToString());
                var loggedInUser = context.HttpContext.User.Identity.Name;

                IAssignmentsService assignmentsService = (IAssignmentsService)context.HttpContext.RequestServices.GetService(typeof(IAssignmentsService));


                if (loggedInUser != assignmentsService.GetSubmission(id).Member.Email && !context.HttpContext.User.IsInRole("Teacher"))
                {
                    context.Result = new UnauthorizedObjectResult("Access Denied");
                }

            }
            catch (Exception ex)
            {
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
