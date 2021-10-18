﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pacmania.InGame.ScoreSprites
{
    public class ScoreRising : MonoBehaviour
    {
        public int Score;

        public void AnimationFinished()
        {
            Destroy(gameObject);
        }
    }
}
