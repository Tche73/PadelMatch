using System.ComponentModel.DataAnnotations;

namespace PadelMatchBlazor.Models.Requests
{
    public class ReservationRequest
    {
        [Required(ErrorMessage = "Le terrain est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un terrain")]
        public int CourtId { get; set; }

        [Required(ErrorMessage = "La date et l'heure de début sont obligatoires")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "La date et l'heure de fin sont obligatoires")]
        public DateTime EndTime { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
