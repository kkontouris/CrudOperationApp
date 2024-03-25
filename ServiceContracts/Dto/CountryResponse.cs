using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.Dto
{
	/// <summary>
	/// DTO class that is used as return type for most of countries service methods
	/// </summary>
	public class CountryResponse
	{
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(CountryResponse))
			{
				return false;
			}
			CountryResponse countryToCompare = (CountryResponse)obj;
			return (CountryName == countryToCompare.CountryName && CountryId==
				countryToCompare.CountryId);

		}

	}
	public static class CountryExtensions
	{
		public static CountryResponse ToCountryResponse(this Country country)
		{
			return new CountryResponse()
			{
				CountryId = country.CountryId,
				CountryName = country.CountryName
			};
		}

	}
}
