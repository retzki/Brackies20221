using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DreamType
{
    GOOD,
    BAD
}

public class Dream : MonoBehaviour
{
    DreamType dreamType;
    Transform dreamTarget;
    Player player;
    public Sprite goodDream;
    public Sprite badDream;
    [SerializeField] private SpriteRenderer m_realDreamSpriteRenderer;
    [SerializeField] private GameObject dreamPoofParticle;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, dreamTarget.position, Time.deltaTime * 2);
        if (Vector2.Distance(transform.position, dreamTarget.position) > 0.1f)
        {
            return;
        }
        switch (dreamType)
        {
            case DreamType.GOOD:
                player.points++;
                break;
            case DreamType.BAD:
                player.points--;
                break;
            default:
                break;
        }
        Destroy(gameObject);
        player.UpdateScoreText(player.points);
        SpawnPoofParticles();
    }

    public DreamType GetDreamType()
    {
        return dreamType;
    }

    public void Init(Transform trans, Player _player)
    {
        dreamTarget = trans;
        dreamType = (DreamType)Random.Range(0, System.Enum.GetValues(typeof(DreamType)).Length);
        player = _player;
        if (dreamType == DreamType.BAD)
        {
            m_realDreamSpriteRenderer.sprite = badDream;
        }
        else
        {
            m_realDreamSpriteRenderer.sprite = goodDream;
        }
    }

    public void SpawnPoofParticles()
    {
        Instantiate(dreamPoofParticle, transform.position, dreamPoofParticle.transform.rotation);
    }
}
