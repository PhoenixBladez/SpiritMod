using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpiritMod.Projectiles.Clubs
{
	public class CryoClubSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spike");
			ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 138;
			projectile.height = 96;
			projectile.hide = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
			projectile.alpha = 0;
			projectile.timeLeft = 35;
			projectile.tileCollide = true;
			//	projectile.extraUpdates = 1;
		}
		bool activated = false;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 220, 220, 100);
        }
		public override void Kill (int timeLeft)
        {
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
            for (int i = 0; i < 6; i++)
            {
                Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CryoBomb/CryoShard1"), Main.rand.NextFloat(.4f, 1f));
            }
            for (int i = 0; i < 6; i++)
            {
                Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CryoBomb/CryoShard2"), Main.rand.NextFloat(.4f, 1f));
            }
            for (int i = 0; i < 6; i++)
            {
                Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CryoBomb/CryoShard3"), Main.rand.NextFloat(.4f, 1f));
            }
        }
    
        float counter;
        public override bool PreAI()
        {
            Lighting.AddLight(projectile.Center, .001f, .001f, .001f);
            if (counter > 0)
            {
                counter -= 0.5f;
            }
            projectile.scale -= .0025f;
            projectile.velocity.X = 0;
			if (!activated) {
				projectile.velocity.Y = 24;
			}
			else {
				projectile.ai[1]++;
				//projectile.extraUpdates = 0;
				if (projectile.ai[1] < 5) {
					if (!activated)
                    {
                        projectile.Kill();
                    }
                    projectile.timeLeft = 25;
                    projectile.velocity = Vector2.Zero;
				}
				else if (projectile.ai[1] < 14) {
					projectile.velocity.Y -= .64525f;
				}
				else if (projectile.ai[1] > 14 && projectile.ai[1] < 20)
                {
                    projectile.velocity = Vector2.Zero;
                }
				else {
					projectile.velocity.Y = 1.45f;
				}
				if (projectile.ai[1] == 35) {
					projectile.alpha = 0;
				}
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != projectile.velocity.Y && !activated) {
                activated = true;
				projectile.tileCollide = false;
			}
			return false;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (Main.rand.NextBool(4))
            {
                target.AddBuff(ModContent.BuffType<Buffs.MageFreeze>(), 120);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
	}
}

