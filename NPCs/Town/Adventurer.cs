using SpiritMod.Items.Accessory;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
    [AutoloadHead]
    public class Adventurer : ModNPC
    {
        public override string Texture => "SpiritMod/NPCs/Town/Adventurer";

        public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Adventurer_Alt_1" };

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Adventurer");
            Main.npcFrameCount[npc.type] = 26;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 1500;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 16;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
        }

        public override void SetDefaults() {
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

        public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
            return Main.player.Any(x => x.active) && !NPC.AnyNPCs(NPCType<BoundAdventurer>()) && !NPC.AnyNPCs(NPCType<Adventurer>());
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer3"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Adventurer/Adventurer4"));
            }
        }

        public override string TownNPCName() {
            string[] names = { "Morgan", "Adam", "Aziz", "Temir", "Evan", "Senzen", "Johanovic", "Adrian" };
            return Main.rand.Next(names);
        }

        public override string GetChat() {
            List<string> dialogue = new List<string>
            {
                "I've been all around this world, and I've got so many things for you to see.",
                "Lovely house you've got here. It's much better lodging than when those savages from The Briar hung me over a spit.",
                "Every dawn brings with it a new opportunity for a journey! You interested?",
                "We're pretty similar, you and I. I sense our shared thirst for adventure.",
                "Buy my stuff and go out there! See what the world has to offer, like I have.",
                "From the depths of temples and the heights of space, peruse my wares.",
            };

            int travellingMerchant = NPC.FindFirstNPC(NPCID.TravellingMerchant);
            if(travellingMerchant >= 0) {
                dialogue.Add($"Ah! It's {Main.npc[travellingMerchant].GivenName}! We've often met on our journeys. I still haven't found all those exotic jungles he speaks of.");
            }

            int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
            if(armsDealer >= 0) {
                dialogue.Add($"Got some great prices today! {Main.npc[armsDealer].GivenName}'s wares can't compete! They literally can't. I don't sell guns anymore.");
            }

            int merchant = NPC.FindFirstNPC(NPCID.Merchant);
            if(merchant >= 0) {
                dialogue.Add($"I swear I've got more goods for sale than {Main.npc[merchant].GivenName}.");
            }

            dialogue.AddWithCondition("A shimmering blue light's on the horizon. Wonder what that's about, huh?", NPC.downedMechBossAny);
            dialogue.AddWithCondition("Like the moon, my merchandise is inconstant.", !Main.dayTime);
            dialogue.AddWithCondition("Everyone seems to be so aggressive tonight. With the zombies knocking at our door, I think you should buy stuff and head underground as quick as you can. Can you take me with you?", Main.bloodMoon);
            dialogue.AddWithCondition("The goblins are more organized than you'd think- I saw their mages build a huge tower over yonder. You should check it out sometime!", MyWorld.gennedTower && !NPC.AnyNPCs(ModContent.NPCType<Rogue>()) && NPC.AnyNPCs(ModContent.NPCType<BoundRogue>()));
            dialogue.AddWithCondition("My old business partner turned to the bandit life a few years ago. I wonder if he's doing okay. I think his associates have set up a bandit camp somewhere near the seas.", !MyWorld.gennedTower && !NPC.AnyNPCs(ModContent.NPCType<Rogue>()) && NPC.AnyNPCs(ModContent.NPCType<BoundRogue>()));

            return Main.rand.Next(dialogue);
        }

        public override void SetupShop(Chest shop, ref int nextSlot) {
            int glowStick = Main.moonPhase == 4 && !Main.dayTime ? ItemID.SpelunkerGlowstick : ItemID.StickyGlowstick;
            AddItem(ref shop, ref nextSlot, glowStick);

            switch(Main.moonPhase) {
                case 4 when !Main.dayTime:
                    AddItem(ref shop, ref nextSlot, ItemID.CursedTorch);
                    break;

                case 7 when !Main.dayTime:
                    AddItem(ref shop, ref nextSlot, ItemID.UltrabrightTorch);
                    break;
            }

            AddItem(ref shop, ref nextSlot, ItemID.TrapsightPotion, 2000);
            AddItem(ref shop, ref nextSlot, ItemID.DartTrap, 5000);
            AddItem(ref shop, ref nextSlot, ItemID.TreasureMap, 50000);
            AddItem(ref shop, ref nextSlot, ItemID.PinkJellyfish, 25000, NPC.FindFirstNPC(NPCID.Angler) >= 0);
            AddItem(ref shop, ref nextSlot, ItemType<GoldSword>());
            AddItem(ref shop, ref nextSlot, ItemType<PlatinumSword>());
            AddItem(ref shop, ref nextSlot, ItemType<ManaFlame>());
            AddItem(ref shop, ref nextSlot, ItemID.WhoopieCushion, 15000, NPC.downedBoss2);
            AddItem(ref shop, ref nextSlot, ItemID.BottledHoney, 5000, NPC.downedQueenBee);
            AddItem(ref shop, ref nextSlot, ItemID.Book, 20, NPC.downedBoss3);
            AddItem(ref shop, ref nextSlot, ItemID.Skull, 100000, NPC.downedBoss3);
            AddItem(ref shop, ref nextSlot, ItemType<Items.Placeable.Furniture.SkullStick>(), 1000, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
            AddItem(ref shop, ref nextSlot, ItemType<AncientBark>(), 200, Main.LocalPlayer.GetSpiritPlayer().ZoneReach);
            AddItem(ref shop, ref nextSlot, ItemType<PolymorphGun>(), check: NPC.downedMechBossAny);
            AddItem(ref shop, ref nextSlot, ItemType<RedMapPin>(), 5000);
            AddItem(ref shop, ref nextSlot, ItemType<BlueMapPin>(), 5000);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
            damage = 13;
            knockback = 3f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
            cooldown = 5;
            randExtraCooldown = 5;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
            projType = 507;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
            multiplier = 11f;
            randomOffset = 2f;
        }

        private bool clickedQuest = false;
        public override void PostAI() {
            if(Main.LocalPlayer.talkNPC == -1) {
                clickedQuest = false;
            }
        }

        public override void SetChatButtons(ref string button, ref string button2) {
            button = Language.GetTextValue("LegacyInterface.28");
            if(SpiritMod.AdventurerQuests.QuestsAvailable()) {
                if(clickedQuest) {
                    if(SpiritMod.AdventurerQuests.CurrentQuestSkippable) {
                        button2 = "Skip";
                    }
                } else {
                    button2 = "Quest";
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop) {
            if(firstButton) {
                shop = true;
                clickedQuest = false;
            } else if(SpiritMod.AdventurerQuests.QuestsAvailable()) {
                Main.PlaySound(SoundID.Chat);
                if(!clickedQuest) {
                    //Check if there is a current quest, if the player has gotten everything required, etc.
                    //returns true if the quest is not complete yet.
                    clickedQuest = SpiritMod.AdventurerQuests.QuestCheck();
                } else {
                    SpiritMod.AdventurerQuests.SetNextQuest();
                }

                Main.npcChatText = SpiritMod.AdventurerQuests.GetChatText();
            }
        }
    }
}
