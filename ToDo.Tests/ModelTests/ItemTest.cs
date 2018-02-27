using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System;
using System.Collections.Generic;

namespace ToDoList.Tests
{
    [TestClass]
      public class ItemTests : IDisposable
    {
        public void Dispose()
        {
            Item.DeleteAll();
            Category.DeleteAll();
        }

        public ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
          int result = Item.GetAll().Count;
          Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetDescription_ReturnsDescription_String()
        {
            string description = "Walk the dog.";
            Item newItem = new Item(description);

            string result = newItem.GetDescription();

            Assert.AreEqual(description, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ItemList()
        {
          Item testItem = new Item("Mow the lawn");
          testItem.Save();

          List<Item> result = Item.GetAll();
          List<Item> testList = new List<Item>{testItem};

          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
        {
          Item firstItem = new Item("Mow the lawn");
          Item secondItem = new Item("Mow the lawn");

          Assert.AreEqual(firstItem, secondItem);
        }

        [TestMethod]
        public void GetAll_ReturnsItems_ItemList()
        {
            string description01 = "Walk the dog";
            string description02 = "Wash the dishes";
            Item newItem1 = new Item(description01);
            Item newItem2 = new Item(description02);
            newItem1.Save();
            newItem2.Save();
            List<Item> newList = new List<Item> { newItem1, newItem2 };

            List<Item> result = Item.GetAll();

            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
          Item testItem = new Item("Mow the lawn");
          testItem.Save();

          Item foundItem = Item.Find(testItem.GetId());

          Assert.AreEqual(testItem, foundItem);
        }

        [TestMethod]
        public void Edit_UpdatesItemInDatabase_String()
        {
          string firstDescription = "Walk the dog";
          Item testItem = new Item(firstDescription);
          testItem.Save();
          string secondDescription = "Mow the lawn";

          testItem.Edit(secondDescription);

          string result = Item.Find(testItem.GetId()).GetDescription();

          Assert.AreEqual(secondDescription, result);
        }

        [TestMethod]
        public void Delete_RemovesItemFromDatabase_ItemList()
        {
            string description01 = "Walk the dog";
            string description02 = "Wash the dishes";
            Item newItem1 = new Item(description01);
            Item newItem2 = new Item(description02);
            newItem1.Save();
            newItem2.Save();
            List<Item> newList = new List<Item> { newItem1 };

            newItem2.Delete();
            List<Item> result = Item.GetAll();

            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Delete_DeletesItemAssociationsFromDatabase_ItemList()
        {
            Category testCategory = new Category("Home stuff");
            testCategory.Save();

            string testDescription = "Mow the lawn";
            Item testItem = new Item(testDescription);
            testItem.Save();

            testItem.AddCategory(testCategory);
            testItem.Delete();

            List<Item> resultCategoryItems = testCategory.GetItems();
            List<Item> testCategoryItems = new List<Item> {};

            CollectionAssert.AreEqual(testCategoryItems, resultCategoryItems);
        }

        [TestMethod]
            public void AddCategory_AddsCategoryToItem_CategoryList()
            {
              Item testItem = new Item("Mow the lawn");
              testItem.Save();

              Category testCategory = new Category("Home stuff");
              testCategory.Save();

              testItem.AddCategory(testCategory);

              List<Category> result = testItem.GetCategories();
              List<Category> testList = new List<Category>{testCategory};

              CollectionAssert.AreEqual(testList, result);
            }
    }
}
