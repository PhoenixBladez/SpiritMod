using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.HauntedTome
{
	public class HauntedTome : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Tome");
			Main.npcFrameCount[npc.type] = 19;
		}

		private int frame = 0;

		public override void SetDefaults()
		{
			npc.Size = new Vector2(34, 34);
			npc.lifeMax = 600;
			npc.damage = 20;
			npc.defense = 8;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.aiStyle = -1;
			npc.value = 600;
			npc.knockBackResist = 1f;
			npc.HitSound = new LegacySoundStyle(SoundID.NPCHit, 15).WithPitchVariance(0.2f);
			npc.DeathSound = SoundID.NPCDeath6;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		ref float AiTimer => ref npc.ai[0];

		ref float AttackType => ref npc.ai[1];

		private delegate void Attack(Player player, NPC npc);

		private enum Attacks
		{
			Skulls = 1,
			Fireballs = 2
		}

		private static readonly IDictionary<int, Attack> AttackDict = new Dictionary<int, Attack> {
			{ (int)Attacks.Skulls, delegate(Player player, NPC npc) { Skulls(player, npc); } },
			{ (int)Attacks.Fireballs, delegate(Player player, NPC npc) { Fireballs(player, npc); } },
		};

		private List<int> Pattern = new List<int>
		{
			(int)Attacks.Skulls,
			(int)Attacks.Fireballs
		};

		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach (int i in Pattern)
				writer.Write(i);
		}

		public override void ReceiveExtraAI(BinaryReader reader) => Pattern = Pattern.Select(i => reader.ReadInt32()).ToList();

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;

			if (++AiTimer < 180) {
				Vector2 homeCenter = player.Center;
				homeCenter.Y -= 100;

				npc.velocity = new Vector2(MathHelper.Clamp(npc.velocity.X + (0.2f * npc.DirectionTo(homeCenter).X), -6, 6), MathHelper.Clamp(npc.velocity.Y + (0.1f * npc.DirectionTo(homeCenter).Y), -2, 2));
			}
			if (AiTimer == 180)
				npc.velocity = -npc.DirectionTo(player.Center) * 6;

			if (AiTimer > 180) {
				AttackDict[Pattern[(int)AttackType]].Invoke(Main.player[npc.target], npc);

				if (frame < 9)
					UpdateFrame(12, 0, 9);
				else
					UpdateFrame(15, 9, 13);
			}
			else {
				if (frame > 4 && frame < 18)
					UpdateFrame(12, 4, 18);
				else
					UpdateFrame(10, 0, 4);
			}
		}

		private void ResetPattern()
		{
			AttackType++;
			AiTimer = 0;
			if (AttackType >= Pattern.Count) {
				AttackType = 0;
				Pattern.Randomize();
			}
			npc.netUpdate = true;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => knockback = (AiTimer < 180) ? knockback : 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => knockback = (AiTimer < 180) ? knockback : 0;

		private static void Skulls(Player player, NPC npc)
		{
			HauntedTome modnpc = npc.modNPC as HauntedTome;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if(modnpc.AiTimer % 20 == 0) {
				if (Main.netMode != NetmodeID.Server)
					Main.PlaySound(new LegacySoundStyle(SoundID.Item, 104).WithPitchVariance(0.2f), npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectileDirect(npc.Center,
									-Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 2) * 3,
									ModContent.ProjectileType<HauntedSkull>(),
									NPCUtils.ToActualDamage(30, 1.5f),
									1,
									Main.myPlayer,
									npc.whoAmI,
									npc.target).netUpdate = true;
			}

			if (modnpc.AiTimer == 340) //todo: replace dust triangle with green flash(same as draugr)
				DustHelper.DrawTriangle(npc.Center, 75, 2);

			if (modnpc.AiTimer > 360)
				modnpc.ResetPattern();
		}

		private static void Fireballs(Player player, NPC npc)
		{
			HauntedTome modnpc = npc.modNPC as HauntedTome;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if (modnpc.AiTimer > 360)
				modnpc.ResetPattern();
		}

		private void UpdateFrame(int framespersecond, int minframe, int maxframe)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= (60 / framespersecond)) {
				frame++;
				npc.frameCounter = 0;
			}
			if (frame > maxframe || frame < minframe)
				frame = minframe;
		}

		public override void NPCLoot()
		{
			npc.DropItem(ModContent.ItemType<Items.Weapon.Magic.ScreamingTome.ScreamingTome>());
			for (int i = 0; i < 8; i++)
				Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(0.5f, 0.5f), 99, Main.rand.NextFloat(0.6f, 1.2f));

			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/DownedMiniboss"));
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
	}

	internal class HauntedSkull : ModProjectile
	{
		public override string Texture => "SpiritMod/Items/Weapon/Magic/ScreamingTome/ScreamingSkull";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Skull");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 42);
			projectile.scale = Main.rand.NextFloat(0.4f, 0.8f);
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			projectile.alpha = (int)MathHelper.Max(projectile.alpha - 10, 0);
			NPC npc = Main.npc[(int)projectile.ai[0]];
			Player player = Main.player[(int)projectile.ai[1]];
			if(!npc.active || npc.type != ModContent.NPCType<HauntedTome>() || !player.active || player.dead) {
				projectile.Kill();
				return;
			}

			if (npc.ai[0] < 180 && projectile.localAI[0] == 0) {
				projectile.velocity = projectile.DirectionTo(player.Center) * 10;
				projectile.localAI[0]++;
				projectile.timeLeft = 180;
				if (Main.netMode != NetmodeID.Server)
					Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/skullscrem").WithPitchVariance(0.2f).WithVolume(0.33f), projectile.Center);
			}

			if(projectile.localAI[1] == 0 && Main.netMode != NetmodeID.Server) {
				SpiritMod.primitives.CreateTrail(new SkullPrimTrail(projectile, Color.Green, (int)(15 * projectile.scale), (int)(20 * projectile.scale)));
				projectile.localAI[1]++;
			}

			projectile.frameCounter++;
			//projectile.rotation = projectile.velocity.ToRotation() - ((projectile.spriteDirection > 0) ? MathHelper.Pi : 0);
			switch (projectile.localAI[0]) {
				case 0: Vector2 homepos = npc.Center;
					homepos.Y -= 90;

					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(homepos) * 3, 0.02f);
					if (projectile.frameCounter % 16 == 0) {
						if ((npc.Center.X > projectile.Center.X && projectile.spriteDirection == 1) || (npc.Center.X <= projectile.Center.X && projectile.spriteDirection == -1)) {
							projectile.frame++;
						}
					}
					if (projectile.frame >= 4) {
						projectile.spriteDirection = 0 - Math.Sign(projectile.spriteDirection);
						projectile.frame = 0;
					}
					break;

				case 1:
					if (projectile.frameCounter % 8 == 0 && projectile.frame < 5) {
						projectile.frame++;
					}

					projectile.spriteDirection = Math.Sign(-projectile.velocity.X);

					projectile.tileCollide = (projectile.timeLeft <= 150);
					if (projectile.velocity.Length() < 16)
						projectile.velocity *= 1.01f;

					break;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle drawFrame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			spriteBatch.Draw(tex,
					projectile.Center - Main.screenPosition,
					drawFrame,
					projectile.GetAlpha(lightColor),
					projectile.rotation,
					drawFrame.Size() / 2,
					projectile.scale,
					(projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
					0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 3, 1f, 0f); 

			for (int i = 0; i <= 3; i++) {
				Gore gore = Gore.NewGoreDirect(projectile.position + new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height)),
					Main.rand.NextVector2Circular(-1, 1),
					mod.GetGoreSlot("Gores/Skelet/bonger" + Main.rand.Next(1, 5)),
					projectile.scale);
				gore.timeLeft = 20;
			}
		}
	}
}