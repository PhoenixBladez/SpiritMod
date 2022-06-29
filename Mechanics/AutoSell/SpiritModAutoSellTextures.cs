using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace SpiritMod.Mechanics.AutoSell
{
    public static class SpiritModAutoSellTextures
    {
        public static Asset<Texture2D> sellLockButton;
        public static Asset<Texture2D> sellNoValueButton;
        public static Asset<Texture2D> sellWeaponsButton;
        public static Asset<Texture2D> autoSellUIButton;

        public static void Load()
        {
            sellLockButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/Sell_Lock/Sell_Lock", AssetRequestMode.ImmediateLoad);
            sellNoValueButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/Sell_NoValue/Sell_NoValue", AssetRequestMode.ImmediateLoad);
            sellWeaponsButton = ModContent.Request<Texture2D>("SpiritMod/Mechanics/AutoSell/Sell_Weapons/Sell_Weapons", AssetRequestMode.ImmediateLoad);
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
