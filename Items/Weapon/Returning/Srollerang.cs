using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Returning
{
    public class Srollerang : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Srollerang");
			Tooltip.SetDefault("'The explosive spine of a Sroller'");
		}



        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodenBoomerang);
            item.damage = 140;
            item.value = Terraria.Item.sellPrice(0, 15, 0, 0);
            item.rare = 9;
            item.shootSpeed = 14;
            item.knockBack = 2;
            item.height = 46;
            item.width = 46;
            item.useStyle = 5;
            item.useAnimation = 26;
            item.useTime = 26;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SrollerangProjectile");

        }
    }
}