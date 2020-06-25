using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Weapon.Summon;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class Vilemoth : ModNPC
    {
        int timer = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 120f;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Festerfly");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.width = 40;
            npc.height = 30;
            npc.damage = 32;
            npc.defense = 15;
            npc.lifeMax = 70;
            npc.HitSound = SoundID.NPCHit7; //Dr Man Fly
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 110f;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.knockBackResist = .45f;
        }


        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
        public override void AI() {
            Player player = Main.player[npc.target];
            npc.rotation = npc.velocity.X * 0.1f;
            if (npc.Center.X >= player.Center.X && moveSpeed >= -60) // flies to players x position
            {
                moveSpeed--;
            }

            if (npc.Center.X <= player.Center.X && moveSpeed <= 60)
            {
                moveSpeed++;
            }

            npc.velocity.X = moveSpeed * 0.06f;

            if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -50) //Flies to players Y position
            {
                moveSpeedY--;
                HomeY = 120f;
            }

            if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 50)
            {
                moveSpeedY++;
            }

            npc.velocity.Y = moveSpeedY * 0.06f;
            timer++;
            if(timer >= 120) {
                bool expertMode = Main.expertMode;
                int damage = expertMode ? 12 : 16;
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector2_2.X, vector2_2.Y, mod.ProjectileType("VileWaspProjectile"), damage, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                Main.projectile[p].hostile = true;
                timer = 0;
            }

            npc.spriteDirection = npc.direction;
        }
    }
}
