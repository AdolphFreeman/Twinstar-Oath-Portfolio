using System.Linq;
using CraneFSM.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StartScene
{
    public class PrepareState : State
    {
        public string[] pickCharacters = new string[2];
        
        public GameObject[] player1PickObjects = new GameObject[2];
        public GameObject[] player2PickObjects = new GameObject[2];
        
        public override void Enter()
        {
            base.Enter();
            
            pickCharacters[0] = "Warrior";
            pickCharacters[1] = "Wizard";
            
            PageManager.Instance.OpenPage("Prepare Page");
            
            SelectRandomUI();
        }

        public override void Execute()
        {
            base.Execute();

            if (pickCharacters[0] == "Warrior")
            {
                player1PickObjects[0].SetActive(true);
                player1PickObjects[1].SetActive(false);
                
                player2PickObjects[0].SetActive(false);
                player2PickObjects[1].SetActive(true);
            }
            else if (pickCharacters[0] == "Wizard")
            {
                player1PickObjects[0].SetActive(false);
                player1PickObjects[1].SetActive(true);
                
                player2PickObjects[0].SetActive(true);
                player2PickObjects[1].SetActive(false);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public void SwitchCharacter()
        {
            if (pickCharacters[0] == "Warrior")
            {
                pickCharacters[1] = "Warrior";
                pickCharacters[0] = "Wizard";
            }
            else if (pickCharacters[0] == "Wizard")
            {
                pickCharacters[0] = "Warrior";
                pickCharacters[1] = "Wizard";
            }
        }

        public void StartGame()
        {
            PlayerInput[] inputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);

            for (int i = 0; i < inputs.Length; i++)
            {
                PlayerInput currentInput = inputs[i];
                GameManger.Instance.playerInfos.Add(new PlayerInfo(currentInput.playerIndex,
                    pickCharacters[i], currentInput.GetDevice<Gamepad>().deviceId, currentInput.GetDevice<Gamepad>()));
            }
            SceneManager.LoadScene(1);
        }
        
        public void SelectRandomUI()
        {
            // 1. 獲取場景中所有啟用的 Selectable 元件 (Button, Slider, Toggle 等)
            var allSelectables = Selectable.allSelectablesArray;

            // 2. 過濾掉不可點擊、隱藏或不屬於 UI 層級的物件
            var eligibleSelectables = allSelectables.Where(s => 
                s.interactable && 
                s.gameObject.activeInHierarchy && 
                s.navigation.mode != Navigation.Mode.None
            ).ToList();

            if (eligibleSelectables.Count > 0)
            {
                // 3. 隨機抽取一個索引
                int randomIndex = Random.Range(0, eligibleSelectables.Count);
                GameObject target = eligibleSelectables[randomIndex].gameObject;

                // 4. 強制 EventSystem 設定焦點
                EventSystem.current.SetSelectedGameObject(target);
            
                Debug.Log($"手把模式啟動：隨機選中了 {target.name}");
            }
        }
    }
}
