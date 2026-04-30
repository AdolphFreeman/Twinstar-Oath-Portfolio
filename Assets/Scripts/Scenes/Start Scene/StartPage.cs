using CraneFSM.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace StartScene
{
    public class StartPage : State
    {
        [Header("Elements")] 
        public GameObject startPage;
        
        public override void Enter()
        {
            base.Enter();
            PageManager.Instance.OpenPage("Start Page"); 
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
          base.Exit();   
        }
        
        //===
        public void StartGame()
        { 
            //StartSceneManager.Instance.DisableAllPage();
            sm.SetTrigger("toLobby");
        }

        public void DeviceChange(PlayerInput playerInput)
        {
            print(playerInput.currentControlScheme);
        }
    }
}