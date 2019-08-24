using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Eyeball : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncy Eyeball");
			Tooltip.SetDefault("Bounces around multiple times \n 'Creatively disgusting, or disgustingly creative?'");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 14;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("EyeballProj");
            item.useAnimation = 20;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 20;
            item.shootSpeed = 10f;
            item.damage = 12;
            item.knockBack = 2.7f;
			item.value = Item.sellPrice(0, 0, 0, 30);
            item.value = Item.buyPrice(0, 0, 0, 40);
            item.rare = 1;
            item.autoReuse = false;
            item.maxStack = 999;
            item.consumable = true;
        }

    }
}
