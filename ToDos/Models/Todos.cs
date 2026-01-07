namespace ToDos.Models
{
    public class TodosVM
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public string? Commentaire { get; set; }
        public DateTime Date_planif {  get; set; }
        public DateTime? Date_realisation { get; set; }
    }
}
