using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTerrainDetection : MonoBehaviour
{
    List<RaycastHit> hits = new List<RaycastHit>();
    List<MeshRenderer> invisibleWalls = new List<MeshRenderer>();
    List<MeshRenderer> oldInvisibleWalls = new List<MeshRenderer>();
    List<Ray> rays = new List<Ray>();
    GameObject player;

    public void Start()
    {
        player = transform.parent.gameObject;
    }

    public void Update()
    {
        Ray ray = new Ray();
        ray.origin = this.transform.position- new Vector3(1, 1, 0);
        ray.direction = transform.forward;

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                Ray tempRay = new Ray();
                tempRay.origin = ray.origin + new Vector3(x, y, 0);
                rays.Add(tempRay);
            }
        }

        RaycastHit[] tempHits;
        foreach(Ray _ray in rays)
        {
            tempHits = Physics.RaycastAll(_ray, (transform.position - player.transform.position).magnitude);
            foreach(RaycastHit hit in tempHits)
            {
                Debug.Log("Test");
                hits.Add(hit);
            }
        }

        TurnInvisible();

    }

    private void TurnInvisible()
    {
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                if (!invisibleWalls.Contains(hit.collider.gameObject.GetComponent<MeshRenderer>()))
                {
                    invisibleWalls.Add(hit.collider.gameObject.GetComponent<MeshRenderer>());
                }
            }
        }

        foreach (MeshRenderer renderer in oldInvisibleWalls)
        {
            if (!invisibleWalls.Contains(renderer))
            {
                renderer.enabled = false;
            }
        }

        oldInvisibleWalls.Clear();
        foreach (MeshRenderer renderer in invisibleWalls)
        {
            invisibleWalls.Add(renderer);
        }
        invisibleWalls.Clear();
    }
}
