﻿using System.Collections;
using UnityEngine;
using Pacmania.InGame.UI;
using Pacmania.InGame.Arenas;
using Pacmania.InGame.Characters.Pacman;

namespace Pacmania.InGame.Pickups
{
    class BonusSpawner : MonoBehaviour
    {
        [SerializeField] private Bonus firstBonus = default;
        [SerializeField] private Bonus secondBonus = default;
        [SerializeField] private Bonus thirdBonus = default;
        [SerializeField] private Bonus secretBonus = default;
        [SerializeField] [Range(1, 10)]  private int secretBonusOdds = 2;
        [SerializeField] [Range(1, 150)] private int firstBonusPellets = 30;
        [SerializeField] [Range(2, 180)] private int secondBonusPellets = 45;
        [SerializeField] [Range(3, 210)] private int thirdBonusPellets = 60;

        private int sortingOrder = 0;
        private Vector3 startingLocation;
        private int pelletsEatenCount = 0;
        private const float secondsToShowBonusText = 2.0f;
        private bool secretBonusSpawned = false;
        private Level level;
        private const string fruitTargetString = "FRUIT TARGET!";
        private const string specialItemString = "SPECIAL ITEM!";

        private void Awake()
        {
            level = FindObjectOfType<Level>();
        }

        private void Start()
        {
            level.Pacman.GetComponent<PacmanCollision>().EatenPellete += BonusSpawner_EatenPellete;
            Arena arena = level.Arena;
            sortingOrder = arena.GetTileSortingOrder(arena.BonusTile);
            startingLocation = arena.GetTransformPositionFromArenaPosition(arena.GetArenaPositionForTileCenter(arena.BonusTile));
        }

        private GameObject SpawnBonus(Bonus defautBonus)
        {
            // On some occasions spawn secret bonus instead.
            if (secretBonusSpawned == false && secretBonus != null && secretBonusOdds > 0 && level.RandomStream.Range(0,secretBonusOdds-1)==0 )
            {
                secretBonusSpawned = true;
                return Instantiate(secretBonus.gameObject, startingLocation, Quaternion.identity);
            }

            // Just instantiate default given game object
            return Instantiate(defautBonus.gameObject, startingLocation, Quaternion.identity);
        }

        private void BonusSpawner_EatenPellete()
        {
            pelletsEatenCount++;
            GameObject bonusObject = null;

            if (pelletsEatenCount == firstBonusPellets && firstBonus != null)
            {
                bonusObject = SpawnBonus(firstBonus);
            }
            else if (pelletsEatenCount == secondBonusPellets && secondBonus != null)
            {
                bonusObject = SpawnBonus(secondBonus);
            }
            if (pelletsEatenCount == thirdBonusPellets && thirdBonus != null)
            {
                bonusObject = SpawnBonus(thirdBonus);
            }

            if (bonusObject != null)
            {       
                bonusObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                level.AudioManager.Play(Audio.SoundType.PicupSpawned);
                StartCoroutine(ShowBonusTextCoroutine(bonusObject));
            }
        }

        private IEnumerator ShowBonusTextCoroutine(GameObject forObject)
        {
            Hud hud = level.Hud;
            hud.SetBonusTargetTextVisibility(true);
            if (forObject.name == "Fruit")
            {
                hud.SetBonusTargetText(fruitTargetString);
            }
            else
            {
                hud.SetBonusTargetText(specialItemString);
            }

            yield return new WaitForSeconds(secondsToShowBonusText);

            hud.SetBonusTargetTextVisibility(false);
        }
    }
}
