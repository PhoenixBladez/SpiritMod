using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
{
    public class Clauncher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clauncher");
            Tooltip.SetDefault("Occasionally shoots out multiple Shell Bolts");

        }


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.rare = 6;
            item.noMelee = true;
            item.useStyle = 5; 
            item.useAnimation = 34; 
            item.useTime = 34;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 1, 43, 0);
            item.damage = 71;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("ClauncherHead");
            item.shootSpeed = 28f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true; 
        }
    }
}