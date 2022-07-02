
using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.IceSculpture.Hostile
{
	[TileTag(TileTags.Indestructible)]
	public class IceFlinxHostile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frozen Snow Flinx");
			DustType = DustID.SnowBlock;
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if (!NPC.downedBoss3) {
				return false;
			}
			return true;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(SoundID.Item27);
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<CreepingIce>(), Main.rand.Next(6, 13));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{

			Player player = Main.LocalPlayer;
			if (closer && NPC.downedBoss3) {
				int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
				if (distance1 < 56) {
					SoundEngine.PlaySound(SoundID.Item27);
					int n = NPC.NewNPC(new Terraria.DataStructures.EntitySource_TileUpdate(i, j), (int)i * 16, (int)j * 16, NPCID.SnowFlinx, 0, 2, 1, 0, 0, Main.myPlayer);
					Main.npc[n].GivenName = "Icy Snow Flinx";
					Main.npc[n].lifeMax = Main.npc[n].lifeMax * 2;
					Main.npc[n].life = Main.npc[n].lifeMax;
					Main.npc[n].damage = (int)(Main.npc[n].damage * 1.5f);
					Main.npc[n].knockBackResist = 0.25f;
                    Main.npc[n].netUpdate = true;
					WorldGen.KillTile(i, j);
				}
			}
		}
	}
}