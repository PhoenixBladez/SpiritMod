using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Graphics.Effects;
using Terraria.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Skies
{
	public class MeteorSky : CustomSky
	{
		private abstract class IMeteorController
		{
			public abstract void InitializeMeteor(ref Meteor Meteor);

			public abstract bool Update(ref Meteor Meteor);
		}

		private class ZipBehavior : IMeteorController
		{
			private Vector2 _speed;

			private int _ticks;

			private int _maxTicks;

			public override void InitializeMeteor(ref Meteor Meteor)
			{
				Meteor.Position.X = (float)(Meteor.Random.NextDouble() * (double)(Main.maxTilesX << 4));
				Meteor.Position.Y = (float)(Meteor.Random.NextDouble() * 5000.0);
				Meteor.Opacity = 0f;
				float num3 = (float)Meteor.Random.NextDouble() * .4f;
				double num2 = Meteor.Random.NextDouble() * 0.60000002384185791 - 0.30000001192092896;
				Meteor.Rotation = (float)num2;
				if (Meteor.Random.Next(2) == 0)
				{
					num2 += 3.1415927410125732 * 1.5;
				}
				this._speed = new Vector2((float)Math.Cos(num2) * num3,  0f);
				this._ticks = 0;
                Player player = Main.LocalPlayer;
                {
                    this._maxTicks = Meteor.Random.Next(3600, 4200);
                }
			}

			public override bool Update(ref Meteor Meteor)
			{
				if (this._ticks < 10)
				{
					Meteor.Opacity += 0.1f;
				}
				else if (this._ticks > this._maxTicks - 10)
				{
					Meteor.Opacity -= 0.1f;
				}
				ref Vector2 position = ref Meteor.Position;
				position += this._speed;
				if (this._ticks >= this._maxTicks)
				{
					return false;
				}
                if (!Main.LocalPlayer.GetSpiritPlayer().ZoneAsteroid)
                {
                    this._ticks+= 300;
                }
                else
                {
                    this._ticks++;
                }
				return true;
			}
		}

		private class HoverBehavior : IMeteorController
		{
			private int _ticks;

			private int _maxTicks;

			public override void InitializeMeteor(ref Meteor Meteor)
			{
				Meteor.Position.X = (float)(Meteor.Random.NextDouble() * (double)(Main.maxTilesX << 4));
				Meteor.Position.Y = (float)(Meteor.Random.NextDouble() * 5000.0);
				Meteor.Opacity = 0f;
				Meteor.Rotation = 0f;
				this._ticks = 0;
				this._maxTicks = Meteor.Random.Next(120, 240);
			}

			public override bool Update(ref Meteor Meteor)
			{
				if (this._ticks < 10)
				{
					Meteor.Opacity += 0.1f;
				}
				else if (this._ticks > this._maxTicks - 10)
				{
					Meteor.Opacity -= 0.1f;
				}
				if (this._ticks == this._maxTicks)
				{
					return false;
				}
				this._ticks++;
				return true;
			}
		}

		private struct Meteor
		{
			private const int MAX_FRAMES = 1;

			private const int FRAME_RATE = 0;

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
				get
				{
					return this._texture;
				}
				set
				{
					this._texture = value;
					this.FrameWidth = value.Width;
					this.FrameHeight = value.Height;
				}
			}

			public IMeteorController Controller
			{
				get
				{
					return this._controller;
				}
				set
				{
					this._controller = value;
					value.InitializeMeteor(ref this);
				}
			}

			public Meteor(Texture2D texture, float depth = 1f)
			{
				this._frame = 0;
				this.Position = Vector2.Zero;
				this._texture = texture;
				this.Depth = depth;
				this.Scale = 1f;
				this.FrameWidth = texture.Width;
				this.FrameHeight = texture.Height;
				this.GlowTexture = null;
				this.Opacity = 0f;
				this.Rotation = 0f;
				this.IsActive = false;
				this._controller = null;
			}

			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(0, this._frame * this.FrameHeight, this.FrameWidth, this.FrameHeight);
			}

			public bool Update()
			{
				return this.Controller.Update(ref this);
			}

			public void AssignNewBehavior()
			{
				switch (Meteor.Random.Next(2))
				{
				case 0:
					this.Controller = new ZipBehavior();
					break;
				case 1:
					this.Controller = new HoverBehavior();
					break;
				}
			}
		}

		private Meteor[] _Meteors;

		private UnifiedRandom _random = new UnifiedRandom();

		private int _maxMeteors;

		private bool _active;

		private bool _leaving;

		private int _activeMeteors;

		public override void Update(GameTime gameTime)
		{
			if (!Main.gamePaused && Main.hasFocus)
			{
				int num = this._activeMeteors;
				for (int i = 0; i < this._Meteors.Length; i++)
				{
					Meteor Meteor = this._Meteors[i];
					if (Meteor.IsActive)
					{
						if (!Meteor.Update())
						{
							if (!this._leaving)
							{
								Meteor.AssignNewBehavior();
							}
							else
							{
								Meteor.IsActive = false;
								num--;
							}
						}
					}
					this._Meteors[i] = Meteor;
				}
				if (!this._leaving && num != this._maxMeteors)
				{
					this._Meteors[num].IsActive = true;
					this._Meteors[num++].AssignNewBehavior();
				}
				this._active = (!this._leaving || num != 0);
				this._activeMeteors = num;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
            if (!(Main.screenPosition.Y > 10000f))
			{
				int num3 = -1;
				int num2 = 0;
				for (int j = 0; j < this._Meteors.Length; j++)
				{
					float depth = this._Meteors[j].Depth;
					if (num3 == -1 && depth < maxDepth)
					{
						num3 = j;
					}
					if (depth <= minDepth)
					{
						break;
					}
					num2 = j;
				}
				if (num3 != -1)
				{
					Color value5 = new Color(Main.bgColor.ToVector4() * 0.9f + new Vector4(0.1f));
					Vector2 value4 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
					Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
					for (int i = num3; i < num2; i++)
					{
						Vector2 value3 = new Vector2(1f / this._Meteors[i].Depth, 0.9f / this._Meteors[i].Depth);
						Vector2 vector2 = this._Meteors[i].Position;
						vector2 = (vector2 - value4) * value3 + value4 - Main.screenPosition;
						if (this._Meteors[i].IsActive && rectangle.Contains((int)vector2.X, (int)vector2.Y))
						{
                            Mod mod = SpiritMod.instance;
                            if (this._Meteors[i].GlowTexture != null)
                            {
                                spriteBatch.Draw(this._Meteors[i].GlowTexture, vector2, this._Meteors[i].GetSourceRectangle(), new Color(130, 130, 130, 60) * this._Meteors[i].Opacity, this._Meteors[i].Rotation, Vector2.Zero, value3.X * 5f * this._Meteors[i].Scale, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateMeteors()
        {
            Mod mod = SpiritMod.instance;
            float num3 = (float)Main.maxTilesX / 4200f;
            this._maxMeteors = (int)(256f * num3);
            this._Meteors = new Meteor[this._maxMeteors];
            int num2 = this._maxMeteors >> 4;
            for (int j = 0; j < num2; j++)
            {
                float num4 = (float)j / (float)num2;
                if (Main.rand.Next(2) == 0)
                {
                    this._Meteors[j] = new Meteor(mod.GetTexture("Textures/MeteorBG2"), (float)Main.rand.NextDouble() * 4f + 6.6f);
                    this._Meteors[j].GlowTexture = mod.GetTexture("Textures/MeteorBGGlow2");
                }
                else
                {
                    this._Meteors[j] = new Meteor(mod.GetTexture("Textures/MeteorBG3"), (float)Main.rand.NextDouble() * 4f + 6.6f);
                    this._Meteors[j].GlowTexture = mod.GetTexture("Textures/MeteorBGGlow3");
                }
            }
            for (int i = num2; i < this._Meteors.Length; i++)
            {
                float num5 = (float)(i - num2) / (float)(this._Meteors.Length - num2);
                if (Main.rand.Next(2) == 0)
                {
                    this._Meteors[i] = new Meteor(mod.GetTexture("Textures/MeteorBG"), (float)Main.rand.NextDouble() * 5f + 1.6f);
                    this._Meteors[i].Scale = 0.5f;
                    this._Meteors[i].GlowTexture = mod.GetTexture("Textures/MeteorBGGlow");
                }
                else
                {
                    this._Meteors[i] = new Meteor(mod.GetTexture("Textures/MeteorBG1"), (float)Main.rand.NextDouble() * 5f + 1.6f);
                    this._Meteors[i].Scale = 0.5f;
                    this._Meteors[i].GlowTexture = mod.GetTexture("Textures/MeteorBGGlow1");
                }
            }
        }

        public override void Activate(Vector2 position, params object[] args)
		{
			this._activeMeteors = 0;
			this.GenerateMeteors();
			Array.Sort(this._Meteors, (Meteor Meteor1, Meteor Meteor2) => Meteor2.Depth.CompareTo(Meteor1.Depth));
			this._active = true;
			this._leaving = false;
		}

		public override void Deactivate(params object[] args)
		{
			this._leaving = true;
		}

		public override bool IsActive()
		{
            return this._active && !Main.gameMenu;
		}

		public override void Reset()
		{
			this._active = false;
		}
	}
}
