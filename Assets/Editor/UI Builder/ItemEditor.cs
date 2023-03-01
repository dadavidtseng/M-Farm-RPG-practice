using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;


public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;
    private List<ItemDetails> itemList = new List<ItemDetails>();
    private VisualTreeAsset itemRowTemplate;
    private ScrollView itemDetailsScetion;
    private ItemDetails activeItem;

    //默認預覽圖片
    private Sprite defaultIcon;
    
    private VisualElement iconPreview;
    
    //獲得VisualElement
    private ListView itemListView;

    [MenuItem("M Studio/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // VisualElement label = new Label("Hello World! From C#");
        // root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
        
        // // A stylesheet can be added to a VisualElement.
        // // The style will be applied to the VisualElement and all of its children.
        // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/UI Builder/ItemEditor.uss");
        // VisualElement labelWithStyle = new Label("Hello World! With Style");
        // labelWithStyle.styleSheets.Add(styleSheet);
        // root.Add(labelWithStyle);
        
        //拿到模板數據
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");

        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");
        
        //變量賦值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsScetion = root.Q<ScrollView>("ItemDetails");
        iconPreview = itemDetailsScetion.Q<VisualElement>("Icon");
        
        //獲得按鍵
        root.Q<Button>("AddButton").clicked += OnAddButtonClicked;
        root.Q<Button>("DeleteButton").clicked += OnDeleteButtonClicked;
        
        //加載數據
        LoadDataBase();
        
        //生成ListView
        GenerateListView();
    }

    #region 按鍵事件
    private void OnDeleteButtonClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsScetion.visible = false;
    }

    private void OnAddButtonClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "NEW ITEM";
        newItem.itemID = 1000 + itemList.Count;
        itemList.Add(newItem);
        itemListView.Rebuild();
    }
    #endregion
    
    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");

        if (dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO)) as ItemDataList_SO;
        }

        itemList = dataBase.itemDetailsList;
        //如果不標記則無法保存數據
        EditorUtility.SetDirty(dataBase);
        // Debug.Log(itemList[0].itemID);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon != null)
                {
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                }

                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEm" : itemList[i].itemName;
            }
        };

        itemListView.fixedItemHeight = 60;
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        itemListView.onSelectionChange += OnListSecectionChange;
        
        //右側信息面板不可見
        itemDetailsScetion.visible = false;
    }

    private void OnListSecectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = selectedItem.First() as ItemDetails;
        GetItemDeatils();
        itemDetailsScetion.visible = true;
    }

    private void GetItemDeatils()
    {
        itemDetailsScetion.MarkDirtyRepaint();

        itemDetailsScetion.Q<IntegerField>("ItemID").value = activeItem.itemID;
        itemDetailsScetion.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });

        itemDetailsScetion.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsScetion.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        itemDetailsScetion.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsScetion.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;


            iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            
            itemListView.Rebuild();
        });
    }
}