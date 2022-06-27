using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Glyph;
using SpiritMod.Dusts;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.DonatorItems;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Glyphs;
using SpiritMod.Items.Halloween;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Sets.FrigidSet;
using SpiritMod.Items.Sets.FlailsMisc.JadeDao;
using SpiritMod.Items.Sets.SwordsMisc.BladeOfTheDragon;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Items.Sets.MoonWizardDrops;
using SpiritMod.NPCs.Boss;
using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.NPCs.Boss.Dusking;
using SpiritMod.NPCs.Boss.Infernon;
using SpiritMod.NPCs.Boss.ReachBoss;
using SpiritMod.NPCs.Boss.Scarabeus;
using SpiritMod.NPCs.Boss.SteamRaider;
using SpiritMod.NPCs.Critters.Algae;
using SpiritMod.Items.Sets.SlingHammerSubclass;
using SpiritMod.NPCs.Town;
using SpiritMod.Projectiles.Arrow;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SpiritMod.Items.Ammo.Rocket.Warhead;
using SpiritMod.Projectiles.Summon.SacrificialDagger;
using Terraria.Audio;
using SpiritMod.Items.Sets.SummonsMisc.PigronStaff;
using SpiritMod.Items.Sets.LaunchersMisc.Liberty;
using Terraria.Localization;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Walls.Natural;
using SpiritMod.Utilities;
using SpiritMod.Items.Placeable.Furniture.Paintings;
using SpiritMod.Items.Sets.PirateStuff;
using SpiritMod.Items.Accessory.MageTree;
using SpiritMod.Items.Sets.ReefhunterSet;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs
{
	public class GNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		#region Fields
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

		public int summonTag;
		public bool sacrificialDaggerBuff;

		public bool felBrand = false;
		public bool spectre = false;
		public bool soulBurn = false;
		public bool Stopped = false;
		public bool SoulFlare = false;
		public bool afflicted = false;
		public bool sunBurn = false;
		public bool starDestiny = false;
		public int bloodInfusion = 0;
		public bool bloodInfused = false;
		public bool death = false;
		public bool iceCrush = false;
		public bool pestilence = false;
		public bool moonBurn = false;
		public bool holyBurn = false;

		public bool DoomDestiny = false;

		public bool sFracture = false;
		public bool Etrap = false;
		public bool necrosis = false;
		public bool blaze = false;
		public bool blaze1 = false;
		#endregion

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
			bloodInfused = false;
			sanguineBleed = false;
			unholyPlague = false;
			frostChill = false;
			stormBurst = false;
			vineTrap = false;
			clatterPierce = false;
			DoomDestiny = false;
			sFracture = false;
			death = false;
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

			summonTag = 0;
			sacrificialDaggerBuff = false;

			felBrand = false;
			spectre = false;
			holyBurn = false;
			pestilence = false;
			//	bloodInfusion = false;
		}

		public override bool PreAI(NPC npc)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (bloodInfusion > 150)
				{
					bloodInfusion = 0;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<FlayedExplosion>(), 25, 0, Main.myPlayer);
				}
			}

			Player player = Main.player[Main.myPlayer];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			Vector2 dist = npc.position - player.position;
			if (Main.netMode != NetmodeID.Server)
			{
				if (player.GetModPlayer<MyPlayer>().HellGaze == true && Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 160 && Main.rand.Next(80) == 1 && !npc.friendly)
					npc.AddBuff(24, 130, false);
				dist = npc.Center - new Vector2(modPlayer.clockX, modPlayer.clockY);
				if (player.GetModPlayer<MyPlayer>().clockActive == true && Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 175 && !npc.friendly)
					npc.AddBuff(ModContent.BuffType<Stopped>(), 3);
			}

			if (Main.netMode != NetmodeID.Server)
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
			return true;
		}

		public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if ((npc.type == NPCID.GraniteFlyer || npc.type == NPCID.GraniteGolem) && NPC.downedBoss2 && Main.netMode != NetmodeID.MultiplayerClient && npc.life <= 0 && Main.rand.Next(3) == 0)
			{
				SoundEngine.PlaySound(new LegacySoundStyle(2, 109));
				{
					for (int i = 0; i < 20; i++)
					{
						int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != npc.Center)
							Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
					Vector2 spawnAt = npc.Center + new Vector2(0f, npc.height / 2f);
					NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y,ModContent.NPCType<NPCs.CracklingCore.GraniteCore>());
				}
			}
			if (npc.life <= 0 && npc.FindBuffIndex(ModContent.BuffType<WanderingPlague>()) >= 0)
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
				if (npc.FindBuffIndex(ModContent.BuffType<AngelLight>()) < 0)
				{
					angelLightStacks = 0;
					return;
				}
			}
			if (angelWrathStacks > 0)
			{
				if (npc.FindBuffIndex(ModContent.BuffType<AngelWrath>()) < 0)
				{
					angelWrathStacks = 0;
					return;
				}
			}

			#region Iriazul
			if (fireStacks > 0)
			{
				if (npc.FindBuffIndex(ModContent.BuffType<StackingFireBuff>()) < 0)
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
				if (npc.FindBuffIndex(ModContent.BuffType<AcidBurn>()) < 0)
				{
					acidBurnStacks = 0;
					return;
				}

				drain = true;
				npc.lifeRegen -= 3 * acidBurnStacks;
				damage = Math.Max(damage, acidBurnStacks * 2);
			}
			if (nebulaFlameStacks > 0)
			{
				if (npc.FindBuffIndex(ModContent.BuffType<NebulaFlame>()) < 0)
				{
					nebulaFlameStacks = 0;
					return;
				}

				drain = true;
				npc.lifeRegen -= 16;
				damage = Math.Max(damage, fireStacks * 20);
			}
			#endregion

			if (voidStacks > 0)
			{
				damage += 5 + 5 * (voidStacks / VoidGlyph.DELAY);
				npc.lifeRegen -= 20 + 20 * voidStacks / VoidGlyph.DELAY;
			}

			if (sanguineBleed)
			{
				damage += 4;
				npc.lifeRegen -= 16;
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
					damage = 10;
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
					damage = (int)def;
				}
				else if (npc.boss || npc.type == NPCID.DungeonGuardian)
				{
					drain = true;
					npc.lifeRegen -= 6;
					damage = 3;
				}
			}

			if (death)
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

			if (holyBurn)
			{
				drain = true;
				npc.lifeRegen -= 25;
				damage = 3;
			}
			if (pestilence)
			{
				drain = true;
				npc.lifeRegen -= 3;
				damage = 3;
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
			Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			if (Main.halloween && !Main.dayTime && AllowTrickOrTreat(npc) && modPlayer.CanTrickOrTreat(npc))
			{
				if (npc.type == NPCID.Guide && !player.HasItem(ModContent.ItemType<CandyBag>()))
				{
					chat = "Take this bag; you can use it to store your Candy. \"How do I get candy?\", you ask? Try talking to the other villagers.";
					player.QuickSpawnItem(ModContent.ItemType<CandyBag>());
				}
				else
				{
					chat = TrickOrTreat(modPlayer, npc);
					ItemUtils.DropCandy(player);
				}
			}
		}

		internal bool AllowTrickOrTreat(NPC npc) => npc.type != NPCID.OldMan && npc.homeTileX != -1 && npc.homeTileY != -1 && !ModContent.GetInstance<SpiritMod>().NPCCandyBlacklist.Contains(npc.type);

		internal string TrickOrTreat(MyPlayer player, NPC npc)
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
						if (player.Player.HeldItem.type == ItemID.CandyCornRifle)
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
						return "Oh, " + player.Player.name + ", you want candy? Let me get your portrait, then you can have some.";
				case NPCID.WitchDoctor:
					if (dialogue == 0)
						return "I decided not to give you lemon heads... or, should I say, lemon-flavored heads. Enjoy!";
					else
						return "Beware, " + player.Player.name + ", for it is the season of ghouls and spirits. This edible talisman will protect you.";
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
						return "I have rare candies from all over " + Main.worldName + ". Here, take some.";
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
			if (npc.type == ModContent.NPCType<Adventurer>())
			{
				if (dialogue == 0)
					return "You wouldn't believe me if I told you I got this from a faraway kingdom made of CANDY! I promise it has an exquisite taste.";
				else
					return "I hear you can get more candy from the goodie bags that monsters hold. As if I needed an excuse to slay some zombies!";
			}
			else if (npc.type == ModContent.NPCType<Rogue>())
			{
				if (dialogue == 0)
					return "You want some candy? Here, catch!";
				else
					return "Hiyah! Candy attack! Oh, it's you. sorry.";
			}
			else if (npc.type == ModContent.NPCType<RuneWizard>())
			{
				if (dialogue == 0)
					return "Behold! Enchanted candy! Enchantingly tasty, that is!";
				else
					return "Watch closely, for I shall channel the power of the spirits to summon... Candy!";

			}
			else if (npc.type == ModContent.NPCType<Gambler>())
			{
				if (dialogue == 0)
					return "Reach into the bowl. You never know what you'll pull out";
				else
					return "I'll trade you any piece of candy for a random pie- no? Ok";
			}

			if (dialogue == 0)
				return "Hello, " + player.Player.name + ". Take some candy!";
			else
				return "Here, I have some candy for you.";
		}

		public override Color? GetAlpha(NPC npc, Color drawColor)
		{
			if (npc.HasBuff(ModContent.BuffType<TopazMarked>()))
				return Color.Lerp(base.GetAlpha(npc, drawColor) ?? Color.Transparent, new Color(158, 255, 253), 0.75f);
			return null;
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.Merchant)
			{
				if (Main.halloween)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<CandyBowl>(), false);
			}
			else if (type == NPCID.ArmsDealer)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Ammo.Bullet.RubberBullet>(), false);
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Warhead>(), false);
				if (Main.player.Where(x => x.HasItem(ModContent.ItemType<Moonshot>())).Any())
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<TinyLunazoaItem>(), false);
			}
			else if (type == NPCID.Cyborg)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Armor.FreemanSet.FreemanHead>(), false);
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Armor.FreemanSet.FreemanBody>(), false);
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Armor.FreemanSet.FreemanLegs>(), false);
			}
			else if (type == NPCID.Clothier)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<TheCouch>(), false);
				shop.item[nextSlot].SetDefaults(410, false);
				shop.item[nextSlot++].shopCustomPrice = 200000;
				shop.item[nextSlot].SetDefaults(411, false);
				shop.item[nextSlot++].shopCustomPrice = 200000;

				if (MyWorld.downedRaider)
				{
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Armor.CommandoSet.CommandoHead>(), false);
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Armor.CommandoSet.CommandoBody>(), false);
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Armor.CommandoSet.CommandoLegs>(), false);
				}
			}
			else if (type == NPCID.Dryad)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Placeable.MusicBox.TranquilWindsBox>(), false);
				if (NPC.downedMoonlord && Main.halloween)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Placeable.Tiles.HalloweenGrass>(), false);

				if (Main.LocalPlayer.GetSpiritPlayer().ZoneReach)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Placeable.Tiles.BriarGrassSeeds>(), false);
			}
			else if (type == NPCID.Wizard)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SurrenderBell>(), false);
			}
			else if (type == NPCID.Steampunker)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Ammo.SpiritSolution>());
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Ammo.OliveSolution>());
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Ammo.SoullessSolution>());
			}
			else if (type == NPCID.PartyGirl)
			{
				if (NPC.downedMechBossAny)
				{
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.GunsMisc.Partystarter.PartyStarter>(), false);
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.MusicBox.NeonMusicBox>(), false);
					shop.item[nextSlot++].shopCustomPrice = 50000;
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SpiritPainting>(), false);
				}
			}
			else if (type == NPCID.WitchDoctor)
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.ClubSubclass.Macuahuitl>());
			else if (type == NPCID.Painter)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Canvas>(), false);
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<FloppaPainting>(), false);

				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SatchelReward>(), false);

				if (ModContent.GetInstance<StarjinxEvent.StarjinxEventWorld>().StarjinxDefeated)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ScrunklyPaintingItem>(), false);
			}
			else if (type == NPCID.Demolitionist)
			{
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<LibertyItem>(), false);
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Warhead>(), false);
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ShortFuse>(), false);
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<LongFuse>(), false);
			}
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (Main.rand.NextBool(8) && NPC.downedPlantBoss)
				shop[nextSlot++] = ModContent.ItemType<JadeDao>();
			if (Main.rand.NextBool(8) && NPC.downedPlantBoss)
				shop[nextSlot++] = ModContent.ItemType<BladeOfTheDragon>();
		}

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			bool surface = player.position.Y <= Main.worldSurface * 16 + NPC.sHeight;
			int activePlayers = 0;

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				if (Main.player[i].active)
					activePlayers++;
			}

			if (player.GetSpiritPlayer().ZoneSpirit)
			{
				spawnRate = (int)(spawnRate * 0.73f);
				maxSpawns = (int)(maxSpawns * 1.1f);
			}

			if (MyWorld.BlueMoon && surface)
			{
				maxSpawns = (int)(maxSpawns * 1.1f);
				spawnRate = (int)(spawnRate * 0.4f);
			}

			if (MyWorld.jellySky && (player.ZoneOverworldHeight || player.ZoneSkyHeight))
			{
				maxSpawns = (int)(maxSpawns * 1.18f);
				spawnRate = 2;
			}

			if (player.GetSpiritPlayer().oliveBranchBuff)
			{
				spawnRate = (int)(spawnRate * 4.5f);
				maxSpawns = (int)(maxSpawns * .5f);
			}
		}

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;

			if (MyWorld.calmNight)
			{
				if (spawnInfo.Invasion || spawnInfo.Sky || MyWorld.BlueMoon) return; //if invasion or in sky
				if (Main.eclipse || Main.bloodMoon) return; //if eclipse or blood moon
				if (!player.ZoneOverworldHeight) return; //if not in overworld
				if (player.ZoneMeteor || player.ZoneRockLayerHeight || player.ZoneDungeon || player.ZoneBeach || player.ZoneCorrupt || player.ZoneCrimson || player.ZoneJungle || player.ZoneHallow || spawnInfo.Player.GetSpiritPlayer().ZoneReach || spawnInfo.Player.GetSpiritPlayer().ZoneSpirit) return; //if in wrong biome

				pool.Clear();
			}

			if (spawnInfo.SpawnTileY <= Main.worldSurface && MyWorld.BlueMoon && !Main.dayTime)
				pool.Remove(0);

			if (spawnInfo.Player.GetSpiritPlayer().ZoneAsteroid)
			{
				if (!spawnInfo.PlayerSafe)
				{
					pool.Clear();
					pool.Add(ModContent.NPCType<Shockhopper.DeepspaceHopper>(), .35f);
					pool.Add(ModContent.NPCType<AstralAmalgam.AstralAmalgram>(), 0.16f);

					if (NPC.downedBoss2)
						pool.Add(ModContent.NPCType<Orbitite.Mineroid>(), 0.3f);
				}

				pool.Add(ModContent.NPCType<Gloop.GloopGloop>(), 0.24f);

				if (NPC.downedBoss3)
					pool.Add(ModContent.NPCType<Starfarer.CogTrapperHead>(), 0.45f);

				if (NPC.downedBoss1 || NPC.downedBoss3 || NPC.downedBoss3)
					if (!NPC.AnyNPCs(ModContent.NPCType<MoonjellyEvent.DistressJelly>()))
						pool.Add(ModContent.NPCType<MoonjellyEvent.DistressJelly>(), .055f);
			}

			if (MyWorld.jellySky && (spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneSkyHeight))
			{
				pool.Add(ModContent.NPCType<MoonjellyEvent.TinyLunazoa>(), 9.35f);
				pool.Add(ModContent.NPCType<MoonjellyEvent.ExplodingMoonjelly>(), 8.35f);
				pool.Add(ModContent.NPCType<MoonjellyEvent.MoonlightPreserver>(), 3.25f);

				if (!NPC.AnyNPCs(ModContent.NPCType<MoonjellyEvent.MoonjellyGiant>()))
					pool.Add(ModContent.NPCType<MoonjellyEvent.MoonjellyGiant>(), .85f);

				if (!NPC.AnyNPCs(ModContent.NPCType<MoonjellyEvent.DreamlightJelly>()))
					pool.Add(ModContent.NPCType<MoonjellyEvent.DreamlightJelly>(), .85f);
			}

			if (spawnInfo.Player.active && spawnInfo.Player.ZoneBeach && MyWorld.luminousOcean && !Main.dayTime)
			{
				pool.Clear();
				if (spawnInfo.Water)
				{
					if (MyWorld.luminousType == 1)
						pool.Add(ModContent.NPCType<GreenAlgae2>(), 3f);
					else if (MyWorld.luminousType == 2)
						pool.Add(ModContent.NPCType<BlueAlgae2>(), 3f);
					else if (MyWorld.luminousType == 3)
						pool.Add(ModContent.NPCType<PurpleAlgae2>(), 3f);
				}
			}
			return;
		}

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (stormBurst)
			{
				float before = knockback;
				knockback *= 1.5f;

				if (knockback > 8f)
					knockback = before > 8 ? before : 8;
			}
		}

		public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			if (stormBurst && npc.knockBackResist > 0 && npc.velocity.LengthSquared() > 1)
			{
				for (int i = 0; i < 8; i++)
				{
					var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<Wind>());
					dust.customData = new WindAnchor(npc.Center, npc.velocity, dust.position);
				}
			}
		}

		public override void ModifyHitByProjectile(NPC target, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
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

			bool summon = (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type] || projectile.sentry);
			if (summon)
				damage += summonTag;

			if (sacrificialDaggerBuff && summon && projectile.type != ModContent.ProjectileType<SacrificialDaggerProj>() && projectile.type != ModContent.ProjectileType<SacrificialDaggerProjectile>())
			{
				if (Main.rand.NextBool(4))
				{
					if (Main.netMode != NetmodeID.Server)
						SoundEngine.PlaySound(new LegacySoundStyle(SoundID.Item, 71).WithPitchVariance(0.2f).WithVolume(0.5f), target.Center);

					int direction = target.position.X > Main.player[projectile.owner].position.X ? 1 : -1;

					Vector2 randPos = target.Center + new Vector2(direction, 0).RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(70, 121);
					var dir = Vector2.Normalize(target.Center - randPos) * 6;

					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(randPos.X, randPos.Y, dir.X, dir.Y, ModContent.ProjectileType<SacrificialDaggerProjectile>(), (int)(damage * 0.75f), 0, projectile.owner);

					DustHelper.DrawTriangle(target.Center, 173, 5, 1.5f, 1f);
				}
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			if (stormBurst && npc.knockBackResist > 0 && npc.velocity.LengthSquared() > 1)
			{
				for (int i = 0; i < 8; i++)
				{
					var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<Wind>());
					dust.customData = new WindAnchor(npc.Center, npc.velocity, dust.position);
				}
			}
		}

		private int GlyphsHeldBy(NPC boss)
		{
			if (boss.type == NPCID.KingSlime || boss.type == ModContent.NPCType<Scarabeus>() || boss.type == NPCID.EyeofCthulhu)
				return 2;
			else if (boss.type == ModContent.NPCType<ReachBoss1>() || boss.type == NPCID.QueenBee || boss.type == NPCID.SkeletronHead || boss.type == ModContent.NPCType<AncientFlyer>() || boss.type == ModContent.NPCType<SteamRaiderHead>())
				return 3;
			else if (boss.type == NPCID.WallofFlesh)
				return 5;
			else if (boss.type == NPCID.TheDestroyer || boss.type == ModContent.NPCType<Infernon>() || boss.type == ModContent.NPCType<InfernoSkull>() || boss.type == NPCID.SkeletronPrime || boss.type == ModContent.NPCType<Dusking>())
				return 4;
			else if (boss.type == NPCID.Plantera || boss.type == NPCID.Golem || boss.type == NPCID.DukeFishron || boss.type == NPCID.CultistBoss || boss.type == ModContent.NPCType<Atlas>())
				return 5;
			else if (boss.type == NPCID.MoonLordCore)
				return 8;

			return 2;
		}

		public override void OnKill(NPC npc)
		{
			Player closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];

			if (NPC.killCount[Item.NPCtoBanner(npc.BannerID())] == 50)
				SoundEngine.PlaySound(SoundLoader.customSoundType, closest.position, Mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/BannerSfx"));

			if (bloodInfused)
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<FlayedExplosion>(), 25, 0, Main.myPlayer);

			if (closest.GetSpiritPlayer().wayfarerSet)
				closest.AddBuff(ModContent.BuffType<Buffs.Armor.ExplorerFight>(), 240);

			#region Glyph
			if (npc.boss && (npc.ModNPC == null || npc.ModNPC.bossBag > 0))
			{
				string name = npc.ModNPC != null ? npc.ModNPC.Mod.Name + ":" + npc.ModNPC.GetType().Name : "Terraria:" + npc.TypeName;

				MyWorld.droppedGlyphs.TryGetValue(name, out bool droppedGlyphs);
				if (!droppedGlyphs)
				{
					int glyphs = GlyphsHeldBy(npc);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Glyph>(), glyphs * Main.ActivePlayersCount);
					MyWorld.droppedGlyphs[name] = true;
				}
			}
			else if (!npc.SpawnedFromStatue && npc.CanDamage() && Main.rand.Next(750) == 0 && npc.type != ModContent.NPCType<ExplodingSpore>())
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Glyph>());
			#endregion

			ItemLoot(npc, closest);

			bool lastTwin = (npc.type == NPCID.Retinazer && !NPC.AnyNPCs(NPCID.Spazmatism)) || (npc.type == NPCID.Spazmatism && !NPC.AnyNPCs(NPCID.Retinazer));
			if ((npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer || lastTwin) && !MyWorld.spiritBiome)
				SpawnSpiritBiome();
		}

		private void ItemLoot(NPC npc, Player player)
		{
			if (player.GetSpiritPlayer().vitaStone)
			{
				if (!npc.friendly && npc.lifeMax > 5 && Main.rand.Next(9) == 1 && player.statLife < player.statLifeMax)
				{
					if (Main.halloween)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 1734);
					else
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 58);
				}
			}

			if (player.HasAccessory<ArcaneNecklace>() && Main.rand.Next(5) == 0 && !npc.friendly && player.HeldItem.magic && player.statMana < player.statManaMax2)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Star);

			if (player.GetSpiritPlayer().ZoneAsteroid && Main.rand.Next(50) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.GunsMisc.Blaster.Blaster>());

			if (npc.type == NPCID.PirateShip || npc.type == NPCID.PirateCaptain)
			{
				if (Main.rand.Next(50) <= 2)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.GunsMisc.CaptainsRegards.CaptainsRegards>());
				if (npc.type == NPCID.PirateShip && Main.rand.NextBool(3))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PirateCrate>());
			}

			if (npc.type == NPCID.Demon && NPC.downedBoss3 && Main.rand.Next(4) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.SlagSet.CarvedRock>(), Main.rand.Next(1) + 2);

			if (player.GetSpiritPlayer().floranSet && Main.rand.Next(9) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawMeat>(), 1);

			if (npc.type == NPCID.EaterofWorldsTail && !NPC.AnyNPCs(NPCID.EaterofWorldsHead) && !NPC.AnyNPCs(NPCID.EaterofWorldsBody) && Main.rand.Next(3) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.SpearsMisc.RotScourge.EoWSpear>(), 1);

			if (!npc.SpawnedFromStatue)
			{
				DropLoot(80, 60, ModContent.ItemType<InfernalPact>(), npc, NPCID.Lavabat, NPCID.RedDevil);
				DropLoot(150, 150, ModContent.ItemType<IceVikingSculpture>(), npc, NPCID.UndeadViking);
				DropLoot(150, 150, ModContent.ItemType<IceFlinxSculpture>(), npc, NPCID.SnowFlinx);
				DropLoot(150, 150, ModContent.ItemType<IceBatSculpture>(), npc, NPCID.IceBat);
				DropLoot(120, 110, ModContent.ItemType<Items.Accessory.RabbitFoot.Rabbit_Foot>(), npc, NPCID.Bunny);
				DropLoot(150, 150, ModContent.ItemType<WinterbornSculpture>(), npc, ModContent.NPCType<Winterborn.WinterbornMelee>(), ModContent.NPCType<WinterbornHerald.WinterbornMagic>());
				DropLoot(5, 5, ModContent.ItemType<Items.Consumable.Potion.BottomlessHealingPotion>(), npc, NPCID.Mimic);
				DropLoot(5, 5, ModContent.ItemType<Items.Sets.MagicMisc.MagicDeck.MagicDeck>(), npc, NPCID.Mimic);

				if (player.ZoneSnow)
					DropLoot(100, 100, ModContent.ItemType<IceWheezerSculpture>(), npc, ModContent.NPCType<Wheezer.Wheezer>());
			}

			if (MyWorld.downedScarabeus)
				DropLoot(40, 40, ModContent.ItemType<DesertSlab>(), npc, NPCID.TombCrawlerHead);

			DropLoot(1, 1, ModContent.ItemType<Glyph>(), npc, NPCID.Tim, NPCID.RuneWizard);
			DropLoot(100, 100, ModContent.ItemType<Items.Consumable.Potion.BottomlessAle>(), npc, NPCID.Pixie);
			DropLoot(250, 200, ModContent.ItemType<Items.Accessory.Ukelele.Ukelele>(), npc, NPCID.AngryNimbus);
			DropLoot(100, 95, ModContent.ItemType<Items.Accessory.BowSummonItem.BowSummonItem>(), npc, NPCID.GoblinArcher);
			DropLoot(100, 100, ModContent.ItemType<Items.Accessory.FlyingFishFin.Flying_Fish_Fin>(), npc, NPCID.FlyingFish);
			DropLoot(3, 3, ModContent.ItemType<Items.Accessory.SeaSnailVenom.Sea_Snail_Poison>(), npc, NPCID.SeaSnail);
			DropLoot(100, 100, ModContent.ItemType<PossessedHammer>(), npc, NPCID.PossessedArmor);
			DropLoot(90, 75, ModContent.ItemType<GoblinSorcererStaff>(), npc, NPCID.GoblinSorcerer);
			DropLoot(110, 95, ModContent.ItemType<Items.Sets.ClubSubclass.BoneClub>(), npc, NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigMuscle);
			DropLoot(45, 45, ModContent.ItemType<Items.Sets.BowsMisc.StarSpray.StarlightBow>(), npc, NPCID.Harpy);
			DropLoot(45, 45, ModContent.ItemType<Items.Sets.MagicMisc.ZephyrBreath.BreathOfTheZephyr>(), npc, NPCID.Harpy);
			DropLoot(1, 1, ModContent.ItemType<TimScroll>(), npc, NPCID.Tim);
			DropLoot(30, 30, ItemID.SnowGlobe, npc, NPCID.IcyMerman);
			DropLoot(1, 1, ModContent.ItemType<PrintPrime>(), Main.rand.Next(2) + 1, npc, NPCID.SkeletronPrime);
			DropLoot(1, 1, ModContent.ItemType<BlueprintTwins>(), Main.rand.Next(2) + 1, npc, NPCID.Retinazer, NPCID.Spazmatism);
			DropLoot(1, 1, ModContent.ItemType<PrintProbe>(), Main.rand.Next(2) + 1, npc, NPCID.TheDestroyer);
			DropLoot(50, 50, ModContent.ItemType<ChaosCrystal>(), npc, NPCID.ChaosElemental);
			DropLoot(100, 80, ModContent.ItemType<Items.Weapon.Thrown.PiecesOfEight.PiecesOfEight>(), npc, NPCID.PirateDeckhand);
			DropLoot(23, 23, ModContent.ItemType<SaucerBeacon>(), npc, NPCID.MartianOfficer);
			DropLoot(35, 35, ModContent.ItemType<SnapperHat>(), npc, NPCID.Crawdad, NPCID.Crawdad2);
			DropLoot(50, 50, ModContent.ItemType<TrapperGlove>(), npc, NPCID.ManEater);
			DropLoot(500, 500, ModContent.ItemType<SnakeStaff>(), npc, NPCID.Lihzahrd, NPCID.LihzahrdCrawler);
			DropLoot(42, 42, ModContent.ItemType<Items.Sets.MagicMisc.TerraStaffTree.DungeonStaff>(), npc, NPCID.DarkCaster);
			DropLoot(200, 200, ModContent.ItemType<Items.Sets.GunsMisc.Swordsplosion.Swordsplosion>(), npc, NPCID.RustyArmoredBonesAxe, NPCID.RustyArmoredBonesFlail, NPCID.RustyArmoredBonesSword, NPCID.RustyArmoredBonesSwordNoArmor, NPCID.BlueArmoredBones, NPCID.BlueArmoredBonesMace,
				NPCID.BlueArmoredBonesNoPants, NPCID.BlueArmoredBonesSword, NPCID.HellArmoredBones, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSword);
			DropLoot(175, 175, ModContent.ItemType<Items.Sets.BowsMisc.Morningtide.Morningtide>(), npc, NPCID.HellArmoredBones, NPCID.HellArmoredBonesSpikeShield, NPCID.HellArmoredBonesMace, NPCID.HellArmoredBonesSword);
			DropLoot(3, 3, ModContent.ItemType<FrigidFragment>(), Main.rand.Next(1, 3), npc, NPCID.ZombieEskimo, NPCID.IceSlime, NPCID.IceBat, NPCID.ArmoredViking);
			DropLoot(20, 16, ModContent.ItemType<FrostGiantBelt>(), 1, npc, NPCID.UndeadViking);
			DropLoot(1, 1, ModContent.ItemType<FrigidFragment>(), Main.rand.Next(1, 3), npc, NPCID.SpikedIceSlime, NPCID.ArmedZombieEskimo);
			DropLoot(14, 10, ModContent.ItemType<SweetThrow>(), npc, NPCID.QueenBee);
			DropLoot(2, 2, ModContent.ItemType<OldLeather>(), Main.rand.Next(1, 3), npc, NPCID.Zombie, NPCID.BaldZombie, NPCID.SlimedZombie, NPCID.SwampZombie, NPCID.TwiggyZombie, NPCID.ZombieRaincoat, NPCID.PincushionZombie);
			DropLoot(50, 50, ModContent.ItemType<SolarRattle>(), npc, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider);
			DropLoot(50, 50, ModContent.ItemType<EngineeringRod>(), npc, NPCID.GrayGrunt, NPCID.RayGunner, NPCID.BrainScrambler);
			DropLoot(5, 5, ModContent.ItemType<Items.Sets.BowsMisc.Eyeshot.Eyeshot>(), npc, NPCID.EyeofCthulhu);
			DropLoot(2, 2, ModContent.ItemType<Martian>(), npc, NPCID.MartianSaucer);
			DropLoot(1, 1, Main.rand.NextBool(2) ? ModContent.ItemType<Ancient>() : ModContent.ItemType<CultistScarf>(), npc, NPCID.CultistBoss);
			DropLoot(50, 50, ModContent.ItemType<IchorPendant>(), npc, NPCID.IchorSticker);
			DropLoot(1, 1, ModContent.ItemType<Typhoon>(), npc, NPCID.DukeFishron);
			DropLoot(6, 4, ModContent.ItemType<PigronStaffItem>(), npc, NPCID.PigronCorruption, NPCID.PigronHallow, NPCID.PigronCrimson);
			DropLoot(18, 18, ModContent.ItemType<TheFireball>(), npc, NPCID.FireImp);
			DropLoot(50, 50, ModContent.ItemType<CursedPendant>(), npc, NPCID.Clinger);
			DropLoot(50, 50, ModContent.ItemType<MagnifyingGlass>(), npc, NPCID.DemonEye, NPCID.DemonEye2, NPCID.DemonEyeOwl, NPCID.DemonEyeSpaceship);
			DropLoot(1, 1, ModContent.ItemType<PirateKey>(), npc, NPCID.PirateShip);
			DropLoot(6, 6, ModContent.ItemType<Items.Sets.SummonsMisc.SanguineFlayer.SanguineFlayerItem>(), npc, NPCID.BigMimicCrimson);
			DropLoot(6, 5, ModContent.ItemType<Items.Accessory.OpalFrog.OpalFrogItem>(), npc, NPCID.BigMimicHallow);
			DropLoot(15, 15, ModContent.ItemType<Items.Sets.SwordsMisc.CurseBreaker.CurseBreaker>(), npc, NPCID.RedDevil);
			DropLoot(1, 1, ModContent.ItemType<IridescentScale>(), Main.rand.Next(2, 5), npc, NPCID.Shark);
		}

		/// <summary>Drops an item given the specific conditions.</summary>
		/// <param name="npc">NPC to check against.</param>
		/// <param name="chance">Chance in not-expert mode (1/x).</param>
		/// <param name="expertChance">Chance in expert mode (1/x).</param>
		/// <param name="itemID">The item to drop.</param>
		/// <param name="types">The NPC IDs to drop from.</param>
		public void DropLoot(int chance, int expertChance, int itemID, NPC npc, params int[] types) => DropLoot(chance, expertChance, itemID, 1, npc, types);

		/// <summary>Drops an item given the specific conditions.</summary>
		/// <param name="npc">NPC to check against.</param>
		/// <param name="chance">Chance in not-expert mode (1/x).</param>
		/// <param name="expertChance">Chance in expert mode (1/x).</param>
		/// <param name="itemID">The item to drop.</param>
		/// <param name="stack">The stack size of the dropped item.</param>
		/// <param name="types">The NPC IDs to drop from.</param>
		public void DropLoot(int chance, int expertChance, int itemID, int stack, NPC npc, params int[] types)
		{
			int r = Main.expertMode ? expertChance : chance;
			if (types.Contains(npc.type) && Main.rand.NextBool(r))
				Item.NewItem(npc.getRect(), itemID, stack);
		}

		private void SpawnSpiritBiome()
		{
			int firstX = WorldGen.genRand.Next(100, (Main.maxTilesX / 2) - 500);
			if (Main.dungeonX > Main.maxTilesX / 2) //rightside dungeon
				firstX = WorldGen.genRand.Next((Main.maxTilesX / 2) + 300, Main.maxTilesX - 500);

			int xAxis = firstX;
			int xAxisMid = xAxis + 70;
			int xAxisEdge = xAxis + 380;
			int yAxis = 0;

			int distanceFromCenter = 0;

			int[] Grasses = { TileID.Grass, TileID.CorruptGrass, TileID.HallowedGrass, TileID.FleshGrass };
			int[] Ices = { TileID.IceBlock, TileID.CorruptIce, TileID.HallowedIce, TileID.FleshIce };
			int[] Stones = { TileID.Stone, TileID.Ebonstone, TileID.Pearlstone, TileID.Crimstone, TileID.GreenMoss, TileID.BrownMoss, TileID.RedMoss, TileID.BlueMoss, TileID.PurpleMoss };
			int[] Sands = { TileID.Sand, TileID.Ebonsand, TileID.Pearlsand, TileID.Crimsand };
			int[] Decors = { TileID.Plants, TileID.CorruptPlants, TileID.CorruptThorns, TileID.Vines, TileID.JungleVines, TileID.HallowedPlants, TileID.HallowedPlants2, TileID.HallowedVines, TileID.Stalactite, TileID.SmallPiles, TileID.LargePiles, TileID.LargePiles2, TileID.FleshWeeds, TileID.CrimsonVines };

			for (int y = 0; y < Main.maxTilesY; y++)
			{
				yAxis++;
				xAxis = firstX;
				for (int i = 0; i < 450; i++)
				{
					xAxis++;
					if (Framing.GetTileSafely(xAxis, yAxis).HasTile)
					{
						int nullRandom = Main.tile[xAxis, yAxis + 1] == null ? 50 : 1;
						int type = -1;

						if (Main.tile[xAxis, yAxis].TileType == TileID.Dirt)
							type = ModContent.TileType<SpiritDirt>();
						if (Grasses.Contains(Main.tile[xAxis, yAxis].TileType))
							type = ModContent.TileType<SpiritGrass>();
						else if (Ices.Contains(Main.tile[xAxis, yAxis].TileType))
							type = ModContent.TileType<SpiritIce>();
						else if (Stones.Contains(Main.tile[xAxis, yAxis].TileType))
							type = ModContent.TileType<SpiritStone>();
						else if (Sands.Contains(Main.tile[xAxis, yAxis].TileType))
							type = ModContent.TileType<Spiritsand>();

						if (xAxis < xAxisMid - 1)
							distanceFromCenter = xAxisMid - xAxis;
						else if (xAxis > xAxisEdge + 1)
							distanceFromCenter = xAxis - xAxisEdge;

						if (type != -1 && Main.rand.NextBool(nullRandom) && Main.rand.Next(distanceFromCenter) < 10)
							Main.tile[xAxis, yAxis].TileType = (ushort)type; //Converts tiles

						if (WallID.Sets.Conversion.Grass[Main.tile[xAxis, yAxis].WallType] && Main.rand.NextBool(50) && Main.rand.Next(distanceFromCenter) < 18)
							Main.tile[xAxis, yAxis].WallType = (ushort)ModContent.WallType<SpiritWall>(); //Converts walls

						if (Decors.Contains(Main.tile[xAxis, yAxis].TileType) && Main.rand.NextBool(nullRandom) && Main.rand.Next(distanceFromCenter) < 18)
							Main.tile[xAxis, yAxis].HasTile = false; //Removes decor

						if (Main.tile[xAxis, yAxis].TileType == ModContent.TileType<SpiritStone>() && yAxis > (int)((Main.rockLayer + Main.maxTilesY - 500) / 2f) && Main.rand.NextBool(300))
							WorldGen.TileRunner(xAxis, yAxis, WorldGen.genRand.Next(5, 7), 1, ModContent.TileType<Items.Sets.SpiritSet.SpiritOreTile>(), false, 0f, 0f, true, true); //Adds ore
					}
				}
			}

			MyWorld.spiritBiome = true;

			if (Main.netMode == NetmodeID.SinglePlayer)
				Main.NewText("The spirits spread through the land...", Color.Orange);
			else if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.WorldData);
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The Spirits spread through the Land..."), Color.Orange, -1);
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (sFracture && Main.rand.Next(2) == 0)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Firework_Yellow, (Main.rand.Next(8) - 4), (Main.rand.Next(8) - 4), 133);

			if (felBrand && Main.rand.Next(2) == 0)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.CursedTorch, (Main.rand.Next(8) - 4), (Main.rand.Next(8) - 4), 75);

			if (vineTrap)
				drawColor = new Color(103, 138, 84);
			if (clatterPierce)
				drawColor = new Color(115, 80, 57);
			if (tracked)
				drawColor = new Color(135, 245, 76);
		}
	}
}