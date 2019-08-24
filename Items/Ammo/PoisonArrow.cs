using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class PoisonArrow : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poison Arrow");
            Tooltip.SetDefault("Hit enemies are poisoned");
        }

        public override void SetDefaults()
		{
			item.width = 10;
			item.height = 28;
            item.rare = 1;
            item.value = Terraria.Item.buyPrice(0, 0, 0, 40);

            item.maxStack = 999;

            item.damage = 7;
			item.knockBack = 0;
            item.ammo = AmmoID.Arrow;

			item.ranged = true;
            item.consumable = true;

            item.shoot = mod.ProjectileType("PoisonArrowProj");
            item.shootSpeed = 4f;
        }
	}
}
