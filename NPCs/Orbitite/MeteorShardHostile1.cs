using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Orbitite
{
	public class MeteorShardHostile1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Shard");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
		}

		public override void AI()
		{
			Projectile.rotation += .3f;
			int num1 = ModContent.NPCType<NPCs.Orbitite.Mineroid>();
			if (!Main.npc[(int)Projectile.ai[1]].active) {
				Projectile.timeLeft = 0;
				Projectile.active = false;
			}
			float num2 = 60f;
			float x = 0.5f;
			float y = 0.25f;
			bool flag2 = false;
			if ((double)Projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)Projectile.ai[1];
				if (Main.npc[index1].active && Main.npc[index1].type == num1) {
					if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
						Projectile.position = Projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
				}
				else {
					Projectile.ai[0] = num2;
					flag4 = false;
				}
				if (flag4 && !flag2) {
					Projectile.velocity = Projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
				}
			}

		}
		public override void Kill(int timeLeft)
		{
			float maxDistance = 500f; // max distance to search for a player
			int index = -1;
			for (int i = 0; i < Main.maxPlayers; i++) {
				Player target = Main.player[i];
				if (!target.active || target.dead) {
					continue;
				}
				float curDistance = Projectile.Distance(target.Center);
				if (curDistance < maxDistance) {
					index = i;
					maxDistance = curDistance;
				}
			}
			if (index != -1)  {
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[index].Center - Projectile.Center;
					direction.Normalize();
					direction *= 5f;
					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i) {
						//float A = (float)Main.rand.Next(-200, 200) * 0.05f;
						//float B = (float)Main.rand.Next(-200, 200) * 0.05f;
						Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, direction, ModContent.ProjectileType<MeteorShardHostile2>(), Projectile.damage, 0, Main.myPlayer);
					}
				}
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 89);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
