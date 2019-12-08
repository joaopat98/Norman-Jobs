using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthSystem
{
    void Hit(GameObject obj, float value);
    bool isAlive();
}
