using SpiritMod.Items.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class ShadowPup : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Pup");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 34;
			npc.damage = 0;
			npc.defense = 26;
			npc.lifeMax = 20;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.knockBackResist = 0f;
			npc.aiStyle = 7;
			aiType = NPCID.Bunny;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(!Main.hardMode) {
				return 0f;
			}

			return SpawnCondition.OverworldNightMonster.Chance * 0.01f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if(npc.velocity.X != 0f) {
				npc.frameCounter += 0.15f;
				npc.frameCounter %= Main.npcFrameCount[npc.type];
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.08f, 0.04f, 0.2f);

			npc.spriteDirection = npc.direction;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(3) == 0) {
				target.AddBuff(BuffID.Slow, 170, true);
				target.AddBuff(BuffID.Cursed, 280, true);
				target.AddBuff(BuffID.Blackout, 280, true);
			}
		}

		public override void NPCLoot()
		{
			if(Main.rand.Next(20) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ShadowCollar>());

			}
		}

	}
}
