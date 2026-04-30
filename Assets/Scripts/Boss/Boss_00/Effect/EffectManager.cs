using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boss_01
{
    public class EffectManager : MonoBehaviour
    { 
        public enum State
        {
            State1, State2
        }
        
        public enum EffectType
        {
            None = 0, Effect0 = 1, Effect1 = 2, Effect2= 3, Effect3 = 4
        }
        
        public State state;
        public int status;
        public float distanceBetweenPlayers;

        public GameObject[] players;
        public float damageCoolTime;
        private float  _currentDamageCoolTime;
        public float damage;
        public EffectType type;
        public GameObject[] vfxs;
        
        private void Start()
        {
            StartCoroutine(Setup());
        }

        private void Update()
        {
            if(players.Length != 2)
                return;

            distanceBetweenPlayers = GetDistanceBetweenPlayers();
            
            ExecuteEffect();
            Randerer();
        }

        public void ApplyEffect()
        {
            switch (state)
            {
                case State.State1:
                    int randType = Random.Range(1, 3);
                    type = (EffectType)randType;
                    break;
            }
        }

        public float GetDistanceBetweenPlayers()
        {
            if (players.Length == 0) return -0.001f;
            return Vector3.Distance(players[0].transform.position, players[1].transform.position);
        }
        
        //===
        void ExecuteEffect()
        {
            if(_currentDamageCoolTime >= 0)
                _currentDamageCoolTime -= Time.deltaTime;
            
            switch (type)
            {
                case EffectType.Effect0:
                    if (_currentDamageCoolTime <= 0 && GetDistanceBetweenPlayers() <= 10)
                    {
                        foreach (GameObject player in players)
                        {
                            PlayerCore core = player.GetComponent<PlayerCore>();
                            core.GetDamage(damage);
                        }
                        _currentDamageCoolTime = damageCoolTime;
                    }
                    break;
                case EffectType.Effect1:
                    if (_currentDamageCoolTime <= 0 && GetDistanceBetweenPlayers() >= 10)
                    {
                        foreach (GameObject player in players)
                        {
                            PlayerCore core = player.GetComponent<PlayerCore>();
                            core.GetDamage(damage);
                        }
                        _currentDamageCoolTime = damageCoolTime;
                    }
                    break;
                case EffectType.Effect2:
                    break;
                case EffectType.Effect3:
                    break;
            }
        }

        void Randerer()
        {
            switch (type)
            {
                case EffectType.None:
                    vfxs[0].SetActive(false);
                    vfxs[1].SetActive(false);
                    vfxs[2].SetActive(false);
                    vfxs[3].SetActive(false);
                    break;
                case EffectType.Effect0:
                    vfxs[0].SetActive(true);
                    vfxs[1].SetActive(true);
                    
                    for(int i = 0; i < players.Length; i++)
                    {
                        vfxs[i].transform.position = players[i].GetComponent<PlayerCore>().Center();
                    }
                    break;
                case EffectType.Effect1:
                    vfxs[2].SetActive(true);
                    vfxs[3].SetActive(true);
                    
                    for(int i = 0; i < players.Length; i++)
                    {
                        vfxs[2 + i].transform.position = players[i].GetComponent<PlayerCore>().Center();
                    }
                    break;
                case EffectType.Effect2:
                    break;
                case EffectType.Effect3:
                    break;
            }
        }

        //===
        IEnumerator Setup()
        {
            while (GameObject.FindGameObjectsWithTag("Player").Length < 2)
            {
                print(GameObject.FindGameObjectsWithTag("Player").Length);
                yield return new WaitForSeconds(0.1f); 
            }
            
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}
