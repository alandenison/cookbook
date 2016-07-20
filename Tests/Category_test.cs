using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cookbook.Objects
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=cookbook_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Recipe.DeleteAll();
      Ingredient.DeleteAll();
      Category.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      //Arrange, Act
      Category firstCategory = new Category("Korean Fusion");
      Category secondCategory = new Category("Korean Fusion");

      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Category testCategory = new Category("Korean Fusion");

      //Act
      testCategory.Save();
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Category testCategory = new Category("Korean Fusion");

      //Act
      testCategory.Save();
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("Korean Fusion");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }
    [Fact]
    public void Test_EqualOverrideTrueForSameName()
    {
      //Arrange, Act
      Category firstCategory = new Category("Korean Fusion");
      Category secondCategory = new Category("Korean Fusion");

      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }
    [Fact]
    public void Test_AddRecipeToCategory_AddsRecipeToCategoryk()
    {
      //Arrange
      Category testCategory = new Category("Korean Fusion");
      testCategory.Save();

      Recipe testRecipe = new Recipe("Korean Tacos", "yummy tacos", 100);
      testRecipe.Save();

      //Act
      testCategory.AddRecipeToCategory(testRecipe);

      List<Recipe> result = testCategory.GetRecipeByCategory();
      List<Recipe> testList = new List<Recipe>{testRecipe};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_GetRecipesByCategory_ReturnsAllRecipies()
    {
      //Arrange
      Category testCategory = new Category("Korean Fustion");
      testCategory.Save();

      Recipe testRecipe1 = new Recipe("Korean Tacos", "tacos from a korean guy", 99);
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("Korean BBQ", "BBQ from Korea", 98);
      testRecipe2.Save();

      //Act
      testCategory.AddRecipeToCategory(testRecipe1);
      List<Recipe> result = testCategory.GetRecipeByCategory();
      List<Recipe> testList = new List<Recipe> {testRecipe1};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Delete_DeletesCategoryAssociationsFromDatabase()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Korean Tacos", "tacos from a korean guy", 99);
      testRecipe.Save();


      Category testCategory = new Category("Korean Fusion");
      testCategory.Save();

      //Act
      testCategory.AddRecipeToCategory(testRecipe);
      testCategory.Delete();

      List<Recipe> resultRecipeCategorys = testCategory.GetRecipeByCategory();
      List<Recipe> testRecipeCategorys = new List<Recipe> {};

      //Assert
      Assert.Equal(testRecipeCategorys, resultRecipeCategorys);
    }
  }
}
