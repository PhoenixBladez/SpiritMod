using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Mountain : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Mountain");
			Tooltip.SetDefault("'Swinging the blade strengthens you \n Occasionally inflicts foes with 'Unstable Affliction''");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 88;
            item.melee = true;
            item.width = 54;
            item.height = 58;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = Terraria.Item.sellPrice(0, 8, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PrismaticBolt");
            item.shootSpeed = 12;
        }
        public override bool UseItem(Player player)
        {
            {
                player.AddBuff(5, 300);
                return false;

            }
            return false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                charger++;
                if (charger >= 7)
                {
                    for (int I = 0; I < 4; I++)
                    {
                        Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 300), speedY + ((float)Main.rand.Next(-230, 230) / 300), mod.ProjectileType("AtlasBolt"), 50, knockBack, player.whoAmI, 0f, 0f);
                    }
                    charger = 0;
                }
                return true;
            }

            if (Main.rand.Next(12) == 0)
            {
                item.shoot = mod.ProjectileType("PrismBolt2");
                return true;
            }
            else
            {
                item.shoot = mod.ProjectileType("PrismaticBolt");
                return true;
            }
            return true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            {
                target.AddBuff(mod.BuffType("Afflicted"), 180);
            }
        }

    }
}