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
			item.CloneDefaults(ItemID.Shuriken);
			item.width = 37;
			item.height = 26;
			item.shoot = ModContent.ProjectileType<BubbleMineProj>();
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 11f;
			item.damage = 18;
			item.knockBack = 1.0f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 5);
			item.crit = 8;
			item.rare = ItemRarityID.Blue;
			item.ranged = true;
			item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 70);
			recipe.AddRecipe();
		}
	}
	public class BubbleMineProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bubble Mine");

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.StickyGrenade);
			aiType = ProjectileID.StickyGrenade;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.thrown = false;
			projectile.timeLeft = 600;
			projectile.width = 20;
			projectile.height = 30;
			projectile.penetrate = 2;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			for (float i = 0; i <= 6.28f; i+= Main.rand.NextFloat(0.5f,2))
				Projectile.NewProjectile(projectile.Center, i.ToRotationVector2() * Main.rand.NextFloat(), ModContent.ProjectileType<BubbleMineBubble>(), (int)(projectile.damage * 0.5f), projectile.knockBack, projectile.owner);

			for (int i = 0; i < 8; i++)
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Wraith, Scale: Main.rand.NextFloat(1f, 1.5f)).noGravity = true;
		}


		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 20;
			projectile.Kill();
		}

		public override void AI()
		{
			projectile.ai[1]++;
			if (projectile.ai[1] >= 7200f) {
				projectile.Kill();
			}
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.196f, 0.870588235f, 0.964705882f);
			projectile.localAI[0] += 1f;
			if (Main.mouseRight && Main.myPlayer == projectile.owner)
			{
				projectile.ai[1] = 7201;
			}
			if (projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419) {
						num416++;
					}

					if (num416 > 5) {
						projectile.netUpdate = true;
						projectile.Kill();
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
			projectile.aiStyle = -1;
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 2;
			projectile.timeLeft = 200;
			projectile.alpha = 110;
			projectile.extraUpdates = 1;
			projectile.scale = Main.rand.NextFloat(0.7f, 1.3f);
		}

		public override void AI()
		{
			
			projectile.velocity.X *= 0.9925f;
			projectile.velocity.Y -= 0.012f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => projectile.Kill();
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FungiHit, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .3f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 7f;
			}
		}
	}
}
