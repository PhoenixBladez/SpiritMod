using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Bone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Bonesaw");
			Tooltip.SetDefault("Bonesaw is READY!");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 24;
			base.item.value = 60150;
			base.item.rare = 3;
			base.item.knockBack = 2f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 25;
			base.item.shoot = base.mod.ProjectileType("BoneP");
		}
	}
}
