using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Weapon.Gun.LadyLuck
{
	public class LadyLuck : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lady Luck");
			Tooltip.SetDefault("Right click to shoot coins \nShoot these coins to richochet the bullet\n'Luck favors the rich'");

		}
		public override void SetDefaults()
		{
			item.damage = 36;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item41;
			item.shoot = ModContent.ProjectileType<LadyLuckProj>();
			item.shootSpeed = 12f;
			item.useAmmo = AmmoID.Bullet;
			item.autoReuse = false;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
            {
				type = item.shoot;
				Vector2 direction = new Vector2(speedX, speedY);
				/*direction = (3.14f + (float)Math.Sqrt(Math.Abs(direction.ToRotation() - 1.57f))).ToRotationVector2() * direction.Length();
				direction.X *= -1;*/
				int proj = Projectile.NewProjectile(position, direction, type, 0, knockBack, player.whoAmI);
				return false;
			}
			else
			{
				Gore.NewGore(player.Center, new Vector2(player.direction * -1, -0.5f) * 4, mod.GetGoreSlot("Gores/BulletCasing"), 1f);
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<LLProj>().shotFromGun = true;
			}
			return false;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.shootSpeed = 9f;
				item.useTime = 16;
				item.useAnimation = 16;
				item.UseSound = SoundID.Item1;
				item.noUseGraphic = true;
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useAmmo = 0;
			}
			else
			{
				item.shootSpeed = 16f;
				item.useTime = 20;
				item.useAnimation = 20;
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.noUseGraphic = false;
				item.useAmmo = AmmoID.Bullet;
				item.UseSound = SoundID.Item41;
			}
			return base.CanUseItem(player);
		}
	}
	public class LLProj : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		public bool shotFromGun = false;
	}	
}