using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public List<Sprite> SpawnObjectList;

    public float xScale;
    public float yScale;
    
    // Start is called before the first frame update
    void Start()
    {
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
