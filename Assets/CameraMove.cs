using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var cameraPos = transform.position;
        var playerPos = player.position;
        cameraPos.y = playerPos.y;
        transform.position = cameraPos;
    }
}
