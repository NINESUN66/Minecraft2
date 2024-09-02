using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public enum ItemsCanBeUse
{
    EnderPearl,
    TNT,
    StrengthPotion,
    EnchantedGoldenApple,
    WeaknessPotion,
    SlownessPotion,
    SwiftnessPotion
}

public class ItemsPutInBag : MonoBehaviour
{
    public GameObject bag;
    public GameObject itemPrefab;
    private Dictionary<string, GameObject> itemsInBag = new Dictionary<string, GameObject>();

    private bool IsItemUsable(string itemName)
    {
        string cleanName = Regex.Replace(itemName, @"\s*\(.*?\)$", "");
        return System.Enum.TryParse(cleanName, true, out ItemsCanBeUse _);
    }

    public void DeleteItemFromBagList(string itemName)
    {
        if (itemsInBag.ContainsKey(itemName)) {
            itemsInBag.Remove(itemName);
        }
        else
        {
            Debug.LogError("要删除的物品不存在" + itemName);
        }
    }

    public void ItemsImagePutInBag(Collider2D collision)
    {
        string itemName = collision.gameObject.name;
        itemName = Regex.Replace(itemName, @"\s*\(.*?\)$", "");
        if (IsItemUsable(itemName))
        {
            if (!itemsInBag.ContainsKey(itemName))
            {
                Sprite itemSprite = Resources.Load<Sprite>($"Items/{itemName}");

                for (int i = 0; i < bag.transform.childCount; i ++)
                {
                    Transform child = bag.transform.GetChild(i);
                    if(DragItems.Is_Null(child.gameObject))
                    {
                        child.GetComponentInChildren<Image>().sprite = itemSprite;
                        child.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                        var num = child.transform.Find("Num").GetComponentInChildren<TextMeshProUGUI>();
                        num.text = "1";
                        itemsInBag[itemName] = child.gameObject;
                        break;
                    }
                }
            }
            else 
            {
                GameObject existingItemInBag = itemsInBag[itemName];
                var num = existingItemInBag.transform.Find("Num").GetComponentInChildren<TextMeshProUGUI>();
                int currentCount = int.Parse(num.text);
                num.text = (currentCount + 1).ToString();
            }
        }
    }

    private void Start()
    {
        if (itemPrefab == null)
        {
            Debug.LogError("ItemInBag预制体未设置");
        }
    }
}
