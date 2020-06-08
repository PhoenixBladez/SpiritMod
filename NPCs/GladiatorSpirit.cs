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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class GladiatorSpirit : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gladiator Spirit");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Wraith];
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 32;
            npc.height = 56;
            npc.damage = 30;
            npc.defense = 9;
            npc.lifeMax = 80;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 320f;
            npc.knockBackResist = .40f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = 22;
            aiType = NPCID.Wraith;
            animationType = NPCID.Wraith;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            return (tile == 367) && spawnInfo.spawnTileY > Main.rockLayer && NPC.downedBoss2 ? 0.06f : 0f;
        }

        public override void HitEffect(int hitDirection, double damage) {
            int d = 54;
            for(int k = 0; k < 10; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladSpirit/GladSpirit1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladSpirit/GladSpirit2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladSpirit/GladSpirit3"), 1f);

            }
        }
        bool reflectPhase;
        int reflectTimer;
        public override void AI() {

            reflectTimer++;
            if(reflectTimer == 720) {
                Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, npc.Center);
            }
            if(reflectTimer > 720) {
                reflectPhase = true;
            } else {
                reflectPhase = false;
            }
            if(reflectTimer >= 1000) {
                reflectTimer = 0;
            }
            if(reflectPhase) {
                npc.velocity = Vector2.Zero;
                npc.defense = 9999;
                Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)npc.height, (float)npc.height) * npc.scale * 1.85f / 2f;
                int index = Dust.NewDust(npc.Center + vector2, 0, 0, 246, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].position = npc.Center + vector2;
                Main.dust[index].velocity = Vector2.Zero;
            } else {
                npc.defense = 9;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                                 lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
                {
                    Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                    for(int k = 0; k < npc.oldPos.Length; k++) {
                        Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                        Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                        spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                    }
                }
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) {
            if(reflectPhase) {
                projectile.hostile = true;
                projectile.friendly = false;
                projectile.penetrate = 2;
                projectile.velocity.X = projectile.velocity.X * -1f;
            }
        }
        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MarbleChunk>());
        }

    }
}
