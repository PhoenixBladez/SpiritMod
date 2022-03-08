using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown.PiecesOfEight
{
	public class PiecesOfEight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pieces of Eight");
			Tooltip.SetDefault("Critical hits cause enemies to drop more coins");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 24;
			item.height = 24;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<EightCoin>();
			item.useAnimation = 41;
			item.useTime = 41;
			item.shootSpeed = 6f;
			item.damage = 34;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 1, 90, 0);
			item.rare = ItemRarityID.LightRed;
			item.autoReuse = true;
			item.consumable = false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 8; i++)
			{
				Vector2 direction = new Vector2(speedX,speedY).RotatedBy(Main.rand.NextFloat(-0.4f,0.4f)) * Main.rand.NextFloat(0.85f,1.25f);
				Projectile.NewProjectile(position, direction, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
	public class EightCoin : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Piece of Eight");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 14;
			projectile.height = 14;
			projectile.ranged = true;
			projectile.penetrate = 1;
		}
		int bounces = 2;
		public override void AI()
		{
			if (Main.rand.Next(10) == 0)
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.CopperCoin, 0, 0).velocity = Vector2.Zero;
			projectile.frameCounter++;
			if (projectile.frameCounter > 6)
			{
				projectile.frame = 1 - projectile.frame; //cheeky
				projectile.frameCounter = 0;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {

			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(0, 1).WithVolume(0.25f), projectile.Center);

			Main.PlaySound(SoundID.Dig, projectile.Center);
			bounces--;
				if (projectile.velocity.X != oldVelocity.X) {
					projectile.velocity.X = -oldVelocity.X * 0.7f;
				}
				if (projectile.velocity.Y != oldVelocity.Y) {
					projectile.velocity.Y = -oldVelocity.Y * 0.7f;
				}
			return bounces <= 0;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (crit && !target.SpawnedFromStatue)
				target.AddBuff(BuffID.Midas, 180);
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.CoinPickup, projectile.Center, 0);
			for (int i = 0; i < 5; i++)
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.CopperCoin).velocity *= 0.4f;
		}
	}
}