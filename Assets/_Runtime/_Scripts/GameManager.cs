using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mPersonPrefab;
    public static int mBoundaryX = 19;
    public static int mBoundaryY = 7;

    void Start()
    {
        InvokeRepeating(nameof(InstantiateObject), 0, 5);
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
        if (FindObjectOfType<PointBoard>().GetPoint() < -20)
        {
            // print(GameObject.FindGameObjectWithTag("GameOver"));
            // GameObject.Find("GameOver").SetActive(true);
            Time.timeScale = 0;
        }
    }
}