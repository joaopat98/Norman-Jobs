using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

public class Weapon : MonoBehaviour
{


    // Start is called before the first frame update
    public WeaponType type;

    public AudioClip weaponSound;
    public float weaponSoundVolume;

 
    public bool Held;
    public float Damage;
    public int Ammo;
    public GameObject destroyVFX;
    public AudioClip destroyWeaponAudio;

    // private float shootTimer = 0;
    protected GameObject thePlayer;
    protected MouseMovement mouse;
    protected SpriteRenderer spr;
    protected Color oldColor;
    private bool enter;
    private float originalAmmo;

    public float BreakSoundVolume = 0.8f;
    public AudioClip BreakSound;
    protected void Start()
    {
        Held = false;
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        mouse = thePlayer.GetComponent<MouseMovement>();
        spr = GetComponent<SpriteRenderer>();
        oldColor = spr.color;
        enter = true;
        originalAmmo = Ammo;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (Held)
        {
            transform.position = thePlayer.transform.position;
            if (Ammo <= originalAmmo * 0.2f)
            {
                if (enter)
                {
                    enter = false;
                    StartCoroutine(AmmonTimerColor());
                }
                
            }
        }
    }

    void LateUpdate()
    {
        if (thePlayer != null)
        {
            if (mouse.GetWeapon() == this)
                spr.sortingOrder = thePlayer.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

    public virtual void Shoot(Vector3 origin, Vector3 direction)
    {

    }

    private IEnumerator AmmonTimerColor()
    {
        int temp = 0;
        float yellowTime = 0.7f;

        while (true)
        {

            spr.color = Color.red;
            yield return new WaitForSeconds(yellowTime);
            spr.color = oldColor;
            yield return new WaitForSeconds(yellowTime);
            temp++;

        }
       
    }

    protected void Die()
    {
        GameObject explosion = Instantiate(destroyVFX, transform.position, transform.rotation);
        Destroy(explosion, 0.3f);
        AudioSource.PlayClipAtPoint(destroyWeaponAudio, Camera.main.transform.position, 3.0f);

    }

}