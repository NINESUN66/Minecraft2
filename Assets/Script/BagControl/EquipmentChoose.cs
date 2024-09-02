using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentChoose : MonoBehaviour
{
    public GameObject bag;
    public GameObject Equipment;
    public ItemsPutInBag itemsPutInBag;

    protected int equipmentIndex = 0;
    protected List<int> equipmentArray = new List<int>();

    public void Update_Equipment_Array()
    {
        GameObject bag = GameObject.Find("Bag") ?? this.bag;

        equipmentArray.Clear();
        for (int i = 0; i < bag.transform.childCount; i++)
        {
            GameObject item = bag.transform.GetChild(i).gameObject;
            if (!DragItems.Is_Null(item))
            {
                equipmentArray.Add(i);
            }
        }
    }

    public void Show_Now_Equipment()
    {
        if (equipmentArray.Count > 0 && equipmentIndex >= 0 && equipmentIndex < equipmentArray.Count)
        {
            Sprite nowEquipment = bag.transform.GetChild(equipmentArray[equipmentIndex]).GetComponentInChildren<Image>().sprite;
            string nowNumber = bag.transform.GetChild(equipmentArray[equipmentIndex]).GetComponentInChildren<TextMeshProUGUI>().text;
            Equipment.GetComponentInChildren<Image>().sprite = nowEquipment;
            Equipment.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            Equipment.GetComponentInChildren<TextMeshProUGUI>().text = nowNumber;
        }
        else
        {
            Equipment.GetComponentInChildren<Image>().color = new Color(0, 0, 0, 0);
            Equipment.GetComponentInChildren<TextMeshProUGUI>().text = null;
            // Debug.LogWarning("无法显示当前装备，可能是 equipmentArray 为空或 equipmentIndex 不在合法范围内。");
        }
    }

    public void Click_Left_Arrow()
    {
        if (equipmentIndex > 0) equipmentIndex--;
        else equipmentIndex = equipmentArray.Count - 1;
    }

    public void Click_Right_Arrow()
    {
        if (equipmentIndex < equipmentArray.Count - 1) equipmentIndex++;
        else equipmentIndex = 0;
    }

    /// <summary>
    /// 调用装备脚本中的 Effect() 方法
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="equipmentItem"></param>
    private void InvokeItemEffect(string itemName, GameObject equipmentItem)
    {
        // 使用 itemName 获取脚本类型
        System.Type itemType = System.Type.GetType(itemName);

        if (itemType != null)
        {
            // 获取或添加对应类型的组件
            MonoBehaviour itemScript = (MonoBehaviour)equipmentItem.GetComponent(itemType) ?? (MonoBehaviour)equipmentItem.AddComponent(itemType);

            // 调用 Effect() 方法
            itemType.GetMethod("Effect")?.Invoke(itemScript, null);
        }
        else
        {
            Debug.LogWarning($"未找到名为 '{itemName}' 的脚本。");
        }
    }

    private void Use_Equipment()
    {
        if (Input.GetMouseButtonDown(0) && !bag.activeSelf &&
            equipmentArray.Count > 0 && equipmentIndex >= 0 && equipmentIndex < equipmentArray.Count)
        {
            // 获取当前装备的数量并更新数量
            GameObject equipmentItem = bag.transform.GetChild(equipmentArray[equipmentIndex]).gameObject;
            string temp = equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text;
            int number = int.Parse(temp);
            equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text = (number - 1).ToString();

            // 获取装备的名称并调用对应脚本中的 Effect() 方法
            string itemName = equipmentItem.GetComponentInChildren<Image>().sprite.name;
            InvokeItemEffect(itemName, equipmentItem);

            if (number <= 1) itemsPutInBag.DeleteItemFromBagList(itemName);
        }
        else
        {
            // Debug.LogWarning("无法使用装备，可能是 equipmentArray 为空或 equipmentIndex 不在合法范围内。");
        }
    }

    private void Start()
    {
        equipmentIndex = 0;
        Update_Equipment_Array();
    }

    private void Update()
    {
        Update_Equipment_Array();

        if (equipmentIndex >= equipmentArray.Count)
        {
            equipmentIndex = equipmentArray.Count > 0 ? equipmentArray.Count - 1 : 0;
        }

        if (Input.GetKeyDown("q"))
        {
            Click_Left_Arrow();
        }
        if (Input.GetKeyDown("e"))
        {
            Click_Right_Arrow();
        }
        Show_Now_Equipment();

        Use_Equipment();
    }
}
