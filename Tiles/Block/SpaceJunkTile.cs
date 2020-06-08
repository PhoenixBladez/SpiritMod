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
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
    public class SpaceJunkTile : ModTile
    {
        public override void SetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(87, 85, 81));
            drop = ModContent.ItemType<SpaceJunkItem>();
            dustType = 54;
        }
        public override bool HasWalkDust() {
            return true;
        }
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color) {
            dustType = 54;
            makeDust = true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            Player player = Main.LocalPlayer;
            int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
            if(distance < 54) {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
                int debuffsWeak = Main.rand.Next(new int[] { 30, 33 });
                int debuffStrong = Main.rand.Next(new int[] { 144, 80, 36, 196, 164 });
                int buffs = Main.rand.Next(new int[] { 5, 8, 18, 114, 11 });
                if(Main.rand.Next(6) == 0) {
                    if(Main.rand.Next(4) == 0) {
                        player.AddBuff(debuffStrong, 70);
                    } else {
                        player.AddBuff(debuffsWeak, 300);
                    }
                }
                if(Main.rand.Next(3) == 0) {
                    player.AddBuff(buffs, 540);
                }
            }
        }
        /*public override void FloorVisuals(Player player)
        {
            player.AddBuff(BuffID.Bleeding, 300);
            player.life -= 1;
        }*/
        public override void NearbyEffects(int i, int j, bool closer) {
            if(closer) {
                if(Main.rand.Next(20) == 0) {
                    int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 10), Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), 54, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division

                    Main.dust[d].velocity *= .8f;
                    Main.dust[d].noGravity = true;
                }
            }
        }
    }
}

