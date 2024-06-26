﻿using System;
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
        public Guid PersonId { get; set; }
		public string? PersonName {  get; set; }
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
		public Guid? CountryId { get; set; }
		public string? Country { get; set; }
		public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public double? Age { get; set; }

		public override bool Equals(object? obj)
		{
			if(obj == null)
			{
				return false;
			}
			if (obj.GetType()!=typeof(PersonResponse))
			{
				return false;
			}

			PersonResponse personResponse = (PersonResponse)obj;
			return PersonId == personResponse.PersonId &&
				PersonName == personResponse.PersonName &&
				Email == personResponse.Email &&
				DateOfBirth == personResponse.DateOfBirth &&
				CountryId == personResponse.CountryId &&
				Address == personResponse.Address &&
				ReceiveNewsLetters == personResponse.ReceiveNewsLetters &&
				Gender == personResponse.Gender;
		}

		public PersonUpdateRequest ToPersonUpdateRequest()
		{
			return new PersonUpdateRequest()
			{
				PersonId = PersonId,
				PersonName = PersonName,
				Email = Email,
				DateOfBirth = DateOfBirth,
				Address = Address,
				Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
				CountryId = CountryId,
				ReceiveNewsLetters = ReceiveNewsLetters
			};
		}
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
				ReceiveNewsLetters = person.ReceiveNewsLeters,
				Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25):null
			};
			
		}
	}
}
