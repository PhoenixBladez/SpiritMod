using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class OakHeart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oak Heart");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 16;

			Projectile.aiStyle = 1;
			Projectile.aiStyle = 113;

			Projectile.friendly = true;

			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.spriteDirection = Main.rand.NextBool() ? 1 : -1;
			AIType = ProjectileID.BoneJavelin;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.netUpdate = true;
		}

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + ((Projectile.spriteDirection == 1) ? 1.57f : 0);
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(6)) {
				for (int k = 0; k < 5; k++) {
					int p = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center.X + Main.rand.Next(-20, 20), target.position.Y - 60, 0f, 8f, ModContent.ProjectileType<PoisonCloud>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
					Main.projectile[p].penetrate = 2;

				}
			}
			MyPlayer mp = Main.player[Projectile.owner].GetSpiritPlayer();
			if (mp.sacredVine && Main.rand.NextBool(2))
				target.AddBuff(ModContent.BuffType<PollinationPoison>(), 200, true);

			else
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Poisoned, 200, true);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GrassBlades);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
		    Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
		    for (int k = 0; k < Projectile.oldPos.Length; k++)
		    {
		        Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
		        Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
		    }
		    return true;
		}
	}
}