using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.ReachBoss;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritMod.Items.Armor.Masks;
namespace SpiritMod.Tiles
{
	public class BloodBlossom : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.addTile(Type);
			//dustType = 7;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blood Blossom");
			AddMapEntry(new Color(234, 0, 0), name);
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if(!MyWorld.downedReachBoss) {
				return false;
			}
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			if(tile.frameY <= 18 && (tile.frameX <= 36 || tile.frameX >= 72)) {
				r = 0.301f * 1.5f;
				g = 0.110f * 1.5f;
				b = 0.126f * 1.5f;
			}
		}
        public override void MouseOver(int i, int j)
        {
            //shows the Cryptic Crystal icon while mousing over this tile
            Main.player[Main.myPlayer].showItemIcon = true;
            Main.player[Main.myPlayer].showItemIcon2 = ModContent.ItemType<ReachMask>();
        }

        public override void RightClick(int i, int j)
		{
			//don't bother if there's already a Crystal King in the world
			for(int x = 0; x < Main.npc.Length; x++) {
				if(Main.npc[x].active && Main.npc[x].type == ModContent.NPCType<ReachBoss>()) return;
			}
			Player player = Main.player[Main.myPlayer];
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ReachBoss>());
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			//if (tile.frameY == 18 || (tile.frameY == 36 && (tile.frameX == 18 || tile.frameX == 72)))
			{
				Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));

				Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/BloodBlossom_Glow");
				Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

				spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), colour);
			}
		}
	}
}
