using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Enemy enemy;

    Image healthBar;

    public float maxHP;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = enemy.HP;
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = enemy.HP / maxHP;
    }
}
