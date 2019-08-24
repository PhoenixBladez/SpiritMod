using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Murk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murk");
			Tooltip.SetDefault("");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 26;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = 7;
			item.crit += 6;
			item.damage = 64;
			item.knockBack = 3.5f;
			item.useStyle = 5;
			item.useTime = 25;
			item.useAnimation = 25;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("Murk");
			item.UseSound = SoundID.Item1;
		}

	}
}
