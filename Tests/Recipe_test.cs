// using Xunit;
// using System.Collections.Generic;
// using System;
// using System.Data;
// using System.Data.SqlClient;
//
// namespace Cookbook.Objects
// {
//   public class RecipeTest : IDisposable
//   {
//     public RecipeTest()
//     {
//       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=cookbook_test;Integrated Security=SSPI;";
//     }
//
//     public void Dispose()
//     {
//       Recipe.DeleteAll();
//       Ingredient.DeleteAll();
//       Category.DeleteAll();
//     }
//
//     [Fact]
//     public void Test_DatabaseEmptyAtFirst()
//     {
//       //Arrange, Act
//       int result = Recipe.GetAll().Count;
//
//       //Assert
//       Assert.Equal(0, result);
//     }
//
//     [Fact]
//     public void Test_Equal_ReturnsTrueIfTheSame()
//     {
//       //Arrange, Act
//       Recipe firstRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//       Recipe secondRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//
//       //Assert
//       Assert.Equal(firstRecipe, secondRecipe);
//     }
//
//     [Fact]
//     public void Test_Save_SavesToDatabase()
//     {
//       //Arrange
//       Recipe testRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//
//       //Act
//       testRecipe.Save();
//       List<Recipe> result = Recipe.GetAll();
//       List<Recipe> testList = new List<Recipe>{testRecipe};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_Save_AssignsIdToObject()
//     {
//       //Arrange
//       Recipe testRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//
//       //Act
//       testRecipe.Save();
//       Recipe savedRecipe = Recipe.GetAll()[0];
//
//       int result = savedRecipe.GetId();
//       int testId = testRecipe.GetId();
//
//       //Assert
//       Assert.Equal(testId, result);
//     }
//
//     [Fact]
//     public void Test_Find_FindsRecipeInDatabase()
//     {
//       //Arrange
//       Recipe testRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//       testRecipe.Save();
//
//       //Act
//       Recipe foundRecipe = Recipe.Find(testRecipe.GetId());
//
//       //Assert
//       Assert.Equal(testRecipe, foundRecipe);
//     }
//
//     [Fact]
//     public void Test_AddIngredient_AddsIngredientToRecipe()
//     {
//       //Arrange
//       Recipe testRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//       testRecipe.Save();
//
//       Ingredient testIngredient = new Ingredient("Kim chi");
//       testIngredient.Save();
//
//       //Act
//       testRecipe.AddIngredient(testIngredient.GetId());
//
//       List<Ingredient> result = testRecipe.GetIngredients();
//       List<Ingredient> testList = new List<Ingredient>{testIngredient};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_GetIngredients_ReturnsAllRecipeIngredients()
//     {
//       //Arrange
//       Recipe testRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//       testRecipe.Save();
//
//       Ingredient testIngredient1 = new Ingredient("Kim chi");
//       testIngredient1.Save();
//
//       Ingredient testIngredient2 = new Ingredient("Pork");
//       testIngredient2.Save();
//
//       //Act
//       testRecipe.AddIngredient(testIngredient1.GetId());
//       List<Ingredient> result = testRecipe.GetIngredients();
//       List<Ingredient> testList = new List<Ingredient> {testIngredient1};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//
//     [Fact]
//     public void Test_Delete_DeletesRecipeAssociationsFromDatabase()
//     {
//       //Arrange
//       Ingredient testIngredient = new Ingredient("Kim chi");
//       testIngredient.Save();
//
//       Recipe testRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//       testRecipe.Save();
//
//       //Act
//       testRecipe.AddIngredient(testIngredient.GetId());
//       testRecipe.Delete();
//
//       List<Recipe> resultIngredientRecipes = testIngredient.GetRecipes();
//       List<Recipe> testIngredientRecipes = new List<Recipe> {};
//
//       //Assert
//       Assert.Equal(testIngredientRecipes, resultIngredientRecipes);
//     }
//     [Fact]
//     public void Test_Update_UpdatesRecipeInDatabase()
//     {
//       //Arrange
//       Recipe newRecipe = new Recipe("Korean tacos", "These tacos are delicious", 100);
//       newRecipe.Save();
//       string newName = "Korean Burrito";
//       string newDescription = "burrito from Korea";
//       int newRating = 99;
//
//       //Act
//       newRecipe.Update(newName, newDescription, newRating);
//       string resultNewName = newRecipe.GetName();
//       string resultNewDescription = newRecipe.GetDescription();
//       int resultNewRating = newRecipe.GetRating();
//
//       //Assert
//       Assert.Equal(newName, resultNewName);
//       Assert.Equal(newDescription, resultNewDescription);
//       Assert.Equal(newRating, resultNewRating);
//     }
//   }
// }
