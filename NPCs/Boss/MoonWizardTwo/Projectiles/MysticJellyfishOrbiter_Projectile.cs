using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticJellyfishOrbiter_Projectile : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
            Projectile.width = 12;
            Projectile.height = 16;
            Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 70;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.aiStyle = -1;
            Projectile.scale = 1.5f;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreAI()
		{
            Projectile.velocity *= 1.01f;
            float num = 1f - (float)Projectile.alpha / 255f;
            num *= Projectile.scale;
            Lighting.AddLight(Projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;

            int num623 = Dust.NewDust(Projectile.Center - Projectile.velocity / 5, 4, 4, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.8f);
            Main.dust[num623].velocity = Projectile.velocity;
            Main.dust[num623].noGravity = true;
			if (Projectile.timeLeft < 55)
                Projectile.tileCollide = true;

            Player player = Main.player[Projectile.owner];
            Vector2 center = Projectile.Center;
            float num8 = (float)player.miscCounter / 40f;
            float num7 = 1.0471975512f * 2;
            if (Projectile.minion)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int num6 = Dust.NewDust(center, 0, 0, DustID.DungeonSpirit, 0f, 0f, 100, default, .85f);
                        Main.dust[num6].noGravity = true;
                        Main.dust[num6].velocity = Vector2.Zero;
                        Main.dust[num6].noLight = true;
                        Main.dust[num6].position = center - Projectile.velocity / 5 * j + (num8 * 6.28318548f + num7 * i).ToRotationVector2() * 9f;
                    }
                }
            }
            return true;
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 24; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			if (Projectile.minion)
			{
				ProjectileExtras.Explode(Projectile.whoAmI, 60, 60, delegate
				{
					SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 3));
					for (int i = 0; i < 10; i++)
					{
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
					DustHelper.DrawDustImage(Projectile.Center, 226, 0.15f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
				}, true);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = new Color(255, 255, 255) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                float scale = Projectile.scale;
                Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/Boss/MoonWizardTwo/Projectiles/WizardBall_Projectile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
            }
        }
    }
}
