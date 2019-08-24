using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class SlimeArrow : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slimed Arrow");
            Tooltip.SetDefault("Hit enemies are slimed up!");
        }

        public override void SetDefaults()
        { 
			item.width = 10;
			item.height = 28;
            item.rare = 1;
            item.value = Terraria.Item.buyPrice(0, 0, 0, 20);

            item.maxStack = 999;

            item.damage = 5;
			item.knockBack = 0;
            item.ammo = AmmoID.Arrow;

			item.ranged = true;
            item.consumable = true;

            item.shoot = mod.ProjectileType("SlimeArrowProj");
            item.shootSpeed = 3f;
        }
	}
}
