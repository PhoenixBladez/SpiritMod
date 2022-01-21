using SpiritMod.Mechanics.CollideableNPC;
using System;
using Terraria;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent
{
	public class SjinxPlatform : SpiritNPC, ISolidTopNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platform");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 300;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 28;
			npc.lifeMax = 1200;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCDeath1;
			npc.DeathSound = SoundID.NPCDeath10;
			npc.value = 0;
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.dontCountMe = true;
			npc.dontTakeDamage = true;
		}

		public override void AI() => npc.velocity.Y = (float)Math.Sin(npc.ai[0]++ * 0.06f) * 0.8f;
	}

	public class SjinxPlatformMedium : SjinxPlatform
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 450;
			npc.height = 30;
		}
	}

	public class SjinxPlatformLarge : SjinxPlatform
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 600;
			npc.height = 50;
		}
	}
}