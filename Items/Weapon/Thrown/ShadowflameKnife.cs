using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class ShadowflameKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowburn Knife");
			Tooltip.SetDefault("Hit foes are afflicted by 'Shadowflame'");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 30;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("ShadowflameKnife");
            item.useAnimation = 23;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 23;
            item.shootSpeed = 14.5f;
            item.damage = 32;
            item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 0, 3, 0);
           	item.value = Item.buyPrice(0, 0, 30, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
