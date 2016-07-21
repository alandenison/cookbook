using System.Collections.Generic;
using System;
using Nancy;

namespace Cookbook
{
  public class HomeModule : NancyModule

  {


    public HomeModule()

    {

      Get["/"] = _ => {

        return View["index.cshtml", Recipe.GetAll()];
      };
      Post["/add-category"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };
      Get["/category/{id}"] = parameters => {
        int categoryId = parameters.id;
        Category categoryFound = Category.Find(categoryId);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Recipe> CategoryRecipes = categoryFound.GetRecipeByCategory();
        model.Add("category", categoryFound);
        model.Add("recipes", CategoryRecipes);
        return View["category.cshtml", model];
      };
      Post["/category/{id}/add-recipe"] = parameters => {
        Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-image"], Request.Form["recipe-rating"]);
        newRecipe.Save();
        int categoryId = parameters.id;
        Category categoryFound = Category.Find(categoryId);
        categoryFound.AddRecipeToCategory(newRecipe);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Recipe> CategoryRecipes = categoryFound.GetRecipeByCategory();
        model.Add("category", categoryFound);
        model.Add("recipes", CategoryRecipes);
        return View["category.cshtml", model];
      };
      Post["/category/delete"] = _ => {
        int categoryId = Request.Form["category-name"];
        Category categoryFound = Category.Find(categoryId);
        categoryFound.Delete();
        return View["categories.cshtml", Category.GetAll()];
      };
      Get["/categories"] = _ => {
        return View["/categories", Category.GetAll()];
      };
      Get["/recipe/{id}"] = parameters => {
        Console.WriteLine("hi");
        int recipeId = parameters.id;
        Console.WriteLine("there");
        Recipe foundRecipe = Recipe.Find(recipeId);
        List<Category> recipeCategories = foundRecipe.GetCategoriesInRecipe();
        List<Category> allCategories = Category.GetAll();
        List<Ingredient> recipeIngredients = foundRecipe.GetIngredients();
        List<Ingredient> allIngredients = Ingredient.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("recipe", foundRecipe);
        model.Add("categories", recipeCategories);
        model.Add("allCategories", allCategories);
        model.Add("ingredients", recipeIngredients);
        model.Add("allIngredients", allIngredients);
        return View["recipe.cshtml", model];
      };
      Post["/recipe/{id}/add-Category"] = parameters => {
        int categoryId = Request.Form["add-category"];
        int recipeId = parameters.id;
        Recipe thisRecipe = Recipe.Find(parameters.id);
        thisRecipe.AddCategory(categoryId);
        return View["category-success.cshtml", recipeId];
      };
      Patch["/recipe/{id}/update"] = parameters => {
        int recipeId = parameters.id;
        Recipe SelectedRecipe = Recipe.Find(parameters.id);
        SelectedRecipe.Update(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-rating"], Request.Form["recipe-image"]);
        return View["category-success.cshtml", recipeId];
      };
      Post["/delete-category-from-recipe"] = _ => {
        int categoryId = Request.Form["recipe-category-name"];
        Recipe SelectedRecipe = Recipe.Find(Request.Form["recipe-update"]);
        SelectedRecipe.DeleteCategoryFromRecipe(categoryId);
        int recipeId = SelectedRecipe.GetId();
        return View["category-success.cshtml", recipeId];
      };
      Post["/delete-ingredient-from-recipe"] = _ => {
        int ingredientId = Request.Form["recipe-ingredient-name"];
        Recipe SelectedRecipe = Recipe.Find(Request.Form["recipe-update"]);
        SelectedRecipe.DeleteIngredientFromRecipe(ingredientId);
        int recipeId = SelectedRecipe.GetId();
        return View["category-success.cshtml", recipeId];
      };
      Post["/add-new-recipe"] = parameter => {
        Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-rating"], Request.Form["recipe-image"]);
        newRecipe.Save();
        return View["/recipe-success.cshtml", newRecipe.GetId()];
      };
      Post["/delete-all-recipes"] = _ => {
        Recipe.DeleteAll();
        return View["index.cshtml",Recipe.GetAll()];
      };
      Post["/delete-all-categories"] = _ => {
        Category.DeleteAll();
        return View["index.cshtml",Recipe.GetAll()];
      };
      Get["/ingredients"] = _ => {
        return View["ingredients.cshtml", Ingredient.GetAll()];
      };
      Post["/add-ingredient"] = _ => {
        Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"]);
        newIngredient.Save();
        return View["ingredients.cshtml", Ingredient.GetAll()];
      };
      Post["/recipe/{id}/add-Ingredient"] = parameters => {
        int ingredientId = Request.Form["add-ingredient"];
        int recipeId = parameters.id;
        Recipe thisRecipe = Recipe.Find(parameters.id);
        thisRecipe.AddIngredient(ingredientId);
        return View["ingredient-success.cshtml", recipeId];
      };
      Get["/ingredients"] = _ => {
        return View["ingredients.cshtml", Ingredient.GetAll()];
      };
      Get["/ingredient/{id}"]= parameters => {
        int ingredientId = parameters.id;
        Ingredient ingredientFound = Ingredient.Find(ingredientId);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Recipe> IngredientRecipes = ingredientFound.GetRecipes();
        model.Add("ingredient", ingredientFound);
        model.Add("recipes", IngredientRecipes);
        return View["ingredient.cshtml", model];
      };
      Post["/ingredient/delete"] = _ => {
        int ingredientId = Request.Form["ingredient-name"];
        Ingredient ingredientFound = Ingredient.Find(ingredientId);
        ingredientFound.Delete();
        return View["ingredients.cshtml", Ingredient.GetAll()];
      };
      Post["/search"] = parameters => {
        string searchWord = Request.Form["recipe-search-name"];
        Console.WriteLine(searchWord);
        string updatedWord = searchWord.Replace(" ", "+");
        Console.WriteLine(updatedWord);
        string searchName = ("http://www.recipe.com/search/?searchType=recipe&searchTerm=" + updatedWord);
        return View["searchlink.cshtml", searchName];
      };

      Get["/searchlink"] = parameters => {

        return View["searchlink.cshtml"];
      };
    }
  }
}
