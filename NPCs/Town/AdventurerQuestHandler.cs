using Microsoft.Xna.Framework;
using SpiritMod.Items.Armor.HunterArmor;
using SpiritMod.Items.Armor.ReachBoss;
using SpiritMod.Items.Armor.CowboySet;
using SpiritMod.Items.Armor.BeekeeperSet;
using SpiritMod.Items.Armor.CapacitorSet;
using SpiritMod.Items.Armor.CenturionSet;
using SpiritMod.Items.Armor.ClatterboneArmor;
using SpiritMod.Items.Armor.WayfarerSet;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Tiles.Furniture.Critters;
using SpiritMod.Tiles.Furniture.SpaceJunk;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.NPCs.Town
{
	public class AdventurerQuestHandler
	{
		private List<Quest> _quests;
		private bool[] _completed;
		private int _questsCompleted;

		private int _previousQuest;
		private int _currentQuest;

		private Mod _mod;

		public bool CurrentQuestSkippable {
			get {
				if (_currentQuest == -1) return true;
				if (_currentQuest == -2) return false;
				return _quests[_currentQuest].NthQuest == -1;
			}
		}

		public AdventurerQuestHandler(Mod mod)
		{
			_mod = mod;

			_quests = new List<Quest>();

			#region FetchQuests
			Quest shadowflameStaffQuest = RegisterQuest(ModContent.ItemType<ShadowflameStoneStaff>(),

				"I've heard tell of an Arcane Goblin Tower near the far shores of this land." +
				" There's supposed to be a staff inside that holds tremendous power. Could you check it out for me?" +
				" I'm still vacationing. Just bring back the staff and show it to me, and I'll reward ya handsomely.",

				"Hope those goblins didn't give you too much trouble, heh." +
				" Wow, look at that craftwork! It's supposed to be real powerful, too." +
				" So maybe you won't get killed while you're out there adventuring, yeah?", false,
				() => {
					MyWorld.shadowflameComplete = true;
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<PottedWillow>(), Main.rand.Next(1, 4));
					Main.LocalPlayer.QuickSpawnItem(ItemID.TatteredCloth, Main.rand.Next(5, 7));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
				});
			shadowflameStaffQuest.CanGiveQuest = () => {
				return MyWorld.gennedTower && !MyWorld.gennedBandits;
			};
            shadowflameStaffQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
            };


            Quest sepulchreChestQuest = RegisterQuest(ModContent.ItemType<SepulchreChest>(),

				"You ever wonder why there're so many skeletons underground?" +
				" Turns out that there was a band of necromancers that holed up in the caverns all across the world and performed all kinds of experiments." +
				" Well, lucky for us they're gone! But their Sepulchres still remain. Mind grabbin' me a chest from there? I'd like to study their architecture further. Don't turn into a skeleton!",

				"Thanks, bud. After studying this artifact, I've managed to reproduce a few vases in their weird style. " +
				"If you ever want your house to have a spooky vibe, here ya go. Don't go conjuring any skeletons, now.", true,

				() => {
					MyWorld.sepulchreComplete = true;
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SepulchrePotItem1>(), Main.rand.Next(6, 10));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SepulchrePotItem2>(), Main.rand.Next(6, 10));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
				});
            sepulchreChestQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
            };

            Quest jadeStaffQuest = RegisterQuest(ModContent.ItemType<JadeStaff>(),

				"Hey, lad. I've got another proposition for ya." +
				" You see, I've been looking at some old maps and I've learned about a cluster of Floating Pagodas above the oceans of this world." +
				" Trouble is, I can't make out whether it's to the left or right, so would ya go explorin' for me? I'm looking for an ornate staff, hundreds of years old. Happy hunting!",

				"Undead samurai? Vengeful spirits? Sounds like a riot! Wish I could've been there." +
				" Either way, this ol' staff is mighty powerful. Quite durable, too, considering it's nearly a millenium old." +
				" Use it carefully, alright? That there is one of a kind.", false,

				() => {
					MyWorld.jadeStaffComplete = true;
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<PottedSakura>(), Main.rand.Next(1, 4));
					int[] lootTable = {
					ModContent.ItemType<Shrine1>(),
					ModContent.ItemType<Shrine2>(),
					ModContent.ItemType<Shrine3>(),
					};
					int loot = Main.rand.Next(lootTable.Length);
					int loot1 = Main.rand.Next(lootTable.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable[loot]);
					Main.LocalPlayer.QuickSpawnItem(lootTable[loot1]);
					if (Main.rand.Next(4) == 0) {
						Main.LocalPlayer.QuickSpawnItem(ItemID.Kimono);
					}
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
				});
			jadeStaffQuest.CanGiveQuest = () => {
				return NPC.downedBoss1;
			};
            jadeStaffQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
            };

            Quest hornetFishQuest = RegisterQuest(ModContent.ItemType<HornetfishQuest>(),

				"I've got a, uh, perfectly normal quest for ya. Why don't you go ahead and head to the Jungle to fish up a Hornetfish for me?" +
				" It's supposed to be a real delicacy, but I'm still on vacation mode. Be careful, though. I've heard it can be a... tough catch." +
				" Whaddya mean, this sounds exactly like something the Angler would want you to do? ",

				"Oops, forgot to mention that the fish could fly. I'm sure that was obvious though, right? Y'know, it's got hornet in the name for a reason." +
				" It's supposed to taste mighty tangy. Hopefully you didn't butcher it too much. You may not know this about me, but I'm quite the cookin' expert." +
				" Can't wait to roast this over a fire. Thanks, lad.",
				 true,
				 () => {
					 MyWorld.spawnHornetFish = false;
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<KoiTotem>());
					 Main.LocalPlayer.QuickSpawnItem(ItemID.Vine, 2);
					 if (Main.rand.Next(3) == 0) {
						 Main.LocalPlayer.QuickSpawnItem(ItemID.TigerSkin);
					 }
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<FishingPainting>());
					 Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
				 }

				);
			hornetFishQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
                MyWorld.spawnHornetFish = true;
			};
			Quest mushroomQuest = RegisterQuest(ModContent.ItemType<VibeshroomItem>(),

				"I've been hearin' stories about some new flora that's cropped up around those strange Mushroom Forests recently." +
				" These lil' buggers seem to just sway from side to side, as if they're dancin'." +
				" I have no real motive this time around, I just wanna see one of 'em. Mind fetching one for me?",

				" It's a cutie for sure. I've put this critter into a little jar. I'm sure it could spice up my home." +
				" We all could use a little more cute from time to time. If you find any more, don't hurt the little things!", true,
				() => {
					MyWorld.vibeShroomComplete = true;
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<VibeshroomJarItem>(), 1);
					if (Main.rand.Next(3) == 0) {
						Main.LocalPlayer.QuickSpawnItem(868);
					}
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<GlowRoot>(), Main.rand.Next(3, 6));
					Main.LocalPlayer.QuickSpawnItem(ItemID.GlowingMushroom, Main.rand.Next(8, 15));
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
				});

			mushroomQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
                MyWorld.spawnVibeshrooms = true;
			};

			Quest darkfeatherQuest = RegisterQuest(ModContent.ItemType<Items.Accessory.DarkfeatherVisage.DarkfeatherVisage>(),

			"Scouts near the far ends of the world have reported somethin' mighty interesting, lad. A witch seems to be terrorizin' the area." +
			" Apparently, it's some type of harpy with a real dangerous staff. Mission's real simple this time. Bring me its head!" +
			" Er... I promise I'm not unhinged. I mean, bring me its hat! Yeah. Safe travels.", 
			
			"Just got back, eh? I'm sure it was a tough battle, but you only seem mildly burned. Hmm, looks like that hat you've got there has some magical qualities." +
			" Hang onto it for me. And take this staff, too. A buddy recovered it from the wreckage of your battle. But don't go terrorizing the land with it, ya hear?", false,

			() => {
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<AkaviriStaff>());
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
                Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(90, 105));
            });
            darkfeatherQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
                MyWorld.spawnDarkfeather = true;
            };
            darkfeatherQuest.TrySkipQuest = () =>
            {
                MyWorld.spawnDarkfeather = false;
                return true;
            };
           darkfeatherQuest.CanGiveQuest = () => {
                return NPC.downedBoss2;
            };

            #endregion
            #region ExplorerQuests
            Quest explorerQuestMushroom = RegisterQuest(ModContent.ItemType<ExplorerScrollMushroomFull>(),

				"Up for a little explorin'? This world's massive, and even I haven't seen it all." +
				" I'd like ya to head out there and map out the far reaches of this land. Specifically, I'm looking for info on any nearby Glowing Mushroom Caverns." +
				" Take this blank map. After you stumble upon one of 'em caverns, wander around for a while and take some notes for me, alright? Return to me when the map's all filled out.",

				"Found a Glowing Mushroom Cavern, did ya? Glad you made it back in one piece, lad. Those caves may seem all light and charming, but they aren't a jokin' matter." +
				" This should help me study the local environment more. Maybe I'll plan a few raids in the future? Thanks for the intel.", false,

				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<PinBlue>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MapScroll>(), Main.rand.Next(1, 3));
					Main.LocalPlayer.QuickSpawnItem(ItemID.MushroomGrassSeeds, Main.rand.Next(2, 3));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));

				});

			explorerQuestMushroom.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/ExplorerStart"));
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ExplorerScrollMushroomEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ExplorerScrollMushroomEmpty>());
			};
			Quest explorerQuestAsteroids = RegisterQuest(ModContent.ItemType<ExplorerScrollAsteroidFull>(),

				"Up for a little explorin'? This world's massive, and even I haven't seen it all." +
				" I'd like ya to head out there and map out the far reaches of this land. There's an asteroid field smack-dab above one of the oceans. Completely uncharted." +
				" Take this blank map. After you stumble upon those asteroids, wander around for a while and take some notes for me, alright? Don't fall off, lad.",

				"Did you stumble upon those asteroids? Must've been real scary up there, almost in orbit. Those Sky Islands are enough for me, phew. Did ya float?" +
				" Sorry, I'm gettin' distracted. I'll use this info to look for any extraterrestrial loot that may be up in those floatin' rocks. Thanks for the journey as always, lad.", false,

				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<PinRed>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<OldTelescope>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<AsteroidBox>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MapScroll>(), Main.rand.Next(1, 3));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<JumpPadItem>(), Main.rand.Next(1, 2));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ScrapItem>(), Main.rand.Next(50, 70));
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));

				});

			explorerQuestAsteroids.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/ExplorerStart"));
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ExplorerScrollAsteroidEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ExplorerScrollAsteroidEmpty>());
			};
			Quest explorerQuestMarble = RegisterQuest(ModContent.ItemType<ExplorerScrollMarbleFull>(),

				"Up for a little explorin'? This world's massive, and even I haven't seen it all." +
				" I'd like ya to head out there and map out the lower reaches of this land. A few caverns are covered in marble and the ruins of some ancient civilization." +
				" Take this blank map. After you stumble upon one of these Marble Caverns, wander around for a while and take some notes for me, alright? Hopefully, those ruins are desolate, eh?",

				"I take it the caverns weren't too empty, then. Did ya manage to get a completed map, though? Great! These caverns are lookin' chock full of ancient loot." +
				" Maybe I'll coordinate some digs with the Demolitionist. You're welcome to join us too, lad. You've more than earned it. ", false,

				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CenturionHead>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CenturionBody>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CenturionLegs>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MarbleBox>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MapScroll>(), Main.rand.Next(1, 3));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.Javelin, Main.rand.Next(50, 95));
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));

				});

			explorerQuestMarble.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/ExplorerStart"));
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ExplorerScrollMarbleEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ExplorerScrollMarbleEmpty>());
			};

			Quest explorerQuestGranite = RegisterQuest(ModContent.ItemType<ExplorerScrollGraniteFull>(),

				 "Up for a little explorin'? This world's massive, and even I haven't seen it all." +
				 " A couple of underground cave systems seem to be made almost entirely of dark granite." +
				 " Some kinda energy source seems to be bringin' the rocks to life, too. I'd like ya to go and investigate. Take this blank map. After you stumble upon one of these Granite Caverns, wander around for a while and take some notes for me, alright?",

				 "You're alright! Phew. I was just hearin' rumors about big ol' granite golems and floating rocks. I can tell that they were true." +
				 " Lookin' at this map, this place still seems real mysterious. Maybe there are some ore deposits we can extract some of that granite energy from?" +
				 " I'll talk to the Merchant and Demolitionist. Mighty fine work as usual, lad.", false,
				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CapacitorHead>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CapacitorBody>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CapacitorLegs>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<GraniteBox>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MapScroll>(), Main.rand.Next(1, 4));
					Main.LocalPlayer.QuickSpawnItem(ItemID.NightVisionHelmet);
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
				});
			explorerQuestGranite.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/ExplorerStart"));
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ExplorerScrollGraniteEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ExplorerScrollGraniteEmpty>());
			};
			Quest explorerQuestHive = RegisterQuest(ModContent.ItemType<ExplorerScrollHiveFull>(),

				"Up for a little explorin'? This world's massive, and even I haven't seen it all." +
				" Have you checked out the lower parts of the Jungle? I've recently heard about a series of massive hives around there." +
				" I loathe bees... an' hornets... an' giant man eatin' plants, so would ya like to check one of these hives out for me? ",

				"You're covered in a lot of bee stings, lad. Maybe the Nurse has something for those. They look super painful. Did ya map the place out, though?" +
				" Hmm, interesting. Lotta honey for the takin'. What's this? This giant larva in the middle. Can't be good, for sure. Tread lightly around there, alright?", false,
				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeekeeperHead>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeekeeperBody>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeekeeperLegs>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MapScroll>(), Main.rand.Next(1, 4));
					Main.LocalPlayer.QuickSpawnItem(ItemID.BottledHoney, Main.rand.Next(8, 12));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));

				});
			explorerQuestHive.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/ExplorerStart"));
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ExplorerScrollHiveEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ExplorerScrollHiveEmpty>());
			};


			#endregion
			#region SlayerQuests
			Quest slayerQuestWinterborn = RegisterQuest(ModContent.ItemType<WinterbornSlayerScrollFull>(),

				"A few of my associates have scouted a growing threat in the icy caverns." +
				" Sometimes, they even get bold enough to roam the surface durin' heavy blizzards. We're calling 'em Winterborn, and I'd like for you to thin their numbers a bit." +
				" They're a tough foe, so be safe, lad. This contract should help keep track of your kills. Oh, and I'd be cautious when roaming those caverns. Some of those Ice Sculptures look a little too lifelike...",

				"You've grown into a top-notch warrior, lad. I'm not even sure I could've handled ten of those freaky cadavers. I'm sure the frozen tundra will be a little safer to explore, thanks to you." +
				" I did some explorin', and managed to wrangle some of these Ice Sculptures. Please take 'em, they still freak me out. But I don't think they're dangerous. Maybe. Come back for more work any time.", true,

				() => {
					int[] lootTable = {
					ModContent.ItemType<IceBatSculpture>(),
					ModContent.ItemType<IceFlinxSculpture>(),
					ModContent.ItemType<IceVikingSculpture>(),
					ModContent.ItemType<IceWheezerSculpture>(),
					ModContent.ItemType<WinterbornSculpture>(),
					};
					int loot = Main.rand.Next(lootTable.Length);
					int loot1 = Main.rand.Next(lootTable.Length);

					Main.LocalPlayer.QuickSpawnItem(lootTable[loot]);
					Main.LocalPlayer.QuickSpawnItem(lootTable[loot1]);

					int[] lootTable1 = {
					ModContent.ItemType<TargetCan>(),
					ModContent.ItemType<TargetBottle>(),
					};
					int loot3 = Main.rand.Next(lootTable1.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));

					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SnowRangerHead>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SnowRangerBody>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SnowRangerLegs>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<IceBerries>(), Main.rand.Next(2, 6));
					MyWorld.winterbornComplete = true;
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
				});
			slayerQuestWinterborn.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<WinterbornSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<WinterbornSlayerScrollEmpty>());
			};
			slayerQuestWinterborn.CanGiveQuest = () => {
				return NPC.downedBoss3;
			};

			Quest slayerQuestBeholder = RegisterQuest(ModContent.ItemType<BeholderSlayerScrollFull>(),

				"You've really shaken up the world after slaying the great evil in that disgusting biome. But not for the better. New horrors are startin' to crop up everwhere" +
				" The Marble Caverns have seen quite a stir. This new monstrosity's got tentacles, eyes, fireballs, you name it." +
				" Eugh. Just thinkin' about it grosses me out. Can you go kill this Beholder for me, lad? This contract should help keep track of your mission.",

				" A job well done. I hope that monster wasn't too much of a threat, but I'd wager you dealt with it pretty handily. Great work again, lad.", true,

				() => {
					int[] lootTable1 = {
					ModContent.ItemType<TargetCan>(),
					ModContent.ItemType<TargetBottle>(),
					};
					int loot3 = Main.rand.Next(lootTable1.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeholderMask>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MarbleChunk>(), Main.rand.Next(6, 11));
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
				});
			slayerQuestBeholder.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<BeholderSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeholderSlayerScrollEmpty>());
			};
			slayerQuestBeholder.CanGiveQuest = () => {
				return NPC.downedBoss2;
			};

            Quest slayerQuestBriar = RegisterQuest(ModContent.ItemType<BriarSlayerScrollFull>(),

                "My disdain for those vined beasts in the Briar may border on, erm, unhealthy. Either way, I need ya to go down there and show them who's boss." +
				" Kill some of those big hounds an' those Thorn Stalkers. They're mighty dangerous, and very stealthy. Maybe we can make the Briar a safer place..."+
                " Ah, who am I kiddin'? That place is bound to stay a hellhole. Just do it for me. This contract should help keep track of your mission.",

                "Wow, you're still kickin'? And with no noticeable bite marks or holes? Color me impressed, lad. I've whipped up somethin' that should make your travels easier." +
				" Keep on keepin' on. And come back to me if you want somewhere new to explore.", true,

                () => {
                    int[] lootTable1 = {
                    ModContent.ItemType<TargetCan>(),
                    ModContent.ItemType<TargetBottle>(),
                    };
                    int loot3 = Main.rand.Next(lootTable1.Length);
                    Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<FeralConcoction>());
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ReachBossHead>());
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ReachBossBody>());
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ReachBossLegs>());
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
                });
            slayerQuestBriar.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<BriarSlayerScrollEmpty>()))
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BriarSlayerScrollEmpty>());
            };

            Quest slayerQuestOwl = RegisterQuest(ModContent.ItemType<ScreechOwlScrollFull>(),

                "I'd like to think I'm an animal lover, y'know? I promise I love dogs, an' cats, and all kinds of furry creatures. Except for that horrifying beast..." +
				" You guessed it, I'm talkin' about Screech Owls, lad. Whaddya mean, that isn't what you thought I'd say? Those things are horrifyin'!" +
				" Every night, I hear their screeches echoin' from the snowy tundra. It ruins my sleep! Could ya get rid of one for me, just so that I can have peace of mind? This contract should help keep track of your mission.",

                "Well, I know there's more than one Screech Owl out there. Tricky pests. But maybe you've allowed me to sleep a bit sounder at night." +
				" Speaking of, I've been workin' on a new contraption that'll maybe help us all sleep better at night. It plays music! Buy it from me any time, lad.", true,

                () => {
                    MyWorld.owlComplete = true;
                    int[] lootTable1 = {
                    ModContent.ItemType<TargetCan>(),
                    ModContent.ItemType<TargetBottle>(),
                    };
                    int loot3 = Main.rand.Next(lootTable1.Length);
                    Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
                    Main.LocalPlayer.QuickSpawnItem(ItemID.MusicBox);
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
                });
            slayerQuestOwl.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ScreechOwlScrollEmpty>()))
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ScreechOwlScrollEmpty>());
            };

            Quest slayerQuestValkyrie = RegisterQuest(ModContent.ItemType<ValkyrieSlayerScrollFull>(),

				"Just who I've been meanin' to talk to! You ever been high enough where those infuriatin' harpies can shoot at ya?" +
				" Well, to make things worse, I've heard tales of a harpy clad in armor and weapons!" +
				" But I'm sure you can handle it, lad. We've taken to calling it a Valkyrie, but don't go joinin' the afterlife when you take it on! This contract should help keep track of your mission.",

				"Gotta hand it to you, that was one tough customer. I'm glad you made it back okay, though. I will say, though- the view up there is nothing short of breathtaking" +
				" You ever seen an aurora from space? It's a feelin' I can't put into words. Hopefully your next journey skyward will be more peaceful, eh?", true,

				() => {
					int[] lootTable1 = {
					ModContent.ItemType<TargetCan>(),
					ModContent.ItemType<TargetBottle>(),
					};
					int loot3 = Main.rand.Next(lootTable1.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ChaosPearl>(), Main.rand.Next(5, 9));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					if (Main.rand.Next(2) == 0) {
						Main.LocalPlayer.QuickSpawnItem(1987);
					}
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
				});
			slayerQuestValkyrie.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ValkyrieSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ValkyrieSlayerScrollEmpty>());
			};
			slayerQuestValkyrie.CanGiveQuest = () => {
				return NPC.downedBoss1;
			};

			Quest slayerQuestAntlion = RegisterQuest(ModContent.ItemType<AntlionSlayerScrollFull>(),

				"Gear up, lad! You're going to assassinate some assassins today. Word on the wind is that a group of bandits have been roaming the desert recently." +
				" They live in some kind of agreement with those freaky antlions. Even ride 'em sometimes, too. Could you go out there an' make the desert a safer place?" +
				" Killing a few of those Antlion Assassins is sure to make travellin' a bit easier. This contract should help keep track of your mission.",

				" Hope you didn't get caught up in one of those nasty sandstorms when you were explorin' the desert. Even without one, those assassins are tough to spot." +
				" It's good to know that you made it home without a knife in your back, lad. Or in your stomach, your arms... you catch my drift. Thanks for makin' it just a little easier to traverse the world.", true,

				 () => {
					 int[] lootTable1 = {
					 ModContent.ItemType<TargetCan>(),
					 ModContent.ItemType<TargetBottle>(),
					 };
					 int loot3 = Main.rand.Next(lootTable1.Length);
					 Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					 Main.LocalPlayer.QuickSpawnItem(934);
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CowboyHead>());
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CowboyBody>());
					 Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<CowboyLegs>());
					 Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
				 });
			slayerQuestAntlion.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<AntlionSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<AntlionSlayerScrollEmpty>());
			};

			Quest slayerQuestDrBones = RegisterQuest(ModContent.ItemType<DrBonesSlayerQuestFull>(),

				"The Jungle's a rough place, that's for sure. My colleague, an expert archaeologist, went roamin' the place for some ancient temple." +
				" He didn't make it, though. Real shame, that lad was a riot. Reports have told me that he's still roamin' the Jungle surface as a zombie." +
				" Mind going out there and puttin' him to rest for me? He's been exploring enough.",

				"You've done a huge service for me, lad. It means a lot that my friend can finally rest easy after years of a cursed, undead life. I have some of his ol' gear for you." +
				" Maybe it'll give you an edge while looking for artifacts? Either way, it's mighty stylish. Good explorin'!", true,
				() => {
					int[] lootTable1 = {
					 ModContent.ItemType<TargetCan>(),
					 ModContent.ItemType<TargetBottle>(),
					 };
					MyWorld.drBonesComplete = true;
					int loot3 = Main.rand.Next(lootTable1.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(252);
					Main.LocalPlayer.QuickSpawnItem(253);
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
				});
			slayerQuestDrBones.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<DrBonesSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<DrBonesSlayerScrollEmpty>());
			};
			slayerQuestDrBones.CanGiveQuest = () => {
				return NPC.downedBoss1;
			};

			Quest slayerQuestWheezers = RegisterQuest(ModContent.ItemType<WheezerSlayerScrollFull>(),

				"Some new creepy crawlies have taken to calling the caverns their home. Disgustin' little fellas that belch poison gas and some spiny little buggers." +
				" I've got a simple task for ya. Do us all a favor and exterminate those nasty things. Killing a dozen of 'em will surely make the underground a less nasty place." +
				" This contract should help keep track of your mission.",

				" I can't lie, lad. You smell like bug juice and it's not the prettiest thing in the world. But that just means you did it, right?" +
				" We can all head down there with a little less fear. Appreciate it.", true,

				() => {
					int[] lootTable1 = {
					 ModContent.ItemType<TargetCan>(),
					 ModContent.ItemType<TargetBottle>(),
					 };
					int loot3 = Main.rand.Next(lootTable1.Length);
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<WheezerPainting>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ClatterSpear>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ClatterboneFaceplate>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ClatterboneBreastplate>());
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ClatterboneLeggings>());
					Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
				});
			slayerQuestWheezers.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<WheezerSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<WheezerSlayerScrollEmpty>());

			};
			slayerQuestWheezers.CanGiveQuest = () => {
				return NPC.downedBoss1;
			};

			Quest slayerQuestStardancers = RegisterQuest(ModContent.ItemType<StardancerSlayerScrollFull>(),
				"You've checked out the Asteroid Fields near the far corner of the world, right? My sources have reported some increasin' mechanical activity around there." +
				" Some kinda weird automatonic worms made of metal are streakin' through the sky there. Something has to be putting 'em on edge. Could you kill a couple and see what makes 'em tick?" +
				" This contract should help keep track of your mission.",

				"So, they seem to be powered by some kind of extraterrestrial energy source, huh? I can't make heads or tails of these things. Maybe the Guide can help ya out?" +
				" What I do know is that my gut is tellin' me that the Asteroids are about to get a whole lot more dangerous. Stay strong, lad.", true,
				() => {
					int[] lootTable1 = {
					 ModContent.ItemType<TargetCan>(),
					 ModContent.ItemType<TargetBottle>(),
					 };
					int[] lootTable = {
					 ModContent.ItemType<ScrapItem1>(),
					 ModContent.ItemType<ScrapItem2>(),
					 ModContent.ItemType<ScrapItem3>(),
					 ModContent.ItemType<ScrapItem4>(),
					 };
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
					int loot = Main.rand.Next(lootTable.Length);
					int loot2 = Main.rand.Next(lootTable1.Length);
					int loot3 = Main.rand.Next(lootTable1.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable[loot3]);
					Main.LocalPlayer.QuickSpawnItem(lootTable[loot2]);
					Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<TechChip>());
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
				});
			slayerQuestStardancers.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SlayerStart"));

                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<StardancerSlayerScrollEmpty>()))
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<StardancerSlayerScrollEmpty>());

			};
			slayerQuestStardancers.CanGiveQuest = () => {
				return NPC.downedBoss3;
			};



			#endregion

			#region ScriptedQuests
			//Scripted Quest 1
			Quest hookbatQuest = RegisterQuest(ModContent.ItemType<DurasilkSheaf>(),

			"So you wanna be an adventurer, eh? Well, you're going to need to gear up if you want to explore the world!" + 
			" As thanks for savin' me from the Briar, I'd actually planned to craft you a set of special armor, perfect for explorin'." +
			" Unfortunately, some mangy Hookbats stole the sheaf of Durasilk I was usin'! They only come out at night around the forest surface. Mind retrievin' that silk for me so I can thank you properly?",

			"Durasilk's mighty strong stuff, it doesn't even look scratched! I picked it up from a travelling merchant in a far-off land. But, I figure you'll need it more now" +
			" Thanks again for savin' me back there. This isn't much, but hopefully this armor'll save ya from turning into a monster's snack. You're 'officially' an adventurer now, lad. Welcome!", true,

			() => {
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<WayfarerHead>());
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<WayfarerBody>());
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<WayfarerLegs>());
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
				Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
			});
            hookbatQuest.OnQuestStart = () => {
                MyWorld.spawnHookbats = true;
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
            };
            hookbatQuest.NthQuest = 1;
			//Scripted Quest 2
			Quest sacredVineQuest = RegisterQuest(ModContent.ItemType<SacredVine>(),

			"Ever since I was captured by those savages from the Briar, I've been doin' some research on the place." +
			" That altar you found me at is supposed to harbor a really venegeful nature spirit." +
			" However, the vines that spirit's made of are said to possess some mystical regenerative properties, or somethin'. Mind investigating? And kill some savages for me while you're at it.",

			"Revenge is sweet! Now those feral beasts in the Briar are sure to think twice about capturin' innocent adventurers in the future! Hopefully." +
			"I've whipped up some healin' potions with those vines you gave me. I'm sure it'll help out when you're in a tight spot, eh?", true,

			() => {
				Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<EnchantedLeaf>(), Main.rand.Next(5, 9));
				Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<AncientBark>(), Main.rand.Next(10, 25));
				Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<Items.Armor.Masks.GladeWraithMask>(), 1);
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
                Main.LocalPlayer.QuickSpawnItem(ItemID.HealingPotion, 4);

				Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
			});
            sacredVineQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
            };
            sacredVineQuest.NthQuest = 2;
			//Scripted Quest 3
			Quest scarabQuest = RegisterQuest(ModContent.ItemType<ScarabIdolQuest>(),

				"The sands of the desert hide a lot of secrets beneath 'em. " +
				"There's supposed to be an Ancient Ziggurat buried near the surface of one of those wastelands. " +
				"Could ya head down there and scavenge some relics from me? I've got a hunch, but I need to confirm it...",

				"I knew it. I was polishin' up this old thing when it started to look real familiar. " +
				"That's a Scarab Idol right there. I'm warning ya, don't mess with it until you get real strong. " +
				"Me and some bounty hunters tried to take that thing on years ago. We barely escaped with our lives. Be safe, lad.", true,

				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<Items.Consumable.ScarabIdol>(), 1);
					Main.LocalPlayer.QuickSpawnItem(848);
					Main.LocalPlayer.QuickSpawnItem(866);
					int[] lootTable = {
					ItemID.Topaz,
					ItemID.Sapphire
					};
					int loot = Main.rand.Next(lootTable.Length);
					Main.LocalPlayer.QuickSpawnItem(lootTable[loot], Main.rand.Next(2, 5));
					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<SatchelReward>());
                });

            scarabQuest.OnQuestStart = () => {
                Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MainQuestComplete"));
            };
            scarabQuest.NthQuest = 3;
                #endregion


                _completed = new bool[_quests.Count];

			_questsCompleted = 0;
			_currentQuest = -1;
			_previousQuest = -1;
		}
		private Func<bool> CreateSkipCheck<T1, T2>() where T1 : ModItem where T2 : ModItem
		{
			return () => {
				int index = Main.LocalPlayer.FindItem(ModContent.ItemType<T1>());
				int index2 = Main.LocalPlayer.FindItem(ModContent.ItemType<T2>());
				if (index == -1 && index2 == -1) return false;

				TryRemoveItem(index);
				TryRemoveItem(index2);
				return true;
			};
		}
		private void TryRemoveItem(int index)
		{
			if (index == -1) return;

			Main.LocalPlayer.inventory[index].stack--;
			if (Main.LocalPlayer.inventory[index].stack == 0)
				Main.LocalPlayer.inventory[index].SetDefaults();
		}
		private Quest RegisterQuest(int itemId, string description, string completeText, bool consumeQuest, Action customReward = null)
		{
			Quest q = new Quest(itemId, description, completeText, consumeQuest);

			q.OnComplete = customReward == null ? DefaultQuestReward : customReward;

			_quests.Add(q);
			return q;
		}

		public void WorldLoad(TagCompound tag)
		{
			for (int i = 0; i < _quests.Count; i++) {
				_completed[i] = tag.GetBool("spiritAdventurerQuest_" + GetItemName(_quests[i].ItemID));
			}
			if (tag.ContainsKey("spiritAdventurerCurrentQuest")) {
				_currentQuest = tag.GetInt("spiritAdventurerCurrentQuest");
			}
			else {
				_currentQuest = -1;
			}
			if (tag.ContainsKey("spiritAdventurerTotal")) {
				_questsCompleted = tag.GetInt("spiritAdventurerTotal");
			}
			else {
				_questsCompleted = 0;
			}
		}

		public void WorldSave(TagCompound tag)
		{
			for (int i = 0; i < _quests.Count; i++) {
				tag.Add("spiritAdventurerQuest_" + GetItemName(_quests[i].ItemID), _completed[i]);
			}
			tag.Add("spiritAdventurerCurrentQuest", _currentQuest);
			tag.Add("spiritAdventurerTotal", _questsCompleted);

		}

		private string GetItemName(int id)
		{
			return id < ItemID.Count ? ItemID.Search.GetName(id) : ItemLoader.GetItem(id).Name;
		}

		public void DefaultQuestReward()
		{
			//Gives the item to the player completing the quest. Just change the item and stack amounts based on random rewards here.
			Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, 5);
		}

		public bool QuestCheck()
		{
			Mod mod = SpiritMod.instance;
			if (_currentQuest == -1) {
				//New Quest
				SetNextQuest();
				return true;
			}
			else if (_currentQuest == -2) {
				return true;
			}
			else {
				Quest current = _quests[_currentQuest];
				for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++) {
					if (Main.LocalPlayer.inventory[i].type == current.ItemID) {
						CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 30, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
						"Quest Complete!");
						Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/QuestCompleteEffect"));

						//take the item if the quest says to
						if (current.ConsumeItem) {
							Main.LocalPlayer.inventory[i].stack--;
							if (Main.LocalPlayer.inventory[i].stack <= 0) {
								Main.LocalPlayer.inventory[i].SetDefaults();
							}
						}

						//complete quest
						_completed[_currentQuest] = true;
						_previousQuest = _currentQuest;
						_currentQuest = -1;

						current.OnComplete?.Invoke(); 
						_questsCompleted++;
						if (Main.netMode != NetmodeID.SinglePlayer) {
							ModPacket packet = _mod.GetPacket();
							packet.Write((byte)MessageType.AdventurerQuestCompleted);
							packet.Send();
							NetMessage.SendData(MessageID.WorldData);
						}
						return false;
					}
				}
			}
			return true;
		}

		public void SetNextQuest()
		{
			List<int> availableIndexes = AvailableQuests();
			if (availableIndexes.Count == 0) {
				_currentQuest = -2;
				return;
			}
			if (_currentQuest != -1 && !(_quests[_currentQuest].TrySkipQuest?.Invoke()).GetValueOrDefault()) {
				return;
			}
			_currentQuest = Main.rand.Next(availableIndexes);
			if (Main.netMode != NetmodeID.SinglePlayer) {
				ModPacket packet = _mod.GetPacket();
				packet.Write((byte)MessageType.AdventurerNewQuest);
				packet.Write(_currentQuest);
				packet.Send();
			}
			_quests[_currentQuest].OnQuestStart?.Invoke();
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
		}
		public void HandlePacket(MessageType type, BinaryReader reader)
		{
			switch (type) {
				case MessageType.AdventurerNewQuest:
					_currentQuest = reader.ReadInt32();
					break;
				case MessageType.AdventurerQuestCompleted:
					_completed[_currentQuest] = true;
					_previousQuest = _currentQuest;
					_currentQuest = -1;
					_questsCompleted++;
					break;
			}
		}
		public string GetChatText()
		{
			if (_previousQuest != -1) {
				string text = _quests[_previousQuest].CompleteText;
				_previousQuest = -1;
				Main.npcChatCornerItem = 0;
				return text;
			}
			if (_currentQuest < 0) return "";
			Main.npcChatCornerItem = _quests[_currentQuest].ItemID;
			return _quests[_currentQuest].Description;
		}

		public bool QuestsAvailable() => AvailableQuests().Count > 0;

		private List<int> AvailableQuests()
		{
			List<int> availableIndexes = new List<int>();
			for (int i = 0; i < _completed.Length; i++) {
				//Main.NewText(GetItemName(_quests[i].ItemID) + ": " + _completed[i]);
				if (_quests[i].NthQuest == _questsCompleted + 1) {
					availableIndexes.Clear();
					availableIndexes.Add(i);
					return availableIndexes;
				}

				if (!_completed[i] && _quests[i].CanGiveQuest() && _quests[i].NthQuest == -1) availableIndexes.Add(i);
			}
			return availableIndexes;
		}

		private class Quest
		{
			public int ItemID;
			public string Description;
			public string CompleteText;
			public bool ConsumeItem;
			public int NthQuest;

			public Action OnComplete;
			public Action OnQuestStart;
			public Func<bool> CanGiveQuest;
			public Func<bool> TrySkipQuest;

			public Quest(int item, string description, string completeText, bool consume)
			{
				ItemID = item;
				Description = description;
				CompleteText = completeText;
				ConsumeItem = consume;
				OnComplete = null;
				NthQuest = -1;
				CanGiveQuest = () => { return true; };
				TrySkipQuest = () => { return true; };
			}
		}
	}
}

