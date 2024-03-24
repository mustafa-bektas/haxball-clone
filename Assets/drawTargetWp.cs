using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawTargetWp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position=GameObject.Find("Enemy").GetComponent<EnemyController>().targetWaypoint;

    }
}
