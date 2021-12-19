using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketManager : MonoBehaviour
{
    GameObject player;
    GameObject flood;
    [SerializeField]
    bool isFull;
    [SerializeField]
    bool isHeld;
    [SerializeField]
    float fillAmount = .1f;
    GameObject bucketUIimage;
    GameObject hudToolImage;
    [SerializeField]
    Sprite bucketFullImage;
    [SerializeField]
    Sprite bucketEmptyImage;
    GameObject bucket;
    bool isCoolingDown;

    // Start is called before the first frame update
    void Start()
    {
        flood = GameObject.Find("Flood");
        player = GameObject.Find("Player");
        isFull = false;
        isHeld = false;
        bucketUIimage = GameObject.Find("BucketImage");    
        bucketUIimage.SetActive(false);
        hudToolImage = GameObject.Find("HUDToolImage").transform.GetChild(0).gameObject;
        bucket = GameObject.Find("Bucket");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<InventoryManager>().heldTool != null)
        {
            if (player.GetComponent<InventoryManager>().heldTool.name == "Bucket" && player.GetComponent<InventoryManager>().getSelectCounter() == 2)
            {
                if (isHeld == false)
                {
                    isHeld = true;
                }
            }
            else
            {
                if (isHeld == true)
                {
                    isHeld = false;
                }
            }
        }
        else
        {
            if(isHeld == true)
            {
                isHeld = false;
            }
        }

        drawUI();

        if (Input.GetKeyDown(KeyCode.E) && isHeld)
        {
            if (!isCoolingDown)
            {
                fill();
            }
        }
        
        
    }

    void drawUI()
    {
        if (flood.transform.localScale.y <= 0)
        {
            return;
        }
        if (isHeld && !isFull)
        {
            bucketUIimage.SetActive(true);
            hudToolImage.GetComponent<Image>().sprite = bucketEmptyImage;
            bucket.GetComponent<toolPickup>().SetSprite(bucketEmptyImage);
            bucketUIimage.GetComponent<Image>().color = new Color(1, 1, 1, hudToolImage.GetComponent<Image>().color.a);
        }
        else if (isHeld && isFull)
        {
            bucketUIimage.SetActive(true);
            hudToolImage.GetComponent<Image>().sprite = bucketFullImage;
            bucket.GetComponent<toolPickup>().SetSprite(bucketFullImage);
            bucketUIimage.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, hudToolImage.GetComponent<Image>().color.a);

        }
        else
        {
            bucketUIimage.SetActive(false);
        }
    }

    public void fill()
    {
        if (isFull)
        {
            return;
        }
        flood.GetComponent<FloodScript>().Lower(fillAmount);
        if (waterLevel.waterBar.fillAmount > 0)
        {
            waterLevel.waterBar.fillAmount -= waterLevel.waterBar.fillAmount * fillAmount;
            isFull = true;
        }
    }

    IEnumerator CoolDown()
    {
        isCoolingDown = true;
        bucketUIimage.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, hudToolImage.GetComponent<Image>().color.a);
        bucketUIimage.GetComponent<Image>().fillAmount = 0;
        float timer = 0;

        while (timer < 5)
        {
            timer += Time.deltaTime;
            bucketUIimage.GetComponent<Image>().fillAmount = timer / 5;
            yield return null;
        }
        bucketUIimage.GetComponent<Image>().fillAmount = 1;
        bucketUIimage.GetComponent<Image>().color = new Color(1, 1, 1, hudToolImage.GetComponent<Image>().color.a);
        isCoolingDown = false;
    }

    public void Empty()
    {
        isFull = false;
        StartCoroutine(CoolDown());
    }

    public bool IsFull()
    {
        return isFull;
    }

}
