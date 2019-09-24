using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class DynamicObjects : MonoBehaviour
{
    [SerializeField] Vector2 minScreenBounce = Vector2.zero;
    [SerializeField] Vector2 maxScreenBounce = Vector2.zero;

    protected Rigidbody2D mRigidBody2D;
    protected PolygonCollider2D mPolygonCollider;
    protected SpriteRenderer mSpriteRenderer;
    protected ParticleSystem mParticleSystem;

    protected virtual void Start()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mPolygonCollider = GetComponent<PolygonCollider2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mParticleSystem = GetComponent<ParticleSystem>();
        CalculateScreenBounce();
    }

    protected virtual void FixedUpdate()
    {
        if (Time.frameCount % 4 == 0)
        {
            if (mRigidBody2D.position.x < minScreenBounce.x || mRigidBody2D.position.x > maxScreenBounce.x)
            {
                mRigidBody2D.position = new Vector2(mRigidBody2D.position.x * -.98f, mRigidBody2D.position.y);
            }

            if (mRigidBody2D.position.y < minScreenBounce.y || mRigidBody2D.position.y > maxScreenBounce.y)
            {
                mRigidBody2D.position = new Vector2(mRigidBody2D.position.x, mRigidBody2D.position.y * -.98f);
            }
        }
    }

    void CalculateScreenBounce()
    {
        minScreenBounce = Camera.main.ScreenToWorldPoint(Vector3.zero);
        maxScreenBounce = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    protected void EmitParticle(int amount = 50)
    {
        mParticleSystem.Emit(amount);
    }
}
