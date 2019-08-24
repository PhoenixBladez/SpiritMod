using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Dryad : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunleaf");
			Tooltip.SetDefault("The warmth of the leaves surrounds you...");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 16;
			base.item.value = 30000;
			base.item.rare = 3;
			base.item.knockBack = 3f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 23;
			base.item.shoot = base.mod.ProjectileType("DryadP");
		}
	}
}
