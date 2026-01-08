namespace ToDos.Models
{
    public class TodoFilter
    {
        public string? Search { get; set; }          // recherche Libellé/Commentaire
        public bool? IsDone { get; set; }            // null = tous, true = faits, false = non faits

        public DateTime? PlanifFrom { get; set; }
        public DateTime? PlanifTo { get; set; }

        public DateTime? DoneFrom { get; set; }
        public DateTime? DoneTo { get; set; }
    }
}
