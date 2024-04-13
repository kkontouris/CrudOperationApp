
using ServiceContracts;
using Services;

namespace _16CrudExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();
			builder.Services.AddSingleton<ICountriesService, CountriesService>();
            builder.Services.AddSingleton<IPersonService, PersonService>();
            var app = builder.Build();

			if(builder.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseRouting();
			app.MapControllers();

			app.Run();
		}
	}
}