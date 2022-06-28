
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using SpiritMod.Buffs;

namespace SpiritMod.Items.Accessory
{
	public class KoiTotem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Koi Totem");
			Tooltip.SetDefault("Increases fishing skill when worn or when placed nearby\nTotem occasionally spits out the bait that was used for reusability\n");
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.createTile = ModContent.TileType<KoiTotem_Tile>();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().KoiTotem = true;
			player.AddBuff(ModContent.BuffType<KoiTotemBuff>(), 2);
		}
	}

	public class KoiTotem_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.addTile(Type);

            DustType = DustID.Stone;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Koi Totem");
			AddMapEntry(new Color(107, 90, 64), name);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Main.LocalPlayer.GetSpiritPlayer().KoiTotem = true;
				Main.LocalPlayer.AddBuff(ModContent.BuffType<KoiTotemBuff>(), 12);
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 48, ModContent.ItemType<KoiTotem>());
	}
}
