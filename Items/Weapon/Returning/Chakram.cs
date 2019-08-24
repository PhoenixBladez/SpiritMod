using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Returning
{
	public class Chakram : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chakram");
		}


		public override void SetDefaults()
		{
            item.damage = 17;            
            item.melee = true;
            item.width = 30;
            item.height = 28;
			item.useTime = 24;
			item.useAnimation = 24;
            item.noUseGraphic = true;
            item.useStyle = 1;
			item.knockBack = 3;
            item.value = Terraria.Item.buyPrice(0, 1, 70, 0);
            item.rare = 2;
			item.shootSpeed = 14f;
			item.shoot = mod.ProjectileType ("Chakram");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}
        public override bool CanUseItem(Player player)       //this make that you can shoot only 1 boomerang at once
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
    }
}