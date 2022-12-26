using Mirror;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : NetworkBehaviour
{
    public TextMeshProUGUI Timer;
    public GameObject BoxPrefab;
    public GameObject RoundEffect;

    [Header("Balance")]
    [SyncVar]
    public int RoundTime;
    [SyncVar]
    public int RangeXMin;
    [SyncVar]
    public int RangeZMin;
    [SyncVar]
    public int RangeXMax;
    [SyncVar]
    public int RangeZMax;

    [SyncVar]
    private float RoundTimer;
    [SyncVar]
    private float BoxTimer;

    [SyncVar]
    private bool StopTimer = false;

    public GameObject dScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        RoundTimer = RoundTime;
        BoxTimer = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer && !StopTimer)
        {
            ProcessTimers();
        }
        
        if (RoundTimer <= 0)
        {
            RoundTimerReset();
        }
        
        if (BoxTimer <= 0)
        {
            BoxTimerReset();
        }
        
        Timer.SetText(Mathf.Round(RoundTimer).ToString());
    }

    private void RoundTimerReset()
    {
        var curTransform = transform;
        var scale = (curTransform.localScale - new Vector3(1, 0, 1)); 
        if (scale.z > 0)
        {
            if (isServer)
            {
                var obj = Instantiate(RoundEffect, curTransform.position, curTransform.rotation);
                NetworkServer.Spawn(obj);
            }

            if (isServer)
            {
                Scale(scale);
                RoundTimer = RoundTime;
                RangeXMin += 1;
                RangeXMax -= 2;
                RangeZMin += 1;
                RangeZMax -= 2;
            }
        }
        else
        {
            StopTimer = true;
        }
    }
    
    private void BoxTimerReset()
    {
        var x = Random.Range(RangeXMin, RangeXMax);
        var z = Random.Range(RangeZMin, RangeZMax);
        var pos = new Vector3(x, transform.position.y + 7, z);
        if (isServer)
        {
            var obj = Instantiate(BoxPrefab, pos, Quaternion.identity);
            NetworkServer.Spawn(obj);
        }
        
        BoxTimer = Random.Range(1, 3);
    }

    private void ProcessTimers()
    {
        RoundTimer -= Time.deltaTime;
        BoxTimer -= Time.deltaTime;
    }

    [ClientRpc]
    private void Scale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
