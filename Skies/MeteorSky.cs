using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Skies
{
	public class MeteorSky : CustomSky
	{
		private interface IMeteorController
		{
			void InitializeMeteor(ref Meteor Meteor);
			bool Update(ref Meteor Meteor);
		}

		private class ZipBehavior : IMeteorController
		{
			private Vector2 _speed;
			private int _ticks;
			private int _maxTicks;

			public void InitializeMeteor(ref Meteor Meteor)
			{
				Meteor.Position.X = (float)(Meteor.Random.NextDouble() * (Main.maxTilesX << 4));
				Meteor.Position.Y = (float)(Meteor.Random.NextDouble() * 5000.0);
				Meteor.Opacity = 0f;
				float num3 = (float)Meteor.Random.NextDouble() * .4f;
				double num2 = Meteor.Random.NextDouble() * 0.6 - 0.3;
				Meteor.Rotation = (float)num2;

				if (Meteor.Random.Next(2) == 0)
					num2 += MathHelper.Pi * 1.5;

				_speed = new Vector2((float)Math.Cos(num2) * num3, 0f);
				_ticks = 0;
				_maxTicks = Meteor.Random.Next(3600, 4200);
			}

			public bool Update(ref Meteor Meteor)
			{
				if (_ticks < 10)
					Meteor.Opacity += 0.1f;
				else if (_ticks > _maxTicks - 10)
					Meteor.Opacity -= 0.1f;

				ref Vector2 position = ref Meteor.Position;
				position += _speed;

				if (_ticks >= _maxTicks)
					return false;

				if (!Main.LocalPlayer.GetSpiritPlayer().ZoneAsteroid || Main.LocalPlayer.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent)
					_ticks += 300;
				else
					_ticks++;
				return true;
			}
		}

		private class HoverBehavior : IMeteorController
		{
			private int _ticks;
			private int _maxTicks;

			public void InitializeMeteor(ref Meteor Meteor)
			{
				Meteor.Position.X = (float)(Meteor.Random.NextDouble() * (Main.maxTilesX << 4));
				Meteor.Position.Y = (float)(Meteor.Random.NextDouble() * 5000.0);
				Meteor.Opacity = 0f;
				Meteor.Rotation = 0f;
				_ticks = 0;
				_maxTicks = Meteor.Random.Next(120, 240);
			}

			public bool Update(ref Meteor Meteor)
			{
				if (_ticks < 10)
					Meteor.Opacity += 0.1f;
				else if (_ticks > _maxTicks - 10)
					Meteor.Opacity -= 0.1f;

				if (_ticks == _maxTicks)
					return false;

				_ticks++;
				return true;
			}
		}

		private struct Meteor
		{
			public static UnifiedRandom Random = new UnifiedRandom();

			private int _frame;
			private Texture2D _texture;
			private IMeteorController _controller;

			public Texture2D GlowTexture;
			public Vector2 Position;
			public int FrameHeight;
			public int FrameWidth;
			public float Depth;
			public float Scale;
			public float Opacity;
			public bool IsActive;
			public float Rotation;

			public Texture2D Texture
			{
				get => _texture;
				set
				{
					_texture = value;
					FrameWidth = value.Width;
					FrameHeight = value.Height;
				}
			}

			public IMeteorController Controller
			{
				get => _controller;
				set
				{
					_controller = value;
					value.InitializeMeteor(ref this);
				}
			}

			public Meteor(Texture2D texture, float depth = 1f, float scale = 1f)
			{
				_frame = 0;
				Position = Vector2.Zero;
				_texture = texture;
				Depth = depth;
				Scale = scale;
				FrameWidth = texture.Width;
				FrameHeight = texture.Height;
				GlowTexture = texture;
				Opacity = 0f;
				Rotation = 0f;
				IsActive = false;
				_controller = null;
			}

			public Rectangle GetSourceRectangle() => new Rectangle(0, _frame * FrameHeight, FrameWidth, FrameHeight);
			public bool Update() => Controller.Update(ref this);

			public void AssignNewBehavior()
			{
				switch (Random.Next(2))
				{
					case 0:
						Controller = new ZipBehavior();
						break;
					case 1:
						Controller = new HoverBehavior();
						break;
				}
			}
		}

		private Meteor[] _Meteors;
		private int _maxMeteors;
		private bool _active;
		private bool _leaving;
		private int _activeMeteors;

		public override void Update(GameTime gameTime)
		{
			if (!Main.gamePaused && Main.hasFocus)
			{
				int num = _activeMeteors;
				for (int i = 0; i < _Meteors.Length; i++)
				{
					Meteor Meteor = _Meteors[i];
					if (Meteor.IsActive)
					{
						if (!Meteor.Update())
						{
							if (!_leaving)
								Meteor.AssignNewBehavior();
							else
							{
								Meteor.IsActive = false;
								num--;
							}
						}
					}
					_Meteors[i] = Meteor;
				}
				if (!_leaving && num != _maxMeteors)
				{
					_Meteors[num].IsActive = true;
					_Meteors[num++].AssignNewBehavior();
				}
				_active = (!_leaving || num != 0) && !Main.LocalPlayer.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent; //only active if conditions are right & there's no sjinx
				_activeMeteors = num;
			}
		}

		public override Color OnTileColor(Color inColor)
		{
			float amt = Opacity * .6f;
			return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.screenPosition.Y <= 10000f)
			{
				int num3 = -1;
				int num2 = 0;

				for (int j = 0; j < _Meteors.Length; j++)
				{
					float depth = _Meteors[j].Depth;

					if (num3 == -1 && depth < maxDepth)
						num3 = j;

					if (depth <= minDepth)
						break;
					num2 = j;
				}

				if (num3 != -1)
				{
					Vector2 value4 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
					var rectangle = new Rectangle(-1000, -1000, 4000, 4000);
					for (int i = num3; i < num2; i++)
					{
						var value3 = new Vector2(1f / _Meteors[i].Depth, 0.9f / _Meteors[i].Depth);
						Vector2 drawPos = _Meteors[i].Position;
						drawPos = (drawPos - value4) * value3 + value4 - Main.screenPosition;
						if (_Meteors[i].IsActive && rectangle.Contains((int)drawPos.X, (int)drawPos.Y) && _Meteors[i].GlowTexture != null)
							spriteBatch.Draw(_Meteors[i].GlowTexture, drawPos, _Meteors[i].GetSourceRectangle(), new Color(130, 130, 130, 60) * _Meteors[i].Opacity, _Meteors[i].Rotation, Vector2.Zero, value3.X * 5f * _Meteors[i].Scale, SpriteEffects.None, 0f);
					}
				}
			}
		}

		private void GenerateMeteors()
		{
			float num3 = Main.maxTilesX / 4200f;

			_maxMeteors = (int)(256f * num3);
			_Meteors = new Meteor[_maxMeteors];

			int num2 = _maxMeteors >> 4;

			for (int j = 0; j < num2; j++)
			{
				var paths = new WeightedRandom<string>();

				paths.Add("MeteorBG3", 0.25f);
				paths.Add("MeteorBG2", 0.5f);
				paths.Add("MeteorBG5", 0.1f);
				paths.Add("MeteorBG4", 0.05f);
				paths.Add("MeteorBG6", 0.025f);
				paths.Add("MeteorBG", 0.01f);

				_Meteors[j] = new Meteor(ModContent.Request<Texture2D>("Textures/" + paths, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (float)Main.rand.NextDouble() * 4f + 6.6f);
			}

			for (int i = num2; i < _Meteors.Length; i++)
			{
				var paths = new WeightedRandom<string>();

				paths.Add("MeteorBG1", 0.33f);
				paths.Add("MeteorBG", 0.6f);
				paths.Add("MeteorBG4", 0.1f);
				paths.Add("MeteorBG3", 0.28f);

				_Meteors[i] = new Meteor(ModContent.Request<Texture2D>("Textures/" + paths, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (float)Main.rand.NextDouble() * 5f + 1.6f, 0.5f);
			}
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_activeMeteors = 0;
			GenerateMeteors();
			Array.Sort(_Meteors, (Meteor Meteor1, Meteor Meteor2) => Meteor2.Depth.CompareTo(Meteor1.Depth));
			_active = true;
			_leaving = false;
		}

		public override void Deactivate(params object[] args) => _leaving = true;
		public override bool IsActive() => _active && !Main.gameMenu;
		public override void Reset() => _active = false;
	}
}