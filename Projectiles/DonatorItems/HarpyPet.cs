  
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Utilities;
using SpiritMod.Buffs.Pet;
using SpiritMod.Items.DonatorItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Projectiles.DonatorItems
{
	class HarpyPet : ModProjectile
	{
		private const float FOV = (float)System.Math.PI / 2;
		private const float Max_Range = 16 * 50;
		private const float Spread = (float)System.Math.PI / 9;
		private const int Damage = 15;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Harpy");
			Main.projFrames[projectile.type] = 3;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 26;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft *= 5;
			aiType = ProjectileID.BabyHornet;
		}

		private float Timer
		{
			get { return projectile.localAI[1]; }
			set { projectile.localAI[1] = value; }
		}

		private int animationCounter;
		private int frame;
		public override void AI()
		{
			if (++animationCounter >= 5)
			{
				animationCounter = 0;
				if (++frame >= Main.projFrames[projectile.type])
					frame = 0;
			}
			projectile.frameCounter = 0;
			projectile.frame = frame;

			var owner = Main.player[projectile.owner];
			if (owner.active && owner.HasBuff(ModContent.BuffType<HarpyPetBuff>()))
				projectile.timeLeft = 2;

			if (projectile.owner != Main.myPlayer)
				return;

			if (Timer > 0)
			{
				--Timer;
				return;
			}
			
			float direction;
			if (projectile.direction < 0)
				direction = FOVHelper.POS_X_DIR + projectile.rotation;
			else
				direction = FOVHelper.NEG_X_DIR - projectile.rotation;

			var origin = projectile.Center;
			var fov = new FOVHelper();
			fov.AdjustCone(origin, FOV, direction);
			float maxDistSquared = Max_Range * Max_Range;
			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];
				Vector2 npcPos = npc.Center;
				if (npc.CanBeChasedBy() &&
					fov.IsInCone(npcPos) &&
					Vector2.DistanceSquared(origin, npcPos) < maxDistSquared &&
					Collision.CanHitLine(origin, 0, 0, npc.position, npc.width, npc.height))
				{
                    if (Main.rand.NextBool(10))
                    {
					    ShootFeathersAt(npcPos);
                    }
					Timer = 140;
					break;
				}
			}
		}

		private void ShootFeathersAt(Vector2 target)
		{
			var origin = projectile.Center;
			var direction = target - origin;
			direction = direction.SafeNormalize(Vector2.UnitX);
			direction *= 6f;
			Projectile.NewProjectile(origin, direction, ModContent.ProjectileType<HarpyBolt>(), Damage * 2, 0, projectile.owner);
        
        }
	}
    public class HarpyBolt : ModProjectile, IDrawAdditive
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Faerie Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
            projectile.width = 12;
            projectile.height = 16;
            projectile.hostile = false;
            projectile.scale = .75f;
			projectile.friendly = true;
            projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 70;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreAI()
		{
            projectile.velocity *= 1.01f;
            float num = 1f - (float)projectile.alpha / 255f;
            num *= projectile.scale;
            Lighting.AddLight(projectile.Center, 0.255f * num, 0.184f * num, 0.229f * num);
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            if (projectile.timeLeft < 55)
            {
                projectile.tileCollide = true;
            }
            return true;
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 24; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.DungeonSpirit, 0f, 0f, 0, default, 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
            if (projectile.minion)
            {
                ProjectileExtras.Explode(projectile.whoAmI, 60, 60, delegate
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 3));
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.DungeonSpirit, 0f, -2f, 0, default, 2f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].scale *= .25f;
                            if (Main.dust[num].position != projectile.Center)
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                }, true);
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
                Color color = new Color(74, 255, 186) * 0.95f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Color color1 = new Color(255, 209, 244) * 0.95f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = projectile.scale;
                Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/DonatorItems/HarpyBolt_Glow");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color1 * .5f, projectile.rotation, tex.Size() / 2, scale * 2.3f, default, default);
            }
        }
    }
}