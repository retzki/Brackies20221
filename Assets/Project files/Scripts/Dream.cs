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
    public Sprite goodDream;
    public Sprite badDream;
    [SerializeField] private SpriteRenderer m_realDreamSpriteRenderer;
    [SerializeField] private GameObject dreamPoofParticle;
    float speed;
    public bool isDreamScanned = false;
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, dreamTarget.position, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, dreamTarget.position) > 0.1f)
        {
            return;
        }
        if(isDreamScanned)
        {
            switch (dreamType)
            {
                case DreamType.GOOD:
                    GameManager.instance.UpdateCurrentDreamValue(GameManager.instance.dreamGain);
                    break;
                case DreamType.BAD:
                    GameManager.instance.UpdateCurrentDreamValue(-GameManager.instance.dreamGain);
                    break;
                default:
                    break;
            }
        }
        else
		{
            GameManager.instance.UpdateCurrentDreamValue(-GameManager.instance.dreamGain / 2);
        }
        GameManager.instance.spawnedDreams.Remove(gameObject);
        Destroy(gameObject);

        SpawnPoofParticles();
    }

    public DreamType GetDreamType()
    {
        return dreamType;
    }

    public void Init(Transform trans, Player _player, float speed)
    {
        this.speed = speed;
        dreamTarget = trans;
        dreamType = (DreamType)Random.Range(0, System.Enum.GetValues(typeof(DreamType)).Length);
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (isDreamScanned)
		{
            return;
		}
		if(other.gameObject.CompareTag("Player"))
		{
            isDreamScanned = true;
		}
	}
}
