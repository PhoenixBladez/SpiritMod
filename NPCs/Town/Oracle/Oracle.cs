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
		public const int AuraRadius = 550;

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

		public override bool Autoload(ref string name)
		{
			name = "Oracle";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oracle");
			Main.npcFrameCount[npc.type] = 1;
			//NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			//NPCID.Sets.AttackFrameCount[npc.type] = 4;
			//NPCID.Sets.DangerDetectRange[npc.type] = 500;
			//NPCID.Sets.AttackType[npc.type] = 0;
			//NPCID.Sets.AttackTime[npc.type] = 16;
			//NPCID.Sets.AttackAverageChance[npc.type] = 30;
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

			if (IsBeingTalkedTo())
			{
				npc.velocity.X *= 0.96f;
				movementTimer = 50;
				movementDir = 0f;
				return;
			}

			if (Teleport-- > 0)
			{
				if (Teleport > 50)
					AttackTimer--;

				movementDir = 0f;
				movementTimer = 100;

				if (Teleport == 195 && Vector2.DistanceSquared(npc.Center, new Vector2(TeleportX, TeleportY)) > 1800 * 1800)
					CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y - 40, npc.width, 20), Color.Red, "I sense a call...");

				if (Teleport == 50)
				{
					npc.Center = new Vector2(TeleportX, TeleportY);

					//add visuals
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

			if ((npc.Center.Y / 16f) + 6 + adjustLevHeight < tileDist)
				npc.velocity.Y += 0.16f; //Grounds the NPC
			if ((npc.Center.Y / 16f) > tileDist - (5 + adjustLevHeight))
				npc.velocity.Y -= 0.16f; //Raises the NPC
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
						cur.AddBuff(BuffID.OnFire, 2);

					enemyNearby = true;
				}
			}

			if (enemyNearby)
				AttackTimer = Math.Min(AttackTimer + 1, 150);
			else
				AttackTimer = Math.Max(AttackTimer - 1, 0);
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

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D aura = mod.GetTexture("NPCs/Town/Oracle/OracleAura");

			Vector2 drawPosition = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);
			spriteBatch.Draw(aura, drawPosition, null, Color.White, timer * 0.2f, aura.Size() / 2f, RealAuraScale, SpriteEffects.None, 0f);
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

				if (Main.mouseLeft && Main.mouseLeftRelease) //If clicked on, give the player the buff.
					Main.LocalPlayer.AddBuff(ModContent.BuffType<OracleBoonBuff>(), 3600 * 5);
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
				"Not having eyes is a blessing from the gods when you work where I work."
			};
			return Main.rand.Next(options);
		}

		public override string TownNPCName()
		{
			string[] names = { "Wow", "If", "Only", "I", "Could", "Come", "Up", "With", "Names" };
			return Main.rand.Next(names);
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
				shop = true;
		}

		public override void SetupShop(Chest shop, ref int nextSlot) //OlympiumToken
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
			nextSlot += 2;

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<OracleScripture>());
			shop.item[nextSlot].shopCustomPrice = 1;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;
		}

		public override void SetChatButtons(ref string button, ref string button2) => button = Language.GetTextValue("LegacyInterface.28");
	}
}
