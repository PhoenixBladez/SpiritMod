using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SpiritMod.Tiles.Ambient.IceSculpture
{
	public class IceFlinxPassive : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frozen Snow Flinx");
            dustType = 51;
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions (int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (!NPC.downedBoss3)
            {
                return false;
            }
            return true;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			{
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
				Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("CreepingIce"), Main.rand.Next(6, 13));
			}
		}
    }
}