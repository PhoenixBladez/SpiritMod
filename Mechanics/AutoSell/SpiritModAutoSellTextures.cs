using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Mechanics.AutoSell
{
    public static class SpiritModAutoSellTextures
    {
        public static Texture2D sellLockButton;
        public static Texture2D sellNoValueButton;
        public static Texture2D sellWeaponsButton;
        public static Texture2D autoSellUIButton;

        public static void Load()
        {
            sellLockButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/Sell_Lock/Sell_Lock");
            sellNoValueButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/Sell_NoValue/Sell_NoValue");
            sellWeaponsButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/Sell_Weapons/Sell_Weapons");
            autoSellUIButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/AutoSellUI");
        }

        public static void Unload()
        {
            sellLockButton = null;
            sellNoValueButton = null;
            sellWeaponsButton = null;
            autoSellUIButton = null;
        }
    }
}
