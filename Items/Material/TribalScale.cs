using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Material
{
	public class TribalScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tribal Scale");
			Tooltip.SetDefault("'Scales of old sea creatures'");
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 42;
			item.value = 2000;
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
		}
	}
}
