using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToiletMiniGame : MiniGameManager
{
    bool isPlaying = false;
    float maxCount = 50;
    float hitCount = 0;
    bool isHittingA = true;
    GameObject fillbarCanvas;
    GameObject aImage;
    GameObject dImage;
    GameObject fillBarMask;
    GameObject AText;
    GameObject DText;
    GameObject plunger;
    int plungeState = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        fillbarCanvas = GameObject.Find("FillBarCanvas");
        fillBarMask = fillbarCanvas.transform.GetChild(0).gameObject;
        aImage = fillbarCanvas.transform.GetChild(2).gameObject;
        dImage = fillbarCanvas.transform.GetChild(3).gameObject;
        AText = aImage.transform.GetChild(0).gameObject;
        DText = dImage.transform.GetChild(0).gameObject;
        fillbarCanvas.SetActive(false);
        fillBarMask.GetComponent<Image>().fillAmount = 0;
        plunger = transform.GetChild(2).gameObject;
        plunger.SetActive(false);
    }

    public override void PlayGame()
    {
        player.transform.GetChild(0).gameObject.AddComponent<MiniGameCameraController>();
        fillbarCanvas.SetActive(true);
        isPlaying = true;
        AText.GetComponent<Text>().color = Color.blue;
        DText.GetComponent<Text>().color = Color.black;
        plunger.SetActive(true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.A) && isHittingA)
            {
                hitCount++;
                isHittingA = false;
                AText.GetComponent<Text>().color = Color.black;
                DText.GetComponent<Text>().color = Color.blue;
                switch (plungeState)
                {
                    case 0:
                        StartCoroutine(PlungeDown());
                        break;
                    case 1:
                        break;
                    case 2:
                        StartCoroutine(PlungeUp());
                        break;
                    case 3:
                        break;
                }
            }


            if (Input.GetKeyDown(KeyCode.D) && !isHittingA)
            {
                hitCount++;
                isHittingA = true;
                AText.GetComponent<Text>().color = Color.blue;
                DText.GetComponent<Text>().color = Color.black;
                switch (plungeState)
                {
                    case 0:
                        StartCoroutine(PlungeDown());
                        break;
                    case 1:
                        break;
                    case 2:
                        StartCoroutine(PlungeUp());
                        break;
                    case 3:
                        break;
                }
            }

            fillBarMask.GetComponent<Image>().fillAmount = hitCount / maxCount;

            if (hitCount == maxCount)
            {

                EndGame();
            }
            
        }
        base.Update();
    }

    IEnumerator PlungeDown()
    {
        plungeState = 1;
        float timer = 0;
        while(timer < .05f)
        {
            timer += Time.deltaTime;
            plunger.transform.localPosition = (new Vector3(plunger.transform.localPosition.x, ((-.5f*timer)/.05f) + 1.25f, plunger.transform.localPosition.z));
            yield return null;
        }
        plungeState = 2;
    }
    IEnumerator PlungeUp()
    {
        plungeState = 3;
        float timer = 0;
        while (timer < .05f)
        {
            timer += Time.deltaTime;
            plunger.transform.localPosition = (new Vector3(plunger.transform.localPosition.x, ((.5f * timer) / .05f) + .75f, plunger.transform.localPosition.z));
            yield return null;
        }
        plungeState = 0;
    }

    public override void Break()
    {
        broken = true;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public override void FixLeak()
    {
        broken = false;
        transform.GetChild(1).gameObject.SetActive(false);
        broken = false;
    }

    public override void EndGame()
    {
        Destroy(player.transform.GetChild(0).gameObject.GetComponent<MiniGameCameraController>());
        fillbarCanvas.SetActive(false);
        isPlaying = false;
        hitCount = 0;
        plunger.SetActive(false);
        plunger.transform.localPosition = new Vector3(plunger.transform.localPosition.x, 1.25f, plunger.transform.localPosition.z);
        FixLeak();
        base.EndGame();
    }
}
