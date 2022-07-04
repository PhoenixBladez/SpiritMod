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
			NPC.aiStyle = -1;
			NPC.lifeMax = 99999;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.knockBackResist = 0f;
			NPC.chaseable = false;
			NPC.width = 14;
			NPC.height = 14;
			NPC.value = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.DD2_SkeletonHurt;
			NPC.DeathSound = SoundID.DD2_SkeletonHurt;
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Cystal Shield");

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (!projectile.minion)
				projectile.penetrate = 0;

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
			Lighting.AddLight(NPC.position, 0.0149f, 0.0142f, 0.0207f);
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			NPC.ai[2]++;

			if (NPC.ai[0] == 0)
			{
				Location = NPC.Center - Main.npc[(int)NPC.ai[1]].Center;
				Location2 = NPC.Center - Main.npc[(int)NPC.ai[1]].Center;
				NPC.ai[0]++;
			}
			else
			{
				Location2 = Location.RotatedBy((MathHelper.Pi / 180));
				Location = Location2;
				NPC.Center = Location + Main.npc[(int)NPC.ai[1]].Center;
			}

			if (Main.npc[(int)NPC.ai[1]].life <= 0)
			{
				NPC.life = 0;
			}

			NPC.rotation = NPC.velocity.ToRotation();
			Vector2 vector2_1 = (new Vector2(0.0f, (float)Math.Cos((double)NPC.frameCounter * 6.28318548202515 / 40.0 - 1.57079637050629)) * 32f).RotatedBy((double)NPC.rotation, Vector2.Zero);
			Vector2 vector2_2 = NPC.velocity.SafeNormalize(Vector2.Zero);
			for (int index = 0; index < 1; ++index)
			{
				Dust dust1 = Dust.NewDustDirect(NPC.Center - NPC.Size / 4f, NPC.width / 2, NPC.height / 2, DustID.PurpleCrystalShard, 0.0f, 0.0f, 0, new Color(), 0.2f);
				int num1 = 1;
				dust1.noGravity = num1 != 0;
				Vector2 vector2_3 = NPC.Center + vector2_1;
				dust1.position = vector2_3;
				Vector2 vector2_4 = dust1.velocity * 0.0f;
				dust1.velocity = vector2_4;
				double num2 = 0.79999997615814;
				dust1.fadeIn = (float)num2;
				dust1.scale = (float)0.1f;
				Vector2 vector2_5 = dust1.position + NPC.velocity * 1.2f;
				dust1.position = vector2_5;
				Vector2 vector2_6 = dust1.velocity + vector2_2 * 2f;
				dust1.velocity = vector2_6;
				Dust dust2 = Dust.NewDustDirect((NPC.Center - NPC.Size / 4f), NPC.width / 2, NPC.height / 2, DustID.PurpleCrystalShard, 0.0f, 0.0f, 0, new Color(), 0.1f);
				int num5 = 1;
				dust2.noGravity = num5 != 0;
				Vector2 vector2_7 = NPC.Center + vector2_1;
				dust2.position = vector2_7;
				Vector2 vector2_8 = dust2.velocity * 0.0f;
				dust2.velocity = vector2_8;
				double num6 = 0.89999997615814;
				dust2.fadeIn = (float)num6;
				dust2.scale = (float)0.05f;
				Vector2 vector2_9 = dust2.position + NPC.velocity * 0.5f;
				dust2.position = vector2_9;
				Vector2 vector2_10 = dust2.position + NPC.velocity * 1.2f;
				dust2.position = vector2_10;
				Vector2 vector2_11 = dust2.velocity + vector2_2 * 2f;
				dust2.velocity = vector2_11;
			}
			int num9 = (int)(NPC.frameCounter + 1);
			NPC.frameCounter = num9;
			if (num9 >= 40)
			{
				NPC.frameCounter = 0;
			}
			//npc.frame = npc.frameCounter / 5;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Cystal/Cystal_Shield").Value);
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cystal_Shield_Gore_1").Type, 1f);
				Filters.Scene.Deactivate("CystalTower", Main.player[Main.myPlayer].position);
				Filters.Scene.Deactivate("CystalBloodMoon", Main.player[Main.myPlayer].position);
			}
		}
	}
}
