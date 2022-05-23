using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Mechanics.Boids
{
	public class BoidHost
	{
		internal List<Flock> Flocks = new List<Flock>();
		private const int SPAWNRATE = 40;

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Flock fishflock in Flocks)
			{
				fishflock.Draw(spriteBatch);
			}
		}

		public void Update()
		{
			foreach (Flock fishflock in Flocks)
				fishflock.Update();

			Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetSpiritPlayer();
			//Test
			if ((Main.GameUpdateCount % SPAWNRATE == 39 && Main.LocalPlayer.ZoneBeach) || (Main.GameUpdateCount % SPAWNRATE > 37 && modPlayer.nearLure))
			{
				int flock = Main.rand.Next(0, Flocks.Count);
				int fluff = 1000;

				var rand = new Vector2(
					Main.rand.Next(-Main.screenWidth / 2 - fluff, Main.screenWidth / 2 + fluff),
					Main.rand.Next(-Main.screenHeight / 2 - fluff, Main.screenHeight / 2 + fluff));

				if (!new Rectangle(0, 0, Main.screenWidth, Main.screenHeight).Contains(rand.ToPoint()))
				{
					Vector2 position = Main.LocalPlayer.Center + rand;
					Point tP = position.ToTileCoordinates();
					if (WorldGen.InWorld(tP.X, tP.Y, 10))
					{
						Tile tile = Framing.GetTileSafely(tP.X, tP.Y);
						if (tile.liquid > 100)
							Flocks[flock].Populate(position, Main.rand.Next(20, 30), 50f);
					}
				}
			}
		}

		public void LoadContent()
		{
			const int AmbientFishTextureCount = 9;
			int seed = (int)Main.GameUpdateCount;

			int flocks = (seed % 8) + 6;
			for (int i = 0; i < flocks; i++)
			{
				int big = (seed + i) % 14;

				Texture2D[] textures = new Texture2D[Main.rand.Next(1, 5)];
				Texture2D[] sardineTexture = new Texture2D[] { SpiritMod.Instance.GetTexture($"Textures/AmbientFish/sardine") };
				Texture2D[] shrimpTexture = new Texture2D[] { SpiritMod.Instance.GetTexture($"Textures/AmbientFish/shrimp") };


				bool[] addedIDs = new bool[AmbientFishTextureCount];

				for (int j = 0; j < textures.Length; ++j)
				{
					int id = Main.rand.Next(AmbientFishTextureCount);

					if (!addedIDs[id]) //So we don't have multiple of the same texture
					{
						textures[j] = SpiritMod.Instance.GetTexture($"Textures/AmbientFish/fish_{id}");
						addedIDs[id] = true;
					}
					else
						j--;
				}
				if (big < 6)
					Flocks.Add(new Flock(textures, 1f, Main.rand.Next(5, 20)));
				if (big >= 6 && big < 11)
					Flocks.Add(new Flock(textures, 1f, Main.rand.Next(30, 60)));
				if (big >= 11 && big < 13)
					Flocks.Add(new Flock(sardineTexture, .525f, 60));
				if (big == 13)
					Flocks.Add(new Flock(shrimpTexture, .8f, Main.rand.Next(30, 60)));
			}

			On.Terraria.Main.DrawWoF += Main_DrawWoF;
		}

		//TODO: Move to update hook soon
		private void Main_DrawWoF(On.Terraria.Main.orig_DrawWoF orig, Main self)
		{
			if (Flocks != null)
			{
				Update();
				Draw(Main.spriteBatch);
			}
			orig(self);
		}

		public void UnloadContent()
		{
			if (Flocks != null)
				Flocks.Clear();

			On.Terraria.Main.DrawWoF -= Main_DrawWoF;
		}
	}
}