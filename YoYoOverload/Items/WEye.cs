using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class WEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Peeper");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 25;
			base.item.value = 50000;
			base.item.rare = 4;
			base.item.knockBack = 3f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 28;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("Eye");
		}
	}
}
