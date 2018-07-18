using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

    public Image playerIcon;
    public Sprite[] playerIcons = new Sprite[4];

    public void SetIcon(int i)
    {
        playerIcon.sprite = playerIcons[i];
    }

  
}
