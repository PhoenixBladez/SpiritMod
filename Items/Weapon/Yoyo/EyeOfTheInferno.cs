using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class EyeOfTheInferno : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye Of The Inferno");
			Tooltip.SetDefault("Hit foes combust, with successful hits increasing the power of the debuff. \nAlso shoots out a spiky ball that inflicts broken armor");
		}



        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 42;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
            item.knockBack = 2.9f;
            item.channel = true;
            item.useStyle = 5;
            item.useAnimation = 26;
            item.useTime = 26;
            item.shoot = mod.ProjectileType("EyeOfTheInfernoProj");           
        }
    }
}