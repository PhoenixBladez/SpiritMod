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
	public class HauntedTome : ModNPC, IBCRegistrable
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
			{ (int)Attacks.Fireballs, delegate(Player player, NPC npc) { Planes(player, npc); } },
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

				if (AiTimer % 30 == 0 && Main.netMode != NetmodeID.Server)
						Main.PlaySound(SpiritMod.instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/PageFlip").WithPitchVariance(0.2f), npc.Center);

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

			npc.localAI[0] = Math.Max(npc.localAI[0] - 0.05f, 0);
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

			if (modnpc.AiTimer == 340)
				npc.localAI[0] = 1;

			if (modnpc.AiTimer > 360)
				modnpc.ResetPattern();
		}

		private static void Planes(Player player, NPC npc)
		{
			HauntedTome modnpc = npc.modNPC as HauntedTome;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if (modnpc.AiTimer % 45 == 0) {
				if (Main.netMode != NetmodeID.Server)
					Main.PlaySound(SpiritMod.instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/PaperRip"), npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectileDirect(npc.Center,
									-Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 4) * 3,
									ModContent.ProjectileType<HauntedPaperPlane>(),
									NPCUtils.ToActualDamage(24, 1.25f),
									1,
									Main.myPlayer,
									npc.whoAmI,
									npc.target).netUpdate = true;
			}

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
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HauntedTomeGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HauntedTomeGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HauntedTomeGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HauntedTomeGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/HauntedTomeGore1"), 1f);
			}
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

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow"));
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_mask"), Color.White * npc.localAI[0]);
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 1.5f;
			name = "Haunted Tome";
			downedCondition = () => MyWorld.downedGladeWraith;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> { ModContent.NPCType<HauntedTome>() },
				null,
				null,
				new List<int> {
					ModContent.ItemType<Items.Weapon.Magic.ScreamingTome.ScreamingTome>()
				});
			spawnInfo =
				"Haunted Tomes can be found while exploring Dark Sepulchres and interacting with certain items.";
			texture = "SpiritMod/Textures/BossChecklist/HauntedTomeTexture";
			headTextureOverride = "SpiritMod/NPCs/HauntedTome/HauntedTome_Head_Boss";
		}
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
				projectile.velocity = projectile.DirectionTo(player.Center) * 14;
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

	internal class HauntedPaperPlane : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Paper Plane");
			Main.projFrames[projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(26, 26);
			projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.spriteDirection = Main.rand.NextBool() ? -1 : 1;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach (float fl in projectile.localAI)
				writer.Write(fl);
		}

		public override void ReceiveExtraAI(BinaryReader reader) => projectile.localAI = projectile.localAI.Select(i => reader.ReadSingle()).ToArray();

		public override void AI()
		{
			NPC npc = Main.npc[(int)projectile.ai[0]];
			Player player = Main.player[(int)projectile.ai[1]];
			if (!npc.active || npc.type != ModContent.NPCType<HauntedTome>() || !player.active || player.dead) {
				projectile.Kill();
				return;
			}

			switch (projectile.localAI[0]) {
				case 0: projectile.alpha = Math.Min(projectile.alpha + 16, 255); //fade out after emerging from tome
					projectile.velocity *= 0.98f;
					if(projectile.alpha >= 255) {//teleport to player's sides
						projectile.localAI[0]++;
						projectile.velocity = Vector2.Zero;
						float X = 250 * Main.rand.NextFloat(0.9f, 1.2f);
						float Y = -100 * Main.rand.NextFloat(0.9f, 1.2f);
						projectile.position = player.position + new Vector2(Main.rand.NextBool() ? -X : X, Y);
						projectile.netUpdate = true;
						projectile.localAI[1] = 1;
					}
					break;
				case 1: projectile.alpha = Math.Max(projectile.alpha - 18, 0); //fade in and animate
					projectile.localAI[1] = Math.Max(projectile.localAI[1] - 0.03f, 0);
					projectile.spriteDirection = Math.Sign(-projectile.DirectionTo(player.Center).X);
					projectile.rotation = Utils.AngleLerp(projectile.rotation, projectile.AngleTo(player.Center), 0.08f);
					if (projectile.alpha <= 0) {
						projectile.frameCounter++;
						if(projectile.frameCounter % 6 == 0) {
							projectile.frame++;
							if(projectile.frame >= Main.projFrames[projectile.type]) { //swoop in on the player
								projectile.frame = Main.projFrames[projectile.type] - 1;
								projectile.localAI[0]++;
								projectile.velocity = projectile.DirectionTo(player.Center) * 12;
								if (Main.netMode != NetmodeID.Server)
									Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 1, 0.6f, 0.5f);

								projectile.netUpdate = true;
							}
						}
					}
					break;
				default:
					projectile.rotation = projectile.velocity.ToRotation();
					if (++projectile.localAI[0] > 20) //move upwards after a delay
						projectile.velocity = Vector2.Lerp(projectile.velocity, -Vector2.UnitY * 8, 0.01f);

					if (projectile.velocity.Length() < 8)
						projectile.velocity = Vector2.Normalize(projectile.velocity) * 8;

					break;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float scalemod = (projectile.localAI[0] == 0) ? 0.5f : 1;
			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle drawFrame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			spriteBatch.Draw(tex,
					projectile.Center - Main.screenPosition,
					drawFrame,
					projectile.GetAlpha(lightColor),
					projectile.rotation - ((projectile.spriteDirection > 0) ? MathHelper.Pi : 0),
					drawFrame.Size() / 2,
					projectile.scale * scalemod,
					(projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
					0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = mod.GetTexture("NPCs/HauntedTome/HauntedPaperPlane_mask");
			Rectangle drawFrame = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			spriteBatch.Draw(tex,
					projectile.Center - Main.screenPosition,
					drawFrame,
					projectile.GetAlpha(Color.White) * projectile.localAI[1],
					projectile.rotation - ((projectile.spriteDirection > 0) ? MathHelper.Pi : 0),
					drawFrame.Size() / 2,
					projectile.scale,
					(projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
					0);
		}

		public override void Kill(int timeLeft) => Gore.NewGore(projectile.position, projectile.velocity, mod.GetGoreSlot("Gores/HauntedPaperPlane_gore"));
	}
}