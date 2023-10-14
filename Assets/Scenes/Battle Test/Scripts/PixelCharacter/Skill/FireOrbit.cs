using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using battle;

public class FireOrbit : MonoBehaviour
{
    public BattleManager bm;
    public PixelHumanoid parent;
    public int damage;
    public float radius;
    public float angle;
    public float speed;

    [SerializeField]private float initialTime;
    [SerializeField]private float leftTime;

    [Header("Reference")]
    public Transform effect;

    public void Initialize(PixelHumanoid parent, int damage, float time, float radius, float speed)
    {
        this.parent = parent;
        this.bm = parent.bm;
        this.damage = damage;
        initialTime = time;
        leftTime= time;

        this.radius = radius;
        angle = 0;
        this.speed = speed;

        transform.localPosition = new Vector3(radius, 0, 0);
    }

    private void Update()
    {
        if (initialTime == 0)
        {
            Debug.LogError("initial time is ZERO");
            return;
        }
            
        leftTime -= Time.deltaTime;
        if (leftTime < 0)
            Destroy(gameObject);

        float elapsedTime = initialTime - leftTime;
        
        float angle = elapsedTime / speed;
        angle = angle - (int)angle;
        angle = angle * Mathf.PI * 2;   // radian conversion: 1.0 => 2PI

        // radius_scale becomes 1 when leftTime equals with initialTime / 2.0f
        float radius_scale = radius * (leftTime / (initialTime / 2.0f));
        radius_scale = Mathf.Min(radius_scale, radius); // cap max by default radius

        float current_radius = radius * radius_scale;

        float x = Mathf.Cos(angle) * current_radius;
        float y = Mathf.Sin(angle) * current_radius;
        transform.position = parent.transform.position + new Vector3(x, y, 0);
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            effect.rotation = Camera.main.transform.localRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            PixelHumanoid other = collision.gameObject.GetComponent<PixelHumanoid>();

            if (parent.teamIndex != other.teamIndex)
            {
                
                if (!other.IsDead())
                {
                    bm.ApplyDamage(parent, other, damage, false, Color.red);
                }
            }
        }
    }
}
