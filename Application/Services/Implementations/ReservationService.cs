using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Cancel(int id)
        {
            var reservation = _unitOfWork.Reservations.GetById(id);
            if (reservation == null)
            {
                throw new InvalidOperationException("La réservation spécifiée n'existe pas");
            }
            // Empêcher l'annulation des réservations déjà terminées
            if (reservation.Status == ReservationStatus.Completed)
                throw new InvalidOperationException("Impossible d'annuler une réservation déjà terminée.");

            reservation.Status = ReservationStatus.Cancelled;
            _unitOfWork.Complete();
        }

        public void ChangeStatus(int id, ReservationStatus status)
        {
            var reservation = _unitOfWork.Reservations.GetById(id);
            if (reservation == null)
            {
                throw new InvalidOperationException("La réservation spécifiée n'existe pas");
            }

            if (reservation.Status == ReservationStatus.Cancelled &&  status != ReservationStatus.Cancelled)
            {
                throw new InvalidOperationException("Impossible de changer le statut d'une réservation annulée.");
            }

            if (reservation.Status != ReservationStatus.Completed && status == ReservationStatus.Completed)
            {
                throw new InvalidOperationException("Impossible de changer le statut d'une réservation terminée.");
            }

            reservation.Status = status;
            _unitOfWork.Complete();

        }

        public void Create(Reservation reservation)
        {
            // Vérifier que le terrain existe
            var court = _unitOfWork.Courts.GetById(reservation.CourtId);
            if (court == null)
                throw new InvalidOperationException("Le terrain spécifié n'existe pas.");

            if (!court.IsActive)
                throw new InvalidOperationException("Le terrain spécifié n'est pas disponible.");

            // Vérifier que l'utilisateur existe
            var user = _unitOfWork.Users.GetById(reservation.CreatedBy);
            if (user == null)
                throw new InvalidOperationException("L'utilisateur spécifié n'existe pas.");

            // Vérifier la validité de la période
            if (reservation.StartDateTime >= reservation.EndDateTime)
                throw new InvalidOperationException("La date de fin doit être postérieure à la date de début.");

            // Vérifier que la date de début n'est pas dans le passé
            if (reservation.StartDateTime < DateTime.Now)
            {
                throw new InvalidOperationException("Impossible de créer une réservation pour une date passée.");
            }

            // Vérifier qu'il n'y a pas de chevauchement avec d'autres réservations
            var existingReservations = _unitOfWork.Reservations
                .GetByDateRange(reservation.StartDateTime, reservation.EndDateTime)
                .Where(r => r.CourtId == reservation.CourtId && r.Status != ReservationStatus.Cancelled)
                .ToList();

            if (existingReservations.Any(r =>
                (r.StartDateTime < reservation.EndDateTime && r.EndDateTime > reservation.StartDateTime)))
            {
                throw new InvalidOperationException("Le créneau horaire est déjà réservé.");
            }

            // Définir les valeurs par défaut
            reservation.CreatedAt = DateTime.Now;
            reservation.Status = ReservationStatus.Confirmed; // Ou Pending selon votre logique

            // Ajouter la réservation
            _unitOfWork.Reservations.Add(reservation);
            _unitOfWork.Complete();
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _unitOfWork.Reservations.GetAll().Where(r => r.Status != ReservationStatus.Cancelled);
        }
        public IEnumerable<Reservation> GetAvailableTimeSlots(int courtId, DateTime date)
        {
            var openingTime = new TimeSpan(8, 0, 0);
            var closingTime = new TimeSpan(22, 0, 0);

            // Durée standard d'une réservation (par exemple, 1 heure)
            var slotDuration = TimeSpan.FromHours(1);

            // Récupérer toutes les réservations pour ce terrain à cette date
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);
            var existingReservations = _unitOfWork.Reservations
                .GetByDateRange(startOfDay, endOfDay)
                .Where(r => r.CourtId == courtId && r.Status != ReservationStatus.Cancelled)
                .ToList();

            // Créer une liste de tous les créneaux horaires disponibles
            var availableSlots = new List<Reservation>();
            for (var time = openingTime; time < closingTime; time = time.Add(slotDuration))
            {
                var slotStart = date.Date.Add(time);
                var slotEnd = slotStart.Add(slotDuration);

                // Vérifier si ce créneau est disponible (non réservé)
                var isSlotAvailable = !existingReservations.Any(r =>
                    (r.StartDateTime < slotEnd && r.EndDateTime > slotStart));

                if (isSlotAvailable)
                {
                    availableSlots.Add(new Reservation
                    {
                        CourtId = courtId,
                        StartDateTime = slotStart,
                        EndDateTime = slotEnd,
                        Status = ReservationStatus.Pending // Statut par défaut pour les créneaux disponibles
                    });
                }
            }

            return availableSlots;
        }
    

        public IEnumerable<Reservation> GetByCourtId(int courtId)
        {
            return _unitOfWork.Reservations.GetByCourtId(courtId);
        }

        public IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end)
        {
            return _unitOfWork.Reservations.GetByDateRange(start, end);
        }

        public Reservation GetById(int id)
        {
            return _unitOfWork.Reservations.GetById(id);
        }

        public IEnumerable<Reservation> GetByUserId(int userId)
        {
            return _unitOfWork.Reservations.GetByUserId(userId);
        }

        public void Update(Reservation reservation)
        {
            // Vérifier que la réservation existe
            var existingReservation = _unitOfWork.Reservations.GetById(reservation.Id);
            if (existingReservation == null)
                throw new InvalidOperationException("La réservation spécifiée n'existe pas.");

            // Empêcher la modification des réservations annulées ou terminées
            if (existingReservation.Status == ReservationStatus.Cancelled ||
                existingReservation.Status == ReservationStatus.Completed)
                throw new InvalidOperationException("Impossible de modifier une réservation annulée ou terminée.");

            // Vérifier la validité de la période
            if (reservation.StartDateTime >= reservation.EndDateTime)
                throw new InvalidOperationException("La date de fin doit être postérieure à la date de début.");

            // Vérifier que la date de début n'est pas dans le passé
            if (reservation.StartDateTime < DateTime.Now)
            {
                throw new InvalidOperationException("Impossible de créer une réservation pour une date passée.");
            }

            // Vérifier qu'il n'y a pas de chevauchement avec d'autres réservations
            var existingReservations = _unitOfWork.Reservations
                .GetByDateRange(reservation.StartDateTime, reservation.EndDateTime)
                .Where(r => r.CourtId == reservation.CourtId &&
                       r.Id != reservation.Id &&
                       r.Status != ReservationStatus.Cancelled)
                .ToList();

            if (existingReservations.Any(r =>
                (r.StartDateTime < reservation.EndDateTime && r.EndDateTime > reservation.StartDateTime)))
            {
                throw new InvalidOperationException("Le créneau horaire est déjà réservé.");
            }

            // Mettre à jour la réservation
            _unitOfWork.Reservations.Update(reservation);
            _unitOfWork.Complete();
        }
    }
}

