using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Equinox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dawnstar");
			Tooltip.SetDefault("A perfect harmony of Dusk and Dawn.");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 45;
			base.item.value = 61420;
			base.item.rare = 5;
			base.item.knockBack = 3f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 23;
			base.item.shoot = base.mod.ProjectileType("EquinoxP");
		}
	}
}
