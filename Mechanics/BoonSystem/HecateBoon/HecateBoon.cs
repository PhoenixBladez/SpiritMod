using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.NPCs;
using Terraria.ID;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoon : Boon
	{
		public override bool CanApply => true;
		public override string TexturePath => "SpiritMod/Mechanics/BoonSystem/HecateBoon/HecateBoon";
		public override Vector2 SigilSize => new Vector2(30, 28);

		private float projectileCounter;
		private const float RUNE_INTERVAL = 300;

		public override void SetStats() => npc.lifeMax = npc.life = (int)(npc.lifeMax * 1.5f);

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Violet.ToVector3() * 0.3f);

			//Spawn 3 rune projectiles every few seconds
			if (++projectileCounter % RUNE_INTERVAL == (RUNE_INTERVAL / 3) && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < 3; i++)
				{
					int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<HecateBoonRune>(), 0, 0, npc.target, npc.whoAmI, i);
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawBeam(new Color(255, 106, 250, 0), new Color(185, 38, 235, 0));

			DrawBloom(spriteBatch, new Color(255, 106, 250) * 0.5f, 0.5f);

			DrawSigil(spriteBatch);
		}
	}
}