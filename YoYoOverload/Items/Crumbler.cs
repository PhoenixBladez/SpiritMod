using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Crumbler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Crumbler");
			Tooltip.SetDefault("There are crumbs everywhere!");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 123;
			base.item.value = 450000;
			base.item.rare = 8;
			base.item.knockBack = 1f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("CrumblerP");
		}
	}
}
