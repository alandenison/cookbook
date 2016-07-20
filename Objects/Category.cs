using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Cookbook
{
  public class Category
  {
    private int _id;
    private string _name;

    public Category(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherCategory)
    {
        if (!(otherCategory is Category))
        {
          return false;
        }
        else
        {
          Category newCategory = (Category) otherCategory;
          bool idEquality = this.GetId() == newCategory.GetId();
          bool nameEquality = this.GetName() == newCategory.GetName();
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
    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCategories;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@CategoryName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CategoryName";
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

      SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM recipe_category WHERE category_id = @CategoryId;", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM categories; DELETE FROM recipe_category", conn);
      cmd.ExecuteNonQuery();
    }

    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = id.ToString();
      cmd.Parameters.Add(categoryIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCategoryId = 0;
      string foundCategoryDescription = null;

      while(rdr.Read())
      {
        foundCategoryId = rdr.GetInt32(0);
        foundCategoryDescription = rdr.GetString(1);
      }
      Category foundCategory = new Category(foundCategoryDescription, foundCategoryId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCategory;
    }
    public List<Recipe> GetRecipeByCategory()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM categories JOIN recipe_category ON (categories.id = recipe_category.category_id) JOIN recipes ON (recipe_category.recipe_id = recipes.id) WHERE categories.id = @CategoryId", conn);
      SqlParameter CategoryIdParam = new SqlParameter();
      CategoryIdParam.ParameterName = "@CategoryId";
      CategoryIdParam.Value = this.GetId().ToString();

      cmd.Parameters.Add(CategoryIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Recipe> recipes = new List<Recipe>{};

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        string recipeDescription = rdr.GetString(2);
        int recipeRating = rdr.GetInt32(3);
        Recipe newRecipe = new Recipe(recipeName, recipeDescription, recipeRating, recipeId);
        recipes.Add(newRecipe);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return recipes;
    }
    // public List<Recipe> GetRecipeByCategory()
    // {
    //   SqlConnection conn = DB.Connection();
    //   SqlDataReader rdr = null;
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT recipe_id FROM recipe_category WHERE category_id = @CategoryId;", conn);
    //   SqlParameter categoryIdParameter = new SqlParameter();
    //   categoryIdParameter.ParameterName = "@CategoryId";
    //   categoryIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(categoryIdParameter);
    //
    //   rdr = cmd.ExecuteReader();
    //
    //   List<int> recipeIds = new List<int> {};
    //   while(rdr.Read())
    //   {
    //     int recipeId = rdr.GetInt32(0);
    //     recipeIds.Add(recipeId);
    //   }
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //
    //   List<Recipe> recipes = new List<Recipe> {};
    //   foreach (int recipeId in recipeIds)
    //   {
    //     SqlDataReader queryReader = null;
    //     SqlCommand recipeQuery = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);
    //
    //     SqlParameter recipeIdParameter = new SqlParameter();
    //     recipeIdParameter.ParameterName = "@RecipeId";
    //     recipeIdParameter.Value = recipeId;
    //     recipeQuery.Parameters.Add(recipeIdParameter);
    //
    //     queryReader = recipeQuery.ExecuteReader();
    //     while (queryReader.Read())
    //     {
    //       int thisRecipeId = queryReader.GetInt32(0);
    //       string recipeName = queryReader.GetString(1);
    //       string recipeDescription = queryReader.GetString(2);
    //       int recipeRating = queryReader.GetInt32(3);
    //       Recipe foundRecipe = new Recipe(recipeName, recipeDescription, recipeRating, thisRecipeId);
    //       recipes.Add(foundRecipe);
    //     }
    //     if (queryReader != null)
    //     {
    //       queryReader.Close();
    //     }
    //   }
    //   return recipes;
    // }
    public void AddRecipeToCategory(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipe_category (category_id, recipe_id) VALUES (@CategoryId, @RecipeId)", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();
      cmd.Parameters.Add(categoryIdParameter);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = newRecipe.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
