using ServiceContracts;
using ServiceContracts.Dto;
using ServiceContracts.Enums;
using Services;
using Entities;
using Xunit.Sdk;
using System.Linq;

namespace CrudTests
{
	public class PersonServiceTest
	{

		private readonly IPersonService _personService;
		private readonly ICountriesService _countriesService;

		public PersonServiceTest()
		{
			_personService = new PersonService();
			_countriesService = new CountriesService();
		}
		#region AddPerson
		//if the personAddRequest is null throws null reference exception
		[Fact]
		public void AddPerson_NullPerson()
		{
			PersonAddRequest? personAddRequest = null;

			Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
		}

		//if we supply personName null , it throws argument exception
		[Fact]
		public void AddPerson_NullPersonName()
		{
			PersonAddRequest? personAddRequest = new PersonAddRequest()
			{
				PersonName = null
			};

			Assert.Throws<ArgumentException>(() => { _personService.AddPerson(personAddRequest); }); ;
		}
		//if
		[Fact]
		public void AddPerson_ProperPersonDetails()
		{
			PersonAddRequest personAddRequest = new PersonAddRequest()
			{
				PersonName = "Konstantinos",
				Address = "Kalavriotn 23",
				Email = "papaki@ymail.com",
				DateOfBirth = DateTime.Parse("2015-02-02"),
				CountryId = Guid.NewGuid(),
				Gender = GenderOptions.Male,
				ReceiveNewsLetters = true
			};
			//Act
			PersonResponse actualPersonResponse = _personService.AddPerson(personAddRequest);
			List<PersonResponse> responsePersonListFromAdd = _personService.GetAllPersons();

			//Assert
			Assert.True(actualPersonResponse.PersonId != Guid.Empty);

			Assert.Contains(actualPersonResponse, responsePersonListFromAdd);
		}
		#endregion

		#region GetPersonByPersonId
		//If PersonId is null then returns null personResponse 
		[Fact]
		public void GetPersonByPersonId_NullPersonId()
		{
			Guid? personId = null;

			Assert.Null(_personService.GetPersonByPersonId(personId));
		}


		#endregion

		#region GetAllPersons
		//If thre is no objects in the list,should return emmpty list 
		[Fact]
		public void GetAllPersons_EmmptyList()
		{
			List<PersonResponse> personResponseList = _personService.GetAllPersons();

			Assert.Empty(personResponseList);
		}
		//i add PersonResponse in the List<PersonResponse> and the method should Return the items
		[Fact]
		public void GetAllPersons_FewPersons()
		{
			CountryAddRequest request1 = new CountryAddRequest()
			{
				CountryName = "Greece"
			};
			CountryResponse countryResponse = _countriesService.AddCountry(request1);

			PersonAddRequest requestPersonNew = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "kkwstas@ymail.com",
				PersonName = "Konstantinos",
				Address = "Kala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("2000-02-02"),
				Gender = GenderOptions.Male
			};
			List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>();
			personAddRequestList.Add(requestPersonNew);


			//list of PersonResponse
			List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

			foreach (PersonAddRequest personAddRequest in personAddRequestList)
			{
				PersonResponse personResponse = _personService.AddPerson(personAddRequest);
				personResponseListFromAdd.Add(personResponse);

			}

			//Person Response from Get
			List<PersonResponse> personResponseListFromGet = _personService.GetAllPersons();

			//Assert
			Assert.Equal(personResponseListFromAdd, personResponseListFromGet);

		}
		#endregion

		#region GetFilterdPersons
		//if the search text is EmptyException and and search by is "PersonName"
		//it should return all persons
		[Fact]
		public void GetFilteredPersons_EmptySearchText()
		{
			CountryAddRequest request1 = new CountryAddRequest()
			{
				CountryName = "Greece"
			};
			CountryResponse countryResponse = _countriesService.AddCountry(request1);

			PersonAddRequest requestPersonNew1 = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "kkwstas@ymail.com",
				PersonName = "Konstantinos",
				Address = "Kala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("2000-02-02"),
				Gender = GenderOptions.Male
			};

