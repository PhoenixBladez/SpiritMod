﻿using System.Collections.Generic;
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

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
    public abstract class CandyBase : ModItem
    {
        public override bool ItemSpace(Player player) {
            Item[] inv = player.inventory;
            for(int i = 0; i < 50; i++) {
                if(inv[i].IsAir || inv[i].type != CandyBag._type)
                    continue;
                if(!((CandyBag)inv[i].modItem).Full)
                    return true;
            }
            return false;
        }

        public override bool OnPickup(Player player) {
            Item[] inv = player.inventory;
            for(int i = 0; i < 50; i++) {
                if(inv[i].IsAir || inv[i].type != CandyBag._type)
                    continue;
                if(((CandyBag)inv[i].modItem).TryAdd(this)) {
                    ItemText.NewText(item, 1);
                    Main.PlaySound(7, (int)player.position.X, (int)player.position.Y);
                    return false;
                }
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            if(CanRightClick())
                tooltips.Add(new TooltipLine(mod, "RightclickHint", "Right click to put into Candy Bag"));
        }

        public override bool CanRightClick() {
            return ItemSpace(Main.player[Main.myPlayer]);
        }

        public override void RightClick(Player player) {
            Item[] inv = player.inventory;
            for(int i = 0; i < 50; i++) {
                if(inv[i].IsAir || inv[i].type != CandyBag._type)
                    continue;
                if(((CandyBag)inv[i].modItem).TryAdd(this)) {
                    Main.PlaySound(7, (int)player.position.X, (int)player.position.Y);
                    return;
                }
            }
            //No bags with free space found.

            //Needed to counter the default consuption.
            item.stack++;
        }
    }
}
