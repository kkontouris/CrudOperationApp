using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.Dto;
using Entities;
using ServiceContracts.Enums;
using System.Collections;
using Services.Helpers;
using System.Net.Http.Headers;

namespace Services
{
	public class PersonService : IPersonService
	{
		private readonly List<Person> _persons;

        public PersonService()
        {
				_persons = new List<Person>();
        }

        public PersonResponse AddPerson(PersonAddRequest request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			if (request.PersonName == null)
			{
				throw new ArgumentException();
			}
			Person personAdded = request.ToPerson();
			personAdded.PersonId=Guid.NewGuid();
			_persons.Add(personAdded);
			PersonResponse personResponseAdded=personAdded.ToPersonResponse();

			return personResponseAdded;
		}

		public List<PersonResponse> GetAllPersons()
		{
			return _persons.Select(temp=>temp.ToPersonResponse()).ToList();
		}


		public PersonResponse GetPersonByPersonId(Guid? personId)
		{
			PersonResponse personResponseWithPersonId;
			if (personId == null)
			{
				return null;
			}

			Person? personWithPersonId = _persons.FirstOrDefault(temp => temp.PersonId == personId);
			if(personWithPersonId == null)
			{
				return null;
			}
			
			return personWithPersonId.ToPersonResponse();
				
		}


		public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
		{
			List<PersonResponse> allPersons = GetAllPersons();
			List<PersonResponse> matchingPersons = allPersons;
			if (searchBy == null || searchString==null)
			{
				return allPersons;
			}
			else
			{
				switch(searchBy)
				{
					case nameof(Person.PersonName):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName)
						? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					case nameof(Person.Email):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email)
						? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					case nameof(Person.Address):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address)
						? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					case nameof(Person.DateOfBirth):
						matchingPersons = allPersons.Where(temp => (temp.DateOfBirth!=null)
						? temp.DateOfBirth.Value.ToString("dd mmm yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
						break;
					case nameof(Person.Gender):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender)
						? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					default:
						matchingPersons=allPersons;
						break;
				}
				return matchingPersons;
				

			}
		}

		public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
			{
				return allPersons;
			}
			List<PersonResponse> sortedPersons = (sortBy, sortOrder)
			switch
			{
				(nameof(PersonResponse.PersonName), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
				
				(nameof(PersonResponse.PersonName), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Age), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Age), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Address), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Address,StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.DateOfBirth), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.Gender), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters),(SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

				_=>allPersons

			};
			return sortedPersons;
		}

		public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			if (personUpdateRequest == null)
			{
				throw new ArgumentNullException(nameof(Person));
			}


			//validation
			ValidationHelpers.ModelValidation(personUpdateRequest);

			//seek in the database person object with the same id
			Person? matchingPerson=_persons.FirstOrDefault(temp=>temp.PersonId == 
			personUpdateRequest.PersonId);

			if(matchingPerson==null)
			{
				throw new ArgumentException("Given Person Id does not exist");
			}

			//update all details
			matchingPerson.PersonName = personUpdateRequest.PersonName;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.CountryId = personUpdateRequest.CountryId;
			matchingPerson.DateOfBirth= personUpdateRequest.DateOfBirth;
			matchingPerson.ReceiveNewsLeters = personUpdateRequest.ReceiveNewsLetters;

			return matchingPerson.ToPersonResponse();
		}

		public bool DeletePerson(Guid? PersonId)
		{
			if(PersonId == null)
			{
				throw new ArgumentNullException(nameof(PersonId));
			}
			Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonId == PersonId);
			if (matchingPerson == null)
			{
				return false;
				
			}
			_persons.RemoveAll(temp=>temp.PersonId==PersonId);
			return true;
		}
	}
}
