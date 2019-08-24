using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Chaparral : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaparral");
		}



        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 92;                            
            item.value = 10000;
            item.rare = 7;
            item.knockBack = 3;
            item.channel = true;
            item.useStyle = 5;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shoot = mod.ProjectileType("ChaparralProjectile");           
        }
    }
}