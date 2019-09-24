using UnityEngine;
using System.Collections.Generic;

public class Asteroid : DynamicObjects
{
    [SerializeField] Sprite[] spriteVariant;

    [Tooltip("initial force used to start moving.")]
    [SerializeField] float initialForceMove = 10f;

    [SerializeField] AudioSource explodeAudio;

    bool destroyWhenParticleStop = false;

    protected override void Start()
    {
        base.Start();

        mSpriteRenderer.sprite = spriteVariant[Random.Range(0, spriteVariant.Length)];

        mPolygonCollider.pathCount = mSpriteRenderer.sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < mPolygonCollider.pathCount; i++)
        {
            path.Clear();
            mSpriteRenderer.sprite.GetPhysicsShape(i, path);
            mPolygonCollider.SetPath(i, path.ToArray());
        }

        Vector2 ramdonForceDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        mRigidBody2D.AddForce(ramdonForceDir * initialForceMove, ForceMode2D.Impulse);
    }

    private void OnEnable()
    {

        if (!mSpriteRenderer || !mPolygonCollider || !mRigidBody2D)
            return;

        mSpriteRenderer.sprite = spriteVariant[Random.Range(0, spriteVariant.Length)];

        mPolygonCollider.pathCount = mSpriteRenderer.sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < mPolygonCollider.pathCount; i++)
        {
            path.Clear();
            mSpriteRenderer.sprite.GetPhysicsShape(i, path);
            mPolygonCollider.SetPath(i, path.ToArray());
        }
    }

    /// <summary>
    /// Add force in an address multiplied by the "initialForceMove" parameter of this class.
    /// </summary>
    /// <param name="direction"></param>
    public void ForceToDirection(Vector2 direction)
    {
        if(mRigidBody2D)
            mRigidBody2D.AddForce(direction * initialForceMove, ForceMode2D.Impulse);
    }


    public void DestroyAsteroid()
    {
        GameManager.instance.DestroyAsteroid();

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Asteroid childAsteroid = transform.GetChild(i).GetComponent<Asteroid>();
                childAsteroid.gameObject.SetActive(true);
                Vector2 dir = this.transform.position - childAsteroid.transform.position;
                dir = Vector2.ClampMagnitude(dir, 1f);
                childAsteroid.ForceToDirection(dir);
            }
        }

        mSpriteRenderer.enabled = false;
        mPolygonCollider.enabled = false;
        mRigidBody2D.velocity = Vector2.zero;

        EmitParticle();
        explodeAudio.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("DamageShip");
    }
}
