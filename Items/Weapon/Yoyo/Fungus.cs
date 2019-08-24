using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Fungus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fungus");
			Tooltip.SetDefault("Leaves behind a damaging trail of mushrooms");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 26;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = 6;
			item.crit += 4;
			item.damage = 54;
			item.knockBack = 3f;
			item.useStyle = 5;
			item.useTime = 25;
			item.useAnimation = 25;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("Saprophyte");
			item.UseSound = SoundID.Item1;
		}
	}
}
