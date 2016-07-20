using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Cookbook
{
  public class Recipe
  {
    private int _id;
    private string _name;
    private string _description;
    private int _rating;

    public Recipe(string Name, string Description, int Rating, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _description = Description;
      _rating = Rating;
    }
    public override bool Equals(System.Object otherRecipe)
    {
        if (!(otherRecipe is Recipe))
        {
          return false;
        }
        else
        {
          Recipe newRecipe = (Recipe) otherRecipe;
          bool idEquality = this.GetId() == newRecipe.GetId();
          bool nameEquality = this.GetName() == newRecipe.GetName();
          bool descriptionEquality = this.GetDescription() == newRecipe.GetDescription();
          bool ratingEquality = this.GetRating() == newRecipe.GetRating();
          return (idEquality && nameEquality && descriptionEquality && ratingEquality);
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
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetRating()
    {
      return _rating;
    }
    public void SetRating(int newRating)
    {
      _rating = newRating;
    }
    public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        string recipeDescription = rdr.GetString(2);
        int recipeRating = rdr.GetInt32(3);
        Recipe newRecipe = new Recipe(recipeName, recipeDescription, recipeRating, recipeId);
        allRecipes.Add(newRecipe);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allRecipes;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes (name, description, rating) OUTPUT INSERTED.id VALUES (@RecipeName, @RecipeDescription, @RecipeRating);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@RecipeName";
      nameParameter.Value = this.GetName();

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@RecipeDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter ratingParameter = new SqlParameter();
      ratingParameter.ParameterName = "@RecipeRating";
      ratingParameter.Value = this.GetRating();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(ratingParameter);
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

      SqlCommand cmd = new SqlCommand("DELETE FROM recipes WHERE id = @RecipeId; DELETE FROM recipe_category WHERE recipe_id = @RecipeId; DELETE FROM recipe_ingredients WHERE recipe_id = @RecipeId", conn);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();

      cmd.Parameters.Add(recipeIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM recipes; DELETE FROM recipe_category; DELETE FROM recipe_ingredients;", conn);
      cmd.ExecuteNonQuery();
    }
    public static Recipe Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = id.ToString();
      cmd.Parameters.Add(recipeIdParameter);
      rdr = cmd.ExecuteReader();

      int foundRecipeId = 0;
      string foundRecipeName = null;
      string foundRecipeDescription = null;
      int foundRecipeRating = 0;

      while(rdr.Read())
      {
        foundRecipeId = rdr.GetInt32(0);
        foundRecipeName = rdr.GetString(1);
        foundRecipeDescription = rdr.GetString(2);
        foundRecipeRating = rdr.GetInt32(3);
      }
      Recipe foundRecipe = new Recipe(foundRecipeName, foundRecipeDescription, foundRecipeRating, foundRecipeId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundRecipe;
    }
    public void AddCategory(int newCategoryId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipe_category (category_id, recipe_id) VALUES (@CategoryId, @RecipeId)", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = newCategoryId;
      cmd.Parameters.Add(categoryIdParameter);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddIngredient(Ingredient newIngredient)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipe_ingredients (ingredient_id, recipe_id) VALUES (@IngredientId, @RecipeId)", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = newIngredient.GetId();
      cmd.Parameters.Add(ingredientIdParameter);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Ingredient> GetIngredients()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT ingredient_id FROM recipe_ingredients WHERE recipe_id = @RecipeId;", conn);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> ingredientIds = new List<int> {};
      while(rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        ingredientIds.Add(ingredientId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Ingredient> ingredients = new List<Ingredient> {};
      foreach (int ingredientId in ingredientIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand ingredientQuery = new SqlCommand("SELECT * FROM ingredients WHERE id = @IngredientId;", conn);

        SqlParameter ingredientIdParameter = new SqlParameter();
        ingredientIdParameter.ParameterName = "@IngredientId";
        ingredientIdParameter.Value = ingredientId;
        ingredientQuery.Parameters.Add(ingredientIdParameter);

        queryReader = ingredientQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisIngredientId = queryReader.GetInt32(0);
          string ingredientName = queryReader.GetString(1);
          Ingredient foundIngredient = new Ingredient(ingredientName, thisIngredientId);
          ingredients.Add(foundIngredient);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      return ingredients;
    }
    public List<Category> GetCategoriesInRecipe()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT category_id FROM recipe_category WHERE recipe_id = @RecipeId;", conn);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> categoryIds = new List<int> {};
      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        categoryIds.Add(categoryId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Category> categories = new List<Category> {};
      foreach (int categoryId in categoryIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand categoryQuery = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);

        SqlParameter categoryIdParameter = new SqlParameter();
        categoryIdParameter.ParameterName = "@CategoryId";
        categoryIdParameter.Value = categoryId;
        categoryQuery.Parameters.Add(categoryIdParameter);

        queryReader = categoryQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisCategoryId = queryReader.GetInt32(0);
          string categoryName = queryReader.GetString(1);
          Category foundCategory = new Category(categoryName, thisCategoryId);
          categories.Add(foundCategory);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      return categories;
    }
    public void Update(string newName, string newDescription, int newRating)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE recipes SET name = @NewName, description = @NewDescription, rating = @NewRating OUTPUT INSERTED.name, INSERTED.description, INSERTED.rating WHERE id = @RecipeId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newDescriptionParameter = new SqlParameter();
      newDescriptionParameter.ParameterName = "@NewDescription";
      newDescriptionParameter.Value = newDescription;
      cmd.Parameters.Add(newDescriptionParameter);

      SqlParameter newRatingParameter = new SqlParameter();
      newRatingParameter.ParameterName = "@NewRating";
      newRatingParameter.Value = newRating;
      cmd.Parameters.Add(newRatingParameter);


      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
        this._description = rdr.GetString(1);
        this._rating = rdr.GetInt32(2);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
