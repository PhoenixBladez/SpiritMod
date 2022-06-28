using Terraria.Audio;
using Terraria.ID;
using SpiritMod.Items.Material;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using SpiritMod.Items.Sets.CascadeSet;

namespace SpiritMod.Items.Sets.CascadeSet.BubbleMine
{
	public class BubbleMine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Mine");
			Tooltip.SetDefault("Right-click to make bubble mines detonate");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Shuriken);
			Item.width = 37;
			Item.height = 26;
			Item.shoot = ModContent.ProjectileType<BubbleMineProj>();
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 11f;
			Item.damage = 18;
			Item.knockBack = 1.0f;
			Item.value = Terraria.Item.sellPrice(0, 0, 0, 5);
			Item.crit = 8;
			Item.rare = ItemRarityID.Blue;
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(70);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
	public class BubbleMineProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bubble Mine");

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.StickyGrenade);
			AIType = ProjectileID.StickyGrenade;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.width = 20;
			Projectile.height = 30;
			Projectile.penetrate = 2;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (float i = 0; i <= 6.28f; i+= Main.rand.NextFloat(0.5f,2))
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, i.ToRotationVector2() * Main.rand.NextFloat(), ModContent.ProjectileType<BubbleMineBubble>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);

			for (int i = 0; i < 8; i++)
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith, Scale: Main.rand.NextFloat(1f, 1.5f)).noGravity = true;
		}


		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 20;
			Projectile.Kill();
		}

		public override void AI()
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.Kill();
			}
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.196f, 0.870588235f, 0.964705882f);
			Projectile.localAI[0] += 1f;
			if (Main.mouseRight && Main.myPlayer == Projectile.owner)
			{
				Projectile.ai[1] = 7201;
			}
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419) {
						num416++;
					}

					if (num416 > 5) {
						Projectile.netUpdate = true;
						Projectile.Kill();
						return;
					}
				}
			}
		}
	}
	public class BubbleMineBubble : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
			Projectile.timeLeft = 200;
			Projectile.alpha = 110;
			Projectile.extraUpdates = 1;
			Projectile.scale = Main.rand.NextFloat(0.7f, 1.3f);
		}

		public override void AI()
		{
			Projectile.velocity.X *= 0.9925f;
			Projectile.velocity.Y -= 0.012f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.Kill();
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .3f;
				if (Main.dust[num].position != Projectile.Center)
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 7f;
			}
		}
	}
}
