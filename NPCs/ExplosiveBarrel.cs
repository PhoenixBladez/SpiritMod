using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Hostile;

namespace SpiritMod.NPCs
{
	public class ExplosiveBarrel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Barrel");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 70;
			npc.damage = 0;
			npc.dontCountMe = true;
			npc.defense = 0;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .45f;
			npc.aiStyle = 0;
			npc.npcSlots = 0;
			npc.noGravity = false;
			npc.HitSound = SoundID.NPCHit4;
			npc.friendly = false;
		}
		public override void AI()
		{
			if (Main.rand.Next(10) == 1) {
				Dust.NewDust(npc.position, npc.width, npc.height, 6, 0, -5, 0, default(Color), 1.5f);
			}
			npc.spriteDirection = (int)npc.ai[0];
			npc.life--;
			if (npc.life <= 0) 
			{
				Explode();
			}

		}
		public void Explode()
		{
			Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<BarrelExplosionLarge>(), 100, 8, Main.myPlayer);
			npc.active = false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Explode();
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.14f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
