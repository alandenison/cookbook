using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Cookbook
{
  public class Ingredient
  {
    private int _id;
    private string _name;

    public Ingredient(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }
    public override bool Equals(System.Object otherIngredient)
    {
        if (!(otherIngredient is Ingredient))
        {
          return false;
        }
        else
        {
          Ingredient newIngredient = (Ingredient) otherIngredient;
          bool idEquality = this.GetId() == newIngredient.GetId();
          bool nameEquality = this.GetName() == newIngredient.GetName();
          return (idEquality && nameEquality);
        }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<Ingredient> GetAll()
    {
      List<Ingredient> allIngredients = new List<Ingredient>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        string ingredientName = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
        allIngredients.Add(newIngredient);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allIngredients;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (name) OUTPUT INSERTED.id VALUES (@IngredientName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@IngredientName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients WHERE id = @IngredientId; DELETE FROM recipe_ingredients WHERE ingredient_id = @IngredientId;", conn);

      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(ingredientIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients; DELETE FROM recipe_ingredients", conn);
      cmd.ExecuteNonQuery();
    }

    public static Ingredient Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @IngredientId;", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = id.ToString();
      cmd.Parameters.Add(ingredientIdParameter);
      rdr = cmd.ExecuteReader();

      int foundIngredientId = 0;
      string foundIngredientName = null;

      while(rdr.Read())
      {
        foundIngredientId = rdr.GetInt32(0);
        foundIngredientName = rdr.GetString(1);
      }
      Ingredient foundIngredient = new Ingredient(foundIngredientName, foundIngredientId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundIngredient;
    }

    public List<Recipe> GetRecipes()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipe_id FROM recipe_ingredients WHERE ingredient_id = @IngredientId;", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = this.GetId();
      cmd.Parameters.Add(ingredientIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> recipeIds = new List<int> {};
      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        recipeIds.Add(recipeId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Recipe> recipes = new List<Recipe> {};
      foreach (int recipeId in recipeIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand recipeQuery = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);

        SqlParameter recipeIdParameter = new SqlParameter();
        recipeIdParameter.ParameterName = "@RecipeId";
        recipeIdParameter.Value = recipeId;
        recipeQuery.Parameters.Add(recipeIdParameter);

        queryReader = recipeQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisRecipeId = queryReader.GetInt32(0);
          string recipeName = queryReader.GetString(1);
          string recipeDescription = queryReader.GetString(2);
          int recipeRating = queryReader.GetInt32(3);
          Recipe foundRecipe = new Recipe(recipeName, recipeDescription, recipeRating, thisRecipeId);
          recipes.Add(foundRecipe);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      return recipes;
    }
  }
}
