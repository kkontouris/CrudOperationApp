using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.Dto
{
	public class PersonResponse
	{
        public Guid? PersonId { get; set; }
		public string? PersonName {  get; set; }
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
		public Guid? CountryId { get; set; }
		public string? Address { get; set; }
        public string? ReceiveNewsLetters { get; set; }

    }
	public static class PersonExtensions
	{
		public static PersonResponse ToPersonResponse(this Person person)
		{
			return new PersonResponse()
			{
				PersonId = person.PersonId,
				PersonName = person.PersonName,
				Email = person.Email,
				DateOfBirth = person.DateOfBirth,
				Gender = person.Gender,
				CountryId = person.CountryId,
				Address = person.Address,
				ReceiveNewsLetters = person.ReceiveNewsLeters
			};
			
		}
	}
}
