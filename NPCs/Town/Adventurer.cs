using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class Adventurer : ModNPC
	{
        public static int _type;

        public override string Texture
        {
            get
            {
                return "SpiritMod/NPCs/Town/Adventurer";
            }
        }

        public override string[] AltTextures
        {
            get
            {
                return new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adventurer");
            Main.npcFrameCount[npc.type] = 26;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 1500;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 16;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Guide);
            npc.townNPC = true;
            npc.friendly = true;
            npc.aiStyle = 7;
            npc.damage = 30;
            npc.defense = 30;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.4f;
            animationType = NPCID.Guide;
        }
        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active)
                {
                    if (!NPC.AnyNPCs(mod.NPCType("BoundAdventurer")) && !NPC.AnyNPCs(mod.NPCType("Adventurer")))
                    {
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer3"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer4"));
            }
        }
        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(8))
            {
                case 0:
                    return "Morgan";
                case 1:
                    return "Adam";
                case 2:
                    return "Aziz";
                case 3:
                    return "Temir";
                case 4:
                    return "Evan";
                case 5:
                    return "Senzen";
                case 6:
                    return "Johanovic";
                default:
                    return "Adrian";
            }
        }

        public override string GetChat()
        {
            int TravellingMerchant = NPC.FindFirstNPC(NPCID.TravellingMerchant);
            if (TravellingMerchant >= 0 && Main.rand.Next(8) == 0)
                return "Ah! It's " + Main.npc[TravellingMerchant].GivenName + "! We've often met on our journeys. I still haven't found all those exotic jungles he speaks of.";

            int ArmsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
            if (ArmsDealer >= 0 && Main.rand.Next(8) == 0)
                return "Got some great prices today!" + Main.npc[ArmsDealer].GivenName + "'s wares can't compete! They literally can't. I don't sell guns anymore.";

            int Merchant = NPC.FindFirstNPC(NPCID.Merchant);
            if (Merchant >= 0 && Main.rand.Next(8) == 0)
                return "I swear I've got more goods for sale than " + Main.npc[Merchant].GivenName + ".";

            if (NPC.downedMechBossAny && Main.rand.Next(8) == 0)
                return "A shimmering blue light's on the horizon. Wonder what that's about, huh?";

            if (!Main.dayTime && Main.rand.Next(6) == 0)
                return "Like the moon, my merchandise is inconstant.";

            if (Main.bloodMoon && Main.rand.Next(4) == 0)
                return "Everyone seems to be so aggressive tonight. With the zombies knocking at our door, I think you should buy stuff and head underground as quick as you can. Can you take me with you?";

            switch (Main.rand.Next(8))
            {
                case 0:
                    return "I've been all around this world, and I've got so many things for you to see.";
                case 1:
                    if (MyWorld.gennedTower)
                    {
                        return "The goblins are more organized than you'd think- I saw their mages build a huge tower over yonder. You should check it out sometime!";
                    }
                    else
                    {
                        return "My old business partner turned to the bandit life a few years ago. I wonder if he's doing okay. I think his associates have set up a bandit camp somewhere near the seas.";
                    }
                case 2:
                    return "Lovely house you've got here. It's much better lodging than when those savages from The Briar hung me over a spit.";
                case 3:
                    return "Every dawn brings with it a new opportunity for a journey! You interested?";
                case 4:
                    return "We're pretty similar, you and I. I sense our shared thirst for adventure.";
                case 5:
                    return "Buy my stuff and go out there! See what the world has to offer, like I have.";
                default:
                    return "From the depths of temples and the heights of space, peruse my wares.";
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            //Rope Coil
            shop.item[nextSlot].SetDefaults(ItemID.RopeCoil);
            nextSlot++;
            //Glowsticks
            if (Main.moonPhase == 4 && !Main.dayTime)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SpelunkerGlowstick);
                nextSlot++;
            }
            else
            {
                shop.item[nextSlot].SetDefaults(ItemID.StickyGlowstick);
                nextSlot++;
            }
            //Torches
            if (Main.moonPhase == 4 && !Main.dayTime)
            {
                shop.item[nextSlot].SetDefaults(ItemID.CursedTorch);
                nextSlot++;
            }
            else if (Main.moonPhase == 7 && !Main.dayTime)
            {
                shop.item[nextSlot].SetDefaults(ItemID.UltrabrightTorch);
                nextSlot++;
            }
            else
            {
                shop.item[nextSlot].SetDefaults(ItemID.Torch);
                nextSlot++;
            }
            //Dangersense
            shop.item[nextSlot].SetDefaults(ItemID.TrapsightPotion);
            shop.item[nextSlot].value = 2000;
            nextSlot++;
            //Dart Trap
            shop.item[nextSlot].SetDefaults(ItemID.DartTrap);
            shop.item[nextSlot].value = 5000;
            nextSlot++;
            //Treasure Map
            shop.item[nextSlot].SetDefaults(ItemID.TreasureMap);
            shop.item[nextSlot].value = 50000;
            nextSlot++;
            //Angler
            int Angler = NPC.FindFirstNPC(NPCID.Angler);
            if (Angler >= 0)
            {
                //JellyBait
                shop.item[nextSlot].SetDefaults(ItemID.PinkJellyfish);
                shop.item[nextSlot].value = 45000;
                nextSlot++;
            }
            //Swords
            shop.item[nextSlot].SetDefaults(mod.ItemType("GoldSword"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("PlatinumSword"));
            nextSlot++;
            //Lunar Wisp		
            shop.item[nextSlot].SetDefaults(mod.ItemType("ManaFlame"));
            nextSlot++;
            //Whoopie Cushion
            if (NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(ItemID.WhoopieCushion);
                shop.item[nextSlot].value = 15000;
                nextSlot++;
            }
            if (NPC.downedQueenBee)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BottledHoney);
                shop.item[nextSlot].value = 5000;
                nextSlot++;
            }
            if (NPC.downedBoss3)
            {
                //Book
                shop.item[nextSlot].SetDefaults(ItemID.Book);
                shop.item[nextSlot].value = 20;
                nextSlot++;
                //Skull
                shop.item[nextSlot].SetDefaults(ItemID.Skull);
                shop.item[nextSlot].value = 100000;
                nextSlot++;
            }
            Player closest = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
            if (closest.GetSpiritPlayer().ZoneReach)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("SkullStick"));
                shop.item[nextSlot].value = 1000;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("AncientBark"));
                shop.item[nextSlot].value = 200;
                nextSlot++;
            }
            if (NPC.downedMechBossAny == true)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("PolymorphGun"));
                nextSlot++;
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 13;
            knockback = 3f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 5;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = 507;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 11f;
            randomOffset = 2f;
        }

        //ADDITIONS

        private bool clickedQuest = false;

        public override void PostAI()
        {
            if (Main.LocalPlayer.talkNPC == -1) clickedQuest = false;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Lang.inter[28].Value;
            if (SpiritMod.AdventurerQuests.QuestsAvailable())
            {
                if (clickedQuest)
                {
                    if (SpiritMod.AdventurerQuests.CurrentQuestSkippable)
                    {
                        button2 = "Skip";
                    }
                }
                else
                {
                    button2 = "Quest";
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
                clickedQuest = false;
            }
            else if (SpiritMod.AdventurerQuests.QuestsAvailable())
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(24, 1));
                if (!clickedQuest)
                {
                    //Check if there is a current quest, if the player has gotten everything required, etc.
                    //returns true if the quest is not complete yet.
                    clickedQuest = SpiritMod.AdventurerQuests.QuestCheck();
                }
                else
                {
                    SpiritMod.AdventurerQuests.SetNextQuest();
                }

                Main.npcChatText = SpiritMod.AdventurerQuests.GetChatText();
            }
        }
    }
}