			PersonAddRequest requestPersonNew2 = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "papaki@ymail.com",
				PersonName = "Katerina",
				Address = "Kalakala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("1998-12-02"),
				Gender = GenderOptions.Female
			};
			List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>();
			personAddRequestList.Add(requestPersonNew1);
			personAddRequestList.Add(requestPersonNew2);


			//list of PersonResponse
			List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

			foreach (PersonAddRequest personAddRequest in personAddRequestList)
			{
				PersonResponse personResponse = _personService.AddPerson(personAddRequest);
				personResponseListFromAdd.Add(personResponse);

			}

			//Person Response from Get
			List<PersonResponse> personResponseListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

			//Assert
			foreach (PersonResponse personresponse_from_add in personResponseListFromAdd)
			{
				Assert.Contains(personresponse_from_add, personResponseListFromSearch);
			}


		}

		//first we will add few persons and then we will search based on the person name
		//with some search string. It should return the matching person objects
		[Fact]
		public void GetFilteredPersons_SearchByPersonName()
		{
			CountryAddRequest request1 = new CountryAddRequest()
			{
				CountryName = "Greece"
			};
			CountryResponse countryResponse = _countriesService.AddCountry(request1);

			PersonAddRequest requestPersonNew1 = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "kkwstas@ymail.com",
				PersonName = "Konstantinos",
				Address = "Kala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("2000-02-02"),
				Gender = GenderOptions.Male
			};

			PersonAddRequest requestPersonNew2 = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "papaki@ymail.com",
				PersonName = "Katerina",
				Address = "Kalakala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("1998-12-02"),
				Gender = GenderOptions.Female
			};
			List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>();
			personAddRequestList.Add(requestPersonNew1);
			personAddRequestList.Add(requestPersonNew2);


			//list of PersonResponse
			List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

			foreach (PersonAddRequest personAddRequest in personAddRequestList)
			{
				PersonResponse personResponse = _personService.AddPerson(personAddRequest);
				personResponseListFromAdd.Add(personResponse);

			}

			//Person Response from Get
			List<PersonResponse> personResponseListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "ko");

			//Assert

			foreach (PersonResponse personresponse_from_add in personResponseListFromAdd)
			{
				if (personresponse_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
				{
					Assert.Contains(personresponse_from_add, personResponseListFromSearch);
				}
			}


		}

		#endregion

		#region GetSortedPersons

		//when we sort based on the PersonName in Desc, it should return persons list 
		//in descending on PersonName
		[Fact]
		public void GetSortedPersons_DescByPersonName()
		{
			CountryAddRequest request1 = new CountryAddRequest()
			{
				CountryName = "Greece"
			};
			CountryResponse countryResponse = _countriesService.AddCountry(request1);

			PersonAddRequest requestPersonNew1 = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "kkwstas@ymail.com",
				PersonName = "Konstantinos",
				Address = "Kala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("2000-02-02"),
				Gender = GenderOptions.Male
			};

			PersonAddRequest requestPersonNew2 = new PersonAddRequest()
			{
				ReceiveNewsLetters = true,
				Email = "papaki@ymail.com",
				PersonName = "Katerina",
				Address = "Kalakala 22",
				CountryId = countryResponse.CountryId,
				DateOfBirth = Convert.ToDateTime("1998-12-02"),
				Gender = GenderOptions.Female
			};
			List<PersonAddRequest> personAddRequestList = new List<PersonAddRequest>();
			personAddRequestList.Add(requestPersonNew1);
			personAddRequestList.Add(requestPersonNew2);


			//list of PersonResponse
			List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

			foreach (PersonAddRequest personAddRequest in personAddRequestList)
			{
				PersonResponse personResponse = _personService.AddPerson(personAddRequest);
				personResponseListFromAdd.Add(personResponse);

			}
			//Act
			//Person Response from Sort
			List<PersonResponse> personResponseListFromSort = _personService.GetSortedPersons(_personService.GetAllPersons(), nameof(Person.PersonName), SortOrderOptions.DESC);

			personResponseListFromAdd = personResponseListFromAdd.OrderByDescending(temp => temp.PersonName).ToList();

			//Assert

			foreach (PersonResponse personresponse_from_add in personResponseListFromAdd)
			{
				for (int i = 0; i < personResponseListFromAdd.Count(); i++)
				{
					Assert.Equal(personResponseListFromAdd[i], personResponseListFromSort[i]);
				}
			}

			#endregion
		}
	}
}
