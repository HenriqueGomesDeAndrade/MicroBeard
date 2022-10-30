namespace MicroBeard.Entities.Parameters
{
    public class CollaboratorParameters : QueryStringParameters
    {
        public CollaboratorParameters()
        {
            OrderBy = "Name";
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
    }
}
