using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class ReefSpearThrown : ModProjectile
	{
		public override string Texture => Mod.Name + "/Items/Sets/ReefhunterSet/Projectiles/ReefSpearProjectile";

		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reef Trident");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = 0;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			if (!hasTarget)
			{
				Projectile.velocity.Y += 0.3f;

				Projectile.rotation = Projectile.velocity.ToRotation();
			}
			else
			{
				NPC npc = Main.npc[(int)Projectile.ai[1]];

				if (!npc.active)
				{
					Projectile.netUpdate = true;
					Projectile.tileCollide = true;
					Projectile.timeLeft *= 2;
					Projectile.velocity *= 0;

					hasTarget = false;
					return;
				}

				Projectile.ai[0]++;
				float factor = 1 - (Projectile.ai[0] / 10f);
				if (Projectile.ai[0] >= 10f)
					factor = 0;

				relativePoint += Projectile.velocity * factor * 0.1f;

				Projectile.Center = npc.Center + relativePoint;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.ai[0] = 0;
			Projectile.ai[1] = target.whoAmI;
			Projectile.tileCollide = false;
			Projectile.netUpdate = true;
			Projectile.timeLeft = 240;

			target.AddBuff(BuffID.Poisoned, 300);

			hasTarget = true;
			relativePoint = Projectile.Center - target.Center;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D projTex = TextureAssets.Projectile[Projectile.type].Value;
			const int halfTipWidth = 15;
			Vector2 drawOrigin = new Vector2(Projectile.spriteDirection > 0 ? projTex.Width - halfTipWidth : halfTipWidth, projTex.Height / 2);
			if(!hasTarget)
				Projectile.QuickDrawTrail(Main.spriteBatch, 0.25f, drawOrigin: drawOrigin);

			Projectile.QuickDraw(Main.spriteBatch, drawOrigin: drawOrigin);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			Vector2 goreVel = hasTarget ? Vector2.Zero : Projectile.oldVelocity / 3;
			Vector2 pos = Projectile.Center;
			for (int i = 1; i <= 6; i++)
			{
				if (i >= 4)
					pos -= Vector2.Normalize(Projectile.oldVelocity) * (15 + (i - 3) * 3);

				Gore g = Gore.NewGorePerfect(Projectile.GetSource_Death(), pos, goreVel, Mod.Find<ModGore>("Gores/Projectiles/ReefTrident/Trident" + i).Type);
				g.timeLeft = 0;
				g.rotation = Projectile.rotation;
			}
		}
	}
}
