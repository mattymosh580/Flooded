using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPipeGame2 : MiniGameManager
{
    [SerializeField]
    bool isPlaying = false;

    Collider triggerBox;

    GameObject playerPipe;

    GameObject normalWall;
    GameObject brokenWall;

    [SerializeField]
    GameObject pipeGameUI;


    float startAngle;
    float angle;

    bool rotatingLeft = false;
    bool rotatingRight = false;

    bool paused = false;

    public bool pressedEsc;

    public enum Directions
    {
        Left,
        Up,
        Right,
        Down
    }

    [SerializeField]
    List<Directions> directions = new List<Directions>();
    


    // Start is called before the first frame update
    protected override void Start()
    {
        triggerBox = transform.GetComponent<Collider>();
        normalWall = transform.GetChild(1).gameObject;
        brokenWall = transform.GetChild(2).gameObject;

        base.Start();
        triggerBox.enabled = broken;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (!NewGameManager.inPause && paused)
        {
            for (int i = 3; i < 7; i++)
            {
                transform.GetChild(i).GetChild(0).GetComponent<ParticleSystem>().Play();
            }
            paused = false;
        }
        else if (NewGameManager.inPause && !paused)
        {
            for (int i = 3; i < 7; i++)
            {
                transform.GetChild(i).GetChild(0).GetComponent<ParticleSystem>().Pause();
            }
            paused = true;
        }

        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(QuitGame());

                pressedEsc = true;
            }

            if (Input.GetKeyDown(KeyCode.A) && playerPipe != null)
            {
                if (!rotatingLeft && !rotatingRight)
                {
                    if (playerPipe != null)
                    {
                        startAngle = playerPipe.transform.localEulerAngles.z;
                        playerPipe.GetComponent<PipeOrientationManager>().RotateLeft();
                        rotatingLeft = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D) && playerPipe != null)
            {
                if (!rotatingLeft && !rotatingRight)
                {
                    if (playerPipe != null)
                    {
                        startAngle = playerPipe.transform.localEulerAngles.z;
                        playerPipe.GetComponent<PipeOrientationManager>().RotateRight();
                        rotatingRight = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {

                if (CheckDirections())
                {
                    WinGame();
                }
            }

            if (rotatingLeft)
            {
                RotateLeft();
            }

            if (rotatingRight)
            {
                RotateRight();
            }
        }
        base.Update();
    }

    void RotateLeft()
    {
        
        angle += Time.deltaTime * 360;
        angle = Mathf.Clamp(angle, 0, 90);

        if (angle == 90)
        {
            playerPipe.transform.localEulerAngles = new Vector3(playerPipe.transform.localEulerAngles.x, playerPipe.transform.localEulerAngles.y, startAngle + 90);
            rotatingLeft = false;
            angle = 0;
        }
        else
        {
            playerPipe.transform.Rotate(0, 0, Time.deltaTime * 360);
        }
    }

    void RotateRight()
    {

        angle += Time.deltaTime * 360;
        angle = Mathf.Clamp(angle, 0, 90);

        if (angle == 90)
        {
            playerPipe.transform.localEulerAngles = new Vector3(playerPipe.transform.localEulerAngles.x, playerPipe.transform.localEulerAngles.y, startAngle - 90);
            rotatingRight = false;
            angle = 0;
        }
        else
        {
            playerPipe.transform.Rotate(0, 0, -Time.deltaTime * 360);
        }
    }

    void ChooseDirections()
    {
        int numDirections = Random.Range(1, 4);
        for(int i = 0; i <= numDirections; ++i)
        {
            int rnd = Random.Range(0, 4);
            while (directions.Contains((Directions)rnd))
            {
                rnd = Random.Range(0, 4);
            }
            directions.Add((Directions)rnd);
        }
    }

    void DrawDirections()
    {
        if (directions.Contains(Directions.Left))
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }

        if (directions.Contains(Directions.Up))
        {
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(4).gameObject.SetActive(false);
        }

        if (directions.Contains(Directions.Right))
        {
            transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(5).gameObject.SetActive(false);
        }

        if (directions.Contains(Directions.Down))
        {
            transform.GetChild(6).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(6).gameObject.SetActive(false);
        }
    }

    bool CheckDirections()
    {
        if(playerPipe == null)
        {
            return false;
        }
        if(playerPipe.GetComponent<PipeOrientationManager>().GetDirections().Count != directions.Count)
        {
            return false;
        }
        foreach(var direction in directions)
        {
            if (!playerPipe.GetComponent<PipeOrientationManager>().GetDirections().Contains(direction))
            {
                return false;
            }
        }
        return true;
    }

    public override void Break()
    {
        broken = true;
        triggerBox.enabled = true;
        ChooseDirections();
        DrawDirections();
        normalWall.SetActive(false);
        brokenWall.SetActive(true);
        
    }

    public override void FixLeak()
    {
        normalWall.SetActive(true);
        brokenWall.SetActive(false);
        directions.Clear();
        base.FixLeak();
        triggerBox.enabled = false;
        for (int i = 3; i < 7; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public override void PlayGame()
    {
        StartCoroutine(ProccessFrame());
        player.transform.GetChild(0).gameObject.AddComponent<PipeGameCameraController>();
        playerPipe = player.GetComponent<InventoryManager>().DropHeldItem();

        if (playerPipe != null)
        {
            playerPipe.GetComponent<ItemPickup>().enabled = false;
            playerPipe.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerPipe.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 pos = transform.position + (transform.forward * .2f);
            
            playerPipe.transform.position = new Vector3(pos.x, player.transform.position.y + .38f, pos.z);
            playerPipe.transform.eulerAngles = Vector3.zero;
            playerPipe.transform.LookAt(new Vector3(transform.position.x, playerPipe.transform.position.y, transform.position.z));

            pipeGameUI.SetActive(true);
        }
        
    }

    IEnumerator ProccessFrame()
    {
        yield return new WaitForEndOfFrame();
        isPlaying = true;
    }

    IEnumerator QuitGame()
    {
        yield return null;

        if (playerPipe != null)
        {
            playerPipe.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            playerPipe.GetComponent<ItemPickup>().enabled = true;
            EventManager.pressEActionsCounter++;
            EventManager.PressEActions += playerPipe.GetComponent<ItemPickup>().PickUp;
            playerPipe.GetComponent<ItemPickup>().PickUp();
            playerPipe.GetComponent<PipeOrientationManager>().Reset();
        }
        EndGame();
    }

    void WinGame()
    {
        GameObject.Find("GameManager").GetComponent<ScoreCalculator>().wallScore += 1;

        Destroy(playerPipe);
        FixLeak();
        EndGame();
    }

    public override void EndGame()
    {
        pipeGameUI.SetActive(false);
        Destroy(player.transform.GetChild(0).gameObject.GetComponent<PipeGameCameraController>());
        isPlaying = false;
        base.EndGame();
    }
}
