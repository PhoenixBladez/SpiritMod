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
	public class AtlanticCod : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlantic Cod");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 22;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ItemID.AtlanticCod;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.dontTakeDamageFromHostiles = false;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			AIType = NPCID.Goldfish;
			NPC.dontCountMe = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override bool PreAI()
        {
            NPC.spriteDirection = NPC.direction;
           
            return true;
        }
		public override void AI()
        {
            Player player = Main.player[NPC.target];
            {
                Player target = Main.player[NPC.target];
                int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
                if (distance < 65 && target.wet && NPC.wet)
                {
                    Vector2 vel = NPC.DirectionFrom(target.Center);
                    vel.Normalize();
                    vel *= 4.5f;
                    NPC.velocity = vel;
                    NPC.rotation = NPC.velocity.X * .06f;
                    if (target.position.X > NPC.position.X)
                    {
                        NPC.spriteDirection = -1;
                        NPC.direction = -1;
                        NPC.netUpdate = true;
                    }
                    else if (target.position.X < NPC.position.X)
                    {
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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AtlanticCod1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AtlanticCod2").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<RawFish>(2);

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneSnow && spawnInfo.Water ? 0.16f : 0f;
		}
	}
}
