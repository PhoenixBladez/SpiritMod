using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Coconut : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut");
			Tooltip.SetDefault("Does more damage if dropped from high up");
		}

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("CoconutP");
            item.useAnimation = 45;
            item.useTime = 45;
            item.shootSpeed = 2f;
            item.damage = 11;
            item.knockBack = 3.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
            item.crit = 8;
            item.rare = 2;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
