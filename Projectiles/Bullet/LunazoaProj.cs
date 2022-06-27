using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class LunazoaProj : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
            Projectile.width = 8;
            Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 360;
			//projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.aiStyle = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		Vector2 initialvel = Vector2.Zero;
		int flipdirection;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(initialvel);
			writer.Write(flipdirection);
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			initialvel = reader.ReadVector2();
			flipdirection = reader.ReadInt32();
			Projectile.localAI[0] = reader.ReadSingle();
			Projectile.localAI[1] = reader.ReadSingle();
		}
		public override void AI()
        {
            float num = 1f - Projectile.alpha / 255f;
            num *= Projectile.scale;
            Lighting.AddLight(Projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if(Projectile.localAI[1] == 0) {
				initialvel = Projectile.velocity;
				Projectile.localAI[1]++;
				flipdirection = Main.rand.NextBool() ? -1 : 1;
				Projectile.netUpdate = true;
			}
			if (initialvel.Length() < 22)
				initialvel *= 1.02f;

			Projectile.localAI[0] += flipdirection;
			Projectile.tileCollide = (Math.Abs(Projectile.localAI[0]) >= 20);
			switch(Projectile.ai[0]) {
				case 0: Projectile.velocity = initialvel;
					int num623 = Dust.NewDust(Projectile.Center, 4, 4, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
					Main.dust[num623].velocity = Projectile.velocity;
					Main.dust[num623].noGravity = true;
					break;
				case 1: Projectile.velocity = initialvel.RotatedBy(Math.Cos(Projectile.localAI[0] / 6) * MathHelper.Pi / 5);
					break;
				case 2: Projectile.velocity = initialvel.RotatedBy(MathHelper.ToRadians(Projectile.localAI[0] * 12))/2 + initialvel/2;
					break;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item122 with { PitchVariance = 0.2f, Volume = 0.5f }, Projectile.Center);
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = (Projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 12; num257++) {
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

        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Color color = new Color(255, 255, 255) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                float scale = Projectile.scale;
                Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/Boss/MoonWizard/Projectiles/WizardBall_Projectile").Value;

                spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.oldRot[k], tex.Size() / 2, scale, default, default);
            }
        }
    }
}
