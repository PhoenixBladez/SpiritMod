using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using SpiritMod.Buffs;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.HauntedTome
{
	[AutoloadBossHead]
	public class HauntedTome : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Tome");
			Main.npcFrameCount[NPC.type] = 19;
		}

		private int frame = 0;

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(34, 34);
			NPC.lifeMax = 600;
			NPC.damage = 20;
			NPC.defense = 8;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.lavaImmune = true;
			NPC.aiStyle = -1;
			NPC.value = 600;
			NPC.knockBackResist = 1f;
			NPC.HitSound = SoundID.NPCHit15 with { PitchVariance = 0.2f };
			NPC.DeathSound = SoundID.NPCDeath6;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("A warding glyph was placed on this tome to protect it from improper use. Unfortunately for you, that means staring down a book with 20 canines."),
			});
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		ref float AiTimer => ref NPC.ai[0];

		ref float AttackType => ref NPC.ai[1];

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
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;

			if (++AiTimer < 180) {
				Vector2 homeCenter = player.Center;
				homeCenter.Y -= 100;

				NPC.velocity = new Vector2(MathHelper.Clamp(NPC.velocity.X + (0.2f * NPC.DirectionTo(homeCenter).X), -6, 6), MathHelper.Clamp(NPC.velocity.Y + (0.1f * NPC.DirectionTo(homeCenter).Y), -2, 2));
			}
			if (AiTimer == 180)
				NPC.velocity = -NPC.DirectionTo(player.Center) * 6;

			if (AiTimer > 180) {
				AttackDict[Pattern[(int)AttackType]].Invoke(Main.player[NPC.target], NPC);

				if (AiTimer % 30 == 0 && Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/PageFlip") with { PitchVariance = 0.2f }, NPC.Center);

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

			NPC.localAI[0] = Math.Max(NPC.localAI[0] - 0.05f, 0);
		}

		private void ResetPattern()
		{
			AttackType++;
			AiTimer = 0;
			if (AttackType >= Pattern.Count) {
				AttackType = 0;
				Pattern.Randomize();
			}
			NPC.netUpdate = true;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => knockback = (AiTimer < 180) ? knockback : 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => knockback = (AiTimer < 180) ? knockback : 0;

		private static void Skulls(Player player, NPC npc)
		{
			HauntedTome modnpc = npc.ModNPC as HauntedTome;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if(modnpc.AiTimer % 20 == 0) {
				if (Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(SoundID.Item104 with { PitchVariance = 0.2f }, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 2) * 3, ModContent.ProjectileType<HauntedSkull>(), NPCUtils.ToActualDamage(30, 1.5f), 1, Main.myPlayer, npc.whoAmI, npc.target).netUpdate = true;
			}

			if (modnpc.AiTimer == 340)
				npc.localAI[0] = 1;

			if (modnpc.AiTimer > 360)
				modnpc.ResetPattern();
		}

		private static void Planes(Player player, NPC npc)
		{
			HauntedTome modnpc = npc.ModNPC as HauntedTome;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

				if (modnpc.AiTimer % 45 == 0) {
				if (Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/PageRip"), npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center,
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
			NPC.frameCounter++;
			if (NPC.frameCounter >= (60 / framespersecond)) {
				frame++;
				NPC.frameCounter = 0;
			}
			if (frame > maxframe || frame < minframe)
				frame = minframe;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HauntedTomeGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HauntedTomeGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HauntedTomeGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HauntedTomeGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HauntedTomeGore1").Type, 1f);
			}
		}

		public override void OnKill()
		{
			MyWorld.downedTome = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			for (int i = 0; i < 8; i++)
				Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(0.5f, 0.5f), 99, Main.rand.NextFloat(0.6f, 1.2f));

			if (Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Items.Sets.SepulchreLoot.ScreamingTome.ScreamingTome>();
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1) + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, screenPos);
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1) + "_mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, screenPos, Color.White * NPC.localAI[0]);
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 1.5f;
			name = "Haunted Tome";
			downedCondition = () => MyWorld.downedTome;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> { ModContent.NPCType<HauntedTome>() },
				null,
				null,
				new List<int> {
					ModContent.ItemType<Items.Sets.SepulchreLoot.ScreamingTome.ScreamingTome>()
				});
			spawnInfo =
				"Haunted Tomes can be found while exploring Dark Sepulchres and interacting with certain items.";
			texture = "SpiritMod/Textures/BossChecklist/HauntedTomeTexture";
			headTextureOverride = "SpiritMod/NPCs/HauntedTome/HauntedTome_Head_Boss";
		}
	}

	internal class HauntedSkull : ModProjectile
	{
		public override string Texture => "SpiritMod/Items/Sets/SepulchreLoot/ScreamingTome/ScreamingSkull";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Skull");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(32, 42);
			Projectile.scale = Main.rand.NextFloat(0.4f, 0.8f);
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.alpha = (int)MathHelper.Max(Projectile.alpha - 10, 0);
			NPC npc = Main.npc[(int)Projectile.ai[0]];
			Player player = Main.player[(int)Projectile.ai[1]];
			if(!npc.active || npc.type != ModContent.NPCType<HauntedTome>() || !player.active || player.dead) {
				Projectile.Kill();
				return;
			}

			if (npc.ai[0] < 180 && Projectile.localAI[0] == 0) {
				Projectile.velocity = Projectile.DirectionTo(player.Center) * 14;
				Projectile.localAI[0]++;
				Projectile.timeLeft = 180;
				if (Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/skullscrem") with { PitchVariance = 0.2f, Volume = 0.33f }, Projectile.Center);
			}

			if(Projectile.localAI[1] == 0 && Main.netMode != NetmodeID.Server) {
				SpiritMod.primitives.CreateTrail(new SkullPrimTrail(Projectile, Color.DarkGreen, (int)(30 * Projectile.scale), (int)(20 * Projectile.scale)));
				Projectile.localAI[1]++;
			}

			Projectile.frameCounter++;
			//projectile.rotation = projectile.velocity.ToRotation() - ((projectile.spriteDirection > 0) ? MathHelper.Pi : 0);
			switch (Projectile.localAI[0]) {
				case 0: Vector2 homepos = npc.Center;
					homepos.Y -= 90;

					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(homepos) * 3, 0.02f);
					if (Projectile.frameCounter % 16 == 0) {
						if ((npc.Center.X > Projectile.Center.X && Projectile.spriteDirection == 1) || (npc.Center.X <= Projectile.Center.X && Projectile.spriteDirection == -1)) {
							Projectile.frame++;
						}
					}
					if (Projectile.frame >= 4) {
						Projectile.spriteDirection = 0 - Math.Sign(Projectile.spriteDirection);
						Projectile.frame = 0;
					}
					break;

				case 1:
					if (Projectile.frameCounter % 8 == 0 && Projectile.frame < 5) {
						Projectile.frame++;
					}

					Projectile.spriteDirection = Math.Sign(-Projectile.velocity.X);

					Projectile.tileCollide = (Projectile.timeLeft <= 150);
					if (Projectile.velocity.Length() < 16)
						Projectile.velocity *= 1.01f;

					break;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle drawFrame = new Rectangle(0, Projectile.frame * tex.Height / Main.projFrames[Projectile.type], tex.Width, tex.Height / Main.projFrames[Projectile.type]);
			Main.spriteBatch.Draw(tex,
					Projectile.Center - Main.screenPosition,
					drawFrame,
					Projectile.GetAlpha(lightColor),
					Projectile.rotation,
					drawFrame.Size() / 2,
					Projectile.scale,
					(Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
					0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center); 

			for (int i = 0; i <= 3; i++) {
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)),
					Main.rand.NextVector2Circular(-1, 1),
					Mod.Find<ModGore>("bonger" + Main.rand.Next(1, 5)).Type,
					Projectile.scale);
				gore.timeLeft = 20;
			}
		}
	}

	internal class HauntedPaperPlane : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Paper Plane");
			Main.projFrames[Projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(26, 26);
			Projectile.scale = Main.rand.NextFloat(0.9f, 1.1f);
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.spriteDirection = Main.rand.NextBool() ? -1 : 1;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach (float fl in Projectile.localAI)
				writer.Write(fl);
		}

		public override void ReceiveExtraAI(BinaryReader reader) => Projectile.localAI = Projectile.localAI.Select(i => reader.ReadSingle()).ToArray();

		public override void AI()
		{
			NPC npc = Main.npc[(int)Projectile.ai[0]];
			Player player = Main.player[(int)Projectile.ai[1]];
			if (!npc.active || npc.type != ModContent.NPCType<HauntedTome>() || !player.active || player.dead) {
				Projectile.Kill();
				return;
			}

			switch (Projectile.localAI[0]) {
				case 0: Projectile.alpha = Math.Min(Projectile.alpha + 16, 255); //fade out after emerging from tome
					Projectile.velocity *= 0.98f;
					if(Projectile.alpha >= 255) {//teleport to player's sides
						Projectile.localAI[0]++;
						Projectile.velocity = Vector2.Zero;
						float X = 250 * Main.rand.NextFloat(0.9f, 1.2f);
						float Y = -100 * Main.rand.NextFloat(0.9f, 1.2f);
						Projectile.position = player.position + new Vector2(Main.rand.NextBool() ? -X : X, Y);
						Projectile.netUpdate = true;
						Projectile.localAI[1] = 1;
					}
					break;
				case 1: Projectile.alpha = Math.Max(Projectile.alpha - 18, 0); //fade in and animate
					Projectile.localAI[1] = Math.Max(Projectile.localAI[1] - 0.03f, 0);
					Projectile.spriteDirection = Math.Sign(-Projectile.DirectionTo(player.Center).X);
					Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.AngleTo(player.Center), 0.08f);
					if (Projectile.alpha <= 0) {
						Projectile.frameCounter++;
						if(Projectile.frameCounter % 6 == 0) {
							Projectile.frame++;
							if(Projectile.frame >= Main.projFrames[Projectile.type]) { //swoop in on the player
								Projectile.frame = Main.projFrames[Projectile.type] - 1;
								Projectile.localAI[0]++;
								Projectile.velocity = Projectile.DirectionTo(player.Center) * 12;
								if (Main.netMode != NetmodeID.Server)
									SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.5f }, Projectile.Center);

								Projectile.netUpdate = true;
							}
						}
					}
					break;
				default:
					Projectile.rotation = Projectile.velocity.ToRotation();
					if (++Projectile.localAI[0] > 20) //move upwards after a delay
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, -Vector2.UnitY * 8, 0.01f);

					if (Projectile.velocity.Length() < 8)
						Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8;

					break;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			float scalemod = (Projectile.localAI[0] == 0) ? 0.5f : 1;
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle drawFrame = new Rectangle(0, Projectile.frame * tex.Height / Main.projFrames[Projectile.type], tex.Width, tex.Height / Main.projFrames[Projectile.type]);
			Main.spriteBatch.Draw(tex,
					Projectile.Center - Main.screenPosition,
					drawFrame,
					Projectile.GetAlpha(lightColor),
					Projectile.rotation - ((Projectile.spriteDirection > 0) ? MathHelper.Pi : 0),
					drawFrame.Size() / 2,
					Projectile.scale * scalemod,
					(Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
					0);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D tex = Mod.Assets.Request<Texture2D>("NPCs/HauntedTome/HauntedPaperPlane_mask").Value;
			Rectangle drawFrame = new Rectangle(0, Projectile.frame * tex.Height / Main.projFrames[Projectile.type], tex.Width, tex.Height / Main.projFrames[Projectile.type]);
			Main.spriteBatch.Draw(tex,
					Projectile.Center - Main.screenPosition,
					drawFrame,
					Projectile.GetAlpha(Color.White) * Projectile.localAI[1],
					Projectile.rotation - ((Projectile.spriteDirection > 0) ? MathHelper.Pi : 0),
					drawFrame.Size() / 2,
					Projectile.scale,
					(Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
					0);
		}

		public override void Kill(int timeLeft) => Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, Mod.Find<ModGore>("HauntedPaperPlane_gore").Type);
	}
}