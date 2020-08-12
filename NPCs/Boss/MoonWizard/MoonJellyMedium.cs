
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory;
using System;

namespace SpiritMod.NPCs.Boss.MoonWizard
{
	public class MoonJellyMedium : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 72;
			npc.damage = 18;
			npc.defense = 6;
			npc.lifeMax = 100;
			npc.noGravity = true;
			npc.value = 90f;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.DD2_GoblinHurt;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.noGravity = true;
        }
		int xoffset = 0;
		Vector2 direction = Vector2.Zero;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[2] != 0) {
				drawAfterimage(spriteBatch, drawColor);
			}
			return base.PreDraw(spriteBatch,drawColor);
		}
		public void drawAfterimage(SpriteBatch spriteBatch, Color drawColor) //we have access to this method already
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Microsoft.Xna.Framework.Color AfterimageColor = new Microsoft.Xna.Framework.Color((int)sbyte.MaxValue, (int)sbyte.MaxValue, (int)sbyte.MaxValue, 0).MultiplyRGBA(new Color(75, 231, 255, 150)) * 2f;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			for (int index = 1; index < 10; ++index) {
				Main.spriteBatch.Draw(texture, new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)index * 0.5f, npc.frame, AfterimageColor, npc.rotation, npc.frame.Size() / 2, MathHelper.Lerp(npc.scale * 1.5f, 0.8f, (float)index / 10), spriteEffects, 0.0f);
			}
		}
		public override void AI()
		{

			Player player = Main.player[npc.target];
			if (npc.ai[2] == 0) {

				npc.ai[0]++;
				if (player.position.X > npc.position.X) {
					xoffset = 16;
				}
				else {
					xoffset = -16;
				}
				npc.velocity.X *= 0.99f;
				if (npc.ai[1] == 0) {
					if (npc.velocity.Y < 2.5f) {
						npc.velocity.Y += 0.1f;
					}
					if (player.position.Y < npc.position.Y && npc.ai[0] % 30 == 0) {
						npc.ai[1] = 1;
						npc.netUpdate = true;
						npc.velocity.X = xoffset / 0.75f;
						npc.velocity.Y = -10;
					}
				}
				if (npc.ai[1] == 1) {
					npc.velocity *= 0.97f;
					if (Math.Abs(npc.velocity.X) < 0.125f) {
						npc.ai[1] = 0;
						npc.netUpdate = true;
					}
					npc.rotation = npc.velocity.ToRotation() + 1.57f;
				}
			}
			else 
			{
				npc.ai[3]++;
				if (npc.ai[3] < 30) 
				{
					npc.velocity = Vector2.Zero;
				}
				else 
				{
					npc.frameCounter += 0.15f;
				}
				if (npc.ai[3] < 60) 
				{
					direction = player.position - npc.position;
					direction.Normalize();
					direction *= 15;
				}
				if (npc.ai[3] == 60) 
				{
					npc.noGravity = true;
				}
				if (npc.ai[3] > 60) 
				{
					npc.velocity = direction;
					if (npc.position.Y > player.position.Y - 50) 
					{
						npc.noTileCollide = false;
					}
					if ((npc.collideX || npc.collideY) || npc.ai[3] > 120) {
						npc.active = false;
						int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("BarrelExplosionLarge"), 20, 0);
					}
						
				}
				npc.rotation = direction.ToRotation() + 1.57f;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
