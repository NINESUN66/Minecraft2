using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttribute : MonoBehaviour
{
    public float nowHP;
    public float maxHP;
    public float nowEXP;
    public float maxEXP;
    public int level;
    public int maxLevel;
    public float damageReductionRate;
    public Image HP_Image;
    public Image EXP_Image;
    public TextMeshProUGUI Level_Text;
    public TextMeshProUGUI HP_Text;
    public ItemsPutInBag itemsPutInBag;
    public GameObject bag;

    private AudioClip hurt;
    private AudioClip levelUp;
    private AudioClip addexp;

    private void ChangeBag()
    {
        if(Input.GetKeyDown("b"))
        {
            bag.SetActive(!bag.activeSelf);
            if (bag.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    private float Get_HP_Percent()
    {
        return nowHP / maxHP;
    }

    private float Get_EXP_Percent()
    {
        return nowEXP / maxEXP;
    }

    private float Get_Level()
    {
        if(level < 10) maxEXP = 500.0f;
        else {
            maxEXP = (level - level % 10) * 100.0f;
        }

        if (level <= maxLevel)
        {
            if (nowEXP >= maxEXP)
            {
                level++;
                AudioSource.PlayClipAtPoint(levelUp, transform.position);
                nowEXP -= maxEXP;
                maxEXP = (level - level % 10) * 100.0f;
            }
        }

        return level;
    }

    public float Get_Damage(float baseDamage, float damageReductionRate = 0 , float damageIncreaseRate = 0)
    {
        if (damageReductionRate == 0)
        {
            damageReductionRate = this.damageReductionRate;
        }

        float damageToTake = baseDamage * (1 - damageReductionRate) * (1 + damageIncreaseRate);
        nowHP -= damageToTake;
        Debug.Log("baseDamage = " + baseDamage + " damageReductionRate = " + damageReductionRate);
        return damageToTake;
    }

    private void PlayerDead()
    {
        if (nowHP <= 0.0f)
        {
            HP_Image.fillAmount = 0f;
            Destroy(transform.parent.gameObject);
            Debug.Log("ÄãËÀÁË");
        }
    }

    private void Show_HP()
    {
        HP_Text.text = Mathf.Ceil(nowHP).ToString() + " / " + Mathf.Ceil(maxHP).ToString();
    }

    private void Check_Zero()
    {
        GameObject bag = GameObject.Find("Bag") ?? this.bag;
        if (bag == null)
        {
            Debug.LogError("Î´ÕÒµ½±³°ü");
            return;
        }
        else
        {
            foreach (Transform item in bag.transform)
            {
                if(item.GetComponentInChildren<TextMeshProUGUI>().text == "0")
                {
                    item.GetComponentInChildren<Image>().color = new Color(0,0,0,0);
                    item.GetComponentInChildren<TextMeshProUGUI>().text = null;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyKnife")
        {
            Get_Damage(collision.gameObject.GetComponent<BaseKnife>().damage);
            AudioSource.PlayClipAtPoint(hurt, transform.position);
            if (nowHP <= 0.0f)
            {
                PlayerDead();
            }
        }

        if (collision.gameObject.tag == "EnemyBody")
        {
            Get_Damage(collision.transform.parent.GetComponentInChildren<BaseKnife>().damage,0 ,collision.GetComponent<BaseEnemy>().touchBodyDamageRate);
            AudioSource.PlayClipAtPoint(hurt, transform.position);
            if (nowHP <= 0.0f)
            {
                PlayerDead();
            }
        }

        if (collision.transform.tag == "ExpBall")
        {
            nowEXP += collision.gameObject.GetComponent<ExpAttribute>().Experience;

            float pitch = Random.Range(0.8f, 1.2f);
            float volume = 0.1f;
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = addexp;
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource, addexp.length);

            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "ExpBottle")
        {
            int addLevel = collision.gameObject.GetComponent<ExpBottle>().addLevel;
            level += addLevel;
            if (level > maxLevel) level = maxLevel;

            AudioSource.PlayClipAtPoint(levelUp, transform.position);
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "RottenFlesh")
        {
            nowHP += collision.gameObject.GetComponent<RottenFlesh>().returnHP;
            if (nowHP > maxHP) maxHP = nowHP;

            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "ItemCanUse")
        {
            itemsPutInBag.ItemsImagePutInBag(collision);
            Destroy(collision.gameObject);
        }
    }

    private void Start()
    {
        nowHP = maxHP;
        damageReductionRate = 0f;
        hurt = Resources.Load<AudioClip>("Audio/hurt");
        levelUp = Resources.Load<AudioClip>("Audio/levelup");
        addexp = Resources.Load<AudioClip>("Audio/addexp");
        Check_Zero();
    }

    private void Update()
    {
        HP_Image.fillAmount = Get_HP_Percent();
        EXP_Image.fillAmount = Get_EXP_Percent();
        Level_Text.text = Get_Level().ToString();

        Show_HP();
        ChangeBag();
        Check_Zero();

        if (nowHP <= 0.0f)
        {
            PlayerDead();
        }
    }
}
