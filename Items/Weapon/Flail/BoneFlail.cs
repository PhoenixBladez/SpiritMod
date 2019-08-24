using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
{
    public class BoneFlail : ModItem
    {
		public override void SetStaticDefaults()
		{

			DisplayName.SetDefault("Serpent Spine");
            Tooltip.SetDefault("Retracts on contact with enemies or blocks");

        }


        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = 3;
            item.noMelee = true;
            item.useStyle = 5; 
            item.useAnimation = 50;
            item.autoReuse = true; 
            item.useTime = 50;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.damage = 27;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("BoneFlailHead");
            item.shootSpeed = 14f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true; 
        }
    }
}