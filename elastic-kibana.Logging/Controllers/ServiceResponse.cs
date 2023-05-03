namespace elastic_kibana.Logging.Controllers
{
    public class ServiceResponse<T>
    {
        public IList<T> List { get; set; }

        public int Entity { get; set; }

        public bool IsSuccessful { get; set; }
        public ServiceResponse(HttpContext context)
        {
            List = new List<T>();
        }
    }
}