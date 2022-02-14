using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Explosion : MonoBehaviour
{
    public bool exploaded = false; 
    [SerializeField] ParticleSystem ps;
    [SerializeField] AudioSource ExpSound;
    public void Explode()
    {
        ps.Play();
        ExpSound.Play();
        GameManager.gm.Explode(this);
    }
}
