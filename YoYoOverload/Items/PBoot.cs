using System;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class PBoot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate's Booty");
			Tooltip.SetDefault("Causes Enemies to drop more Gold");
		}


		public override void SetDefaults()
		{
			base.item.CloneDefaults(3278);
			base.item.damage = 42;
			base.item.value = 151420;
			base.item.rare = 5;
			base.item.knockBack = 2f;
			base.item.channel = true;
			base.item.useStyle = 5;
			base.item.useAnimation = 25;
			base.item.useTime = 23;
			base.item.shoot = base.mod.ProjectileType("PBootP");
		}
	}
}
