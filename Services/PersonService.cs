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
using Services.Helpers;

namespace Services
{
	public class PersonService : IPersonService
	{
		private readonly List<Person> _persons;
		private readonly ICountriesService _countriesService;

		public PersonService(bool initialize=true)
        {
			_persons = new List<Person>();
			_countriesService = new CountriesService();
			if (initialize)
			{
				_persons.AddRange(
					new List<Person>()
					{
						new Person(){PersonId=Guid.Parse("8B19CD5C-2361-4CEC-BCF0-F3D03975F42C"),PersonName="Konstantinos",
						Address="Kalavrita 22", DateOfBirth=Convert.ToDateTime("1985-04-24"),Email="kkwstas@gaga.com",
						Gender="Male",ReceiveNewsLeters=true,CountryId=Guid.Parse("1E9CC1B4-FD93-4E4C-B174-9448F6BEB0E6")},

                        new Person(){PersonId=Guid.Parse("3300FD8F-DEED-4FFB-BB25-04937C386981"),PersonName="Magdalini",
                        Address="Peukon 54", DateOfBirth=Convert.ToDateTime("1990-08-15"),Email="magda@gala.com",
                        Gender="Female",ReceiveNewsLeters=false,CountryId=Guid.Parse("6ECC0D80-B535-4A56-8EDB-D204FA95E8C4")},

                        new Person(){PersonId=Guid.Parse("72B839B5-CBE9-4243-9C29-34D3136DA891"),PersonName="Kate",
                        Address="Alamanas 32", DateOfBirth=Convert.ToDateTime("2000-11-16"),Email="katia@gala.com",
                        Gender="Female",ReceiveNewsLeters=true,CountryId=Guid.Parse("49A67E49-94C4-45E3-873B-AD841FD2F580")}

                    });
			}
        }

		private PersonResponse ConvertPersonToPersonResponse(Person person)
		{
			PersonResponse personResponse = person.ToPersonResponse();
			personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
			return personResponse;
		}

		public PersonResponse AddPerson(PersonAddRequest? request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			//Model validation
			ValidationHelpers.ModelValidation(request);

			Person person = request.ToPerson();
			person.PersonId=Guid.NewGuid();
			_persons.Add(person);

			return ConvertPersonToPersonResponse(person);
		}

		public List<PersonResponse> GetAllPersons()
		{
			return _persons.Select(temp=>ConvertPersonToPersonResponse(temp)).ToList();
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

			return ConvertPersonToPersonResponse(personWithPersonId);
				
		}


		public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
		{
			List<PersonResponse> allPersons = GetAllPersons();
			List<PersonResponse> matchingPersons = allPersons;
			if (searchBy == null || searchString==null)
			{
				return matchingPersons;
			}
			else
			{
				switch(searchBy)
				{
					case nameof(PersonResponse.PersonName):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName)
						? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					case nameof(PersonResponse.Email):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email)
						? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					case nameof(PersonResponse.Address):
						matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address)
						? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
						break;
					case nameof(PersonResponse.DateOfBirth):
						matchingPersons = allPersons.Where(temp => (temp.DateOfBirth!=null)
						? temp.DateOfBirth.Value.ToString("dd mmm yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
						break;
					case nameof(PersonResponse.Gender):
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

			return ConvertPersonToPersonResponse(matchingPerson);
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
