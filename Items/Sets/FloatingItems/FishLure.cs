using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;

namespace SpiritMod.Items.Sets.FloatingItems
{
	public class FishLure : FloatingItem
	{
		public override float SpawnWeight => .008f;
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.08f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fish Lure");
			Tooltip.SetDefault("Can only be placed in water\nAttracts schools of fish to nearby waters");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.maxStack = 99;
			Item.createTile = ModContent.TileType<FishLureTile>();
			Item.useTime = Item.useAnimation = 20;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.noMelee = true;
			Item.autoReuse = false;
		}
	}

	public class FishLureTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.WaterPlacement = Terraria.Enums.LiquidPlacement.OnlyInFullLiquid;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bar");
			ItemDrop = ModContent.ItemType<FishLure>();
			AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public sealed override void NearbyEffects(int i, int j, bool closer)
		{
			if (Framing.GetTileSafely(i, j + 1).LiquidAmount < 155 && Framing.GetTileSafely(i, j).LiquidAmount < 155) //Kill me if I'm thirsty (aka kill if there's no water)
				WorldGen.KillTile(i, j);
			Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (closer)
				modPlayer.nearLure = true;
			else
				modPlayer.nearLure = false;
		}
	}
}