using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
        public CategoryTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

       [TestMethod]
       public void GetAll_CategoriesEmptyAtFirst_0()
       {
         int result = Category.GetAll().Count;

         Assert.AreEqual(0, result);
       }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Category()
      {
        Category firstCategory = new Category("Household chores");
        Category secondCategory = new Category("Household chores");

        Assert.AreEqual(firstCategory, secondCategory);
      }

      [TestMethod]
      public void Save_SavesCategoryToDatabase_CategoryList()
      {
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        List<Category> result = Category.GetAll();
        List<Category> testList = new List<Category>{testCategory};

        CollectionAssert.AreEqual(testList, result);
      }


     [TestMethod]
     public void Save_DatabaseAssignsIdToCategory_Id()
     {
       Category testCategory = new Category("Household chores");
       testCategory.Save();

       Category savedCategory = Category.GetAll()[0];

       int result = savedCategory.GetId();
       int testId = testCategory.GetId();

       Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void GetItems_RetrievesAllItemsWithCategory_ItemList()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Item firstItem = new Item("Mow the lawn", testCategory.GetId());
      firstItem.Save();
      Item secondItem = new Item("Do the dishes", testCategory.GetId());
      secondItem.Save();


      List<Item> testItemList = new List<Item> {firstItem, secondItem};
      List<Item> resultItemList = testCategory.GetItems();

      CollectionAssert.AreEqual(testItemList, resultItemList);
    }

    [TestMethod]
    public void Find_FindsCategoryInDatabase_Category()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Category foundCategory = Category.Find(testCategory.GetId());

      Assert.AreEqual(testCategory, foundCategory);
    }

    public void Dispose()
    {
      Item.DeleteAll();
      Category.DeleteAll();
    }
  }
}
