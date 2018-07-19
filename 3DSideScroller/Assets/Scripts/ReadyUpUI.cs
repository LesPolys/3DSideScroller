using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUpUI : MonoBehaviour {


    public PlayerActions Actions { get; set; }

	public Sprite playerStartingIconImage;
    public Sprite[] playerReadyIconImages;
    private int iconIndex = 0;


    public Sprite characterNotReady;
    public Sprite characterReady;

    public Image playerImage;
    public Image readyUpImage;


	[SerializeField]
	PlayerManager PlayerManager;

	[HideInInspector]
	public bool isReady = false;

	[HideInInspector]
	public bool hasAssignedInput = false;

	// Use this for initialization


    void OnDisable()
	{
		if (Actions != null)
		{
			Actions.Destroy();
		}
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Actions != null){



            if (hasAssignedInput)
            {


                if (Actions.A.WasPressed)
                {
                    

                    if (isReady)
                    {
                        PlayerManager.AreAllPlayersReady();
                    }

                    if (!isReady)
                    {
                        AkSoundEngine.PostEvent("Menu_Accept", gameObject);
                        readyUpImage.sprite = characterReady;
                        isReady = true;
                    }

                }

                if (Actions.B)
                {
                    if (isReady)
                    {
                        AkSoundEngine.PostEvent("Menu_Cancel", gameObject);
                        readyUpImage.sprite = characterNotReady;
                        isReady = false;
                    }
                }

                if (Actions.Up.WasPressed)
                {
                    AkSoundEngine.PostEvent("Menu_Scroll", gameObject);
                    iconIndex++;
                    if (iconIndex >= playerReadyIconImages.Length)
                    {
                        iconIndex = 0;
                    }
                    SetImage();
                }
                if (Actions.Down.WasPressed)
                {
                    AkSoundEngine.PostEvent("Menu_Scroll", gameObject);
                    iconIndex--;
                    if (iconIndex < 0)
                    {
                        iconIndex = playerReadyIconImages.Length-1;
                    }
                    SetImage();
                }
               

            }
            else
            {
                if (Actions.A.WasPressed)
                {
                    readyUpImage.gameObject.SetActive(true);
                    hasAssignedInput = true;
                    AkSoundEngine.PostEvent("Menu_Pause", gameObject);
                    SetImage();

                }
            }
          

        
        }

	}

    void SetImage()
    {

        playerImage.sprite = playerReadyIconImages[iconIndex];
    }


	

}
