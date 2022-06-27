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
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 100;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 999;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}