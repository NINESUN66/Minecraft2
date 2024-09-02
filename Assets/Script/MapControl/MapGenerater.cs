using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    private GameObject PlayerTransform;
    private float xMapWidth;
    private float xMapScale;
    private int xMapNum;
    private float yMapWidth;
    private float yMapScale;
    private int yMapNum;
    
    private float xTotalMapWidth;
    private float yTotalMapWidth;
    
    private Vector3 initPos;

    void Start()
    {
        PlayerTransform = GameObject.Find("Player");
        initPos = transform.position;
        xMapNum = 6;
        yMapNum = 4;
        xMapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        xMapScale = transform.localScale.x;
        xTotalMapWidth = xMapWidth * xMapNum * xMapScale;
        yMapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        yMapScale = transform.localScale.y;
        yTotalMapWidth = yMapWidth * yMapNum * yMapScale;
    }

    private void Update()
    {
        Vector3 nowPos = PlayerTransform.transform.position;

        if (nowPos.x > initPos.x + xTotalMapWidth / 2)
        {
            initPos.x += xTotalMapWidth;
            transform.position = initPos;
        }
        else if (nowPos.x < initPos.x - xTotalMapWidth / 2)
        {
            initPos.x -= xTotalMapWidth;
            transform.position = initPos;
        }

        if (nowPos.y > initPos.y + yTotalMapWidth / 2)
        {
            initPos.y += yTotalMapWidth;
            transform.position = initPos;
        }
        else if (nowPos.y < initPos.y - yTotalMapWidth / 2)
        {
            initPos.y -= yTotalMapWidth;
            transform.position = initPos;
        }
    }
}
