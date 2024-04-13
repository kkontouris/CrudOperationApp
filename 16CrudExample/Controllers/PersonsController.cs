using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Dto;
using ServiceContracts.Enums;

namespace _16CrudExample.Controllers
{
	[Route("controller")]
	public class PersonsController : Controller
	{

		//private field
		private readonly ICountriesService _countriesService;
		private readonly IPersonService _personService;

		public PersonsController(IPersonService personService, ICountriesService countriesService)
		{
			_personService = personService;
			_countriesService = countriesService;
		}

		[Route("[action]")]
		[Route("/")]
		public IActionResult Index(string? searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName)
			, SortOrderOptions sortOrder = SortOrderOptions.ASC)
		{
			ViewBag.SearchFields = new Dictionary<string, string>()

			{ {nameof(PersonResponse.PersonName),"Person Name" },
				{nameof(PersonResponse.Gender), "Gender" },
				{nameof(PersonResponse.Address), "Address" },
				{nameof(PersonResponse.Email), "Email" },
				{nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
				{nameof(PersonResponse.Country), "Country" }
			};
			//search
			List<PersonResponse> allPersons = _personService.GetFilteredPersons(searchBy, searchString);
			ViewBag.CurrentSearchBy = searchBy;
			ViewBag.CurrentSearchString = searchString;

			//sort 
			List<PersonResponse> sortedPersons = _personService.GetSortedPersons(allPersons, sortBy, sortOrder);
			ViewBag.CurrentSortBy = sortBy;
			ViewBag.CurrentSortOrder = sortOrder.ToString();

			return View(sortedPersons);
		}
		//Executes when the user clicks on "Create Person" hyperlink
		//while opening the create view
		[Route("[action]")]
		[HttpGet]
		public IActionResult Create()
		{
			List<CountryResponse> allCountries=_countriesService.GetAllCountries();
			ViewBag.Countries = allCountries;
			return View();
		}

		//Post request
		
		[HttpPost]
		[Route("[action]")]
		public IActionResult Create(PersonAddRequest personAddRequest)
		{
			if(!ModelState.IsValid)
			{
				List<CountryResponse> allCountries = _countriesService.GetAllCountries();
				ViewBag.Countries = allCountries;
				ViewBag.errors=ModelState.Values.SelectMany(v=>v.Errors).SelectMany(v=>v.ErrorMessage).ToList();
				return View();
			}
			_personService.AddPerson(personAddRequest);
			return RedirectToAction("Index","Persons");
		}
		//Update  (Get) request
		[HttpGet]
		[Route("[action]/{PersonId}")]
		public IActionResult Edit(Guid? PersonId)
		{
			PersonResponse? personResponse=_personService.GetPersonByPersonId(PersonId);
			if(personResponse == null)
			{
				return RedirectToAction("Index");
			}
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
			List<CountryResponse> allCountries = _countriesService.GetAllCountries();
			ViewBag.Countries = allCountries;
			return View(personUpdateRequest);
		}

		//Update(Post) request
		[HttpPost]
		[Route("[action]/{PersonId}")]
		public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse= _personService.GetPersonByPersonId(personUpdateRequest.PersonId);
			if( personResponse == null)
			{
				return RedirectToAction("Index");
			}
			if(ModelState.IsValid)
			{
				PersonResponse updatedPerson= _personService.UpdatePerson(personUpdateRequest);
				return RedirectToAction("Index");
			}
			else
			{
				List<CountryResponse> allCountries = _countriesService.GetAllCountries();
				ViewBag.Countries = allCountries;
				ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(v => v.ErrorMessage).ToList();
				return View(personResponse.ToPersonUpdateRequest());
			}
		}

		[HttpGet]
		[Route("[action]/{PersonId}")]
		public IActionResult Delete(Guid? PersonId)
		{

			PersonResponse? personResponse=_personService.GetPersonByPersonId(PersonId);
			if(personResponse == null)
			{
				return RedirectToAction("Index");
			}

			return View(personResponse);
		}

		[HttpPost]
		[Route("[action]/{PersonId}")]
		public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse=_personService.GetPersonByPersonId(personUpdateRequest.PersonId);
			if(personResponse==null)
			{
				return RedirectToAction("Index");
			}
			_personService.DeletePerson(personResponse.PersonId);


			return RedirectToAction("Index");
		}
	}
}
