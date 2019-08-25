using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SpiritMod.Tiles.Ambient
{
	public class AvianEgg : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;	
			Main.tileCut[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16,
			16,
			16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Avian Egg");
			AddMapEntry(new Color(227, 195, 124), name);
		}
		public override void SetDrawPositions (int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
            Main.NewText("The Ancient Avian has awoken!", 80, 80, 210, true);
		    NPC.NewNPC((int)i * 16 + Main.rand.Next(-60, 60), (int)j * 16 + 150, mod.NPCType("AncientFlyer"), 0, 2, 1, 0, 0, Main.myPlayer);
		   	Main.PlaySound(SoundID.Roar, new Vector2((int)i *16, (int)j*16), 0);
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 1));
			for (int k = 0; k < 8; k++)
            {
            int d  = Dust.NewDust(new Vector2(i*16 , j * 16 -10), 0, 16, 0, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division
			Gore.NewGore(new Vector2((int)i*16+ Main.rand.Next(-10, 10), (int)j*16 + Main.rand.Next(-10, 10)), new Vector2(-1, 1), mod.GetGoreSlot("Gores/Apostle2"), 1f);
            }
        }

	}
}