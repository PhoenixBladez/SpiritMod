using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class SoullessSolution : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soulless Solution");
            Tooltip.SetDefault("Used by the Clentaminator\nDestroys the Spirit");
        }

        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("SoullessSolution") - ProjectileID.PureSpray;
            item.ammo = AmmoID.Solution;
            item.width = 10;
            item.height = 12;
            item.value = 0;
            item.rare = 3;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
