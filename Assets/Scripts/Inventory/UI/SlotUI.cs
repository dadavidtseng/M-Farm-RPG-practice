using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour
    {
        [Header("組件獲取")] 
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image slotHightlight;
        [SerializeField] private Button button;

        [Header("格子類型")] 
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;

        //物品信息
        public ItemDetails itemDetails;
        public int itemAmount;

        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有數量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 讓Slot更新為空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }
    }
}
