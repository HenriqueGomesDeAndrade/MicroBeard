namespace MicroBeard.Entities.Parameters
{
    public class ContactParameters : QueryStringParameters
    {
        public ContactParameters()
        {
            OrderBy = "Name";
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
    }
}
