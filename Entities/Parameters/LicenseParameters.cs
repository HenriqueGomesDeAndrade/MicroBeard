namespace MicroBeard.Entities.Parameters
{
    public class LicenseParameters : QueryStringParameters
    {
        public LicenseParameters()
        {
            OrderBy = "Description";
        }
        public string Description { get; set; }
    }
}
