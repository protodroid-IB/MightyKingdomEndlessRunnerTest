using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAbility : MonoBehaviour
{
    private IDisposable abilitySub;
    private float abilityStartTime = 0f;

    private PickUp pickUp;

    [SerializeField]
    private MagnetAttractor magnetAttractor;

    [SerializeField]
    private LaserGun laserGun;

    private bool stopAbility = false;

    [SerializeField]
    private GameObject abilitiesParentObject;

    private GameObject activeAbilityGO;

    private void Start()
    {
        GameManager.instance.onStopGame.AddListener(() =>
        {
            stopAbility = true;
        });

        GameManager.instance.onStartGame.AddListener(() =>
        {
            stopAbility = false;
        });
    }

    private void Update()
    {
        abilitiesParentObject.transform.position = transform.position;
    }

    public void SetPickUp(PickUp inPickUp)
    {
        abilitySub?.Dispose();
        activeAbilityGO?.SetActive(false);

        pickUp = inPickUp;
        String type = pickUp.GetType().ToString();
        abilityStartTime = Time.time;

        switch (type)
        {
            case "Magnet":
                abilitySub = this.UpdateAsObservable()
                    .Subscribe(_ => 
                    {
                        magnetAttractor.gameObject.SetActive(true);
                        Magnet(pickUp.ActiveTime);
                        activeAbilityGO = magnetAttractor.gameObject;
                    });
                break;

            case "Laser":
                abilitySub = this.UpdateAsObservable()
                  .Subscribe(_ =>
                  {
                      laserGun.gameObject.SetActive(true);
                      Laser(pickUp.ActiveTime);
                      activeAbilityGO = laserGun.gameObject;
                  });
                break;

        }
    }


    private void Magnet(float activeTime)
    {
        if(!IsAbilityActive(activeTime) && magnetAttractor)
        {
            magnetAttractor.gameObject.SetActive(false);
            abilitySub?.Dispose();
        }
    }

    private void Laser(float activeTime)
    {
        if (!IsAbilityActive(activeTime) && laserGun)
        {
            laserGun.gameObject.SetActive(false);
            abilitySub?.Dispose();
        }
    }





    private bool IsAbilityActive(float activeTime)
    {
        if ((abilityStartTime + activeTime) <= Time.time || stopAbility)
            return false;

        return true;
    }
}
