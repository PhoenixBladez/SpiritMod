using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Typhoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Typhoon");
			Tooltip.SetDefault("Shoots sharks at nearby enemies");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 26;
			item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			item.rare = 11;
			item.crit += 4;
			item.damage = 115;
			item.knockBack = 4f;
			item.useStyle = 5;
			item.useTime = 25;
			item.useAnimation = 25;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("Typhoon");
			item.UseSound = SoundID.Item1;
		}
	}
}
