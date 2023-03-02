using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品數據")] public ItemDataList_SO itemDataList_SO;
        [Header("背包數據")] public InventoryBag_SO playerBag;

        /// <summary>
        /// 通過ID返回物品信息
        /// </summary>
        /// <param name="ID">Item ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// 添加物品到Player背包裡
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy">是否要銷毀物品</param>
        public void AddItem(Item item, bool toDestroy)
        {
            //是否已經有該物品
            var index = GetItemIndexInBag(item.itemID);
            
            AddItemAtIndex(item.itemID, index, 1);
            
            Debug.Log(GetItemDetails(item.itemID).itemID + "Name: " + GetItemDetails(item.itemID).itemName);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
        }

        /// <summary>
        /// 檢查背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 通過物品ID找到背包已有物品位置
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>-1則沒有這個物品否則返回序號</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 在指定背包序號位置添加物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">序號</param>
        /// <param name="amount">數量</param>
        private void AddItemAtIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity())    //背包沒有這個物品，同時背包有空位
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else    //背包有這個物品
            {
                int currentAmount = playerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };

                playerBag.itemList[index] = item;
            }
        }
    }
}