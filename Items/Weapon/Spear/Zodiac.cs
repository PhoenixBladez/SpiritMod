using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Spear
{
    public class Zodiac : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zodiac");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 1, 30, 0);
            item.rare = 12;
            item.damage = 125;
            item.knockBack = 4f;
            item.useStyle = 5;
            item.useTime = 20;
            item.useAnimation = 20;
            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("ZodiacProj2");
            item.shootSpeed = 5f;
            item.UseSound = SoundID.Item1;
        }
    }
}