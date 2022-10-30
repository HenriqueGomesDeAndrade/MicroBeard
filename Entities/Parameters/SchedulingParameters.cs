namespace MicroBeard.Entities.Parameters
{
    public class SchedulingParameters :QueryStringParameters
    {
        public SchedulingParameters()
        {
            OrderBy = "Date";
        }
        public int? ServiceCode { get; set; }
        public int? ContactCode { get; set; }
        public int? DateDay { get; set; }
        public int? DateMonth { get; set; }
        public int? DateYear { get; set; }
    }
}
