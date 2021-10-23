using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite rightSprite;

    private SpriteRenderer spriteRenderer;
    private PointBoard pointBoard;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pointBoard = FindObjectOfType<PointBoard>();
    }

    private void Update()
    {
        // Obtain input information (See "Horizontal" and "Vertical" in the Input Manager)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0.0f);
        direction = direction.normalized;

        // Translate the gameobject
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the sprite
        if (direction != Vector3.zero)
        {
            if (direction.y > 0)
            {
                spriteRenderer.sprite = upSprite;
            }
            else if (direction.y < 0)
            {
                spriteRenderer.sprite = downSprite;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.sprite = rightSprite;
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.sprite = rightSprite;
                spriteRenderer.flipX = true;
            }
        }


        if (Input.GetButtonDown("Give Mask"))
        {
            Person[] people = Resources.FindObjectsOfTypeAll(typeof(Person)) as Person[];
            int count = 0;
            foreach (var person in people)
            {
                if (person.GetPersonType() == Person.Type.UNVACCINATED_WITHOUT_MASK)
                {
                    float distance = (transform.position - person.transform.position).sqrMagnitude;
                    if (distance < 5)
                    {
                        count++;
                        person.WearMask();
                        pointBoard.Increment(1);
                    }

                    if (count >= 2)
                    {
                        pointBoard.Increment(2);
                    }
                }
            }
        }

        if (Input.GetButtonDown("Clean"))
        {
            Infectable[] infectables = Resources.FindObjectsOfTypeAll(typeof(Infectable)) as Infectable[];
            foreach (var infectable in infectables)
            {
                float distance = (transform.position - infectable.transform.position).sqrMagnitude;
                if (distance < 5)
                {
                    infectable.SetIsInfected(false);
                    pointBoard.Increment(1);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("hello:" + col.name);
    }
}