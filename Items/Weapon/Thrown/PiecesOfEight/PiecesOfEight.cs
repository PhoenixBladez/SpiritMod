using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 24;
			Item.height = 24;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<EightCoin>();
			Item.useAnimation = 41;
			Item.useTime = 41;
			Item.shootSpeed = 6f;
			Item.damage = 34;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 1, 90, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.autoReuse = true;
			Item.consumable = false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int i = 0; i < 8; i++)
			{
				Vector2 direction = new Vector2(speedX,speedY).RotatedBy(Main.rand.NextFloat(-0.4f,0.4f)) * Main.rand.NextFloat(0.85f,1.25f);
				Projectile.NewProjectile(position, direction, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
	public class EightCoin : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Piece of Eight");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
		}
		int bounces = 2;
		public override void AI()
		{
			if (Main.rand.Next(10) == 0)
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0, 0).velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 6)
			{
				Projectile.frame = 1 - Projectile.frame; //cheeky
				Projectile.frameCounter = 0;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {

			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(0, 1).WithVolume(0.25f), Projectile.Center);

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			bounces--;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X * 0.7f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y * 0.7f;
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
			SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.Center, 0);
			for (int i = 0; i < 5; i++)
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin).velocity *= 0.4f;
		}
	}
}