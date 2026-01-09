using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDos.Models
{
    public class TodosVM
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public string? Commentaire { get; set; }
        public DateTime Date_planif { get; set; }
        public DateTime? Date_realisation { get; set; }

        // Différence en jours : real - planif
        // -3 => 3 jours d'avance ; +2 => 2 jours de retard ; 0 => à temps ; null => en cours
        [NotMapped]
        public int? DeltaJours
        {
            get
            {
                if (!Date_realisation.HasValue)
                    return null;

                var planif = Date_planif.Date;
                var real = Date_realisation.Value.Date;

                return (real - planif).Days;
            }
        }

        [NotMapped]
        public string Statut
        {
            get
            {
                if (!Date_realisation.HasValue)
                    return "En cours";

                var delta = DeltaJours ?? 0;

                if (delta < 0) return "En avance";
                if (delta > 0) return "En retard";
                return "À temps";
            }
        }

        [NotMapped]
        public int DureeAvanceJours => DeltaJours.HasValue && DeltaJours.Value < 0 ? -DeltaJours.Value : 0;

        [NotMapped]
        public int DureeRetardJours => DeltaJours.HasValue && DeltaJours.Value > 0 ? DeltaJours.Value : 0;

        // Optionnel : affichage ready-to-use
        [NotMapped]
        public string DureeAvance => DureeAvanceJours > 0 ? $"{DureeAvanceJours} jour(s)" : "";

        [NotMapped]
        public string DureeRetard => DureeRetardJours > 0 ? $"{DureeRetardJours} jour(s)" : "";
    }
}
