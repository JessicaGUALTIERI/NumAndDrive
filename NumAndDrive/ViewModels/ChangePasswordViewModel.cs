using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.ViewModels
{
	public class ChangePasswordViewModel
	{
		[Required]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit faire au moins 8 caractères")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Doit contenir au moins une majuscule, minuscule, chiffre et caracère spécial")]
		public string NewPassword { get; set; }

		[Required]
		[Compare("NewPassword", ErrorMessage = "Les 2 mots de passe doivent être identiques")]
		public string ConfirmNewPassword { get; set; }

	}
}

