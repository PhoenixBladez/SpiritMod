﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class LargeVentItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Large Hydrothermal Vent");

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<Breakable1x3Vent>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}

		public override bool UseItem(Player player)
		{
			item.placeStyle = Main.rand.Next(2);
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AshBlock, 20);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[TileTag(TileTags.Indestructible)]
	public class HydrothermalVent1x3 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 3);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
			dustType = DustID.Stone;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hydrothermal Vent");
			AddMapEntry(new Color(64, 54, 66), name);
		}

		public override bool NewRightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (player.ZoneBeach && player.GetSpiritPlayer().isFullySubmerged)
				HitWire(i, j);
			return true;
		}

		public sealed override void HitWire(int i, int j)
		{
			if (Wiring.CheckMech(i, j, 7200))
			{
				for (int k = 0; k <= 20; k++)
					Dust.NewDustPerfect(new Vector2(i * 16 + 12, j * 16 - 36), ModContent.DustType<Dusts.BoneDust>(), new Vector2(0, 6).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1));

				for (int k = 0; k <= 20; k++)
					Dust.NewDustPerfect(new Vector2(i * 16 + 12, j * 16 - 36), ModContent.DustType<Dusts.FireClubDust>(), new Vector2(0, 6).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1));

				Projectile.NewProjectile(i * 16 + 12, j * 16 - 36, 0, -7, ModContent.ProjectileType<Projectiles.HydrothermalVentPlume>(), 5, 0f);
			}
		}

		public sealed override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<LargeVentItem>();
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			var config = ModContent.GetInstance<Utilities.SpiritClientConfig>();

			Tile t = Framing.GetTileSafely(i, j);

			if (t.frameY == 0 && t.frameX == 0)
				HydrothermalVent1x2.SpawnSmoke(new Vector2(i - 0.75f, j) * 16);

			if (config.VentCritters)
			{
				if (t.liquid > 155)
				{
					int npcIndex = -1;
					if (Main.rand.NextBool(2200))
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
					if (!Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - 1, j).active() && !Framing.GetTileSafely(i + 2, j).active() && !Framing.GetTileSafely(i - 2, j).active())
					{
						if (Main.rand.NextBool(300))
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

					int npcIndex2 = -1;
					if (!Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - 1, j).active())
					{
						if (Main.rand.NextBool(85))
						{
							if (NPC.MechSpawn((float)i * 16, (float)j * 16, ModContent.NPCType<NPCs.Critters.TubeWorm>()))
								npcIndex2 = NPC.NewNPC(i * 16, j * 16, ModContent.NPCType<NPCs.Critters.TubeWorm>());
						}
					}

					if (npcIndex2 >= 0)
					{
						Main.npc[npcIndex2].value = 0f;
						Main.npc[npcIndex2].npcSlots = 0f;
					}
				}
			}
		}
	}

	[TileTag()]
	public class Breakable1x3Vent : HydrothermalVent1x3
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = texture.Replace(nameof(Breakable1x3Vent), nameof(HydrothermalVent1x3));
			return base.Autoload(ref name, ref texture);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 16, 48, ModContent.ItemType<LargeVentItem>());
		public override bool CanExplode(int i, int j) => true;
		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => true;
	}
}
