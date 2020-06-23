using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{

    public class DungeonCubeBlue : ModNPC
    {
        bool xacc = true;
        bool yacc = false;
        bool xchase = true;
        int timer = 0;
        bool ychase = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dungeon Cube");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.width = 36;
            npc.height = 32;
            npc.noGravity = true;
            npc.lifeMax = 150;
            npc.defense = 10;
            npc.damage = 32;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath44;

            npc.knockBackResist = 0f;
            npc.value = 500f;

            npc.netAlways = true;
            npc.chaseable = true;
            npc.lavaImmune = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(BuffID.Cursed, 250, true);
        }
        public override bool PreAI() {
            timer++;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if(npc.velocity == Vector2.Zero) {
                xchase = true;
                xacc = true;
                yacc = false;
                ychase = false;
                if(player.position.X > npc.position.X) {
                    npc.velocity.X = 0.1f;
                } else {
                    npc.velocity.X = -0.1f;
                }
            }
            if(xchase) {
                npc.velocity.Y = 0;
                if(Math.Abs(npc.position.X - player.position.X) > 48 && xacc && timer < 200) {
                    if(xacc && npc.velocity.X < 15) {
                        npc.velocity.X *= 1.06f;
                    }
                } else {
                    xacc = false;
                    timer = 0;
                    npc.velocity.X *= 0.94f;
                    if(Math.Abs(npc.velocity.X) < 0.1f) {
                        yacc = true;
                        ychase = true;

                        if(player.position.Y > npc.position.Y) {
                            npc.velocity.Y = 0.1f;
                        } else {
                            npc.velocity.Y = -0.1f;
                        }
                        xchase = false;
                    }
                }
                if(npc.velocity.X == 0) {
                    yacc = true;
                    ychase = true;

                    if(player.position.Y > npc.position.Y) {
                        npc.velocity.Y = 0.1f;
                    } else {
                        npc.velocity.Y = -0.1f;
                    }
                    timer = 0;
                    xchase = false;
                }
            }

            if(ychase) {
                npc.velocity.X = 0;
                if(Math.Abs(npc.position.Y - player.position.Y) > 48 && yacc && timer < 200) {
                    if(yacc && npc.velocity.Y < 15) {
                        npc.velocity.Y *= 1.06f;
                    }
                } else {
                    yacc = false;
                    timer = 0;
                    npc.velocity.Y *= 0.94f;
                    if(Math.Abs(npc.velocity.Y) < 0.1f) {
                        xacc = true;
                        xchase = true;

                        if(player.position.X > npc.position.X) {
                            npc.velocity.X = 0.1f;
                        } else {
                            npc.velocity.X = -0.1f;
                        }
                        ychase = false;
                    }
                }
                if(npc.velocity.Y == 0) {
                    xacc = true;
                    xchase = true;

                    if(player.position.X > npc.position.X) {
                        npc.velocity.X = 0.1f;
                    } else {
                        npc.velocity.X = -0.1f;
                    }
                    timer = 0;
                    ychase = false;
                }
            }
            return false;
        }



        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= 6;
            int frame = (int)npc.frameCounter;
            if(xchase) {
                npc.frame.Y = (int)(MathHelper.Clamp(Math.Abs(npc.velocity.X * 3), 0, 6)) * frameHeight;
            } else {
                npc.frame.Y = (int)(MathHelper.Clamp(Math.Abs(npc.velocity.Y * 3), 0, 6)) * frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(spawnInfo.spawnTileType == TileID.BlueDungeonBrick) {
                return spawnInfo.player.ZoneDungeon ? 0.04f : 0f;
            }
            return 0f;
        }

        public override void NPCLoot() {
            if(Main.rand.Next(10) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
            }
            if(Main.rand.Next(75) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar);
            }
            if(Main.rand.Next(100) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 134, Main.rand.Next(4));
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeBlueGore1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeBlueGore2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeBlueGore3"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonCubeBlueGore4"));
            }
        }

    }
}