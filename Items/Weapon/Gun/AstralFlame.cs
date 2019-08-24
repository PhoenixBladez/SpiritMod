using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Gun
{
	public class AstralFlame : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pleiadean Flare");
			Tooltip.SetDefault("Turns Gel into Astral Fire\nHas a 25% chance not to consume ammo");
		}



		public override void SetDefaults()
		{
			item.damage = 45;
			item.noMelee = true;
			item.ranged = true;
			item.width = 58;
			item.height = 20;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.shoot = 3;
			item.useAmmo = 23;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.shootSpeed = 9f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int projectileFired = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("AstralFlareRanged"), item.damage, item.knockBack, player.whoAmI);
			Main.projectile[projectileFired].friendly = true;
			Main.projectile[projectileFired].friendly = true;
			Main.projectile[projectileFired].hostile = false;
			return false;
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (Main.rand.Next(4) == 0)
				return false;

			return true;
		}

	}
}
