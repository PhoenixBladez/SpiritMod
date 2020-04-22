using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tide;
using SpiritMod.Items.Glyphs;
using SpiritMod.NPCs.Town;

namespace SpiritMod.NPCs
{
	public class GNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public int fireStacks;
		public int nebulaFlameStacks;
		public int GhostJellyStacks;
        public int angelLightStacks;
        public int angelWrathStacks;
        public int titanicSetStacks;
		public int duneSetStacks;
		public int acidBurnStacks;
		public bool vineTrap = false;
		public bool clatterPierce = false;
        public bool tracked = false;
		//Glyphs
		public bool voidInfluence;
		public int voidStacks;
		public bool sanguineBleed;
		public bool sanguinePrev;
		public bool unholyPlague;
		public int unholySource;
		public bool frostChill;
		public bool stormBurst;

		public bool amberFracture;

		public bool felBrand = false;
		public bool spectre = false;
		public bool soulBurn = false;
		public bool Stopped = false;
		public bool SoulFlare = false;
		public bool afflicted = false;
		public bool sunBurn = false;
		public bool starDestiny = false;
		public bool Death = false;
		public bool iceCrush = false;
		public bool pestilence = false;
		public bool moonBurn = false;
		public bool holyBurn = false;

		public bool DoomDestiny = false;

		public bool DoomDestiny1 = false;


		public bool sFracture = false;
		public bool Etrap = false;
		public bool necrosis = false;
		public bool blaze = false;
		public bool blaze1 = false;

		private static int[] martianMobs =
				new int[]
		{
			NPCID.MartianDrone,
			NPCID.MartianEngineer,
			NPCID.MartianOfficer,
			NPCID.MartianProbe,
			NPCID.MartianSaucer,
			NPCID.MartianTurret,
			NPCID.MartianWalker
		};

		public override void ResetEffects(NPC npc)
		{
			if (!voidInfluence)
			{
				if (voidStacks > VoidGlyph.DECAY)
					voidStacks -= VoidGlyph.DECAY;
				else
					voidStacks = 0;
			}
			else
				voidInfluence = false;
			sanguinePrev = sanguineBleed;
			sanguineBleed = false;
			unholyPlague = false;
			frostChill = false;
			stormBurst = false;
			vineTrap = false;
			clatterPierce = false;
			DoomDestiny = false;
			sFracture = false;
			Death = false;
			starDestiny = false;
			SoulFlare = false;
			afflicted = false;
			Etrap = false;
			Stopped = false;
			soulBurn = false;
			necrosis = false;
			moonBurn = false;
			sunBurn = false;
			blaze = false;
            tracked = false;
			iceCrush = false;
			blaze1 = false;
			felBrand = false;
			spectre = false;
			holyBurn = false;
			pestilence = false;
		}

		public override bool PreAI(NPC npc)
		{
			Player player = Main.player[Main.myPlayer];
			Vector2 dist = npc.position - player.position;
			if (Main.netMode == 0)
			{
				if (player.GetModPlayer<MyPlayer>().HellGaze == true && Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 160 && Main.rand.Next(80) == 1 && !npc.friendly)
				{
					npc.AddBuff(24, 130, false);
				}
			}
			if (Main.netMode == 0)
			{

				if (Stopped)
				{
					if (!npc.boss)
					{
						npc.velocity *= 0;
						npc.frame.Y = 0;
						return false;
					}
				}
			}

			return base.PreAI(npc);
		}

