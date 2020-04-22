using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SpiritMod.Tiles.Block
{
	public class ScrapTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
            Main.tileBlendAll[this.Type] = true;
            Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(150, 150, 150));
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("ScrapItem");
            soundType = 21;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.LocalPlayer;
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
        }
    }
}