using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class BZombieArm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Zombie Arm");
			Tooltip.SetDefault("'As if regular Zombie Arms weren't gross enough'");
		}


        public override void SetDefaults()
        {
            item.damage = 16;            
            item.melee = true;            
            item.width = 44;              
            item.height = 44;
            item.useTime = 45;           
            item.useAnimation = 24;     
            item.useStyle = 1;        
            item.knockBack = 4;      
            item.value = 1200;        
            item.rare = 2;
            item.UseSound = SoundID.Item1;          
            item.autoReuse = true;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.value = Item.sellPrice(0, 0, 55, 0);
            item.useTurn = true;
            item.crit = 8;              
        }
    }
}