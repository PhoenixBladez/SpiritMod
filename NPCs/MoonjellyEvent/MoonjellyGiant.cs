using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class MoonjellyGiant : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tethervolt Jelly");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 70;
			npc.damage = 16;
			npc.defense = 10;
			npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.value = 250f;
			npc.knockBackResist = 0f;
			npc.alpha = 100;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = -1;
			banner = npc.type;
		}
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.08f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
        int timer = 0;
		public override bool PreAI()
        {
            timer++;
            Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
			npc.spriteDirection = -npc.direction;
			Player target = Main.player[npc.target];
			MyPlayer modPlayer = target.GetSpiritPlayer();
			npc.spriteDirection = npc.direction;
			float num1 = 5f;
			float moveSpeed = 0.05f;
			npc.TargetClosest(true);
			Vector2 vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(0, -100f);
			float num2 = vector2_1.Length();
			Vector2 desiredVelocity;
			if ((double)num2 < 20.0)
				desiredVelocity = npc.velocity;
			else if ((double)num2 < 40.0) {
				vector2_1.Normalize();
				desiredVelocity = vector2_1 * (num1 * 0.025f);
			}
			else if ((double)num2 < 80.0) {
				vector2_1.Normalize();
				desiredVelocity = vector2_1 * (num1 * 0.075f);
			}
			else {
				vector2_1.Normalize();
				desiredVelocity = vector2_1 * num1;
			}
			npc.SimpleFlyMovement(desiredVelocity, moveSpeed);
			npc.rotation = npc.velocity.X * 0.1f;
			if (npc.ai[0] == 0f) {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(3.14159237050629f) * new Vector2(Main.rand.Next(3, 8), Main.rand.Next(3, 8));
                    bool expertMode = Main.expertMode;
                    int damage = expertMode ? 6 : 10;
                    int p = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-20, 20), npc.Center.Y + Main.rand.Next(-20, 20), vector2_2.X, vector2_2.Y, ModContent.ProjectileType<ElectricJellyfishOrbiter>(), damage, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                    Main.projectile[p].scale = Main.rand.NextFloat(.6f, .95f);
                    Main.projectile[p].ai[0] = npc.whoAmI;

                    npc.ai[0] = 1f;
                    npc.netUpdate = true;
                }
			}
			return false;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/MoonjellyEvent/MoonjellyGiant_Glow"));
        }
		public override void HitEffect(int hitDirection, double damage)
		{
            for (int k = 0; k < 15; k++)
            {
                Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.65f);
                d.noGravity = true;
            }
            if (npc.life <= 0)
            {
                for (int k = 0; k < 30; k++)
                {
                    Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.95f);
                    d.noGravity = true;
                }
            }
        }
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(2, 5));
		}
	}
}