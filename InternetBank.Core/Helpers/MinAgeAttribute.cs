using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBank.Core.Helpers
{
	public class MinAgeAttribute :ValidationAttribute
	{
		private readonly int _minAge;

		public MinAgeAttribute(int minAge)
		{
			_minAge = minAge;
			ErrorMessage = $"سن باید بالای {minAge} سال باشد";
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is DateTime birthDate)
			{
				var age = DateTime.Today.Year - birthDate.Year;
				if (birthDate > DateTime.Today.AddYears(-age)) age--;

				if (age < _minAge)
				{
					return new ValidationResult(ErrorMessage);
				}
			}

			return ValidationResult.Success;
		}
	}
}
