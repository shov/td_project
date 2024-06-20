using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity_old : MonoBehaviour
{
    // Health
    public int maxHP = 100;
    public int hp = 100;

    // HP Bar
    public Image hpBar;
    public Camera fieldCam;
    public Gradient hpGradient;


    // Events
    public delegate void OnDeath(Entity_old entity);
    public event OnDeath onDeath;

    // Movement and Agro
    public string relativeEnemyTag;
    public float approachRange = 0.5f;

    protected virtual void Start()
    {
        hp = maxHP;

        if (hpBar != null)
        {
            hpBar.fillAmount = 1;
            hpBar.color = hpGradient.Evaluate(hpBar.fillAmount);
        }
    }

    protected virtual void LateUpdate()
    {
        // Look at the camera
        hpBar?.transform.LookAt(fieldCam.transform.position);
    }

    public void SetFieldCam(Camera fieldCam)
    {
        this.fieldCam = fieldCam;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpBar.fillAmount = (float)hp / maxHP;
        // Set the color of the bar based on the gradient
        hpBar.color = hpGradient.Evaluate(hpBar.fillAmount);

        if (hp > 0) return;
        // Invoke the event OnDeath and then destroy the object
        onDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
