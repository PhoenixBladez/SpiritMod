using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Magic.Rhythm.Anthem
{
	public class GuitarMinigame : RhythmMinigame
	{
		public static Texture2D Bolt { get; set; }
		public static SoundEffect Guitar { get; set; }

		public GuitarMinigame(Vector2 position, Player owner, IRhythmWeapon item) : base(position, owner, item, 130, Guitar)
		{
			Left = new Color(255, 46, 46);
			LeftOutline = new Color(255, 160, 160);
			Right = new Color(255, 219, 79);
			RightOutline = new Color(255, 253, 199);
		}

		public new static void LoadStatic()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Bolt = SpiritMod.instance.GetTexture("Items/Weapon/Magic/Rhythm/Anthem/Bolt");
				Guitar = SpiritMod.instance.GetSound("Sounds/130bpm-guitar");
			}
		}

		public override void Draw(SpriteBatch sB)
		{
			base.Draw(sB);

			sB.End();

			sB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

			float alpha = Utils.Clamp(0.1f + ComboScale * 0.1f, 0, 1f) * Visibility;
			float coolScale = Utils.Clamp(0.1f + ComboScale, 0, 1f) + (float)Math.Sin(Main.time * 0.05f) * 0.1f;

			float rot = 3.6f;

			DrawBolt(sB, Position + new Vector2(48, -10), rot, BeatScale * 0.4f, coolScale, BeatScale, alpha, SpriteEffects.None);
			DrawBolt(sB, Position + new Vector2(-48, -10), -rot, -BeatScale * 0.4f, coolScale, BeatScale, alpha, SpriteEffects.FlipHorizontally);

			rot = 4.2f;

			DrawBolt(sB, Position + new Vector2(32, 20), rot, BeatScale * 0.25f, coolScale * 0.6f, BeatScale, alpha, SpriteEffects.None);
			DrawBolt(sB, Position + new Vector2(-32, 20), -rot, -BeatScale * 0.25f, coolScale * 0.6f, BeatScale, alpha, SpriteEffects.FlipHorizontally);

			sB.End();

			sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
		}

		void DrawBolt(SpriteBatch sB, Vector2 pos, float mainRotation, float beatRotation, float mainScale, float beatScale, float alpha, SpriteEffects effects)
		{
			sB.Draw(Bolt, pos, null, new Color(0, 0, 1f, alpha), mainRotation + beatRotation * 1.1f, new Vector2(64, 0), (1f + beatScale * 1.1f) * mainScale * 0.5f, effects, 0);
			sB.Draw(Bolt, pos, null, new Color(0, 1f, 0, alpha), mainRotation + beatRotation * 1.05f, new Vector2(64, 0), (1f + beatScale * 1.05f) * mainScale * 0.5f, effects, 0);
			sB.Draw(Bolt, pos, null, new Color(1f, 0, 0, alpha), mainRotation + beatRotation, new Vector2(64, 0), (1f + beatScale) * mainScale * 0.5f, effects, 0);
		}
	}
}
