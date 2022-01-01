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
			item.reuseDelay = 40;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<GtechGrenadeProj>();
			item.shootSpeed = 15;
			item.noUseGraphic = true;
			item.maxStack = 999;
			item.consumable = true;
		}
	}

	public class GtechGrenadeProj : ModProjectile
	{
		private bool activated;

		private bool damageAura => projectile.frame > 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gtech Grenade");
			Main.projFrames[projectile.type] = 15;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.width = projectile.height = 32;
			projectile.timeLeft = 400;
		}
		public override void AI()
		{
			projectile.velocity *= 0.96f;
			if (projectile.velocity.Length() < 1 && !activated)
				activated = true;
			if (activated)
			{
				projectile.velocity = Vector2.Zero;
				projectile.frameCounter++;
				if (projectile.frameCounter % 5 == 0)
				{
					projectile.frame++;
					if (projectile.frame >= Main.projFrames[projectile.type])
						projectile.frame = 7;
				}
			}
			if (projectile.velocity.Length() < 2)
			{
				float rotationOffset = damageAura ? 0 : (float)Math.Sin(projectile.frameCounter * 0.5f) * MathHelper.Lerp(0.05f, 0.02f, projectile.frameCounter / 25f);
				projectile.rotation = MathHelper.Lerp(projectile.rotation, 6.28f, 0.2f) + rotationOffset;
			}
			else
			{
				projectile.rotation += projectile.velocity.Length() * 0.04f;
				projectile.rotation %= 6.28f;
			}
			if (damageAura)
			{
				Lighting.AddLight(projectile.Center, Color.Cyan.ToVector3());
				foreach (NPC npc in Main.npc)
				{
					if (npc.townNPC || !npc.active || npc.immortal)
						continue;
					if (InAura(npc.Hitbox))
					{
						npc.AddBuff(ModContent.BuffType<ElectrifiedV2>(), 10);
						if (!npc.boss)
							npc.AddBuff(ModContent.BuffType<MageFreeze>(), 10);
					}
				}
			}

			if (damageAura && CheckHit() && projectile.timeLeft > 18)
			{
				projectile.timeLeft = 18;
				for (int i = 0; i < 6; i++)
					Dust.NewDustPerfect(projectile.Center, DustID.Electric);
			}

			if (projectile.timeLeft < 18)
				projectile.scale = Math.Max((projectile.timeLeft - 5) / 13f, 0);
		}

		private bool CheckHit()
		{
			foreach (Projectile proj in Main.projectile)
			{
				if (!proj.active || !proj.friendly || proj == projectile || proj.type == projectile.type)
					continue;
				if (proj.modProjectile == null)
				{
					if (proj.Hitbox.Intersects(projectile.Hitbox))
						return true;
				}
				else
				{
					bool? colliding = proj.modProjectile.Colliding(proj.Hitbox, projectile.Hitbox);
					if (colliding == null)
					{
						if (proj.Hitbox.Intersects(projectile.Hitbox))
							return true;
					}
					else
						return (bool)colliding;
				}
			}

			return false;
		}

		private bool InAura(Rectangle targetHitbox)
		{
			if (!damageAura)
				return false;
			float collisionPoint = 0f;
			for (float i = 0; i < 6.28f; i += 0.78f)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + (i.ToRotationVector2() * 50 * projectile.scale), projectile.width, ref collisionPoint))
					return true;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<GtechGrenadeExplode>(), projectile.damage, 0, projectile.owner);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D aura = ModContent.GetTexture(Texture + "_Aura");
			if (damageAura)
				spriteBatch.Draw(aura, projectile.Center - Main.screenPosition, null, Color.White * 0.3f, projectile.rotation, new Vector2(aura.Width, aura.Height) / 2, projectile.scale, SpriteEffects.None, 0f);

			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

			if (damageAura)
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
			DrawAberration.DrawChromaticAberration(Vector2.UnitY, 2f, delegate (Vector2 offset, Color colorMod)
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