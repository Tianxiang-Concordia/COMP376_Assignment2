using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infectable : MonoBehaviour
{
    private bool isInfected;
    public Sprite infectableWithVirus;
    public Sprite infectable;
    private SpriteRenderer spriteRenderer;

    public Infectable()
    {
        isInfected = false;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Person person = col.GetComponent<Person>();
        if (person == null) return;
        if (person.GetIsInfected())
        {
            isInfected = true;
        }
    }

    void Update()
    {
        if (GetIsInfected())
        {
            spriteRenderer.sprite = infectableWithVirus;
        }
        else
        {
            spriteRenderer.sprite = infectable;
        }
    }

    public bool GetIsInfected()
    {
        return isInfected;
    }

    public void SetIsInfected(bool isInfected)
    {
        this.isInfected = isInfected;
    }
}