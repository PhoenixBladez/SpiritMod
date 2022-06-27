using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SpiritMod.NPCs.ScreechOwl
{
	public class ScreechOwlNote : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cacophonous Note");
        }

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 18;
			Projectile.height = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 60;
			Projectile.alpha = 75;
            Projectile.extraUpdates = 2;
            Projectile.hide = true;
		}
		public override void AI()
        {
            Projectile.ai[1]++;
			Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[1] == 15)
            {
                Projectile.height += 10;
                for (int j = 0; j < 30; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(16f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default) * 1.3f;
                    int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit, 0f, 0f, 0, new Color(), .6f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = Projectile.Center + vector2;
                    Main.dust[num8].velocity = Projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = 1.46f;
                }
            }
            if (Projectile.ai[1] == 30)
            {
                Projectile.height += 10;
                for (int j = 0; j < 40; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(20f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default) * 1.3f;
                    int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit, 0f, 0f, 0, new Color(), .7f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = Projectile.Center + vector2;
                    Main.dust[num8].velocity = Projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = 1.36f;
                }
            }
            if (Projectile.ai[1] == 45)
            {
                Projectile.height += 10;
                for (int j = 0; j < 50; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(24f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default) * 1.3f;
                    int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit, 0f, 0f, 0, new Color(), .8f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = Projectile.Center + vector2;
                    Main.dust[num8].velocity = Projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = 1.16f;
                }
            }
            if (Projectile.ai[1] == 60)
            {
                Projectile.height += 10;
                for (int j = 0; j < 60; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(30f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default) * 1.3f;
                    int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.DungeonSpirit, 0f, 0f, 0, new Color(), 1.1f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = Projectile.Center + vector2;
                    Main.dust[num8].velocity = Projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = .96f;
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(BuffID.Confused, 120);
            }
        }
    }
}
