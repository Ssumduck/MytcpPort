using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjects : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            DontDestroyOnLoad(objects[i]);
        }
        Destroy(gameObject);
    }
}
