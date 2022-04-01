using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InventoryTests
    {
        private InventoryController inventory;
        [SetUp]
        public void Setup()
        {
            inventory = new InventoryController();
        }
        // A Test behaves as an ordinary method
        [Test]
        public void AddingUpdatesCount()
        {
            bool updated = false;
            inventory.OnItemCountUpdated += (pickupType, count) =>
            {
                if(pickupType == ItemType.DamageAmmo)
                {
                    Assert.AreEqual(1, count);
                    updated = true;
                }
            };
            inventory.Add(ItemType.DamageAmmo);
            Assert.IsTrue(updated);
        }
        
        [Test]
        public void CanAddReturnFalseAtMax()
        {
            for(int i=0; i<InventoryController. MAX_NUMBER_PER_CATEGORY; i++)
            {
                if(inventory.CanAdd(ItemType.DamageAmmo) == false)
                {
                    Assert.Fail();
                }

                inventory.Add(ItemType.DamageAmmo);
            }

            Assert.IsFalse(inventory.CanAdd(ItemType.DamageAmmo));
        }
        [Test]
        public void RemovingUpdatesCount()
        {
            bool updated = false;
            inventory.Add(ItemType.DamageAmmo);

            inventory.OnItemCountUpdated += (pickupType, count) =>
            {
                Assert.AreEqual(0, count);
                updated = true;
            };

            inventory.Remove(ItemType.DamageAmmo);
            Assert.IsTrue(updated);
        }
        [Test]
        public void CannotRemoveWithoutNoContents()
        {
            Assert.IsFalse(inventory.CanRemove(ItemType.Gun));
        }
        [Test]
        public void RemoveAllReturnsCorrectCount()
        {
            bool updated = false;
            inventory.Add(ItemType.DamageAmmo);
            inventory.Add(ItemType.DamageAmmo);
            inventory.Add(ItemType.DamageAmmo);

            inventory.OnItemCountUpdated += (type, count) =>
            {
                if(type == ItemType.DamageAmmo)
                {
                    Assert.AreEqual(0, count);
                    updated = true;
                }                
            };

            inventory.RemoveAll(ItemType.DamageAmmo);
            Assert.IsTrue(updated);
        }
    }
}
