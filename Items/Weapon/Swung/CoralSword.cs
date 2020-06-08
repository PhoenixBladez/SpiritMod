using Microsoft.Xna.Framework;
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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class CoralSword : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hightide Blade");
            Tooltip.SetDefault("Weapon damage and swing speed increase greatly if the player is underwater\nHitting enemies occasionally causes the sword to splinter and deal extra damage");
        }


        public override void SetDefaults() {
            item.damage = 9;
            item.melee = true;
            item.width = 36;
            item.height = 44;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
            item.rare = 1;
            item.shootSpeed = 14f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player) {
            if(player.wet) {
                item.damage = 12;
                item.useTime = 20;
                item.useAnimation = 20;
            } else {
                item.useTime = 26;
                item.useAnimation = 26;
                item.damage = 9;
            }
            return base.CanUseItem(player);
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(3) == 0) {
                target.StrikeNPC(item.damage / 4, 0f, 0, crit);
                for(int k = 0; k < 20; k++) {
                    Dust.NewDust(target.position, target.width, target.height, 225, 2.5f, -2.5f, 0, Color.White, 0.7f);
                    Dust.NewDust(target.position, target.width, target.height, 225, 2.5f, -2.5f, 0, default(Color), .34f);
                }
            }
        }
        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(ItemID.Coral, 8);
            modRecipe.AddIngredient(ItemID.Seashell, 2);
            modRecipe.AddIngredient(ItemID.BottledWater, 1);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}