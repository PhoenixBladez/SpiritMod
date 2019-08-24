using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Copper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fool's Gold");
			Tooltip.SetDefault("I mean... Copper's better than nothing, right?");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 11;
			base.item.value = 20000;
			base.item.rare = 3;
			base.item.knockBack = 2f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("CopperP");
		}
	}
}
