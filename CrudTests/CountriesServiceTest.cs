using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.Dto;
using Entities;
using CrudTests;
using Services;

namespace CrudTests
{
	
	public class CountriesServiceTest
	{
		private readonly ICountriesService _countriesService;


        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }
		#region AddCountry
		//when you add null addrequest it should throw null argument exception
		[Fact]
        public void AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;
           
            Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));
            
        }

        //when the country name is null, throw null exception
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            CountryAddRequest? request=new CountryAddRequest() { CountryName=null};
            Assert.Throws<ArgumentException>(() => { _countriesService.AddCountry(request); });
        }

		//when the country name is duplicate , throws argument exception
		[Fact]
		public void AddCountry_CountryNameIsDublicate()
		{
			CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "Greece" };

			CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Greece" };

			Assert.Throws<ArgumentException>(() =>
			{
				_countriesService.AddCountry(request1);
				_countriesService.AddCountry(request2);
			}); 
		}


		//when you supply proper country name, it should insert in a list
		[Fact]
		public void AddCountry_CountryNameIsProper()
		{
			CountryAddRequest? request = new CountryAddRequest() { CountryName = "Greece" };


			CountryResponse countryResponse= _countriesService.AddCountry(request);
			Assert.Equal("Greece", countryResponse.CountryName);
		}
		#endregion

		#region GetAllCountries
		//Validation : The list of countries is empty
		[Fact]
		public void GetAllCountries_EmptyList()
		{
			//Act
			List<CountryResponse> actual_country_response_empty=
				_countriesService.GetAllCountries();

			//Assert
			Assert.Empty(actual_country_response_empty);
		}

		//Validation: There are countries in the list
		[Fact]
		public void GetAllCountries_AddFewCountries()
		{
			//Arrange
			List<CountryAddRequest> countryAddRequest_list =
			new List<CountryAddRequest>()
			{
				new CountryAddRequest() {CountryName="Greece"},
			};
			//Act
			List<CountryResponse> countryResponse_List = new List<CountryResponse>();
			foreach (CountryAddRequest countryAddRequest in countryAddRequest_list)
			{
				countryResponse_List.Add(_countriesService.AddCountry
					(countryAddRequest));
			}
			List<CountryResponse> actual_countryResponses_List=_countriesService.GetAllCountries();
			//readd ever country from countryResponse_list
			foreach(CountryResponse expected_country in countryResponse_List )
			{
				Assert.Contains(expected_country, actual_countryResponses_List);
			}

		}

		#endregion

		#region GetCountryByCountryId
		//Validation CountryId =null
		[Fact]
		public void GetCountryByCountryId_NullCountryId()
		{
			Guid? countryId = null;

			Assert.Null(_countriesService.GetCountryByCountryId(countryId));
		}
		//validation an tou dwsoume guid an epistrepsei to sosto countryId
		[Fact]
		public void GetCountryByCountryId_FindRightCountryId()
		{
			//Arrange-add new country in countryaddrequest
			CountryAddRequest? countryAddRequestNew = new CountryAddRequest()
			{
				CountryName = "Greece"
			};
			CountryResponse? countryResponse_FromAdd = _countriesService.AddCountry(countryAddRequestNew);

			//Act
			CountryResponse? expectedCountryResponse=_countriesService.GetCountryByCountryId(countryResponse_FromAdd.CountryId);

			//Assert
			Assert.Equal(countryResponse_FromAdd.CountryName, expectedCountryResponse.CountryName);
		}




		#endregion
	}
}
