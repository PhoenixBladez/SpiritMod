using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class UmbraStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Staff");
			Tooltip.SetDefault("Shoots out homing Shadow Balls");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.value = Item.buyPrice(0, 7, 0, 0);
			item.rare = 5;
			item.damage = 44;
			item.knockBack = 3;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 24;
			item.useAnimation = 24;
			item.mana = 6;
			item.magic = true;
            item.autoReuse = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("ShadowBall_Friendly");
			item.shootSpeed = 10f;
		}

	}
}
