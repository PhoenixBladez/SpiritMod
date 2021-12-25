using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.OlympiumSet.MarkOfZeus
{
	public class MarkOfZeus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark Of Zeus");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage\nConsumes 20 mana per second while charging");
			//  SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Equipment/StarMap_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = ItemRarityID.LightRed;
			item.width = 18;
			item.height = 18;
			item.useTime = 15;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.magic = true;
			item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<MarkOfZeusProj>();
			item.shootSpeed = 0f;
			item.value = Item.sellPrice(0, 2, 0, 0);
		}
	}
	public class MarkOfZeusProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark of Zeus");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.alpha = 0;
			projectile.timeLeft = 999999;
			projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		float counter = 3.15f;
		int manaCounter = 0;
		int trailcounter = 0;
		Vector2 holdOffset = new Vector2(0, -3);
		bool charged = false;
		bool firing = false;
		float growCounter;

		float glowCounter;
		public override bool PreAI()
		{
			growCounter++;
			if (growCounter == 10)
            {
				Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/ElectricCharge").WithPitchVariance(0.4f).WithVolume(0.2f), projectile.Center);
			}
			Player player = Main.player[projectile.owner];
			if (player.statMana <= 0)
				projectile.Kill();
			if (projectile.owner == Main.myPlayer)
				{
					Vector2 direction2 = Main.MouseWorld - (projectile.position);
					direction2.Normalize();
					direction2 *= counter;
					projectile.ai[0] = direction2.X;
					projectile.ai[1] = direction2.Y;
					projectile.netUpdate = true;
				}
			Vector2 direction = new Vector2(projectile.ai[0], projectile.ai[1]);
			if (player.channel && !firing) {
				projectile.position = player.position + holdOffset;
				if (Main.rand.Next(4) == 0)
					Dust.NewDustDirect(projectile.Center + ((projectile.rotation + 1.57f).ToRotationVector2() * Main.rand.Next(-30, 30)), 2, 2, DustID.Firework_Yellow).noGravity = true;

				if (Main.rand.Next(8) == 0)
				{
					int timeLeft = Main.rand.Next(20, 50);
					StarParticle particle = new StarParticle(
					projectile.Center + ((projectile.rotation + 1.57f).ToRotationVector2() * Main.rand.Next(-30, 30)),
					Main.rand.NextVector2Circular(1.5f, 1.5f),
					Color.Gold,
					Main.rand.NextFloat(0.1f, 0.2f),
					timeLeft);
					ParticleHandler.SpawnParticle(particle);
				}

				if (counter < 15) {
					counter += 0.15f;
					manaCounter++;
					if (manaCounter % 7 == 0)
					{
						if (player.statMana > 0)
						{
							player.statMana -= 5;
							player.manaRegenDelay = 60;
							if (player.statMana <= 0)
								Launch(player, direction);
						}
						else
							firing = true;
					}
				}
				else if (!charged)
				{
					charged = true;
					Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
				}
				else
				{
					glowCounter += 0.025f;
				}
				projectile.rotation = direction.ToRotation() - 1.57f;
				if (direction.X > 0) {
					holdOffset.X = -10;
					player.direction = 1;
				}
				else {
					holdOffset.X = 10;
					player.direction = 0;
				}
				trailcounter++;
			}
			else {
				Launch(player, direction);
				projectile.active = false;
			}
			player.heldProj = projectile.whoAmI;
			player.itemTime = 30;
			player.itemAnimation = 30;
			//	player.itemRotation = 0;
			return true;
		}
		int flickerTime = 0;

		private void Launch(Player player, Vector2 direction)
		{
			Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Thunder").WithPitchVariance(0.6f).WithVolume(0.6f), projectile.Center);

			if (projectile.owner == Main.myPlayer)
			{
				int proj = Projectile.NewProjectile(projectile.Center, direction, ModContent.ProjectileType<MarkOfZeusProj2>(), (int)(projectile.damage * Math.Sqrt(counter) * 0.5f), projectile.knockBack, projectile.owner);
				if (Main.projectile[proj].modProjectile is MarkOfZeusProj2 modItem)
				{
					modItem.charge = counter;
					if (Main.netMode != NetmodeID.Server)
					{
						MarkOfZeusPrimTrailTwo trail = new MarkOfZeusPrimTrailTwo(Main.projectile[proj], 2f * (float)(Math.Sqrt(counter) / 5));
						modItem.trail = trail;
						SpiritMod.primitives.CreateTrail(trail);
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float growScale = growCounter > 10 ? 1 : (growCounter / 10f);
			Player player = Main.player[projectile.owner];
			var effects = player.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D tex = Main.projectileTexture[projectile.type];

			lightColor = Color.White;

			if (charged)
			{
				float progress = glowCounter % 1;
				float transparency = (float)Math.Pow(1 - progress, 2);
				float scale = 1 + progress;
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Rectangle(0, 0, tex.Width, tex.Height / 2),
							 lightColor * transparency, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale * scale, effects, 0);
			}

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Rectangle(0,0,tex.Width,tex.Height / 2),
							 lightColor, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale, effects, 0);
			if (counter >= 15 && flickerTime < 16)
			{
				flickerTime++;
				Color color = Color.White;
				float flickerTime2 = (float)(flickerTime / 20f);
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0) {
					alpha = 0;
				}
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Rectangle(0,tex.Height / 2,tex.Width,tex.Height / 2),
							 color * alpha, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 4), projectile.scale * new Vector2(1, 0.75f + counter / 30f) * growScale, effects, 0);
			}
			return false;
		}
	}
	public class MarkOfZeusProj2 : ModProjectile
	{
		public MarkOfZeusPrimTrailTwo trail;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mark Of Zeus");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public float charge;
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 36;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 5;
			projectile.light = 0;
			aiType = ProjectileID.ThrowingKnife;
			projectile.alpha = 255;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.timeLeft = 0;
		}
		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}

		public override void AI()
		{
			trail?.AddPoints();
			if (Main.rand.Next(3) == 0)
			{
				int timeLeft = Main.rand.Next(40, 100);
				StarParticle particle = new StarParticle(
				projectile.Center,
				projectile.velocity.RotatedBy(Main.rand.NextFloat(-1.57f,1.57f)) * 0.3f,
				Color.Gold,
				Main.rand.NextFloat(0.1f, 0.2f),
				timeLeft);
				particle.TimeActive = (uint)(timeLeft / 2);
				ParticleHandler.SpawnParticle(particle);
			}
		}
		public override void Kill(int timeLeft)
		{
			SpiritMod.tremorTime = (int)(charge * 0.66f);
			Main.PlaySound(SoundID.Item70, projectile.Center);
			 for (double i = 0; i < 6.28; i += Main.rand.NextFloat(1f, 2f))
            {
                int lightningproj = Projectile.NewProjectile(projectile.Center + projectile.velocity - (new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 5f), new Vector2((float)Math.Sin(i), (float)Math.Cos(i)) * 2.5f, ModContent.ProjectileType<MarkOfZeusProj3>(), projectile.damage, projectile.knockBack, projectile.owner);
				if (Main.projectile[lightningproj].modProjectile is MarkOfZeusProj3 modProj)
				{
					if (Main.netMode != NetmodeID.Server)
					{
						MarkOfZeusPrimTrail trail = new MarkOfZeusPrimTrail(Main.projectile[lightningproj], 2f * (float)(Math.Sqrt(charge) / 3));
						modProj.trail = trail;
						SpiritMod.primitives.CreateTrail(trail);
					}
				}
				Main.projectile[lightningproj].timeLeft = (int)(30 * Math.Sqrt(charge));
            }
			for (double i = 0; i < 6.28; i+= 0.15)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center + projectile.velocity, 133, new Vector2((float)Math.Sin(i) * Main.rand.NextFloat(3f) * (float)(Math.Sqrt(charge) / 3), (float)Math.Cos(i)) * Main.rand.NextFloat(4f) * (float)(Math.Sqrt(charge) / 3));
				 Dust dust2 = Dust.NewDustPerfect(projectile.Center + projectile.velocity, 133, new Vector2((float)Math.Sin(i) * Main.rand.NextFloat(1.8f) * (float)(Math.Sqrt(charge) / 3), (float)Math.Cos(i)) * Main.rand.NextFloat(2.4f) * (float)(Math.Sqrt(charge) / 3));
                dust.noGravity = true;
				dust2.noGravity = true;
				dust.scale = 0.75f;
				dust2.scale = 0.75f;
            }
			for (int j = 0; j < 13; j++)
			{
				Vector2 vel = Vector2.Zero - projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 0.25f;

				StarParticle particle = new StarParticle(
				projectile.Center + Main.rand.NextVector2Circular(10, 10) + projectile.velocity,
				Main.rand.NextVector2Circular(7, 5),
				Color.Gold,
				Main.rand.NextFloat(0.1f, 0.2f),
				Main.rand.Next(30,60));
				ParticleHandler.SpawnParticle(particle);
			}
		}
	}
	public class MarkOfZeusProj3 : ModProjectile
    {
		public MarkOfZeusPrimTrail trail;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mark of Zeus");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.damage = 0;
            projectile.timeLeft = 300;
            projectile.alpha = 255;
            projectile.extraUpdates = 12;
        }

        Vector2 initialVelocity = Vector2.Zero;

        public Vector2 DrawPos;
        public int boost;
        public override void AI()
        {
			trail?.AddPoints();
            if (initialVelocity == Vector2.Zero)
                initialVelocity = projectile.velocity;
            if (projectile.timeLeft % 10 == 0)
                projectile.velocity = initialVelocity.RotatedBy(Main.rand.NextFloat(-1, 1));
            DrawPos = projectile.position;
        }
    }
}