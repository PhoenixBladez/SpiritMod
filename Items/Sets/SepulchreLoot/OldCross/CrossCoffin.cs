using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Sets.SepulchreLoot.OldCross
{
	public class CrossCoffin : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Coffin");
			Main.projFrames[Projectile.type] = 3;
		}
		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 60;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.hide = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		int rotationtimer = 0;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(rotationtimer);
		public override void ReceiveExtraAI(BinaryReader reader) => rotationtimer = reader.ReadInt32();
		public override void AI()
		{
			if (Projectile.velocity.Y != 0)
			{
				rotationtimer++;//solution to it jittering on slopes
				if (rotationtimer > 1)
				{
					if (Math.Abs(Projectile.rotation) < 0.3f)
						Projectile.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
					else
						Projectile.rotation /= 2;
				}
				Projectile.netUpdate = true;
			}
			else
			{
				rotationtimer = 0;
				Projectile.rotation = 0;
			}

			if (Projectile.velocity.Y < 15)
				Projectile.velocity.Y += 0.2f;

			if(Projectile.ai[0] == -1)
			{
				MakeDust();
				Projectile.ai[0]++;
			}

			NPC target = null;
			NPC miniontarget = Projectile.OwnerMinionAttackTargetNPC;
			float maxdist = 900f;
			if(miniontarget != null && Projectile.Distance(miniontarget.Center) < maxdist && miniontarget.CanBeChasedBy(Projectile))
			{
				target = miniontarget;
			}
			else for(int i = 0; i < Main.npc.Length; i++)
			{
				NPC potentialtarget = Main.npc[i];
				if(potentialtarget != null && Projectile.Distance(potentialtarget.Center) < maxdist && potentialtarget.CanBeChasedBy(Projectile))
				{
					maxdist = Projectile.Distance(potentialtarget.Center);
					target = potentialtarget;
				}
			}
			if(target != null)
			{
				Projectile.ai[0]++;
				if(Projectile.ai[0] > 30)
				{
					Projectile.frameCounter++;
					if(Projectile.frameCounter >= 10)
					{
						Projectile.frameCounter = 0;
						if (Projectile.frame < 2 && Projectile.ai[0] < 120)
							Projectile.frame++;
						if (Projectile.frame > 0 && Projectile.ai[0] > 120)
							Projectile.frame--;
					}
					if(Projectile.ai[0] == 90)
					{
						MakeDust();
						int skeletstospawn = Main.rand.Next(1, 3);
						Projectile.velocity.Y -= 3;
						for(int i = 0; i <= skeletstospawn; i++)
                        {
							Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CrossSkelet>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI, target.whoAmI);
							proj.position.X += Main.rand.Next(-20, 21);
							proj.damage = (int)(proj.damage * (0.5f / skeletstospawn + 0.5f));
                        }

						for (int i = 0; i <= 12 + skeletstospawn; i++)
						{
							Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)), 
								Main.rand.NextVector2Circular(-3, 3), 
								Mod.Find<ModGore>("Gores/Skelet/bonger" + Main.rand.Next(1, 5)).Type);
							gore.timeLeft = 40;
						}
					}
					if (Projectile.ai[0] > 160)
						Projectile.ai[0] = 0;
				}
			}
			else
			{
				Projectile.ai[0] = 0;
				Projectile.ai[1] = 0;
				Projectile.frame = 0;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCs.Add(index);

		public override void Kill(int timeLeft) => MakeDust();

		public void MakeDust()
		{
			for(int i = 0; i < 40; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Poisoned, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-0.25f, 0.5f));
				dust.scale *= 0.66f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle drawframe = new Rectangle(0, Projectile.frame * tex.Height / Main.projFrames[Projectile.type], tex.Width, tex.Height / Main.projFrames[Projectile.type]);
			SpriteEffects flip = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Main.spriteBatch.Draw(tex,
					Projectile.Center - Main.screenPosition + (Vector2.UnitX * Projectile.spriteDirection * 10),
					drawframe,
					Projectile.GetAlpha(Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16)),
					Projectile.rotation,
					drawframe.Size() / 2,
					Projectile.scale,
					flip,
					0);

			return false;
		}
	}
}