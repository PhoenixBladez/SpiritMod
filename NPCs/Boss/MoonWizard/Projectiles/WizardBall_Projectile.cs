using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class WizardBall_Projectile : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 70;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.aiStyle = 1;
            Projectile.scale = 1.5f;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreAI()
        {
            float num = 1f - (float)Projectile.alpha / 255f;
            num *= Projectile.scale;
            Lighting.AddLight(Projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Projectile.velocity = Projectile.velocity.RotatedBy(System.Math.PI / 40);

            int num623 = Dust.NewDust(Projectile.Center, 4, 4, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.8f);
            Main.dust[num623].velocity = Projectile.velocity;
            Main.dust[num623].noGravity = true;

            return true;
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 24; num257++) {
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
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

        public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = new Color(255, 255, 255) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                float scale = Projectile.scale;
                Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/Boss/MoonWizard/Projectiles/WizardBall_Projectile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
            }
        }
    }
}
