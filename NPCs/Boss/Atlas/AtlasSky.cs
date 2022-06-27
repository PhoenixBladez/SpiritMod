using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Atlas
{
	public class AtlasSky : CustomSky
	{
		private bool isActive = false;
		private float intensity = 0f;
		private int AtlasIndex = -1;

		public override void Update(GameTime gameTime)
		{
			if (isActive && intensity < 1f) {
				intensity += 0.02f;
			}
			else if (!isActive && intensity > 0f) {
				intensity -= 0.02f;
			}
		}

		public override Color OnTileColor(Color inColor)
		{
			float amt = intensity * .02f;
			return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
		}

		private bool UpdateAtlasIndex()
		{
			int AtlasType = ModLoader.GetMod("SpiritMod").Find<ModNPC>("Atlas").Type;
			if (AtlasIndex >= 0 && Main.npc[AtlasIndex].active && Main.npc[AtlasIndex].type == AtlasType)
				return true;

			AtlasIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++) {
				if (Main.npc[i].active && Main.npc[i].type == AtlasType) {
					AtlasIndex = i;
					break;
				}
			}
			//this.DoGIndex = DoGIndex;
			return AtlasIndex != -1;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.13f, 0.13f, 0.13f) * intensity);
			}
			//front of bg
			if (maxDepth >= 0 && minDepth < 0) {
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.3f, 0.3f, 0.3f) * 0.5f);
			}
		}

		public override float GetCloudAlpha() => 0f;
		public override void Activate(Vector2 position, params object[] args) => isActive = true;
		public override void Deactivate(params object[] args) => isActive = false;
		public override void Reset() => isActive = false;
		public override bool IsActive() => isActive || intensity > 0f;
	}
}