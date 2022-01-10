using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SpiritMod.NPCs.Town.Oracle
{
	[AutoloadHead]
	public class Oracle : ModNPC
	{
		public const int AuraRadius = 263;

		private float RealAuraRadius => AuraRadius * RealAuraScale;
		private float RealAuraScale => Math.Min(AttackTimer / 150f, 1f);
		//public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };

		private float timer = 0;
		private float movementDir = 0;
		private float movementTimer = 0;

		public ref float Teleport => ref npc.ai[0];
		public ref float AttackTimer => ref npc.ai[1];
		public ref float TeleportX => ref npc.ai[2];
		public ref float TeleportY => ref npc.ai[3];

		private ref Player NearestPlayer => ref Main.player[npc.target];

		private Rectangle[] runeSources = null;

		public override bool Autoload(ref string name)
		{
			name = "Oracle";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oracle");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.Guide);
			npc.townNPC = true;
			npc.friendly = true;
			npc.aiStyle = -1;
			npc.damage = 30;
			npc.defense = 30;
			npc.lifeMax = 300;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.dontTakeDamage = true;
			npc.immortal = true;
		}

		public override void AI()
		{
			timer++;
			npc.velocity.Y = (float)Math.Sin(timer * 0.06f) * 0.4f;

			npc.TargetClosest(true);
			Movement();
			OffenseAbilities();
		}

		private void Movement()
		{
			const float MoveSpeed = 1f;

			if (IsBeingTalkedTo() || AttackTimer > 5)
			{
				npc.velocity.X *= 0.96f;
				movementTimer = 50;
				movementDir = 0f;
				return;
			}

			if (Teleport-- > 0)
			{
				if (Teleport > 50)
				{
					npc.alpha += 2;
					AttackTimer--;
				}

				movementDir = 0f;
				movementTimer = 100;

				if (Teleport == 195)
					CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y - 40, npc.width, 20), Color.LightGoldenrodYellow, "I sense a call...");

				if (Teleport == 50)
				{
					npc.Center = new Vector2(TeleportX, TeleportY);
					npc.alpha = 0;
					float swirlSize = 1.664f;
					float degrees = 0;
					float Closeness = 50f;
					degrees += 2.5f;
					for (float swirlDegrees = degrees; swirlDegrees < 160 + degrees; swirlDegrees += 7f)
					{
						Closeness -= swirlSize; //It closes in
						double radians = swirlDegrees * (Math.PI / 180); //convert to radians

						Vector2 eastPosFar = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
						Vector2 westPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
						Vector2 northPosFar = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
						Vector2 southPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
						int d4 = Dust.NewDust(eastPosFar, 2, 2, DustID.GoldCoin, 0, 0);
						Main.dust[d4].noGravity = true;
						int d5 = Dust.NewDust(westPosFar, 2, 2, DustID.GoldCoin, 0, 0);
						Main.dust[d5].noGravity = true;
						int d6 = Dust.NewDust(northPosFar, 2, 2, DustID.GoldCoin, 0, 0);
						Main.dust[d6].noGravity = true;
						int d7 = Dust.NewDust(southPosFar, 2, 2, DustID.GoldCoin, 0, 0);
						Main.dust[d7].noGravity = true;
					}
					Main.PlaySound(SoundID.DD2_DarkMageCastHeal, npc.Center);
				}
			}

			int tileDist = GetTileAt(0, out bool liquid);

			HandleFloatHeight(tileDist);

			if (!liquid)
			{
				GetTileAt(-1, out bool left);
				GetTileAt(1, out bool right);

				movementTimer--;
				if (movementTimer < 0)
				{
					var options = new List<float> { 0f };

					if (!left)
						options.Add(-MoveSpeed);

					if (!right)
						options.Add(MoveSpeed);

					if (movementDir == 0)
						movementDir = Main.rand.Next(options);
					else
						movementDir = 0f;

					movementTimer = movementDir == 0f ? Main.rand.Next(200, 300) : Main.rand.Next(300, 400);

					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
				}

				if (movementDir < 0f && left)
					movementDir = 0f;
				else if (movementDir > 0f && right)
					movementDir = 0f;
			}
			else
				ScanForLand();

			npc.velocity.X = movementDir == 0f ? npc.velocity.X * 0.98f : movementDir;

			if ((movementDir == 0f && Math.Abs(npc.velocity.X) < 0.15f) || IsBeingTalkedTo())
			{
				if (npc.DistanceSQ(NearestPlayer.Center) < 400 * 400)
					npc.direction = npc.spriteDirection = NearestPlayer.Center.X < npc.Center.X ? -1 : 1;
			}
			else
				npc.direction = npc.spriteDirection = npc.velocity.X < 0 ? -1 : 1;
		}

		private void HandleFloatHeight(int tileDist)
		{
			int[] ceilingHeights = new int[5];
			for (int i = -2; i < 3; ++i)
				ceilingHeights[i + 2] = GetTileAt(-1, out _, true);

			int avgCeilingHeight = 0;

			for (int i = 0; i < ceilingHeights.Length; ++i)
			{
				if (ceilingHeights[i] == -1)
					avgCeilingHeight += 10;
				else
					avgCeilingHeight += (int)(npc.Center.Y / 16f) - ceilingHeights[i];
			}

			avgCeilingHeight /= 5;
			int adjustLevHeight = 5;

			if (avgCeilingHeight <= 10)
				adjustLevHeight = (int)(avgCeilingHeight * 0.25f);

			adjustLevHeight -= 5;

			if ((npc.Center.Y / 16f) + 6 + adjustLevHeight < tileDist)
				npc.velocity.Y += 0.36f; //Grounds the NPC
			if ((npc.Center.Y / 16f) > tileDist - (5 + adjustLevHeight))
				npc.velocity.Y -= 0.36f; //Raises the NPC
		}

		private void ScanForLand()
		{
			const int SearchDist = 20;

			int nearestTileDir = 1000;

			for (int i = -SearchDist; i < SearchDist + 1; ++i)
			{
				GetTileAt(i, out bool liq);
				int thisDist = (int)(npc.Center.X / 16f) + i;
				if (!liq && Math.Abs((int)(npc.Center.X / 16f) - nearestTileDir) > Math.Abs((int)(npc.Center.X / 16f) - thisDist))
					nearestTileDir = thisDist;
			}

			if (nearestTileDir != 1000)
			{
				int dir = (npc.Center.X / 16f) > nearestTileDir ? -1 : 1;
				npc.velocity.X = dir * 1.15f;
			}
		}

		public bool IsBeingTalkedTo()
		{
			for (int i = 0; i < Main.maxPlayers; ++i)
			{
				Player p = Main.player[i];
				if (p.active && !p.dead && p.talkNPC == npc.whoAmI)
					return true;
			}
			return false;
		}

		private int GetTileAt(int xOffset, out bool liquid, bool up = false)
		{
			int tileDist = (int)(npc.Center.Y / 16f);
			liquid = true;

			while (true)
			{
				tileDist += !up ? 1 : -1;

				if (tileDist < 20)
					return -1;

				Tile t = Framing.GetTileSafely((int)(npc.Center.X / 16f) + xOffset, tileDist);
				if (t.active() && Main.tileSolid[t.type])
				{
					liquid = false;
					break;
				}
				else if (t.liquid > 155)
					break;
			}
			return tileDist;
		}

		private void OffenseAbilities()
		{
			bool enemyNearby = false;

			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC cur = Main.npc[i];
				if (cur.active && cur.CanBeChasedBy() && cur.DistanceSQ(npc.Center) < AuraRadius * AuraRadius) //Scan for NPCs
				{
					if (cur.DistanceSQ(npc.Center) < RealAuraRadius * RealAuraRadius) //Actually inflict damage to NPCs
						cur.AddBuff(ModContent.BuffType<GreekFire>(), 2);

					enemyNearby = true;
				}
			}

			if (float.IsNaN(AttackTimer))
				AttackTimer = 0;

			if (enemyNearby)
				AttackTimer = (float)Math.Min(Math.Pow(AttackTimer + 1, 1.005f), 150);
			else
			{
				AttackTimer = (float)Math.Max(Math.Pow(AttackTimer, 0.991f), 0f);
				if (AttackTimer < 10)
					AttackTimer--;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(timer);
			writer.Write(movementDir);
			writer.Write(movementTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			timer = reader.ReadSingle();
			movementDir = reader.ReadSingle();
			movementTimer = reader.ReadSingle();
		}

		public override void FindFrame(int frameHeight)
		{
			if (AttackTimer > 5 && npc.frame.Y < 200)
				npc.frame.Y = 200;
			else if (AttackTimer <= 5 && npc.frame.Y >= 200)
				npc.frame.Y = 0;

			npc.frameCounter += 6;
			if (AttackTimer > 2)
				npc.frameCounter += 2;

			if (npc.frameCounter > 42)
			{
				npc.frame.Y += frameHeight;

				int max = AttackTimer > 5 ? 8 : 4;
				if (npc.frame.Y >= frameHeight * max)
					npc.frame.Y = frameHeight * (max - 4);

				npc.frameCounter = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (AttackTimer > 10 || Teleport > 50)
			{
				float wave = (float)Math.Cos(Main.GlobalTime % 2.4f / 2.4f * MathHelper.TwoPi) + 0.5f;

				SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LightGoldenrodYellow);

				for (int i = 0; i < 4; i++)
				{
					Color col = npc.GetAlpha(baseCol) * (1f - wave);
					Vector2 drawPos = npc.Center + (i / 4f * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * wave + 4f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, npc.frame, col, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
				}
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D aura = mod.GetTexture("NPCs/Town/Oracle/OracleAura");

			if (runeSources == null) //Initialize runeSources
			{
				Rectangle IndividualRuneSource() => new Rectangle(0, 32 * Main.rand.Next(8), 32, 32);

				runeSources = new Rectangle[8];

				for (int i = 0; i < runeSources.Length; ++i)
					runeSources[i] = IndividualRuneSource();
			}

			float wave = (float)Math.Cos(Main.GlobalTime % 2.4f / 2.4f * MathHelper.TwoPi) + 0.5f;

			SpriteEffects direction = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LightGoldenrodYellow);

			for (int i = 0; i < 4; i++)
			{
				Color col = npc.GetAlpha(baseCol) * (1f - wave);
				Vector2 drawPos = npc.Center + (i / 4f * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * wave + 4f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
				spriteBatch.Draw(aura, drawPos, null, col, timer * 0.02f, aura.Size() / 2f, RealAuraScale, direction, 0f);
				DrawRuneCircle(spriteBatch, i, col, wave);
				DrawLetter(spriteBatch, i, col, wave);
			}

			Vector2 drawPosition = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);
			spriteBatch.Draw(aura, drawPosition, null, baseCol, timer * 0.02f, aura.Size() / 2f, RealAuraScale, SpriteEffects.None, 0f);
			DrawRuneCircle(spriteBatch, -1, baseCol);
			DrawLetter(spriteBatch, -1, baseCol);
		}

		private void DrawRuneCircle(SpriteBatch spriteBatch, int i, Color col, float wave = 0f)
		{
			Texture2D runes = mod.GetTexture("NPCs/Town/Oracle/OracleRunes");

			Vector2 drawPos = npc.Center + (i / 4f * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * wave + 4f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
			if (i == -1)
				drawPos = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);

			void DrawIndividualRune(int offset)
			{
				Vector2 circleOffset = new Vector2(0, 88 * RealAuraScale).RotatedBy((-timer * 0.02f) + (MathHelper.PiOver4 * offset));
				spriteBatch.Draw(runes, drawPos + circleOffset, runeSources[offset], col, 0f, new Vector2(16), RealAuraScale * 0.9f, SpriteEffects.None, 0f);
			}

			for (int j = 0; j < 8; ++j)
				DrawIndividualRune(j);
		}

		private void DrawLetter(SpriteBatch spriteBatch, int i, Color col, float wave = 0f)
		{
			Texture2D letter = mod.GetTexture("NPCs/Town/Oracle/OracleAuraLetter");

			Vector2 drawPos = npc.Center + (i / 4f * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * wave + 4f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
			if (i == -1)
				drawPos = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);

			Vector2 circleOffset = new Vector2(0, 228 * RealAuraScale).RotatedBy(timer * 0.02f);
			spriteBatch.Draw(letter, drawPos + circleOffset, null, col, 0f, letter.Size() / 2f, RealAuraScale, SpriteEffects.None, 0f);
		}

		public static bool HoveringBuffButton = false;
		public static void DrawBuffButton(int superColor, int numLines)
		{
			const string text = "Bless";

			DynamicSpriteFont font = Main.fontMouseText;
			Vector2 scale = new Vector2(0.9f);
			Vector2 stringSize = ChatManager.GetStringSize(font, text, scale);
			Vector2 position = new Vector2(180 + Main.screenWidth / 2 + stringSize.X - 20f, 130 + numLines * 30);
			Color baseColor = new Color(superColor, (int)(superColor / 1.1), superColor / 2, superColor);
			Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);

			if (mousePos.Between(position, position + stringSize * scale) && !PlayerInput.IgnoreMouseInterface) //Mouse hovers over button
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.releaseUseItem = true;
				scale *= 1.1f;

				if (!HoveringBuffButton)
					Main.PlaySound(SoundID.MenuTick);

				HoveringBuffButton = true;

				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					var options = new List<string>
					{
						"You wish for a challenge? I may stop you not, as it benefits us both. I do hope you're prepared!",
						"I shall consult the gods about their boons - these monsters will become relentless, I hope you are aware.",
						"You want me to call them what?! Such foul battle language, yet I will deliver the message. The Gods won't be happy about this!",
						"The boons cause slain foes to drop stronger tokens, yes, but do remember that the foes become stronger, too!",
						"Do come back alive! I would enjoy hearing tales of your victories. Bring many tokens as well!"
					};
					for (int i = 0; i < 30; i++)
					{
						int num = Dust.NewDust(new Vector2(Main.LocalPlayer.Center.X + Main.rand.Next(-100, 100), Main.LocalPlayer.Center.Y + Main.rand.Next(-100, 100)), Main.LocalPlayer.width, Main.LocalPlayer.height, ModContent.DustType<Dusts.BlessingDust>(), 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].scale = Main.rand.Next(70, 105) * 0.01f;
						Main.dust[num].fadeIn = 1;
					}
					int glyphnum = Main.rand.Next(10);
					DustHelper.DrawDustImage(new Vector2(Main.LocalPlayer.Center.X, Main.LocalPlayer.Center.Y - 25), ModContent.DustType<Dusts.MarbleDust>(), 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1f);
					Main.npcChatText = Main.rand.Next(options);
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29).WithPitchVariance(0.4f).WithVolume(.6f), Main.LocalPlayer.Center);
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 6).WithPitchVariance(0.4f).WithVolume(.2f), Main.LocalPlayer.Center);

					Main.LocalPlayer.AddBuff(ModContent.BuffType<OracleBoonBuff>(), 3600 * 5);
				}
			}
			else
			{
				if (HoveringBuffButton)
					Main.PlaySound(SoundID.MenuTick);

				HoveringBuffButton = false;
			}

			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, position + new Vector2(16f, 14f), baseColor, 0f, stringSize * 0.5f, scale);
		}

		public override string GetChat()
		{
			var options = new List<string>
			{
				$"The heavens have certainly spoken of you, {Main.LocalPlayer.name}.",
				"The divinity I offer isn't for any simple coin, traveler.",
				"Have you caught wind of a man named Zagreus? ...nevermind.",
				"Oh, how far I'd go for some ichor...",
				"I have a little scroll for sale, if you wish to find me elsewhere.",
				"Not having eyes is a blessing from the gods when you work where I work.",
				"Warm greetings, warrior. I sense power within you, perhaps I can aid in its growth. The creatures within these walls hold mighty tokens, in which I am interested. If you were to trade them with me, I would grant you a weapon enchanted by the gods themselves!",
				"Ah, what I would give for some aged wine... Has Anthesteria arrived already?",
				"My epic tale has no end, and may never have one!",
				"Mythology? What part of this makes you believe it is a myth?",
				"I have lost track of time, and the gods refuse to tell me where it is!",
				"Lorem ipsum dolor sit amet... Be patient, I'm not finished.",
				"I am unable to die unless I am forgotten. I wonder who still remembers me...",
				"What do you need? I don't have unending time. Hm...on second thought...",
				"I had all life to write a glorious tale, but I cannot get past 'the'.",
				"Between you and me, reptiles cause me great distress.",
				"I ponder about the presence of ambient song in the distance, yet cannot stop myself from indulging in it.",
				"Boons? You want them? They're yours, my friend!",
				"Ah, I see. Or do I? Intriguing, is it not?",
				"The reason I float is simple - why should I not, if I can?"
			};
			return Main.rand.Next(options);
		}

		public override string TownNPCName()
		{
			string[] names = { "Pythia", "Cassandra", "Chrysame", "Eritha", "Theoclea", "Hypatia", "Themistoclea", "Phemonoe" };
			return Main.rand.Next(names);
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
				shop = true;
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Sets.OlympiumSet.ArtemisHunt.ArtemisHunt>());
			shop.item[nextSlot].shopCustomPrice = 25;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Sets.OlympiumSet.MarkOfZeus.MarkOfZeus>());
			shop.item[nextSlot].shopCustomPrice = 25;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Sets.OlympiumSet.BetrayersChains.BetrayersChains>());
			shop.item[nextSlot].shopCustomPrice = 25;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Sets.OlympiumSet.Eleutherios.Eleutherios>());
			shop.item[nextSlot].shopCustomPrice = 20;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.Potion.MirrorCoat>());
			shop.item[nextSlot].shopCustomPrice = 2;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot ++;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.OliveBranch>());
			shop.item[nextSlot].shopCustomPrice = 2;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<OracleScripture>());
			shop.item[nextSlot].shopCustomPrice = 1;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;
		}

		public override void SetChatButtons(ref string button, ref string button2) => button = Language.GetTextValue("LegacyInterface.28");

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<Oracle>()) && spawnInfo.marble ? 0.1f : 0f;
	}
}
