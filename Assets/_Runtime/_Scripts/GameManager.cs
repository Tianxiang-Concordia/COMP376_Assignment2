using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mPersonPrefab;
    public static int mBoundaryX = 20;
    public static int mBoundaryY = 9;

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
}