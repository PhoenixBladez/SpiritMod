﻿using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Arrow
{
	class MartianArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electrified Arrow");
			Tooltip.SetDefault("'Positively negative for enemies!'");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 30;
			item.value = Item.buyPrice(0, 0, 3, 0);
			item.rare = ItemRarityID.Cyan;

			item.maxStack = 999;

			item.damage = 16;
			item.knockBack = 2f;
			item.ammo = AmmoID.Arrow;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<ElectricArrow>();
			item.shootSpeed = 4f;
		}


	}
}
