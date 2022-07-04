using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class Damselfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Damselfish");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ItemID.Damselfish;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.dontCountMe = true;
			NPC.npcSlots = 0;
			AIType = NPCID.Goldfish;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void AI()
        {
            Player player = Main.player[NPC.target]; {
                Player target = Main.player[NPC.target];
                int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
                if (distance < 65 && target.wet && NPC.wet)
                {
                    Vector2 vel = NPC.DirectionFrom(target.Center);
                    vel.Normalize();
                    vel *= 4.5f;
                    NPC.velocity = vel;
                    NPC.rotation = NPC.velocity.X * .06f;
                    if (target.position.X > NPC.position.X) {
                        NPC.spriteDirection = -1;
                        NPC.direction = -1;
                        NPC.netUpdate = true;
                    }
                    else if (target.position.X < NPC.position.X) {
                        NPC.spriteDirection = 1;
                        NPC.direction = 1;
                        NPC.netUpdate = true;
                    }
                }
            }
        }
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}


		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Damselfish1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Damselfish2").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<RawFish>(2);

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Sky && spawnInfo.Water ? 0.65f : 0f;
		}
	}
}
