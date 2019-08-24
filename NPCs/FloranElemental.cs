using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class FloranElemental : ModNPC
	{
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Elemental");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 38;
			npc.damage = 17;
			npc.defense = 11;
			npc.noTileCollide = true;
			npc.lifeMax = 55;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 360f;
			npc.knockBackResist = .25f;
			npc.noGravity = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY > Main.rockLayer && spawnInfo.player.ZoneJungle && NPC.downedBoss1 ? 0.02f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 44, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
				int dust1 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 44, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);


				int dust2 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 44, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
			}
		}

		public override void NPCLoot()
		{
			int Bark = Main.rand.Next(4) + 3;
			for (int J = 0; J <= Bark; J++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FloranOre"));
			}
		}

		public override void AI()
		{

			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			if (npc.Center.X >= player.Center.X && moveSpeed >= -40) // flies to players x position
			{
				moveSpeed--;
			}

			if (npc.Center.X <= player.Center.X && moveSpeed <= 40)
			{
				moveSpeed++;
			}

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -25) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 150f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 25)
			{
				moveSpeedY++;
			}

			npc.velocity.Y = moveSpeedY * 0.11f;
			if (Main.rand.Next(220) == 6)
			{
				HomeY = -35f;
			}
			npc.rotation += 0.2f;
		}
	}
}
