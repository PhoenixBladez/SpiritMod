using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class SmallVentItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Small Hydrothermal Vent");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<HydrothermalVent1x2>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}

		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(0, 2);
			return base.UseItem(player);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AshBlock, 10);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class HydrothermalVent1x2 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand };
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
			dustType = DustID.Stone;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hydrothermal Vent");
			AddMapEntry(new Color(64, 54, 66), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		public override void NearbyEffects(int i, int j, bool closer)
		{

			Tile t = Framing.GetTileSafely(i, j);

			if (t.frameY == 0)
				SpawnSmoke(new Vector2(i - 0.75f, j) * 16);

			if (t.liquid > 155)
			{
				int npcIndex = -1;
				if (Main.rand.NextBool(1600))
				{
					if (NPC.MechSpawn((float)i * 16, (float)j * 16, ModContent.NPCType<NPCs.Critters.TinyCrab>()))
						npcIndex = NPC.NewNPC(i * 16, j * 16, ModContent.NPCType<NPCs.Critters.TinyCrab>());
				}
				if (npcIndex >= 0)
				{
					Main.npc[npcIndex].value = 0f;
					Main.npc[npcIndex].npcSlots = 0f;

				}
				int npcIndex1 = -1;
				if (!Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - 1, j).active())
				{
					if (Main.rand.NextBool(350))
					{
						if (NPC.MechSpawn((float)i * 16, (float)j * 16, ModContent.NPCType<NPCs.Critters.Crinoid>()))
							npcIndex1 = NPC.NewNPC(i * 16, j * 16, ModContent.NPCType<NPCs.Critters.Crinoid>());
					}
				}
				if (npcIndex1 >= 0)
				{
					Main.npc[npcIndex1].value = 0f;
					Main.npc[npcIndex1].npcSlots = 0f;

				}
			}
		}

		public static void SpawnSmoke(Vector2 pos)
		{
			if (Main.rand.NextBool(16))
			{
				int type = 99;
				Gore.NewGorePerfect(pos, new Vector2(0, Main.rand.NextFloat(-2.2f, -1.5f)), type, Main.rand.NextFloat(0.5f, 0.8f));
			}
		}
	}
}
