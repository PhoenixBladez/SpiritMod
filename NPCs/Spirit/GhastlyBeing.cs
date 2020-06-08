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
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
    public class GhastlyBeing : ModNPC
    {
        private bool circling;
        int startdist = 0;
        Vector2 target = Vector2.Zero;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ghastly Being");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.width = 56;
            npc.height = 42;
            npc.damage = 20;
            npc.defense = 20;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 60f;
            npc.knockBackResist = .45f;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        //public override void HitEffect(int hitDirection, double damage)
        //{
        //	if (npc.life <= 0)
        //	{
        //		Gore.NewGore(npc.position, npc.velocity, 13);
        //		Gore.NewGore(npc.position, npc.velocity, 12);
        //		Gore.NewGore(npc.position, npc.velocity, 11);
        //	}
        //}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/GhastlyBeing_Glow"));
        }
        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.25f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            Player player = spawnInfo.player;
            if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
                int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
                return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > (Main.rockLayer + 50) ? 3.08f : 0f;
            }
            return 0f;
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 delta = player.position - npc.position;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.12f, 0.12f, 1f);
            //64 pixel radius

            if(Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) <= 250f && circling == false) //circle starting
            {
                npc.ai[1] = 0; //reset rotation
                npc.aiStyle = -1;
                circling = true; //launch circle action into effect
            }
            if(Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) > 250f && circling == false) //normal AI
            {
                npc.aiStyle = 22;
                aiType = NPCID.Wraith;
            }
            if(Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) > 500f && circling == true) //stop circling
            {
                circling = false;
                npc.velocity = Vector2.Zero;
            }
            if(circling == true) {
                double deg = (double)npc.ai[1]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 250; //Distance away from the player

                /*Position the npc based on where the player is, the Sin/Cos of the angle times the /
                /distance for the desired distance away from the player minus the npc's width   /
                /and height divided by two so the center of the npc is at the right place.     */
                target.X = player.Center.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
                target.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;

                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                npc.ai[1] += 1f;
                Vector2 Vel = target - npc.Center;
                Vel.Normalize();
                Vel *= 4f;
                npc.velocity = Vel;
            }
        }

        public override void NPCLoot() {
            if(Main.rand.Next(5) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SoulShred>(), Main.rand.Next(1) + 1);
        }
    }
}
