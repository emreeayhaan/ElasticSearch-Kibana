using Amazon.CloudTrail.Model;
using elastic_kibana.Logging.Interface;
using ElasticSearch.Core.Configuration;
using ElasticSearch.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace elastic_kibana.Logging.Infrastructure
{
    public class LoginLogFilter : IActionFilter
    {
        private readonly IElasticSearchService<LoginLogModel> _elasticSearchService;
        Microsoft.Extensions.Options.IOptions<ElasticConnectionSettings> _elasticConfig;
        public LoginLogFilter(IElasticSearchService<LoginLogModel> elasticSearchService, Microsoft.Extensions.Options.IOptions<ElasticConnectionSettings> elasticConfig)
        {
            _elasticSearchService = elasticSearchService;
            _elasticConfig = elasticConfig;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string action = (string)context.RouteData.Values["action"];
            string controller = (string)context.RouteData.Values["controller"];
            var result = context.Result;

            string userID = String.Empty;
            if (result.GetType() == typeof(UnauthorizedResult))
            {
                return;
            }
            

            //Insert ElasticSearch
            LoginLogModel logModel = new LoginLogModel();
            logModel.Action = action;
            logModel.Controller = controller;
            logModel.PostDate = DateTime.Now;
            logModel.UserID = userID;
            _elasticSearchService.CheckExistsAndInsertLog(logModel, _elasticConfig.Value.ElasticLoginIndex);
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try { }
            catch (InvalidTokenException ex)
            {
                //Forbidden 430 Result. Yetkiniz Yoktur..
                context.Result = new ObjectResult(context.ModelState)
                {
                    Value = "Geçerli Bir Token Girilmemiştir",
                    StatusCode = 430
                };
                return;
            }

        }
    }
}
