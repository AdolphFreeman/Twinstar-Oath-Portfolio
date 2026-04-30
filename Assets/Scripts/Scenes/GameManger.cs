using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;
    public List<PlayerInfo> playerInfos;
    
    public List<GameObject> characterPrefabs;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
       //PlayerInput.Instantiate()
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            GenerateCharacters();

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCharactersTest();
        }*/
    }

    void GenerateCharactersTest()
    {
        Camera[] cameras = Camera.allCameras;
        List<GameObject> characters = new List<GameObject>();

        foreach (PlayerInfo info in playerInfos)
        {
            GameObject prefab = characterPrefabs[0];
            if(info.characterName == "Wizard")
                prefab = characterPrefabs[1];
            else if(info.characterName == "Warrior")
                prefab = characterPrefabs[0];

            GameObject player = Instantiate(prefab);
            //player.GetComponentInChildren<Canvas>().targetDisplay = info.userID;
            
            characters.Add(player);
        }
        
        for (int i = 0; i < characters.Count; i++)
        {
            CameraTrack track = cameras[i].GetComponent<CameraTrack>();
            track.target = characters[i].transform;
            
            cameras[i].targetDisplay = i;
            characters[i].GetComponentInChildren<Canvas>().targetDisplay = i;
        }
    }
    
    void GenerateCharacters()
    {
        CameraTrack[] tracks = FindObjectsByType<CameraTrack>(FindObjectsSortMode.None);
        List<GameObject> characters = new List<GameObject>();
        
        foreach (PlayerInfo info in playerInfos)
        {
            GameObject prefab = characterPrefabs[0];
            if(info.characterName == "Wizard")
                prefab = characterPrefabs[1];
            else if(info.characterName == "Warrior")
                prefab = characterPrefabs[0];

            GameObject player = PlayerInput.Instantiate(prefab, 
                playerIndex: info.userID, 
                pairWithDevice: info.device).gameObject;

            //player.GetComponentInChildren<Canvas>().targetDisplay = info.userID;
            
            characters.Add(player);
        }

        for (int i = 0; i < characters.Count; i++)
        {
            tracks[i].target = characters[i].transform;
            
            tracks[i].GetComponent<Camera>().targetDisplay = i;
            characters[i].GetComponentInChildren<Canvas>().targetDisplay = i;
        }
    }
/*
    void ApplyCamera(CameraTrack[] tracks, Canvas canvas)
    {
        int display = canvas.targetDisplay;

        foreach (CameraTrack track in tracks)
        {
            int currentDisplay = track.GetComponent<Camera>().targetDisplay;
            if (display == currentDisplay)
            {
                track.
            }
        }
    }
  */  
}

[System.Serializable]
public class PlayerInfo
{
    public PlayerInfo(int userID, string characterName, int deviceId, InputDevice device)
    {
        this.userID = userID;
        this.characterName = characterName;
        this.deviceId = deviceId;
        this.device = device;
    }
    
    public int userID;
    public string characterName;
    public int deviceId;
    public InputDevice device;
}