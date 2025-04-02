﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Court
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsIndoor { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Propriété de navigation
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
