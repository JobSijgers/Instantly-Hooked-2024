using System;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Boat
{
    public class BoatDeath : MonoBehaviour
    {
        [SerializeField] private Image deathBackground;
        [SerializeField] private GameObject deathUI;
        private void OnEnable()
        {
            EventManager.PlayerDied += Die;
        }
        
        private void OnDisable()
        {
            EventManager.PlayerDied -= Die;
        }

        private void Die()
        {
            StartCoroutine(ShowDeathUI());
        }
        
        private IEnumerator ShowDeathUI()
        {
            deathUI.gameObject.SetActive(true);
            deathBackground.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(4);
            float alpha = 1;
            deathUI.gameObject.SetActive(false);
            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                deathBackground.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
    }
}