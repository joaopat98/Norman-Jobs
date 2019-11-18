using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * moveSpeed;
    }
}
