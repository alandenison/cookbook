// using Xunit;
// using System.Collections.Generic;
// using System;
// using System.Data;
// using System.Data.SqlClient;
//
// namespace Cookbook.Objects
// {
//   public class IngredientTest : IDisposable
//   {
//     public IngredientTest()
//     {
//       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=cookbook_test;Integrated Security=SSPI;";
//     }
//     public void Dispose()
//     {
//       Recipe.DeleteAll();
//       Ingredient.DeleteAll();
//       Category.DeleteAll();
//     }
//     [Fact]
//     public void Test_DatabaseEmptyAtFirst()
//     {
//       //Arrange, Act
//       int result = Ingredient.GetAll().Count;
//
//       //Assert
//       Assert.Equal(0, result);
//     }
//     [Fact]
//     public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
//     {
//       //Arrange, Act
//       Ingredient firstIngredient = new Ingredient("Kimchi");
//       Ingredient secondIngredient = new Ingredient("Kimchi");
//
//       //Assert
//       Assert.Equal(firstIngredient, secondIngredient);
//     }
//     [Fact]
//     public void Test_Save_SavesToDatabase()
//     {
//       //Arrange
//       Ingredient testIngredient = new Ingredient("Kimchi");
//
//       //Act
//       testIngredient.Save();
//       List<Ingredient> result = Ingredient.GetAll();
//       List<Ingredient> testList = new List<Ingredient>{testIngredient};
//
//       //Assert
//       Assert.Equal(testList, result);
//     }
//     [Fact]
//     public void Test_Save_AssignsIdToObject()
//     {
//       //Arrange
//       Ingredient testIngredient = new Ingredient("Kimchi");
//
//       //Act
//       testIngredient.Save();
//       Ingredient savedIngredient = Ingredient.GetAll()[0];
//
//       int result = savedIngredient.GetId();
//       int testId = testIngredient.GetId();
//
//       //Assert
//       Assert.Equal(testId, result);
//     }
//     [Fact]
//     public void Test_Find_FindsIngredientInDatabase()
//     {
//       //Arrange
//       Ingredient testIngredient = new Ingredient("Kimchi");
//       testIngredient.Save();
//
//       //Act
//       Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());
//
//       //Assert
//       Assert.Equal(testIngredient, foundIngredient);
//     }
//
//   }
//
//
// }
