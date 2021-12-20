using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.Oracle
{
	[AutoloadHead]
	public class Oracle : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/Oracle/Oracle";
		//public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };

		private ref float Timer => ref npc.ai[0];

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
			npc.velocity.Y = (float)Math.Sin(Timer * 0.02f) * 0.25f;

			npc.TargetClosest(true);
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

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			NPCUtils.AddItem(ref shop, ref nextSlot, ItemID.Meowmere, 1);
			NPCUtils.AddItem(ref shop, ref nextSlot, ItemID.CelestialSigil, 99);
		}

		public override void SetChatButtons(ref string button, ref string button2) => button = Language.GetTextValue("LegacyInterface.28");
	}
}
