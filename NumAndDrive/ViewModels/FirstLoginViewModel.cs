using System;
using System.ComponentModel.DataAnnotations;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
    public class FirstLoginViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit faire au moins 8 caractères")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, minuscule, chiffre et caracère spécial")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Les 2 mots de passe doivent être identiques")]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire.")]
        public int? StatusId { get; set; }

        [Required(ErrorMessage = "Le service est obligatoire.")]
        public int? DepartmentId { get; set; }

        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Department> Departments { get; set; }
    }
}

