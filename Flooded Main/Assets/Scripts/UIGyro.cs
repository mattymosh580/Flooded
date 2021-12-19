using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGyro : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    float floatDistance = .5f;

    private void Start()
    {
        player = GameObject.Find("GameManager").GetComponent<NewGameManager>().player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 90, 0f);
        }
        Vector3 pos = transform.parent.transform.position;
        pos.y += floatDistance;
        transform.position = pos;
    }
}
