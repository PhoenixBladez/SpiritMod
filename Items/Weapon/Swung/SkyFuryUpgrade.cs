using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class SkyFuryUpgrade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Demon's Wrath");
			Tooltip.SetDefault("Right click to thrust as a spear, releasing multiple dark flames\nInflicts Shadowflame and Shadow Curse, which reduces enemy defense");
		}


        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.useStyle = 100;
            item.width = 58;
            item.height = 58;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
			item.channel = true;
            item.noMelee = true;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 6f;
            item.knockBack = 6f;
            item.damage = 102;
            item.value = Item.sellPrice(0, 25, 60, 0);
            item.rare = 10;
            item.shoot = mod.ProjectileType("SkyFuryProjectile1");
        }
		public override bool UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            return true;
		
        }
    }
}