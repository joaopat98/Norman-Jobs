using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollerSpeed = 0.3f;
    Material myMaterial;
    Vector2 offset;


    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0, backgroundScrollerSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.y = Camera.main.transform.position.y;
        transform.position = pos;
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}