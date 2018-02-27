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

        // [TestMethod]
        // public void GetItems_RetrievesAllItemsWithCategory_ItemList()
        // {
        //   Category testCategory = new Category("Household chores");
        //   testCategory.Save();
        //
        //   Item firstItem = new Item("Mow the lawn", testCategory.GetId());
        //   firstItem.Save();
        //   Item secondItem = new Item("Do the dishes", testCategory.GetId());
        //   secondItem.Save();
        //
        //
        //   List<Item> testItemList = new List<Item> {firstItem, secondItem};
        //   List<Item> resultItemList = testCategory.GetItems();
        //
        //   CollectionAssert.AreEqual(testItemList, resultItemList);
        // }

        [TestMethod]
        public void Find_FindsCategoryInDatabase_Category()
        {
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            Category foundCategory = Category.Find(testCategory.GetId());

            Assert.AreEqual(testCategory, foundCategory);
        }

        [TestMethod]
        public void Test_AddItem_AddsItemToCategory()
        {
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            Item testItem = new Item("Mow the lawn");
            testItem.Save();

            Item testItem2 = new Item("Water the garden");
            testItem2.Save();

            testCategory.AddItem(testItem);
            testCategory.AddItem(testItem2);

            List<Item> result = testCategory.GetItems();
            List<Item> testList = new List<Item>{testItem, testItem2};

            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetItems_ReturnsAllCategoryItems_ItemList()
        {
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            Item testItem1 = new Item("Mow the lawn");
            testItem1.Save();

            Item testItem2 = new Item("Buy plane ticket");
            testItem2.Save();

            testCategory.AddItem(testItem1);
            List<Item> savedItems = testCategory.GetItems();
            List<Item> testList = new List<Item> {testItem1};

            CollectionAssert.AreEqual(testList, savedItems);
        }

        [TestMethod]
        public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
        {
            Item testItem = new Item("Mow the lawn");
            testItem.Save();

            string testName = "Home stuff";
            Category testCategory = new Category(testName);
            testCategory.Save();

            testCategory.AddItem(testItem);
            testCategory.Delete();

            List<Category> resultItemCategories = testItem.GetCategories();
            List<Category> testItemCategories = new List<Category> {};

            CollectionAssert.AreEqual(testItemCategories, resultItemCategories);
        }

        public void Dispose()
        {
            Item.DeleteAll();
            Category.DeleteAll();
        }
    }
}
