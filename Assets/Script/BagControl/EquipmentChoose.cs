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
            // Debug.LogWarning("�޷���ʾ��ǰװ���������� equipmentArray Ϊ�ջ� equipmentIndex ���ںϷ���Χ�ڡ�");
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
    /// ����װ���ű��е� Effect() ����
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="equipmentItem"></param>
    private void InvokeItemEffect(string itemName, GameObject equipmentItem)
    {
        // ʹ�� itemName ��ȡ�ű�����
        System.Type itemType = System.Type.GetType(itemName);

        if (itemType != null)
        {
            // ��ȡ����Ӷ�Ӧ���͵����
            MonoBehaviour itemScript = (MonoBehaviour)equipmentItem.GetComponent(itemType) ?? (MonoBehaviour)equipmentItem.AddComponent(itemType);

            // ���� Effect() ����
            itemType.GetMethod("Effect")?.Invoke(itemScript, null);
        }
        else
        {
            Debug.LogWarning($"δ�ҵ���Ϊ '{itemName}' �Ľű���");
        }
    }

    private void Use_Equipment()
    {
        if (Input.GetMouseButtonDown(0) && !bag.activeSelf &&
            equipmentArray.Count > 0 && equipmentIndex >= 0 && equipmentIndex < equipmentArray.Count)
        {
            // ��ȡ��ǰװ������������������
            GameObject equipmentItem = bag.transform.GetChild(equipmentArray[equipmentIndex]).gameObject;
            string temp = equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text;
            int number = int.Parse(temp);
            equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text = (number - 1).ToString();

            // ��ȡװ�������Ʋ����ö�Ӧ�ű��е� Effect() ����
            string itemName = equipmentItem.GetComponentInChildren<Image>().sprite.name;
            InvokeItemEffect(itemName, equipmentItem);

            if (number <= 1) itemsPutInBag.DeleteItemFromBagList(itemName);
        }
        else
        {
            // Debug.LogWarning("�޷�ʹ��װ���������� equipmentArray Ϊ�ջ� equipmentIndex ���ںϷ���Χ�ڡ�");
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
