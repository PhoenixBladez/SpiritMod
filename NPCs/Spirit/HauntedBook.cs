using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles.Block;
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
                return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && player.ZoneOverworldHeight ? 2.09f : 0f;
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
