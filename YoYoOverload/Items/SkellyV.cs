using System;
using Terraria;
using Terraria.ID;
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
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 41;
			item.value = Item.buyPrice(gold: 30);
			item.rare = 5;
			item.knockBack = 3f;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 28;
			item.useTime = 25;
			item.shoot = ModContent.ProjectileType<SkellyP>();
		}
	}
}
