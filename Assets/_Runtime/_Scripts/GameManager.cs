using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mPersonPrefab;
    public static int mBoundaryX = 19;
    public static int mBoundaryY = 7;
    public GameObject gameOverObject;

    void Start()
    {
        InvokeRepeating(nameof(InstantiateObject), 0, 5);
        InvokeRepeating(nameof(GenerateMob), 8, 15);
        InvokeRepeating(nameof(UpdateLevel), 15, 15);
    }

    void InstantiateObject()
    {
        Random random = new Random();
        int x = random.Next(-mBoundaryX, mBoundaryX);
        int y = random.Next(-mBoundaryY, mBoundaryY);
        Instantiate(mPersonPrefab, new Vector3(x, y, 0), transform.rotation);
    }

    void Update()
    {
        if (FindObjectOfType<PointBoard>().GetPoint() <= -20)
        {
            gameOverObject.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetButtonDown("SwitchMode"))
        {
            GenerateMob();
            Person.DefaultSpeed = 0.2f;
            Person[] people = Resources.FindObjectsOfTypeAll(typeof(Person)) as Person[];
            foreach (var person in people)
            {
                person.SetSpeed(0.2f);
            }

            Invoke(nameof(SwitchModeCallback), 5.0f);
        }
    }

    public void GenerateMob()
    {
        Random random = new Random();
        int x = random.Next(-mBoundaryX, mBoundaryX);
        int y = random.Next(-mBoundaryY, mBoundaryY);

        for (var i = 0; i < 5; i++)
        {
            Instantiate(mPersonPrefab, new Vector3(x, y, 0), transform.rotation);
        }
    }

    void SwitchModeCallback()
    {
        Person.DefaultSpeed = 1.0f;
        Person[] people = Resources.FindObjectsOfTypeAll(typeof(Person)) as Person[];
        foreach (var person in people)
        {
            person.SetSpeed(1.0f);
        }
    }

    void UpdateLevel()
    {
        Person.DefaultSpeed += 0.1f;
    }
}