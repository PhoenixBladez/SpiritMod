using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
	public class SmallComet : ModNPC
	{
		public override sealed bool CloneNewInstances => true;

		protected virtual string Size => "Small";
		protected virtual float BeamScale => 0.75f;

		ref NPC Parent => ref Main.npc[(int)npc.ai[0]];
		ref float TimerOffset => ref npc.ai[1];
		ref float SpinMomentum => ref npc.ai[2];
		ref float RotationOffset => ref npc.ai[3];

		public float initialDistance = 0f;

		private readonly List<int> enemies = new List<int>();
		private bool spawnedEnemies = false;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Small Starjinx Comet");

		public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 10;
            npc.defense = 0;
            npc.value = 0f;
            aiType = 0;
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 38;
            npc.damage = 0;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.Item89;
            npc.alpha = 255;

            for (int i = 0; i < BuffLoader.BuffCount; i++)
                npc.buffImmune[i] = true;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            crit = false;
			damage = player.HeldItem.pick > 0 ? 5 : 1;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;
			damage = Main.player[projectile.owner].HeldItem.pick > 0 ? 5 : 1;
		}

		float sinCounter;
        float sinIncrement = 0;

		private bool spinPlateau = false;

        public override void AI()
        {
            if (npc.alpha > 0)
                npc.alpha -= 10;
            else
                npc.alpha = 0;

            if (RotationOffset == -1) //rotation init
			{
				RotationOffset = npc.AngleTo(Parent.Center);
				initialDistance = npc.Distance(Parent.Center);
				if (this is MediumComet)
					initialDistance *= 0.5f;
				if (this is LargeComet)
					initialDistance *= 0.1f;
				npc.position = Parent.Center + new Vector2(0, initialDistance).RotatedBy(RotationOffset);
			}
			else
			{
				var pos = new Vector2(0, initialDistance * (float)(1 + Math.Sin(sinCounter) * 0.05f));
				npc.position = Parent.Center + pos.RotatedBy(RotationOffset);
				npc.rotation = RotationOffset;
				RotationOffset += SpinMomentum * BeamScale;
			}

            if (sinIncrement == 0) sinIncrement = Main.rand.NextFloat(0.025f, 0.035f);

			if (!spinPlateau) //startup spin speed thing
			{
				SpinMomentum += 0.00005f;
				if (SpinMomentum > 0.005f && Main.rand.NextBool(10))
					spinPlateau = true;
			}

            sinCounter += sinIncrement;
            npc.TargetClosest(true);

			if (spawnedEnemies && enemies.Count > 0)
			{
				for (int i = 0; i < enemies.Count; ++i) //update enemy list
				{
					NPC npc = Main.npc[enemies[i]];
					if ((!npc.active || npc.life <= 0) || !npc.GetGlobalNPC<StarjinxGlobalNPC>().spawnedByComet)
					{
						enemies.RemoveAt(i);
						i--;
					}
				}

				if (enemies.Count <= 0) //if all enemies are dead, make me vulnerable
					npc.dontTakeDamage = false;
			}

            if (!Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>()) //kill me if the main meteor is dead somehow
                npc.active = false;

            if (Main.rand.Next(200) == 0)
                Gore.NewGorePerfect(npc.Center, (Parent.Center - npc.Center) / 45f, mod.GetGoreSlot("Gores/StarjinxGore"), 1);
        }

		public virtual void SpawnWave()
		{
			int choice = Main.rand.Next(4);

			switch (choice)
			{
				case 0: //4 random starachnid/starweaver
					for (int i = 0; i < 4; ++i)
						SpawnValidNPC(Main.rand.NextBool(2) ? ModContent.NPCType<Starachnid>() : ModContent.NPCType<StarWeaverNPC>());
					break;
				case 1: //fan of 3 starweavers
					for (int i = -1; i < 2; ++i)
					{
						float rotation = i * (MathHelper.Pi / 3);
						SpawnValidNPC(ModContent.NPCType<StarWeaverNPC>(), (Vector2.UnitY * 200).RotatedBy(rotation));
					}
					break;

				default: //2 starachnid, 1 pathfinder
					for (int i = 0; i < 2; ++i)
						SpawnValidNPC(ModContent.NPCType<Starachnid>());
					SpawnValidNPC(ModContent.NPCType<Pathfinder>());
					break;
			}
		}

		/// <summary>Spawns an NPC that is attached to this comet with an automatic (and overrideable) offset.</summary>
		/// <param name="type">NPCID to spawn.</param>
		/// <param name="overrideOffset">If not null, this is the offset used to spawn, based off of the origin npc.Center.</param>
		/// <returns>The spawn offset used.</returns>
		internal Vector2 SpawnValidNPC(int type, Vector2? overrideOffset = null)
		{
			int X = (int)(npc.Center.X + Main.rand.Next(-500, 500));
			int Y = (int)(npc.Center.Y + Main.rand.Next(-500, 500));

			if (overrideOffset.HasValue)
			{
				X = (int)(npc.Center.X + overrideOffset.Value.X);
				Y = (int)(npc.Center.Y + overrideOffset.Value.Y);
			}

			bool success = false;

			while (!success)
			{
				var temp = new NPC(); //so I can get the size of the NPC without hardcoding
				temp.SetDefaults(type);

				if (!Collision.SolidCollision(new Vector2(X, Y), temp.width, temp.height))
				{
					success = true;

					int id = NPC.NewNPC(X, Y, type);
					NPC n = Main.npc[id];
					n.GetGlobalNPC<StarjinxGlobalNPC>().spawnedByComet = true;

					enemies.Add(id);
					spawnedEnemies = true;

					if (Main.netMode == NetmodeID.MultiplayerClient)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);

					//add spawn vfx here
				}
			}
			return new Vector2(X, Y) - npc.Center;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Teleporter, 2.5f * hitDirection, -2.5f, 0, default, 0.6f);
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.TeleportationPotion, 2.5f * hitDirection, -2.5f, 0, default, 1.25f);
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Teleporter, 2.5f * hitDirection, -2.5f, 0, default, 0.85f);
            }

            if (npc.life <= 0)
                Main.PlaySound(SoundID.Item14, npc.position);
        }

        public float Timer => Main.GlobalTime + TimerOffset; //Used to offset the beam/sine wave motion

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var center = new Vector2(Main.npcTexture[npc.type].Width / 2, (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float cos = (float)Math.Cos(Timer % 2.4f / 2.4f * MathHelper.TwoPi) / 2f + 0.5f;

			DrawBeam(spriteBatch);

            SpriteEffects effect = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
            Color col = npc.GetAlpha(baseCol) * (1f - cos);

            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White), npc.rotation, center, 1, SpriteEffects.None, 0f);

            for (int i = 0; i < 6; i++)
            {
                var drawPos = npc.Center + (i / 4 * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * cos + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
                spriteBatch.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}CometGlow"), drawPos, npc.frame, col, npc.rotation, npc.frame.Size() / 2f, npc.scale, effect, 0f);
            }
			return false;
		}

		private void DrawBeam(SpriteBatch b)
		{
			Texture2D beam = mod.GetTexture("Textures/Medusa_Ray");
			float rotation = npc.DirectionTo(Main.npc[(int)npc.ai[0]].Center).ToRotation();
			float fluctuate = (float)Math.Sin(Timer * 1.2f) / 6 + 0.125f;

			Color color = SpiritMod.StarjinxColor(Timer * 0.8f);
			color = Color.Lerp(color, Color.Transparent, fluctuate * 2);

			Rectangle rect = new Rectangle(0, 0, beam.Width, beam.Height);
			Vector2 scale = new Vector2((1 - fluctuate) * npc.Distance(Main.npc[(int)npc.ai[0]].Center) / beam.Width, 1) * BeamScale;
			Vector2 offset = new Vector2(100 * scale.X, 0).RotatedBy(rotation);
			b.Draw(beam, npc.Center - Main.screenPosition + offset / 2, new Rectangle?(rect), npc.GetAlpha(color), rotation, rect.Size() / 2, scale, SpriteEffects.None, 0);
		}

		public override bool CheckDead()
		{
			(Parent.modNPC as StarjinxMeteorite).updateCometOrder = true;

			enemies.Clear();
			spawnedEnemies = false;
			return true;
		}

		public override bool CheckActive() => !Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>();
	}
}