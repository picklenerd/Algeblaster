using UnityEngine;
using System.Collections;

#pragma warning disable 0649

public class playerShieldManager : MonoBehaviour {

    private int juiceCost;
    private float emptyJuiceDelay;
    private float juiceDrainTickTime;
    private bool canShield = true;
    private bool triggerJuiceDrain = true;
    private Timer juiceDrainTimer;
    private Timer juiceDelayTimer;
    private PlayerManager playerManager;
    private playerJuiceManager juiceManager;
    private GameObject myShield;

    //Upgrades
    private bool hasShieldMagnet;
    private float shieldMagnetForce;
    private float shieldMagnetRadius;

    void Awake() {
        myShield = GameObject.FindGameObjectWithTag("Shield");
        myShield.SetActive(false);

        //Get settings froom Game Manager
        playerManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<PlayerManager>();
        juiceManager = GetComponent<playerJuiceManager>();
        juiceCost = playerManager.JuiceCost;
        emptyJuiceDelay = playerManager.EmptyJuiceDelay;
        hasShieldMagnet = playerManager.HasShieldMagnet;
        shieldMagnetForce = playerManager.ShieldMagnetForce;
        shieldMagnetRadius = playerManager.ShieldMagnetRadius;
        
        //Setup timers
        juiceDrainTimer = gameObject.AddComponent<Timer>();
        juiceDrainTimer.Trigger += DrainJuice;
        juiceDelayTimer = gameObject.AddComponent<Timer>();
        juiceDelayTimer.Trigger += ResetShield;        
    }

    void Update() {
        int currentJuice = juiceManager.CurrentJuice;
        if (currentJuice <= 0) {
            canShield = false;
            juiceDelayTimer.Go(emptyJuiceDelay);
        }
    }

    void ResetShield() {
        canShield = true;
    }

    void DrainJuice() {
        triggerJuiceDrain = true;
    }

    public void ShieldOn() {
        if (canShield) {
            //Activate shield and drain juice from Juice Manager
            myShield.SetActive(true);
            if (triggerJuiceDrain) {
                juiceManager.UseJuice(juiceCost);
                triggerJuiceDrain = false;
                juiceDrainTimer.Go(juiceDrainTickTime);
            }

            //Shield Magnet
            if (hasShieldMagnet) {
                Collider[] colliders = Physics.OverlapSphere(transform.position, shieldMagnetRadius);
                foreach (Collider c in colliders) {
                    if (c.GetComponent<Rigidbody>() && (c.GetComponent<Rigidbody>().CompareTag("HealthOrb"))) {
                        c.GetComponent<Rigidbody>().AddExplosionForce(-shieldMagnetForce, transform.position, shieldMagnetRadius, 0, ForceMode.Force);
                    }
                }
            }


        }
        else
            ShieldOff();
    }

    public void ShieldOff() {
        myShield.SetActive(false);
        juiceDrainTimer.Cancel();
        triggerJuiceDrain = true;
    }

}
