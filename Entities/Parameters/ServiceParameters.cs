namespace MicroBeard.Entities.Parameters
{
    public class ServiceParameters : QueryStringParameters
    {
        public ServiceParameters()
        {
            OrderBy = "Name";
        }
        public string Name { get; set; }
    }
}
