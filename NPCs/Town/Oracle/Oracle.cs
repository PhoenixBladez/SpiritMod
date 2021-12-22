using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.Oracle
{
	[AutoloadHead]
	public class Oracle : ModNPC
	{
		public const int AuraRadius = 550;

		private float RealAuraRadius => AuraRadius * RealAuraScale;
		private float RealAuraScale => Math.Min(AttackTimer / 150f, 1f);
		//public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };

		private ref float Timer => ref npc.ai[0];
		private ref float AttackTimer => ref npc.ai[1];


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
			Timer++;
			npc.velocity.Y = (float)Math.Sin(Timer * 0.02f) * 0.12f;

			npc.TargetClosest(true);

			if (npc.DistanceSQ(NearestPlayer.Center) < 400 * 400)
				npc.spriteDirection = NearestPlayer.Center.X < npc.Center.X ? -1 : 1;

			OffenseAbilities();
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

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D aura = mod.GetTexture("NPCs/Town/Oracle/OracleAura");

			Vector2 drawPosition = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);
			spriteBatch.Draw(aura, drawPosition, null, Color.White, Timer * 0.2f, aura.Size() / 2f, RealAuraScale, SpriteEffects.None, 0f);
		}

		public override string GetChat()
		{
			var options = new List<string>
			{
				$"The heavens have certainly spoken of you, {Main.LocalPlayer.name}.",
				"The divinity I offer isn't for simple coin, traveller.",
				"Have you caught wind of a man named Zagreus? ...nevermind.",
				"Oh, how far I'd go for some ichor...",
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
			shop.item[nextSlot].SetDefaults(ItemID.Bananarang);
			shop.item[nextSlot].shopCustomPrice = 3;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(ItemID.LandMine);
			shop.item[nextSlot].shopCustomPrice = 20;
			shop.item[nextSlot].shopSpecialCurrency = SpiritMod.OlympiumCurrencyID;
			nextSlot++;
		}

		public override void SetChatButtons(ref string button, ref string button2) => button = Language.GetTextValue("LegacyInterface.28");
	}
}
