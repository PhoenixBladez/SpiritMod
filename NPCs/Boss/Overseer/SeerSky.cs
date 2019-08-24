using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Overseer
{
	public class SeerSky : CustomSky
	{
		private bool isActive = false;
		private float intensity = 0f;
		private int SeerIndex = -1;

		public override void Update(GameTime gameTime)
		{
			if (isActive && intensity < 1f)
			{
				intensity += 0.01f;
			}
			else if (!isActive && intensity > 0f)
			{
				intensity -= 0.01f;
			}
		}

		private float GetIntensity()
		{
			if (this.UpdateSeerIndex())
			{
				float x = 0f;
				if (this.SeerIndex != -1)
				{
					x = Vector2.Distance(Main.player[Main.myPlayer].Center, Main.npc[this.SeerIndex].Center);
				}
				return 1f - Utils.SmoothStep(3000f, 6000f, x);
			}
			return 0f;
		}

		public override Color OnTileColor(Color inColor)
		{
			float intensity = this.GetIntensity();
			return new Color(Vector4.Lerp(new Vector4(0f, 0.3f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		private bool UpdateSeerIndex()
		{
			int SeerType = ModLoader.GetMod("SpiritMod").NPCType("Overseer");
			if (SeerIndex >= 0 && Main.npc[SeerIndex].active && Main.npc[SeerIndex].type == SeerType)
				return true;

			SeerIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == SeerType)
				{
					SeerIndex = i;
					break;
				}
			}
			//this.DoGIndex = DoGIndex;
			return SeerIndex != -1;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0 && minDepth < 0)
			{
				float intensity = this.GetIntensity();
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			isActive = false;
		}

		public override void Reset()
		{
			isActive = false;
		}

		public override bool IsActive()
		{
			return isActive || intensity > 0f;
		}
	}
}