using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Boss;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class AvianEgg : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileCut[Type] = false;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16 };
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Avian Egg");
			AddMapEntry(new Color(227, 195, 124), name);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			TileUtilities.BlockActuators(i, j);
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Main.NewText("The Ancient Avian has awoken!", 175, 75, 255);
			int n = NPC.NewNPC(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16 + Main.rand.Next(-60, 60), j * 16 - 120, ModContent.NPCType<AncientFlyer>(), 0, 2, 1, 0, 0, Main.myPlayer);
			Main.npc[n].netUpdate = true;

			for (int l = 0; l < 2; l++)
			{
				float scale = 0.2f;
				if (l == 1)
					scale = 0.5f;
				else if (l == 2)
					scale = 1f;

				for (int k = 0; k < 2; ++k)
				{
					Gore gore = Main.gore[Gore.NewGore(new Terraria.DataStructures.EntitySource_TileBreak(i, j), new Vector2(i * 16 + Main.rand.Next(-60, 60), j * 16 - 120), default, Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scale;
					gore.velocity.X += 1f;
					gore.velocity.Y += i == 0 ? -1f : 1f;
				}
			}

			SoundEngine.PlaySound(SoundID.Roar, new Vector2(i * 16, j * 16));
			SoundEngine.PlaySound(SoundID.NPCDeath1);

			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16 - 10), 0, 16, DustID.Dirt, 0.0f, -1, 0, new Color(), 0.5f);
				Gore.NewGore(new Terraria.DataStructures.EntitySource_TileBreak(i, j), new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-10, 10)), new Vector2(-1, 1), Mod.Find<ModGore>("Gores/Apostle2").Type, Main.rand.NextFloat(.7f, 1.8f));
			}
		}
	}
}