using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Undead_Warlock
{
	public class Undead_Warlock_Crystal : ModProjectile
	{
		private Vector2 Location;
		private Vector2 Location2;
		public int healTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necromancer's Magic Crystal");
		}
		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 34;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
			projectile.scale = 0.85f;
		}
		public override void AI()
		{
			if (!Main.npc[(int)projectile.ai[1]].active)
				projectile.Kill();
			if (projectile.ai[0] == 0)
			{
				Location = projectile.Center - new Vector2(Main.npc[(int)projectile.ai[1]].Center.X, Main.npc[(int)projectile.ai[1]].Center.Y - 90);
				Location2 = projectile.Center - new Vector2(Main.npc[(int)projectile.ai[1]].Center.X, Main.npc[(int)projectile.ai[1]].Center.Y - 90);
				projectile.ai[0]++;
			}
			else
			{
				Location2 = Location.RotatedBy((MathHelper.Pi / 180));
				Location = Location2;
				projectile.Center = Location + new Vector2(Main.npc[(int)projectile.ai[1]].Center.X, Main.npc[(int)projectile.ai[1]].Center.Y - 90);
			}
			if (Main.rand.Next(28)==0)
			{
				for (int index1 = 0; index1 < 4; ++index1)
				{
					int chosenDust = Main.rand.Next(2)==0 ? 173 : 157;
					float scale = (float) Main.rand.Next(3, 10) * 0.1f;
					float fadeIn = 1.2f;
					if (chosenDust == 258)
					{
						scale = (float) Main.rand.Next(1, 3) * 0.1f;
						fadeIn = 0.7f;
					}
					int index2 = Dust.NewDust(new Vector2
					(
						projectile.position.X,
						projectile.position.Y
					), 22, 34, chosenDust, 0.0f, 0.0f, 200, new Color(), 1.2f);
					Main.dust[index2].scale = scale;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].fadeIn = fadeIn;
				}
			}
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if ((double)Vector2.Distance(projectile.Center, npc.Center) < (double)400f && !npc.boss && !npc.friendly && npc.damage > 0 && npc.type != mod.NPCType("Undead_Warlock") && npc.active)
				{	
					drawDustBeetweenThisAndThat(projectile.Center, npc.Center);			
					npc.GetGlobalNPC<Undead_Warlock_NPC>().isNecrofied = true;
					break;						
				}			
			}
		}
		public void drawDustBeetweenThisAndThat(Vector2 vector3, Vector2 vector1)
		{
			healTimer++;
			if (healTimer % 60 == 0 && Main.npc[(int)projectile.ai[1]].life < Main.npc[(int)projectile.ai[1]].lifeMax - 5)
			{
				Main.npc[(int)projectile.ai[1]].life += 5;
				Main.npc[(int)projectile.ai[1]].HealEffect(5, true);
			}
				
			Vector2 range = vector3 - vector1;
			for (int i = 0; i < 12; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(vector1 + range * Main.rand.NextFloat() + Vector2.Zero, 1, 1, DustID.ChlorophyteWeapon)];
				dust.noGravity = true;
				dust.noLight = false;
				dust.velocity = range * 0.01f;
				dust.scale = 0.7f;
				dust.alpha = 240;
			}
		}
		public override void Kill(int timeLeft)
		{
		}
	}
}