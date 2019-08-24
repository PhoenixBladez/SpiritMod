using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class SickleBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Sickle");
            Tooltip.SetDefault("Shoots out a rapidly moving Soul Sickle at foes");

        }


        public override void SetDefaults()
        {
            item.damage = 62;            
            item.melee = true;            
            item.width = 34;              
            item.height = 40;
            item.autoReuse = true;
            item.useTime = 27;           
            item.useAnimation = 27;     
            item.useStyle = 1;        
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 1, 40, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;          
            item.shoot = mod.ProjectileType("SpiritSickle");
            item.shootSpeed = 9f;                               
        }

    }
}