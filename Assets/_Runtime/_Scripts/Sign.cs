using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] public Sprite vaccinated;
    [SerializeField] public Sprite unvaccinated;
    [SerializeField] public Sprite susceptible;
    [SerializeField] public Sprite infected;

    private SpriteRenderer spriteRenderer;

    public void SetSprite(Person.Type personType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (personType)
        {
            case Person.Type.FULLLY_VACCINATED:
                spriteRenderer.sprite = vaccinated;
                break;
            case Person.Type.UNVACCINATED_WITH_MASK:
                spriteRenderer.sprite = unvaccinated;
                break;
            case Person.Type.UNVACCINATED_WITHOUT_MASK:
                spriteRenderer.sprite = unvaccinated;
                break;
            case Person.Type.HIGHLY_SUSCEPTIBLE_INFLECTED:
                spriteRenderer.sprite = susceptible;
                break;
            case Person.Type.INFLECTED:
                spriteRenderer.sprite = infected;
                break;
        }
    }
}