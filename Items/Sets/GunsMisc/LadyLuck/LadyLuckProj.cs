using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.GunsMisc.LadyLuck
{
	public class LadyLuckProj : ModProjectile
	{
        int cooldown = -1;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Coin");
			Main.projFrames[projectile.type] = 2;
		}

        public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 14;
			projectile.damage = 0;
			projectile.height = 14;
			projectile.ranged = false;
			projectile.penetrate = 5;
		}

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 6)
            {
                projectile.frame = 1 - projectile.frame; //cheeky
                projectile.frameCounter = 0;
            }
            if (Main.rand.Next(10) == 0)
                Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GoldCoin, 0, 0).velocity = Vector2.Zero;
            Lighting.AddLight(projectile.Center, Color.Gold.R * 0.007f, Color.Gold.G * 0.007f, Color.Gold.B * 0.007f);
            cooldown--;
            Rectangle Hitbox = new Rectangle((int)projectile.Center.X - 30, (int)projectile.Center.Y - 30, 60, 60);
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(Hitbox));
            foreach (var proj in list)
            {
                if (proj.ranged && proj.active && proj.friendly && !proj.hostile && proj.GetGlobalProjectile<LLProj>().shotFromGun && cooldown < 0)
                {
                    for (int i = 0; i < 5; i++)
                        Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GoldCoin).velocity *= 0.4f;
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/coinhit"), projectile.Center);
                    projectile.velocity = proj.velocity / 2;
                    float attackRange = 800;
                    NPC target = Main.npc.Where(n => n.CanBeChasedBy() && Vector2.Distance(n.Center, projectile.Center) < attackRange).OrderBy(n => n.life / n.lifeMax).FirstOrDefault();
                    if (target != default)
                    {
                        Vector2 direction = target.Center - proj.Center;
                        direction.Normalize();
						float velocity = proj.velocity.Length();

						direction *= velocity;
                        proj.velocity = direction;
                        proj.damage = (int)(proj.damage * (5.75f - projectile.penetrate));
						SpiritMod.primitives.CreateTrail(new LLPrimTrail(proj, Color.Gold));

						proj.GetGlobalProjectile<LLProj>().hit = true;
						proj.GetGlobalProjectile<LLProj>().target = target;
						proj.GetGlobalProjectile<LLProj>().initialVel = velocity;

						proj.GetGlobalProjectile<LLProj>().shotFromGun = false;
                    }
                    else
                        proj.velocity = proj.velocity.RotatedBy(Main.rand.NextFloat(6.28f));
                    projectile.penetrate--;
                    if (projectile.penetrate == 0)
                        for (int i = 1; i < 3; ++i)
                            Gore.NewGore(projectile.Center, Vector2.Zero, mod.GetGoreSlot("Gores/CoinGores/CoinHalf" + i), 1f);
                    cooldown = 5;
                }
            }

            if (Math.Abs(projectile.velocity.Y) < 2f)
                projectile.velocity.Y *= 0.98f;
            projectile.velocity *= .996f;
        }

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.CoinPickup, projectile.Center, 0);
			Main.PlaySound(SoundID.Dig, projectile.Center, 0);
			for (int i = 0; i < 5; i++)
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.GoldCoin).velocity *= 0.4f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
		    Vector2 drawPos = projectile.Center - Main.screenPosition;
		    Color color = new Color(Color.Gold.R, Color.Gold.G, Color.Gold.B, 0);
			Texture2D tex = mod.GetTexture("Items/Sets/GunsMisc/LadyLuck/CoinBloom");
		    spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, projectile.scale, SpriteEffects.None, 0f);
		    return true;
		}
	}
}