using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;


    public class MyPlayer : MonoBehaviour
    {
   
        public MyCharacterController Character;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";

        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        public Transform controlRotation;

        private void Update()
        {
     
            HandleCharacterInput();
        }



        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(HorizontalInput);
            characterInputs.MoveAxisRight = -1f * Input.GetAxisRaw( VerticalInput); //invert the up down
            characterInputs.CameraRotation = controlRotation.rotation;
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
