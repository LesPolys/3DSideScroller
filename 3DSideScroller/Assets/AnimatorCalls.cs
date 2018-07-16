using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCalls : MonoBehaviour {

    public Player player;

    void EndOfAttack()
    {
        player.AttackOver();
    }


}
