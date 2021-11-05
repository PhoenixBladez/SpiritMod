using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent.Comets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	class StarjinxEnemySpawner : ModProjectile
	{
		public int enemyToSpawn = NPCID.Guide; //lol
		//public int cometWhoAmI = -1;
		public Vector2 spawnPosition = Vector2.Zero;
		public bool rawPos = false;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Starjinx Enemy Spawner");

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.timeLeft = Main.rand.Next(60, 180);
			projectile.height = 28;
			projectile.alpha = 30;
			projectile.width = 18;
			projectile.penetrate = -1;
			projectile.extraUpdates = 1;
		}

		public override void Kill(int timeLeft)
		{
			//NPC parent = Main.npc[cometWhoAmI];
			//if (parent.modNPC != null && parent.modNPC is SmallComet comet)
			//{
				int id = NPC.NewNPC((int)spawnPosition.X, (int)spawnPosition.Y, enemyToSpawn);
				NPC n = Main.npc[id];
				n.GetGlobalNPC<StarjinxGlobalNPC>().spawnedByComet = true;

				if (Main.netMode == NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, id);
			//}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => true; //add spawn vfx here or above
	}
}
