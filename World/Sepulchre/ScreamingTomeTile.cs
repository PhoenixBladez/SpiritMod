using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SepulchreLoot.ScreamingTome;
using SpiritMod.NPCs.HauntedTome;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.World.Sepulchre
{
	public class ScreamingTomeTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Mysterious Tome");
			AddMapEntry(new Color(179, 146, 107), name);
			DustType = -1;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3; 
		
		public override void MouseOver(int i, int j)
		{
			Main.player[Main.myPlayer].cursorItemIconEnabled = true;
			Main.player[Main.myPlayer].cursorItemIconID = ModContent.ItemType<ScreamingTome>();
		}

		public override bool RightClick(int i, int j)
		{
			WorldGen.KillTile(i, j);
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);

			return true;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient || fail)
				return;

			Main.npc[NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, ModContent.NPCType<HauntedTome>())].netUpdate = true;
			SoundEngine.PlaySound(SoundID.NPCDeath6, new Vector2(i * 16, j * 16));
		}
	}
}
