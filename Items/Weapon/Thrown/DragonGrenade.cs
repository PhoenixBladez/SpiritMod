using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class DragonGrenade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breath of the Sky Dragon");
			Tooltip.SetDefault("'Harness the breath of the Dragon'");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Grenade);
            item.shoot = mod.ProjectileType("DragonGrenade");
            item.useAnimation = 34;
            item.rare = 8;
            item.useTime = 34;
            item.knockBack = 7f;
            item.maxStack = 999;
            item.damage = 90;
            item.shootSpeed = 10f;
            item.autoReuse = true;
			item.value = 1900;
        }

    }
}