using UnityEngine;
using UnityEngine.InputSystem;

public class Stater : MonoBehaviour
{
    public GameObject prefab;
    public GameObject[] characterPrefabs;
    
    void Start()
    {
        for(int i = 0; i < 2; ++i)
        {
            PlayerInput playerInput = PlayerInput.Instantiate(characterPrefabs[i], 
                playerIndex: i,
                controlScheme: "gamepad",
                pairWithDevice: Gamepad.all[i]);

            Canvas canvas = playerInput.GetComponentInChildren<Canvas>();
            canvas.targetDisplay = i;
        }
    }
}
