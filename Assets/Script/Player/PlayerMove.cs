using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    public float playerMoveSpeed = 1.0f;

    private void Player_Move()
    {

        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        transform.Translate(Horizontal * playerMoveSpeed * Time.deltaTime, Vertical * playerMoveSpeed * Time.deltaTime, 0);
    }

    private void Update()
    {
        Player_Move();
    }
}
