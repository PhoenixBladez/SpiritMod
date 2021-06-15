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
			npc.width = 56;
			npc.height = 74;
			npc.aiStyle = -1;
			npc.knockBackResist = 0;
			npc.lifeMax = 500;
			npc.damage = 100;
			npc.defense = 20;
			npc.noTileCollide = false;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			npc.ai[0]++;

			NPC parent = Main.npc[parentid];
			if (!parent.active)
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				return;
			}
			chain.Update(parent.Center - new Vector2(0, 1), npc.Center);

			float X = (float)Math.Sin(npc.ai[0] / 50f);

			Player player = Main.player[npc.target];
			if (npc.ai[0] % 600 < 30)
			{
				if (npc.ai[0] % 600 == 1) CachedVel = Vector2.Normalize(player.Center - npc.Center);
				if (npc.ai[0] % 600 > 1) arbitraryVelocity += CachedVel / 1.5f;
			}
			else if (npc.ai[0] % 600 > 200)
			{
				float timeSince = npc.ai[0] % 600 - 200;
				arbitraryVelocity = new Vector2(X, 0) * timeSince / 150f;
			}
			else
				arbitraryVelocity *= 0.42f;

			chain.LastVertex.Position += arbitraryVelocity;

			npc.Center = chain.LastVertex.Position;
		}

		public override bool CheckDead()
		{
			NPC parent = Main.npc[parentid];
			parent.active = false;
			return true;
		}

		public void InitializeChain(Vector2 position) => chain = new Chain(mod.GetTexture("NPCs/ChainedSinner/ChainedSinner_Chain"), mod.GetTexture("NPCs/ChainedSinner/ChainedSinner_Chain"), 16, 16, position, new ChainPhysics(0.95f, 0.5f, 0.4f), true, false);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			chain.Draw(spriteBatch);
			return true;
		}
	}
}