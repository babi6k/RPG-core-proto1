﻿using GameDevTV.Core.UI.Tooltips;
using UnityEngine;

namespace RPG.UI
{
    public class QuestToolTipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
           
        }
    }
}
