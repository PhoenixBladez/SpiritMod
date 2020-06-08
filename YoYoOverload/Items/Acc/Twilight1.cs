using Terraria;
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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
    public class Twilight1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Twilight Talisman");
            Tooltip.SetDefault("Increases melee speed by 5%\nIncreases damage reduction and damage dealt by 5%\nIncreases critical strike chance by 4%\nIncreases movement speed by 10%\nAttacks have a small chance of inflicting Shadowflame");
        }


        public override void SetDefaults() {
            base.item.width = 14;
            base.item.height = 24;
            base.item.rare = 3;
            base.item.UseSound = SoundID.Item11;
            base.item.accessory = true;
            base.item.value = Item.buyPrice(0, 2, 30, 0);
            base.item.value = Item.sellPrice(0, 1, 6, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.meleeDamage += 0.05f;
            player.magicDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.minionDamage += 0.05f;
            player.thrownDamage += 0.05f;
            player.meleeCrit += 4;
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
            player.endurance += 0.05f;
            player.meleeSpeed += 0.05f;
            player.moveSpeed += .1f;
            player.GetSpiritPlayer().twilightTalisman = true;
        }

        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "YoyoCharm2", 1);
            modRecipe.AddIngredient(null, "MCharm", 1);
            modRecipe.AddIngredient(null, "DCharm", 1);
            modRecipe.AddIngredient(null, "HCharm", 1);
            modRecipe.AddTile(TileID.DemonAltar);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
