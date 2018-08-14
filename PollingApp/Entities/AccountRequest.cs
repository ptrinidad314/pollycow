using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollingApp.Entities
{
    public class AccountRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public bool Complete { get; set; }
    }
}
