using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class ThrownCutlass : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Greedy Dagger");
            Tooltip.SetDefault("Hit enemies drop more gold");
        }


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 30;
            item.height = 30;           
            item.shoot = mod.ProjectileType("ThrownCutlass");
            item.useAnimation = 21;
            item.useTime = 21;
            item.shootSpeed = 9f;
            item.thrown = true;
            item.damage = 33;
            item.autoReuse = true;
            item.knockBack = 2f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 45);
            item.crit = 3;
            item.rare = 4;
        }
    }
}
