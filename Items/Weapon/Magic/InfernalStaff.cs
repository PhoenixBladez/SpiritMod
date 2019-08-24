using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
    public class InfernalStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seal of Torment");
			Tooltip.SetDefault("Shoots three exploding, homing, fiery souls\n3 second cooldown");
		}


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.rare = 5;
            item.mana = 12;
            item.damage = 55;
            item.knockBack = 5F;
            item.useStyle = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 50, 0);
            item.useTime = 24;
            item.useAnimation = 24;
            item.magic = true;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("FireSoul");
            item.shootSpeed = 12f;
        }

        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0574532925);
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
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
            modPlayer.shootDelay = 180;
            Vector2[] speeds = randomSpread(speedX, speedY, 8, 3);
            for (int i = 0; i < 2; ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
            if (modPlayer.shootDelay == 0)
                return true;
            return false;
        }

    }
}