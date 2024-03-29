﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : DynamicObjects
{
    [SerializeField] float moveForce = 5;
    [SerializeField] float rotSpeed = 10;
    [SerializeField] ParticleSystem shotParticle;
    [SerializeField] ParticleSystem turboParticle;
    [SerializeField] GameObject shield;
    [SerializeField] float shieldActiveTime = 3;
    [SerializeField] AudioSource turboAudio;
    [SerializeField] AudioSource respawnAudio;
    [SerializeField] AudioSource shotAudio;
    [SerializeField] AudioSource explodeAudio;


    float lastTimeShieldActive = 0;

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

                if(!turboAudio.isPlaying)
                    turboAudio.Play();
            }
            else
            {
                turboEmmitModule.enabled = false;
                turboAudio.Stop();
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                rotAcumulation += Input.GetAxis("Horizontal") * -rotSpeed;
                mRigidBody2D.SetRotation(rotAcumulation);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                shotParticle.Emit(1);
                shotAudio.Play();
            }
        }

        if (shield.activeSelf)
        {
            if ((Time.time - lastTimeShieldActive) > shieldActiveTime)
            {
                shield.SetActive(false);
            }
        }
    }

    public void DamageShip()
    {
        if (shield.activeSelf)
            return;

        if (isShipDestroy)
            return;

        isShipDestroy = true;
        mSpriteRenderer.enabled = false;
        mPolygonCollider.enabled = false;
        mRigidBody2D.velocity = Vector2.zero;
        turboEmmitModule.enabled = false;
        turboAudio.Stop();

        EmitParticle();
        explodeAudio.Play();

        GameManager.instance.DamageShip();
    }

    public void ResetShip()
    {
        
        rotAcumulation = 0;
        isShipDestroy = false;
        mSpriteRenderer.enabled = true;
        mPolygonCollider.enabled = true;
        mRigidBody2D.SetRotation(0);
        mRigidBody2D.position = Vector2.zero;

        lastTimeShieldActive = Time.time;
        shield.SetActive(true);
        respawnAudio.Play();
    }
}
