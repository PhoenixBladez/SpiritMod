
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.Linq;
using SpiritMod.Utilities;

namespace SpiritMod.Projectiles.Bullet
{
	public class ConfluxPellet : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scattergun Pellet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 6;
			Projectile.width = 6;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}

		public bool bounce = false;
        int numBounce = 3;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 mouse = Main.MouseWorld;
			if (bounce)
            {
                Projectile.velocity.Y += 0.1F;
                Projectile.velocity.X *= 1.005F;
                Projectile.ai[0] += .0205f;
                for (int i = 0; i < 4; i++)
                {
                    int num = Dust.NewDust(Projectile.position, 6, 6, DustID.WitherLightning, 0f, 0f, 0, default, .65f);
                    Main.dust[num].position = Projectile.Center - Projectile.velocity / num * (float)i;

                    //Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(18, Main.LocalPlayer);
                    Main.dust[num].velocity = Projectile.velocity;
                    Main.dust[num].scale = MathHelper.Clamp(Projectile.ai[0], .015f, 1.25f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = (float)(100 + Projectile.owner);

                }
                if (Main.rand.NextBool(16))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, 6, 6, DustID.WitherLightning, 0f, 0f, 0, default, .65f);
                        Main.dust[num].position = Projectile.Center - Projectile.velocity / num * (float)i;
    
                        //Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(18, Main.LocalPlayer);
                        Main.dust[num].velocity *= Projectile.velocity/2;
                        Main.dust[num].scale = MathHelper.Clamp(Projectile.ai[0], .015f, Projectile.ai[0] / 3);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].fadeIn = (float)(100 + Projectile.owner);
                    }
                }
            }
			else
            {
                for (int i = 0; i < 3; i++)
                {
                    float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                    float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
                    int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.WitherLightning);

                    Main.dust[num].position = Projectile.Center - Projectile.velocity / num * (float)i;
                    Main.dust[num].alpha = Projectile.alpha;
                    Main.dust[num].velocity = Projectile.velocity;
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = 0.4684f;
                    Main.dust[num].scale *= .1235f;
                }

            }
        }
        public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
				Color color = new Color(240, 199, 255) * 0.65f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                float scale = Projectile.scale;
                Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Bullet/ConfluxPellet", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.position);

		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (float numBounce = 0.0f; (double)numBounce < 10; ++numBounce)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, 2, 2, DustID.WitherLightning, 0f, 0f, 0, default, .56f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity = Vector2.Normalize(Projectile.Center.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
            }
            bounce = true;
            Projectile.ai[0] = .1f;
            numBounce--;
            if (numBounce <= 0)
                Projectile.Kill();
            else
            {
				float maxdist = 500; //the maximum distance in pixels to check for an npc
				var targets = Main.npc.Where(x => x.CanBeChasedBy(this) && x != null && x.active && Projectile.Distance(x.Center) < maxdist && Collision.CanHit(Projectile.Center, 0, 0, x.Center, 0, 0)); //look through each npc, and find the ones that meet the given requirements
				if (targets.Any())
				{ //only run this code if there are any npcs that fulfill the above requirements
					NPC finaltarget = null;
					foreach (NPC npc in targets)
					{ //loop through each npc that meets the above requirements, and find the closest by decreasing the maximum distance each time a closer npc is found
						if (Projectile.Distance(npc.Center) <= maxdist)
						{
							maxdist = Projectile.Distance(npc.Center);
							finaltarget = npc;
						}
					}
					if (finaltarget != null) //only run this code if the final target isn't null, to prevent any possible null reference errors
						Projectile.velocity = Projectile.GetArcVel(finaltarget.Center, 0.1f, maxXvel: 8);
				}
				else 
					Projectile.Bounce(oldVelocity, 0.75f);
            }
            return false;
        }
    }
}
