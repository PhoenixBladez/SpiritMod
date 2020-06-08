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
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
    public class HauntedBook : ModNPC
    {
        int timer = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Tome");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults() {
            npc.width = 48;
            npc.height = 40;
            npc.damage = 45;
            npc.defense = 12;
            npc.lifeMax = 410;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 3060f;
            npc.knockBackResist = .45f;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            npc.noGravity = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/HauntedBook_Glow"));
        }
        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            Player player = spawnInfo.player;
            if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
                int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
                return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > (Main.rockLayer + 100) ? 3.09f : 0f;
            }
            return 0f;
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 13);
                Gore.NewGore(npc.position, npc.velocity, 12);
                Gore.NewGore(npc.position, npc.velocity, 11);
            }
        }

        public override void AI() {
            if(Main.rand.Next(150) == 8) {
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 2f;
                direction.Y *= 2f;

                int amountOfProjectiles = Main.rand.Next(1, 2);
                for(int i = 0; i < amountOfProjectiles; ++i) {
                    float A = (float)Main.rand.Next(-1, 1) * 0.03f;
                    float B = (float)Main.rand.Next(-1, 1) * 0.03f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<RuneHostile>(), 38, 1, Main.myPlayer, 0, 0);
                }
            }
            npc.spriteDirection = npc.direction;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.675f, 2.50f);
            timer++;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if(timer == 25) {
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                npc.velocity.Y = direction.Y * 3f;
                npc.velocity.X = direction.X * 3f;
                timer = 0;
            }
            if(timer == 32) {
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                npc.velocity.Y = direction.Y * 3f;
                npc.velocity.X = direction.X * 3f;
            }
        }

        public override void NPCLoot() {
            if(Main.rand.Next(2) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Material.Rune>(), Main.rand.Next(1) + 2);

            if(Main.rand.Next(3) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SoulShred>(), Main.rand.Next(1) + 1);

            if(Main.rand.Next(20) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PossessedBook>());
        }

    }
}
