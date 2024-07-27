﻿using Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBuff.Core.BuffMenu.Manual
{
    internal class BuffGameModeManualPage : BuffManualPage
    {
        public BuffGameModeManualPage(BuffGameManual manual, int pageNumber, MenuObject owner) : base(manual, pageNumber, owner)
        {
            float anchorY = rectHeight;
            if(pageNumber == 0)
            {
                AddIllusitration("buffassets/illustrations", "manual_gamemode_instruction", ref anchorY);
                anchorY -= 25f;
                AddText(BuffResourceString.Get("BuffManual_GameMode_Content_0"), true, ref anchorY);
            }
            else if(pageNumber == 1)
            {
                AddIllusitration("buffassets/illustrations", "manual_gamemode_freemode", ref anchorY);
                anchorY += 85f;
                AddText(BuffResourceString.Get("BuffManual_GameMode_Content_1"), false, ref anchorY);
            }
            else if (pageNumber == 2)
            {
                anchorY -= 25f;
                AddText(BuffResourceString.Get("BuffManual_GameMode_Content_2"), false, ref anchorY);
            }
            else if (pageNumber == 3)
            {
                anchorY -= 25f;
                AddText(BuffResourceString.Get("BuffManual_GameMode_Content_3"), false, ref anchorY);
            }
            else if (pageNumber == 4)
            {
                anchorY -= 25f;
                AddText(BuffResourceString.Get("BuffManual_GameMode_Content_4"), false, ref anchorY);
            }
            else if (pageNumber == 5)
            {
                anchorY -= 25f;
                AddText(BuffResourceString.Get("BuffManual_GameMode_Content_5"), false, ref anchorY);
            }
        }
    }
}