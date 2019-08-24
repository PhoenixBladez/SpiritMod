using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class GrassBall : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grassy Sphere");
		}

		public override void SetDefaults()
		{
			npc.width = 8;
			npc.height = 8;
			npc.alpha = 255;

			npc.damage = 13;
			npc.defense = 0;
			npc.lifeMax = 1;
			npc.knockBackResist = 0;

			npc.friendly = false;
			npc.noGravity = true;
			npc.noTileCollide = true;

			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath3;
		}

		public override bool PreAI()
		{
			if (npc.target == 255)
			{
				npc.TargetClosest(true);
				float num1 = 6f;
				Vector2 vector2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
				float num2 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector2.X;
				float num3 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector2.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				float num5 = num1 / num4;
				npc.velocity.X = num2 * num5;
				npc.velocity.Y = num3 * num5;
			}
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 44);
			int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, 44);
			int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, 44);
			Main.dust[dust2].scale = 2f;
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;

			if (npc.timeLeft > 100)
				npc.timeLeft = 100;

			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180);
		}
	}
}
