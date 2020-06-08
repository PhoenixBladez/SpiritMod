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
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Walls.Natural;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class SpiritSolution : ModProjectile
    {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spirit Spray");
        }

        public override void SetDefaults() {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI() {
            int dustType = 206;
            if(projectile.owner == Main.myPlayer)
                Convert((int)(projectile.position.X + (float)(projectile.width / 2)) / 16, (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16, 2);

            if(projectile.timeLeft > 133)
                projectile.timeLeft = 133;

            if(projectile.ai[0] > 7f) {
                float dustScale = 1f;
                if(projectile.ai[0] == 8f)
                    dustScale = 0.2f;
                else if(projectile.ai[0] == 9f)
                    dustScale = 0.4f;
                else if(projectile.ai[0] == 10f)
                    dustScale = 0.6f;
                else if(projectile.ai[0] == 11f)
                    dustScale = 0.8f;

                projectile.ai[0] += 1f;
                for(int i = 0; i < 1; i++) {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.scale *= 1.75f;
                    dust.velocity.X = dust.velocity.X * 2f;
                    dust.velocity.Y = dust.velocity.Y * 2f;
                    dust.scale *= dustScale;
                }
            } else {
                projectile.ai[0] += 1f;
            }
            projectile.rotation += 0.3f * projectile.direction;
        }

        public void Convert(int i, int j, int size = 4) {
            for(int k = i - size; k <= i + size; k++) {
                for(int l = j - size; l <= j + size; l++) {
                    if(WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
                        int type = (int)Main.tile[k, l].type;
                        int wall = (int)Main.tile[k, l].wall;
                        if(wall != 0) {
                            if(wall != 87 || wall != 94 || wall != 95 || wall != 96 || wall != 97 || wall != 98 || wall != 99 || wall != 100 || wall != 101 || wall != 102 || wall != 103 || wall != 104 || wall != 105 || wall != 62) {
                                Main.tile[k, l].wall = (ushort)ModContent.WallType<SpiritWall>();
                                WorldGen.SquareWallFrame(k, l, true);
                                NetMessage.SendTileSquare(-1, k, l, 1);
                            }
                        }
                        if(TileID.Sets.Conversion.Stone[type] || type == 179 || type == 180 || type == 181 || type == 182 || type == 183) {
                            Main.tile[k, l].type = (ushort)ModContent.TileType<SpiritStone>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        } else if(type == 0) {
                            Main.tile[k, l].type = (ushort)ModContent.TileType<SpiritDirt>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        } else if(type == 2 || type == 23 || type == 109 || type == 199) {
                            Main.tile[k, l].type = (ushort)ModContent.TileType<SpiritGrass>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        } else if(type == 53) {
                            Main.tile[k, l].type = (ushort)ModContent.TileType<Spiritsand>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        } else if(type == 161 || type == 200 || type == 163 || type == 164) {
                            Main.tile[k, l].type = (ushort)ModContent.TileType<SpiritIce>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }

    }
}