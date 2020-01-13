using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollerSpeed = 0.3f;
    Material myMaterial;
    Vector2 offset;
    Vector3 initPos;


    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0, backgroundScrollerSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.y = Camera.main.transform.position.y;
        pos.x = -Camera.main.transform.position.x / 10 + initPos.x;
        transform.position = pos;
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}