using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class SkellyV : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ViceLaser");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 41;
			base.item.value = 300000;
			base.item.rare = 5;
			base.item.knockBack = 3f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 28;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("SkellyP");
		}
	}
}
