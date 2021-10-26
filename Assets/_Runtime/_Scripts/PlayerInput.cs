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

    private AudioSource punchAudio;
    private AudioSource vaccinateAudio;
    private AudioSource successAudio;
    private SpriteRenderer spriteRenderer;
    private PointBoard pointBoard;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pointBoard = FindObjectOfType<PointBoard>();
        var audioSources = GetComponents<AudioSource>();
        punchAudio = audioSources[0];
        vaccinateAudio = audioSources[1];
        successAudio = audioSources[2];
    }

    private void Update()
    {
        // Obtain input information (See "Horizontal" and "Vertical" in the Input Manager)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0.0f);
        direction = direction.normalized;

        // Translate the game object
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += direction * speed * 2 * Time.deltaTime;
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }


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
                if (!person.GetIsMasked())
                {
                    float distance = (transform.position - person.transform.position).sqrMagnitude;
                    if (distance < 5)
                    {
                        count++;
                        person.SetIsMasked(true);
                        pointBoard.Increment(1);
                    }

                    if (count >= 2)
                    {
                        pointBoard.Increment(2);
                    }
                    successAudio.Play();
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
                    successAudio.Play();
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Vaccinate"))
        {
            Person[] people = Resources.FindObjectsOfTypeAll(typeof(Person)) as Person[];
            foreach (var person in people)
            {
                float distance = (transform.position - person.transform.position).sqrMagnitude;
                if (distance < 5 && !person.GetIsVaccinated() && person.GetPersonType() != Person.Type.INFLECTED)
                {
                    if (person.GetPersonType() == Person.Type.HIGHLY_SUSCEPTIBLE_INFLECTED)
                    {
                        pointBoard.Increment(2);
                    }
                    else
                    {
                        pointBoard.Increment(1);
                    }
                    vaccinateAudio.Play();
                    person.SetIsVaccinated(true);
                    person.SetPersonType(Person.Type.FULLLY_VACCINATED);
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Isolation"))
        {
            Person[] people = Resources.FindObjectsOfTypeAll(typeof(Person)) as Person[];
            foreach (var person in people)
            {
                float distance = (transform.position - person.transform.position).sqrMagnitude;
                if (distance < 5 && person.GetPersonType() == Person.Type.INFLECTED)
                {
                    Destroy(person.gameObject);
                    punchAudio.Play();
                    pointBoard.Increment(1);
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Drive Away"))
        {
            Person[] people = Resources.FindObjectsOfTypeAll(typeof(Person)) as Person[];
            int count = 0;
            foreach (var person in people)
            {
                float distance = (transform.position - person.transform.position).sqrMagnitude;
                if (distance < 5)
                {
                    person.translateDirection = -person.translateDirection;
                    count++;
                }
            }

            if (count >= 3)
            {
                pointBoard.Increment(1);
            }
        }
    }
}