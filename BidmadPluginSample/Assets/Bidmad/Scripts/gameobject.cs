using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameobject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject bidmadManager = new GameObject("BidmadManager");
        bidmadManager.AddComponent<BidmadManager>();
        DontDestroyOnLoad(bidmadManager);
        var obj = FindObjectsOfType<BidmadManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(bidmadManager);
        }

        else
        {
            Destroy(bidmadManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
