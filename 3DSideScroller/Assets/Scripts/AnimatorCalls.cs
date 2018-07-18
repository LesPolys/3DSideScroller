using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCalls : MonoBehaviour {

    public Player player;
    public TrailRenderer lightTrailRenderer;
    public TrailRenderer heavyTrailRenderer;
    public TrailRenderer spinTrailRenderer;



    void EndOfAttack()
    {
        player.AttackOver();
    }


    void TurnOnLightTrailRenderer()
    {
        lightTrailRenderer.enabled = true;
    }

    void TurnOffLightTrailRender()
    {
        lightTrailRenderer.enabled = false;
    }

    void TurnOnHeavyTrailRenderer()
    {
        heavyTrailRenderer.enabled = true;
    }

    void TurnOffHeavyTrailRender()
    {
        heavyTrailRenderer.enabled = false;
    }


    void TurnOnSpinTrailRenderer()
    {
        spinTrailRenderer.enabled = true;
    }

    void TurnOffSpinTrailRender()
    {
        spinTrailRenderer.enabled = false;
    }



}
