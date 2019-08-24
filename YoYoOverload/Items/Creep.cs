using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Creep : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Creeper");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 18;
			base.item.value = 90040;
			base.item.rare = 3;
			base.item.knockBack = 3f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("CreepP");
		}
	}
}