		public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if (npc.type == NPCID.MartianSaucer)
			{
				if (Main.netMode != 1 && npc.life < 0 && !NPC.AnyNPCs(mod.NPCType("Martian")))
				{
					NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, mod.NPCType("Martian"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
				}
			}
            if (npc.type == NPCID.GraniteFlyer && NPC.downedBoss2 && Main.netMode != 1 && npc.life <= 0 && Main.rand.Next(2) == 0)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                {
                    for (int i = 0; i < 20; i++)
                    {
                        int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .25f;
                        if (Main.dust[num].position != npc.Center)
                            Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                    }
                    Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height / 2f);
                    NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType("GraniteCore"));
                }
            }
            if (npc.life <= 0 && npc.FindBuffIndex(mod.BuffType("WanderingPlague")) >= 0)
				UnholyGlyph.ReleasePoisonClouds(npc, 0);
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			int before = npc.lifeRegen;
			bool drain = false;
			bool noDamage = damage <= 1;
			int damageBefore = damage;
            if (angelLightStacks > 0)
            {
                if (npc.FindBuffIndex(mod.BuffType("AngelLight")) < 0)
                {
                    angelLightStacks = 0;
                    return;
                }
            }
            if (angelWrathStacks > 0)
            {
                if (npc.FindBuffIndex(mod.BuffType("AngelWrath")) < 0)
                {
                    angelWrathStacks = 0;
                    return;
                }
            }
            #region Iriazul
            if (fireStacks > 0)
			{
				if (npc.FindBuffIndex(mod.BuffType("StackingFireBuff")) < 0)
				{
					fireStacks = 0;
					return;
				}

				drain = true;
				npc.lifeRegen -= 16;
				damage = Math.Max(damage, fireStacks * 5);
			}
			if (acidBurnStacks > 0)
			{
				if (npc.FindBuffIndex(mod.BuffType("AcidBurn")) < 0)
				{
					acidBurnStacks = 0;
					return;
				}
				
				drain = true;
				npc.lifeRegen -= 6;
				damage = Math.Max(damage, fireStacks * 2);
			}
			if (nebulaFlameStacks > 0)
			{
				if (npc.FindBuffIndex(mod.BuffType("NebulaFlame")) < 0)
				{
					nebulaFlameStacks = 0;
					return;
				}
				
				drain = true;
				npc.lifeRegen -= 16;
				damage = Math.Max(damage, fireStacks * 20);
			}
			if (amberFracture)
			{
				if (npc.FindBuffIndex(mod.BuffType("AmberFracture")) < 0)
				{
					fireStacks = 0;
					return;
				}

				drain = true;
				npc.lifeRegen -= 16;
				damage = Math.Max(damage, 25);
			}
			#endregion

			if (voidStacks > 0)
			{
				damage += 5 + 5 * (voidStacks / VoidGlyph.DELAY);
				npc.lifeRegen -= 20 + 20*voidStacks / VoidGlyph.DELAY;
			}
			if (sanguineBleed)
			{
				damage += 4;
				npc.lifeRegen -= 32;
			}
			if (unholyPlague)
			{
				damage += 5;
				npc.lifeRegen -= 20;
			}

			if (DoomDestiny)
			{
				drain = true;
				npc.lifeRegen -= 16;
				if (damage < 10)
				{
					damage = 10;
				}
			}
			if (starDestiny)
			{
				drain = true;
				npc.lifeRegen -= 150;
				damage = 75;
			}
			if (sFracture)
			{
				drain = true;
				npc.lifeRegen -= 9;
				damage = 3;
			}
			if (DoomDestiny1)
			{
				drain = true;
				npc.lifeRegen -= 30;
				damage = 10;
			}
			if (soulBurn)
			{
				drain = true;
				npc.lifeRegen -= 15;
				damage = 5;
			}
			if (afflicted)
			{
				drain = true;
				npc.lifeRegen -= 20;
				damage = 20;
			}
			if (iceCrush)
			{
				if (!npc.boss)
				{
					drain = true;
					float def = 2 + (npc.lifeMax / (npc.life * 1.5f));
					npc.lifeRegen -= (int)def;
					npc.damage = (int)def;
				}
				else if (npc.boss || npc.type == NPCID.DungeonGuardian)
				{
					drain = true;
					npc.lifeRegen -= 6;
					npc.damage = 3;
				}
			}
			if (Death)
			{
				drain = true;
				npc.lifeRegen -= 10000;
				damage = 10000;
			}
			if (SoulFlare)
			{
				drain = true;
				npc.lifeRegen -= 9;

			}
			if (felBrand)
			{
				drain = true;
				npc.lifeRegen -= 30;
				damage = 10;
			}
			if (spectre)
			{
				drain = true;
				npc.lifeRegen -= 20;
				damage = 5;
			}
			if (moonBurn)
			{
				drain = true;
				npc.lifeRegen -= 10;
				damage = 6;
			}
			if (sunBurn)
			{
				drain = true;
				npc.lifeRegen -= 6;
				damage = 3;
			}
			if (necrosis)
			{
				MyPlayer mp = Main.player[npc.target].GetSpiritPlayer();
				if (mp.KingSlayerFlask)
				{
					drain = true;
					npc.lifeRegen -= 36;
					damage = 12;
				}
				else
				{
					drain = true;
					npc.lifeRegen -= 30;
					damage = 10;
				}
			}
			if (holyBurn)
			{
				drain = true;
				npc.lifeRegen -= 25;
				damage = 3;
			}
			if (pestilence)
			{
				MyPlayer mp = Main.player[npc.target].GetSpiritPlayer();
				if (mp.KingSlayerFlask)
				{
					drain = true;
					npc.lifeRegen -= 5;
					damage = 3;
				}
				else
				{
					drain = true;
					npc.lifeRegen -= 3;
					damage = 3;
				}
			}
			if (blaze)
			{
				drain = true;
				npc.lifeRegen -= 4;
				damage = 2;
			}
			if (blaze1)
			{
				drain = true;
				npc.lifeRegen -= 20;
				damage = 2;
			}


			if (noDamage)
				damage -= damageBefore;
			if (drain && before > 0)
				npc.lifeRegen -= before;
		}

		public override void GetChat(NPC npc, ref string chat)
		{
			Player player = Main.player[Main.myPlayer];
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
			if (Main.halloween && !Main.dayTime && AllowTrickOrTreat(npc) && modPlayer.CanTrickOrTreat(npc))
			{
				if (npc.type == NPCID.Guide && !player.HasItem(Items.Halloween.CandyBag._type))
				{
					chat = "Take this bag; you can use it to store your Candy. \"How do I get candy?\", you ask? Try talking to the other villagers.";
					player.QuickSpawnItem(Items.Halloween.CandyBag._type);
				}
				else
				{
					chat = TrickOrTeat(modPlayer, npc);
					ItemUtils.DropCandy(player);
				}
			}

		}

		internal bool AllowTrickOrTreat(NPC npc)
		{
			if (npc.type == NPCID.OldMan)
				return false;
			return true;
		}

		internal string TrickOrTeat(MyPlayer player, NPC npc)
		{
			string name;
			int dialogue = Main.rand.Next(2);
			switch (npc.type)
			{
				case NPCID.Merchant:
					if (dialogue == 0)
						return "Oh, here's some candy. You have no idea how hard it was to get this.";
					else if (dialogue == 1)
						return "I can be greedy, but I like to be festive. Here you go!";
					else
						return "I'll give you this piece for free. I can't give you any promises about the next piece.";
				case NPCID.Nurse:
					if (dialogue == 0)
						return "Here, kiddo. Make sure there aren't any razorblades in there!";
					else if (dialogue == 1)
						return "It may not be the healithiest option, but candy is pretty nice. Take some.";
					else
						return "I'm pretty sure this candy that makes you healthier. Maybe. Don't quote me on this."; 
				case NPCID.ArmsDealer:
					if (dialogue == 0)
						if (player.player.HeldItem.type == ItemID.CandyCornRifle)
							return "Is that... it is! A Candy Corn Rifle! Here, I want you to have this for showing it to me.";
						else
							return "I hear there is a gun that shoots candy. Oh what I wouldn't give for one. What? Oh, yes, here's your candy.";
					else
						return "You wouldn't believe it. I asked for ammo, but my suppliers gave me candy instead! You want a piece?";
				case NPCID.Dryad:
					if (dialogue == 0)
						return "Do you have any idea what's in that candy? Here, this stuff is much better for you. I made it myself.";
					else if (dialogue == 1)
						return "Is it that time of year again? Time flies by so fast when you are as old as I am... Oh, here, have some candy.";
					else
						return "I wish I had candy seeds to sell you. Growing sweets would be far more sustainable than going door-to-door asking for them.";
				case (NPCID.Guide):
					if (dialogue == 0)
						return "Here. You may collect one piece of candy a night from every villager during halloween.";
					else
						return "Candy can be used during any season to get special effects.";
				case NPCID.Demolitionist:
					if (dialogue == 0)
						return "I was making a sugar rocket, and this was left over. Do you want some?";
					else
						return "Ach, this candy may or may not have explosives in it, I don't remember.";
				case NPCID.Clothier:
					if (dialogue == 0)
						return "My mama always told me candy was like life. Or was it a box? ... er, something like that. Here, take a piece.";
					else
						return "I'm quite the candy enthusiast. You want a piece?";
				case NPCID.GoblinTinkerer:
					if (dialogue == 0)
						return "I tried combining rocket boots and candy, but it didn't really work out. You want what's left?";
					else
						return "It turns out putting together every flavor of candy tastes pretty bad. I know, I'm disappointed too.";
				case NPCID.Wizard:
					if (dialogue == 0)
						return "I'm pretty sure this isn't enchanted candy, but I could make some if you want! No? Ok...";
					else if (dialogue == 1)
						return "I have some candy for you, but I could enchant it if you would li... No? Ok.";
					else
						return "Are you sure you don't want enchanted candy? It wouldn't be a bother if I just... No? Fine...";
				case NPCID.Mechanic:
					if (dialogue == 0)
						return "Don't mind the hydraulic fluid on the candy. In fact, consider it extra flavor.";
					else if (dialogue == 1)
						return "If you keep this candy in your pocket, it can monitor your heart rate, blood pressure, and tell how many steps you take!";
					else
						return "It turns out you can't make an engine powered by candy. Birds are fine, but candy? Too much, apparently.";
				case NPCID.SantaClaus:
					if (dialogue == 0)
						return "Something isn't right. This feels all wrong.";
					else
						return "Ho ho ho! 'Tis the season -- wait, 'tisn't the season! What am I doing here?";
				case NPCID.Truffle:
					if (dialogue == 0)
						return "Is this candy vegan? Of course not, you sicko!";
					else if ((name = NPC.GetFirstNPCNameOrNull(NPCID.Nurse)) != null)
						return name + " wanted some of these for her supply. I wonder what that was about?";
					else
						return "What do you mean there's mold on this piece? That's clearly fungus!";
				case NPCID.Steampunker:
					if (dialogue == 0)
						return "All hallow's eve, you say? Cor, I've got just the thing! Here, have some treacle!";
					else
						return "I suppose you want some puddings, yeah? Here you are, love!";
				case NPCID.DyeTrader:
					if (dialogue == 0)
						return "I put some special dyes in this sweet. It will make your tongue turn brilliant colors!";
					else
						return "It isn't about how it tastes... It's about how rich the colors look. Take a piece, why don't you?";
				case NPCID.PartyGirl:
					if (dialogue == 0)
						return "I love this time of year! Now I don't need an excuse give out free candy! Here, have a piece!";
					else
						return "Who ever said you needed drugs to party? Candy is waaaay better!";
				case NPCID.Cyborg:
					if (dialogue == 0)
						return "My calendar programming has determined that it is approximately Halloween; enjoy your sucrose based food!";
					else
						return "Sugar always seems to gum up my inaards. Here, hold this candy while I try and fix that.";
				case NPCID.Painter:
					if (dialogue == 0)
						return "I might've dripped a little paint on this candy, but it's probably lead-free. Hopefully.";
					else
						return "Oh, "+player.player.name+", you want candy? Let me get your portrait, then you can have some.";
				case NPCID.WitchDoctor:
					if (dialogue == 0)
						return "I decided not to give you lemon heads... or, should I say, lemon-flavored heads. Enjoy!";
					else
						return "Beware, "+ player.player.name +", for it is the season of ghouls and spirits. This edible talisman will protect you.";
				case NPCID.Pirate:
					if (dialogue == 0)
						return "Yo ho ho and a bottle of... candy. Take some!";
					else if (dialogue == 1)
						return "This candy cost me an arm and a leg. Enjoy that now, or it's to the plank with ye!";
					else
						return "Arrr, there's an old sayin' that goes \"Do what ye want, 'cause a pirate is free.\" I'd like to think that applies to eatin' candy as well!";
				case NPCID.Stylist:
					if (dialogue == 0)
						return "I usually save these for after haircuts, but go ahead and take a piece, darling.";
					else
						return "I've got plenty of candy, hon! Take as much as you want.";
				case NPCID.TravellingMerchant:
					if (dialogue == 0)
						return "I have rare candies from all over "+ Main.worldName +". Here, take some.";
					else
						return "I hear in far-off lands they have candy so sour it will melt your tongue! Unfortunately, I only have mundane candy for you.";
				case NPCID.Angler:
					if ((dialogue = Main.rand.Next(3)) == 0)
						return "What? You want some of MY candy? I think I have some ichorice here somewhere...";
					else if (dialogue == 1)
						return "This one came out of a fish. Here, you have it, I don't want it";
					else
						return "Dude, you don't ask a kid for candy. You just don't.";
				case NPCID.TaxCollector:
					if ((dialogue = Main.rand.Next(3)) == 0)
						return "Halloween? Bah, humbug! Take your candy and get out.";
					else if (dialogue == 1)
						return "You come to my door to take my sweets? Well go on then, take 'em!";
					else
						return "Here, have this. It's the cheapest brand I could find.";
				case NPCID.SkeletonMerchant:
					if (dialogue == 0)
						return "I'm feeling happy, it's my people's season! Take some candy!";
					else
						return "Did you know that rock candy doesn't actually grow underground? Oh, you did? Hmph.";
				case NPCID.DD2Bartender:
					if (dialogue == 0)
						return "I managed to find some ale-flavored candy! Maybe this world ain't so bad after all.";
					else
						return "Wiping a counter all day has made me appreciate the little things in life, like candy. Care for a piece?";
			}
			if (npc.type == Adventurer._type)
			{
				if (dialogue == 0)
					return "You wouldn't believe me if I told you I got this from a faraway kingdom made of CANDY! I promise it has an exquisite taste.";
				else
					return "I hear you can get more candy from the goodie bags that monsters hold. As if I needed an excuse to slay some zombies!";
			}
			else if (npc.type == LoneTrapper._type)
			{
				if (dialogue == 0)
					return "The only thing that makes me forget my suffering is candy. I suppose you can have some.";
				else
					return "Candy helps fill the aching void where my sould used to be. Maybe it can help you too.";
			}
			else if (npc.type == Martian._type)
			{
				if (dialogue == 0)
					return "I've determined through years of scientific analysis that this candy here is irresistible to any anyone who- hey, give it back!";
				else
					return "I'm unfamiliar with this holiday of yours. Am I supposed to give only treats? Why can I not trick as well?";
			}
			else if (npc.type == Rogue._type)
			{
				if (dialogue == 0)
					return "You want some candy? Here, catch!";
				else
					return "Hiyah! Candy attack! Oh, it's you. sorry.";
			}
			else if (npc.type == RuneWizard._type)
			{
				if (dialogue == 0)
					return "Behold! Enchanted candy! Enchantingly tasty, that is!";
				else
					return "Watch closely, for I shall channel the power of the spirits to summon... Candy!";
			}
			if (dialogue == 0)
				return "Hello, " + player.player.name + ". Take some candy!";
			else
				return "Here. I have some candy for you.";
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.Merchant)
			{
				shop.item[nextSlot].SetDefaults(base.mod.ItemType("Copper"), false);
				nextSlot++;
				if (Main.halloween && Main.halloween)
				{
					shop.item[nextSlot].SetDefaults(base.mod.ItemType("CandyBowl"), false);
					nextSlot++;
				}
			}
			else if (type == NPCID.Dryad)
			{
				shop.item[nextSlot].SetDefaults(base.mod.ItemType("Dryad"), false);
				nextSlot++;
				if (NPC.downedMoonlord && Main.halloween)
				{
					shop.item[nextSlot].SetDefaults(base.mod.ItemType("HalloweenGrass"), false);
				nextSlot++;
				}
			}
			else if (type == NPCID.Clothier)
			{
				shop.item[nextSlot].SetDefaults(Items.DonatorItems.TheCouch._type, false);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(410, false);
				shop.item[nextSlot].shopCustomPrice = 200000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(411, false);
				shop.item[nextSlot].shopCustomPrice = 200000;
				nextSlot++;
			}
			else if (type == NPCID.Steampunker)
			{
				shop.item[nextSlot].SetDefaults(base.mod.ItemType("Cog"), false);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("SpiritSolution"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("SoullessSolution"));
				nextSlot++;
			}
			else if (type == NPCID.PartyGirl)
			{
				if (NPC.downedMechBossAny)
				{
					shop.item[nextSlot].SetDefaults(mod.ItemType("PartyStarter"), false);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("SpiritPainting"), false);
					nextSlot++;
				}
			}
			else if (type == NPCID.WitchDoctor)
			{
				if (NPC.downedPlantBoss)
				{
					shop.item[nextSlot].SetDefaults(mod.ItemType("TikiArrow"));
					nextSlot++;
				}
			}
		}

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			bool surface = player.position.Y <= Main.worldSurface * 16 + NPC.sHeight;
			int activePlayers = 0;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
					activePlayers++;
			}

			if (MyWorld.BlueMoon && surface)
			{
				maxSpawns = (int)(maxSpawns * 2f);
				spawnRate = (int)(spawnRate * 0.19f);
			}
			if (TideWorld.TheTide && player.ZoneBeach)
			{
				maxSpawns = (int)(10 + 1.5f * activePlayers);
				spawnRate = 20;
			}
		}

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.spawnTileY <= Main.worldSurface)
            {
                if (MyWorld.BlueMoon && !Main.dayTime)
                    pool.Remove(0);
                if (TideWorld.TheTide && spawnInfo.player.ZoneBeach)
                    pool.Remove(0);
            }
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (spawnInfo.player.GetSpiritPlayer().ZoneAsteroid)
                {
                    pool.Clear();
                    pool.Add(mod.NPCType("DeepspaceHopper"), .85f);
                    pool.Add(mod.NPCType("AstralAmalgram"), 0.38f);
                    if (NPC.downedBoss2)
                    {
                        pool.Add(mod.NPCType("Mineroid"), 0.55f);
                    }
                    if (NPC.downedBoss3)
                    {
                        pool.Add(mod.NPCType("CogTrapperHead"), 0.25f);
                    }
                }
            }
            return;
        }
    

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			
			if (stormBurst)
			{
                knockback = 0;
                float before = knockback;
				knockback *= 2f;
				if (knockback > 0.5 && knockback < 2)
					knockback = 2f;
				else if (knockback > 8f)
					knockback = before > 8 ? before : 8;
			}
		}

		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			if (stormBurst && npc.knockBackResist > 0 && npc.velocity.LengthSquared() > 1)
			{
				for (int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, mod.DustType("Wind"));
					dust.customData = new Dusts.WindAnchor(npc.Center, npc.velocity, dust.position);
				}
			}
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			
			if (stormBurst)
			{
                knockback = 0;
                float before = knockback;
				knockback *= 2f;
				if (knockback > 0.5 && knockback < 2)
					knockback = 2f;
				else if (knockback > 8f)
					knockback = before > 8 ? before : 8;
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			if (stormBurst && npc.knockBackResist > 0 && npc.velocity.LengthSquared() > 1)
			{
				for (int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, mod.DustType("Wind"));
					dust.customData = new Dusts.WindAnchor(npc.Center, npc.velocity, dust.position);
				}
			}
		}


		private int GlyphsHeldBy(NPC boss)
		{
			if (boss.type == NPCID.KingSlime || boss.type == Boss.Scarabeus.Scarabeus._type)
				return 1;

			if (boss.type == NPCID.QueenBee || boss.type == NPCID.SkeletronHead ||
					boss.type == Boss.AncientFlyer._type || boss.type == Boss.SteamRaider.SteamRaiderHead._type)
				return 3;

			if (boss.type == NPCID.WallofFlesh)
				return 5;

			if (boss.type == NPCID.TheDestroyer || boss.type == Boss.Infernon.Infernon._type || boss.type == Boss.Infernon.InfernoSkull._type ||
					boss.type == NPCID.SkeletronPrime || boss.type == Boss.Dusking.Dusking._type || boss.type == Boss.SpiritCore.SpiritCore._type)
				return 4;

			if (boss.type == NPCID.Plantera || boss.type == NPCID.Golem || boss.type == NPCID.DukeFishron || boss.type == NPCID.CultistBoss ||
					boss.type == Boss.IlluminantMaster.IlluminantMaster._type || boss.type == Boss.Atlas.Atlas._type || boss.type == Boss.Overseer.Overseer._type)
				return 5;

			if (boss.type == NPCID.MoonLordCore)
				return 8;

			return 2;
		}

		public override void NPCLoot(NPC npc)
		{
			#region Glyph
			if (npc.boss && (npc.modNPC == null || npc.modNPC.bossBag > 0))
			{
				string name;
				if (npc.modNPC != null)
					name = npc.modNPC.mod.Name + ":" + npc.modNPC.GetType().Name;
				else
					name = "Terraria:" + npc.TypeName;

				bool droppedGlyphs = false;
				MyWorld.droppedGlyphs.TryGetValue(name, out droppedGlyphs);
				if (!droppedGlyphs)
				{
					int glyphs = GlyphsHeldBy(npc);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Glyph._type, glyphs * Main.ActivePlayersCount);
					MyWorld.droppedGlyphs[name] = true;
				}
			}
			else if (!npc.SpawnedFromStatue && npc.CanDamage() && Main.rand.Next(750) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Glyph._type);
			}
			#endregion
            /* 
			if (Main.rand.Next(40) == 1 && !npc.SpawnedFromStatue)
			{
				npc.DropItem(mod.ItemType("PrimordialMagic"));
			}
			if (npc.type == mod.NPCType("Reachman") || npc.type == mod.NPCType("ReachObserver") || npc.type == mod.NPCType("GrassVine") || npc.type == mod.NPCType("ReachShaman"))
			{
				if (Main.rand.Next(Main.expertMode ? 140 : 190) < 2)
					npc.DropItem(mod.ItemType("RootPod"));
				if (Main.hardMode && Main.rand.Next(Main.expertMode ? 250 : 350) < 2)
					npc.DropItem(mod.ItemType("RootPod"));
				if (NPC.downedMechBossAny && Main.rand.Next(Main.expertMode ? 400 : 550) < 2)
					npc.DropItem(mod.ItemType("RootPod"));
			}
			if (npc.type == NPCID.EyeofCthulhu)
			{
				if (Main.rand.Next(Main.expertMode ? 73 : 90) < 10)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DemonLens"));
				}
			}
			if (npc.type == NPCID.IceSlime || npc.type == NPCID.IceBat || npc.type == NPCID.UndeadViking)
			{
				if (Main.rand.Next(Main.expertMode ? 200 : 250) < 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostLotus"));
				}
			}
			if (npc.type == NPCID.GoblinSorcerer)
			{
				if (Main.rand.Next(Main.expertMode ? 80 : 100) < 7)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChaosEmber"));
				}
			}
			if (npc.type == NPCID.Tim)
			{
				if (Main.rand.Next(Main.expertMode ? 80 : 90) < 10)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChaosEmber"));
				}
			}
			if (npc.type == NPCID.FireImp || npc.type == NPCID.Demon)
			{
				if (Main.rand.Next(Main.expertMode ? 175 : 225) < 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FireLamp"));
				}
			}
			if (npc.type == NPCID.AngryBones || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigMuscle)
			{
				if (Main.rand.Next(Main.expertMode ? 200 : 250) < 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NecroticSkull"));
				}
				if (Main.hardMode && Main.rand.Next(Main.expertMode ? 225 : 275) < 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NecroticSkull"));
				}
			}
			if (npc.type == mod.NPCType("Clamper") || npc.type == mod.NPCType("GreenFinTrapper") || npc.type == mod.NPCType("MindFlayer") || npc.type == mod.NPCType("MurkTerror"))
			{
				if (Main.rand.Next(Main.expertMode ? 175 : 225) < 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TideStone"));
				}
				if (Main.hardMode && Main.rand.Next(Main.expertMode ? 225 : 275) < 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TideStone"));
				}
			}
			if (npc.type == NPCID.PirateCorsair || npc.type == NPCID.PirateDeadeye || npc.type == NPCID.PirateCrossbower)
			{
				if (Main.rand.Next(Main.expertMode ? 175 : 225) < 3)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedMedallion"));
				}
				if (NPC.downedMechBoss1 && Main.rand.Next(Main.expertMode ? 150 : 200) < 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedMedallion"));
				}
			}
			if (npc.type == NPCID.SkeletronPrime && NPC.downedMechBoss1 && NPC.downedMechBoss2)
			{
				if (Main.rand.Next(Main.expertMode ? 95 : 105) < 15)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BatteryCore"));
				}
			}
			if (npc.type == NPCID.Plantera)
			{
				if (Main.rand.Next(Main.expertMode ? 95 : 105) < 11)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PlanteraBloom"));
				}
			}
			if (npc.type == NPCID.Mothron && NPC.downedPlantBoss)
			{
				if (Main.rand.Next(Main.expertMode ? 95 : 105) < 12)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ApexFeather"));
				}
			}
			#endregion
			*/
			if (npc.type == 140)
			{
				if (Main.rand.Next(100) <= 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowSword"));
				}
				if (Main.rand.Next(100) <= 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowAxe"));
				}
				if (Main.rand.Next(100) <= 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowHammer"));
				}
				if (Main.rand.Next(100) <= 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowBody"));
				}
				if (Main.rand.Next(100) <= 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowHelmet"));
				}
				if (Main.rand.Next(100) <= 2)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowLeggings"));
				}
			}

			Player closest = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
            if (closest.GetSpiritPlayer().ZoneAsteroid && Main.rand.Next(50) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Blaster"));
            }
            if (npc.type == NPCID.DungeonSpirit && Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DungeonSpirit"), Main.rand.Next(2) + 1);
			}
			if (npc.type == 48 && Main.rand.Next(35) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarlightBow"));
			}
			if (npc.type == 48 && Main.rand.Next(35) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BreathOfTheZephyr"));
			}
			if (npc.type == 48 && Main.rand.Next(35) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HarpyBlade"));
			}
			if (npc.type == 48 && Main.rand.Next(4) == 0 && NPC.downedQueenBee)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Talon"), Main.rand.Next(2) + 2);
			}
			if (npc.type == NPCID.Tim)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TimScroll"));
			}
			if (npc.type == 206 && Main.rand.Next(30) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SnowGlobe);
			}
			if (npc.type == 127)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrintPrime"), Main.rand.Next(2) + 1);
			}
			if (npc.type == 125 || npc.type == 126)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlueprintTwins"), Main.rand.Next(2) + 1);
			}
			if (npc.type == 134)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrintProbe"), Main.rand.Next(2) + 1);
			}
			if (npc.type == NPCID.WallofFlesh && Main.rand.Next(3) >= 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BallOfFlesh"));
			}
			if (npc.type == 120 && Main.rand.Next(50) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChaosCrystal"));
			}

			if (npc.type == 491 || npc.type == 216)
			{
				if (Main.rand.Next(50) <= 5)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CaptainsRegards"));
				}
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PirateCrate"));
				if (Main.rand.Next(100) <= 6)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulSiphon"));
				}
			}
			if (npc.type == 398)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AccursedRelic"), Main.rand.Next(3, 6));
				if (Main.rand.Next(4) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HeartofMoon"));					
				}
			}
			if (npc.type == 213 && Main.rand.Next(50) <= 8)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ThrownCutlass"), Main.rand.Next(11, 17));
			}
			if (npc.type == 471)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowflameKnife"), Main.rand.Next(57, 87));
			}
			if (npc.type == NPCID.DD2Betsy)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DragonGrenade"), Main.rand.Next(90, 240));
			}
			if (npc.type == NPCID.DeadlySphere && Main.rand.Next(10) <= 7.3f)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DeadlySphere"), Main.rand.Next(8, 15));
			}
			if (npc.type == 409 && Main.rand.Next(40) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarlightStaff"));
			}
			if (npc.type == 383 && Main.rand.Next(23) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SaucerBeacon"));
			}
            if ((npc.type == 513 || npc.type == mod.NPCType("Chompasaur")) && Main.rand.Next(40) == 1 && MyWorld.downedScarabeus)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DesertSlab"));
            }
            if ((npc.type == 494 || npc.type == 495) && Main.rand.Next(35) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SnapperHat"));
            }
            if ((npc.type == 43 || npc.type == 56) && Main.rand.Next(50) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TrapperGlove"));
            }
            if (npc.type == 546 && Main.rand.Next(25) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Tumblesoul"));
            }
            if (npc.type == 39 && Main.rand.Next(16) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BoneFlail"));
			}
			if (npc.type == 199 && Main.rand.Next(500) == 1 || npc.type == 198 && Main.rand.Next(500) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SnakeStaff"));
			}
			if (npc.type == 477 && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				int dropCheck = Main.rand.Next(5);
				if (dropCheck == 0)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrokenStaff"));
				else if (dropCheck == 1)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrokenParts"));
			}
			if (npc.type == 543 && Main.rand.Next(33) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Thresher"));
			}
			if (npc.type == 32 && Main.rand.Next(30) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DungeonStaff"));
			}
			if (npc.type == 269 && Main.rand.Next(100) == 1 || npc.type == 270 && Main.rand.Next(100) == 1 || npc.type == 271 && Main.rand.Next(100) == 1 || npc.type == 272 && Main.rand.Next(100) == 1 || npc.type == 273 && Main.rand.Next(100) == 1 || npc.type == 274 && Main.rand.Next(100) == 1 || npc.type == 275 && Main.rand.Next(100) == 1 || npc.type == 276 && Main.rand.Next(100) == 1 || npc.type == 277 && Main.rand.Next(100) == 1 || npc.type == 278 && Main.rand.Next(100) == 1 || npc.type == 279 && Main.rand.Next(100) == 1 || npc.type == 280 && Main.rand.Next(100) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Swordsplosion"));
			}
			if (npc.type == 544 && Main.rand.Next(33) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Cookiecutter"));
			}
			if (npc.type == NPCID.ZombieEskimo || npc.type == NPCID.IceSlime || npc.type == NPCID.IceBat || npc.type == 197)
			{
				if (Main.rand.Next(2) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrigidFragment"), Main.rand.Next(1, 3));
			}
			if (npc.type == 184 || npc.type == 431)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrigidFragment"), Main.rand.Next(1, 3));
            }
			if ((npc.type == NPCID.ZombieEskimo || npc.type == NPCID.IceBat) && Main.rand.Next(33) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ThrallGate"));
			}
			if (npc.type == 545 && Main.rand.Next(33) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Mako"));
			}
			if (npc.type == 425 && Main.rand.Next(40) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StormPhaser"));
			}
			if (npc.type == 426 && Main.rand.Next(40) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DistortionSting"));
			}
            if (closest.GetSpiritPlayer().vitaStone)
            {
                if (!npc.friendly && npc.lifeMax > 5 && Main.rand.Next(20) == 1 && closest.statLife < closest.statLifeMax)
                {
                    if (Main.halloween)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,1734);
                    }
                    else
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 58);
                    }
                }
            }
            if (npc.type == 425 && Main.rand.Next(40) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StormPhaser"));
            }

            #region Iriazul
            // Bubble Shield dropping.
            if (martianMobs.Contains(npc.type))
			{
				if (Main.rand.Next(100) <= 2)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BubbleShield"));
			}

			// Essence Dropping
			if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && npc.lifeMax > 99)
			{
				int chance = npc.FindBuffIndex(mod.BuffType("EssenceTrap")) > -1 ? 4 : 8;
				if (Main.rand.Next(chance) == 0)
				{
					// Drop essence according to closest player location.
					if (closest.ZoneUnderworldHeight)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryEssence"));
					}
					else if (closest.ZoneBeach)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TidalEssence"));
					}
					else if (closest.ZoneUndergroundDesert)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DuneEssence"));
					}
					else if (closest.ZoneSnow && closest.position.Y > WorldGen.worldSurfaceLow)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IcyEssence"));
					}
					else if (closest.ZoneJungle && closest.position.Y > WorldGen.worldSurfaceLow)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrimevalEssence"));
					}
				}
			}
			#endregion

			//Vanilla NPCs
			if (npc.type == NPCID.ElfCopter && Main.rand.Next(Main.expertMode ? 25 : 50) < 3)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CandyRotor"));

			if (npc.type == NPCID.QueenBee && Main.rand.Next(Main.expertMode ? 10 : 20) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SweetThrow"));


			//Zone dependant
			if (closest.ZoneHoly && Main.rand.Next(200) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Mystic"), 1);

			if (closest.ZoneJungle && NPC.downedPlantBoss && npc.type != NPCID.Bee && Main.rand.Next(200) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Chaparral"), 1);

			if (npc.type == NPCID.Zombie || npc.type == NPCID.BaldZombie || npc.type == NPCID.SlimedZombie || npc.type == NPCID.SwampZombie || npc.type == NPCID.TwiggyZombie || npc.type == NPCID.ZombieRaincoat || npc.type == NPCID.PincushionZombie)
			{
				if (Main.rand.Next(2) == 0)
				{
					int amount = Main.rand.Next(1, 3);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OldLeather"), amount);
				}
			}
			if (npc.type == NPCID.MartianTurret || npc.type == NPCID.GigaZapper)
			{
				if (Main.rand.Next(98) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TeslaSpike"));
				}
			}
			if (npc.type == NPCID.SolarDrakomire || npc.type == NPCID.SolarDrakomireRider)
			{
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SolarRattle"));
				}
			}
			if (npc.type == 385 || npc.type == 382 || npc.type == 381)
			{
				if (Main.rand.Next(50) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EngineeringRod"));
				}
			}
			if (npc.type == NPCID.EyeofCthulhu && Main.rand.Next(5) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Eyeshot"));
			
			if (npc.type == 508 && Main.rand.Next(60) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AntlionClaws"));

			if (npc.type == 392 && Main.rand.Next(2) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Martian"));

			if (npc.type == NPCID.AnglerFish && Main.rand.Next(30) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Angelure"));

			if (npc.type == NPCID.ChaosElemental && Main.rand.Next(24) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Crystal"));


			if (npc.type == 439)
			{
				int item = 0;
				switch (Main.rand.Next(5))
				{
					case 0:
						item = mod.ItemType("Ancient");
						break;
					case 1:
						item = mod.ItemType("CultistScarf");
						break;
					case 2:
						item = mod.ItemType("CosmicHourglass");
						break;
					case 3:
						item = mod.ItemType("Tesseract");
						break;
					case 4:
						item = mod.ItemType("CultDagger");
						break;
				}				
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, item);
			}
			if (npc.type == 417 && Main.rand.Next(40) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Srollerang"));

			if (npc.type == 166 && Main.rand.Next(28) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Murk"));

			if (npc.type == 290 && Main.rand.Next(25) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EnchantedPaladinsHammerStaff"));

			if (npc.type == 268 && Main.rand.Next(50) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IchorPendant"));

			if (npc.type == 156 && Main.rand.Next(40) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryPendant"));

			if (npc.type == 260 || npc.type == 257)
			{
				if (Main.rand.Next(25) == 0 && Main.hardMode)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Fungus"));
				}
			}
			if (npc.type == 370)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Typhoon"));

			if (npc.type == 62 && NPC.downedBoss3 && Main.rand.Next(4) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarvedRock"), Main.rand.Next(1) + 2);

			if (npc.type == 31 && Main.rand.Next(45) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlazingWheel"));

			if (npc.type == 170 || npc.type == 171 || npc.type == 180)
			{
				if (Main.rand.Next(18) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PigronStaff"));
				}
			}
			if (npc.type == 113 && Main.rand.Next(2) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FamineScepter"));

			if (npc.type == 113 && Main.rand.Next(4) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ThrowerEmblem"));

			if (npc.type == 24 && Main.rand.Next(18) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheFireball"));

			if (npc.type == 101 && Main.rand.Next(50) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedPendant"));

			if (npc.type == 29 && Main.rand.Next(20) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChaosBall"));

			if (npc.type == NPCID.DemonEye || npc.type == NPCID.DemonEye2 || npc.type == NPCID.DemonEyeOwl || npc.type == NPCID.DemonEyeSpaceship)
			{
				if (Main.rand.Next(25) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MagnifyingGlass"), 1);
				}
			}
			if (closest.GetSpiritPlayer().floranSet)
			{
				if (Main.rand.Next(13) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RawMeat"), 1);
				}
			}
			if (npc.type == NPCID.EaterofSouls && Main.rand.Next(28) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EaterStaff"), 1);

			if (npc.type == NPCID.EaterofWorldsTail && !NPC.AnyNPCs(NPCID.EaterofWorldsHead) && !NPC.AnyNPCs(NPCID.EaterofWorldsBody))
			{
				if (Main.rand.Next(3) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EoWSpear"), 1);
				}
			}
			if (npc.type == NPCID.FaceMonster && Main.rand.Next(28) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CreeperStaff"), 1);


			// Donator Items

			//Folv
			if (npc.type == mod.NPCType("Scarabeus") || npc.type == mod.NPCType("AncientFlyer") || npc.type == mod.NPCType("SteamRaiderHead"))
			{
				if (Main.rand.Next(10) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FolvBlade1"), 1);
				}
			}
			if (npc.type == mod.NPCType("Infernon") || npc.type == mod.NPCType("Dusking"))
			{
				if (Main.rand.Next(8) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Whetstone"), 1);
				}
			}
			if (npc.type == mod.NPCType("IlluminantMaster") || npc.type == mod.NPCType("Atlas"))
			{
				if (Main.rand.Next(4) == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Enchantment"), 1);
				}
			}
			if (npc.type == mod.NPCType("Overseer"))
			{
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Hilt"), 1);
				}
			}
			//End Folv

			if (npc.type == 156)
			{
				if (Main.rand.Next(80) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CombatShotgun"));
				}
			}
			if (npc.type == NPCID.LihzahrdCrawler || npc.type == NPCID.Lihzahrd)
			{
				if (Main.rand.Next(6) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SunShard"));
				}
			}
			if (npc.type == NPCID.Eyezor)
			{
				if (Main.rand.Next(6) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Eyezor"));
				}
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (sFracture && Main.rand.Next(2) == 0)
				Dust.NewDust(npc.position, npc.width, npc.height, 133, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);

			if (felBrand && Main.rand.Next(2) == 0)
				Dust.NewDust(npc.position, npc.width, npc.height, 75, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 75);
			if (vineTrap)
			{	
				drawColor = new Color(103, 138, 84);
			}
			if (clatterPierce)
			{	
				drawColor = new Color(115, 80, 57);
			}
            if (tracked)
            {
                drawColor = new Color(135, 245, 76);
            }
        }

	}
}
