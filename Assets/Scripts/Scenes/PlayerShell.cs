using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShell : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject characterPrefab;

    public List<InputActionAsset> inputAssets = new List<InputActionAsset>();
    
    private void Awake()
    {
        //int id = playerInput.playerIndex;
        //InputActionAsset inputAsset = inputAssets[id];
        
        //playerInput.actions = inputAsset;

        //playerInput.currentActionMap = inputAsset.FindActionMap("UI");
    }
}
