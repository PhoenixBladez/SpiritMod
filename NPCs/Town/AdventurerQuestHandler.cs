using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.HunterArmor;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Material;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Tiles.Furniture.Critters;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using SpiritMod;
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
                if(_currentQuest == -1) return true;
                if(_currentQuest == -2) return false;
                return _quests[_currentQuest].NthQuest == -1;
            }
        }

        public AdventurerQuestHandler(Mod mod) {
            _mod = mod;

            _quests = new List<Quest>();

            //This is just a normal reward quest, uses default reward system
            Quest shadowflameStaffQuest = RegisterQuest(ModContent.ItemType<ShadowflameStoneStaff>(),

                "I've heard tell of an Arcane Goblin Tower near the far shores of this land." +
                " There's supposed to be a staff inside that holds tremendous power. Could you check it out for me?" +
                " I'm still vacationing. Just bring back the staff and show it to me, and I'll reward ya handsomely.",

                "Hope those goblins didn't give you too much trouble, heh." +
                " Wow, look at that craftwork! It's supposed to be real powerful, too." +
                " So maybe you won't get killed while you're out there adventuring, yeah?", false);
            shadowflameStaffQuest.CanGiveQuest = () => {
                return MyWorld.gennedTower && !MyWorld.gennedBandits;
            };

            Quest sepulchreChestQuest = RegisterQuest(ModContent.ItemType<SepulchreChest>(),

                "You ever wonder why there're so many skeletons underground?" +
                " Turns out that there was a band of necromancers that holed up in the caverns all across the world and performed all kinds of experiments." +
                " Well, lucky for us they're gone! But their Sepulchres still remain. Mind grabbin' me a chest from there? I'd like to study their architecture further. Don't turn into a skeleton!",

                "Thanks, bud. After studying this artifact, I've managed to reproduce a few vases in their weird style. " +
                "If you ever want your house to have a spooky vibe, here ya go. Don't go conjuring any skeletons, now.", true,

                () => {
                    MyWorld.sepulchreComplete = true;
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
                });

            Quest jadeStaffQuest = RegisterQuest(ModContent.ItemType<JadeStaff>(),

                "Hey, lad. I've got another proposition for ya." +
                " You see, I've been looking at some old maps and I've learned about a cluster of Floating Pagodas at the far end of this world." +
                " Trouble is, I can't make out whether it's to the left or right, so would ya go explorin' for me? I'm looking for an ornate staff, hundreds of years old. Happy hunting!",

                "Undead samurai? Vengeful spirits? Sounds like a riot! Wish I could've been there." +
                " Either way, this ol' staff is mighty powerful. Quite durable, too, considering it's nearly a millenium old." +
                " Use it carefully, alright? That there is one of a kind.", false,

                () => {
                    int[] lootTable = {
                    ModContent.ItemType<Shrine1>(),
                    ModContent.ItemType<Shrine2>(),
                    ModContent.ItemType<Shrine3>(),
                    };
                    int loot = Main.rand.Next(lootTable.Length);
                    int loot1 = Main.rand.Next(lootTable.Length);
                    Main.LocalPlayer.QuickSpawnItem(lootTable[loot]);
                    Main.LocalPlayer.QuickSpawnItem(lootTable[loot1]);
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.LocalPlayer.QuickSpawnItem(ItemID.Kimono);
                    }
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
                });
            jadeStaffQuest.CanGiveQuest = () => {
                return NPC.downedBoss1;
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
                     Main.LocalPlayer.QuickSpawnItem(ItemID.Vine, 2);
                     if (Main.rand.Next(3) == 0)
                     {
                         Main.LocalPlayer.QuickSpawnItem(ItemID.TigerSkin);
                     }
                     Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
                 }

                );
            hornetFishQuest.OnQuestStart = () => {
                MyWorld.spawnHornetFish = true;
            };

			Quest mushroomQuest = RegisterQuest(ModContent.ItemType<VibeshroomItem>(),

				"I've been hearin' stories about some new flora that's cropped up around those strange Mushroom Forests recently." +
				" These lil' buggers seem to just sway from side to side, as if they're dancin'." +
				" I have no real motive this time around, I just wanna see one of 'em. Mind fetching one for me?",

				" It's a cutie for sure. I've put this critter into a little jar. I'm sure it could spice up my home." +
				" We all could use a little more cute from time to time. If you find any more, don't hurt the little things!", true,
				() =>
				{
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<VibeshroomJarItem>(), 1);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.LocalPlayer.QuickSpawnItem(868);
                    }
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<GlowRoot>(), Main.rand.Next(1, 4));
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
                });

            mushroomQuest.OnQuestStart = () => {
                MyWorld.spawnVibeshrooms = true;
            };

            Quest explorerQuestMushroom = RegisterQuest(ModContent.ItemType<ExplorerScrollMushroomFull>(),

				"Up for a little explorin'? This world's massive, and even I haven't seen it all." +
				" I'd like ya to head out there and map out the far reaches of this land. Specifically, I'm looking for info on any nearby Glowing Mushroom Caverns" +
				" Take this blank map. After you stumble upon one of 'em caverns, wander around for a while and take some notes for me, alright? Return to me when the map's all filled out.",

				"Found a Glowing Mushroom Cavern, did ya? Glad you made it back in one piece, lad. Those caves may seem all light and charming, but they aren't a jokin' matter." +
				" This should help me study the local environment more. Maybe I'll plan a few raids in the future? Thanks for the intel.", true, 

				() =>
                {
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<PinBlue>());
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MapScroll>(), Main.rand.Next(1, 3));
                    Main.LocalPlayer.QuickSpawnItem(ItemID.MushroomGrassSeeds, Main.rand.Next(2, 3));
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));

                });

            explorerQuestMushroom.OnQuestStart = () => {
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ExplorerScrollMushroomEmpty>());
            };	

            Quest slayerQuestWinterborn = RegisterQuest(ModContent.ItemType<WinterbornSlayerScrollFull>(),

				"A few of my associates have scouted a growing threat in the icy caverns." +
                " Sometimes, they even get bold enough to roam the surface durin' heavy blizzards. We're calling 'em Winterborn, and I'd like for you to thin their numbers a bit." +
				" They're a tough foe, so be safe, lad. This contract should help keep track of your kills. Oh, and I'd be cautious when roaming those caverns. Some of those Ice Scupltures look too lifelike...",

				"You've grown into a top-notch warrior, lad. I'm not even sure I could've handled ten of those freaky cadavers. I'm sure the frozen tundra will be a little safer to explore, thanks to you." +
				" I did some explorin', and managed to wrangle some of these Ice Sculptures. Please take 'em, they still freak me out. But I don't think they're dangerous. Maybe. Come back for more work any time.", true,

				() =>
                {
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

                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
                });
            slayerQuestWinterborn.OnQuestStart = () => {
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

                () =>
                {
                    int[] lootTable1 = {
                    ModContent.ItemType<TargetCan>(),
                    ModContent.ItemType<TargetBottle>(),
                    };
                    int loot3 = Main.rand.Next(lootTable1.Length);
                    Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeholderMask>());
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<MarbleChunk>(), Main.rand.Next(6, 11));
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
                });
            slayerQuestBeholder.OnQuestStart = () => {
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<BeholderSlayerScrollEmpty>());
            };
            slayerQuestBeholder.CanGiveQuest = () => {
                return NPC.downedBoss2;
            };


            Quest slayerQuestValkyrie = RegisterQuest(ModContent.ItemType<ValkyrieSlayerScrollFull>(),

                "Just who I've been meanin' to talk to! You ever been high enough where those infuriatin' harpies can shoot at ya?"+
				" Well, to make things worse, I've heard tales of a harpy clad in armor and weapons!" +
				" But I'm sure you can handle it, lad. We've taken to calling it a Valkyrie, but don't go joinin' the afterlife when you take it on! This contract should help keep track of your mission.",
                
				"Gotta hand it to you, that was one tough customer. I'm glad you made it back okay, though. I will say, though- the view up there is nothing short of breathtaking" +
				" You ever seen an aurora from space? It's a feelin' I can't put into words. Hopefully your next journey skyward will be more peaceful, eh?", true,

                () =>
                {
                    int[] lootTable1 = {
                    ModContent.ItemType<TargetCan>(),
                    ModContent.ItemType<TargetBottle>(),
                    };
                    int loot3 = Main.rand.Next(lootTable1.Length);
                    Main.LocalPlayer.QuickSpawnItem(lootTable1[loot3], Main.rand.Next(18, 30));
                    Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ChaosPearl>(), Main.rand.Next(5, 9));
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.LocalPlayer.QuickSpawnItem(1987);
                    }
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(99, 175));
                });
            slayerQuestValkyrie.OnQuestStart = () => {
                Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<ValkyrieSlayerScrollEmpty>());
            };


            //Scripted Quest 2
            Quest sacredVineQuest = RegisterQuest(ModContent.ItemType<SacredVine>(),

				"Ever since I was captured by those savages from the Briar, I've been doin' some research on the place." +
				" That altar you found me at is supposed to harbor a really venegeful nature spirit." +
				" However, the vines that spirit's made of are said to possessive some mystical regenerative properties, or somethin'. Mind investigating? And kill some savages for me while you're at it.",

				"Revenge is sweet! Now those feral beasts in the Briar are sure to think twice about capturin' innocent adventurers in the future! Hopefully." +
				"I've whipped up some healin' potions with those vines you gave me. I'm sure it'll help out when you're in a tight spot, eh?", true,

				() => {
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<EnchantedLeaf>(), Main.rand.Next(5, 9));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<AncientBark>(), Main.rand.Next(10, 25));
					Main.LocalPlayer.QuickSpawnItem(ModContent.ItemType<Items.Armor.Masks.GladeWraithMask>(), 1);
					Main.LocalPlayer.QuickSpawnItem(ItemID.HealingPotion, 4);

					Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
				});

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
                    int[] lootTable = {
                    ItemID.Topaz,
                    ItemID.Sapphire
                    };
                    int loot = Main.rand.Next(lootTable.Length);
                    Main.LocalPlayer.QuickSpawnItem(lootTable[loot], Main.rand.Next(2, 5));
                    Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(40, 75));
                });

            scarabQuest.NthQuest = 3;



            _completed = new bool[_quests.Count];

            _questsCompleted = 0;
            _currentQuest = -1;
            _previousQuest = -1;

        }

        private Quest RegisterQuest(int itemId, string description, string completeText, bool consumeQuest, Action customReward = null) {
            Quest q = new Quest(itemId, description, completeText, consumeQuest);

            q.OnComplete = customReward == null ? DefaultQuestReward : customReward;

            _quests.Add(q);
            return q;
        }

        public void WorldLoad(TagCompound tag) {
            for(int i = 0; i < _quests.Count; i++) {
                _completed[i] = tag.GetBool("spiritAdventurerQuest_" + GetItemName(_quests[i].ItemID));
            }
            if(tag.ContainsKey("spiritAdventurerCurrentQuest")) {
                _currentQuest = tag.GetInt("spiritAdventurerCurrentQuest");
            } else {
                _currentQuest = -1;
            }
            if(tag.ContainsKey("spiritAdventurerTotal")) {
                _questsCompleted = tag.GetInt("spiritAdventurerTotal");
            } else {
                _questsCompleted = 0;
            }
        }

        public void WorldSave(TagCompound tag) {
            for(int i = 0; i < _quests.Count; i++) {
                tag.Add("spiritAdventurerQuest_" + GetItemName(_quests[i].ItemID), _completed[i]);
            }
            tag.Add("spiritAdventurerCurrentQuest", _currentQuest);
            tag.Add("spiritAdventurerTotal", _questsCompleted);

            _questsCompleted = 0;
            _currentQuest = -1;
        }

        private string GetItemName(int id) {
            return id < ItemID.Count ? ItemID.Search.GetName(id) : ItemLoader.GetItem(id).Name;
        }

        public void DefaultQuestReward() {
            //Gives the item to the player completing the quest. Just change the item and stack amounts based on random rewards here.
            Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, 5);
        }

        public bool QuestCheck() {
            Mod mod = SpiritMod.instance;
            if(_currentQuest == -1) {
                //New Quest
                SetNextQuest();
                return true;
            } else if(_currentQuest == -2) {
                return true;
            } else {
                Quest current = _quests[_currentQuest];
                for(int i = 0; i < Main.LocalPlayer.inventory.Length; i++) {
                    if(Main.LocalPlayer.inventory[i].type == current.ItemID) {
                        CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 30, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
                        "Quest Complete!");
                        Main.PlaySound(SoundLoader.customSoundType, Main.LocalPlayer.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/QuestCompleteEffect"));

                        //take the item if the quest says to
                        if (current.ConsumeItem) {
                            Main.LocalPlayer.inventory[i].stack--;
                            if(Main.LocalPlayer.inventory[i].stack <= 0) {
                                Main.LocalPlayer.inventory[i].SetDefaults();
                            }
                        }

                        //complete quest
                        _completed[_currentQuest] = true;
                        _previousQuest = _currentQuest;
                        _currentQuest = -1;

                        current.OnComplete?.Invoke();
                        _questsCompleted++;

                        return false;
                    }
                }
            }
            return true;
        }

        public void SetNextQuest() {
            List<int> availableIndexes = AvailableQuests();
            if(availableIndexes.Count == 0) {
                _currentQuest = -2;
                return;
            }
            _currentQuest = Main.rand.Next(availableIndexes);
            _quests[_currentQuest].OnQuestStart?.Invoke();
        }

        public string GetChatText() {
            if(_previousQuest != -1) {
                string text = _quests[_previousQuest].CompleteText;
                _previousQuest = -1;
                Main.npcChatCornerItem = 0;
                return text;
            }
            if(_currentQuest < 0) return "";
            Main.npcChatCornerItem = _quests[_currentQuest].ItemID;
            return _quests[_currentQuest].Description;
        }

        public bool QuestsAvailable() => AvailableQuests().Count > 0;

        private List<int> AvailableQuests() {
            List<int> availableIndexes = new List<int>();
            for(int i = 0; i < _completed.Length; i++) {
                //Main.NewText(GetItemName(_quests[i].ItemID) + ": " + _completed[i]);
                if(_quests[i].NthQuest == _questsCompleted + 1) {
                    availableIndexes.Clear();
                    availableIndexes.Add(i);
                    return availableIndexes;
                }

                if(!_completed[i] && _quests[i].CanGiveQuest() && _quests[i].NthQuest == -1) availableIndexes.Add(i);
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

            public Quest(int item, string description, string completeText, bool consume) {
                ItemID = item;
                Description = description;
                CompleteText = completeText;
                ConsumeItem = consume;
                OnComplete = null;
                NthQuest = -1;
                CanGiveQuest = () => { return true; };
            }
        }
    }
}

