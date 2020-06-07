using Terraria.ID;
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
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 24;
			item.value = 60150;
			item.rare = 3;
			item.knockBack = 2f;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shoot = ModContent.ProjectileType<BoneP>();
		}
	}
}
