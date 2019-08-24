using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class DemonArrow : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Arrow");
            Tooltip.SetDefault("Gains velocity while in air");
        }

        public override void SetDefaults()
		{
			
			item.width = 10;
			item.height = 28;
            item.rare = 3;
            item.value = Terraria.Item.buyPrice(0, 0, 2, 0);
            item.value = 1000;

            item.maxStack = 999;

            item.damage = 11;
			item.knockBack = 0;
            item.ammo = AmmoID.Arrow;

			item.ranged = true;
            item.consumable = true;

            item.shoot = mod.ProjectileType("DemonArrowProj");
            item.shootSpeed = 1f;
        }
	}
}
