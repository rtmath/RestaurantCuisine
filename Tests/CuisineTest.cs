using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Food
{
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=eating_out_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CuisineEmptyAtFirst()
    {
      int result = Cuisine.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Cuisine firstCuisine = new Cuisine("Chef Boyardee du Jour");
      Cuisine secondCuisine = new Cuisine("Chef Boyardee du Jour");
      Assert.Equal(firstCuisine, secondCuisine);
    }

    [Fact]
    public void Test_Save_SavesCuisineToDatabase()
    {
      Cuisine testCuisine = new Cuisine("Resers Potato Bouquet adorned with a modest Mustard fluorish");
      testCuisine.Save();

      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCuisineObject()
    {
      Cuisine testCuisine = new Cuisine("Cup o Noodles");
      testCuisine.Save();

      Cuisine savedCuisine = Cuisine.GetAll()[0];
      int result = savedCuisine.GetId();
      int testId = testCuisine.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCuisineInDatabase()
    {
      Cuisine testCuisine = new Cuisine("Top Ramen with a spritz of beef powder");
      testCuisine.Save();

      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      Assert.Equal(testCuisine, foundCuisine);
    }

    [Fact]
    public void Test_Update_UpdatesCategoryInDatabase()
    {
      string name = "German bratwursts";
      Cuisine testCuisine = new Cuisine(name);
      testCuisine.Save();
      string newName = "Baseball-Game Franks";

      testCuisine.Update(newName);
      string result = testCuisine.GetCuisineType();

      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryFromDatabase()
    {
     string name1 = "Denver Chexican Fusion";
     Cuisine testCuisine1 = new Cuisine(name1);
     testCuisine1.Save();

     string name2 = "Mediterranean";
     Cuisine testCuisine2 = new Cuisine(name2);
     testCuisine2.Save();

     Restaurant testaurant1 = new Restaurant("Bargain Brats", testCuisine1.GetId());
     testaurant1.Save();
     Restaurant testaurant2 = new Restaurant("Bobs Kebabs", testCuisine2.GetId());
     testaurant2.Save();

     testCuisine1.Delete();
     List<Cuisine> resultCuisines = Cuisine.GetAll();
     List<Cuisine> testCuisineList = new List<Cuisine> {testCuisine2};

     List<Restaurant> resultRestaurants = Restaurant.GetAll();
     List<Restaurant> testRestaurantList = new List<Restaurant> {testaurant2};

     Assert.Equal(testCuisineList, resultCuisines);
     Assert.Equal(testCuisineList, resultCuisines);
    }

    public void Dispose()
    {
      Cuisine.DeleteAll();
    }
  }
}
