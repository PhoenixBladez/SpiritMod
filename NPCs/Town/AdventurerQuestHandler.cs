using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

using SpiritMod;

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

        public bool CurrentQuestSkippable
        {
            get
            {
                if (_currentQuest == -1) return true;
                if (_currentQuest == -2) return false;
                return _quests[_currentQuest].NthQuest == -1;
            }
        }

        public AdventurerQuestHandler(Mod mod)
        {
            _mod = mod;

            _quests = new List<Quest>();

            //This is just a normal reward quest, uses default reward system
            Quest ShadowflameStaff = RegisterQuest(mod.ItemType("ShadowflameStoneStaff"),

                "I've heard tell of an Arcane Goblin Tower near the far shores of this land." +
                " There's supposed to be a staff inside that holds tremendous power. Could you check it out for me?" +
                " I'm still vacationing. Just bring back the staff and show it to me, and I'll reward ya handsomely.",

                "Hope those goblins didn't give you too much trouble, heh." +
                " Wow, look at that craftwork! It's supposed to be real powerful, too." +
                " So maybe you won't get killed while you're out there adventuring, yeah?", false);
            ShadowflameStaff.CanGiveQuest = () =>
            {
                return MyWorld.gennedTower && !MyWorld.gennedBandits;
            };

            Quest SepulchreChest = RegisterQuest(mod.ItemType("SepulchreChest"),

                "You ever wonder why there're so many skeletons underground?" +
                " Turns out that there was a band of necromancers that holed up in the caverns all across the world and performed all kinds of experiments." +
                " Well, lucky for us they're gone! But their Sepulchres still remain. Mind grabbin' me a chest from there? I'd like to study their architecture further. Don't turn into a skeleton!",

                "Thanks, bud. After studying this artifact, I've managed to reproduce a few vases in their weird style. " +
                "If you ever want your house to have a spooky vibe, here ya go. Don't go conjuring any skeletons, now.", true);

            //Quest 2
            Quest scarabQuest = RegisterQuest(mod.ItemType("ScarabIdolQuest"), 
                
                "The sands of the desert hide a lot of secrets beneath 'em. " +
                "There's supposed to be an Ancient Ziggurat buried near the surface of one of those wastelands. " +
                "Could ya head down there and scavenge some relics from me? I've got a hunch, but I need to confirm it...", 
                
                "I knew it. I was polishin' up this old thing when it started to look real familiar. " +
                "That's a Scarab Idol right there. I'm warning ya, don't mess with it until you get real strong. " +
                "Me and some bounty hunters tried to take that thing on years ago. We barely escaped with our lives. Be safe, kid.", true,
                () => { Main.LocalPlayer.QuickSpawnItem(mod.ItemType("ScarabIdol"), 1);
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.LocalPlayer.QuickSpawnItem(ItemID.Topaz, Main.rand.Next(2, 5));
                    }
                    else
                    {
                        Main.LocalPlayer.QuickSpawnItem(ItemID.Sapphire, Main.rand.Next(2, 5));
                    }
                        Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(8, 15));
                });

            scarabQuest.NthQuest = 2;

            _completed = new bool[_quests.Count];

            _questsCompleted = 0;
            _currentQuest = -1;
            _previousQuest = -1;
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
            for (int i = 0; i < _quests.Count; i++)
            {
                _completed[i] = tag.GetBool("spiritAdventurerQuest_" + GetItemName(_quests[i].ItemID));
            }
            if (tag.ContainsKey("spiritAdventurerCurrentQuest"))
            {
                _currentQuest = tag.GetInt("spiritAdventurerCurrentQuest");
            }
            else
            {
                _currentQuest = -1;
            }
            if (tag.ContainsKey("spiritAdventurerTotal"))
            {
                _questsCompleted = tag.GetInt("spiritAdventurerTotal");
            }
            else
            {
                _questsCompleted = 0;
            }
        }

        public void WorldSave(TagCompound tag)
        {
            for (int i = 0; i < _quests.Count; i++)
            {
                tag.Add("spiritAdventurerQuest_" + GetItemName(_quests[i].ItemID), _completed[i]);
            }
            tag.Add("spiritAdventurerCurrentQuest", _currentQuest);
            tag.Add("spiritAdventurerTotal", _questsCompleted);

            _questsCompleted = 0;
            _currentQuest = -1;
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
            if (_currentQuest == -1)
            {
                //New Quest
                SetNextQuest();
                return true;
            }
            else if (_currentQuest == -2)
            {
                return true;
            }
            else
            {
                Quest current = _quests[_currentQuest];
                for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
                {
                    if (Main.LocalPlayer.inventory[i].type == current.ItemID)
                    {
                        CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 30, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(29, 240, 255, 100),
                        "Quest Complete!");
                        //take the item if the quest says to
                        if (current.ConsumeItem)
                        {
                            Main.LocalPlayer.inventory[i].stack--;
                            if (Main.LocalPlayer.inventory[i].stack <= 0)
                            {
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

        public void SetNextQuest()
        {
            List<int> availableIndexes = AvailableQuests();
            if (availableIndexes.Count == 0)
            {
                _currentQuest = -2;
                return;
            }
            _currentQuest = Main.rand.Next(availableIndexes);
        }

        public string GetChatText()
        {
            if (_previousQuest != -1)
            {
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
            for (int i = 0; i < _completed.Length; i++)
            {
                //Main.NewText(GetItemName(_quests[i].ItemID) + ": " + _completed[i]);
                if (_quests[i].NthQuest == _questsCompleted + 1)
                {
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
            public Func<bool> CanGiveQuest;

            public Quest(int item, string description, string completeText, bool consume)
            {
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

