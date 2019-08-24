using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Slugger : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Slugger");
			Tooltip.SetDefault("Right click to considerably slow the sword, but shoot out a returning boomerang");
		}


        public override void SetDefaults()
        {
            item.damage = 26;            
            item.melee = true;            
            item.width = 36;              
            item.height = 44;
            item.useTime = 22;
            item.useAnimation = 22;     
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.shoot = mod.ProjectileType("Slugger1");
            item.rare = 3;
            item.shootSpeed = 14f;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }


        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 40;
                item.useAnimation = 40;
                item.shoot = mod.ProjectileType("Slugger1");
            }
            else
            {
                item.useTime = 24;
                item.useAnimation = 24;
                item.shoot = 0;
            }
            return base.CanUseItem(player);
        }
    }
}