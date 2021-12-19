using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Generates a random number between a min and a max every second.
// If the number corresponds to an index on the list of breakable
// objects, that object will break and begin leaking.

public class RandomLeakGenerator : MonoBehaviour
{
    // Put any object that can break/leak here (toilets, sinks, walls with pipes, etc.)
    [SerializeField] List<GameObject> breakables;

    // The in-game time
    Timer timer;

    public bool pauseLeaks = false;

    IEnumerator breakCheck;

    [SerializeField]
    float breakCheckFrequency = 5;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>().GetComponent<Timer>();

        breakCheck = BreakCheck();
        StartCoroutine(breakCheck);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.timeValue <= 0 && breakables.Count > 0)
        {
            FixAllLeaks(true);
        }
    }

    IEnumerator BreakCheck()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(breakCheckFrequency);
            if (Random.Range(0, 2) == 0)
            {
                RLG();
            }
        }
    }

    // Object breaking method
    void RLG()
    {
        // Generate a random number
        int randNum = Random.Range(0, breakables.Count);
        //Debug.Log(randNum);
        if (breakables[randNum].GetComponent<MiniGameBase>())
        {
            breakables[randNum].GetComponent<MiniGameBase>().Break();
        }
        else if (!breakables[randNum].GetComponent<MiniGameManager>().IsBroken())
        {
            breakables[randNum].GetComponent<MiniGameManager>().Break();
        }       
    }

    // Add breakable objects to breakables list
    public void AddBreakable(GameObject breakable)
    {
        if(breakable.CompareTag("Breakable"))
        {
            breakables.Add(breakable);
        }
    }

    // Fix all leaks and can also clear the list if clearList is set to true
    public void FixAllLeaks(bool clearList = false)
    {
        for(int i = 0; i < breakables.Count; i++)
        {
            if (breakables[i].GetComponent<MiniGameBase>())
            {
                breakables[i].GetComponent<MiniGameBase>().FixLeak();
            }
            else if (breakables[i].GetComponent<MiniGameManager>())
            {
                breakables[i].GetComponent<MiniGameManager>().FixLeak();
            }
        }
        if(clearList)
        {
            breakables.Clear();
            StopAllCoroutines();
        }
    }
}
