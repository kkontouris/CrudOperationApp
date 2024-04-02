using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.Dto
{
	/// <summary>
	/// Acts as a DTO for inserting a new Person
	/// </summary>
	public class PersonAddRequest
	{
		[Required(ErrorMessage ="Person name can not be blanc")]
        public string? PersonName { get; set; }
		[Required(ErrorMessage = "Email can not be blanc")]
		[EmailAddress(ErrorMessage ="Email address should be in specific format")]
		public string? Email { get; set; }

		public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

		public Guid? CountryId { get; set; }

		public string? Address { get; set; }

		public bool ReceiveNewsLetters { get; set; }

		/// <summary>
		/// Converts the current object of PersonAddRequest into a new
		/// Object of Person Type for the Domain Model
		/// </summary>
		/// <returns></returns>
        public Person ToPerson() 
		{
			return new Person()
			{

				PersonName = PersonName,
				Email = Email,
				DateOfBirth = DateOfBirth,
				Gender=Gender.ToString(),
				CountryId = CountryId,
				Address = Address,
				ReceiveNewsLeters = ReceiveNewsLetters
			};

		}
    }
}
