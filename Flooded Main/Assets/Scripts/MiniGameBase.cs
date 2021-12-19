using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    [SerializeField]
    protected bool broken = false;
    protected ParticleSystem particles;
    protected FloodScript flood;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(broken)
        {
            Break();
        }

        // Automatically adds the object to Random Leak Generator's breakables list
        FindObjectOfType<RandomLeakGenerator>().AddBreakable(gameObject);

        // Find flood object
        flood = FindObjectOfType<FloodScript>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(broken)
        {
            waterLevel.waterBar.fillAmount += Time.deltaTime / 250;   // 0.2% of water level bar per second; 500 seconds before filled
            flood.Raise(Time.deltaTime / 250);
        }
    }

    // Makes the object start leaking
    public abstract void Break();

    // Fixes the object's leak\
    public abstract void FixLeak();

}
