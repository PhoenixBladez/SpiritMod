using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace SpiritMod
{
	public static class SpiritDetours
	{
		public static void Initialize()
		{
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Main.DrawNPC += Main_DrawNPC;
			On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile;
			On.Terraria.Player.KeyDoubleTap += Player_KeyDoubleTap;
			On.Terraria.Main.DrawDust += DrawAdditive;
		}

		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			SpiritMod.primitives.DrawTrailsProj(Main.spriteBatch);
			SpiritMod.TrailManager.DrawTrails(Main.spriteBatch);
			orig(self);
		}

		private static void Main_DrawNPC(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
		{
			SpiritMod.primitives.DrawTrailsNPC(Main.spriteBatch);
			orig(self, iNPCIndex, behindTiles);
		}

		private static int Projectile_NewProjectile(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
		{
			int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
			Projectile projectile = Main.projectile[index];

			if (Main.netMode != NetmodeID.Server) SpiritMod.TrailManager.DoTrailCreation(projectile);

			return index;
		}

		private static void Player_KeyDoubleTap(On.Terraria.Player.orig_KeyDoubleTap orig, Player self, int keyDir)
		{
			orig(self, keyDir);
			self.GetSpiritPlayer().DoubleTapEffects(keyDir);
		}

		//Additive drawing stuff. Optimize this later?
		private static void DrawAdditive(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);
			Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

			for (int k = 0; k < Main.maxProjectiles; k++) //projectiles
				if (Main.projectile[k].active && Main.projectile[k].modProjectile is IDrawAdditive) (Main.projectile[k].modProjectile as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			for (int k = 0; k < Main.maxNPCs; k++) //NPCs
				if (Main.npc[k].active && Main.npc[k].modNPC is IDrawAdditive) (Main.npc[k].modNPC as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			Main.spriteBatch.End();
		}
	}
}
