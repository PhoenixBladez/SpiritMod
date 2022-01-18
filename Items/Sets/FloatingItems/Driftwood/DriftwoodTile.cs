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
			DisplayName.SetDefault("Driftwood Block");		}

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.White;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.maxStack = 99;
			item.createTile = ModContent.TileType<DriftwoodTile>();
			item.useTime = item.useAnimation = 20;
			item.useAnimation = 15;
			item.useTime = 15;
			item.noMelee = true;
			item.autoReuse = false;
		}
	}

	public class DriftwoodTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(138, 79, 45));
			drop = ModContent.ItemType<DriftwoodTileItem>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}