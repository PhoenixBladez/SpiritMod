
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class GloopGloop : ModNPC
	{
		int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloop");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 48;
			npc.damage = 18;
			npc.defense = 10;
			npc.lifeMax = 106;
			npc.noGravity = true;
			npc.value = 90f;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.DD2_GoblinHurt;
			npc.DeathSound = SoundID.NPCDeath22;
		}
		public override void AI()
		{
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
			counter++;
			npc.velocity *= 0.995f;
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			npc.scale = num395 + 0.95f;
			if(counter > 65) {
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction *= 10;
				npc.velocity = direction;
				for(int i = 0; i < 10; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 167, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if(Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 4f;
				}
				counter = 0;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, default(Color), .61f);
			}
			if(npc.life <= 0) {
				for(int k = 0; k < 20; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, default(Color), .91f);
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(6) == 0) {
				target.AddBuff(BuffID.Poisoned, 180);
			}
		}
	}
}
