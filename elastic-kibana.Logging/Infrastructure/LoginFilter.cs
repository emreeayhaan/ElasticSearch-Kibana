using elastic_kibana.Logging.Interface;
using ElasticSearch.Core.Configuration;
using ElasticSearch.Core.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace elastic_kibana.Logging.Infrastructure
{
    public class LoginFilter
    {
        private readonly IElasticSearchService<LoginLogModel>? _elasticSearchService;
        Microsoft.Extensions.Options.IOptions<ElasticConnectionSettings>? _elasticConfig;
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (HasLogAttribute(context))
            {
                //Alınan Model Kaydedilecek
                string action = (string)context.RouteData.Values["action"];
                string controller = (string)context.RouteData.Values["controller"];
                int.TryParse(context.HttpContext.Request.Headers["UserId"].FirstOrDefault(), out var userId);
                if (userId != 0)
                {
                    //Elastic Log-----
                    LoginLogModel logModel = new LoginLogModel();
                    logModel.Action = action;
                    logModel.Controller = controller;
                    logModel.PostDate = DateTime.Now;
                    logModel.UserID = userId.ToString();
                    _elasticSearchService.CheckExistsAndInsertLog(logModel, _elasticConfig.Value.ElasticLoginIndex);
                    return;
                }
            }
        }

        public bool HasLogAttribute(FilterContext context)
        {
            return ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes.Any(filterDescriptors => filterDescriptors.AttributeType == typeof(LogAttribute));
        }
    }
}