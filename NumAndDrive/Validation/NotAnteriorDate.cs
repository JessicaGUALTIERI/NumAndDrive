using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Validation
{
	public class NotAnteriorDate : ValidationAttribute
	{
		public NotAnteriorDate()
		{
			ErrorMessage = "La date ne peut pas être antérieur à aujourd'hui";
		}
    }
}

