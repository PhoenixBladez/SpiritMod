using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops
{
	public class TribalScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Scale");
			Tooltip.SetDefault("'Scales of old sea creatures'");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 42;
			Item.value = 2000;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
		}
	}
}
