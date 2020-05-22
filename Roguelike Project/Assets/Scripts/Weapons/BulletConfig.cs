using UnityEngine;

public class BulletConfig : MonoBehaviour {
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletDuration;
    public GameObject[] phases;
    public int creatorLayer;
    public Vector3 direction;
    
    [System.NonSerialized] public Vector3 targetPosition;
    
    private bool _destinyReached;
    private int _currentPhase;
    
    void Start ()
    {
        Destroy(gameObject, bulletDuration);
    }
    
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            DestinyReached();
        }
        else if (_destinyReached == false)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, bulletSpeed * Time.deltaTime);
    }

    public void SetVals (Vector3 dir, int shooterLayer)
    {
        direction = dir;
        creatorLayer = shooterLayer;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("DestroyableObstacle"))
        {
            col.gameObject.GetComponent<Stats>()?.AttackHit(bulletDamage);
            transform.rotation = Quaternion.identity;
            _destinyReached = true;
            DestinyReached();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            transform.rotation = Quaternion.identity;
            _destinyReached = true;
            DestinyReached();
        }   
    }

    private void DestinyReached()
    {
        if (_currentPhase + 1 == phases.Length)
            return;
        
        phases[_currentPhase].SetActive(false);
        _currentPhase += 1;
        transform.rotation = Quaternion.identity;
        phases[_currentPhase].SetActive(true);
    }
}
