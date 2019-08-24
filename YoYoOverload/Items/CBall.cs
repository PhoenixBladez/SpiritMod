using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class CBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Chaos Ball");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 15;
			base.item.value = 70000;
			base.item.rare = 2;
			base.item.knockBack = 2f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 24;
			base.item.shoot = base.mod.ProjectileType("CBallP");
		}
	}
}
