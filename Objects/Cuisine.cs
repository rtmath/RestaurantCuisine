using System.Collection
using System.Data.SqlClient;
using System;

namespace Food
{
  public class Cuisine
  {
    private int _id;
    private string _cuisineType;

    public Cuisine(string type, int id = 0)
    {
      _cuisineType = type;
      _id = id;
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if(!(otherCuisine is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;
        bool idEquality = this.GetId() == newCuisine.GetId();
        bool typeEquality = this.GetCuisineType() == newCuisine.GetCuisineType();
        return (idEquality && typeEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetCuisineType()
    {
      return _cuisineType;
    }

    public void Save()
    {
      
    }
  }
}
