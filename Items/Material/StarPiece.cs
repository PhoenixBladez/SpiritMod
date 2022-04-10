using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class StarPiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Piece");
			Tooltip.SetDefault("'A Cosmic Shard'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 100;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 999;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}