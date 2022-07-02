using Microsoft.Xna.Framework;
using SpiritMod.Particles;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace SpiritMod
{
	internal class SpiritModSystem : ModSystem
	{
		public override void ModifyLightingBrightness(ref float scale)
		{
			if (Main.LocalPlayer.GetSpiritPlayer().ZoneReach && !Main.dayTime)
				scale *= .96f;
		}

		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		{
			if (MyWorld.SpiritTiles > 0)
			{
				float strength = MyWorld.SpiritTiles / 160f;
				if (strength > MyWorld.spiritLight)
					MyWorld.spiritLight += 0.01f;
				if (strength < MyWorld.spiritLight)
					MyWorld.spiritLight -= 0.01f;
			}
			else
				MyWorld.spiritLight -= 0.02f;

			if (MyWorld.spiritLight < 0f)
				MyWorld.spiritLight = 0f;
			else if (MyWorld.spiritLight > .9f)
				MyWorld.spiritLight = .9f;

			int ColorAdjustment(int col, float light)
			{
				float val = 250f / 1.14f * light * (col / 255f);
				if (val < 0)
					val = 0;
				return (int)val;
			}

			if (MyWorld.spiritLight > 0f)
			{
				int r = backgroundColor.R - ColorAdjustment(backgroundColor.R, MyWorld.spiritLight);
				int g = backgroundColor.G - ColorAdjustment(backgroundColor.G, MyWorld.spiritLight);
				int b = backgroundColor.B - ColorAdjustment(backgroundColor.B, MyWorld.spiritLight);

				backgroundColor.R = (byte)r;
				backgroundColor.G = (byte)g;
				backgroundColor.B = (byte)b;
			}

			if (MyWorld.AsteroidTiles > 0)
			{
				float strength = MyWorld.AsteroidTiles / 160f;
				if (strength > MyWorld.asteroidLight)
					MyWorld.asteroidLight += 0.01f;
				if (strength < MyWorld.asteroidLight)
					MyWorld.asteroidLight -= 0.01f;
			}
			else
				MyWorld.asteroidLight -= 0.02f;

			if (MyWorld.asteroidLight < 0f)
				MyWorld.asteroidLight = 0f;
			else if (MyWorld.asteroidLight > 1f)
				MyWorld.asteroidLight = 1f;

			if (MyWorld.asteroidLight > 0f)
			{
				int r = backgroundColor.R - ColorAdjustment(backgroundColor.R, MyWorld.asteroidLight);
				if (backgroundColor.R > r)
					backgroundColor.R = (byte)r;

				int g = backgroundColor.G - ColorAdjustment(backgroundColor.G, MyWorld.asteroidLight);
				if (backgroundColor.G > g)
					backgroundColor.G = (byte)g;

				int b = backgroundColor.B - ColorAdjustment(backgroundColor.B, MyWorld.asteroidLight);

				if (backgroundColor.B > b)
					backgroundColor.B = (byte)b;
			}
		}

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			SpiritMod mod = Mod as SpiritMod;

			if (!Main.gameMenu)
			{
				mod.screenshakeTimer++;

				if (SpiritMod.tremorTime > 0 && mod.screenshakeTimer >= 20) // so it doesnt immediately decrease
					SpiritMod.tremorTime -= 0.5f;
				if (SpiritMod.tremorTime < 0)
					SpiritMod.tremorTime = 0;

				Main.screenPosition += new Vector2(SpiritMod.tremorTime * Main.rand.NextFloat(), SpiritMod.tremorTime * Main.rand.NextFloat());
			}
			else // dont shake on the menu
			{
				SpiritMod.tremorTime = 0;
				mod.screenshakeTimer = 0;
			}

			mod.InvokeModifyTransform(Transform);
		}

		public override void UpdateUI(GameTime gameTime)
		{
			SpiritMod mod = Mod as SpiritMod;

			mod.BookUserInterface?.Update(gameTime);
			mod.SlotUserInterface?.Update(gameTime);
		}

		public override void PostUpdateInput()
		{
			SpiritMod.nighttimeAmbience?.Update();
			SpiritMod.underwaterAmbience?.Update();
			SpiritMod.wavesAmbience?.Update();
			SpiritMod.lightWind?.Update();
			SpiritMod.desertWind?.Update();
			SpiritMod.caveAmbience?.Update();
			SpiritMod.spookyAmbience?.Update();
			SpiritMod.scarabWings?.Update();
		}

		public override void PostUpdateEverything()
		{
			if (!Main.dedServ)
			{
				ParticleHandler.RunRandomSpawnAttempts();
				ParticleHandler.UpdateAllParticles();
			}
		}
	}
}
