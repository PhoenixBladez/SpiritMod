using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SpiritMod.Dusts;
using SpiritMod.Items.Material;
using SpiritMod.Buffs;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Sets.GranitechSet.GtechGrenade
{
	public class GtechGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("G-TEK Grenade");
			Tooltip.SetDefault("Slows and electrocutes enemies in it's aura \nCan be destroyed by the player \nDestroying it causes an explosion");
		}

		public override void SetDefaults()
		{
			item.damage = 70;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<GtechGrenadeProj>();
			item.shootSpeed = 22;
			item.noUseGraphic = true;
			item.maxStack = 999;
			item.consumable = true;
		}
	}

	public class GtechGrenadeProj : ModProjectile
	{
		private const int ACTIVATION_TIME = 40; //How long the projectile takes to activate, slows down and moves in an arc until then
		private const int DESPAWN_TIME = 10; //How long the projectile takes to shrink before despawning

		private bool DamageAura => projectile.frame > 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gtech Grenade");
			Main.projFrames[projectile.type] = 15;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.width = projectile.height = 32;
			projectile.timeLeft = 300;
			projectile.tileCollide = true;
		}

		private ref float Timer => ref projectile.ai[0];

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Timer < ACTIVATION_TIME)
			{
				Timer = ACTIVATION_TIME;
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 92).WithPitchVariance(0.2f), projectile.Center);
				projectile.velocity = Vector2.Zero;
			}
			return false;
		}
		public override void AI()
		{
			++Timer;

			if(Timer < ACTIVATION_TIME)
			{
				projectile.velocity *= 0.94f;
				float progress = Timer / ACTIVATION_TIME;

				//Travel in an arc, with reduced gravity over time
				projectile.velocity.Y += 0.6f * (1 - progress);

				//Ease rotation through multiple circles based on progress, in direction of movement
				int numFullRotations = 3;
				projectile.rotation = numFullRotations * MathHelper.TwoPi * EaseFunction.EaseQuadOut.Ease(progress) * (Math.Sign(projectile.velocity.X) > 0 ? 1 : -1);
				projectile.rotation %= 6.28f;
			}

			else
			{
				if(Timer == ACTIVATION_TIME)
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 92).WithPitchVariance(0.2f), projectile.Center);

				projectile.velocity = Vector2.Zero;
				projectile.frameCounter++;
				projectile.UpdateFrame(12, 7);

				float rotationOffset = DamageAura ? 0 : (float)Math.Sin(projectile.frameCounter * 0.5f) * MathHelper.Lerp(0.05f, 0.02f, projectile.frameCounter / 25f);

				float rotDifference = ((((0f - projectile.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;

				projectile.rotation = MathHelper.Lerp(projectile.rotation, projectile.rotation + rotDifference, 0.2f) + rotationOffset;
			}

			if (DamageAura)
			{
				Lighting.AddLight(projectile.Center, Color.Cyan.ToVector3());
				foreach (NPC npc in Main.npc)
				{
					//Ignore if the npc shouldn't be able to be hit
					if (npc.townNPC || !npc.active || npc.immortal || npc.dontTakeDamage)
						continue;

					if (InAura(npc.Hitbox))
					{
						npc.AddBuff(ModContent.BuffType<ElectrifiedV2>(), 10);
						if (!npc.boss)
							npc.AddBuff(ModContent.BuffType<MageFreeze>(), 10);
					}
				}
			}
			else

			if (DamageAura && CheckHit() && projectile.timeLeft > DESPAWN_TIME)
			{
				projectile.timeLeft = DESPAWN_TIME;
				/*for (int i = 0; i < 6; i++)
					Dust.NewDustPerfect(projectile.Center, DustID.Electric);*/
			}

			if (projectile.timeLeft < DESPAWN_TIME)
				projectile.scale = 1 - (float)Math.Pow(projectile.timeLeft / (float)DESPAWN_TIME, 0.5f);
		}

		private bool CheckHit()
		{
			foreach (Projectile proj in Main.projectile)
			{
				//Skip projectiles that aren't friendly, active, or of the same type
				if (!proj.active || !proj.friendly || proj.type == projectile.type || proj == null)
					continue;

				if (proj.Colliding(proj.Hitbox, projectile.Hitbox))
					return true;
			}

			return false;
		}

		private bool InAura(Rectangle targetHitbox)
		{
			if (!DamageAura)
				return false;

			float collisionPoint = 0f;
			for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.PiOver4)
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + (i.ToRotationVector2() * 50 * projectile.scale), projectile.width, ref collisionPoint))
					return true;

			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 94).WithPitchVariance(0.2f).WithVolume(.6f), projectile.Center);
			Main.PlaySound(SoundID.DD2_SkyDragonsFurySwing, projectile.Center);
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<GtechGrenadeExplode>(), projectile.damage, 0, projectile.owner);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D aura = ModContent.GetTexture(Texture + "_Aura");
			if (DamageAura)
				spriteBatch.Draw(aura, projectile.Center - Main.screenPosition, null, Color.White * 0.3f, projectile.rotation, new Vector2(aura.Width, aura.Height) / 2, projectile.scale, SpriteEffects.None, 0f);

			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

			if (DamageAura)
			{
				float startScale = 1f;
				float endScale = 1.3f;
				tex = ModContent.GetTexture(Texture + "_Core");
				for (int i = 0; i < 15; i++)
					spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, Color.Lerp(Color.White * 0.5f, Color.White * 0.2f, i / 15f), projectile.rotation, new Vector2(tex.Width, frameHeight) / 2, projectile.scale * MathHelper.Lerp(startScale, endScale, i / 15f), SpriteEffects.None, 0f);
			}

			tex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, lightColor, projectile.rotation, new Vector2(tex.Width, frameHeight) / 2, projectile.scale, SpriteEffects.None, 0f);

			tex = ModContent.GetTexture(Texture + "_Glow");
			DrawAberration.DrawChromaticAberration(Vector2.UnitY, 2.5f, delegate (Vector2 offset, Color colorMod)
			{
				spriteBatch.Draw(tex, projectile.Center + offset - Main.screenPosition, frame, Color.White.MultiplyRGBA(colorMod),
					projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, SpriteEffects.None, 0);
			});
	
			return false;
		}
	}

	public class GtechGrenadeExplode : ModProjectile
	{
		private bool damaging => projectile.frame >= 2 && projectile.frame < 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gtech Grenade");
			Main.projFrames[projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.width = projectile.height = 20;
			projectile.timeLeft = 400;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.Cyan.ToVector3());
			projectile.frameCounter++;
			if (projectile.frameCounter % 5 == 0)
			{
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.active = false;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!damaging)
				return false;
			float collisionPoint = 0f;
			for (float i = 0; i < 6.28f; i += 0.392f)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + (i.ToRotationVector2() * 114), projectile.width, ref collisionPoint))
					return true;
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			DrawAberration.DrawChromaticAberration(Vector2.UnitY, 2f, delegate (Vector2 offset, Color colorMod)
			{
				spriteBatch.Draw(tex, projectile.Center + offset - Main.screenPosition, frame, Color.White.MultiplyRGBA(colorMod), projectile.rotation, new Vector2(tex.Width, frameHeight) / 2, projectile.scale, SpriteEffects.None, 0f);
			});
			return false;
		}
	}
}