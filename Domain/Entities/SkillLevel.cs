using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SkillLevel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        public int Value { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        // Propriété de navigation
        public virtual ICollection<User> Users { get; set; }
    }
}
