using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Granite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crackling Core");
			Tooltip.SetDefault("The Core within occasionally reduces enemy defense.");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 23;
			base.item.value = 61420;
			base.item.rare = 3;
			base.item.knockBack = 5f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 23;
			base.item.shoot = base.mod.ProjectileType("GraniteP");
		}
	}
}
