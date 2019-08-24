using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class ScarabSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Blade");
			Tooltip.SetDefault("Shoots a cluster of sand and dust on swing");
		}


        public override void SetDefaults()
        {
            item.damage = 14;            
            item.melee = true;            
            item.width = 50;
            item.autoReuse = true;           
            item.height = 50;             
            item.useTime = 32;           
            item.useAnimation = 32;     
            item.useStyle = 1;        
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item1;          
            item.shoot = mod.ProjectileType("ScarabProjectile");
            item.shootSpeed = 7; ;            
            item.crit = 8;                     
        }
    }
}