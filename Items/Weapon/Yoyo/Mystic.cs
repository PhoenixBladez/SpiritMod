using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Mystic : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic");
			Tooltip.SetDefault("A one-of-a-kind yo-yo that uses magic!");
		}



        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 41;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 3;
            item.knockBack = 2;
            item.channel = true;
            item.useStyle = 5;
            item.useAnimation = 25;
            item.useTime = 27;
            item.shoot = mod.ProjectileType("MysticProjectile"); 
			item.magic = true;
			item.mana = 4;   
			item.melee = false;     
        }
		    }
}