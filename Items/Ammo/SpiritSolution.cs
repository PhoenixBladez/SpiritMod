using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class SpiritSolution : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Solution");
            Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Spirit");
        }

        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("SpiritSolution") - ProjectileID.PureSpray;
            item.ammo = AmmoID.Solution;
            item.width = 10;
            item.height = 12;
            item.value = Item.buyPrice(0, 0, 7, 0);
            item.rare = 3;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
