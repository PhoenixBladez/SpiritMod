using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class PlasmaRod : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Rod");
			Tooltip.SetDefault("Shoots a plasma whip to grapple to nearby enemies");
		}


        public override void SetDefaults()
        {
            item.damage = 1;
            item.magic = true;
            item.mana = 1;
            item.width = 50;
            item.height = 60;
            item.useTime = 1;
            item.useAnimation = 2;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2.5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item21;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PlasmaWhip");
            item.shootSpeed = 9f;
        }
        
    }
}
