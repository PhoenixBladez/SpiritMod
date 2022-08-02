using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
    public class MysticWizardBall : ModProjectile, IDrawAdditive
    {
		public bool Small = false;
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
			Projectile.timeLeft = 90;
        }
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			Projectile.Kill();
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f*2, 0.231f*2, 0.255f*2);
            if (Projectile.timeLeft < 85 && Projectile.timeLeft > 15)
            {
                Projectile.velocity.Y += .09f;
            }
			if (Projectile.timeLeft < 15)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.scale += .07f;
            }
			if (Projectile.timeLeft % 10 == 0)
            {
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * new Vector2(5f, 3f);
  
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Main.rand.Next(-50, 50), Projectile.Center.Y + Main.rand.Next(-50, 50), vector2_2.X, vector2_2.Y, ModContent.ProjectileType<MysticWizardBallEnergyEffect>(), 0, 0.0f, Main.myPlayer, 0.0f, (float)Projectile.whoAmI);
                Main.projectile[p].scale = Main.rand.NextFloat(.4f, 1.4f);

            }
        }
        public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            {
                for (int k = 0; k < 1; k++)
                {
                    Color color = Color.White * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                    float scale = Projectile.scale;
                    Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/Boss/MoonWizardTwo/Projectiles/MysticWizardBall_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                    spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
                    //spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - screenPos, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
		public override void Kill(int timeLeft)
        {
			float speed = 3;
            for (int k = 0; k < 18; k++)
            {

                Dust d = Dust.NewDustPerfect(Projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
                d.noGravity = true;
            }
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 3.75f * speed, 3.75f * speed, ModContent.ProjectileType<MysticWizardBall_Projectile>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -3.75f * speed, -3.75f * speed, ModContent.ProjectileType<MysticWizardBall_Projectile>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 7.5f * speed, 0f, ModContent.ProjectileType<MysticWizardBall_Projectile>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -7.5f * speed, 0f, ModContent.ProjectileType<MysticWizardBall_Projectile>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, 7.5f * speed, ModContent.ProjectileType<MysticWizardBall_Projectile>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -7.5f * speed, ModContent.ProjectileType<MysticWizardBall_Projectile>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

        }
        public override void PostDraw(Color lightColor)
        {
            Color color1 = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            int r1 = color1.R;
            drawOrigin.Y += 34f;
            drawOrigin.Y += 8f;
            --drawOrigin.X;
            Vector2 position1 = Projectile.Center - Main.screenPosition;
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
            drawOrigin = r2.Size() / 2f;
            Vector2 position3 = position1 + new Vector2(6, 5);
            Color color3 = new Color(84, 207, 255) * 1.6f;
            Main.spriteBatch.Draw(texture2D2, position3, r2, color3, Projectile.rotation, drawOrigin, Projectile.scale * .73f, SpriteEffects.FlipHorizontally, 0.0f);
            float num15 = 1f + num11 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num12, Projectile.rotation, drawOrigin, Projectile.scale * .73f * num15, SpriteEffects.FlipHorizontally, 0.0f);
            float num16 = 1f + num13 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num14, Projectile.rotation, drawOrigin, Projectile.scale * .73f * num16, SpriteEffects.FlipHorizontally, 0.0f);
        }
    }
}
