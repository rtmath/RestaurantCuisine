using System.Collections.Generic;
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

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> allCuisines = new List<Cuisine>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
      int cuisineId = rdr.GetInt32(0);
      string cuisineType = rdr.GetString(1);
      Cuisine newCuisine = new Cuisine(cuisineType, cuisineId);
      allCuisines.Add(newCuisine);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCuisines;
    }

    public List<Restaurant> GetRestaurants()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE cuisine_id = @CuisineId;", conn);

      SqlParameter cuisineIdParam = new SqlParameter();
      cuisineIdParam.ParameterName = "@CuisineId";
      cuisineIdParam.Value = this.GetId();
      cmd.Parameters.Add(cuisineIdParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Restaurant> restaurants = new List<Restaurant>{};
      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        int cuisineId = rdr.GetInt32(2);
        Restaurant newRestaurant = new Restaurant(restaurantName, cuisineId, restaurantId);
        restaurants.Add(newRestaurant);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return restaurants;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cuisine (cuisine_type) OUTPUT INSERTED.id VALUES (@CuisineType);", conn);

      SqlParameter cuisineParameter = new SqlParameter();
      cuisineParameter.ParameterName = "@CuisineType";
      cuisineParameter.Value = this.GetCuisineType();
      cmd.Parameters.Add(cuisineParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn !=  null)
      {
        conn.Close();
      }
    }

    public void Update(string newType)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE cuisine SET cuisine_type = @newType OUTPUT INSERTED.cuisine_type WHERE id = @CuisineId;", conn);

      SqlParameter newCuisineTypeParam = new SqlParameter();
      newCuisineTypeParam.ParameterName = "@newType";
      newCuisineTypeParam.Value = newType;
      cmd.Parameters.Add(newCuisineTypeParam);

      SqlParameter newCuisineIdParam = new SqlParameter();
      newCuisineIdParam.ParameterName = "@CuisineId";
      newCuisineIdParam.Value = this.GetId();
      cmd.Parameters.Add(newCuisineIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._cuisineType = rdr.GetString(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Cuisine Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisine WHERE id = (@CuisineId)", conn);

      SqlParameter idParam = new SqlParameter();
      idParam.ParameterName   = "@CuisineId";
      idParam.Value = id.ToString();
      cmd.Parameters.Add(idParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCuisineId = 0;
      string foundCuisineType = null;

      while (rdr.Read())
      {
        foundCuisineId = rdr.GetInt32(0);
        foundCuisineType = rdr.GetString(1);
      }
      Cuisine newCuisine = new Cuisine(foundCuisineType, foundCuisineId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return newCuisine;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cuisine WHERE id = @CuisineId; DELETE FROM restaurants WHERE cuisine_id = @CuisineId;", conn);

      SqlParameter cuisineIdParam = new SqlParameter();
      cuisineIdParam.ParameterName = "@CuisineId";
      cuisineIdParam.Value = this.GetId();

      cmd.Parameters.Add(cuisineIdParam);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cuisine; DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
