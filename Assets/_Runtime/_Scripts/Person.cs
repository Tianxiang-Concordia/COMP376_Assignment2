using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Person : MonoBehaviour
{
    public Vector3 translateDirection;
    private bool isMasked;
    private bool isInfected;
    private Type personType;
    private float infectionProbability;
    private PointBoard pointBoard;
    private Sign sign;

    public enum Type
    {
        FULLLY_VACCINATED = 0,
        UNVACCINATED_WITH_MASK = 1,
        UNVACCINATED_WITHOUT_MASK = 2,
        HIGHLY_SUSCEPTIBLE_INFLECTED = 3,
        INFLECTED = 4
    }

    public Person()
    {
        Random random = new Random();
        personType = (Type) random.Next(0, 5);
        switch (personType)
        {
            case Type.FULLLY_VACCINATED:
                isMasked = true;
                isInfected = false;
                infectionProbability = 0;
                break;

            case Type.UNVACCINATED_WITH_MASK:
                isMasked = true;
                isInfected = false;
                infectionProbability = 0.2f;
                break;

            case Type.UNVACCINATED_WITHOUT_MASK:
                isMasked = false;
                isInfected = false;
                infectionProbability = 0.8f;
                break;

            case Type.HIGHLY_SUSCEPTIBLE_INFLECTED:
                isMasked = true;
                isInfected = false;
                infectionProbability = 1.0f;
                break;

            case Type.INFLECTED:
                isMasked = true;
                isInfected = true;
                infectionProbability = 1.0f;
                break;
        }
    }

    private void Awake()
    {
        pointBoard = FindObjectOfType<PointBoard>();
        sign = GetComponentInChildren<Sign>();
        sign.SetSprite(personType);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeDirection();
        InvokeRepeating(nameof(ChangeDirection), 5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        // Update Sign Sprite 
        sign.SetSprite(personType);

        // Update position
        transform.Translate(translateDirection * Time.deltaTime);

        // change the direction if the character touches the boundary. 
        if (transform.position.x > GameManager.mBoundaryX || transform.position.x < -GameManager.mBoundaryX
                                                          || transform.position.y > GameManager.mBoundaryY
                                                          || transform.position.y < -GameManager.mBoundaryY
        )
        {
            translateDirection = -translateDirection;
        }

        // Flip the sprite
        if (translateDirection != Vector3.zero)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (translateDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (translateDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }

        GetComponent<Animator>().SetBool("isMasked", isMasked);
    }

    void ChangeDirection()
    {
        Random random = new Random();
        float x = (float) random.Next(-10, 9) / 10;
        float y = (float) random.Next(-10, 9) / 10;
        translateDirection = new Vector3(x, y, 0).normalized;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Person"))
        {
            print("touched!");
            translateDirection = -translateDirection;

            // @TODO: check the type of the person
        }
        else if (col.tag.Equals("Infectable"))
        {
            translateDirection = -translateDirection;
            Infectable infectable = col.GetComponent<Infectable>();
            if (infectable != null) return;
            if (infectable.GetIsInfected())
            {
                // TODO: calculate the probility
                Random random = new Random();
                double randomDouble = random.NextDouble();
                if (randomDouble < infectionProbability)
                {
                    if (personType == Type.HIGHLY_SUSCEPTIBLE_INFLECTED)
                    {
                        // Dead
                        Destroy(gameObject);
                        pointBoard.Decrement(1);
                        return;
                    }

                    isInfected = true;
                    personType = Type.INFLECTED;
                }
            }
        }
    }

    public void WearMask()
    {
        personType = Type.UNVACCINATED_WITH_MASK;
        isMasked = true;
    }

    public bool GetIsMasked()
    {
        return isMasked;
    }
    
    public Type GetPersonType()
    {
        return personType;
    }
  
    public bool GetIsInfected()
    {
        return isInfected;
    }
}