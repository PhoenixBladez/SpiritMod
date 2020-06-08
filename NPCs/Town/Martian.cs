using SpiritMod.Items.Ammo;
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
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Thrown;
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
    public class Martian : ModNPC
    {
        public override string Texture => "SpiritMod/NPCs/Town/Martian";

        public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/Martian_Alt_1" };

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Martian Scientist");
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
            npc.knockBackResist = 0.2f;
            animationType = NPCID.Guide;
        }

        public override void HitEffect(int hitDirection, double damage) {
            int dustAmount = npc.life > 0 ? 1 : 5;
            for(int k = 0; k < dustAmount; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 206);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
            return NPC.downedMartians && Main.player.Any(x => x.active && x.inventory.Any(y => y.type == ItemID.GoldCoin));
        }

        public override string TownNPCName() {
            string[] names = { "Marvin", "Grunk", "24-x3888", "Dr. Quagnar", "Dr. Boidzerg", "Zorgnax", "Gazorp", "Xanthor" };
            return Main.rand.Next(names);
        }

        public override string GetChat() {
            List<string> dialogue = new List<string>
            {
                "Oh, I'm going to blow it up; it obstructs my view of Venus.",
                "EX-TERM-IN-ATE",
                "So many interesting creatures on this planet!",
                "Open your mouth and say 'ah'... your other mouth.",
                "How many earth-dollars do you want for your earth brain?",
            };

            int armsDealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
            if(armsDealer >= 0) {
                dialogue.Add($"The illudium Q-36 explosive space modulator! {Main.npc[armsDealer].GivenName} has stolen the space modulator!");
            }

            int tinkerer = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
            if(tinkerer >= 0) {
                dialogue.Add($"I wonder why {Main.npc[tinkerer].GivenName} was so angry when I asked to 'borrow' his arm for science.");
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2) {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop) {
            if(firstButton) {
                shop = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot) {
            AddItem(ref shop, ref nextSlot, ItemType<MartianTransmitter>());
            AddItem(ref shop, ref nextSlot, ItemType<MartianGrenade>());
            AddItem(ref shop, ref nextSlot, ItemType<MarsBullet>());
            AddItem(ref shop, ref nextSlot, ItemType<MartianArrow>());
            AddItem(ref shop, ref nextSlot, ItemType<BrainslugLauncher>());
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
            damage = 65;
            knockback = 6f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
            cooldown = 2;
            randExtraCooldown = 2;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
            projType = 440;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
            multiplier = 11f;
            randomOffset = 2f;
        }
    }
}
