using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace SpiritMod.Items.Sets.FloatingItems.Driftwood
{
	public class DriftwoodTileItem : FloatingItem
	{
		public override float SpawnWeight => 1f;
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.05f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Driftwood Block");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.White;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.createTile = ModContent.TileType<DriftwoodTile>();
			Item.useTime = Item.useAnimation = 20;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noMelee = true;
			Item.autoReuse = true;
		}
	}

	public class DriftwoodTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(138, 79, 45));
			ItemDrop = ModContent.ItemType<DriftwoodTileItem>();
		}

		public override bool CanExplode(int i, int j) => true;
	}
}