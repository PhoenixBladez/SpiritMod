using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Cystal
{
	public class Cystal_Shield : ModNPC
	{
		private Vector2 Location;
		private Vector2 Location2;
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 99999;
			npc.damage = 0;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.chaseable = false;
			npc.width = 14;
			npc.height = 14;
			npc.value = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(42, 166);
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(42, 166);
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Cystal Shield");

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (!projectile.minion)
			{
				projectile.penetrate = 0;
			}

			damage = 0;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			damage = 0;
		}

		public override bool CheckDead()
		{
			return false;
		}

		public override bool? DrawHealthBar(byte hbPos, ref float scale, ref Vector2 Pos)
		{
			return false;
		}

		public override void AI()
		{
			Lighting.AddLight(npc.position, 0.0149f, 0.0142f, 0.0207f);
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			npc.ai[2]++;

			if (npc.ai[0] == 0)
			{
				Location = npc.Center - Main.npc[(int)npc.ai[1]].Center;
				Location2 = npc.Center - Main.npc[(int)npc.ai[1]].Center;
				npc.ai[0]++;
			}
			else
			{
				Location2 = Location.RotatedBy((MathHelper.Pi / 180));
				Location = Location2;
				npc.Center = Location + Main.npc[(int)npc.ai[1]].Center;
			}

			if (Main.npc[(int)npc.ai[1]].life <= 0)
			{
				npc.life = 0;
			}

			npc.rotation = npc.velocity.ToRotation();
			Vector2 vector2_1 = (new Vector2(0.0f, (float)Math.Cos((double)npc.frameCounter * 6.28318548202515 / 40.0 - 1.57079637050629)) * 32f).RotatedBy((double)npc.rotation, Vector2.Zero);
			Vector2 vector2_2 = npc.velocity.SafeNormalize(Vector2.Zero);
			for (int index = 0; index < 1; ++index)
			{
				Dust dust1 = Dust.NewDustDirect(npc.Center - npc.Size / 4f, npc.width / 2, npc.height / 2, DustID.PurpleCrystalShard, 0.0f, 0.0f, 0, new Color(), 0.2f);
				int num1 = 1;
				dust1.noGravity = num1 != 0;
				Vector2 vector2_3 = npc.Center + vector2_1;
				dust1.position = vector2_3;
				Vector2 vector2_4 = dust1.velocity * 0.0f;
				dust1.velocity = vector2_4;
				double num2 = 0.79999997615814;
				dust1.fadeIn = (float)num2;
				dust1.scale = (float)0.1f;
				Vector2 vector2_5 = dust1.position + npc.velocity * 1.2f;
				dust1.position = vector2_5;
				Vector2 vector2_6 = dust1.velocity + vector2_2 * 2f;
				dust1.velocity = vector2_6;
				Dust dust2 = Dust.NewDustDirect((npc.Center - npc.Size / 4f), npc.width / 2, npc.height / 2, DustID.PurpleCrystalShard, 0.0f, 0.0f, 0, new Color(), 0.1f);
				int num5 = 1;
				dust2.noGravity = num5 != 0;
				Vector2 vector2_7 = npc.Center + vector2_1;
				dust2.position = vector2_7;
				Vector2 vector2_8 = dust2.velocity * 0.0f;
				dust2.velocity = vector2_8;
				double num6 = 0.89999997615814;
				dust2.fadeIn = (float)num6;
				dust2.scale = (float)0.05f;
				Vector2 vector2_9 = dust2.position + npc.velocity * 0.5f;
				dust2.position = vector2_9;
				Vector2 vector2_10 = dust2.position + npc.velocity * 1.2f;
				dust2.position = vector2_10;
				Vector2 vector2_11 = dust2.velocity + vector2_2 * 2f;
				dust2.velocity = vector2_11;
			}
			int num9 = (int)(npc.frameCounter + 1);
			npc.frameCounter = num9;
			if (num9 >= 40)
			{
				npc.frameCounter = 0;
			}
			//npc.frame = npc.frameCounter / 5;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Cystal/Cystal_Shield"));
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Cystal_Shield_Gore_1"), 1f);
				Filters.Scene.Deactivate("CystalTower", Main.player[Main.myPlayer].position);
				Filters.Scene.Deactivate("CystalBloodMoon", Main.player[Main.myPlayer].position);
			}
		}
	}
}
