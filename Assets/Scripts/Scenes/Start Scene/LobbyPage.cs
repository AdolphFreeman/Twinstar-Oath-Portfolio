using System.Collections;
using CraneFSM.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace StartScene
{
    public class LobbyPage : State
    {
        public TextMeshProUGUI text1;
        public Button continueBtn;
        
        public override void Enter()
        {
            base.Enter();

            PageManager.Instance.OpenPage("Lobby Page");
            
            GetComponent<PlayerInputManager>().EnableJoining();
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
            GetComponent<PlayerInputManager>().DisableJoining();
        }
        
        //===
        public void BackStartPage()
        {
            sm.SetTrigger("backStart");
            
            PlayerInput[] inputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
            foreach (PlayerInput playerInput in inputs)
            {
                Destroy(playerInput.gameObject);
            }
        }

        public void StartGame()
        {
            sm.SetTrigger("toPrepare");
        }

        public void ToPreparePage()
        {
            //StartCoroutine(ToPrepareCoroutine());
            //eventSystems.SetSelectedGameObject(firstSelectObject);
            sm.SetTrigger("toPrepare");
        }

        IEnumerator ToPrepareCoroutine()
        {
            yield return new WaitForSeconds(3);
            sm.SetTrigger("toPrepare");
        }

        public void PlayerJoined(PlayerInput input)
        {
            print(input.playerIndex);
            int userId = input.playerIndex;

            if (userId == 0)
            {
                text1.text = "成功配對一位玩家，尚缺一位！";
            }
            
            if (userId == 1)
            {
                text1.text = "所有玩家已成功配對，請按下 '繼續' 以進行遊戲";
                continueBtn.gameObject.SetActive(true);
                
                EventSystem.current.SetSelectedGameObject(continueBtn.gameObject);
            }
        }
    }
}
