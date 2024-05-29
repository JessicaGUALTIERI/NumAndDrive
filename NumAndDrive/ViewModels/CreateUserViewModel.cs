using System;
using System.ComponentModel.DataAnnotations;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class CreateUserViewModel
	{
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$", ErrorMessage = "Le nom saisi n'est pas conforme.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Le nom saisi doit faire entre 1 et 100 caractères.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$", ErrorMessage = "Le prénom saisi n'est pas conforme..")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Le prénom saisi doit faire entre 1 et 100 caractères.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "L'adresse mail est obligatoire.")]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9._%+-]*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "L'adresse mail saisie n'est pas conforme.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le numéro est obligatoire.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Le numéro saisi n'est pas conforme.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire.")]
        public int? StatusId { get; set; }

        [Required(ErrorMessage = "Le service est obligatoire.")]
        public int? DepartmentId { get; set; }

        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Department> Departments { get; set; }
	}
}


