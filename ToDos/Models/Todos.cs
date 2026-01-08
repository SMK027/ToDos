using System.ComponentModel.DataAnnotations.Schema;

namespace ToDos.Models
{
    public class TodosVM
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public string? Commentaire { get; set; }
        public DateTime Date_planif {  get; set; }
        public DateTime? Date_realisation { get; set; }

        [NotMapped]
        public string Statut
        {
            get
            {
                if (!Date_realisation.HasValue)
                    return "En cours";

                var planif = Date_planif.Date;
                var real = Date_realisation.Value.Date;

                if (real < planif) return "En avance";
                if (real > planif) return "En retard";
                return "Terminée à temps";
            }
        }
    }
}
