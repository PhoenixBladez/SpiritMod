using Microsoft.Xna.Framework.Graphics;
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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
    public class SpiritGlowmask : ModPlayer
    {
        private static readonly Dictionary<int, Texture2D> ItemGlowMask = new Dictionary<int, Texture2D>();

        internal static void Unload() {
            ItemGlowMask.Clear();
        }

        public static void AddGlowMask(int itemType, string texturePath) {
            ItemGlowMask[itemType] = ModContent.GetTexture(texturePath);
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers) {
            Texture2D textureLegs;
            if(!player.armor[12].IsAir) {
                if(player.armor[12].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[12].type, out textureLegs))//Vanity Legs
                {
                    InsertAfterVanillaLayer(layers, "Legs", new PlayerLayer(mod.Name, "GlowMaskLegs", delegate (PlayerDrawInfo info) {
                        GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, info);
                    }));
                }
            } else if(player.armor[2].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[2].type, out textureLegs))//Legs
              {
                InsertAfterVanillaLayer(layers, "Legs", new PlayerLayer(mod.Name, "GlowMaskLegs", delegate (PlayerDrawInfo info) {
                    GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, info);
                }));
            }
            Texture2D textureBody;
            if(!player.armor[11].IsAir) {
                if(player.armor[11].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[11].type, out textureBody))//Vanity Body
                {
                    InsertAfterVanillaLayer(layers, "Body", new PlayerLayer(mod.Name, "GlowMaskBody", delegate (PlayerDrawInfo info) {
                        GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, info);
                    }));
                }
            } else if(player.armor[1].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[1].type, out textureBody))//Body
              {
                InsertAfterVanillaLayer(layers, "Body", new PlayerLayer(mod.Name, "GlowMaskBody", delegate (PlayerDrawInfo info) {
                    GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, info);
                }));
            }
            Texture2D textureHead;
            if(!player.armor[10].IsAir) {
                if(player.armor[10].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[10].type, out textureHead))//Vanity Head
                {
                    InsertAfterVanillaLayer(layers, "Head", new PlayerLayer(mod.Name, "GlowMaskHead", delegate (PlayerDrawInfo info) {
                        GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureHead, info);
                    }));
                }
            } else if(player.armor[0].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[0].type, out textureHead))//Head
              {
                InsertAfterVanillaLayer(layers, "Head", new PlayerLayer(mod.Name, "GlowMaskHead", delegate (PlayerDrawInfo info) {
                    GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureHead, info);
                }));
            }
            Texture2D textureItem;
            if(player.HeldItem.type >= ItemID.Count && ItemGlowMask.TryGetValue(player.HeldItem.type, out textureItem))//Held ItemType
            {
                InsertAfterVanillaLayer(layers, "HeldItem", new PlayerLayer(mod.Name, "GlowMaskHeldItem", delegate (PlayerDrawInfo info) {
                    GlowmaskUtils.DrawItemGlowMask(textureItem, info);
                }));
            }
        }

        public static void InsertAfterVanillaLayer(List<PlayerLayer> layers, string vanillaLayerName, PlayerLayer newPlayerLayer) {
            for(int i = 0; i < layers.Count; i++) {
                if(layers[i].Name == vanillaLayerName && layers[i].mod == "Terraria") {
                    layers.Insert(i + 1, newPlayerLayer);
                    return;
                }
            }
            layers.Add(newPlayerLayer);
        }
    }
}
