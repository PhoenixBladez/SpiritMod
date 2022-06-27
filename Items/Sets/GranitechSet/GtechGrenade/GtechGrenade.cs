using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Sets.GranitechSet.GtechGrenade
{
	public class GtechGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("G-TEK Grenade");
			Tooltip.SetDefault("Slows and electrocutes enemies in it's aura\nCan be destroyed by the player\nDestroying it causes an explosion");
		}

		public override void SetDefaults()
		{
			Item.damage = 70;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GtechGrenadeProj>();
			Item.shootSpeed = 22;
			Item.noUseGraphic = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(10);
			recipe.AddIngredient(ModContent.ItemType<GranitechMaterial>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	public class GtechGrenadeProj : ModProjectile
	{
		private const int ACTIVATION_TIME = 40; //How long the projectile takes to activate, slows down and moves in an arc until then
		private const int DESPAWN_TIME = 10; //How long the projectile takes to shrink before despawning

		private bool DamageAura => Projectile.frame > 4;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gtech Grenade");
			Main.projFrames[Projectile.type] = 15;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.width = Projectile.height = 32;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = true;
		}

		private ref float Timer => ref Projectile.ai[0];

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Timer < ACTIVATION_TIME)
			{
				Timer = ACTIVATION_TIME;
				SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 92).WithPitchVariance(0.2f), Projectile.Center);
				Projectile.velocity = Vector2.Zero;
			}
			return false;
		}

		public override void AI()
		{
			++Timer;

			//Kill self when hit by a friendly projectile
			for (int i = 0; i < Main.maxProjectiles; ++i)
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.friendly && p.DistanceSQ(Projectile.Center) < 20 * 20 && p.owner == Projectile.owner)
				{
					Projectile.Kill();
					p.velocity *= -1;
					ProjectileLoader.OnTileCollide(p, -p.velocity);
					return;
				}
			}

			if (Timer < ACTIVATION_TIME)
			{
				Projectile.velocity *= 0.94f;
				float progress = Timer / ACTIVATION_TIME;

				//Travel in an arc, with reduced gravity over time
				Projectile.velocity.Y += 0.6f * (1 - progress);

				//Ease rotation through multiple circles based on progress, in direction of movement
				int numFullRotations = 3;
				Projectile.rotation = numFullRotations * MathHelper.TwoPi * EaseFunction.EaseQuadOut.Ease(progress) * (Math.Sign(Projectile.velocity.X) > 0 ? 1 : -1);
				Projectile.rotation %= 6.28f;
			}
			else
			{
				if (Timer == ACTIVATION_TIME)
					SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 92).WithPitchVariance(0.2f), Projectile.Center);

				Projectile.velocity = Vector2.Zero;
				Projectile.frameCounter++;
				Projectile.UpdateFrame(12, 7);

				float rotationOffset = DamageAura ? 0 : (float)Math.Sin(Projectile.frameCounter * 0.5f) * MathHelper.Lerp(0.05f, 0.02f, Projectile.frameCounter / 25f);
				float rotDifference = ((((0f - Projectile.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;

				Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.rotation + rotDifference, 0.2f) + rotationOffset;
			}

			if (DamageAura)
			{
				Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3());
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

			if (DamageAura && CheckHit() && Projectile.timeLeft > DESPAWN_TIME)
			{
				Projectile.timeLeft = DESPAWN_TIME;
				/*for (int i = 0; i < 6; i++)
					Dust.NewDustPerfect(projectile.Center, DustID.Electric);*/
			}

			if (Projectile.timeLeft < DESPAWN_TIME)
				Projectile.scale = 1 - (float)Math.Pow(Projectile.timeLeft / (float)DESPAWN_TIME, 0.5f);
		}

		private bool CheckHit()
		{
			foreach (Projectile proj in Main.projectile)
			{
				//Skip projectiles that aren't friendly, active, or of the same type
				if (!proj.active || !proj.friendly || proj.type == Projectile.type || proj == null)
					continue;

				if (proj.Colliding(proj.Hitbox, Projectile.Hitbox))
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
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + (i.ToRotationVector2() * 50 * Projectile.scale), Projectile.width, ref collisionPoint))
					return true;

			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(SoundID.Item, 94).WithPitchVariance(0.2f).WithVolume(.6f), Projectile.Center);
			SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFurySwing, Projectile.Center);
			Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GtechGrenadeExplode>(), Projectile.damage, 0, Projectile.owner);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D aura = ModContent.Request<Texture2D>(Texture + "_Aura");
			if (DamageAura)
				spriteBatch.Draw(aura, Projectile.Center - Main.screenPosition, null, Color.White * 0.3f, Projectile.rotation, new Vector2(aura.Width, aura.Height) / 2, Projectile.scale, SpriteEffects.None, 0f);

			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);

			if (DamageAura)
			{
				float startScale = 1f;
				float endScale = 1.3f;
				tex = ModContent.Request<Texture2D>(Texture + "_Core");
				for (int i = 0; i < 15; i++)
					spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, Color.Lerp(Color.White * 0.5f, Color.White * 0.2f, i / 15f), Projectile.rotation, new Vector2(tex.Width, frameHeight) / 2, Projectile.scale * MathHelper.Lerp(startScale, endScale, i / 15f), SpriteEffects.None, 0f);
			}

			tex = TextureAssets.Projectile[Projectile.type].Value;
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, new Vector2(tex.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0f);

			tex = ModContent.Request<Texture2D>(Texture + "_Glow");
			DrawAberration.DrawChromaticAberration(Vector2.UnitY, 2.5f, delegate (Vector2 offset, Color colorMod)
			{
				spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, frame, Color.White.MultiplyRGBA(colorMod),
					Projectile.rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			});

			return false;
		}
	}

	public class GtechGrenadeExplode : ModProjectile
	{
		private bool damaging => Projectile.frame >= 2 && Projectile.frame < 4;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gtech Grenade");
			Main.projFrames[Projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.width = Projectile.height = 20;
			Projectile.timeLeft = 400;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.Cyan.ToVector3());
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 5 == 0)
			{
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.active = false;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!damaging)
				return false;
			float collisionPoint = 0f;
			for (float i = 0; i < 6.28f; i += 0.392f)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + (i.ToRotationVector2() * 114), Projectile.width, ref collisionPoint))
					return true;
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
			DrawAberration.DrawChromaticAberration(Vector2.UnitY, 2f, delegate (Vector2 offset, Color colorMod)
			{
				spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, frame, Color.White.MultiplyRGBA(colorMod), Projectile.rotation, new Vector2(tex.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0f);
			});
			return false;
		}
	}
}