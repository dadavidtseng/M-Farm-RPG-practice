using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{ 
    public class InventoryManager : Singleton<InventoryManager> 
    { 
        [Header("物品數據")]
        public ItemDataList_SO itemDataList_SO;
        [Header("背包數據")] 
        public InventoryBag_SO playerBag;
        
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
            //背包是否有空位
            //是否已經有該物品
            InventoryItem newItem = new InventoryItem();
            newItem.itemID = item.itemID;
            newItem.itemAmount = 1;

            playerBag.itemList[0] = newItem;
            Debug.Log(GetItemDetails(item.itemID).itemID + "Name: " + GetItemDetails(item.itemID).itemName);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
        }
    }
}