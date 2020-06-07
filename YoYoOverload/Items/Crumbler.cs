using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class Crumbler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Crumbler");
			Tooltip.SetDefault("There are crumbs everywhere!");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 123;
			item.value = Item.buyPrice(gold: 45);
			item.rare = 8;
			item.knockBack = 1f;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shoot = ModContent.ProjectileType<CrumblerP>();
		}
	}
}
