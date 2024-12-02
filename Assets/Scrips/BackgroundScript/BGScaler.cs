using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float worldHeignt = Camera.main.orthographicSize * 2f;
        Debug.Log (worldHeignt);
        float worldWitdh = worldHeignt * Screen.width / Screen.height;
        Debug.Log(worldWitdh);

        transform.localScale = new Vector3(worldWitdh, worldHeignt, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
