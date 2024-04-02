using ServiceContracts.Dto;
using ServiceContracts.Enums;


namespace ServiceContracts
{
	public interface IPersonService
	{
		/// <summary>
		/// Adds a new person into the list of persons
		/// </summary>
		/// <param name="request">Person to add</param>
		/// <returns>Returns the same person details, with PersonId</returns>
		/// <exception cref="NotImplementedException"></exception>
		public PersonResponse AddPerson(PersonAddRequest request);

		/// <summary>
		/// Returns all persons
		/// </summary>
		/// <returns>returns a list of persons with type PersonResponse</returns>
		List<PersonResponse> GetAllPersons();

		/// <summary>
		/// Returns the person object based on the personId
		/// </summary>
		/// <param name="personId">Person Id to search</param>
		/// <returns>Returns matching Person Object</returns>
		public PersonResponse GetPersonByPersonId(Guid? personId);

		/// <summary>
		/// Get all the specific persons objects based on the searchBy and searchstrings which is gived 
		/// fromthe user 
		/// </summary>
		/// <param name="searchBy">search field to search</param>
		/// <param name="searchString">search string to search</param>
		/// <returns>List of Person objects</returns>
		public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

		/// <summary>
		/// return sorted list of person
		/// </summary>
		/// <param name="allPersons">represent all objects of person in list</param>
		/// <param name="sortBy">name of the property (key), based on which the persons 
		/// should be sorted</param>
		/// <param name="sortOrder">Asc or Desc</param>
		/// <returns>returns sorted list of persons as PersonResponse</returns>
		public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);


		/// <summary>
		/// Updates the specified given Person object based on the specified Person Id
		/// </summary>
		/// <param name="request">Person Details to Update</param>
		/// <returns>The PersonResponse object after update</returns>
		public PersonResponse UpdatePerson(PersonUpdateRequest? request);


		/// <summary>
		/// Deletes a person based on the given person id
		/// </summary>
		/// <param name="PersonId">PersonId for the person to delete</param>
		/// <returns>true if the deletion is successfull or false</returns>
		public bool DeletePerson(Guid? PersonId);

	}

}
