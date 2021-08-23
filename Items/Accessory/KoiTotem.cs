
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
namespace SpiritMod.Items.Accessory
{
	public class KoiTotem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Koi Totem");
			Tooltip.SetDefault("Increases fishing skill\nTotem occasionally spits out the bait that was used for reusability");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 36;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.createTile = ModContent.TileType<KoiTotem_Tile>();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().KoiTotem = true;
			player.fishingSkill = player.fishingSkill + 5;
		}

	}
	public class KoiTotem_Tile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16
			};
			TileObjectData.addTile(Type);
            dustType = 1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Koi Totem");
			AddMapEntry(new Color(107, 90, 64), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<KoiTotem>());
		}
    }
}
