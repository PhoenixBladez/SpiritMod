using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Summon
{
    public class GhastStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast Staff");
			Tooltip.SetDefault("Summons an Ethereal Ghast that homes onto the nearest player\nPeriodically shoots out homing wisps at nearby foes\nEnemies that directly hit the Ghast will combust\nOnly one Ghast can be active at a given time");
		}


        public override void SetDefaults()
        {
            item.damage = 60;
            item.summon = true;
            item.mana = 10;
            item.width = 44;
            item.height = 44;
            item.useTime = 31;
            item.useAnimation = 31;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 20000;
            item.rare = 8;
            item.UseSound = SoundID.Item105;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Ghast");
            item.buffType = mod.BuffType("GhastBuff");
            item.shootSpeed = 2f;
            item.buffTime = 3600;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //remove any other owned SpiritBow projectiles, just like any other sentry minion
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == item.shoot && p.owner == player.whoAmI)
                {
                    p.active = false;
                }
            }
            return true;
          
        }
    }
}