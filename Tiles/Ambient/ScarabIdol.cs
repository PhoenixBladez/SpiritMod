
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.NPCs.Boss.Scarabeus;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class ScarabIdol : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16
			};
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Scarab Idol");
			AddMapEntry(new Color(245, 179, 66), name);
			dustType = DustID.GoldCoin;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .245f;
			g = .179f;
			b = .066f;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<ScarabIdolQuest>());
			Main.PlaySound(SoundID.Zombie, (int)i * 16, (int)j * 16, 44);
			CombatText.NewText(new Rectangle((int)i * 16 + 10, (int)j * 16 - 10, 48, 48), new Color(204, 153, 0, 100),
"Scarabs are pouring out from the walls!");
			NPC.NewNPC((int)i * 16 + Main.rand.Next(-40, -30), (int)j * 16 + 5, ModContent.NPCType<Scarab>(), 0, 2, 1, 0, 0, Main.myPlayer);
			NPC.NewNPC((int)i * 16 + Main.rand.Next(-30, -10), (int)j * 16 + 3, ModContent.NPCType<Scarab>(), 0, 2, 1, 0, 0, Main.myPlayer);
			NPC.NewNPC((int)i * 16 + Main.rand.Next(0, 20), (int)j * 16, ModContent.NPCType<Scarab>(), 0, 2, 1, 0, 0, Main.myPlayer);
			NPC.NewNPC((int)i * 16 + Main.rand.Next(30, 40), (int)j * 16 + 3, ModContent.NPCType<Scarab>(), 0, 2, 1, 0, 0, Main.myPlayer);
			NPC.NewNPC((int)i * 16 + Main.rand.Next(50, 60), (int)j * 16 + 5, ModContent.NPCType<Scarab>(), 0, 2, 1, 0, 0, Main.myPlayer);
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile t = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if(t.frameX % 72 == 0 && t.frameY == 0) {
				Main.spriteBatch.Draw(Main.extraTexture[89], new Vector2(i * 16 - (int)Main.screenPosition.X - 6, j * 16 - (int)Main.screenPosition.Y - 9) + zero, null, new Color(245, 179, 66, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}