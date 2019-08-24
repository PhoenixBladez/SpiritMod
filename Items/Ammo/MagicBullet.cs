using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	public class MagicBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Bullet");
			Tooltip.SetDefault("'What can't it do?'");
		}


		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 16;
            item.value = 1000;
            item.rare = 10;
            item.value = Item.buyPrice(0, 0, 8, 0);
            item.maxStack = 999;

            item.damage = 25;
			item.knockBack = 2.5f;
            item.ammo = AmmoID.Bullet;

            item.ranged = true;
            item.consumable = true;

            item.shoot = mod.ProjectileType("MagicBullet");
			item.shootSpeed = 9f;

		}
    }
}