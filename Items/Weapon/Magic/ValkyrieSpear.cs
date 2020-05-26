using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ValkyrieSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Spirit Spear");
			Tooltip.SetDefault("Deals both magic and melee damage");
		}


		public override void SetDefaults()
		{
			item.damage = 16;
			item.magic = true;
			item.mana = 11;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 2;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("ValkyrieSpearHostile");
			item.shootSpeed = 11.5f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            for (int I = 0; I < 2; I++)
			{
                float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
                {
                    position += spawnPlace;
                }

                Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
                int p = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, 0, 0.0f, 0.0f);
                Main.projectile[p].friendly = true;
                Main.projectile[p].hostile = false;
                Main.projectile[p].melee = true;
                Main.projectile[p].magic = true;
                for (float num2 = 0.0f; (double)num2 < 10; ++num2)
                {
                    int dustIndex = Dust.NewDust(position, 2, 2, 263, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
                }
            }

			return false;
		}
	}
}
