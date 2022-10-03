using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Town;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.NPCs
{
	internal class GlobalNPCHappiness : GlobalNPC
	{
		public override void SetStaticDefaults()
		{
			NPCHappiness.Get(NPCID.Wizard)
				.SetNPCAffection<Gambler>(AffectionLevel.Like)
				.SetNPCAffection<RuneWizard>(AffectionLevel.Like);
			NPCHappiness.Get(NPCID.Pirate)
				.SetNPCAffection<Adventurer>(AffectionLevel.Like)
				.SetNPCAffection<Gambler>(AffectionLevel.Like)
				.SetNPCAffection<Rogue>(AffectionLevel.Dislike);
			NPCHappiness.Get(NPCID.GoblinTinkerer)
				.SetNPCAffection<RuneWizard>(AffectionLevel.Like)
				.SetNPCAffection<Gambler>(AffectionLevel.Dislike);
			NPCHappiness.Get(NPCID.ArmsDealer)
				.SetNPCAffection<Adventurer>(AffectionLevel.Like)
				.SetNPCAffection<Rogue>(AffectionLevel.Hate);
			NPCHappiness.Get(NPCID.Demolitionist)
				.SetNPCAffection<RuneWizard>(AffectionLevel.Like)
				.SetNPCAffection<Rogue>(AffectionLevel.Like);
			NPCHappiness.Get(NPCID.DD2Bartender).SetNPCAffection<Adventurer>(AffectionLevel.Like);
			NPCHappiness.Get(NPCID.BestiaryGirl).SetNPCAffection<Adventurer>(AffectionLevel.Like);
		}

		public override void GetChat(NPC npc, ref string chat)
		{
			float replaceChance = 0;
			List<string> dialogue = new();

			if (npc.type == NPCID.Wizard)
			{
				AddDialogueAboutNPC<RuneWizard>(dialogue, "You heard about {N}? His wares are interesting, I haven't seen that sort of magic myself.", 0.3f, npc, ref replaceChance);
				AddDialogueAboutNPC<Gambler>(dialogue, "With only a little transmutation spell, {N}'s boxes can ERUPT in doves. Try it sometime!", 0.1f, npc, ref replaceChance);
				AddDialogueAboutNPC<Gambler>(dialogue, "{N} never notices that my deck is magic, it's not even real. She's too focused!", 0.1f, npc, ref replaceChance);
			}
			else if (npc.type == NPCID.Pirate)
			{
				AddDialogueAboutNPC<Adventurer>(dialogue, "Aye, that {N} guy sure knows his swashbucklin'. He may be a landlubber but he's not o' bad one if I say so meself.", 0.2f, npc, ref replaceChance);
				AddDialogueAboutNPC<Rogue>(dialogue, "Hmph. Ol' {N} ov'r there reminds me of days past. An' not good 'uns, mind ye.", 0.3f, npc, ref replaceChance);
				AddDialogueAboutNPC<Gambler>(dialogue, "Y'know, {N} has it in her to be a good pirate on day. Aye...she's got an eye fer gold.", 0.3f, npc, ref replaceChance);
			}
			else if (npc.type == NPCID.GoblinTinkerer)
			{
				AddDialogueAboutNPC<RuneWizard>(dialogue, "Me and {N} share experience daily - his magic is fascinating! A wonder to speak to.", 0.4f, npc, ref replaceChance);
				AddDialogueAboutNPC<Gambler>(dialogue, "{N} reminds me of my people. Lazy sellers peddling you MORE spiky balls for ABSURD prices...", 0.2f, npc, ref replaceChance);
			}
			else if (npc.type == NPCID.ArmsDealer)
			{
				AddDialogueAboutNPC<Rogue>(dialogue, "{N}? That little pest. Tried to take my Minishark without coughing up the cash.", 0.3f, npc, ref replaceChance);
				AddDialogueAboutNPC<Rogue>(dialogue, "I've never seen a man as annoying as that {N}. Thankfully, I have ways to keep him gone.", 0.3f, npc, ref replaceChance);
				AddDialogueAboutNPC<Adventurer>(dialogue, "I know {N}. That guy's not too bad. Especially once he stopped selling guns, heh.", 0.2f, npc, ref replaceChance);
			}
			else if (npc.type == NPCID.Demolitionist)
			{
				AddDialogueAboutNPC<RuneWizard>(dialogue, "Bahaha! That {N} guy is a hoot. He enchants my goods free of charge!", 0.4f, npc, ref replaceChance);
				AddDialogueAboutNPC<Rogue>(dialogue, "{N}'s a bit of a wily fellow, but any fella who's good on buyin' a round gets my respect!", 0.2f, npc, ref replaceChance);
			}
			else if (npc.type == NPCID.Golfer)
				AddDialogueAboutNPC<Gambler>(dialogue, "Who's {N} and why does she want to sell me \"[RARE] GOLF LOOTBOXES\"?", 0.3f, npc, ref replaceChance);
			else if (npc.type == NPCID.DD2Bartender)
				AddDialogueAboutNPC<Adventurer>(dialogue, "That {N} knows how to tell a good story. Doesn't even buy that much drink, but I enjoy it nonetheless.", 0.2f, npc, ref replaceChance);
			else if (npc.type == NPCID.Nurse)
				AddDialogueAboutNPC<Gambler>(dialogue, "Ahem...\"Buy yourself a couple boxes and you won't need healing!\" Did {N} really tell me to read you an ad?", 0.2f, npc, ref replaceChance);
			else if (npc.type == NPCID.BestiaryGirl)
				AddDialogueAboutNPC<Adventurer>(dialogue, "Like, {N} really knows a lot about animals. He's totes cool!", 0.2f, npc, ref replaceChance);

			if (Main.rand.NextFloat() < replaceChance)
				chat = Main.rand.Next(dialogue);
		}

		public void AddDialogueAboutNPC<T>(List<string> dialogue, string text, float increase, NPC npc, ref float replaceChance) where T : ModNPC => AddDialogueAboutNPC(dialogue, ModContent.NPCType<T>(), text, increase, npc, ref replaceChance);
		public void AddDialogueAboutNPC(List<string> dialogue, int npcID, string text, float increase, NPC self, ref float replaceChance)
		{
			int npc = NPC.FindFirstNPC(npcID);
			if (npc >= 0 && IsTownNPCNearby(self, npcID, out bool _))
			{
				dialogue.Add(text.Replace("{N}", $"{Main.npc[npc].GivenName}"));

				float CombineChances(float p1, float p2) => p1 + p2 - (p1 * p2);

				replaceChance = CombineChances(replaceChance, increase);
			}
		}

		/// <summary>Adapted from vanilla's ShopHelper.</summary>
		internal static List<NPC> GetNearbyResidentNPCs(NPC current, out int npcsWithinHouse, out int npcsWithinVillage)
		{
			List<NPC> list = new List<NPC>();
			npcsWithinHouse = 0;
			npcsWithinVillage = 0;

			Vector2 npcHome = new(current.homeTileX, current.homeTileY);
			if (current.homeless)
				npcHome = new(current.Center.X / 16f, current.Center.Y / 16f);

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (i == current.whoAmI)
					continue;

				NPC npc = Main.npc[i];
				if (npc.active && npc.townNPC && !IsNotReallyTownNPC(npc) && !WorldGen.TownManager.CanNPCsLiveWithEachOther_ShopHelper(current, npc))
				{
					Vector2 otherHome = new(npc.homeTileX, npc.homeTileY);
					if (npc.homeless)
						otherHome = npc.Center / 16f;

					float dist = Vector2.DistanceSquared(npcHome, otherHome);

					if (dist < 25f * 25f)
					{
						list.Add(npc);
						npcsWithinHouse++;
					}
					else if (dist < 120f * 120f)
						npcsWithinVillage++;
				}
			}
			return list;
		}

		/// <summary>Adapted from vanilla's ShopHelper.</summary>
		internal static bool IsTownNPCNearby(NPC current, int otherNPCType, out bool otherNPCInHome)
		{
			otherNPCInHome = false;
			Vector2 npcHome = new(current.homeTileX, current.homeTileY);
			if (current.homeless)
				npcHome = new(current.Center.X / 16f, current.Center.Y / 16f);

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (i == current.whoAmI)
					continue;

				NPC npc = Main.npc[i];
				if (npc.active && npc.townNPC && !IsNotReallyTownNPC(npc) && !WorldGen.TownManager.CanNPCsLiveWithEachOther_ShopHelper(current, npc) && npc.type == otherNPCType)
				{
					Vector2 otherHome = new(npc.homeTileX, npc.homeTileY);
					if (npc.homeless)
						otherHome = npc.Center / 16f;

					float dist = Vector2.DistanceSquared(npcHome, otherHome);

					if (dist < 25f * 25f)
					{
						otherNPCInHome = true;
						return true;
					}
					else if (dist < 120f * 120f)
						return true;
				}
			}
			return false;
		}

		/// <summary>Adapted from vanilla's ShopHelper.</summary>
		private static bool IsNotReallyTownNPC(NPC npc) => npc.type == NPCID.OldMan || npc.type == NPCID.TravellingMerchant || NPCID.Sets.ActsLikeTownNPC[npc.type];
	}
}