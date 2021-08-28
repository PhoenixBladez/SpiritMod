using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Sets.SepulchreLoot.OldCross
{
	public class CrossCoffin : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Coffin");
			Main.projFrames[projectile.type] = 3;
		}
		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 60;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.friendly = true;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.hide = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override bool CanDamage() => false;

		int rotationtimer = 0;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(rotationtimer);
		public override void ReceiveExtraAI(BinaryReader reader) => rotationtimer = reader.ReadInt32();
		public override void AI()
		{
			if (projectile.velocity.Y != 0)
			{
				rotationtimer++;//solution to it jittering on slopes
				if (rotationtimer > 1)
				{
					if (Math.Abs(projectile.rotation) < 0.3f)
						projectile.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
					else
						projectile.rotation /= 2;
				}
				projectile.netUpdate = true;
			}
			else
			{
				rotationtimer = 0;
				projectile.rotation = 0;
			}

			if (projectile.velocity.Y < 15)
				projectile.velocity.Y += 0.2f;

			if(projectile.ai[0] == -1)
			{
				MakeDust();
				projectile.ai[0]++;
			}

			NPC target = null;
			NPC miniontarget = projectile.OwnerMinionAttackTargetNPC;
			float maxdist = 900f;
			if(miniontarget != null && projectile.Distance(miniontarget.Center) < maxdist && miniontarget.CanBeChasedBy(projectile))
			{
				target = miniontarget;
			}
			else for(int i = 0; i < Main.npc.Length; i++)
			{
				NPC potentialtarget = Main.npc[i];
				if(potentialtarget != null && projectile.Distance(potentialtarget.Center) < maxdist && potentialtarget.CanBeChasedBy(projectile))
				{
					maxdist = projectile.Distance(potentialtarget.Center);
					target = potentialtarget;
				}
			}
			if(target != null)
			{
				projectile.ai[0]++;
				if(projectile.ai[0] > 30)
				{
					projectile.frameCounter++;
					if(projectile.frameCounter >= 10)
					{
						projectile.frameCounter = 0;
						if (projectile.frame < 2 && projectile.ai[0] < 120)
							projectile.frame++;
						if (projectile.frame > 0 && projectile.ai[0] > 120)
							projectile.frame--;
					}
					if(projectile.ai[0] == 90)
					{
						MakeDust();
						int skeletstospawn = Main.rand.Next(1, 3);
						projectile.velocity.Y -= 3;
						for(int i = 0; i <= skeletstospawn; i++)
                        {
							Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, mod.ProjectileType("CrossSkelet"), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, target.whoAmI);
							proj.position.X += Main.rand.Next(-20, 21);
							proj.damage = (int)(proj.damage * (0.5f / skeletstospawn + 0.5f));

                        }

						for (int i = 0; i <= 12 + skeletstospawn; i++)
						{
							Gore gore = Gore.NewGoreDirect(projectile.position + new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height)), 
								Main.rand.NextVector2Circular(-3, 3), 
								mod.GetGoreSlot("Gores/Skelet/bonger" + Main.rand.Next(1, 5)));
							gore.timeLeft = 40;
						}
					}
					if (projectile.ai[0] > 160)
						projectile.ai[0] = 0;
				}
			}
			else
			{
				projectile.ai[0] = 0;
				projectile.ai[1] = 0;
				projectile.frame = 0;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) => drawCacheProjsBehindNPCs.Add(index);

		public override void Kill(int timeLeft) => MakeDust();

		public void MakeDust()
		{
			for(int i = 0; i < 40; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Poisoned, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-0.25f, 0.5f));
				dust.scale *= 0.66f;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle drawframe = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			SpriteEffects flip = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(tex,
					projectile.Center - Main.screenPosition + (Vector2.UnitX * projectile.spriteDirection * 10),
					drawframe,
					projectile.GetAlpha(Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16)),
					projectile.rotation,
					drawframe.Size() / 2,
					projectile.scale,
					flip,
					0);

			return false;
		}
	}
}