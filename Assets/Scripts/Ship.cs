using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : DynamicObjects
{
    [SerializeField] float moveForce = 5;
    [SerializeField] float rotSpeed = 10;
    [SerializeField] ParticleSystem shotParticle;
    [SerializeField] ParticleSystem turboParticle;

    ParticleSystem.EmissionModule turboEmmitModule;

    float rotAcumulation = 0;

    bool isShipDestroy = false;

    protected override void Start()
    {
        base.Start();

        turboEmmitModule = turboParticle.emission;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isShipDestroy)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                turboEmmitModule.enabled = true;
                mRigidBody2D.AddForce(mRigidBody2D.transform.up * moveForce);
            }
            else
            {
                turboEmmitModule.enabled = false;
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                rotAcumulation += Input.GetAxis("Horizontal") * -rotSpeed;
                mRigidBody2D.SetRotation(rotAcumulation);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                shotParticle.Emit(1);
            }
        }
    }

    public void DamageShip()
    {
        if (isShipDestroy == false)
        {
            isShipDestroy = true;
            mSpriteRenderer.enabled = false;
            mPolygonCollider.enabled = false;
            mRigidBody2D.velocity = Vector2.zero;
            turboEmmitModule.enabled = false;

            EmitParticle();

            GameManager.instance.DamageShip();
        }
    }

    public void ResetShip()
    {
        rotAcumulation = 0;
        isShipDestroy = false;
        mSpriteRenderer.enabled = true;
        mPolygonCollider.enabled = true;
        mRigidBody2D.SetRotation(0);
        mRigidBody2D.position = Vector2.zero;
    }
}
