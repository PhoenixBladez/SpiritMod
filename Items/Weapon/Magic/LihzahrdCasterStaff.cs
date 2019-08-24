using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class LihzahrdCasterStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Suncaller Staff");
			Tooltip.SetDefault("'Used in ancient sacrifices'\nCalls for a spread of solar energy to explode when hitting foes");
		}


        public override void SetDefaults()
        {
            item.damage = 64;
            item.magic = true;
            item.mana = 9;
            item.width = 34;
            item.height = 34;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 9;
            item.value = Terraria.Item.sellPrice(0, 2, 50, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item69;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SolarBlastFriendly");
            item.shootSpeed = 11f;
        }

        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0874532925);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = randomSpread(speedX, speedY, 8, 3);
            for (int i = 0; i < 3; ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}