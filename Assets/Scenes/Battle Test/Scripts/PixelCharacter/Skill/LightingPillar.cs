using battle;
using UnityEngine;

public class LightingPillar : MonoBehaviour
{
    public BattleManager bm;
    public PixelHumanoid parent;
    public uint targetId;
    public int damage;

    public void Initialize(BattleManager bm, PixelHumanoid parent, uint targetId, int damage)
    {
        this.bm = bm;
        this.parent = parent;
        this.targetId = targetId;
        this.damage = damage;

        PixelCharacter target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);

        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            bm.ApplyDamage(parent, target, this.damage, true, Color.red);
            transform.position = target.transform.position;
        }
    }

    [SerializeField]
    private float m_leftTime;
    private void Update()
    {
        m_leftTime -= Time.deltaTime;
        if (m_leftTime <= 0)
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.localRotation;
        }
    }
}