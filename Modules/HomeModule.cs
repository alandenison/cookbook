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
        List<Category> AllCategories = Category.GetAll();
        return View["index.cshtml", AllCategories];
      };
      Post["/add-category"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        List<Category> AllCategories = Category.GetAll();
        return View["index.cshtml", AllCategories];
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
        Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-rating"]);
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
      Get["/recipe/{id}"] = parameters => {
        int recipeId = parameters.id;
        Recipe foundRecipe = Recipe.Find(recipeId);
        List<Category> recipeCategories = foundRecipe.GetCategoriesInRecipe();
        List<Category> allCategories = Category.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("recipe", foundRecipe);
        model.Add("categories", recipeCategories);
        model.Add("allCategories", allCategories);
        return View["recipe.cshtml", model];
      };
      Post["/recipe/{id}/add-Category"] = parameters => {
        int categoryId = Request.Form["add-category"];
        Recipe thisRecipe = Recipe.Find(parameters.id);
        thisRecipe.AddCategory(categoryId);
        return View["success.cshtml"];
      };
      Patch["/recipe/{id}/update"] = parameters => {
        Recipe SelectedRecipe = Recipe.Find(parameters.id);
        SelectedRecipe.Update(Request.Form["recipe-name"], Request.Form["recipe-description"], Request.Form["recipe-rating"]);
        return View["success.cshtml"];
      };
      // Post["/recipe/{id}/add-category"] = parameters => {
      //
      // };

    }
  }
}
