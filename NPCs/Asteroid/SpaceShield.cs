using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class SpaceShield : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Shield");
		}

		public override void SetDefaults()
		{
			npc.noTileCollide = true;
			npc.width = 32;
			npc.height = 32;
			npc.netAlways = true;
			npc.damage = 15;
			npc.defense = 9999;
			npc.alpha = 255;
			npc.npcSlots = 0;
			npc.dontTakeDamage = true;
			npc.lifeMax = 100;
			npc.friendly = false;
			npc.chaseable = false;
			npc.noGravity = true;
			npc.knockBackResist = 0f;
			for(int k = 0; k < npc.buffImmune.Length; k++) {
				npc.buffImmune[k] = true;
			}
			npc.dontCountMe = true;
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			npc.dontTakeDamage = true;
			if(!projectile.minion) {
				projectile.hostile = true;
				projectile.friendly = false;
				projectile.penetrate = 2;
				projectile.velocity.X = projectile.velocity.X * -1f;
			}
			damage = 0;
			npc.life = 100;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			npc.dontTakeDamage = true;
			damage = 0;
			npc.life = 100;
			if(!npc.active)
				for(int i = 0; i < 20; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 206, 0f, -2f, 0, default(Color), 1f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if(Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
		}

		public override void NPCLoot()
		{
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}

		public override bool PreAI()
		{
			npc.life = 100;
			npc.dontTakeDamage = false;
			Player player = Main.LocalPlayer;
			Vector2 center = npc.Center;
			float num8 = (float)player.miscCounter / 60f;
			float num7 = 1.0471975512f;
			for(int i = 0; i < 6; i++) {
				int num6 = Dust.NewDust(center, 0, 0, 180, 0f, 0f, 100, default(Color), 1.3f);
				Main.dust[num6].noGravity = true;
				Main.dust[num6].velocity = Vector2.Zero;
				Main.dust[num6].noLight = true;
				Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 12f;
			}
			npc.rotation = npc.rotation + 3f;
			//npc.rotation = npc.rotation + 3f;
			float lowestDist = float.MaxValue;
			if(npc.ai[3] < (double)Main.npc.Length) {
				NPC parent = Main.npc[(int)npc.ai[3]];
				//Factors for calculations
				double deg = (double)npc.ai[1]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
				double rad = deg * (Math.PI / 180); //Convert degrees to radians
				double dist = 60; //Distance away from the npc


				//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
				npc.ai[1] += 1.2f;

				npc.position.X = parent.Center.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
				npc.position.Y = parent.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;
				if(!parent.active) {
					npc.life = 0;
					npc.HitEffect(0, 10.0);
					npc.active = false;
				}
			}
			return false;
		}


	}
}
