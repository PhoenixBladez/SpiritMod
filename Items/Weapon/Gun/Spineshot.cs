using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class Spineshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spineshot");
			Tooltip.SetDefault("Shoots a burst of two projectiles\nAllows the collection of seeds for ammo");
		}
		public override bool CloneNewInstances => true;


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = true;
			item.useAnimation = 50;
			item.useTime = 50;
			item.width = 38;
			item.height = 6;
			item.shoot = ProjectileID.PurificationPowder;
			item.rare = ItemRarityID.Blue;
			item.useAmmo = AmmoID.Dart;
			item.UseSound = SoundID.Item63;
			item.damage = 11;
			item.shootSpeed = 6.5f;
			item.noMelee = true;
			item.value = 10000;
			item.knockBack = 3.5f;
			item.ranged = true;
		}
		public override bool UseItemFrame(Player player)
		{
			player.bodyFrame.Y = player.bodyFrame.Height * 2;
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f;
			if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			for(int X = 0; X <= 1; X++) {
				float spread = MathHelper.ToRadians(22f); //45 degrees converted to radians
				float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
				double baseAngle = Math.Atan2(speedX, speedY);
				double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				speedX = baseSpeed * (float)Math.Sin(randomAngle);
				speedY = baseSpeed * (float)Math.Cos(randomAngle);
				int proj2 = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
