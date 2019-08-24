using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
	public class GaleJavelin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ocean's Gale Javelin");
			Tooltip.SetDefault("Left-click to thrust at foes, Right-click to throw at enemies");
		}


		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.useStyle = 5;
			item.width = 24;
			item.height = 24;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.noMelee = true;
			item.useAnimation = 40;
			item.useTime = 40;
			item.shootSpeed = 6f;
			item.knockBack = 4f;
			item.damage = 25;
			item.value = Item.sellPrice(0, 2, 60, 0);
			item.rare = 3;
			item.shoot = mod.ProjectileType("GaleJavelinProj1");
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				item.melee = false;
				item.thrown = true;
				item.autoReuse = true;

				player.itemTime -= 10;
				player.itemAnimation -= 10;
				player.itemAnimationMax -= 10;
				Projectile.NewProjectile(position.X, position.Y, speedX * 2.5f, speedY * 2.5f, mod.ProjectileType("GaleJavelinProj2"), damage, knockBack, player.whoAmI, 0f, 0f);
				return false;
			}
			else
			{
				item.melee = true;
				item.thrown = false;
				item.autoReuse = false;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("GaleJavelinProj1"), damage, knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}
	}
}