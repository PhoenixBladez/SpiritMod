using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
    public class MysticWizardBallSmall : ModProjectile, IDrawAdditive
    {
		public bool Small = false;
		readonly float gravity = 0.3f;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wizard's Moonjelly Ball");
        }
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.penetrate = -1; 
			Projectile.friendly = false; 
            Projectile.hostile = true; 
            Projectile.aiStyle = -1; 
			Projectile.scale = 0.6f;
			Projectile.timeLeft = 900;
        }
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			Projectile.Kill();
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f*2, 0.231f*2, 0.255f*2);
			Projectile.velocity.Y += gravity;
			if (Projectile.timeLeft < 15)
			{
				Projectile.scale += .06f;
			}
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list)
				if (proj.active && proj.friendly && !proj.hostile && Projectile.timeLeft > 2)
					Projectile.timeLeft = 2;
			for (int i = 0; i < Main.player.Length; i++)
			{
				Player player = Main.player[i];
				if ((player.Center - Projectile.Center).Length() < 300 && Projectile.timeLeft > 15)
					Projectile.timeLeft = 15;
			}
		}
		public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			for (int k = 0; k < 1; k++)
			{
				Color color = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				float scale = Projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/Boss/MoonWizardTwo/Projectiles/MysticWizardBall_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				spriteBatch.Draw(tex, Projectile.oldPos[k] + tex.Size() / 2 - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
				//spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
			}
		}
		public override void Kill(int timeLeft)
        {
			float speed = Small ? 1.25f : 3;
            for (int k = 0; k < 18; k++)
            {

                Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
                d.noGravity = true;
            }
           for (float i = 0; i < 6.28; i+= (1.57f / 2f))
			{
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, i.ToRotationVector2() * 5, ModContent.ProjectileType<MysticWizardBallSmall_Projectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}

        }
		public override void PostDraw(Color lightColor)
		{
			Color color1 = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
			Vector2 drawOrigin = new Vector2(TextureAssets.GlowMask[239].Value.Width * 0.5f, TextureAssets.GlowMask[239].Value.Height * 0.5f);
			int r1 = color1.R;
			Vector2 position1 = Projectile.Center - Main.screenPosition;
			position1 += new Vector2(10, 12);
			Texture2D texture2D2 = TextureAssets.GlowMask[239].Value;
			float num11 = (float)(Main.GlobalTimeWrappedHourly % 1.0 / 1.0);
			float num12 = num11;
			if (num12 > 0.5)
				num12 = 1f - num11;
			if (num12 < 0.0)
				num12 = 0.0f;
			float num13 = (float)((num11 + 0.5) % 1.0);
			float num14 = num13;
			if (num14 > 0.5)
				num14 = 1f - num13;
			if (num14 < 0.0)
				num14 = 0.0f;
			Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
			Color color3 = new Color(84, 207, 255) * 1.6f;
			Main.spriteBatch.Draw(texture2D2, position1, r2, color3, Projectile.rotation, drawOrigin, Projectile.scale * .73f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num15 = 1f + num11 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position1, r2, color3 * num12, Projectile.rotation, drawOrigin, Projectile.scale * .73f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num16 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position1, r2, color3 * num14, Projectile.rotation, drawOrigin, Projectile.scale * .73f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
		}
	}
}
