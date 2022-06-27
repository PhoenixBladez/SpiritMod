using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Sets.GunsMisc.LadyLuck
{
	public class LadyLuckProj : ModProjectile
	{
        int cooldown = -1;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Coin");
			Main.projFrames[Projectile.type] = 2;
		}

        public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 14;
			Projectile.damage = 0;
			Projectile.height = 14;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
		}

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame = 1 - Projectile.frame; //cheeky
                Projectile.frameCounter = 0;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0).velocity = Vector2.Zero;

            Lighting.AddLight(Projectile.Center, Color.Gold.R * 0.0007f, Color.Gold.G * 0.0007f, Color.Gold.B * 0.0007f);
            cooldown--;
            Rectangle Hitbox = new Rectangle((int)Projectile.Center.X - 30, (int)Projectile.Center.Y - 30, 60, 60);
            var list = Main.projectile.Where(x => x.active && x.Hitbox.Intersects(Hitbox) && x.friendly && !x.hostile);

            foreach (var proj in list)
            {
                if (proj.IsRanged() && proj.GetGlobalProjectile<LLProj>().shotFromGun && cooldown < 0)
                {
                    for (int i = 0; i < 5; i++)
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin).velocity *= 0.4f;

					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Ricochet") with { PitchVariance = 0.4f, Volume = 0.6f}, Projectile.Center);
					Projectile.velocity = proj.velocity / 2;
                    float attackRange = 800;
                    NPC target = Main.npc.Where(n => n.CanBeChasedBy() && Vector2.Distance(n.Center, Projectile.Center) < attackRange).OrderBy(n => n.life / n.lifeMax).FirstOrDefault();

                    if (target != default)
                    {
                        Vector2 direction = Vector2.Normalize(target.Center - proj.Center);
						float velocity = proj.velocity.Length();

						direction *= velocity;
                        proj.velocity = direction;
                        proj.damage = (int)(proj.damage * 3f);
						SpiritMod.primitives.CreateTrail(new LLPrimTrail(proj, Color.Gold));

						proj.GetGlobalProjectile<LLProj>().hit = true;
						proj.GetGlobalProjectile<LLProj>().target = target;
						proj.GetGlobalProjectile<LLProj>().initialVel = velocity;
						proj.GetGlobalProjectile<LLProj>().shotFromGun = false;

						proj.netUpdate = true;
                    }
                    else
                        proj.velocity = proj.velocity.RotatedBy(Main.rand.NextFloat(6.28f));

                    Projectile.penetrate--;

                    if (Projectile.penetrate == 0)
                        for (int i = 1; i < 3; ++i)
                            Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, Mod.Find<ModGore>("Gores/CoinGores/CoinHalf" + i).Type, 1f);
                    cooldown = 5;
                }
            }

            if (Math.Abs(Projectile.velocity.Y) < 2f)
                Projectile.velocity.Y *= 0.98f;
            Projectile.velocity *= .996f;
			Projectile.velocity.Y -= 0.1f;
        }

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < 5; i++)
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin).velocity *= 0.4f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
		    Vector2 drawPos = Projectile.Center - Main.screenPosition;
		    Color color = new Color(Color.Gold.R, Color.Gold.G, Color.Gold.B, 0);
			Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Sets/GunsMisc/LadyLuck/CoinBloom").Value;
		    Main.spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale, SpriteEffects.None, 0f);
		    return true;
		}
	}
}