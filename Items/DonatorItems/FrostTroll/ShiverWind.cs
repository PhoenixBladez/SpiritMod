using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.FrostTroll
{
    public class ShiverWind : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shiver Wind");
			Tooltip.SetDefault("Shoots a chilly bolt that morphs into an icy rune!");
		}


        public override void SetDefaults()
        {
            item.damage = 60;
            item.magic = true;
            item.mana = 14;
            item.width = 52;
            item.height = 52;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = 80000;
            item.rare = 5;
            item.UseSound = SoundID.Item34;
			item.crit = 11;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ChillBolt");
            item.shootSpeed = 7f;
            item.autoReuse = true;
        }
    }
}
