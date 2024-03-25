using ServiceContracts;
using ServiceContracts.Dto;
using Entities;


namespace Services
{
	public class CountriesService : ICountriesService
	{
		//like a database
		List<Country> _countries;
        public CountriesService()
        {
				_countries = new List<Country>();
        }

		/// <summary>
		/// Give the CountryAddRequest and return the matching CountryResponse
		/// </summary>
		/// <param name="countryAddRequest"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
		{

			//Validation: The countryAddRequest can not be null
			if(countryAddRequest == null)
			{
				throw new ArgumentNullException(nameof(countryAddRequest));
			}
			//validation : if countryName is null throw exception
			if(countryAddRequest.CountryName == null)
			{
				throw new ArgumentException();
			}
			//Validation: if countryName is duplicate throw argument exception
			foreach(var _country in _countries)
			{
				if (countryAddRequest.CountryName == _country.CountryName)
				{
					throw new ArgumentException("Given country name already exists");	
				}
			}
			Country country=countryAddRequest.ToCountry();

			country.CountryId = Guid.NewGuid();

			_countries.Add(country);

			return country.ToCountryResponse();
		}

		/// <summary>
		/// Return all the countries of list
		/// </summary>
		/// <returns>List of CountryResponse</returns>
		public List<CountryResponse> GetAllCountries()
		{
			List<CountryResponse> countryResponseList=new List<CountryResponse>();
			foreach(var _country in _countries)
			{
				CountryResponse countryResponse=_country.ToCountryResponse();
				countryResponseList.Add(countryResponse);
			}
			return countryResponseList;
		}

		/// <summary>
		/// Returns country object based on the given countryId
		/// </summary>
		/// <param name="CountryId">CountryId Guid</param>
		/// <returns>Matching Country as Country Response</returns>
		/// <exception cref="NotImplementedException"></exception>
		public CountryResponse? GetCountryByCountryId(Guid? CountryId)
		{
			if (CountryId == null)
			{
				return null;
			}

			Country? countryFromGuid=_countries.FirstOrDefault(country=>country.CountryId==CountryId);
			 CountryResponse? countryResponseFromGuid = (CountryResponse)countryFromGuid.ToCountryResponse();
			return countryResponseFromGuid;
		}
	}
}