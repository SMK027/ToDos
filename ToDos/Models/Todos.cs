using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDos.Models
{
    public class TodosVM : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le libellé est obligatoire.")]
        public string Libelle { get; set; } = string.Empty;

        public string? Commentaire { get; set; }

        [Required(ErrorMessage = "La date planifiée est obligatoire.")]
        public DateTime Date_planif { get; set; }

        public DateTime? Date_realisation { get; set; }

        // =============================
        // Règles Date_planif
        // =============================

        [NotMapped]
        public bool IsPlanifInPast => Date_planif.Date < DateTime.Today;

        // Warning seulement si c'est dans le passé, mais pas au-delà d'une semaine
        [NotMapped]
        public bool ShouldWarnPlanifInPastButAllowed =>
            IsPlanifInPast && Date_planif.Date >= DateTime.Today.AddDays(-7);

        [NotMapped]
        public string? PlanifWarningMessage =>
            ShouldWarnPlanifInPastButAllowed
                ? "Attention : la date planifiée est antérieure à aujourd'hui."
                : null;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var minAllowed = DateTime.Today.AddDays(-7);

            if (Date_planif.Date < minAllowed)
            {
                yield return new ValidationResult(
                    "La date planifiée ne peut pas être antérieure à plus d'une semaine avant aujourd'hui.",
                    new[] { nameof(Date_planif) }
                );
            }
        }

        // =============================
        // Statut / Durées (inchangé)
        // =============================

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
    }
}
