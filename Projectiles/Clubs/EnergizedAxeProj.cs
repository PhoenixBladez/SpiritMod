using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Clubs
{
	class EnergizedAxeProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Adze");
			Main.projFrames[projectile.type] = 3;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 100; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.BoneDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 9f);
			}
            for (int k = 0; k <= 30; k++)
            {
                Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), 226, new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 9f);
            }
        }
		public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int size = 60;
			if (projectile.ai[0] >= ChargeTime) {
				
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, TrueRotation, Origin, projectile.scale, Effects, 1);
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] >= ChargeTime)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                {
                    for (int i = 0; i < 20; i++)
                    {
                        int num = Dust.NewDust(target.position, target.width, target.height, DustID.Electric, 0f, -2f, 0, default, 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .25f;
                        if (Main.dust[num].position != target.Center)
                            Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
                int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y,
                    0, 0, mod.ProjectileType("GraniteSpike1"), projectile.damage, projectile.knockBack/2, Main.myPlayer, 0f, 0f);
                Main.projectile[proj].timeLeft = 2;
            }
        }
       public EnergizedAxeProj() : base(40, 23, 53, -1, 60, 6, 10, 4f, 19f){}
	}
}
