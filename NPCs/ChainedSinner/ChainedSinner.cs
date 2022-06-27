using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.VerletChains;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ChainedSinner
{
	internal class ChainedSinner : ModNPC
	{
		public int parentid;
		public Chain chain;

		public float angularMomentum = 0;

		private Vector2 arbitraryVelocity;
		private Vector2 CachedVel;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Chained Sinner");

		public override void SetDefaults()
		{
			NPC.width = 56;
			NPC.height = 74;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;
			NPC.lifeMax = 500;
			NPC.damage = 100;
			NPC.defense = 20;
			NPC.noTileCollide = false;
		}

		public override void AI()
		{
			NPC.TargetClosest(true);
			NPC.ai[0]++;

			NPC parent = Main.npc[parentid];
			if (!parent.active)
			{
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				return;
			}
			chain.Update(parent.Center - new Vector2(0, 1), NPC.Center);

			float X = (float)Math.Sin(NPC.ai[0] / 50f);

			Player player = Main.player[NPC.target];
			if (NPC.ai[0] % 600 < 30)
			{
				if (NPC.ai[0] % 600 == 1) CachedVel = Vector2.Normalize(player.Center - NPC.Center);
				if (NPC.ai[0] % 600 > 1) arbitraryVelocity += CachedVel / 1.5f;
			}
			else if (NPC.ai[0] % 600 > 200)
			{
				float timeSince = NPC.ai[0] % 600 - 200;
				arbitraryVelocity = new Vector2(X, 0) * timeSince / 150f;
			}
			else
				arbitraryVelocity *= 0.42f;

			chain.LastVertex.Position += arbitraryVelocity;

			NPC.Center = chain.LastVertex.Position;
		}

		public override bool CheckDead()
		{
			NPC parent = Main.npc[parentid];
			parent.active = false;
			return true;
		}

		public void InitializeChain(Vector2 position) => chain = new Chain(16, 16, position, new ChainPhysics(0.95f, 0.5f, 0.4f), true, false);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			chain.Draw(spriteBatch, Mod.Assets.Request<Texture2D>("NPCs/ChainedSinner/ChainedSinner_Chain").Value);
			return true;
		}
	}
}