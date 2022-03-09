using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;

namespace SpiritMod.NPCs.Sea_Mandrake
{
	public class Sea_Mandrake : ModNPC
	{
		public bool hasGottenColor = false;
		public bool screamed = false;
		public int r = 0;
		public int g = 0;
		public int b = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Mandrake");
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 20; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 16;
			npc.lifeMax = 50;
			npc.defense = 7;
			npc.value = 200f;
			npc.knockBackResist = 1.2f;
			npc.width = 30;
			npc.aiStyle = 16;
			aiType = NPCID.Goldfish;
			npc.height = 50;
			npc.damage = 0;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(r);
			writer.Write(g);
			writer.Write(b);
			writer.Write(hasGottenColor);

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			r = reader.ReadInt32();
			g = reader.ReadInt32();
			b = reader.ReadInt32();
			hasGottenColor = reader.ReadBoolean();

		}
		public override void AI()
		{
			npc.rotation = npc.velocity.X * .1f;
			Player player = Main.player[npc.target];
			
			if (Main.rand.Next(500)==0)
				Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Mandrake_Idle"));
			
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = -1;
			}
			else if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = 1;
			}
			if (Vector2.Distance(player.Center, npc.Center) <= 45f)
			{
				npc.velocity.X = 0f;
			}
			
			if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <=  200f && npc.wet)
				spawnInk();
			else if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) >  200f)
				screamed = false;
			
			if (npc.wet)
				Movement();

			if (!npc.wet && !player.wet)
				npc.velocity.Y = 8f;
				
			if (!hasGottenColor)
			{
				hasGottenColor = true;
				r = Main.rand.Next(1,255);
				g = Main.rand.Next(1,255);
				b = Main.rand.Next(1,255);
			}
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), r*0.002f, g*0.002f, b*0.002f);
			
			if (!player.wet)
			{
				for(int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile type = Main.projectile[i];

					if ( Vector2.Distance(type.Center, npc.Center) <=  100f &&  Vector2.Distance(type.Center, npc.Center) >  20f && type.friendly && type.position.X > npc.position.X && npc.wet && type.active)
					{
						Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10,10) + (float) (type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10,10) + (float) (type.height / 2) - vector2.Y;
						float num4 = 8f / (float) Math.Sqrt( num2 *  num2 +  num3 *  num3);
						npc.velocity.X = num2 * num4 * -1 * (5f / 6);
						npc.velocity.Y = num3 * num4 * -1 * (5f / 6);
						npc.spriteDirection = -1;
						npc.direction = -1;
					}
					else if ( Vector2.Distance(type.Center, npc.Center) <=  100f &&  Vector2.Distance(type.Center, npc.Center) >  20f && type.friendly && type.position.X < npc.position.X && npc.wet && type.active)
					{
						Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10,10) + (float) (type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10,10) + (float) (type.height / 2) - vector2.Y;
						float num4 = 8f / (float) Math.Sqrt( num2 *  num2 +  num3 *  num3);
						npc.velocity.X = num2 * num4 * -1 * (5f / 6);
						npc.velocity.Y = num3 * num4 * -1 * (5f / 6);
						npc.spriteDirection = 1;
						npc.direction = 1;
					}
				}
			}
		}
		
		private void Movement()
		{
			Player player = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
			if (distance < 130 && player.wet && npc.wet)
			{
				Vector2 vel = npc.DirectionFrom(player.Center);
				vel.Normalize();
				vel *= 6.5f;
				npc.velocity = vel;
				npc.rotation = npc.velocity.X * .15f;
				if (player.position.X > npc.position.X)
				{
					npc.spriteDirection = -1;
					npc.direction = -1;
					npc.netUpdate = true;
				}
				else if (player.position.X < npc.position.X)
				{
					npc.spriteDirection = 1;
					npc.direction = 1;
					npc.netUpdate = true;
				}
			}
		}
		
		private void spawnInk()
		{
			Player player = Main.player[npc.target];
			if (!screamed)
			{
				//Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Mandrake_Scream"));
				screamed = true;
			}
			if (Vector2.Distance(player.Center, npc.Center) <= 130f && player.position.Y < npc.position.Y + 100 && player.position.Y > npc.position.Y - 100 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))  
			{
				player.AddBuff(80,2);
				player.AddBuff(22,2);
				player.AddBuff(163,2);
				player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage/1.5f), npc.direction, false, false, false, -1);
			}
			for (int i = 0; i < 1; i++)
			{
				int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 0.0f, 0.0f, 100, new Color(r,g,b), 1.25f);
				Main.dust[index2].alpha += Main.rand.Next(100);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.3f;
				Main.dust[index2].velocity.X += (float)Main.rand.Next(-80, -40) * 0.025f * npc.velocity.X;
				Main.dust[index2].velocity.Y -= (float)(0.4f + Main.rand.Next(-3, 14) * 0.15f);
				Main.dust[index2].fadeIn = (float)(0.25 + Main.rand.Next(15) * 0.15f);
			}
			
			for (int i = 0; i < 1; i++)
			{
				int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 0.0f, 0.0f, 100, new Color(r,g,b), 3f);
				Main.dust[index2].alpha += Main.rand.Next(100);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.3f;
				Main.dust[index2].velocity.X += (float)Main.rand.Next(-240, -180) * 0.025f * npc.velocity.X;
				Main.dust[index2].velocity.Y -= (float)(0.4f + Main.rand.Next(-3, 14) * 0.15f);
				Main.dust[index2].fadeIn = (float)(0.25 + Main.rand.Next(10) * 0.15f);
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Luminance_Seacone"), 1);
			}
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int index1 = 0; index1 < 26; ++index1)
				{
				  int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 261, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, new Color(r,g,b), 2.5f);
				  Main.dust[index2].noGravity = true;
				  Main.dust[index2].fadeIn = 1f;
				  Main.dust[index2].velocity *= 4f;
				  Main.dust[index2].noLight = true;
				}
			}
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 261, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 261, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 261, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 0.7f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OceanMonster.Chance * 0.05f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			float addHeight = 13f;
			Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int) ( npc.position.X +  npc.width * 0.5) / 16, (int) (( npc.position.Y +  npc.height * 0.5) / 16.0));
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Sea_Mandrake/Sea_Mandrake_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) ( -Main.npcTexture[npc.type].Width *  npc.scale / 2.0 +  vector2_3.X *  npc.scale), (float) ( -Main.npcTexture[npc.type].Height *  npc.scale /  Main.npcFrameCount[npc.type] + 4.0 +  vector2_3.Y *  npc.scale) + addHeight + npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color((int) r - npc.alpha, (int) byte.MaxValue - npc.alpha, (int) g - npc.alpha, (int) b - npc.alpha), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
            float num = (float) (0.25 +  (npc.GetAlpha(color1).ToVector3() + new Vector3(2.5f)).Length() * 0.25);
            for (int index = 0; index < 16; ++index)
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/Sea_Mandrake/Sea_Mandrake_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) ( -Main.npcTexture[npc.type].Width *  npc.scale / 2.0 +  vector2_3.X *  npc.scale), (float) ( -Main.npcTexture[npc.type].Height *  npc.scale /  Main.npcFrameCount[npc.type] + 4.0 +  vector2_3.Y *  npc.scale) + addHeight + npc.gfxOffY) + npc.velocity.RotatedBy( index * 47079637050629, new Vector2()) * num, new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color(r, g, b, 0), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
		}
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (npc.frameCounter < 6)
			{
				npc.frame.Y = Frame_1 * frameHeight;
			}
			else if (npc.frameCounter < 12)
			{
				npc.frame.Y = Frame_2 * frameHeight;
			}
			else if (npc.frameCounter < 18)
			{
				npc.frame.Y = Frame_3 * frameHeight;
			}
			else if (npc.frameCounter < 24)
			{
				npc.frame.Y = Frame_4 * frameHeight;
			}
			else
			{
				npc.frameCounter = 0;
			}
		}
	}
}
