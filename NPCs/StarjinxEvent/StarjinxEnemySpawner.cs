using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent.Comets;
using SpiritMod.Particles;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	class StarjinxEnemySpawner : ModProjectile, IDrawAdditive
	{
		public int enemyToSpawn = NPCID.Guide; //Enemy that is spawned by this projectile
		public Vector2 spawnPosition = Vector2.Zero; //Position to spawn the enemy at

		/// <summary>See <seealso cref="SmallComet.SpawnSpawnerProjectile"/> for what this does.</summary>
		public bool rawPos = false;

		private ref float Timer => ref projectile.ai[0];
		private Color chosenColor;

		private const int MINTIMELEFT = 90; //Used to determine when its timer activates
		private const int MAXTIMELEFT = 180;

		private const int FADEINTIME = 40; //How long it takes to fade in fully
		private const int IDLETIME = 20; //How long it stays at full opacity before expanding and fading out
		private int FADEOUTTIME => MINTIMELEFT - FADEINTIME - IDLETIME; //Remaining lifetime calculation, how long the projectile takes to fade out and expand

		public override void SetStaticDefaults() => DisplayName.SetDefault("Starjinx Enemy Spawner");

		public override void SetDefaults()
		{
			projectile.Size = Vector2.One;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = Main.rand.Next(MINTIMELEFT, MAXTIMELEFT);
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			projectile.hide = true;

			switch (Main.rand.Next(3))
			{
				case 0:
					chosenColor = Color.Gold;
					break;
				case 1:
					chosenColor = Color.Cyan;
					break;
				case 2:
					chosenColor = Color.Magenta;
					break;
			}

			chosenColor = Color.Lerp(chosenColor, Color.White, 0.7f);
		}

		public override void AI()
		{
			if (projectile.timeLeft < MINTIMELEFT) //Start incrementing timer when the random delay is over
				Timer++;

			projectile.rotation += 0.1f;
			if (Timer == 0) //Stop if timer hasn't started incrementing yet
				return;

			Lighting.AddLight(projectile.Center, Color.White.ToVector3() * projectile.Opacity);
			if (Timer < FADEINTIME) //Fade in and shrink scale
			{
				float progress = Timer / FADEINTIME;
				projectile.alpha = (int)MathHelper.Lerp(255, 0, PowF(progress, 0.5f));
				projectile.scale = MathHelper.Lerp(1f, 0.66f, PowF(progress, 0.5f));
			}

			else if((Timer - FADEINTIME) < IDLETIME) //Compress slightly at full opacity
			{
				float progress = (Timer - FADEINTIME) / IDLETIME;
				projectile.scale = MathHelper.Lerp(0.66f, 0.5f, progress);
			}

			else
			{
				if(Timer == IDLETIME + FADEINTIME + 1)
				{
					int id = NPC.NewNPC((int)spawnPosition.X, (int)spawnPosition.Y, enemyToSpawn);
					NPC n = Main.npc[id];
					n.GetGlobalNPC<StarjinxGlobalNPC>().spawnedByComet = true;

					if (Main.netMode == NetmodeID.MultiplayerClient)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);

					if (Main.netMode != NetmodeID.Server)
					{
						for (int i = 0; i < 12; i++)
							ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 5), chosenColor, Main.rand.NextFloat(0.2f, 0.3f), 25));

						ParticleHandler.SpawnParticle(new PulseCircle(projectile.Center, chosenColor * 0.4f, 200, 15, PulseCircle.MovementType.OutwardsSquareRooted) { RingColor = chosenColor }); ;
					}
				}

				float progress = (Timer - FADEINTIME - IDLETIME) / FADEOUTTIME;
				projectile.alpha = (int)MathHelper.Lerp(0, 255, PowF(progress, 1.2f));
				projectile.scale = MathHelper.Lerp(0.66f, 2.5f, PowF(progress, 1.2f));
			}
		}

		private float PowF(float x, float y) => (float)Math.Pow(x, y); //Shorthand for casting Math.Pow to a float

		public void AdditiveCall(SpriteBatch sB)
		{
			Texture2D star = mod.GetTexture("Effects/Masks/Star");
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			Texture2D ring = mod.GetTexture("Effects/WispSwitchGlow2");
			Texture2D ray = mod.GetTexture("Effects/Mining_Helmet");

			//Make a dummy npc to determine the base scale, by finding the ratio between the used texture and the npc's size
			NPC dummy = new NPC();
			dummy.SetDefaults(enemyToSpawn);
			float baseScale = Math.Max(dummy.width, dummy.height);
			baseScale /= star.Width;
			baseScale *= projectile.scale;
			if (enemyToSpawn == ModContent.NPCType<Enemies.StarWeaver.StarWeaverNPC>()) //Hardcoded to be bigger because of star weaver head
				baseScale *= 1.75f;

			const int MaxRays = 6;
			for (int i = 0; i < MaxRays; i++)
			{
				float rotation = projectile.rotation + (MathHelper.TwoPi * i / MaxRays);
				sB.Draw(ray, projectile.Center - Main.screenPosition, null, chosenColor * projectile.Opacity, rotation, ray.Size() / 2, baseScale, SpriteEffects.None, 0);
			}

			sB.Draw(bloom, projectile.Center - Main.screenPosition, null, chosenColor * 0.66f * projectile.Opacity, 0f, bloom.Size() / 2, baseScale, SpriteEffects.None, 0);

			sB.Draw(star, projectile.Center - Main.screenPosition, null, chosenColor * projectile.Opacity, projectile.rotation, star.Size() / 2, baseScale, SpriteEffects.None, 0);
			sB.Draw(star, projectile.Center - Main.screenPosition, null, chosenColor * projectile.Opacity, -projectile.rotation, star.Size() / 2, baseScale * 0.75f, SpriteEffects.None, 0);

			sB.Draw(ring, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, 0, ring.Size() / 2, baseScale * 0.8f, SpriteEffects.None, 0);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(enemyToSpawn);
			writer.WriteVector2(spawnPosition);
			writer.Write(rawPos);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			enemyToSpawn = reader.ReadInt32();
			spawnPosition = reader.ReadVector2();
			rawPos = reader.ReadBoolean();
		}
	}
}
