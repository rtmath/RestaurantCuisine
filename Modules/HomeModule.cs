using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Food
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };

      Get["/cuisine-form"] = _ =>
      {
        return View["cuisine_form.cshtml"];
      };

      Get["/cuisine-restaurants/{id}"] = parameters =>{
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        List<Restaurant> restaurantList = selectedCuisine.GetRestaurants();
        return View["cuisine_restaurants.cshtml", restaurantList];
      };

      Get["/restaurant-form"] = _ =>
      {
        List<Cuisine> newList = Cuisine.GetAll();
        return View["restaurant_form.cshtml", newList];
      };

      Post["/add-cuisine"] = _ =>
      {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisineform"]){};
        newCuisine.Save();
        List<Cuisine> newList = Cuisine.GetAll();
        return View["index.cshtml", newList];
      };

      Post["/add-restaurant"] = _ =>
      {
        Restaurant newRestaurant = new Restaurant(Request.Form["restform"], Request.Form["cuisine-id"]){};
        newRestaurant.Save();

        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };

      Get["/home"] = _ =>
      {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };
    }
  }
}
