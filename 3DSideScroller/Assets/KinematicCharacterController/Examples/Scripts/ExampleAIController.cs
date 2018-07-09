using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class ExampleAIController : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public ExampleCharacterController[] Characters;

        private bool _stepHandling;
        private bool _ledgeHandling;
        private bool _intHandling;
        private bool _safeMove;

        private void Update()
        {
            AICharacterInputs inputs = new AICharacterInputs();

            // Simulate an input on all controlled characters
            inputs.MoveVector = new Vector3(-0.1f * moveSpeed, 0f, 0f);
            inputs.LookVector = Vector3.left; 
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i].SetInputs(ref inputs);
            }
        }
    }
}