using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecuringApplicationsAssignment.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PresentationAssignmentApp.ActionFilters
{
    public class  CommentValidationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                ViewSubmissionViewModel comment = (ViewSubmissionViewModel)context.ActionArguments["data"];
                comment.Comment.Data = HtmlEncoder.Default.Encode(comment.Comment.Data);

                //IAssignmentsService assignmentsService = (IAssignmentsService)context.HttpContext.RequestServices.GetService(typeof(IAssignmentsService));


                //if (loggedInUser != assignmentsService.GetSubmission(id).Member.Email || !context.HttpContext.User.IsInRole("Teacher"))
                //{
                // context.Result = new UnauthorizedObjectResult("Access Denied");
                //}
                

            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult("Bad Request");
            }
        }
    }
}
