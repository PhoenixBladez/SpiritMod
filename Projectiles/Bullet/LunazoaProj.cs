using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class LunazoaProj : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
            projectile.width = 8;
            projectile.height = 8;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 4;
			projectile.timeLeft = 360;
			//projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.aiStyle = -1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}
		Vector2 initialvel = Vector2.Zero;
		int flipdirection;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(initialvel);
			writer.Write(flipdirection);
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			initialvel = reader.ReadVector2();
			flipdirection = reader.ReadInt32();
			projectile.localAI[0] = reader.ReadSingle();
			projectile.localAI[1] = reader.ReadSingle();
		}
		public override void AI()
        {
            float num = 1f - projectile.alpha / 255f;
            num *= projectile.scale;
            Lighting.AddLight(projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if(projectile.localAI[1] == 0) {
				initialvel = projectile.velocity;
				projectile.localAI[1]++;
				flipdirection = Main.rand.NextBool() ? -1 : 1;
				projectile.netUpdate = true;
			}
			if (initialvel.Length() < 22)
				initialvel *= 1.02f;

			projectile.localAI[0] += flipdirection;
			projectile.tileCollide = (Math.Abs(projectile.localAI[0]) >= 20);
			switch(projectile.ai[0]) {
				case 0: projectile.velocity = initialvel;
					int num623 = Dust.NewDust(projectile.Center, 4, 4, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
					Main.dust[num623].velocity = projectile.velocity;
					Main.dust[num623].noGravity = true;
					break;
				case 1: projectile.velocity = initialvel.RotatedBy(Math.Cos(projectile.localAI[0] / 6) * MathHelper.Pi / 5);
					break;
				case 2: projectile.velocity = initialvel.RotatedBy(MathHelper.ToRadians(projectile.localAI[0] * 12))/2 + initialvel/2;
					break;
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(SoundID.Item, 122).WithPitchVariance(0.2f).WithVolume(0.5f), projectile.Center);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 12; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.DungeonSpirit, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = new Color(255, 255, 255) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = projectile.scale;
                Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Boss/MoonWizard/Projectiles/WizardBall_Projectile");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.oldRot[k], tex.Size() / 2, scale, default, default);
            }
        }
    }
}
