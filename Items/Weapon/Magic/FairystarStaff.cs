using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class FairystarStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fairystar Staff");
            Tooltip.SetDefault("Shoots out multiple low velocity bolts of Fae energy\nFae energy occasionally inflicts 'Holy Light'\n'Holy Light' reduces enemy defense");

        }


        public override void SetDefaults()
        {
            item.damage = 50;
            item.magic = true;
            item.mana = 12;
            item.width = 46;
            item.height = 46;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item34;
			item.crit = 2;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Fae");
            item.shootSpeed = 17f;
            item.autoReuse = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 4; i++)
            {
                float spread = 40f * 0.0174f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                speedX = baseSpeed * (float)Math.Sin(randomAngle);
                speedY = baseSpeed * (float)Math.Cos(randomAngle);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Fae"), item.damage, knockBack, item.owner, 0, 0);
            }
            return false;
        }
    }
}
