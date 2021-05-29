using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Automata
{
	public class AutomataCreeper : ModNPC
	{

		bool attacking = false;
		Vector2 moveDirection;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Automata Creeper");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 36;
			npc.damage = 70;
			npc.defense = 30;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 10000f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			moveDirection = new Vector2((Main.rand.Next(2) * 2) - 1, 0);
			npc.noTileCollide = false;
		}
		public override void AI()
        {
			if (!attacking)
            {
				Crawl();
            }
        }

		private void Crawl()
        {
			npc.rotation = npc.velocity.ToRotation() + 3.14f;
			if (npc.ai[0] == 0f)
			{
				npc.TargetClosest(true);
				moveDirection.Y = -1;
				npc.ai[0] = 1f;
			}
			float speed = 3;
			if (npc.ai[1] == 0f)
			{
				if (npc.collideY)
				{
					npc.ai[0] = 2f;
				}
				if (!npc.collideY && npc.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					npc.ai[1] = 1f;
					npc.ai[0] = 1f;
				}
				if (npc.collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					npc.ai[1] = 1f;
				}
			}
			else
			{
				if (npc.collideX)
				{
					npc.ai[0] = 2f;
				}
				if (!npc.collideX && npc.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					npc.ai[1] = 0f;
					npc.ai[0] = 1f;
				}
				if (npc.collideY)
				{
					moveDirection.X = -moveDirection.X;
					npc.ai[1] = 0f;
				}
			}
			npc.velocity = speed * moveDirection;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.GetSpiritPlayer().ZoneMarble && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.135f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = Main.npcTexture[npc.type].Width / 2;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			if (attacking)
			{
				npc.frameCounter += 0.40f;
				npc.frame.X = npc.frame.Width;
			}
			else
			{
				npc.frameCounter += 0.40f;
				npc.frame.X = 0;
			}
		}
	}
}
