using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class SmallVentItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Small Hydrothermal Vent");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<Breakable1x2Vent>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
		}

		public override bool? UseItem(Player player)
		{
			Item.placeStyle = Main.rand.Next(0, 2);
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AshBlock, 10);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();
		}
	}

	[TileTag(TileTags.Indestructible)]
	public class HydrothermalVent1x2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.addTile(Type);

			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.Stone;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hydrothermal Vent");
			AddMapEntry(new Color(64, 54, 66), name);
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (player.ZoneBeach && player.GetSpiritPlayer().Submerged(45))
				HitWire(i, j);
			return true;
		}

		public sealed override void HitWire(int i, int j)
		{
			if (Wiring.CheckMech(i, j, 7200))
			{
				for (int k = 0; k <= 20; k++)
					Dust.NewDustPerfect(new Vector2(i * 16 + 12, j * 16 - 18), ModContent.DustType<Dusts.BoneDust>(), new Vector2(0, 6).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1));
				for (int k = 0; k <= 20; k++)
					Dust.NewDustPerfect(new Vector2(i * 16 + 12, j * 16 - 18), ModContent.DustType<Dusts.FireClubDust>(), new Vector2(0, 6).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1));
				Projectile.NewProjectile(new EntitySource_Wiring(i, j), i * 16 + 12, j * 16 - 36, 0, -4, ModContent.ProjectileType<Projectiles.HydrothermalVentPlume>(), 5, 0f);
			}
		}

		public sealed override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<SmallVentItem>();
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			var config = ModContent.GetInstance<Utilities.SpiritClientConfig>();

			Tile t = Framing.GetTileSafely(i, j);

			if (t.TileFrameY == 0)
				SpawnSmoke(new Vector2(i - 0.75f, j) * 16);

			if (config.VentCritters)
			{
				if (t.LiquidAmount > 155)
				{
					int npcIndex = -1;
					if (Main.rand.NextBool(2200))
					{
						if (NPC.MechSpawn((float)i * 16, (float)j * 16, ModContent.NPCType<NPCs.Critters.TinyCrab>()))
							npcIndex = NPC.NewNPC(new EntitySource_TileUpdate(i, j), i * 16, j * 16, ModContent.NPCType<NPCs.Critters.TinyCrab>());
					}
					if (npcIndex >= 0)
					{
						Main.npc[npcIndex].value = 0f;
						Main.npc[npcIndex].npcSlots = 0f;

					}
					int npcIndex1 = -1;
					if (!Framing.GetTileSafely(i + 1, j).HasTile && !Framing.GetTileSafely(i - 1, j).HasTile && !Framing.GetTileSafely(i + 2, j).HasTile && !Framing.GetTileSafely(i - 2, j).HasTile)
					{
						if (Main.rand.NextBool(300))
						{
							if (NPC.MechSpawn((float)i * 16, (float)j * 16, ModContent.NPCType<NPCs.Critters.Crinoid>()))
								npcIndex1 = NPC.NewNPC(new EntitySource_TileUpdate(i, j), i * 16, j * 16, ModContent.NPCType<NPCs.Critters.Crinoid>());
						}
					}
					if (npcIndex1 >= 0)
					{
						Main.npc[npcIndex1].value = 0f;
						Main.npc[npcIndex1].npcSlots = 0f;

					}
					int npcIndex2 = -1;
					if (!Framing.GetTileSafely(i + 1, j).HasTile && !Framing.GetTileSafely(i - 1, j).HasTile)
					{
						if (Main.rand.NextBool(85))
							if (NPC.MechSpawn((float)i * 16, (float)j * 16, ModContent.NPCType<NPCs.Critters.TubeWorm>()))
								npcIndex2 = NPC.NewNPC(new EntitySource_TileUpdate(i, j), i * 16, j * 16, ModContent.NPCType<NPCs.Critters.TubeWorm>());
					}
					if (npcIndex2 >= 0)
					{
						Main.npc[npcIndex2].value = 0f;
						Main.npc[npcIndex2].npcSlots = 0f;
					}
				}
			}
		}

		public static void SpawnSmoke(Vector2 pos)
		{
			if (Main.rand.NextBool(16))
				Gore.NewGorePerfect(new EntitySource_TileUpdate((int)pos.X / 16, (int)pos.Y / 16), pos, new Vector2(0, Main.rand.NextFloat(-2.2f, -1.5f)), 99, Main.rand.NextFloat(0.5f, 0.8f));
		}
	}

	[TileTag()]
	public class Breakable1x2Vent : HydrothermalVent1x2
	{
		public override string Texture => base.Texture.Replace(nameof(Breakable1x2Vent), nameof(HydrothermalVent1x2));

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<LargeVentItem>());
		public override bool CanExplode(int i, int j) => true;
		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => true;
	}
}
