using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Ichor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purulence");
			Tooltip.SetDefault("It oozes pus... Ugh...");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 33;
			base.item.value = 40450;
			base.item.rare = 5;
			base.item.knockBack = 1f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("IchorP");
		}
	}
}
