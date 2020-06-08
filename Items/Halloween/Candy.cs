using System;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Halloween
{
    public class Candy : CandyBase
    {
        public static int _type;

        public override bool CloneNewInstances => true;

        public int Variant { get; internal set; }


        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Candy");
            Tooltip.SetDefault("Increases all stats slightly");
        }

        public override void SetDefaults() {
            item.width = 20;
            item.height = 30;
            item.rare = 2;
            item.maxStack = 1;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;

            item.buffType = ModContent.BuffType<CandyBuff>();
            item.buffTime = 14400;

            item.UseSound = SoundID.Item2;

            Variant = Main.rand.Next(CandyNames.Count);
        }


        internal static readonly ReadOnlyCollection<string> CandyNames =
            Array.AsReadOnly(new string[]
        {
            "Popstone",
            "Three Muskets",
            "Lhizzlers",
            "Moon Jelly Beans",
            "Silk Duds",
            "Necro Wafers",
            "Blinkroot Pop",
            "Gummy Slimes",
            "Cry Goblin",
            "Sour patch Slimes",
            "Stardust Burst",
            "Hellfire Tamales",
            "Blinkroot Patty",
            "Xenowhoppers",
            "Gem&Ms",
            "100,000 copper bar",
            "Toblerbone",
            "Delicious Looking Eye",
            "Silky Way",
            "Malted Silk Balls",
            "Cloudheads",
            "Red Devil Hots",
            "Rune Pop",
            "Nursey Kisses",
            "Skullies",
            "Firebolts",
            "Vinewrath Cane",
            "Candy Acorn",
            "Bunnyfinger",
            "Ichorice",
            "Lunatic-tac"
        });

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            base.ModifyTooltips(tooltips);

            int index = tooltips.FindIndex(tooltip => tooltip.Name.Equals("ItemName"));
            if(index >= 0) {
                TooltipLine name = tooltips.ElementAt(index);
                TooltipLine line = new TooltipLine(mod, "ItemNameSub", "'" + CandyNames[Variant] + "'");
                tooltips.Insert(index + 1, line);
            }
        }


        public override TagCompound Save() {
            TagCompound tag = new TagCompound();
            tag.Add("Variant", Variant);
            return tag;
        }

        public override void Load(TagCompound tag) {
            Variant = tag.GetInt("Variant");
        }

        public override void NetSend(BinaryWriter writer) {
            writer.Write((byte)Variant);
        }

        public override void NetRecieve(BinaryReader reader) {
            Variant = reader.ReadByte();
        }
    }
}
