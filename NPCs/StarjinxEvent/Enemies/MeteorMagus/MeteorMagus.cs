using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MeteorMagus : SpiritNPC, IStarjinxEnemy
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Magus");
			Main.npcFrameCount[npc.type] = 5;
		}

		//large chunk of this(attack pattern use and randomizing) directly ripped from haunted tome, TODO: reduce boilerplate a lot

		public override void SetDefaults()
		{
			npc.Size = new Vector2(168, 118);
			npc.lifeMax = 1500;
			npc.damage = 56;
			npc.defense = 28;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.aiStyle = -1;
			npc.value = 1100;
			npc.knockBackResist = .55f;
			npc.HitSound = new LegacySoundStyle(SoundID.NPCHit, 55).WithPitchVariance(0.2f);
			npc.DeathSound = SoundID.NPCDeath51;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		ref float AiTimer => ref npc.ai[0];

		ref float AttackType => ref npc.ai[1];

		private delegate void Attack(Player player, NPC npc);

		private enum Attacks
		{
			FallingStars = 1,
			CirclingStars = 2,
			ShootingStars = 3
		}

		private static readonly IDictionary<int, Attack> AttackDict = new Dictionary<int, Attack> {
			{ (int)Attacks.CirclingStars, delegate(Player player, NPC npc) { CirclingStars(player, npc); } },
			{ (int)Attacks.FallingStars, delegate(Player player, NPC npc) { FallingStars(player, npc); } },
		};

		private List<int> Pattern = new List<int>
		{
			(int)Attacks.FallingStars,
			(int)Attacks.CirclingStars,
		};

		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach (int i in Pattern)
				writer.Write(i);
		}

		public override void ReceiveExtraAI(BinaryReader reader) => Pattern = Pattern.Select(i => reader.ReadInt32()).ToList();

		private readonly int IdleTime = 210;

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;

			bool empowered = npc.GetGlobalNPC<PathfinderGNPC>().Targetted;

			if (++AiTimer < IdleTime)
			{
				Vector2 homeCenter = player.Center;

				float extraSpeed = empowered ? 1.2f : 1f;
				npc.rotation = npc.velocity.X * .035f;

				switch (Pattern[(int)AttackType])
				{
					case (int)Attacks.FallingStars:
						homeCenter.Y -= 300;
						float xClamp = MathHelper.Clamp(npc.velocity.X + (0.18f * npc.DirectionTo(homeCenter).X), -4, 4);
						float yClamp = MathHelper.Clamp(npc.velocity.Y + (0.2f * npc.DirectionTo(homeCenter).Y), -5, 5);
						npc.velocity = new Vector2(xClamp, yClamp);
						break;

					default:
						homeCenter.Y -= 50;
						xClamp = MathHelper.Clamp(npc.velocity.X + (0.15f * npc.DirectionTo(homeCenter).X), -5, 5);
						yClamp = MathHelper.Clamp(npc.velocity.Y + (0.1f * npc.DirectionTo(homeCenter).Y), -2, 2);
						npc.velocity = new Vector2(xClamp, yClamp);
						break;
				}
			}

			if (AiTimer == IdleTime && Main.netMode != NetmodeID.Server)
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 123).WithPitchVariance(0.2f).WithVolume(1.3f), npc.Center);

			if (AiTimer > IdleTime)
			{
				AttackDict[Pattern[(int)AttackType]].Invoke(Main.player[npc.target], npc);

				npc.rotation = 0f;

				UpdateYFrame(8, 0, 3);
				frame.X = 1;
			}
			else
			{
				UpdateYFrame(10, 0, 4);
				frame.X = 0;
			}

			npc.localAI[0] = Math.Max(npc.localAI[0] - 0.05f, 0);
		}

		private void ResetPattern()
		{
			AttackType++;
			AiTimer = 0;
			if (AttackType >= Pattern.Count)
			{
				AttackType = 0;
				Pattern.Randomize();
			}
			npc.netUpdate = true;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => knockback = (AiTimer < IdleTime) ? knockback : 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => knockback = (AiTimer < IdleTime) ? knockback : 0;

		private void PlayCastSound(Vector2 position)
		{
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.3f).WithVolume(0.7f), position);
		}

		private static void FallingStars(Player player, NPC npc)
		{
			var modnpc = npc.modNPC as MeteorMagus;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if (modnpc.AiTimer % 15 == 0)
			{
				modnpc.PlayCastSound(npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					var pos = npc.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-120, -70));
					Projectile.NewProjectileDirect(pos, Vector2.Zero, ModContent.ProjectileType<MortarStar>(), npc.damage / 4, 1, Main.myPlayer, npc.whoAmI, player.whoAmI);
				}
			}

			if (modnpc.AiTimer > 330)
				modnpc.ResetPattern();
		}

		private static void CirclingStars(Player player, NPC npc)
		{
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			bool empowered = npc.GetGlobalNPC<PathfinderGNPC>().Targetted;

			var modnpc = npc.modNPC as MeteorMagus;

			if (modnpc.AiTimer == 220)
			{
				modnpc.PlayCastSound(player.Center);

				int numStars = 3 + (empowered ? 1 : 0);
				int direction = Main.rand.NextBool() ? -1 : 1;
				Vector2 offset = new Vector2(0, 250).RotatedByRandom(MathHelper.TwoPi);

				for (int i = 0; i < numStars; i++)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						var proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<CirclingStar>(), npc.damage / 4, 1, Main.myPlayer, npc.whoAmI, player.whoAmI);

						var star = proj.modProjectile as CirclingStar;
						star.Direction = direction;
						star.CirclingTime = empowered ? 50f : 60f;
						star.Offset = offset.RotatedBy(MathHelper.TwoPi * i / numStars);

						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
					}
				}
			}

			if (modnpc.AiTimer > 300)
				modnpc.ResetPattern();
		}

		public override void SafeFindFrame(int frameHeight) => npc.frame.Width = 168;

		public override void OnHitKill(int hitDirection, double damage)
		{
			for (int i = 0; i < 6; i++)
				Gore.NewGore(npc.Center, npc.velocity * .5f, 99, Main.rand.NextFloat(.75f, 1f));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.frame.Width > 168)
			{
				frame = new Point(0, 0);
				npc.FindFrame();
			}

			if (AiTimer > IdleTime)
			{
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				float num366 = num395 + 1.15f;
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.5f, Color.White * .7f, Color.White * .1f, 0.45f, num366, .65f);
			}
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
			return false;
		}

		public void DrawPathfinderOutline(SpriteBatch spriteBatch) => PathfinderOutlineDraw.DrawAfterImage(spriteBatch, npc, npc.frame, Vector2.Zero, Color.White, 0.75f, 1, 1.4f, npc.frame.Size() / 2);

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			void DrawGlowmask(Vector2 position, float opacity = 1)
			{
				spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_glow"), position - Main.screenPosition, npc.frame, Color.White * opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
			}

			DrawGlowmask(npc.Center);

			for (int i = 0; i < 4; i++)
			{
				Vector2 drawpos = npc.Center + new Vector2(0, 4 * (((float)Math.Sin(Main.GlobalTime * 4) / 2) + 0.5f)).RotatedBy(i * MathHelper.PiOver2);
				DrawGlowmask(drawpos, 0.3f);
			}
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
	}
}